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

        public event Action<char> OnLetterRevealed;

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

            List<Hexagon> positions = new(grid.Hexagons);
            positions.Shuffle();

            for (int i = 0; i < Words.Letters.Count; i++)
            {
                grid[positions[i]].SetLetter(Words.Letters[i]);
            }

            foreach (Hexagon hexagon in grid.Hexagons)
            {
                Cell cell = grid[hexagon];

                cell.OnRevealed += Cell_OnRevealed;

                cell.UpdateDanger(grid);

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
                OnLetterRevealed?.Invoke(cell.Letter);
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
                    neighbor.Reveal();
                }
            }
        }
    }
}
