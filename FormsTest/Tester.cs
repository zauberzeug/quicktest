using System.Linq;
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

		public void ShouldSee(string text)
		{
		}

		public void Click(string text)
		{
		}
	}
}
