using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace FormsTest
{
	public class User
	{
		readonly Page page;

		public User(Application app)
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

		public bool CanSee(string text)
		{
			return CurrentPage.Find(text).Any();
		}

		public void Tap(string text)
		{
			var element = CurrentPage.Find(text).FirstOrDefault();

			if (element == null)
				return;

			(element as Button)?.Tap();

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

			(element as ToolbarItem)?.Tap();

			element.GetNearestAncestorWithTapGestureRecognizer()?.Tap();
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
