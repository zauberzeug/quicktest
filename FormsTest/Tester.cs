using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace FormsTest
{
	public abstract class Tester
	{
		public Page Page;

		public readonly ContentPage ResultPage = new ContentPage {
			Title = nameof(Tester),
		};

		ContentPage CurrentPage {
			get {
				if (Page is ContentPage)
					return Page as ContentPage;
				if (Page is NavigationPage)
					return (Page as NavigationPage).CurrentPage as ContentPage;
				if (Page is MasterDetailPage) {
					var masterDetailPage = Page as MasterDetailPage;
					if (masterDetailPage.IsPresented)
						return masterDetailPage.Master as ContentPage;
					if (masterDetailPage.Navigation.ModalStack.Any())
						return masterDetailPage.Navigation.ModalStack.Last() as ContentPage;
					return (masterDetailPage.Detail as NavigationPage).CurrentPage as ContentPage;
				}
				return null;
			}
		}

		public abstract void RunTest();

		public void LogPage()
		{
			ResultPage.Content = new Label {
				Text = ToString(CurrentPage.Content),
				LineBreakMode = LineBreakMode.WordWrap,
				MinimumWidthRequest = 1e6,
				Margin = 8,
			};
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

		List<View> Query(string text, View view = null)
		{
			view = view ?? CurrentPage.Content;

			if (view is ScrollView)
				return Query(text, (view as ScrollView).Content);

			if (view is Layout<View>)
				return (view as Layout<View>).Children.SelectMany(child => Query(text, child)).ToList();

			if ((view as Button)?.Text == text)
				return new List<View> { view };

			if ((view as Label)?.Text == text)
				return new List<View> { view };

			return new List<View>();
		}

		public void ShouldSee(string text)
		{
			if (Query(text).Any())
				Console.WriteLine($"Seeing {text}");
			else
				Console.WriteLine($"Missing {text}!");
		}

		public void Click(string text)
		{
			var view = Query(text).First();

			if (view is Button) {
				var flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
				var method = typeof(Button).GetMethods(flags).First(m => m.Name.EndsWith("SendClicked", StringComparison.Ordinal));
				method.Invoke(view, new object[] { });
			}

			if (view is Label)
				view.GestureRecognizers.OfType<TapGestureRecognizer>().ToList().ForEach(gestureRecognizer => {
					var flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
					var method = gestureRecognizer.GetType().GetMethod("SendTapped", flags);
					method.Invoke(gestureRecognizer, new object[] { view });
				});
		}
	}
}
