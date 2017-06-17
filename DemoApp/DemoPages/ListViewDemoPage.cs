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
                    new DemoListViewWithTextCell(),
                    new DemoListViewWithViewCell(),
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

    public class DemoListViewWithViewCell : ListView
    {
        public DemoListViewWithViewCell()
        {
            ItemsSource = new List<string> { "Item A2", "Item B2", "Item C2" };
            ItemTemplate = new DataTemplate(typeof(DemoCell));

            BackgroundColor = Color.GhostWhite;
            HeightRequest = 200;

            ItemTapped += (sender, e) => App.ShowMessage("Success", e.Item + " tapped");
        }
    }

    public class DemoCell : ViewCell
    {
        public DemoCell()
        {
            var label = new DemoLabel();
            label.SetBinding(Label.TextProperty, ".");
            View = new StackLayout {
                Children = { label },
                BackgroundColor = Color.Gray.MultiplyAlpha(0.2),
            };
        }
    }
}
