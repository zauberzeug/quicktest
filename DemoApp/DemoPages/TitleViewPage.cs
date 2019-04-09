using Xamarin.Forms;

namespace DemoApp
{
    public class TitleViewPage : ContentPage
    {
        public TitleViewPage()
        {
            var titleView = new StackLayout {
                Children = {
                    new Label { Text = "TitleViewLabel" }
                }
            };
            NavigationPage.SetTitleView(this, titleView);
            Content = new StackLayout { };
        }
    }
}
