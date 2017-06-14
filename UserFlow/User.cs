using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace UserFlow
{
	public class User
	{
		readonly Page page;
		readonly Stack<AlertArguments> alerts = new Stack<AlertArguments>();

		public User(Application app)
		{
			page = app.MainPage;

			MessagingCenter.Subscribe<Page, AlertArguments>(this, Page.AlertSignalName, (page, alert) => {
				alerts.Push(alert);
			});

			if (page is NavigationPage)
				throw new NotImplementedException("MainPage of type NavigationPage is currently not supported");

			if (page is MasterDetailPage) {
				var masterDetailPage = page as MasterDetailPage;
				masterDetailPage.PropertyChanging += (sender, e) => {
					if (e.PropertyName == nameof(MasterDetailPage.Detail))
						(masterDetailPage.Detail as IPageController).SendDisappearing();
				};
				masterDetailPage.PropertyChanged += (sender, e) => {
					if (e.PropertyName == nameof(MasterDetailPage.Detail))
						(masterDetailPage.Detail as IPageController).SendAppearing();
				};
				var detail = masterDetailPage.Detail;
				(detail as IPageController).SendAppearing();
				if (detail is NavigationPage) {
					(detail as NavigationPage).Pushed += (sender, e) => {
						var stack = CurrentPage.Navigation.NavigationStack;
						(stack[stack.Count - 2]).SendDisappearing();
						(e.Page as IPageController).SendAppearing();
					};
					(detail as NavigationPage).Popped += (sender, e) => {
						(e.Page as IPageController).SendDisappearing();
						(CurrentPage as IPageController).SendAppearing();
					};
					(detail as NavigationPage).PoppedToRoot += (sender, e) => {
						((e as PoppedToRootEventArgs).PoppedPages.Last() as IPageController).SendDisappearing();
					};
				}
			}
		}

		ContentPage CurrentPage {
			get {
				if ((page as MasterDetailPage)?.IsPresented ?? false)
					return (page as MasterDetailPage).Master as ContentPage;
				var rootPage = (page as MasterDetailPage)?.Detail ?? page;
				return rootPage.Navigation.NavigationStack.Concat(page.Navigation.ModalStack).Last() as ContentPage;
			}
		}

		public bool CanSee(string text)
		{
			if (alerts.Any()) {
				var alert = alerts.Peek();
				return alert.Title == text || alert.Message == text;
			}

			return CurrentPage.Find(text).Any();
		}

		public void Tap(string text)
		{
			if (alerts.Any()) {
				var alert = alerts.Peek();
				if (alert.Accept == text)
					alert.SetResult(true);
				else if (alert.Cancel == text)
					alert.SetResult(false);
				else
					Assert.Fail($"Could not tap \"{text}\" on alert\n{alert}");

				alerts.Pop();
				return;
			}

			var elementInfos = CurrentPage.Find(text);
			Assert.That(elementInfos, Is.Not.Empty, $"Did not find \"{text}\" on current page");
			Assert.That(elementInfos, Has.Count.LessThan(2), $"Found multiple \"{text}\" on current page");

			var elementInfo = elementInfos.First();

			(elementInfo.Element as ToolbarItem)?.Command.Execute(null);
			(elementInfo.Element as Button)?.Command.Execute(null);
			elementInfo.EnclosingListView?.Invoke("NotifyRowTapped", elementInfo.ListViewIndex, null);
			elementInfo.InvokeTapGestures?.Invoke();
		}

		public void Input(string automationId, string text)
		{
			var elements = CurrentPage.Find(automationId).Select(i => i.Element).OfType<Entry>().ToList();
			Assert.That(elements, Is.Not.Empty, $"Did not find entry \"{automationId}\" on current page");
			Assert.That(elements, Has.Count.LessThan(2), $"Found multiple entries \"{automationId}\" on current page");

			elements.First().Text = text;
		}

		public void OpenMenu()
		{
			(page as MasterDetailPage).IsPresented = true;
		}

		public void GoBack()
		{
			page.SendBackButtonPressed();
		}

		public void Print()
		{
			Console.WriteLine(Render());
		}

		public string Render()
		{
			if (alerts.Any())
				return alerts.Peek().Render();
			else
				return CurrentPage.Render().Trim();
		}
	}
}
