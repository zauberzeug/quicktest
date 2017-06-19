using System;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

namespace UserFlow
{
    public partial class User
    {
        void WireNavigation()
        {
            app.PropertyChanging += (s, args) => {
                if (args.PropertyName == nameof(Application.MainPage))
                    HandleDisappearing();
            };
            app.PropertyChanged += (s, args) => {
                if (args.PropertyName == nameof(Application.MainPage))
                    HandleAppearing();
            };

            HandleAppearing();
        }

        void HandleDisappearing()
        {
            (CurrentPage as IPageController).SendDisappearing();

            if (app.MainPage is MasterDetailPage) {
                (app.MainPage as MasterDetailPage).PropertyChanging -= HandleMasterDetailPropertyChanging;
                (app.MainPage as MasterDetailPage).PropertyChanged -= HandleMasterDetailPropertyChanged;
            }

            CurrentNavigationPage.Pushed -= HandlePushed;
            CurrentNavigationPage.Popped -= HandlePopped;
            CurrentNavigationPage.PoppedToRoot -= HandlePoppedToRoot;
        }

        void HandleAppearing()
        {
            (CurrentPage as IPageController).SendAppearing();

            if (app.MainPage is MasterDetailPage) {
                (app.MainPage as MasterDetailPage).PropertyChanging += HandleMasterDetailPropertyChanging;
                (app.MainPage as MasterDetailPage).PropertyChanged += HandleMasterDetailPropertyChanged;
            }

            CurrentNavigationPage.Pushed += HandlePushed;
            CurrentNavigationPage.Popped += HandlePopped;
            CurrentNavigationPage.PoppedToRoot += HandlePoppedToRoot;
        }

        void HandleMasterDetailPropertyChanging(object sender, Xamarin.Forms.PropertyChangingEventArgs e)
        {
            if (e.PropertyName == nameof(MasterDetailPage.Detail))
                (app.MainPage as MasterDetailPage).Detail.Navigation.NavigationStack.LastOrDefault()?.SendDisappearing();
        }

        void HandleMasterDetailPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MasterDetailPage.Detail))
                (app.MainPage as MasterDetailPage).Detail.Navigation.NavigationStack.LastOrDefault()?.SendAppearing();
        }

        void HandlePushed(object sender, NavigationEventArgs e)
        {
            var stack = CurrentPage.Navigation.NavigationStack;
            (stack[stack.Count - 2]).SendDisappearing();
            (e.Page as IPageController).SendAppearing();
        }

        void HandlePopped(object sender, NavigationEventArgs e)
        {
            (e.Page as IPageController).SendDisappearing();
            (CurrentPage as IPageController).SendAppearing();
        }

        void HandlePoppedToRoot(object sender, NavigationEventArgs e)
        {
            ((e as PoppedToRootEventArgs).PoppedPages.Last() as IPageController).SendDisappearing();
            (CurrentPage as IPageController).SendAppearing();
        }
    }
}
