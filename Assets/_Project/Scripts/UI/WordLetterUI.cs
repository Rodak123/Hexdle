using TMPro;
using UnityEngine;

namespace Rodak.Hexdle
{
    public class WordLetterUI : MonoBehaviour
    {
        public bool IsActive;
        public int Index;

        [Header("UI")]
        [SerializeField] private TMP_Text text;

        [Space]
        [SerializeField] private GameObject typeIndicator;
        [SerializeField] private float typeIndicatorFlashInterval = 0.35f;

        [Space]
        [SerializeField] private GameObject inactiveIndicator;

        private float typeIndicatorFlashTimer;

        private bool isSelected;

        private GameStateManager.LetterState State
        {
            get
            {
                if (GameStateManager.State.LetterStates[Index] == null)
                {
                    GameStateManager.State.LetterStates[Index] = new();
                }
                return GameStateManager.State.LetterStates[Index];
            }
        }

        public char Letter => HasLetter ? State.Letter[0] : ' ';
        public bool HasLetter => State.Letter.Length != 0;


        private void Update()
        {
            text.text = $"{Letter}";

            inactiveIndicator.SetActive(!IsActive);

            if (isSelected && !GameStateManager.State.GuessedRight)
            {
                typeIndicatorFlashTimer += Time.deltaTime;
                if (typeIndicatorFlashTimer >= typeIndicatorFlashInterval)
                {
                    typeIndicatorFlashTimer -= typeIndicatorFlashInterval;
                    typeIndicator.SetActive(!typeIndicator.activeSelf);
                }
            }
            else
            {
                typeIndicator.SetActive(false);
            }
        }

        public bool Type(char letter)
        {
            if (HasLetter || GameManager.Instance.GetLetterStatus(letter) == GameManager.LetterStatus.Missing) return false;
            State.Letter = $"{letter}";
            GameStateManager.Save();
            return true;
        }

        public void Erase()
        {
            State.Letter = string.Empty;
            GameStateManager.Save();
        }

        public void Select()
        {
            isSelected = true;
        }

        public void Unselect()
        {
            isSelected = false;
        }
    }
}
