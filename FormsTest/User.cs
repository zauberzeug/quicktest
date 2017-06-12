using System;
using System.Linq;
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
			var elementInfo = CurrentPage.Find(text).FirstOrDefault();

			(elementInfo.Element as ToolbarItem)?.Command.Execute(null);
			(elementInfo.Element as Button)?.Invoke("SendClicked");
			elementInfo.EnclosingListView?.Invoke("NotifyRowTapped", elementInfo.ListViewIndex, null);
			elementInfo.InvokeTapGestures?.Invoke();
		}

		public void GoBack()
		{
			if (page is NavigationPage || page is MasterDetailPage)
				page.Invoke("SendBackButtonPressed");
			else
				throw new NotImplementedException($"Currently \"{nameof(GoBack)}()\" is supported for {nameof(NavigationPage)}s and {nameof(MasterDetailPage)}s only.");
		}

		public void PrintCurrentPage()
		{
			Console.WriteLine(CurrentPage.Render());
		}
	}
}
