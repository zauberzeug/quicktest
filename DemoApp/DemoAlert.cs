using Xamarin.Forms;

namespace DemoApp
{
	public class DemoAlert : Button
	{
		public DemoAlert()
		{
			Text = "DemoAlert";

			Command = new Command(o => Application.Current.MainPage.DisplayAlert("Alert", "Message", "Ok"));
		}
	}
}
