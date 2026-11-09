using Rodak.Hexagons;
using Rodak.Hexagons.HexGeometry;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Rodak.Hexdle
{
    public class CellBehaviour : MonoBehaviour
    {
        public Cell Cell;

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

        private bool WasMousePressedThisFrame => Mouse.current.leftButton.wasPressedThisFrame || Mouse.current.rightButton.wasPressedThisFrame;
        private bool WasMouseReleasedThisFrame => Mouse.current.leftButton.wasReleasedThisFrame || Mouse.current.rightButton.wasReleasedThisFrame;

        private bool isHovered;
        private bool isPressed;
        private int pressedButtonIndex;

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
        }

        private void Update()
        {
            textContainer.SetActive(Cell.IsRevealed);
            UpdateMouse();
            UpdateUI();
        }

        private void UpdateMouse()
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
            Hexagon mouseHexagon = HexagonGeometryExtensions.GetHexagonAt(worldPoint.x, worldPoint.y);
            if (mouseHexagon == Cell.Position)
            {
                isHovered = true;
            }
            else
            {
                isHovered = false;
            }

            if (isHovered && WasMousePressedThisFrame)
            {
                isPressed = true;
                pressedButtonIndex = Mouse.current.leftButton.isPressed ? 0 : 1;
            }

            if (isPressed && WasMouseReleasedThisFrame)
            {
                isPressed = false;
                if (isHovered)
                {
                    if (pressedButtonIndex == 0)
                    {
                        OnClickLeft();
                    }
                    else if (pressedButtonIndex == 1)
                    {
                        OnClickRight();
                    }
                }
            }
        }

        private void UpdateUI()
        {
            cover.SetActive(!Cell.IsRevealed);
            outline.SetActive(isHovered);
            danger.SetActive(Cell.IsRevealed && Cell.IsInsideWord);

            bool showMid = !Cell.IsRevealed && isPressed;

            middle.SetActive(showMid);
            if (!Cell.IsFlagged)
            {
                eyeClosed.SetActive(pressedButtonIndex == 0 && showMid && !isHovered);
                eyeOpen.SetActive(pressedButtonIndex == 0 && showMid && isHovered);
            }

            if (Cell.IsFlagged)
            {
                middle.SetActive(true);
                flagThin.SetActive(pressedButtonIndex == 1 && showMid && isHovered);
                flagThick.SetActive(pressedButtonIndex == 1 && showMid && !isHovered || !showMid);
            }
            else
            {
                flagThin.SetActive(pressedButtonIndex == 1 && showMid && !isHovered);
                flagThick.SetActive(pressedButtonIndex == 1 && showMid && isHovered);
            }
        }

        private void OnClickLeft()
        {
            GameManager.Instance.GainPenalty(GameManager.PenaltyType.RevealCells);
            Cell.Reveal();
        }

        private void OnClickRight()
        {
            Cell.ToggleFlag();
        }
    }
}
