using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Rodak.Hexdle
{
    public class KeyboardInput : MonoBehaviour
    {
        [Header("Backspace")]
        [SerializeField] private float backspaceInterval = 0.2f;

        private float backspaceTimer;

        public event Action<char> OnTyped;
        public event Action OnBackspace;
        public event Action OnEnter;

        private void OnEnable()
        {
            Keyboard.current.onTextInput += Keyboard_OnTextInput;
        }

        private void OnDisable()
        {
            Keyboard.current.onTextInput -= Keyboard_OnTextInput;
        }

        private void Update()
        {
            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                Enter();
            }

            if (Keyboard.current.backspaceKey.isPressed)
            {
                backspaceTimer += Time.deltaTime;
            }

            if (Keyboard.current.backspaceKey.wasReleasedThisFrame)
            {
                backspaceTimer = 0;
            }

            if (Keyboard.current.backspaceKey.wasPressedThisFrame)
            {
                Backspace();
            }
            else if (backspaceTimer >= backspaceInterval)
            {
                backspaceTimer -= backspaceInterval;
                Backspace();
            }
        }

        private void Keyboard_OnTextInput(char letter)
        {
            Type(letter);
        }

        public void Type(char letter)
        {
            letter = $"{letter}".ToLower()[0];
            if (!Words.Letters.Contains(letter)) return;
            OnTyped?.Invoke(letter);
        }

        public void Enter()
        {
            OnEnter?.Invoke();
        }

        public void Backspace()
        {
            OnBackspace?.Invoke();
        }
    }
}
