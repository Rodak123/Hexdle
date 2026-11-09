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

            Map.OnLetterRevealed += Map_OnLetterRevealed;
        }

        private void Map_OnLetterRevealed(char letter)
        {
            if (letter != Letter) return;
            if (GameManager.Instance.Word.Contains(letter))
            {
                letterTextColor.SetIsInverted(!letterTextColor.IsInverted);
                bgColor.SetIsInverted(!bgColor.IsInverted);
            }
            else
            {
                strike.SetActive(true);
            }
        }
    }
}
