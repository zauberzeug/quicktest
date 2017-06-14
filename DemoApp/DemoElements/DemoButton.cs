using Xamarin.Forms;

namespace DemoApp
{
	public class DemoButton : Button
	{
		public DemoButton(string text)
		{
			Text = text;

			BackgroundColor = Color.AliceBlue;

			Command = new Command(o => App.ShowMessage("Success", text + " tapped"));
		}
	}
}
