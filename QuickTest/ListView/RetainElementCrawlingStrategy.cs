using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using ListViewItemsList = Xamarin.Forms.Internals.TemplatedItemsList<Xamarin.Forms.ItemsView<Xamarin.Forms.Cell>, Xamarin.Forms.Cell>;

namespace QuickTest
{
    public class RetainElementCrawlingStrategy : CrawlingStrategy
    {
        public List<CellGroup> GetCellGroups(ListView listView)
        {
            if (listView.IsGroupingEnabled)
                return listView.TemplatedItems.Cast<ListViewItemsList>().Select(t => GetCellGroup(t)).ToList();
            else
                return new List<CellGroup> { GetCellGroup(listView.TemplatedItems) };
        }

        static CellGroup GetCellGroup(ListViewItemsList templatedItems)
        {
            return new CellGroup() {
                Header = templatedItems.HeaderContent,
                Content = templatedItems.ToList(),
            };
        }
    }
}
