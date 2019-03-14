using Xamarin.Forms;

namespace DemoApp
{
    public class ListViewDemoPage : ContentPage
    {
        public ListViewDemoPage(ListViewCachingStrategy cachingStrategy)
        {
            Title = "ListView demos";

            Content = new StackLayout {
                Children = {
                    CreateSubpageButton(new DemoListViewWithTextCell(cachingStrategy) {
                        Header = "plain header", Footer = "plain footer"
                    }),
                    CreateSubpageButton(new DemoListViewWithStringViewCell(cachingStrategy) {
                        Header = new DemoLabel("header label"), Footer = new DemoLabel("footer label")
                    }),
                    CreateSubpageButton(new DemoListViewWithItemViewCell(cachingStrategy)),
                    CreateSubpageButton(new DemoListViewWithGroups(cachingStrategy)),
                    CreateSubpageButton(new DemoListViewWithGroupsAndHeaderTemplate(cachingStrategy)),
                    CreateSubpageButton(new DemoListViewWithGestureRecognizers(cachingStrategy)),
                    CreateSubpageButton(new DemoListViewForRecycling(cachingStrategy)),
                    CreateSubpageButton(new DemoListViewForRecyclingWithTemplateSelector(cachingStrategy)),
                    CreateSubpageButton(new DemoListViewForRecyclingWithGroups(cachingStrategy)),
                },
            };
        }

        DemoButton CreateSubpageButton(ListView listView)
        {
            return new DemoButton(listView.GetType().Name) {
                Command = new Command(o => Navigation.PushAsync(new NavigationDemoPage(listView.GetType().Name) { Content = listView })),
            };
        }
    }
}