using Rodak.Utils.Singleton;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Rodak.Hexdle
{
    [DefaultExecutionOrder(-100), RequireComponent(typeof(KeyboardInput))]
    public class InputManager : SingletonMonoBehaviour<InputManager>
    {
        private InputSystem_Actions actions;

        private KeyboardInput keyboardInput;

        public KeyboardInput KeyboardInput => keyboardInput;

        protected override void Awake()
        {
            actions = new();

            keyboardInput = GetComponent<KeyboardInput>();
        }

        private void OnEnable()
        {
            actions.Enable();

            actions.Game.Enable();
        }

        private void OnDisable()
        {
            actions.Disable();

            actions.Game.Disable();
        }

        public bool IsPointerOverUI()
        {
            if (EventSystem.current == null)
                return false;

            if (EventSystem.current.IsPointerOverGameObject())
                return true;

            if (Touchscreen.current != null)
            {
                foreach (var touch in Touchscreen.current.touches)
                {
                    if (touch.press.isPressed && EventSystem.current.IsPointerOverGameObject(touch.touchId.ReadValue()))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        // Game

        public InputAction MapInteractionToggle => actions.Game.MapInteractionModeToggle;
        public InputAction MapInteraction => actions.Game.MapInteraction;

        public Vector2 ReadMapInteractionPosition() => actions.Game.MapInteractionPosition.ReadValue<Vector2>();

    }
}