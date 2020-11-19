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
        User User {
            get {
                if (user == null)
                    throw new LaunchException();
                return user;
            }
            set {
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
            Print();
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

        public virtual void Tap(char text)
        {
            Tap(text.ToString());
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
            Assert.That(User.SeesPopup(), Is.False, "TapNath is not supported on popups");
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
            if (list.All((t) => User.CanSee(t, 1)))
                return; // NOTE: prevent Assert from waiting 10 ms each time if text is seen immediately
            Assert.That(() => list.All((t) => User.CanSee(t, 1)), Is.True.After((int)Timeout.TotalMilliseconds, 10),
                        $"User should see {{ {string.Join(", ", texts)} }} only once in \n{ User?.Render() }");
        }

        public virtual void ShouldSee(char text)
        {
            ShouldSee(text.ToString());
        }

        public virtual void ShouldSee(string text, int count)
        {
            if (User.CanSee(text, count))
                return; // NOTE: prevent Assert from waiting 10 ms each time if text is seen immediately
            Assert.That(() => User.CanSee(text, count), Is.True.After((int)Timeout.TotalMilliseconds, 10),
                $"User should see {{ {text} }} {count} times in \n{ User?.Render() }");
        }

        public virtual void ShouldSee(char text, int count)
        {
            ShouldSee(text.ToString(), count);
        }

        public virtual void ShouldNotSee(params string[] texts)
        {
            var list = new List<string>(texts);
            if (!list.Any(User.CanSee))
                return; // NOTE: prevent Assert from waiting 10 ms each time if text is seen immediately
            Assert.That(() => !list.Any(User.CanSee), Is.True.After((int)Timeout.TotalMilliseconds, 10),
                        $"User should not see any of {{ {string.Join(", ", texts)} }} in \n{ User?.Render() }");
        }

        public virtual void ShouldNotSee(char text)
        {
            ShouldNotSee(text.ToString());
        }

        /// <summary>
        /// Whether a given text is visible to the user or not.
        /// </summary>
        protected virtual bool CanSee(string text)
        {
            return User.CanSee(text);
        }

        protected virtual bool CanSee(char text)
        {
            return CanSee(text.ToString());
        }

        /// <summary>
        /// Whether an alert is shown.
        /// </summary>
        protected virtual bool SeesAlert() => User.SeesAlert();

        /// <summary>
        /// Whether an alert is shown.
        /// </summary>
        protected virtual bool SeesActionSheet() => User.SeesActionSheet();

        /// <summary>
        /// Find elements matching the exact string.
        /// Containers like StackLayouts or ListViews are only traversed.
        /// </summary>
        protected virtual List<Element> Find(string text)
        {
            if (User.SeesPopup())
                return new List<Element>();
            return User.Find(text);
        }

        /// <summary>
        /// Find first element matching the exact string.
        /// Containers like StackLayouts or ListViews are only traversed.
        /// </summary>
        protected virtual Element FindFirst(string text)
        {
            return Find(text).FirstOrDefault();
        }

        /// <summary>
        /// Find elements matching the predicate.
        /// Containers like StackLayouts or ListViews are only traversed if they match the containerPredicate.
        /// </summary>
        protected virtual List<Element> Find(Predicate<Element> predicate, Predicate<Element> containerPredicate = null)
        {
            if (User.SeesPopup())
                return new List<Element>();
            return User.Find(predicate, containerPredicate);
        }

        /// <summary>
        /// Finds first element matching the predicate.
        /// Containers like StackLayouts or ListViews are only traversed if they match the containerPredicate.
        /// </summary>
        protected virtual Element FindFirst(Predicate<Element> predicate, Predicate<Element> containerPredicate = null)
        {
            return Find(predicate, containerPredicate).FirstOrDefault();
        }

        /// <summary>
        /// Opens the menu.
        /// </summary>
        /// <param name="textToTap">If provided the text will be selected.</param>
        protected virtual void OpenMenu(string textToTap = null)
        {
            Assert.That(User.SeesPopup(), Is.False, "Cannot open menu if popup is presented");
            User.OpenMenu();

            if (textToTap != null)
                Tap(textToTap);
        }

        protected virtual void CloseMenu()
        {
            Assert.That(User.SeesPopup(), Is.False, "Cannot close menu if popup is presented");
            User.CloseMenu();
        }

        protected virtual void GoBack()
        {
            Assert.That(User.SeesPopup(), Is.False, "Cannot go back if popup is presented");
            User.GoBack();
        }

        protected virtual string Render()
        {
            return User.Render();
        }

        protected virtual void Print()
        {
            Console.WriteLine(Render());
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
