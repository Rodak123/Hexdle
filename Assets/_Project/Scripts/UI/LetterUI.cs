using TMPro;
using UnityEngine;

namespace Rodak.Hexdle
{
    public class LetterUI : MonoBehaviour
    {
        public char Letter = 'a';
        public Map Map;

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
            if (!Map.RevealedLetters.Contains(Letter))
            {
                return;
            }

            if (GameStateManager.State.Word.Contains(Letter))
            {
                letterTextColor.SetIsInverted(true);
                bgColor.SetIsInverted(false);
            }
            else
            {
                strike.SetActive(true);
            }
        }
    }
}
