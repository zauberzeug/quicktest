using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using ListViewItemsList = Xamarin.Forms.Internals.TemplatedItemsList<Xamarin.Forms.ItemsView<Xamarin.Forms.Cell>, Xamarin.Forms.Cell>;

namespace QuickTest
{
    public class RecycleElementCrawlingStrategy : CrawlingStrategy
    {
        static ConditionalWeakTable<ListView, ListViewCellCache> cellCaches = new ConditionalWeakTable<ListView, ListViewCellCache>();

        public List<CellGroup> GetCellGroups(ListView listView)
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
    }
}
