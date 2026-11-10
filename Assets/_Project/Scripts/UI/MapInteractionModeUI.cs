using UnityEngine;
using UnityEngine.UI;

namespace Rodak.Hexdle
{
    public class MapInteractionModeUI : MonoBehaviour
    {
        [SerializeField] Map map;

        [Header("UI")]
        [SerializeField] private Button toggleButton;
        [SerializeField] private ColoredImage buttonBg;
        [SerializeField] private GameObject revealModeContainer;
        [SerializeField] private GameObject flagModeContainer;

        private void Awake()
        {
            toggleButton.onClick.AddListener(Button_OnClick);
        }

        private void Button_OnClick()
        {
            switch (map.CellInteractionMode)
            {
                case CellInteractionMode.Reveal:
                    map.CellInteractionMode = CellInteractionMode.Flag;
                    break;
                case CellInteractionMode.Flag:
                    map.CellInteractionMode = CellInteractionMode.Reveal;
                    break;
            }
        }

        private void Update()
        {
            revealModeContainer.SetActive(false);
            flagModeContainer.SetActive(false);
            switch (map.CellInteractionMode)
            {
                case CellInteractionMode.Reveal:
                    buttonBg.SetIsInverted(true);
                    revealModeContainer.SetActive(true);
                    break;
                case CellInteractionMode.Flag:
                    buttonBg.SetIsInverted(false);
                    flagModeContainer.SetActive(true);
                    break;
            }
        }
    }
}
