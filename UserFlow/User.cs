using System;
using System.Linq;
using NUnit.Framework;
using Xamarin.Forms;

namespace UserFlow
{
	public class User
	{
		readonly Page page;

		public User(Application app)
		{
			page = app.MainPage;

			MessagingCenter.Subscribe<Page, Xamarin.Forms.Internals.AlertArguments>(this, Page.AlertSignalName, (obj, obj1) => {
				Console.WriteLine("Showing Dialog");
			});

			if (page is NavigationPage) {
				(page as NavigationPage).Pushed += (sender, e) => (page as IPageController).SendAppearing();
				(page as NavigationPage).Popped += (sender, e) => (page as IPageController).SendDisappearing();
			}
		}

		ContentPage CurrentPage {
			get {
				return page.Navigation.NavigationStack.Concat(page.Navigation.ModalStack).Last() as ContentPage;
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
