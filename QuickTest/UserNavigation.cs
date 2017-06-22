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

            OnNavigationPageRemoved();

            app.ModalPushing -= HandleModalPushing;
            app.ModalPushed -= HandleModalPushed;
            app.ModalPopping -= HandleModalPopping;
            app.ModalPopped -= HandleModalPopped;
        }

        void HandleAppearing()
        {
            (CurrentPage as IPageController).SendAppearing();

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
                (app.MainPage as MasterDetailPage).Detail.Navigation.NavigationStack.LastOrDefault()?.SendDisappearing();
                OnNavigationPageRemoved();
            }
        }

        void HandleMasterDetailPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MasterDetailPage.Detail)) {
                (app.MainPage as MasterDetailPage).Detail.Navigation.NavigationStack.LastOrDefault()?.SendAppearing();
                OnNavigationPageAdded();
            }
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

        void HandleModalPushing(object sender, ModalPushingEventArgs e)
        {
            (CurrentPage as IPageController).SendDisappearing();
        }

        void HandleModalPushed(object sender, ModalPushedEventArgs e)
        {
            (e.Modal as IPageController).SendAppearing();
            if (e.Modal is NavigationPage) {
                (e.Modal as NavigationPage).Pushed += HandlePushed;
                (e.Modal as NavigationPage).Popped += HandlePopped;
                (e.Modal as NavigationPage).PoppedToRoot += HandlePoppedToRoot;
            }
        }

        void HandleModalPopping(object sender, ModalPoppingEventArgs e)
        {
            (e.Modal as IPageController).SendDisappearing();
        }

        void HandleModalPopped(object sender, ModalPoppedEventArgs e)
        {
            (e.Modal as IPageController).SendAppearing();
            if (e.Modal is NavigationPage) {
                (e.Modal as NavigationPage).Pushed -= HandlePushed;
                (e.Modal as NavigationPage).Popped -= HandlePopped;
                (e.Modal as NavigationPage).PoppedToRoot -= HandlePoppedToRoot;
            }
        }
    }
}
