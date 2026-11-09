using System;
using Rodak.Utils.Singleton;
using UnityEngine;

namespace Rodak.Hexdle
{
    public class ColorManager : SingletonMonoBehaviour<ColorManager>
    {
        [SerializeField] private Color[] colors;

        private int selectedColorIndex;

        private int NextSelectedColorIndex => (selectedColorIndex + 1) % colors.Length;

        public Color Color => colors[selectedColorIndex];
        public Color NextColor => colors[NextSelectedColorIndex];

        public event Action OnColorChanged;

        public Color GetColor(float alpha)
        {
            return new(Color.r, Color.g, Color.b, alpha);
        }

        public void CycleColor()
        {
            selectedColorIndex = NextSelectedColorIndex;
            OnColorChanged?.Invoke();
        }
    }
}
