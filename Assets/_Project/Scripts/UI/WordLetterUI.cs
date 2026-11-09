using TMPro;
using UnityEngine;

namespace Rodak.Hexdle
{
    public class WordLetterUI : MonoBehaviour
    {
        public bool IsActive;

        [Header("UI")]
        [SerializeField] private TMP_Text text;

        [Space]
        [SerializeField] private GameObject typeIndicator;
        [SerializeField] private float typeIndicatorFlashInterval = 0.35f;

        [Space]
        [SerializeField] private GameObject inactiveIndicator;

        private char letter = ' ';
        private float typeIndicatorFlashTimer;

        private bool isSelected;

        public char Letter => letter;
        public bool HasLetter => letter != ' ';

        private void Awake()
        {
            text.text = string.Empty;
        }

        private void Update()
        {
            inactiveIndicator.SetActive(!IsActive);

            if (isSelected)
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
            if (HasLetter) return false;
            this.letter = letter;
            text.text = $"{letter}";
            return true;
        }

        public void Erase()
        {
            letter = ' ';
            text.text = $"{letter}";
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
