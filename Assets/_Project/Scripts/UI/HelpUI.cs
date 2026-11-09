using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Rodak.Hexdle
{
    public class HelpUI : MonoBehaviour
    {
        [SerializeField] private WordUI wordUI;

        [Header("UI")]
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text bodyText;

        [Space]
        [SerializeField] private Button submitButton;
        [SerializeField] private ColoredText submitTextColor;
        [SerializeField] private ColoredImage submitBgImageColor;

        private string defaultTitle;
        private string defaultBody;

        private void Awake()
        {
            defaultTitle = titleText.text;
            defaultBody = bodyText.text;

            wordUI.OnWordTyped += WordUI_OnWordTyped;
            wordUI.OnWordErased += WordUI_OnWordErased;
            submitButton.onClick.AddListener(Button_OnClick);
        }

        private void Button_OnClick()
        {
            wordUI.TryGuess();
            WordUI_OnWordTyped(titleText.text);
        }

        private void WordUI_OnWordTyped(string word)
        {
            titleText.text = word;
            if (Words.WordDefinitions.TryGetValue(word, out string definition))
            {
                bool alreadyGuessed = GameStateManager.State.WrongGuesses.Contains(word);

                if (alreadyGuessed)
                {
                    bodyText.text = "Word already guessed.";
                    submitTextColor.SetIsInverted(false);
                    submitBgImageColor.SetIsInverted(true);
                    submitButton.interactable = false;
                }
                else
                {
                    submitButton.interactable = true;
                    bodyText.text = definition;
                    submitTextColor.SetIsInverted(true);
                    submitBgImageColor.SetIsInverted(false);
                }
            }
            else
            {
                bodyText.text = "Not defined";
            }
        }

        private void WordUI_OnWordErased()
        {
            titleText.text = defaultTitle;
            bodyText.text = defaultBody;
            submitTextColor.SetIsInverted(false);
            submitBgImageColor.SetIsInverted(true);
            submitButton.interactable = false;
        }
    }
}
