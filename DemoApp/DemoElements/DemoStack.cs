using Xamarin.Forms;

namespace DemoApp
{
	public class DemoStack : StackLayout
	{
		public DemoStack()
		{
			Children.Add(new DemoLabel("Nested label"));

			BackgroundColor = Color.Gray.MultiplyAlpha(0.2);
			Padding = 10;

			GestureRecognizers.Add(new TapGestureRecognizer {
				Command = new Command(o => App.ShowMessage("Success", "StackLayout tapped")),
			});
		}
	}
}
