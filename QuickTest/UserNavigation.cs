using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

namespace QuickTest
{
    public partial class User
    {
        void WireNavigation()
        {
            app.PropertyChanging += (s, args) => {
                if (args.PropertyName == nameof(Application.MainPage))
                    HandleMainPageChanging();
            };
            app.PropertyChanged += (s, args) => {
                if (args.PropertyName == nameof(Application.MainPage))
                    HandleMainPageChanged();
            };

            HandleMainPageChanged();
        }

        void HandleMainPageChanging()
        {
            var mainPage = app.MainPage;
            SendDisappearing(mainPage);
            UnsubscribeFromPageEvents(mainPage);

            app.ModalPushing -= HandleModalPushing;
            app.ModalPushed -= HandleModalPushed;
            app.ModalPopping -= HandleModalPopping;
            app.ModalPopped -= HandleModalPopped;
        }

        void HandleMainPageChanged()
        {
            var mainPage = app.MainPage;
            EnablePlatform(mainPage);
            SendAppearing(mainPage);
            SubscribeToPageEvents(mainPage);

            app.ModalPushing += HandleModalPushing;
            app.ModalPushed += HandleModalPushed;
            app.ModalPopping += HandleModalPopping;
            app.ModalPopped += HandleModalPopped;
        }

        void EnablePlatform(Page page)
        {
            page.IsPlatformEnabled = true;

            if (page is FlyoutPage flyoutPage) {
                EnablePlatform(flyoutPage.Flyout);
                EnablePlatform(flyoutPage.Detail);
            } else if (page is IPageContainer<Page> pageContainer) {
                EnablePlatform(pageContainer.CurrentPage);
            }
        }

        void SendAppearing(Page page)
        {
            page.SendAppearing();
            if (page is FlyoutPage flyoutPage) {
                if (flyoutPage.IsPresented)
                    flyoutPage.Flyout.SendAppearing();
                else
                    flyoutPage.Detail.SendAppearing();
            }
        }

        void SendDisappearing(Page page)
        {
            if (page is FlyoutPage flyoutPage) {
                if (flyoutPage.IsPresented)
                    flyoutPage.Flyout.SendDisappearing();
                else
                    flyoutPage.Detail.SendDisappearing();
            }
            page.SendDisappearing();
        }

        void SubscribeToPageEvents(Page page)
        {
            if (page is FlyoutPage flyoutPage) {
                flyoutPage.PropertyChanging += HandleFlyoutPagePropertyChanging;
                flyoutPage.PropertyChanged += HandleFlyoutPagePropertyChanged;
                SubscribeToPageEvents(flyoutPage.Flyout);
                SubscribeToPageEvents(flyoutPage.Detail);
            }

            if (page is IPageContainer<Page> pageContainer) {
                page.PropertyChanging += HandlePageContainerPropertyChanging;
                page.PropertyChanged += HandlePageContainerPropertyChanged;
                SubscribeToPageEvents(pageContainer.CurrentPage);
            }
        }

        void UnsubscribeFromPageEvents(Page page)
        {
            if (page is FlyoutPage flyoutPage) {
                flyoutPage.PropertyChanging -= HandleFlyoutPagePropertyChanging;
                flyoutPage.PropertyChanged -= HandleFlyoutPagePropertyChanged;
                UnsubscribeFromPageEvents(flyoutPage.Flyout);
                UnsubscribeFromPageEvents(flyoutPage.Detail);
            }

            if (page is IPageContainer<Page> pageContainer) {
                page.PropertyChanging -= HandlePageContainerPropertyChanging;
                page.PropertyChanged -= HandlePageContainerPropertyChanged;
                UnsubscribeFromPageEvents(pageContainer.CurrentPage);
            }
        }

        void HandleFlyoutPagePropertyChanging(object sender, Xamarin.Forms.PropertyChangingEventArgs e)
        {
            var flyoutPage = sender as FlyoutPage;

            Page page = null;
            if (e.PropertyName == nameof(flyoutPage.Detail))
                page = flyoutPage.Detail;
            else if (e.PropertyName == nameof(flyoutPage.Flyout))
                page = flyoutPage.Flyout;

            if (page != null) {
                SendDisappearing(page);
                UnsubscribeFromPageEvents(page);
            }
        }

        void HandleFlyoutPagePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var flyoutPage = sender as FlyoutPage;

            Page page = null;
            if (e.PropertyName == nameof(flyoutPage.Detail))
                page = flyoutPage.Detail;
            else if (e.PropertyName == nameof(flyoutPage.Flyout))
                page = flyoutPage.Flyout;

            if (page != null) {
                EnablePlatform(page);
                SendAppearing(page);
                SubscribeToPageEvents(page);
            }
        }

        void HandlePageContainerPropertyChanging(object sender, Xamarin.Forms.PropertyChangingEventArgs e)
        {
            var pageContainer = sender as IPageContainer<Page>;
            if (e.PropertyName != nameof(pageContainer.CurrentPage))
                return;
            SendDisappearing(pageContainer.CurrentPage);
        }

        void HandlePageContainerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var pageContainer = sender as IPageContainer<Page>;
            if (e.PropertyName != nameof(pageContainer.CurrentPage))
                return;
            var page = pageContainer.CurrentPage;
            EnablePlatform(page);
            SendAppearing(page);
            SubscribeToPageEvents(page);
        }

        void HandleModalPushing(object sender, ModalPushingEventArgs e)
        {
            var page = GetCurrentModalOrMainPage();
            SendDisappearing(page);
            UnsubscribeFromPageEvents(page);
        }

        void HandleModalPushed(object sender, ModalPushedEventArgs e)
        {
            var page = e.Modal;
            EnablePlatform(page);
            SendAppearing(page);
            SubscribeToPageEvents(page);
        }

        void HandleModalPopping(object sender, ModalPoppingEventArgs e)
        {
            var page = e.Modal;
            SendDisappearing(page);
            UnsubscribeFromPageEvents(page);
        }

        void HandleModalPopped(object sender, ModalPoppedEventArgs e)
        {
            var page = GetCurrentModalOrMainPage();
            EnablePlatform(page);
            SendAppearing(page);
            SubscribeToPageEvents(page);
        }

        Page GetCurrentModalOrMainPage() => app.MainPage.Navigation.ModalStack.LastOrDefault() ?? app.MainPage;
    }
}
