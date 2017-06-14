using Xamarin.Forms;

namespace DemoApp
{
	public class DemoLabel : Label
	{
		public DemoLabel(string text = null)
		{
			Text = text;

			BackgroundColor = Color.FloralWhite;
			HorizontalTextAlignment = TextAlignment.Center;
		}

		public DemoLabel WithGestureRecognizer()
		{
			GestureRecognizers.Add(new TapGestureRecognizer {
				Command = new Command(o => App.ShowMessage("Success", Text + " tapped")),
			});

			return this;
		}
	}
}
