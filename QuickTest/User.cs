using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace QuickTest
{
    public partial class User
    {
        readonly Application app;
        readonly List<Popup> popups = new List<Popup>();

        public User(Application app)
        {
            this.app = app;
            app.Invoke("OnStart");

            MessagingCenter.Subscribe<Page, AlertArguments>(this, Page.AlertSignalName, (page, alert) => {
                popups.Add(new AlertPopup(alert));
            });

            MessagingCenter.Subscribe<Page, ActionSheetArguments>(this, Page.ActionSheetSignalName, (page, actionSheet) => {
                popups.Add(new ActionSheetPopup(actionSheet));
            });

            WireNavigation();
        }

        public NavigationPage CurrentNavigationPage {
            get {
                var modalNavigationPage = app.MainPage.Navigation.ModalStack.LastOrDefault() as NavigationPage;
                if (modalNavigationPage != null)
                    return modalNavigationPage;
                return (app.MainPage as NavigationPage)
                ?? (app.MainPage as FlyoutPage).Detail as NavigationPage;
            }
        }

        public Page CurrentPage {
            get {
                var modalStack = app.MainPage.Navigation.ModalStack;
                var currentPage = (modalStack.LastOrDefault() as ContentPage)
                    ?? ((modalStack.LastOrDefault() as NavigationPage)?.CurrentPage as ContentPage);

                var flyoutPage = app.MainPage as FlyoutPage;
                if (currentPage == null && flyoutPage != null && flyoutPage.IsPresented)
                    currentPage = flyoutPage.Flyout as ContentPage;

                var rootPage = flyoutPage?.Detail ?? app.MainPage;
                if (currentPage == null) {
                    var page = rootPage.Navigation.NavigationStack.Last();
                    if (page is TabbedPage)
                        return (page as TabbedPage).CurrentPage as ContentPage;
                    else if (page is CarouselPage)
                        return (page as CarouselPage).CurrentPage;
                    else if (page is ContentPage)
                        return page;
                }

                if (currentPage == null)
                    Assert.Fail("TypeOf CurrentPage is supported yet");

                return currentPage;
            }
        }

        public bool CanSee(string text)
        {
            if (popups.Any())
                return popups.Last().Contains(text);
            else
                return CurrentPage.Find(text).Any();
        }

        public bool CanSee(string text, int count)
        {
            if (popups.Any())
                return popups.Last().Count(text) == count;
            else
                return CurrentPage.Find(text).Count == count;
        }

        public bool SeesPopup() => popups.Any();

        public bool SeesAlert() => popups.Any() && popups.Last() is AlertPopup;

        public bool SeesActionSheet() => popups.Any() && popups.Last() is ActionSheetPopup;

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
            if (popups.Any()) {
                Assert.That(index, Is.Null, "Tap indices are not supported on alerts");
                var popup = popups.Last();
                if (popup.Tap(text))
                    popups.Remove(popup);
                else
                    Assert.Fail($"Could not tap \"{text}\" on popup\n{popup}");
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
            var elements = FindElements(automationId);

            elements.First().SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, true);
            if (elements.First() is Entry) {
                var maxTextLength = Math.Min(text.Length, (elements.First() as Entry).MaxLength);
                (elements.First() as Entry).Text = text.Substring(0, maxTextLength);
                (elements.First() as Entry).SendCompleted();
            } else if (elements.First() is Editor) {
                var maxTextLength = Math.Min(text.Length, (elements.First() as Editor).MaxLength);
                (elements.First() as Editor).Text = text.Substring(0, maxTextLength);
                (elements.First() as Editor).SendCompleted();
            } else if (elements.First() is SearchBar)
                (elements.First() as SearchBar).Text = text;
            else if (elements.First() is Slider)
                (elements.First() as Slider).Value = double.Parse(text);
            else
                throw new InvalidOperationException($"element '{automationId}' cannot be used for input");

            elements.First().SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
        }

        public void Input(string automationId, int value)
        {
            Input(automationId, value.ToString());
        }

        public void Pick(string automationId, string text)
        {
            var elements = FindElements(automationId);
            if (elements.First() is Picker picker) {
                var indexToSelect = picker.Items.IndexOf(text);
                if (indexToSelect == -1)
                    throw new InvalidOperationException($"picker does not contain item '{text}'");
                picker.SelectedIndex = indexToSelect;
            } else
                throw new InvalidOperationException($"element '{automationId}' is not a Picker");
        }

        public void Cancel(string automationId)
        {
            var elements = FindElements(automationId);

            elements.First().SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, true);

            if (elements.First() is SearchBar)
                (elements.First() as SearchBar).Text = null;
            else
                throw new InvalidOperationException($"element '{automationId}' cannot be used for input");

            elements.First().SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
        }

        public void OpenMenu()
        {
            (app.MainPage as FlyoutPage).IsPresented = true;
        }

        public void CloseMenu()
        {
            (app.MainPage as FlyoutPage).IsPresented = false;
        }

        public void GoBack()
        {
            var modalPage = app.MainPage.Navigation.ModalStack.LastOrDefault();
            if (modalPage != null) {
                // Xamarin.Forms expects a synchronization context when popping a page from the modal stack via back button press
                try {
                    SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
                    modalPage.SendBackButtonPressed();
                } finally {
                    SynchronizationContext.SetSynchronizationContext(null);
                }
            } else
                app.MainPage.SendBackButtonPressed();
        }

        public void Print()
        {
            Console.WriteLine(Render());
        }

        public string Render()
        {
            if (popups.Any())
                return popups.Last().Render();
            else
                return CurrentPage.Render().Trim();
        }

        List<VisualElement> FindElements(string automationId)
        {
            var elements = CurrentPage.Find(automationId).Select(i => i.Element).OfType<VisualElement>().ToList();

            Assert.That(elements, Is.Not.Empty, $"Did not find entry \"{automationId}\" on current page");
            Assert.That(elements, Has.Count.LessThan(2), $"Found multiple entries \"{automationId}\" on current page");

            return elements;
        }
    }
}
