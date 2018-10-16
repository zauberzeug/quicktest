using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace QuickTest
{
    public partial class User
    {
        readonly Application app;
        readonly Stack<AlertArguments> alerts = new Stack<AlertArguments>();

        public User(Application app)
        {
            this.app = app;

            MessagingCenter.Subscribe<Page, AlertArguments>(this, Page.AlertSignalName, (page, alert) => {
                alerts.Push(alert);
            });

            WireNavigation();
        }

        public NavigationPage CurrentNavigationPage {
            get {
                if (app.MainPage.Navigation.ModalStack.Any())
                    return (app.MainPage.Navigation.ModalStack.LastOrDefault() as NavigationPage);
                return (app.MainPage as NavigationPage)
                ?? (app.MainPage as MasterDetailPage).Detail as NavigationPage;
            }
        }

        public Page CurrentPage {
            get {
                var modalStack = app.MainPage.Navigation.ModalStack;
                var currentPage = (modalStack.LastOrDefault() as ContentPage)
                    ?? ((modalStack.LastOrDefault() as NavigationPage)?.CurrentPage as ContentPage);

                var masterDetailPage = app.MainPage as MasterDetailPage;
                if (currentPage == null && masterDetailPage != null && masterDetailPage.IsPresented)
                    currentPage = masterDetailPage.Master as ContentPage;

                var rootPage = masterDetailPage?.Detail ?? app.MainPage;
                if (currentPage == null) {
                    var page = rootPage.Navigation.NavigationStack.Last();
                    if (page is TabbedPage)
                        return (page as TabbedPage).CurrentPage as ContentPage;
                    else
                        currentPage = page as ContentPage;
                }

                if (currentPage == null)
                    Assert.Fail("CurrentPage is no ContentPage");

                return currentPage;
            }
        }

        public bool CanSee(string text)
        {
            if (CanSeeAlertWith(text))
                return true;

            return CurrentPage.Find(text).Any();
        }

        public bool CanSeeOnce(string text)
        {
            if (CanSeeAlertWith(text))
                return true;

            return CurrentPage.Find(text).Count == 1;
        }

        public bool CanSeeAlertWith(string text)
        {
            if (alerts.Any()) {
                var alert = alerts.Peek();
                return alert.Title == text
                    || alert.Message == text
                    || alert.Cancel == text
                    || alert.Accept == text;
            }
            return false;
        }

        public bool SeesAlert() => alerts.Any();

        public List<Element> Find(string text)
        {
            return CurrentPage.Find(text).Select(i => i.Element).ToList();
        }

        public List<Element> Find(Predicate<Element> predicate, Predicate<Element> containerPredicate = null)
        {
            return CurrentPage.Find(predicate, containerPredicate).Select(i => i.Element).ToList();
        }

        public void Tap(string text, int? index = null)
        {
            if (alerts.Any()) {
                Assert.That(index, Is.Null, "Tap indices are not supported on alerts");

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

            ElementInfo elementInfo;
            if (index == null) {
                Assert.That(elementInfos, Has.Count.LessThan(2), $"Found multiple \"{text}\" on current page");
                elementInfo = elementInfos.First();
            } else {
                Assert.That(elementInfos, Has.Count.GreaterThan(index), $"Did not find enough \"{text}\" on current page");
                elementInfo = elementInfos.Skip(index.Value).First();
            }

            if (elementInfo.Element is ToolbarItem)
                (elementInfo.Element as ToolbarItem).Command.Execute(null);
            else if (elementInfo.Element is Button)
                (elementInfo.Element as Button).Command.Execute(null);
            else if (elementInfo.InvokeTap != null)
                elementInfo.InvokeTap.Invoke();
            else if (elementInfo.Element.Parent is TabbedPage) {
                var tabbedPage = (elementInfo.Element.Parent as TabbedPage);
                tabbedPage.CurrentPage = tabbedPage.Children.FirstOrDefault(p => p.Title == text);
            } else
                throw new InvalidOperationException($"element with text '{text}' is not tappable");
        }

        public void Input(string automationId, string text)
        {
            var elements = CurrentPage.Find(automationId).Select(i => i.Element).OfType<VisualElement>().ToList();

            Assert.That(elements, Is.Not.Empty, $"Did not find entry \"{automationId}\" on current page");
            Assert.That(elements, Has.Count.LessThan(2), $"Found multiple entries \"{automationId}\" on current page");
            elements.First().SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, true);
            if (elements.First() is Entry) {
                (elements.First() as Entry).Text = text;
                (elements.First() as Entry).SendCompleted();
            } else if (elements.First() is Editor) {
                (elements.First() as Editor).Text = text;
                (elements.First() as Editor).SendCompleted();
            } else if (elements.First() is SearchBar)
                (elements.First() as SearchBar).Text = text;
            else if (elements.First() is Slider)
                (elements.First() as Slider).Value = double.Parse(text);
            else
                throw new InvalidOperationException($"element '{automationId}' can not be used for input");

            elements.First().SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
        }

        public void Input(string automationId, int value)
        {
            Input(automationId, value.ToString());
        }

        public void Cancel(string automationId)
        {
            var elements = CurrentPage.Find(automationId).Select(i => i.Element).OfType<VisualElement>().ToList();

            Assert.That(elements, Is.Not.Empty, $"Did not find entry \"{automationId}\" on current page");
            Assert.That(elements, Has.Count.LessThan(2), $"Found multiple entries \"{automationId}\" on current page");
            elements.First().SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, true);

            if (elements.First() is SearchBar)
                (elements.First() as SearchBar).Text = null;
            else
                throw new InvalidOperationException($"element '{automationId}' can not be used for input");

            elements.First().SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
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

            return CurrentPage.Render().Trim();
        }
    }
}
