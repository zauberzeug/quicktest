using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Xamarin.Forms;
using Xamarin.Forms.Mocks;

namespace QuickTest
{
    public class IntegrationTest<T> where T : Application, new()
    {
        public User User { get; private set; }
        TimeSpan timeout;

        protected T App { get; private set; }

        static IntegrationTest<T> CreateWithTimeout(T app, User user, TimeSpan timeout)
        {
            return new IntegrationTest<T> {
                App = app,
                User = user,
                timeout = timeout,
            };
        }

        [SetUp]
        virtual protected void SetUp()
        {
            MockForms.Init();

            App = CreateApp();
            User = new User(App);
            timeout = TimeSpan.FromSeconds(0.2);
        }

        virtual protected T CreateApp()
        {
            return new T();
        }

        virtual public void Tap(params string[] texts)
        {
            foreach (var text in texts) {
                ShouldSee(text);
                User.Tap(text);
            }
        }

        virtual public void TapNth(string text, int index)
        {
            Assert.That(() => User.Find(text), Has.Count.GreaterThan(index).After((int)timeout.TotalMilliseconds, 10),
                        $"User can't see {index + 1}th \"{text}\"  in \n{ User?.Render() }");
            User.Tap(text, index);
        }

        virtual protected void Input(string automationId, string text)
        {
            ShouldSee(automationId);
            User.Input(automationId, text);
        }

        virtual protected void Cancel(string automationId)
        {
            ShouldSee(automationId);
            User.Cancel(automationId);
        }

        virtual public void ShouldSee(params string[] texts)
        {
            var list = new List<string>(texts);
            if (list.All(User.CanSee))
                return; // NOTE: prevent Assert from waiting 10 ms each time if text is seen immediately
            Assert.That(() => list.All(User.CanSee), Is.True.After((int)timeout.TotalMilliseconds, 10),
                        $"User can't see {{ {string.Join(", ", texts)} }} in \n{ User?.Render() }");
        }

        virtual public void ShouldNotSee(params string[] texts)
        {
            var list = new List<string>(texts);
            if (!list.Any(User.CanSee))
                return; // NOTE: prevent Assert from waiting 10 ms each time if text is seen immediately
            Assert.That(() => !list.Any(User.CanSee), Is.True.After((int)timeout.TotalMilliseconds, 10),
                        $"User can see any of {{ {string.Join(", ", texts)} }} in \n{ User?.Render() }");
        }

        /// <summary>
        /// Weather a given text is visible to the user or not.
        /// </summary>
        virtual protected bool CanSee(string text)
        {
            return User.CanSee(text);
        }

        /// <summary>
        /// Weather a popup is shown.
        /// </summary>
        virtual protected bool SeesAlert()
        {
            return User.SeesAlert();
        }

        /// <summary>
        /// Find elements matching the exact string.
        /// Containers like StackLayouts or ListViews are only traversed.
        /// </summary>
        virtual protected List<Element> Find(string text)
        {
            return User.Find(text);
        }

        /// <summary>
        /// Find first element matching the exact string.
        /// Containers like StackLayouts or ListViews are only traversed.
        /// </summary>
        virtual protected Element FindFirst(string text)
        {
            return User.Find(text).FirstOrDefault();
        }

        /// <summary>
        /// Find elements matching the predicate.
        /// Containers like StackLayouts or ListViews are only traversed if they match the containerPredicate.
        /// </summary>
        virtual protected List<Element> Find(Predicate<Element> predicate, Predicate<Element> containerPredicate = null)
        {
            return User.Find(predicate, containerPredicate);
        }

        /// <summary>
        /// Finds first element matching the predicate.
        /// Containers like StackLayouts or ListViews are only traversed if they match the containerPredicate.
        /// </summary>
        virtual protected Element FindFirst(Predicate<Element> predicate, Predicate<Element> containerPredicate = null)
        {
            return User.Find(predicate, containerPredicate).FirstOrDefault();
        }

        /// <summary>
        /// Opens the menu.
        /// </summary>
        /// <param name="textToTap">If provided the text will be selected.</param>
        virtual protected void OpenMenu(string textToTap = null)
        {
            User.OpenMenu();

            if (textToTap != null)
                Tap(textToTap);
        }

        virtual protected void GoBack()
        {
            User.GoBack();
        }

        virtual protected string Render()
        {
            return User.Render();
        }

        [TearDown]
        virtual protected void TearDown()
        {
            User?.Print();
        }

        protected IntegrationTest<T> After(double seconds)
        {
            return CreateWithTimeout(App, User, TimeSpan.FromSeconds(seconds));
        }

        protected IntegrationTest<T> Now {
            get { return CreateWithTimeout(App, User, TimeSpan.FromSeconds(0.2)); }
        }
    }
}
