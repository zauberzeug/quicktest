using Xamarin.Forms;

namespace DemoApp
{
    public class PopupDemoPage : ContentPage
    {
        Label result;

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
                    new Button {
                        Text = "Show action sheet",
                        Command = new Command(ShowActionSheet),
                    },
                    new Button {
                        Text = "Show action sheet without cancel",
                        Command = new Command(ShowActionSheetWithoutCancel),
                    },
                    new Button {
                        Text = "Show action sheet without destruction",
                        Command = new Command(ShowActionSheetWithoutDestruction),
                    },
                    (result = new Label()),
                }
            };
        }

        async void ShowYesNoAlert()
        {
            var result = await DisplayAlert("Alert", "Message", "Yes", "No");
            this.result.Text = $"Alert result: {result}";
        }

        async void ShowOkAlert()
        {
            await DisplayAlert("Alert", "Message", "Ok");
            result.Text = $"Alert result: Ok";
        }

        async void ShowActionSheet()
        {
            var result = await DisplayActionSheet("Action sheet", "Cancel", "Destroy", "Option 1", "Option 2");
            this.result.Text = $"Action sheet result: {result}";
        }

        async void ShowActionSheetWithoutCancel()
        {
            var result = await DisplayActionSheet("Action sheet without cancel", null, "Destroy", "Option 1");
            this.result.Text = $"Action sheet without cancel: {result}";
        }

        async void ShowActionSheetWithoutDestruction()
        {
            var result = await DisplayActionSheet("Action sheet without destruction", "Cancel", null);
            this.result.Text = $"Action sheet without destruction result: {result}";
        }
    }
}
