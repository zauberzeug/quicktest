using Xamarin.Forms;

namespace FormsTest
{
	public class DemoPage : ContentPage
	{
		public DemoPage()
		{
			Content = new StackLayout {
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Children = {
					new Label {
						Text = "Label",
					},
					new Button {
						Text = "Button",
					},
				},
			};
		}
	}
}
