using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Xamarin.Forms;
using Xamarin.Forms.Mocks;

namespace QuickTest
{
    public class QuickTest<T> where T : Application
    {
        User user;
        public User User {
            get {
                if (user == null)
                    throw new LaunchException();
                return user;
            }
            private set {
                user = value;
            }
        }

        protected TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(0.2);

        protected T App { get; private set; }

        [SetUp]
        protected virtual void SetUp()
        {
            // Use null as runtimePlatform to be able to set all caching strategies in ListView
            MockForms.Init(runtimePlatform: null);
        }

        [TearDown]
        protected virtual void TearDown()
        {
            User?.Print();
        }

        public void Launch(T app)
        {
            App = app;
            User = new User(App);
        }

        public virtual void Tap(string text)
        {
            ShouldSee(text);
            User.Tap(text);
        }

        public virtual void Tap(params string[] texts)
        {
            foreach (var text in texts) {
                ShouldSee(text);
                User.Tap(text);
            }
        }

        public virtual void TapNth(string text, int index)
        {
            Assert.That(() => User.Find(text), Has.Count.GreaterThan(index).After((int)Timeout.TotalMilliseconds, 10),
                        $"User can't see {index + 1}th \"{text}\"  in \n{ User?.Render() }");
            User.Tap(text, index);
        }

        public virtual void TapNth(char text, int index)
        {
            TapNth(text.ToString(), index);
        }

        protected virtual void Input(string automationId, string text)
        {
            ShouldSee(automationId);
            User.Input(automationId, text);
        }

        protected virtual void Input(string automationId, int value)
        {
            ShouldSee(automationId);
            User.Input(automationId, value);
        }

        protected virtual void Pick(string automationId, string text)
        {
            ShouldSee(automationId);
            User.Pick(automationId, text);
        }

        protected virtual void Cancel(string automationId)
        {
            ShouldSee(automationId);
            User.Cancel(automationId);
        }

        public virtual void ShouldSee(params string[] texts)
        {
            var list = new List<string>(texts);
            if (list.All(User.CanSee))
                return; // NOTE: prevent Assert from waiting 10 ms each time if text is seen immediately
            Assert.That(() => list.All(User.CanSee), Is.True.After((int)Timeout.TotalMilliseconds, 10),
                        $"User should see {{ {string.Join(", ", texts)} }} in \n{ User?.Render() }");
        }

        public virtual void ShouldSeeOnce(params string[] texts)
        {
            var list = new List<string>(texts);
            Assert.That(() => list.All(User.CanSeeOnce), Is.True.After((int)Timeout.TotalMilliseconds, 10),
                        $"User should see {{ {string.Join(", ", texts)} }} only once in \n{ User?.Render() }");
        }

        public virtual void ShouldSee(char match)
        {
            ShouldSee(match.ToString());
        }

        public virtual void ShouldSee(string match, int count)
        {
            var matchCount = Find(match).Count;
            Assert.That(matchCount, Is.EqualTo(count), $"The count of '{match}' should be {count}, but was {matchCount}");
        }

        public virtual void ShouldSee(char match, int count)
        {
            ShouldSee(match.ToString(), count);
        }

        public virtual void ShouldNotSee(params string[] texts)
        {
            var list = new List<string>(texts);
            if (!list.Any(User.CanSee))
                return; // NOTE: prevent Assert from waiting 10 ms each time if text is seen immediately
            Assert.That(() => !list.Any(User.CanSee), Is.True.After((int)Timeout.TotalMilliseconds, 10),
                        $"User should not see any of {{ {string.Join(", ", texts)} }} in \n{ User?.Render() }");
        }

        public virtual void ShouldNotSee(char match)
        {
            ShouldNotSee(match.ToString());
        }

        /// <summary>
        /// Weather a given text is visible to the user or not.
        /// </summary>
        protected virtual bool CanSee(string text)
        {
            return User.CanSee(text);
        }

        /// <summary>
        /// Weather a popup is shown.
        /// </summary>
        protected virtual bool SeesAlert()
        {
            return User.SeesAlert();
        }

        /// <summary>
        /// Find elements matching the exact string.
        /// Containers like StackLayouts or ListViews are only traversed.
        /// </summary>
        protected virtual List<Element> Find(string text)
        {
            return User.Find(text);
        }

        /// <summary>
        /// Find first element matching the exact string.
        /// Containers like StackLayouts or ListViews are only traversed.
        /// </summary>
        protected virtual Element FindFirst(string text)
        {
            return User.Find(text).FirstOrDefault();
        }

        /// <summary>
        /// Find elements matching the predicate.
        /// Containers like StackLayouts or ListViews are only traversed if they match the containerPredicate.
        /// </summary>
        protected virtual List<Element> Find(Predicate<Element> predicate, Predicate<Element> containerPredicate = null)
        {
            return User.Find(predicate, containerPredicate);
        }

        /// <summary>
        /// Finds first element matching the predicate.
        /// Containers like StackLayouts or ListViews are only traversed if they match the containerPredicate.
        /// </summary>
        protected virtual Element FindFirst(Predicate<Element> predicate, Predicate<Element> containerPredicate = null)
        {
            return User.Find(predicate, containerPredicate).FirstOrDefault();
        }

        /// <summary>
        /// Opens the menu.
        /// </summary>
        /// <param name="textToTap">If provided the text will be selected.</param>
        protected virtual void OpenMenu(string textToTap = null)
        {
            User.OpenMenu();

            if (textToTap != null)
                Tap(textToTap);
        }

        protected virtual void GoBack()
        {
            User.GoBack();
        }

        protected virtual string Render()
        {
            return User.Render();
        }

        protected QuickTest<T> After(double seconds)
        {
            return CreateWithTimeout(App, User, TimeSpan.FromSeconds(seconds));
        }

        protected QuickTest<T> Now {
            get { return CreateWithTimeout(App, User, TimeSpan.FromSeconds(0.2)); }
        }

        static QuickTest<T> CreateWithTimeout(T app, User user, TimeSpan timeout)
        {
            return new QuickTest<T> {
                App = app,
                User = user,
                Timeout = timeout,
            };
        }
    }
}
