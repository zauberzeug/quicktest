using Xamarin.Forms;

namespace DemoApp
{
    public class TitleViewPage : ContentPage
    {
        Label infoLabel;

        public TitleViewPage()
        {
            infoLabel = new Label();
            var titelViewLabel = new Label {
                Text = "TitleViewLabel",
                VerticalOptions = LayoutOptions.Center
            };
            titelViewLabel.GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(OnTitleViewLabelTapped) });


            var titleView = new StackLayout {
                Orientation = StackOrientation.Horizontal,
                Spacing = 10,
                Children = {
                    titelViewLabel,
                    new Button {
                        Text = "TitleViewButton",
                        Command = new Command(OnTitleViewButtonTapped),
                    },
                }
            };
            NavigationPage.SetTitleView(this, titleView);
            Content = new StackLayout {
                Margin = 20,
                Children = { infoLabel }
            };
        }

        public void OnTitleViewLabelTapped() => infoLabel.Text = "Tapped on Label";

        public void OnTitleViewButtonTapped() => infoLabel.Text = "Tapped on Button";
    }
}
