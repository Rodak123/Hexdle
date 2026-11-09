using System;
using Rodak.Utils.Singleton;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Rodak.Hexdle
{
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        public event Action OnGameStarted;
        public event Action OnPenaltyGained;

        public event Action OnGuessedWrong;
        public event Action OnGuessedRight;

        protected override void Awake()
        {
            base.Awake();

            GameStateManager.Load();

            string word = GameStateManager.State.Word;
            Debug.Log($"{word} -> {Words.WordDefinitions[word]}");
        }

        private void Start()
        {
            StartGame();
        }

        private void Update()
        {
            if (Keyboard.current.f4Key.wasPressedThisFrame)
            {
                GameStateManager.Reset();
                GameStateManager.Save();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        private void StartGame()
        {
            OnGameStarted?.Invoke();
        }

        private int GetPenaltyAmount(PenaltyType penaltyType)
        {
            switch (penaltyType)
            {
                case PenaltyType.RevealCells:
                    return 1;
                case PenaltyType.GuessWrong:
                    return 30;
                case PenaltyType.HitLetter:
                    return 80;
            }
            return 0;
        }

        public void GuessWord(string word)
        {
            if (word == GameStateManager.State.Word)
            {
                GameStateManager.State.GuessedRight = true;
                GameStateManager.Save();
                OnGuessedRight?.Invoke();
            }
            else
            {
                GainPenalty(PenaltyType.GuessWrong);
                GameStateManager.State.WrongGuesses.Add(word);
                GameStateManager.Save();
                OnGuessedWrong?.Invoke();
            }
        }

        public void GainPenalty(PenaltyType penaltyType)
        {
            GameStateManager.State.Penalty += GetPenaltyAmount(penaltyType);
            GameStateManager.Save();
            OnPenaltyGained?.Invoke();
        }

        public enum PenaltyType
        {
            RevealCells,
            HitLetter,
            GuessWrong
        }
    }
}
