using System;
using System.Collections.Generic;
using UnityEngine;
namespace Rodak.Hexdle
{
    public static class GameStateManager
    {
        private const string SAVE_KEY = "HexdleSave";

        private static GameState state = new();

        public static GameState State => state;

        public static void Save()
        {
            string json = JsonUtility.ToJson(state);
            PlayerPrefs.SetString(SAVE_KEY, json);
        }

        public static void Load()
        {
            string json = PlayerPrefs.GetString(SAVE_KEY, string.Empty);
            try
            {
                state = JsonUtility.FromJson<GameState>(json);
                if (state.Word != Words.WordOfTheDay)
                {
                    // clear old save
                    Reset();
                }
            }
            catch
            {
                Reset();
            }


            int seed = state.Word.GetHashCode();
            UnityEngine.Random.InitState(seed);
        }

        public static void Reset()
        {
            state = new();
        }

        [Serializable]
        public class GameState
        {
            public string Word;

            public int Penalty;
            public List<string> WrongGuesses;
            public bool GuessedRight;

            public CellState[] CellStates;
            public LetterState[] LetterStates;

            public int ColorIndex;

            public bool SeenInfo;

            public GameState()
            {
                Word = Words.WordOfTheDay;
                GuessedRight = false;
                Penalty = 0;
                WrongGuesses = new();
                CellStates = new CellState[0];
                LetterStates = new LetterState[Words.MAX_WORD_LENGTH];
                ColorIndex = 0;
                SeenInfo = false;
            }
        }

        [Serializable]
        public class CellState
        {
            public bool IsRevealed;
            public bool IsFlagged;
        }

        [Serializable]
        public class LetterState
        {
            public string Letter;

            public LetterState()
            {
                Letter = string.Empty;
            }
        }
    }
}
