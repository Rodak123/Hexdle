using System;
using Rodak.Hexagons;
using Rodak.Hexagons.HexGrid;
using Rodak.Hexagons.HexNavigation;

namespace Rodak.Hexdle
{
    public class Cell
    {
        private char? letter = null;
        private int dangerLevel;
        private bool isRevealed;
        private bool isFlagged;

        public readonly Hexagon Position;
        public char Letter => letter.HasValue ? letter.Value : '*';
        public bool HasLetter => letter != null;

        public int DangerLevel => dangerLevel;
        public bool IsRevealed => isRevealed;
        public bool IsFlagged => isFlagged;

        public bool IsInsideWord => HasLetter && GameManager.Instance.Word.Contains(Letter);

        public event Action<Cell> OnRevealed;

        public CellType Type
        {
            get
            {
                if (IsInsideWord) return CellType.TargetLetter;
                return HasLetter ? CellType.RandomLetter : CellType.NoLetter;
            }
        }

        public Cell(Hexagon position)
        {
            Position = position;
        }

        public void SetLetter(char? letter)
        {
            this.letter = letter;
        }

        public void UpdateDanger(HexagonGrid<Cell> grid)
        {
            dangerLevel = 0;

            foreach (Hexagon around in Position.GetNeighbors())
            {
                if (!grid.TryGetValue(around, out Cell neighbor))
                {
                    continue;
                }

                if (!neighbor.IsInsideWord)
                {
                    continue;
                }

                dangerLevel++;
            }
        }

        public void Reveal()
        {
            if (isRevealed || isFlagged) return;
            isRevealed = true;
            OnRevealed?.Invoke(this);
        }

        public void ToggleFlag()
        {
            if (isRevealed) return;
            isFlagged = !isFlagged;
        }

        public enum CellType
        {
            NoLetter,
            RandomLetter,
            TargetLetter,
        }
    }
}
