using Xamarin.Forms;

namespace DemoApp
{
	public class DemoPage : ContentPage
	{
		public DemoPage()
		{
			Title = "Demo page";

			Content = new ScrollView {
				Content = new StackLayout {
					VerticalOptions = LayoutOptions.CenterAndExpand,
					Children = {
						new DemoLabel(),
						new DemoButton(),
						new DemoStack(),
						new DemoListViewWithTextCell(),
						new DemoListViewWithViewCell(),
						new DemoGrid(),
					},
				},
			};

			ToolbarItems.Add(new DemoToolbarItem());
		}
	}
}
