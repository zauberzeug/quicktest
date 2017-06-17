using Xamarin.Forms;

namespace DemoApp
{
	public class ElementDemoPage : ContentPage
	{
		public ElementDemoPage()
		{
			Title = "Element demo";

			var bindableText = new BindableText("bound text");
			Content = new ScrollView {
				Content = new StackLayout {
					Children = {
						new DemoButton("Button"),
						new DemoLabel("Label").WithGestureRecognizer(),
						new DemoStack(),
						new DemoGrid(),
						new DemoEntry("entry_automation_id", "Placeholder"),
						new DemoEditor("editor_automation_id", "editor content"),
						new DemoLabel("Invisible Label").Invisible(),
						new DemoLabel().BindTo(bindableText, BindableText.TextProperty, Label.TextProperty),
						new DemoCountdown(),
		},
				},
			};

			ToolbarItems.Add(new DemoToolbarItem());
			bindableText.Text = "bound text 2";
		}
	}

	public class BindableText : BindableObject
	{
		public static readonly BindableProperty TextProperty = BindableProperty.Create("Text", typeof(string), typeof(string), "");

		public BindableText(string text)
		{
			Text = text;
		}

		public string Text {
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}
	}
}
