using System;
using Xamarin.Forms;

namespace FormsTest
{
	public class ResultPage : ContentPage
	{
		readonly StackLayout stackLayout = new StackLayout {
			Padding = new Thickness(0, 5),
		};

		public ResultPage()
		{
			Title = "Test result";

			Content = new ScrollView {
				Content = stackLayout,
			};
		}

		public void LogInfo(string message)
		{
			Log(message, Color.Blue);
			Console.WriteLine("Info: " + message);
		}

		public void LogError(string message)
		{
			Log(message, Color.Red);
			Console.WriteLine("Error: " + message);
		}

		public void LogPage(ContentPage page)
		{
			var text = ToString(page.Content);
			stackLayout.Children.Clear();
			stackLayout.Children.Add(new Label {
				Text = text,
				FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
				LineBreakMode = LineBreakMode.WordWrap,
				MinimumWidthRequest = 1e6,
				Margin = new Thickness(10, 0),
			});
			Console.WriteLine(text);
		}

		void Log(string message, Color color)
		{
			stackLayout.Children.Add(new Label {
				Text = message,
				TextColor = color,
				Margin = new Thickness(10, 0),
			});
		}

		static string ToString(View view)
		{
			if (view is ScrollView)
				return ToString((view as ScrollView).Content);

			if (view is Layout<View>) {
				var tree = "";
				foreach (var child in (view as Layout<View>).Children)
					tree += ToString(child).Replace("\n", "\n    ") + "\n";
				return tree.Trim();
			}

			if (view is ListView) {
				var tree = "";
				var listView = view as ListView;
				foreach (var item in listView.ItemsSource) {
					var itemView = (listView.ItemTemplate.CreateContent() as ViewCell).View as View;
					itemView.BindingContext = item;
					tree += ToString(itemView).Replace("\n", "\n    ") + "\n";
				}
				return tree.Trim();
			}

			if (view is Label)
				return (view as Label).Text ?? "(null)";

			if (view is Button)
				return (view as Button).Text ?? "(null)";

			if (view is SearchBar)
				return (view as SearchBar).Placeholder;

			return "";
		}
	}
}
