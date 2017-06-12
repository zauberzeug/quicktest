using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace FormsTest
{
	public static class ElementExtensions
	{
		public static List<Element> Find(this Element element, string text)
		{
			if (text == null)
				throw new InvalidOperationException("Can't search for (null) text");

			var result = new List<Element>();
			IEnumerable<Element> empty = new List<Element>();

			result.AddRange((element as Page)?.ToolbarItems.Where(t => t.Text == text) ?? empty);

			if ((element as ContentPage)?.Title == text)
				result.Add(element);

			result.AddRange((element as ContentPage)?.Content.Find(text) ?? empty);

			result.AddRange((element as ScrollView)?.Content.Find(text) ?? empty);

			result.AddRange((element as Layout<View>)?.Children.SelectMany(child => child.Find(text)) ?? empty);

			result.AddRange((element as ListView)?.ItemsSource.Cast<object>().SelectMany(item => {
				var content = (element as ListView).ItemTemplate.CreateContent();
				if (content is TextCell) {
					(content as TextCell).BindingContext = item;
					(content as TextCell).Parent = element;
					return (content as TextCell).Find(text);
				}
				throw new NotImplementedException($"Currently \"{content.GetType()}\" is not supported.");
			}) ?? empty);

			if ((element as Button)?.Text == text)
				result.Add(element);

			if ((element as Label)?.Text == text)
				result.Add(element);

			return result;
		}

		public static View GetNearestAncestorWithTapGestureRecognizer(this Element element)
		{
			while (element.Parent != null) {
				if ((element as View)?.GestureRecognizers.OfType<TapGestureRecognizer>().Any() ?? false)
					return element as View;
				element = element.Parent;
			}
			return null;
		}

		public static void Tap(this View view)
		{
			var gestureRecognizers = view.GestureRecognizers.OfType<TapGestureRecognizer>().ToList();
			gestureRecognizers.ForEach(gestureRecognizer => {
				var flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
				var method = gestureRecognizer.GetType().GetMethod("SendTapped", flags);
				method.Invoke(gestureRecognizer, new object[] { view });
			});
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
	}
}
