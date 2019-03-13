using System.Collections.Generic;
using Xamarin.Forms;

namespace DemoApp
{
    public class DemoListViewWithGroupsAndHeaderTemplate : DemoListView
    {
        public DemoListViewWithGroupsAndHeaderTemplate()
        {
            ItemsSource = new List<List<string>> {
                new StringGroup(new [] { "A6", "B6", "C6" }, "Group 6"),
                new StringGroup(new [] { "A7", "B7", "C7" }, "Group 7"),
            };
            ItemTemplate = new DataTemplate(typeof(StringDemoCell));
            IsGroupingEnabled = true;
            GroupHeaderTemplate = new DataTemplate(typeof(GroupHeaderCell));
            BackgroundColor = Color.GhostWhite;

            ItemTapped += (sender, e) => App.ShowMessage("Success", (e.Item as string) + " tapped");
        }

        class GroupHeaderCell : ViewCell
        {
            public GroupHeaderCell()
            {
                var label = new DemoLabel();
                label.SetBinding(Label.TextProperty, "Title");
                View = label;
            }
        }
    }
}