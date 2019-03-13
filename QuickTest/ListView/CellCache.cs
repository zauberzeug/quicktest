using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace QuickTest
{
    class CellCache
    {
        List<Cell> cells = new List<Cell>();
        int reuseIndex = 0;

        public void ResetReuse()
        {
            reuseIndex = 0;
        }

        public Cell GetNextCell()
        {
            if (cells.Count > reuseIndex)
                return cells[reuseIndex++];
            else
                return null;
        }

        public void Add(Cell cell)
        {
            if (reuseIndex < cells.Count)
                throw new InvalidOperationException("Cells can only be added after cached cells have been used.");
            cells.Add(cell);
            reuseIndex++;
        }
    }
}
