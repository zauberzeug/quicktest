using Xamarin.Forms;

namespace DemoApp
{
	public class DemoLabel : Label
	{
		public DemoLabel()
		{
			Text = "Label";

			BackgroundColor = Color.FloralWhite;

			GestureRecognizers.Add(new TapGestureRecognizer {
				Command = new Command(o => App.PushMessagePage("Label tapped")),
			});
		}
	}
}
