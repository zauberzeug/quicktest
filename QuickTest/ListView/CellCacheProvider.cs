using System.Collections.Generic;
using Xamarin.Forms;

namespace QuickTest
{
    class CellCacheProvider
    {
        Dictionary<DataTemplate, CellCache> cellCaches = new Dictionary<DataTemplate, CellCache>();
        CellCache defaultCache = new CellCache();
        ListView listView;


        public CellCacheProvider(ListView listView)
        {
            this.listView = listView;
        }

        public void RestartReuse()
        {
            defaultCache.RestartReuse();
            foreach (var cellCache in cellCaches.Values)
                cellCache.RestartReuse();
        }

        public CellCache GetCellCache(object item)
        {
            DataTemplate template;

            var selector = listView.ItemTemplate as DataTemplateSelector;
            if (selector == null)
                return defaultCache;

            template = selector.SelectTemplate(item, listView);
            return cellCaches.GetOrCreate(template);
        }
    }
}
