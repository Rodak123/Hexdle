using UnityEngine;
using UnityEngine.EventSystems;

namespace Rodak.Utils.UITooltips
{
    [RequireComponent(typeof(RectTransform))]
    public class HasUITooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private UITooltip tooltip;

        [Header("Config")]
        public bool ShowOnHover
        {
            get => showOnHover;
            set
            {
                showOnHover = value;
                UpdateUI();
            }
        }

        private bool isHovered;
        private bool showOnHover = true;

        private void Start()
        {
            if (tooltip == null)
            {
                Debug.LogWarning($"No tooltip set.");
            }

            UpdateUI();
        }

        private void UpdateUI()
        {
            if (tooltip == null) return;

            if (showOnHover == false)
            {
                tooltip.Hide();
                return;
            }

            if (isHovered)
            {
                tooltip.Show();
            }
            else
            {
                tooltip.Hide();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isHovered = true;
            UpdateUI();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isHovered = false;
            UpdateUI();
        }
    }
}