using System;
using System.Linq;
using Xamarin.Forms;

namespace FormsTest
{
	public static class ElementRendering
	{
		public static string Render(this Element element)
		{
			var result = "· ";

			result += (element as NavigationPage)?.CurrentPage.Render();
			if (element is ContentPage)
				result += (element as ContentPage).Title + " " + string.Join(" ", (element as Page).ToolbarItems.Select(t => $"[{t.Text}]"));
			result += (element as ContentPage)?.Content.Render();

			result += (element as ScrollView)?.Content.Render();
			result += string.Join("", (element as Layout<View>)?.Children.Select(c => c.Render()) ?? new[] { "" });
			result += (element as ListView)?.Render();

			result += (element as Label)?.Text;
			result += (element as Button)?.Text;

			result = "\n" + result.Replace("\n", "\n  ");

			return result;
		}

		public static string Render(this ListView listView)
		{
			var result = "\n";

			foreach (var item in listView.ItemsSource) {
				var content = listView.ItemTemplate.CreateContent();
				if (content is TextCell) {
					(content as TextCell).BindingContext = item;
					result += $"- {(content as TextCell).Text}\n";
				} else if (content is ViewCell) {
					(content as ViewCell).BindingContext = item;
					result += (content as ViewCell).View.Render();
				} else
					throw new NotImplementedException($"Currently \"{content.GetType()}\" is not supported.");
			}
			return result;
		}
	}
}
