using System;
using System.Collections.Generic;
using Rodak.Hexagons;
using Rodak.Hexagons.HexGrid;
using Rodak.Hexagons.HexNavigation;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Rodak.Hexdle
{
    public class Map : MonoBehaviour
    {
        [SerializeField] private int mapSize = 8;

        [Header("Map")]
        [SerializeField] private CellBehaviour cellBehaviourPrefab;
        public CellInteractionMode CellInteractionMode;

        private HexagonGrid<Cell> grid;
        private readonly List<char> revealedLetters = new();

        private bool didInteract;
        private CellInteractionMode? nextMode;

        public event Action OnInteracted;
        public List<char> RevealedLetters => revealedLetters;

        private void Awake()
        {
            GameManager.Instance.OnGameStarted += GameManager_OnGameStarted;
        }

        private void Start()
        {
            InputManager.Instance.MapInteractionToggle.started += MapInteractionToggle;
            InputManager.Instance.MapInteractionToggle.performed += MapInteractionToggle;
            InputManager.Instance.MapInteractionToggle.canceled += MapInteractionToggle;
        }

        private void Update()
        {
            if (InputManager.Instance.MapInteraction.WasReleasedThisFrame())
            {
                OnInteracted?.Invoke();
                didInteract = false;
            }

            if (nextMode.HasValue)
            {
                CellInteractionMode = nextMode.Value;
                nextMode = null;
            }
        }

        private void MapInteractionToggle(InputAction.CallbackContext ctx)
        {
            if (InputManager.Instance.MapInteractionToggle.IsPressed())
            {
                nextMode = CellInteractionMode.Flag;
            }
            else
            {
                nextMode = CellInteractionMode.Reveal;
            }
        }

        private void GameManager_OnGameStarted()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            grid = new(mapSize, (hexagon) => new(hexagon));

            List<Hexagon> letterPositions = new(grid.Hexagons);
            for (int i = letterPositions.Count - 1; i >= 0; i--)
            {
                if (!grid[letterPositions[i]].IsAWall) continue;
                letterPositions.RemoveAt(i);
            }
            for (int n = 0; n < Words.Letters.Count - 1; n++)
            {
                int k = GameStateManager.Random.Next(0, n + 1);
                Hexagon value = letterPositions[k];
                letterPositions[k] = letterPositions[n];
                letterPositions[n] = value;
            }

            for (int i = 0; i < Words.Letters.Count; i++)
            {
                Cell cell = grid[letterPositions[i]];
                cell.SetLetter(Words.Letters[i]);
            }

            List<Hexagon> gridHexagons = new(grid.Hexagons);

            if (GameStateManager.State.CellStates.Length != gridHexagons.Count)
            {
                GameStateManager.State.CellStates = new GameStateManager.CellState[gridHexagons.Count];
            }

            foreach (Hexagon hexagon in gridHexagons)
            {
                Cell cell = grid[hexagon];

                int saveIndex = gridHexagons.IndexOf(hexagon);

                cell.OnRevealed += Cell_OnRevealed;

                cell.UpdateState(saveIndex, grid);

                CellBehaviour cellBehaviour = Instantiate(cellBehaviourPrefab, transform);
                cellBehaviour.Cell = cell;
                cellBehaviour.Map = this;
            }
        }

        private void Cell_OnRevealed(Cell cell)
        {
            if (cell.HasLetter)
            {
                revealedLetters.Add(cell.Letter);
            }

            if (cell.IsInsideWord)
            {
                GameManager.Instance.GainPenalty(GameManager.PenaltyType.HitLetter);
            }

            if (cell.DangerLevel == 0 && !cell.IsInsideWord)
            {
                foreach (Hexagon around in cell.Position.GetNeighbors())
                {
                    if (!grid.TryGetValue(around, out Cell neighbor))
                    {
                        continue;
                    }
                    neighbor.TryReveal();
                }
            }
        }
    }
}
