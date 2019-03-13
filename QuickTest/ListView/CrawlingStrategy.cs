using System.Collections.Generic;
using Xamarin.Forms;

namespace QuickTest
{
    public interface CrawlingStrategy
    {
        List<CellGroup> GetCellGroups(ListView listView);
    }
}
