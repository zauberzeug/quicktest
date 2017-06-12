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

		IEnumerable<Element> Query(string text, Element element = null)
		{
			element = element ?? CurrentPage;

			if (element is ContentPage)
				return Query(text, (element as ContentPage).Content).Concat(
					(element as Page).ToolbarItems.Where(t => t.Text == text));

			if (element is ScrollView)
				return Query(text, (element as ScrollView).Content);

			if (element is Layout<View>)
				return (element as Layout<View>).Children.SelectMany(child => Query(text, child)).ToList();

			if ((element as Button)?.Text == text)
				return new List<Element> { element };

			if ((element as Label)?.Text == text)
				return new List<Element> { element };

			return new List<Element>();
		}

		public void ShouldSee(string text)
		{
			if (!Query(text).Any())
				throw new NotFoundException(text);
		}

		public void Click(string text)
		{
			var element = Query(text).FirstOrDefault();

			if (element == null)
				throw new NotFoundException(text);

			if (element is Button) {
				var flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
				var method = typeof(Button).GetMethods(flags).First(m => m.Name.EndsWith("SendClicked", StringComparison.Ordinal));
				method.Invoke(element, new object[] { });
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
			if (Page is NavigationPage || Page is MasterDetailPage) {
				var flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
				var method = typeof(ContentPage).GetMethod("SendBackButtonPressed", flags);
				method.Invoke(Page, new object[] { });
			} else
				throw new NotImplementedException($"Currently \"{nameof(GoBack)}()\" is supported for {nameof(NavigationPage)}s and {nameof(MasterDetailPage)}s only.");
		}
	}
}
