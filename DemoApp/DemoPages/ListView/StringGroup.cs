using System.Collections.Generic;

namespace DemoApp
{
    public class StringGroup : List<string>
    {
        public string Title { get; private set; }

        public StringGroup(IEnumerable<string> items, string title)
        {
            Title = title;
            AddRange(items);
        }
    }
}