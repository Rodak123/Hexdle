using TMPro;
using UnityEngine;

namespace Rodak.Hexdle
{
    public class LetterUI : MonoBehaviour
    {
        public char Letter = 'a';

        [Header("UI")]
        [SerializeField] private TMP_Text letterText;
        [SerializeField] private GameObject strike;
        [SerializeField] private ColoredText letterTextColor;
        [SerializeField] private ColoredImage bgColor;

        private void Start()
        {
            strike.SetActive(false);

            letterText.text = $"{Letter}";
        }

        private void Update()
        {
            switch (GameManager.Instance.GetLetterStatus(Letter))
            {
                case GameManager.LetterStatus.Hidden:
                    return;
                case GameManager.LetterStatus.Missing:
                    strike.SetActive(true);
                    break;
                case GameManager.LetterStatus.Included:
                    letterTextColor.SetIsInverted(true);
                    bgColor.SetIsInverted(false);
                    break;
            }
        }
    }
}
