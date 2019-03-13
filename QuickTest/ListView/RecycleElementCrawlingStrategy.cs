using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using ListViewItemsList = Xamarin.Forms.Internals.TemplatedItemsList<Xamarin.Forms.ItemsView<Xamarin.Forms.Cell>, Xamarin.Forms.Cell>;

namespace QuickTest
{
    public class RecycleElementCrawlingStrategy : CrawlingStrategy
    {
        static ConditionalWeakTable<ListView, CellCacheProvider> cacheProviders = new ConditionalWeakTable<ListView, CellCacheProvider>();

        public List<CellGroup> GetCellGroups(ListView listView)
        {
            var cacheProvider = cacheProviders.GetValue(listView, (l) => new CellCacheProvider(l));
            cacheProvider.RestartReuse();
            if (listView.IsGroupingEnabled)
                return listView.TemplatedItems.Cast<ListViewItemsList>().Select(t => GetCellGroup(t, cacheProvider)).ToList();
            else
                return new List<CellGroup> { GetCellGroup(listView.TemplatedItems, cacheProvider) };
        }

        static CellGroup GetCellGroup(ITemplatedItemsList<Cell> templatedItems, CellCacheProvider cacheProvider)
        {
            var result = new CellGroup();
            result.Header = templatedItems.HeaderContent;
            result.Content = new List<Cell>();

            for (int i = 0; i < templatedItems.Count; i++) {
                var item = templatedItems.ListProxy[i];
                var cellCache = cacheProvider.GetCellCache(item);
                Cell cell = cellCache.GetNextUnusedCell();

                if (cell != null) {
                    cell.SendDisappearing();
                    templatedItems.UpdateContent(cell, i);
                    cell.SendAppearing();
                } else {
                    cell = templatedItems[i];
                    cellCache.AddUsedCell(cell);
                    cell.SendAppearing();
                }

                result.Content.Add(cell);
            }
            return result;
        }
    }
}
