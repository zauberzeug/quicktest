using System;
using Xamarin.Forms;

namespace DemoApp
{
    public class DemoListView : ListView
    {
        public DemoListView(ListViewCachingStrategy cachingStrategy) : base(cachingStrategy)
        {
            // ListView ignores caching stragey on platforms where recycling is not supported.
            // ListView will use caching strategy when runtimePlatform is null ("unit test mode"),
            // which can be set when initialising Xamarin.Forms.Mocks.
            if (CachingStrategy != cachingStrategy)
                throw new ArgumentException("Caching strategy must not be ignored.");
        }
    }
}