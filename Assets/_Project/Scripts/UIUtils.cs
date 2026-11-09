using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Rodak.Hexdle
{
    public static class UIUtils
    {
        public static bool IsPointerOverUI()
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
    }
}