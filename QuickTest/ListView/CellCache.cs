using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace QuickTest
{
    class CellCache
    {
        List<Cell> cells = new List<Cell>();
        int reuseIndex = 0;

        public void RestartReuse()
        {
            reuseIndex = 0;
        }

        public Cell GetNextUnusedCell()
        {
            if (cells.Count > reuseIndex)
                return cells[reuseIndex++];
            else
                return null;
        }

        public void AddUsedCell(Cell cell)
        {
            if (cell == null)
                throw new ArgumentException($"Cell must not be null");
            if (reuseIndex < cells.Count)
                throw new InvalidOperationException("Cells can only be added after all cached cells have been reused.");
            cells.Add(cell);
            reuseIndex++;
        }
    }
}
