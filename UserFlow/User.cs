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
        readonly Application app;
        readonly Stack<AlertArguments> alerts = new Stack<AlertArguments>();

        public User(Application app)
        {
            this.app = app;

            MessagingCenter.Subscribe<Page, AlertArguments>(this, Page.AlertSignalName, (page, alert) => {
                alerts.Push(alert);
            });

            app.PropertyChanging += (s, args) => {
                if (args.PropertyName == nameof(Application.MainPage))
                    (CurrentPage as IPageController).SendDisappearing();
            };
            app.PropertyChanged += (s, args) => {
                if (args.PropertyName == nameof(Application.MainPage))
                    HandleDisAppearing();
            };

            HandleDisAppearing();
        }

        public NavigationPage CurrentNavigationPage {
            get {
                var navigationPage = (app.MainPage as NavigationPage) ?? ((app.MainPage as MasterDetailPage)?.Detail as NavigationPage);
                if (navigationPage == null)
                    Assert.Fail("We must have a NavigationPage");

                return navigationPage;
            }
        }

        ContentPage CurrentPage {
            get {
                var badPage = new ContentPage {
                    Title = "Error",
                    Content = new Label {
                        Text = "The expected page is not of type \"ContentPage\"",
                    },
                };

                var modalStack = app.MainPage.Navigation.ModalStack;
                if (modalStack.Any())
                    return modalStack.Last() as ContentPage
                                     ?? (modalStack.Last() as NavigationPage).CurrentPage as ContentPage
                                     ?? badPage;

                if ((app.MainPage as MasterDetailPage)?.IsPresented ?? false)
                    return (app.MainPage as MasterDetailPage).Master as ContentPage ?? badPage;

                var rootPage = (app.MainPage as MasterDetailPage)?.Detail ?? app.MainPage;
                return rootPage.Navigation.NavigationStack.Last() as ContentPage ?? badPage;
            }
        }

        public bool CanSee(string text)
        {
            if (alerts.Any()) {
                var alert = alerts.Peek();
                return alert.Title == text
                    || alert.Message == text
                    || alert.Cancel == text
                    || alert.Accept == text;
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
            elementInfo.InvokeTap?.Invoke();
        }

        public void Input(string automationId, string text)
        {
            var elements = CurrentPage.Find(automationId).Select(i => i.Element).OfType<VisualElement>().ToList();
            Assert.That(elements, Is.Not.Empty, $"Did not find entry \"{automationId}\" on current page");
            Assert.That(elements, Has.Count.LessThan(2), $"Found multiple entries \"{automationId}\" on current page");

            if (elements.First() is Entry)
                (elements.First() as Entry).Text = text;
            if (elements.First() is Editor)
                (elements.First() as Editor).Text = text;
            if (elements.First() is SearchBar)
                (elements.First() as SearchBar).Text = text;
        }

        public void OpenMenu()
        {
            (app.MainPage as MasterDetailPage).IsPresented = true;
        }

        public void GoBack()
        {
            app.MainPage.SendBackButtonPressed();
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

        void HandleDisAppearing()
        {
            (CurrentPage as IPageController).SendAppearing();

            if (app.MainPage is MasterDetailPage) {
                var masterDetailPage = app.MainPage as MasterDetailPage;
                masterDetailPage.PropertyChanging += (sender, e) => {
                    if (e.PropertyName == nameof(MasterDetailPage.Detail)) {
                        var page = masterDetailPage.Detail.Navigation.NavigationStack.Last();
                        Console.WriteLine("disappearing: " + page.Title);
                        page.SendDisappearing();
                    }
                };
                masterDetailPage.PropertyChanged += (sender, e) => {
                    if (e.PropertyName == nameof(MasterDetailPage.Detail)) {
                        var page = masterDetailPage.Detail.Navigation.NavigationStack.Last();
                        Console.WriteLine("appearing: " + page.Title);
                        page.SendAppearing();
                    }
                };
            }

            CurrentNavigationPage.Pushed += (sender, e) => {
                var stack = CurrentPage.Navigation.NavigationStack;
                (stack[stack.Count - 2]).SendDisappearing();
                (e.Page as IPageController).SendAppearing();
            };
            CurrentNavigationPage.Popped += (sender, e) => {
                (e.Page as IPageController).SendDisappearing();
                (CurrentPage as IPageController).SendAppearing();
            };
            CurrentNavigationPage.PoppedToRoot += (sender, e) => {
                ((e as PoppedToRootEventArgs).PoppedPages.Last() as IPageController).SendDisappearing();
            };
        }
    }
}
