using Xamarin.Forms;

namespace DemoApp
{
    public class PopupDemoPage : ContentPage
    {
        Label alertResult;

        public PopupDemoPage()
        {
            Title = "Popup demo";

            Content = new StackLayout {
                Children = {
                    new Button {
                        Text = "Show yes/no alert",
                        Command = new Command(ShowYesNoAlert),
                    },
                    new Button {
                        Text = "Show ok alert",
                        Command = new Command(ShowOkAlert),
                    },
                    (alertResult = new Label()),
                }
            };
        }

        async void ShowYesNoAlert()
        {
            var result = await DisplayAlert("Alert", "Message", "Yes", "No");
            alertResult.Text = $"Alert result: {result}";
        }

        async void ShowOkAlert()
        {
            await DisplayAlert("Alert", "Message", "Ok");
            alertResult.Text = $"Alert result: Ok";
        }
    }
}
