using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace FormsTest
{
	public static class ElementInteractions
	{
		public static List<ElementInfo> Find(this Element element, string text)
		{
			if (text == null)
				throw new InvalidOperationException("Can't search for (null) text");

			var result = new List<ElementInfo>();
			IEnumerable<ElementInfo> empty = new List<ElementInfo>();

			result.AddRange((element as Page)?.ToolbarItems.Where(t => t.Text == text).Select(ElementInfo.FromElement) ?? empty);

			if ((element as ContentPage)?.Title == text)
				result.Add(ElementInfo.FromElement(element));

			result.AddRange((element as ContentPage)?.Content.Find(text) ?? empty);

			result.AddRange((element as ScrollView)?.Content.Find(text) ?? empty);

			result.AddRange((element as Layout<View>)?.Children.SelectMany(child => child.Find(text)) ?? empty);

			result.AddRange((element as ListView)?.Find(text) ?? empty);

			if ((element as Button)?.Text == text)
				result.Add(ElementInfo.FromElement(element));

			if ((element as Label)?.Text == text)
				result.Add(ElementInfo.FromElement(element));

			AddTapGestureRecognizers(element, result);

			return result;
		}

		static void AddTapGestureRecognizers(Element sourceElement, IEnumerable<ElementInfo> result)
		{
			var tapGestureRecognizers = (sourceElement as View)?.GestureRecognizers.OfType<TapGestureRecognizer>().ToList();

			if (tapGestureRecognizers == null || !tapGestureRecognizers.Any())
				return;

			foreach (var info in result.Where(i => i.InvokeTapGestures == null))
				info.InvokeTapGestures = () => tapGestureRecognizers.ForEach(r => r.Invoke("SendTapped", sourceElement));
		}

		public static List<ElementInfo> Find(this ListView listView, string text)
		{
			var result = new List<ElementInfo>();
			foreach (var item in listView.ItemsSource) {
				var content = listView.ItemTemplate.CreateContent();
				if (content is TextCell) {
					(content as TextCell).BindingContext = item;
					if ((content as TextCell).Text == text)
						result.Add(new ElementInfo {
							EnclosingListView = listView,
							ListViewIndex = listView.ItemsSource.Cast<object>().ToList().IndexOf(item),
						});
				} else if (content is ViewCell) {
					(content as ViewCell).BindingContext = item;
					if ((content as ViewCell).View.Find(text).Any())
						result.Add(new ElementInfo {
							EnclosingListView = listView,
							ListViewIndex = listView.ItemsSource.Cast<object>().ToList().IndexOf(item),
						});
				} else
					throw new NotImplementedException($"Currently \"{content.GetType()}\" is not supported.");
			}
			return result;
		}
	}
}
