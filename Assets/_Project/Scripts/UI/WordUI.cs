using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Rodak.Hexdle
{
    public class WordUI : MonoBehaviour
    {
        [SerializeField] private WordLetterUI wordLetterUIPrefab;
        [SerializeField] private Transform wordLettersContainer;

        [Space]
        [SerializeField] private float eraseInterval = 0.1f;

        private float eraseTimer;

        private readonly List<WordLetterUI> wordLetters = new();

        private int selectedWordLetterIndex;
        private WordLetterUI SelectedWordLetter => wordLetters[selectedWordLetterIndex];

        public bool ReadyToGuess => selectedWordLetterIndex == wordLetters.Count - 1 && SelectedWordLetter.HasLetter && Words.WordDefinitions.ContainsKey(GetWord());

        public event Action<string> OnWordTyped;
        public event Action OnWordErased;

        private void Awake()
        {
            GameManager.Instance.OnGameStarted += GameManager_OnGameStarted;
        }

        private void OnEnable()
        {
            Keyboard.current.onTextInput += Keyboard_OnTextInput;
        }

        private void OnDisable()
        {
            Keyboard.current.onTextInput -= Keyboard_OnTextInput;
        }

        private void Update()
        {
            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                TryGuess();
            }

            if (Keyboard.current.backspaceKey.isPressed)
            {
                eraseTimer += Time.deltaTime;
            }

            if (Keyboard.current.backspaceKey.wasReleasedThisFrame)
            {
                eraseTimer = 0;
            }

            if (Keyboard.current.backspaceKey.wasPressedThisFrame)
            {
                Erase();
            }
            else if (eraseTimer >= eraseInterval)
            {
                eraseTimer -= eraseInterval;
                Erase();
            }
        }

        private string GetWord()
        {
            string word = string.Empty;
            foreach (WordLetterUI wordLetterUI in wordLetters)
            {
                word += wordLetterUI.Letter;
            }
            return word;
        }

        private void Erase()
        {
            if (selectedWordLetterIndex == wordLetters.Count - 1 && SelectedWordLetter.HasLetter)
            {
                SelectedWordLetter.Erase();
                OnWordErased?.Invoke();
            }
            else if (selectedWordLetterIndex > 0)
            {
                SelectLetterAt(selectedWordLetterIndex - 1);
                SelectedWordLetter.Erase();
            }
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

            for (int i = 0; i < GameManager.Instance.Word.Length; i++)
            {
                WordLetterUI wordLetterUI = Instantiate(wordLetterUIPrefab, wordLettersContainer);
                wordLetters.Add(wordLetterUI);
            }

            selectedWordLetterIndex = 0;
            SelectedWordLetter.Select();
        }

        private void Keyboard_OnTextInput(char letter)
        {
            if (!Words.Letters.Contains(letter)) return;

            if (!SelectedWordLetter.Type(letter)) return;

            if (selectedWordLetterIndex < wordLetters.Count - 1)
            {
                SelectLetterAt(selectedWordLetterIndex + 1);
            }
            else
            {
                string word = string.Empty;
                foreach (WordLetterUI wordLetterUI in wordLetters)
                {
                    word += wordLetterUI.Letter;
                }
                OnWordTyped?.Invoke(word);
            }
        }

        public void TryGuess()
        {
            if (!ReadyToGuess) return;
            GameManager.Instance.GuessWord(GetWord());
        }
    }
}
