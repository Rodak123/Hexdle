using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rodak.Hexdle
{
    public class WordUI : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private WordLetterUI wordLetterUIPrefab;
        [SerializeField] private Transform wordLettersContainer;

        private readonly List<WordLetterUI> wordLetters = new();
        private int activeLetterCount;

        private int selectedWordLetterIndex;
        private WordLetterUI SelectedWordLetter => wordLetters[selectedWordLetterIndex];

        public bool ReadyToGuess => selectedWordLetterIndex == activeLetterCount - 1 && SelectedWordLetter.HasLetter && Words.WordDefinitions.ContainsKey(GetWord());

        public event Action<string> OnWordTyped;
        public event Action OnWordErased;

        private void Awake()
        {
            GameManager.Instance.OnGameStarted += GameManager_OnGameStarted;
            InputManager.Instance.KeyboardInput.OnTyped += KeyboardInput_OnTyped;
            InputManager.Instance.KeyboardInput.OnEnter += KeyboardInput_OnEnter;
            InputManager.Instance.KeyboardInput.OnBackspace += KeyboardInput_OnBackspace;
        }

        private void KeyboardInput_OnTyped(char letter)
        {
            if (!SelectedWordLetter.Type(letter)) return;

            if (selectedWordLetterIndex < activeLetterCount - 1)
            {
                SelectLetterAt(selectedWordLetterIndex + 1);
            }

            if (selectedWordLetterIndex > Words.MIN_WORD_LENGTH - 1)
            {
                OnWordTyped?.Invoke(GetWord());
            }
        }

        private void KeyboardInput_OnEnter()
        {
            TryGuess();
        }

        private void KeyboardInput_OnBackspace()
        {
            if (GameStateManager.State.GuessedRight) return;
            if (selectedWordLetterIndex == activeLetterCount - 1 && SelectedWordLetter.HasLetter)
            {
                SelectedWordLetter.Erase();
            }
            else if (selectedWordLetterIndex > 0)
            {
                SelectLetterAt(selectedWordLetterIndex - 1);
                SelectedWordLetter.Erase();
            }

            if (selectedWordLetterIndex > Words.MIN_WORD_LENGTH - 1)
            {
                OnWordTyped?.Invoke(GetWord());
            }
            else
            {
                OnWordErased?.Invoke();
            }
        }

        private string GetWord()
        {
            string word = string.Empty;
            foreach (WordLetterUI wordLetterUI in wordLetters)
            {
                if (wordLetterUI.IsActive)
                {
                    word += wordLetterUI.HasLetter ? wordLetterUI.Letter : "";
                }
            }
            return word.Trim();
        }


        private void SelectLetterAt(int index)
        {
            SelectedWordLetter.Unselect();
            selectedWordLetterIndex = index;
            SelectedWordLetter.Select();
        }

        private void GameManager_OnGameStarted()
        {
            foreach (Transform child in wordLettersContainer)
            {
                Destroy(child.gameObject);
            }

            activeLetterCount = GameStateManager.State.Word.Length;
            for (int i = 0; i < Words.MAX_WORD_LENGTH; i++)
            {
                bool isActive = true;  //i < activeLetterCount;

                WordLetterUI wordLetterUI = Instantiate(wordLetterUIPrefab, wordLettersContainer);
                wordLetters.Add(wordLetterUI);

                wordLetterUI.Index = i;
                wordLetterUI.IsActive = isActive;
            }

            selectedWordLetterIndex = activeLetterCount - 1;
            for (int i = 0; i < wordLetters.Count; i++)
            {
                if (wordLetters[i].HasLetter)
                {
                    continue;
                }
                selectedWordLetterIndex = i;
                break;
            }
            SelectedWordLetter.Select();

            if (selectedWordLetterIndex > Words.MIN_WORD_LENGTH - 1)
            {
                OnWordTyped?.Invoke(GetWord());
            }
        }

        public void TryGuess()
        {
            if (GameStateManager.State.GuessedRight) return;
            if (!ReadyToGuess) return;
            GameManager.Instance.GuessWord(GetWord());
        }
    }
}
