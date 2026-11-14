using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

namespace Rodak.Hexdle
{
    public class KeyboardKeyStatusUI : MonoBehaviour
    {
        [SerializeField] private KeyboardKeyUI keyboardKey;

        [Header("UI")]
        [SerializeField] private GameObject strike;
        [SerializeField] private ColoredText letterTextColor;
        [SerializeField] private ColoredImage bgColor;

        private void Start()
        {
            strike.SetActive(false);
        }

        private void Update()
        {
            switch (GameManager.Instance.GetLetterStatus(keyboardKey.KeyChar))
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
