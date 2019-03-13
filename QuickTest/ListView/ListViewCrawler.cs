using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using ListViewItemsList = Xamarin.Forms.Internals.TemplatedItemsList<Xamarin.Forms.ItemsView<Xamarin.Forms.Cell>, Xamarin.Forms.Cell>;

namespace QuickTest
{
    public static class ListViewCrawler
    {
        static CrawlingStrategy retainStrategy = new RetainElementCrawlingStrategy();
        static CrawlingStrategy recycleStrategy = new RecycleElementCrawlingStrategy();

        public static List<CellGroup> GetCellGroups(ListView listView)
        {
            return GetCrawlingStrategy(listView.CachingStrategy).GetCellGroups(listView);
        }

        static CrawlingStrategy GetCrawlingStrategy(ListViewCachingStrategy cachingStrategy)
        {
            if ((cachingStrategy & ListViewCachingStrategy.RecycleElement) != 0)
                return recycleStrategy;
            else
                return retainStrategy;
        }
    }
}
