using System.Collections.Generic;
using Xamarin.Forms;

namespace DemoApp
{
    public class ListViewDemoPage : ContentPage
    {
        public ListViewDemoPage()
        {
            Title = "ListView demo";

            Content = new StackLayout {
                Children = {
                    new DemoListViewWithTextCell(){Header = "plain header", Footer = "plain footer"},
                    new DemoListViewWithStringViewCell(){Header = new DemoLabel("header label"), Footer = new DemoLabel("footer label")},
                    new DemoListViewWithItemViewCell(),
                    new DemoListViewWithGroups(),
                },
            };
        }
    }

    public class DemoListViewWithTextCell : ListView
    {
        public DemoListViewWithTextCell()
        {
            ItemsSource = new List<string> { "Item A1", "Item B1", "Item C1" };
            ItemTemplate = new DataTemplate(typeof(TextCell));

            BackgroundColor = Color.GhostWhite;
            HeightRequest = 200;

            ItemTemplate.SetBinding(TextCell.TextProperty, ".");
            ItemTapped += (sender, e) => App.ShowMessage("Success", e.Item + " tapped");
        }
    }

    public class DemoListViewWithStringViewCell : ListView
    {
        public DemoListViewWithStringViewCell()
        {
            ItemsSource = new List<string> { "Item A2", "Item B2", "Item C2" };
            ItemTemplate = new DataTemplate(typeof(StringDemoCell));

            BackgroundColor = Color.GhostWhite;
            HeightRequest = 200;

            ItemTapped += (sender, e) => App.ShowMessage("Success", e.Item + " tapped");
        }

        class StringDemoCell : ViewCell
        {
            public StringDemoCell()
            {
                var label = new DemoLabel();
                label.SetBinding(Label.TextProperty, ".");
                View = label;
            }
        }
    }

    public class DemoListViewWithItemViewCell : ListView
    {
        public DemoListViewWithItemViewCell()
        {
            ItemsSource = new List<Item> { new Item { Name = "Item A3" }, new Item { Name = "Item B3" }, new Item { Name = "Item C3" } };
            ItemTemplate = new DataTemplate(typeof(ItemDemoCell));

            BackgroundColor = Color.GhostWhite;
            HeightRequest = 200;

            ItemTapped += (sender, e) => App.ShowMessage("Success", (e.Item as Item).Name + " tapped");
        }

        class Item : BindableObject
        {
            public static readonly BindableProperty NameProperty = BindableProperty.Create(nameof(Name), typeof(string), typeof(Item), null);

            public string Name {
                get { return (string)GetValue(NameProperty); }
                set { SetValue(NameProperty, value); }
            }
        }

        class ItemDemoCell : ViewCell
        {
            public ItemDemoCell()
            {
                var label = new DemoLabel();
                label.SetBinding(Label.TextProperty, nameof(Item.Name));
                View = label;
            }
        }
    }

    public class DemoListViewWithGroups : ListView
    {
        public DemoListViewWithGroups()
        {
            ItemsSource = new List<List<string>> {
                new StringGroup(new [] { "A4", "B4", "C4" }, "Group 4"),
                new StringGroup(new [] { "A5", "B5", "C5" }, "Group 5"),
            };
            ItemTemplate = new DataTemplate(typeof(StringDemoCell));
            IsGroupingEnabled = true;
            GroupDisplayBinding = new Binding(nameof(StringGroup.Title));

            BackgroundColor = Color.GhostWhite;
            HeightRequest = 200;

            ItemTapped += (sender, e) => App.ShowMessage("Success", (e.Item as string) + " tapped");
        }

        public class StringDemoCell : ViewCell
        {
            public StringDemoCell()
            {
                var label = new DemoLabel();
                label.SetBinding(Label.TextProperty, ".");
                View = label;
            }
        }

        class StringGroup : List<string>
        {
            public string Title { get; private set; }

            public StringGroup(IEnumerable<string> items, string title)
            {
                Title = title;
                AddRange(items);
            }
        }
    }
}