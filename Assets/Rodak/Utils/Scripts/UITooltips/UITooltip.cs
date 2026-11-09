using UnityEngine;

namespace Rodak.Utils.UITooltips
{
    public class UITooltip : MonoBehaviour
    {
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
    }
}