using Xamarin.Forms;

namespace DemoApp
{
    public class TitleViewPage : ContentPage
    {
        public TitleViewPage()
        {
            var titleView = new StackLayout {
                Orientation = StackOrientation.Horizontal,
                Spacing = 10,
                Children = {
                    new Label { Text = "TitleViewLabel", VerticalOptions = LayoutOptions.Center },
                    new Button {
                        Text = "TitleViewButton",
                        Command = new Command(OnTitleViewButtonTapped),
                    },
                }
            };
            NavigationPage.SetTitleView(this, titleView);
            Content = new StackLayout { };
        }

        public void OnTitleViewButtonTapped()
        {
            (Content as StackLayout).Children.Add(new Label { Text = "Label added by button" });
        }
    }
}
