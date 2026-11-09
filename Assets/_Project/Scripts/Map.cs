using System;
using System.Collections.Generic;
using Rodak.Hexagons;
using Rodak.Hexagons.HexGrid;
using Rodak.Hexagons.HexNavigation;
using UnityEngine;

namespace Rodak.Hexdle
{
    public class Map : MonoBehaviour
    {
        [SerializeField] private int mapSize = 8;

        [Header("Map")]
        [SerializeField] private CellBehaviour cellBehaviourPrefab;

        private HexagonGrid<Cell> grid;
        private readonly List<char> revealedLetters = new();

        public List<char> RevealedLetters => revealedLetters;


        private void Awake()
        {
            GameManager.Instance.OnGameStarted += GameManager_OnGameStarted;
        }

        private void GameManager_OnGameStarted()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            grid = new(mapSize, (hexagon) => new(hexagon));

            List<Hexagon> letterPositions = new(grid.Hexagons);
            letterPositions.Shuffle();

            for (int i = 0; i < Words.Letters.Count; i++)
            {
                grid[letterPositions[i]].SetLetter(Words.Letters[i]);
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
            }
        }

        private void Update()
        {
            // grid.DebugDraw(PlacementPlane.XYPlane, Color.red);
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
