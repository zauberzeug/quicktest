using Xamarin.Forms;

namespace DemoApp
{
	public class DemoStack : StackLayout
	{
		public DemoStack()
		{
			Children.Add(new Label {
				Text = "Nested label",
				BackgroundColor = Color.FloralWhite,
				Margin = 5,
			});

			BackgroundColor = Color.Gray.MultiplyAlpha(0.2);

			GestureRecognizers.Add(new TapGestureRecognizer {
				Command = new Command(o => App.PushMessagePage("StackLayout tapped")),
			});
		}
	}
}
