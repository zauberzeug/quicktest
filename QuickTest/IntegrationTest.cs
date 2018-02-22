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
        User user;
        TimeSpan timeout;

        protected T App { get; private set; }

        static IntegrationTest<T> CreateWithTimeout(T app, User user, TimeSpan timeout)
        {
            return new IntegrationTest<T> {
                App = app,
                user = user,
                timeout = timeout,
            };
        }

        [SetUp]
        protected virtual void SetUp()
        {
            MockForms.Init();

            App = CreateApp();
            user = new User(App);
            timeout = TimeSpan.FromSeconds(0.2);
        }

        virtual protected T CreateApp()
        {
            return new T();
        }

        public void Tap(params string[] texts)
        {
            foreach (var text in texts) {
                ShouldSee(text);
                user.Tap(text);
            }
        }

        public void TapNth(string text, int index)
        {
            Assert.That(() => user.Find(text), Has.Count.GreaterThan(index).After((int)timeout.TotalMilliseconds, 10),
                        $"User can't see {index + 1}th \"{text}\"  in \n{ user?.Render() }");
            user.Tap(text, index);
        }

        protected void Input(string automationId, string text)
        {
            ShouldSee(automationId);
            user.Input(automationId, text);
        }

        protected void Cancel(string automationId)
        {
            ShouldSee(automationId);
            user.Cancel(automationId);
        }

        public void ShouldSee(params string[] texts)
        {
            var list = new List<string>(texts);
            if (list.All(user.CanSee))
                return; // NOTE: prevent Assert from waiting 10 ms each time if text is seen immediately
            Assert.That(() => list.All(user.CanSee), Is.True.After((int)timeout.TotalMilliseconds, 10),
                        $"User can't see {{ {string.Join(", ", texts)} }} in \n{ user?.Render() }");
        }

        public void ShouldNotSee(params string[] texts)
        {
            var list = new List<string>(texts);
            if (!list.Any(user.CanSee))
                return; // NOTE: prevent Assert from waiting 10 ms each time if text is seen immediately
            Assert.That(() => !list.Any(user.CanSee), Is.True.After((int)timeout.TotalMilliseconds, 10),
                        $"User can see any of {{ {string.Join(", ", texts)} }} in \n{ user?.Render() }");
        }

        protected List<Element> Find(string text)
        {
            return user.Find(text);
        }

        /// <summary>
        /// Find elements matching the predicate.
        /// Containers like StackLayouts or ListViews are only traversed if they match the containerPredicate.
        /// </summary>
        protected List<Element> Find(Predicate<Element> predicate, Predicate<Element> containerPredicate = null)
        {
            return user.Find(predicate, containerPredicate);
        }

        protected void OpenMenu(string textToTap = null)
        {
            user.OpenMenu();

            if (textToTap != null)
                Tap(textToTap);
        }

        protected void GoBack()
        {
            user.GoBack();
        }

        protected string Render()
        {
            return user.Render();
        }

        [TearDown]
        protected virtual void TearDown()
        {
            user?.Print();
        }

        protected IntegrationTest<T> After(double seconds)
        {
            return CreateWithTimeout(App, user, TimeSpan.FromSeconds(seconds));
        }

        protected IntegrationTest<T> Now {
            get { return CreateWithTimeout(App, user, TimeSpan.FromSeconds(0.2)); }
        }
    }
}
