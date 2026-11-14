using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;

namespace Rodak.Hexdle
{
    public class KeyboardKeyUI : MonoBehaviour
    {
        [SerializeField] private Key key;

        [Header("UI")]
        [SerializeField] private TMP_Text keyText;
        [SerializeField] private Button keyButton;
        [SerializeField] private GameObject enterIcon;
        [SerializeField] private GameObject backspaceIcon;

        public event Action<char> OnTyped;
        public event Action OnBackspace;
        public event Action OnEnter;

        public Key Key => key;
        public char KeyChar => key.ToString().ToLower()[0];

        private void Awake()
        {
            keyText.gameObject.SetActive(false);
            enterIcon.SetActive(false);
            backspaceIcon.SetActive(false);

            if (key == Key.Backspace)
            {
                backspaceIcon.SetActive(true);
            }
            else if (key == Key.Enter)
            {
                enterIcon.SetActive(true);
            }
            else
            {
                keyText.gameObject.SetActive(true);
                keyText.text = key.ToString();
            }

            keyButton.onClick.AddListener(Button_OnClick);
        }

        private void Button_OnClick()
        {
            if (key == Key.Backspace)
            {
                OnBackspace?.Invoke();
            }
            else if (key == Key.Enter)
            {
                OnEnter?.Invoke();
            }
            else
            {
                OnTyped?.Invoke(KeyChar);
            }
        }
    }
}
