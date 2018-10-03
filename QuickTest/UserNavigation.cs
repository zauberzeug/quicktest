using System;
using System.Collections.Specialized;
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
                    HandleAppDisappearing();
            };
            app.PropertyChanged += (s, args) => {
                if (args.PropertyName == nameof(Application.MainPage))
                    HandleAppAppearing();
            };

            HandleAppAppearing();
        }

        void HandleAppDisappearing()
        {
            HandleDisappearing(CurrentPage);

            if (app.MainPage is MasterDetailPage) {
                (app.MainPage as MasterDetailPage).PropertyChanging -= HandleMasterDetailPropertyChanging;
                (app.MainPage as MasterDetailPage).PropertyChanged -= HandleMasterDetailPropertyChanged;
            }

            OnNavigationPageRemoved();

            app.ModalPushing -= HandleModalPushing;
            app.ModalPushed -= HandleModalPushed;
            app.ModalPopping -= HandleModalPopping;
            app.ModalPopped -= HandleModalPopped;
        }

        void HandleAppAppearing()
        {
            HandleAppearing(CurrentPage);

            if (app.MainPage is MasterDetailPage) {
                (app.MainPage as MasterDetailPage).PropertyChanging += HandleMasterDetailPropertyChanging;
                (app.MainPage as MasterDetailPage).PropertyChanged += HandleMasterDetailPropertyChanged;
            }

            OnNavigationPageAdded();

            app.ModalPushing += HandleModalPushing;
            app.ModalPushed += HandleModalPushed;
            app.ModalPopping += HandleModalPopping;
            app.ModalPopped += HandleModalPopped;
        }

        void HandleAppearing(Page page)
        {
            (page as IPageController).SendAppearing();

            if (page is MultiPage<Page>) {
                var multiPage = page as MultiPage<Page>;
                multiPage.PropertyChanging += HandleMultiPagePropertyChanging;
                multiPage.PropertyChanged += HandleMultiPagePropertyChanged;
            }
        }

        private void HandleMultiPagePropertyChanging(object sender, Xamarin.Forms.PropertyChangingEventArgs e)
        {
            var multiPage = sender as MultiPage<Page>;
            HandleDisappearing(multiPage.CurrentPage);
        }

        private void HandleMultiPagePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var multiPage = sender as MultiPage<Page>;
            HandleAppearing(multiPage.CurrentPage);
        }

        void HandleDisappearing(Page page)
        {
            (page as IPageController).SendDisappearing();
        }

        void OnNavigationPageAdded()
        {
            CurrentNavigationPage.Pushed += HandlePushed;
            CurrentNavigationPage.Popped += HandlePopped;
            CurrentNavigationPage.PoppedToRoot += HandlePoppedToRoot;
        }

        void OnNavigationPageRemoved()
        {
            CurrentNavigationPage.Pushed -= HandlePushed;
            CurrentNavigationPage.Popped -= HandlePopped;
            CurrentNavigationPage.PoppedToRoot -= HandlePoppedToRoot;
        }

        void HandleMasterDetailPropertyChanging(object sender, Xamarin.Forms.PropertyChangingEventArgs e)
        {
            if (e.PropertyName == nameof(MasterDetailPage.Detail)) {
                HandleDisappearing((app.MainPage as MasterDetailPage).Detail.Navigation.NavigationStack.LastOrDefault());
                OnNavigationPageRemoved();
            }
        }

        void HandleMasterDetailPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MasterDetailPage.Detail)) {
                HandleAppearing((app.MainPage as MasterDetailPage).Detail.Navigation.NavigationStack.LastOrDefault());
                OnNavigationPageAdded();
            }
        }

        void HandlePushed(object sender, NavigationEventArgs e)
        {
            var stack = CurrentPage.Navigation.NavigationStack;
            (stack[stack.Count - 2]).SendDisappearing();
            HandleAppearing(e.Page);
        }

        void HandlePopped(object sender, NavigationEventArgs e)
        {
            HandleDisappearing(e.Page);
            HandleAppearing(CurrentPage);
        }

        void HandlePoppedToRoot(object sender, NavigationEventArgs e)
        {
            ((e as PoppedToRootEventArgs).PoppedPages.Last() as IPageController).SendDisappearing();
            HandleAppearing(e.Page);
        }

        void HandleModalPushing(object sender, ModalPushingEventArgs e)
        {
            HandleDisappearing(CurrentPage);
        }

        void HandleModalPushed(object sender, ModalPushedEventArgs e)
        {
            HandleAppearing(e.Modal);
            if (e.Modal is NavigationPage) {
                (e.Modal as NavigationPage).Pushed += HandlePushed;
                (e.Modal as NavigationPage).Popped += HandlePopped;
                (e.Modal as NavigationPage).PoppedToRoot += HandlePoppedToRoot;
            }
        }

        void HandleModalPopping(object sender, ModalPoppingEventArgs e)
        {
            HandleDisappearing(e.Modal);
        }

        void HandleModalPopped(object sender, ModalPoppedEventArgs e)
        {
            HandleDisappearing(e.Modal);
            if (e.Modal is NavigationPage) {
                (e.Modal as NavigationPage).Pushed -= HandlePushed;
                (e.Modal as NavigationPage).Popped -= HandlePopped;
                (e.Modal as NavigationPage).PoppedToRoot -= HandlePoppedToRoot;
            }
        }
    }
}
