using System;
using System.Threading.Tasks;
using DemoApp;
using NUnit.Framework;
using NUnit.Framework.Internal;
using QuickTest;
using Xamarin.Forms;

namespace Tests
{
    public class UsageTests : QuickTest<App>
    {
        protected override void TearDown()
        {
            base.TearDown();
            ResetApp();
        }

        [Test]
        public void ErrorWhenNotLaunchingApp()
        {
            Assert.Throws<LaunchException>(() => ShouldSee("something"));
            Launch(new App());
            ShouldSee("Navigation");
        }

        [Test]
        public void SettingNavigationPageWhileModalPageIsShown()
        {
            Launch(new App());
            App.MainPage.Navigation.PushModalAsync(new ContentPage { Content = new Label { Text = "Modal Page" } });
            App.Flyout.Detail = new NavigationPage(new ContentPage { Content = new Label { Text = "New Page" } });
            ShouldSee("Modal Page");
        }

        // This test reproduces a problem with QuickTest output in case of a failing test.
        [Test]
        public void ErrorOutputPrintsUpToDateViewHierarchyEvenWhenPolling()
        {
            Launch(new App());
            var layout = new StackLayout {
                Orientation = StackOrientation.Vertical,
                Children = {
                    new Label { Text = "Label 1" },
                    new Label { Text = "Label 2" },
                }
            };
            var contentPage = new ContentPage {
                Content = layout,
            };
            App.Instance.Flyout.Detail = new NavigationPage(contentPage);

            Task.Run(async delegate {
                await Task.Delay(100);
                layout.Children.Add(new Label { Text = "Label 3" });
            });

            Exception assertionException = null;

            using (new TestExecutionContext.IsolatedContext()) {
                try {
                    // Label 3 should be visible in the error output
                    After(1).ShouldSee("Label 4");
                } catch (Exception e) {
                    assertionException = e;
                }
            }

            Assert.That(assertionException, Is.Not.Null);
            Assert.That(assertionException.Message, Does.Contain("Label 3"));
        }
    }
}
