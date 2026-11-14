using UnityEngine;
using UnityEngine.UI;

namespace Rodak.Hexdle
{
    public class AudioSettingsSliderUI : MonoBehaviour
    {
        [SerializeField] public AudioSettingsManager.AudioMixerCategory category;

        [Header("UI")]
        [SerializeField] private Slider slider;

        private void Awake()
        {
            slider.onValueChanged.AddListener(Slider_OnValueChanged);
        }

        private void Start()
        {
            float volume = AudioSettingsManager.Instance.GetVolume(category);
            slider.value = volume;
        }

        private void Slider_OnValueChanged(float value)
        {
            AudioSettingsManager.Instance.SetVolume(value, category);
            AudioSettingsManager.Instance.ApplyAllSettings();
        }
    }
}
