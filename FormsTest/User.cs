using System;
using System.Linq;
using NUnit.Framework;
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
			var elementInfos = CurrentPage.Find(text);
			Assert.That(elementInfos, Is.Not.Empty, $"Did not find \"{text}\" on current page");
			Assert.That(elementInfos, Has.Count.LessThan(2), $"Found multiple \"{text}\" on current page");

			var elementInfo = elementInfos.First();

			(elementInfo.Element as ToolbarItem)?.Command.Execute(null);
			(elementInfo.Element as Button)?.Command.Execute(null);
			elementInfo.EnclosingListView?.Invoke("NotifyRowTapped", elementInfo.ListViewIndex, null);
			elementInfo.InvokeTapGestures?.Invoke();
		}

		public void GoBack()
		{
			page.SendBackButtonPressed();
		}

		public void PrintCurrentPage()
		{
			Console.WriteLine(CurrentPage.Render().Trim());
		}
	}
}
