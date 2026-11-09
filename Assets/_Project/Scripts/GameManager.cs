using System;
using System.Collections.Generic;
using Rodak.Utils.Singleton;
using UnityEngine;

namespace Rodak.Hexdle
{
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        private int penalty;
        private string word;

        private readonly List<string> wrongGuesses = new();

        public event Action OnGameStarted;
        public event Action OnPenaltyGained;

        public event Action OnGuessedWrong;
        public event Action OnGuessedRight;

        public string Word => word;
        public string Definition => Words.WordDefinitions[word];

        public int Penalty => penalty;

        protected override void Awake()
        {
            base.Awake();

            word = Words.WordOfTheDay;

            int seed = word.GetHashCode();
            UnityEngine.Random.InitState(seed);

            Debug.Log($"{Word} -> {Definition}");
        }

        private void Start()
        {
            StartGame();
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
                case PenaltyType.HitLetter:
                    return 100;
                case PenaltyType.GuessWrong:
                    return 50;
            }
            return 0;
        }

        public void GuessWord(string word)
        {
            if (word == Word)
            {
                OnGuessedRight?.Invoke();
                // TODO: Add UI
            }
            else
            {
                GainPenalty(PenaltyType.GuessWrong);
                wrongGuesses.Add(word);
                OnGuessedWrong?.Invoke();
            }
        }

        public void GainPenalty(PenaltyType penaltyType)
        {
            penalty += GetPenaltyAmount(penaltyType);
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
