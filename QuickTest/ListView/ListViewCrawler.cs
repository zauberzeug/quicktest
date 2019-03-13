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
        static ConditionalWeakTable<ListView, ListViewCellCache> cellCaches = new ConditionalWeakTable<ListView, ListViewCellCache>();

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
            cellCache.ResetAll();
            if (listView.IsGroupingEnabled)
                return listView.TemplatedItems.Cast<ListViewItemsList>().Select(t => GetCellGroup(listView, t, cellCache)).ToList();
            else
                return new List<CellGroup> { GetCellGroup(listView, listView.TemplatedItems, cellCache) };
        }

        static CellGroup GetCellGroup(ListView listView, ListViewItemsList templatedItems, ListViewCellCache cellCache)
        {
            var result = new CellGroup();
            result.Header = templatedItems.HeaderContent;
            result.Content = new List<Cell>();
            for (int i = 0; i < templatedItems.Count; i++) {
                var item = (templatedItems as ITemplatedItemsList<Cell>).ListProxy[i];
                var realCache = cellCache.GetCache(listView, item);
                var reusedCell = realCache.GetNextCell();
                if (reusedCell != null) {
                    reusedCell.SendDisappearing();
                    (templatedItems as ITemplatedItemsList<Cell>).UpdateContent(reusedCell, i);
                    reusedCell.SendAppearing();
                    result.Content.Add(reusedCell);
                } else {
                    var cell = templatedItems[i];
                    realCache.Add(cell);
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

    class ListViewCellCache
    {
        Dictionary<DataTemplate, CellCache> cellCaches = new Dictionary<DataTemplate, CellCache>();
        CellCache defaultCache = new CellCache();

        public void ResetAll()
        {
            defaultCache.ResetReuse();
            foreach (var cellCache in cellCaches.Values)
                cellCache.ResetReuse();
        }

        public CellCache GetCache(ListView listView, object item)
        {
            DataTemplate template;

            var selector = listView.ItemTemplate as DataTemplateSelector;
            if (selector == null)
                return defaultCache;

            template = selector.SelectTemplate(item, listView);

            if (cellCaches.ContainsKey(template))
                return cellCaches[template];
            else {
                var cellCache = new CellCache();
                cellCaches.Add(template, cellCache);
                return cellCache;
            }
        }
    }

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
