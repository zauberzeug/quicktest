using System.Collections.Generic;
using Xamarin.Forms;

namespace DemoApp
{
    public class DemoListViewWithGestureRecognizers : DemoListView
    {
        public DemoListViewWithGestureRecognizers()
        {
            ItemsSource = new List<Item> { new Item { Name = "Item" } };
            ItemTemplate = new DataTemplate(typeof(ItemGestureCell));

            BackgroundColor = Color.GhostWhite;

            ItemTapped += (sender, e) => App.ShowMessage("ListView Cell", (e.Item as Item).Name + " tapped");
        }

        class Item : BindableObject
        {
            public static readonly BindableProperty NameProperty = BindableProperty.Create(nameof(Name), typeof(string), typeof(Item), null);

            public string Name {
                get { return (string)GetValue(NameProperty); }
                set { SetValue(NameProperty, value); }
            }
        }

        class ItemGestureCell : ViewCell
        {
            public ItemGestureCell()
            {
                var name = new DemoLabel();
                name.SetBinding(Label.TextProperty, nameof(Item.Name));

                var tappable = new DemoLabel { BackgroundColor = Color.DimGray, Text = "tap me", Margin = new Thickness(10, 0) };
                tappable.GestureRecognizers.Add(new TapGestureRecognizer {
                    Command = new Command(o => {
                        tappable.Text += "!";
                    })
                });

                View = new FlexLayout {
                    Margin = new Thickness(20, 0),
                    Children ={
                        name,
                        tappable,
                        }
                };
            }
        }
    }
}