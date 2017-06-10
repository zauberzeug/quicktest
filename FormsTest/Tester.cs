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

		public readonly ResultPage ResultPage = new ResultPage();

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

		protected abstract void RunTest();

		public void TryRunTest()
		{
			try {
				RunTest();
				ResultPage.LogPage(CurrentPage);
				ResultPage.LogInfo("Test succeeded");
			} catch (TestException e) {
				ResultPage.LogPage(CurrentPage);
				ResultPage.LogError(e.Message);
			}
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
			if (!Query(text).Any())
				throw new NotFoundException(text);
		}

		public void Click(string text)
		{
			var view = Query(text).FirstOrDefault();

			if (view == null)
				throw new NotFoundException(text);

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

		public void GoBack()
		{
			if (Page is NavigationPage || Page is MasterDetailPage) {
				var flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
				var method = typeof(ContentPage).GetMethod("SendBackButtonPressed", flags);
				method.Invoke(Page, new object[] { });
			} else
				throw new NotImplementedException($"Currently \"{nameof(GoBack)}()\" is supported for {nameof(NavigationPage)}s and {nameof(MasterDetailPage)}s only.");
		}
	}
}
