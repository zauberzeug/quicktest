using System.Collections.Generic;
using Xamarin.Forms;

namespace DemoApp
{
    public class DemoListViewWithItemViewCell : DemoListView
    {
        public DemoListViewWithItemViewCell(ListViewCachingStrategy cachingStrategy) : base(cachingStrategy)
        {
            ItemsSource = new List<Item> { new Item { Name = "Item A3" }, new Item { Name = "Item B3" }, new Item { Name = "Item C3" } };
            ItemTemplate = new DataTemplate(typeof(ItemDemoCell));

            BackgroundColor = Color.GhostWhite;

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
}