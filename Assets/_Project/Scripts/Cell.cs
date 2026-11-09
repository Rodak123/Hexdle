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

        private int saveIndex;

        private GameStateManager.CellState State => saveIndex >= GameStateManager.State.CellStates.Length ? null : GameStateManager.State.CellStates[saveIndex];

        public readonly Hexagon Position;
        public char Letter => letter.HasValue ? letter.Value : '*';
        public bool HasLetter => letter != null;

        public int DangerLevel => dangerLevel;
        public bool IsRevealed
        {
            get => State?.IsRevealed ?? false;
            private set
            {
                if (State == null) return;
                State.IsRevealed = value;
                GameStateManager.Save();
            }
        }
        public bool IsFlagged
        {
            get => State?.IsFlagged ?? false;
            private set
            {
                if (State == null) return;
                State.IsFlagged = value;
                GameStateManager.Save();
            }
        }

        public bool IsInsideWord => HasLetter && GameStateManager.State.Word.Contains(Letter);

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

        public void UpdateState(int saveIndex, HexagonGrid<Cell> grid)
        {
            this.saveIndex = saveIndex;
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

            if (State == null)
            {
                GameStateManager.State.CellStates[saveIndex] = new();
            }
        }

        public bool TryReveal()
        {
            if (IsRevealed || IsFlagged) return false;
            IsRevealed = true;
            OnRevealed?.Invoke(this);
            return true;
        }

        public void ToggleFlag()
        {
            if (IsRevealed) return;
            IsFlagged = !IsFlagged;
        }

        public enum CellType
        {
            NoLetter,
            RandomLetter,
            TargetLetter,
        }
    }
}
