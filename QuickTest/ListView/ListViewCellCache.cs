using System.Collections.Generic;
using Xamarin.Forms;

namespace QuickTest
{
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
}
