﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace FormsTest
{
	public class Tester
	{
		readonly Page page;

		public Tester(Application app)
		{
			page = app.MainPage;
		}

		ContentPage CurrentPage {
			get {
				if (page is ContentPage)
					return page as ContentPage;
				if (page is NavigationPage)
					return (page as NavigationPage).CurrentPage as ContentPage;
				if (page is MasterDetailPage) {
					var masterDetailPage = page as MasterDetailPage;
					if (masterDetailPage.IsPresented)
						return masterDetailPage.Master as ContentPage;
					if (masterDetailPage.Navigation.ModalStack.Any())
						return masterDetailPage.Navigation.ModalStack.Last() as ContentPage;
					return (masterDetailPage.Detail as NavigationPage).CurrentPage as ContentPage;
				}
				return null;
			}
		}

		IEnumerable<Element> Query(string text, Element element = null)
		{
			element = element ?? CurrentPage;

			if (element is ContentPage) {
				if ((element as ContentPage).Title == text)
					return new List<Element> { element };
				return Query(text, (element as ContentPage).Content).Concat(
					(element as Page).ToolbarItems.Where(t => t.Text == text));
			}

			if (element is ScrollView)
				return Query(text, (element as ScrollView).Content);

			if (element is Layout<View>)
				return (element as Layout<View>).Children.SelectMany(child => Query(text, child)).ToList();

			if (element is ListView)
				foreach (var item in (element as ListView).ItemsSource) {
					var content = (element as ListView).ItemTemplate.CreateContent();
					if (content is TextCell) {
						if ((item as string) == text)
							return new List<Element> { element };
					} else
						throw new NotImplementedException($"Currently \"{content.GetType()}\" is not supported.");
				}

			if ((element as Button)?.Text == text)
				return new List<Element> { element };

			if ((element as Label)?.Text == text)
				return new List<Element> { element };

			return new List<Element>();
		}

		public bool Contains(string text)
		{
			return Query(text).Any();
		}

		public void Click(string text)
		{
			var element = Query(text).FirstOrDefault();

			if (element == null)
				return;

			if (element is Button) {
				var flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
				var method = typeof(Button).GetMethods(flags).First(m => m.Name.EndsWith("SendClicked", StringComparison.Ordinal));
				method.Invoke(element, new object[] { });
			}

			if (element is ListView) {
				var listView = element as ListView;
				var index = 0;
				foreach (var item in listView.ItemsSource) {
					var content = listView.ItemTemplate.CreateContent();
					if (content is TextCell) {
						if ((item as string) == text) {
							var flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
							var method = listView.GetType().GetMethods(flags).First(m => m.Name == "NotifyRowTapped" && m.GetParameters().Length == 2);
							method.Invoke(listView, new object[] { index, null });
						}
					} else
						throw new NotImplementedException($"Currently \"{content.GetType()}\" is not supported.");
					index++;
				}
			}

			if (element is ToolbarItem)
				(element as ToolbarItem).Command.Execute(null);

			while (element is View) {
				var view = element as View;
				var gestureRecognizers = view.GestureRecognizers.OfType<TapGestureRecognizer>().ToList();
				gestureRecognizers.ForEach(gestureRecognizer => {
					var flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
					var method = gestureRecognizer.GetType().GetMethod("SendTapped", flags);
					method.Invoke(gestureRecognizer, new object[] { view });
				});
				if (gestureRecognizers.Any())
					break;
				element = element.Parent;
			}
		}

		public void GoBack()
		{
			if (page is NavigationPage || page is MasterDetailPage) {
				var flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
				var method = typeof(ContentPage).GetMethod("SendBackButtonPressed", flags);
				method.Invoke(page, new object[] { });
			} else
				throw new NotImplementedException($"Currently \"{nameof(GoBack)}()\" is supported for {nameof(NavigationPage)}s and {nameof(MasterDetailPage)}s only.");
		}

		public void PrintCurrentPage()
		{
			Console.WriteLine(ToString(CurrentPage.Content));
		}

		static string ToString(View view)
		{
			if (view is ScrollView)
				return ToString((view as ScrollView).Content);

			if (view is Layout<View>) {
				var tree = "Layout:\n";
				foreach (var child in (view as Layout<View>).Children)
					tree += ToString(child).Replace("\n", "\n    ") + "\n";
				return tree.Trim();
			}

			if (view is ListView) {
				var tree = "ListView:\n";
				var listView = view as ListView;
				foreach (var item in listView.ItemsSource) {
					var content = listView.ItemTemplate.CreateContent();
					if (content is TextCell)
						tree += $"\"{item}\"\n";
					else
						throw new NotImplementedException($"Currently \"{content.GetType()}\" is not supported.");
				}
				return tree.Trim();
			}

			if (view is Label)
				return $"Label: \"{(view as Label).Text ?? "(null)"}\"";

			if (view is Button)
				return $"Button: \"{(view as Button).Text ?? "(null)"}\"";

			if (view is SearchBar)
				return $"SearchBar: \"{(view as SearchBar).Placeholder}\"";

			return "";
		}
	}
}
