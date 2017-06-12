using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace FormsTest
{
	public class ElementInfo
	{
		public Element Element;
		public Action InvokeTapGestures;
		public ListView EnclosingListView;
		public int ListViewIndex;

		public static ElementInfo FromElement(Element element)
		{
			return new ElementInfo {
				Element = element,
			};
		}
	}

	public static class ElementExtensions
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

			if (element is ListView)
				foreach (var item in (element as ListView)?.ItemsSource) {
					var content = (element as ListView).ItemTemplate.CreateContent();
					if (content is TextCell) {
						(content as TextCell).BindingContext = item;
						if ((content as TextCell).Text == text)
							result.Add(new ElementInfo {
								EnclosingListView = element as ListView,
								ListViewIndex = (element as ListView)?.ItemsSource.Cast<object>().ToList().IndexOf(item) ?? -1,
							});
					} else
						throw new NotImplementedException($"Currently \"{content.GetType()}\" is not supported.");
				}

			if ((element as Button)?.Text == text)
				result.Add(ElementInfo.FromElement(element));

			if ((element as Label)?.Text == text)
				result.Add(ElementInfo.FromElement(element));

			var newTapGestureRecognizers = (element as View)?.GestureRecognizers.OfType<TapGestureRecognizer>().ToList();
			if (newTapGestureRecognizers?.Any() ?? false)
				foreach (var info in result.Where(i => i.InvokeTapGestures == null))
					info.InvokeTapGestures = () => newTapGestureRecognizers.ForEach(r => {
						var flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
						var method = r.GetType().GetMethod("SendTapped", flags);
						method.Invoke(r, new object[] { element
	});
					});

			return result;
		}

		public static void Tap(this Button button)
		{
			var flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
			var method = typeof(Button).GetMethods(flags).First(m => m.Name.EndsWith("SendClicked", StringComparison.Ordinal));
			method.Invoke(button, new object[] { });
		}

		public static void Tap(this ToolbarItem toolbarItem)
		{
			toolbarItem.Command.Execute(null);
		}

		public static void Tap(this ListView listView, int index)
		{
			var flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
			var method = listView.GetType().GetMethods(flags).First(m => m.Name == "NotifyRowTapped" && m.GetParameters().Length == 2);
			method.Invoke(listView, new object[] { index, null });
		}
	}
}
