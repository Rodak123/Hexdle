using Rodak.Hexagons;
using Rodak.Hexagons.HexGeometry;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rodak.Hexdle
{
    public class CellBehaviour : MonoBehaviour
    {
        public Cell Cell;
        public Map Map;

        [Header("UI")]
        [SerializeField] private Image hexagonImage;

        [Space]
        [SerializeField] private GameObject textContainer;
        [SerializeField] private TMP_Text letterText;
        [SerializeField] private TMP_Text dangerText;

        [Header("Sprites")]
        [SerializeField] private GameObject cover;
        [SerializeField] private GameObject outline;
        [SerializeField] private GameObject danger;
        [SerializeField] private GameObject middle;

        [Space]
        [SerializeField] private GameObject eyeClosed;
        [SerializeField] private GameObject eyeOpen;

        [Space]
        [SerializeField] private GameObject flagThin;
        [SerializeField] private GameObject flagThick;

        private bool isHovered;
        private bool isPressed;

        private void Start()
        {
            transform.position = Cell.Position.GetCenter();

            letterText.text = string.Empty;
            dangerText.text = Cell.DangerLevel > 0 ? $"{Cell.DangerLevel}" : string.Empty;

            switch (Cell.Type)
            {
                case Cell.CellType.RandomLetter:
                    letterText.text = $"{Cell.Letter}";
                    break;
                case Cell.CellType.TargetLetter:
                    letterText.text = $"{Cell.Letter}";
                    dangerText.text = string.Empty;
                    break;
            }

            cover.SetActive(false);
            outline.SetActive(false);
            danger.SetActive(false);
            eyeClosed.SetActive(false);
            eyeOpen.SetActive(false);
            flagThin.SetActive(false);
            flagThick.SetActive(false);

            Map.OnInteracted += Map_OnInteracted;
        }

        private void Map_OnInteracted()
        {
            if (!isPressed) return;
            isPressed = false;
            if (!isHovered) return;
            switch (Map.CellInteractionMode)
            {
                case CellInteractionMode.Reveal:
                    TryReveal();
                    break;
                case CellInteractionMode.Flag:
                    TryFlag();
                    break;
            }
        }

        private void Update()
        {
            textContainer.SetActive(Cell.IsRevealed);
            UpdateMouse();
            UpdateUI();
        }

        private void UpdateMouse()
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(InputManager.Instance.ReadMapInteractionPosition());
            Hexagon mouseHexagon = HexagonGeometryExtensions.GetHexagonAt(worldPoint.x, worldPoint.y);

            if (mouseHexagon == Cell.Position && !InputManager.Instance.IsPointerOverUI())
            {
                isHovered = true;
            }
            else
            {
                isHovered = false;
            }

            if (isHovered && InputManager.Instance.MapInteraction.WasPressedThisFrame())
            {
                isPressed = true;
            }
        }

        private void UpdateUI()
        {
            cover.SetActive(!Cell.IsRevealed);
            outline.SetActive(isHovered);
            danger.SetActive(Cell.IsRevealed && Cell.IsInsideWord);

            bool showMid = !Cell.IsRevealed && isPressed && !GameStateManager.State.GuessedRight;

            bool isRevealing = Map.CellInteractionMode == CellInteractionMode.Reveal;
            bool isFlagging = Map.CellInteractionMode == CellInteractionMode.Flag;

            middle.SetActive(showMid);
            if (!Cell.IsFlagged)
            {
                eyeClosed.SetActive(isRevealing && showMid && !isHovered);
                eyeOpen.SetActive(isRevealing && showMid && isHovered);
            }

            if (Cell.IsFlagged)
            {
                middle.SetActive(true);
                flagThin.SetActive(isFlagging && showMid && isHovered);
                flagThick.SetActive(isFlagging && showMid && !isHovered || !showMid);
            }
            else
            {
                flagThin.SetActive(isFlagging && showMid && !isHovered);
                flagThick.SetActive(isFlagging && showMid && isHovered);
            }
        }

        private void TryReveal()
        {
            if (GameStateManager.State.GuessedRight) return;
            if (Cell.TryReveal())
            {
                GameManager.Instance.GainPenalty(GameManager.PenaltyType.RevealCells);
            }
        }

        private void TryFlag()
        {
            if (GameStateManager.State.GuessedRight) return;
            Cell.ToggleFlag();
        }
    }
}
