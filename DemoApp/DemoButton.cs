using Xamarin.Forms;

namespace DemoApp
{
	public class DemoButton : Button
	{
		public DemoButton()
		{
			Text = "Button";

			BackgroundColor = Color.AliceBlue;

			Command = new Command(o => App.PushMessagePage("Button tapped"));
		}
	}
}
