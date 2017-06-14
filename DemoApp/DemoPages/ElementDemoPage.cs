using Xamarin.Forms;

namespace DemoApp
{
	public class ElementDemoPage : ContentPage
	{
		public ElementDemoPage()
		{
			Title = "Element demo";

			Content = new ScrollView {
				Content = new StackLayout {
					Children = {
						new DemoButton("Button"),
						new DemoLabel("Label").WithGestureRecognizer(),
						new DemoStack(),
						new DemoGrid(),
						new DemoEntry("entry_automation_id"),
					},
				},
			};

			ToolbarItems.Add(new DemoToolbarItem());
		}
	}
}
