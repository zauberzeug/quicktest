using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace DemoApp
{
    public class ListViewDemoPage : ContentPage
    {
        public ListViewDemoPage(ListViewCachingStrategy cachingStrategy)
        {
            Title = "ListView demos";

            DemoListView.ConstructionCachingStrategy = cachingStrategy;

            Content = new StackLayout {
                Children = {
                    CreateSubpageButton(new DemoListViewWithTextCell(){Header = "plain header", Footer = "plain footer"}),
                    CreateSubpageButton(new DemoListViewWithStringViewCell(){Header = new DemoLabel("header label"), Footer = new DemoLabel("footer label")}),
                    CreateSubpageButton(new DemoListViewWithItemViewCell()),
                    CreateSubpageButton(new DemoListViewWithGroups()),
                    CreateSubpageButton(new DemoListViewWithGroupsAndHeaderTemplate()),
                    CreateSubpageButton(new DemoListViewWithGestureRecognizers()),
                    CreateSubpageButton(new DemoListViewWithRecycling()),
                    CreateSubpageButton(new DemoListViewWithRecyclingAndTemplateSelector()),
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