using UnityEngine;

namespace Rodak.Hexdle
{
    public class KeyboardUI : MonoBehaviour
    {
        [SerializeField] private KeyboardInput keyboardInput;

        private void Start()
        {
            foreach (KeyboardKeyUI keyboardKeyUI in GetComponentsInChildren<KeyboardKeyUI>())
            {
                keyboardKeyUI.OnTyped += KeyboardKeyUI_OnTyped;
                keyboardKeyUI.OnEnter += KeyboardKeyUI_OnEnter;
                keyboardKeyUI.OnBackspace += KeyboardKeyUI_OnBackspace;
            }
        }

        private void KeyboardKeyUI_OnTyped(char letter)
        {
            keyboardInput.Type(letter);
        }

        private void KeyboardKeyUI_OnEnter()
        {
            keyboardInput.Enter();
        }

        private void KeyboardKeyUI_OnBackspace()
        {
            keyboardInput.Backspace();
        }
    }
}
