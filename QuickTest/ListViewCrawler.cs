using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using ListViewItemsList = Xamarin.Forms.Internals.TemplatedItemsList<Xamarin.Forms.ItemsView<Xamarin.Forms.Cell>, Xamarin.Forms.Cell>;

namespace QuickTest
{
    public static class ListViewCrawler
    {
        static ConditionalWeakTable<ListView, CellCache> cellCaches = new ConditionalWeakTable<ListView, CellCache>();

        public static List<CellGroup> GetCellGroups(ListView listView)
        {
            if ((listView.CachingStrategy & ListViewCachingStrategy.RecycleElement) != 0)
                return GetCellGroupsRecyclingCells(listView);
            else
                return GetCellGroupsRetainingCells(listView);
        }

        static List<CellGroup> GetCellGroupsRetainingCells(ListView listView)
        {
            if (listView.IsGroupingEnabled)
                return listView.TemplatedItems.Cast<ListViewItemsList>().Select(t => GetCellGroup(t)).ToList();
            else
                return new List<CellGroup> { GetCellGroup(listView.TemplatedItems) };
        }

        static List<CellGroup> GetCellGroupsRecyclingCells(ListView listView)
        {
            var cellCache = cellCaches.GetOrCreateValue(listView);
            cellCache.ResetReuse();
            if (listView.IsGroupingEnabled)
                return listView.TemplatedItems.Cast<ListViewItemsList>().Select(t => GetCellGroup(t, cellCache)).ToList();
            else
                return new List<CellGroup> { GetCellGroup(listView.TemplatedItems, cellCache) };
        }

        static CellGroup GetCellGroup(ListViewItemsList templatedItems, CellCache cellCache)
        {
            var result = new CellGroup();
            result.Header = templatedItems.HeaderContent;
            result.Content = new List<Cell>();
            for (int i = 0; i < templatedItems.Count; i++) {
                var reusedCell = cellCache.GetNextCell();
                if (reusedCell != null) {
                    reusedCell.SendDisappearing();
                    (templatedItems as ITemplatedItemsList<Cell>).UpdateContent(reusedCell, i);
                    reusedCell.SendAppearing();
                    result.Content.Add(reusedCell);
                } else {
                    var cell = templatedItems[i];
                    cellCache.Add(cell);
                    result.Content.Add(cell);
                }
            }
            return result;
        }

        static CellGroup GetCellGroup(ListViewItemsList templatedItems)
        {
            return new CellGroup() {
                Header = templatedItems.HeaderContent,
                Content = templatedItems.ToList(),
            };
        }
    }

    class CellCache
    {
        List<Cell> cells = new List<Cell>();
        int reuseIndex = 0;

        Guid guid = Guid.NewGuid();

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
            cells.Add(cell);
            reuseIndex++;
        }
    }
}
