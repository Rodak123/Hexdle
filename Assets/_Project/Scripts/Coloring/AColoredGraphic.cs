using UnityEngine;
using UnityEngine.UI;

namespace Rodak.Hexdle
{
    public abstract class AColoredGraphic<T> : MonoBehaviour where T : Graphic
    {
        [SerializeField, Range(0, 100)] private float alpha = 100;
        [SerializeField] private bool isInverted;

        public bool IsInverted => isInverted;

        private void Awake()
        {
            ColorManager.Instance.OnColorChanged += ColorManager_OnColorChanged;
            UpdateUI();
        }

        private void UpdateUI()
        {
            GetComponent<T>().color = isInverted ? new Color(0, 0, 0, alpha / 100f) : ColorManager.Instance.GetColor(alpha / 100f);
        }

        private void ColorManager_OnColorChanged()
        {
            UpdateUI();
        }

        public void SetIsInverted(bool isInverted)
        {
            this.isInverted = isInverted;
            UpdateUI();
        }
    }
}
