using System.Collections.Generic;
using Xamarin.Forms;

namespace DemoApp
{
	public class DemoListViewWithTextCell : ListView
	{
		public DemoListViewWithTextCell()
		{
			ItemsSource = new List<string> { "A1", "B1", "C1" };
			ItemTemplate = new DataTemplate(typeof(TextCell));

			BackgroundColor = Color.GhostWhite;

			ItemTemplate.SetBinding(TextCell.TextProperty, ".");
			ItemTapped += (sender, e) => App.PushMessagePage(e.Item + " tapped");
		}
	}

	public class DemoListViewWithViewCell : ListView
	{
		public DemoListViewWithViewCell()
		{
			ItemsSource = new List<string> { "A2", "B2", "C2" };
			ItemTemplate = new DataTemplate(typeof(DemoCell));

			BackgroundColor = Color.GhostWhite;

			ItemTapped += (sender, e) => App.PushMessagePage(e.Item + " tapped");
		}
	}

	public class DemoCell : ViewCell
	{
		public DemoCell()
		{
			var label = new Label {
				BackgroundColor = Color.FloralWhite,
			};
			label.SetBinding(Label.TextProperty, ".");
			View = new StackLayout {
				Children = { label },
				BackgroundColor = Color.Gray.MultiplyAlpha(0.2),
			};
		}
	}
}
