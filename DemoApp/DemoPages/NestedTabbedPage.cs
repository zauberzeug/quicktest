using System;
using Xamarin.Forms;

namespace DemoApp
{
    public class NestedTabbedPage : TabbedPage
    {
        public NestedTabbedPage()
        {
            Title = "NestedTabbedPage";
            PageExtensions.AddPageLog(this);

            var contentPageA = new TabbedPageDemoPage {
                Title = "Outer Tab A",
                AutomationId = "_OuterTab_A_AutomationId_",
            };

            var contentPageB = new ContentPage {
                Title = "Outer Tab B",
                AutomationId = "_OuterTab_B_AutomationId_",
                Content = new StackLayout {
                    Children = {
                        new Label { Text = "This is content on outer tab B" }
                    },
                },
                ToolbarItems = {
                    new ToolbarItem { Text = "OuterToolbarItem", Command = new Command(() => { }) }
                }
            }.AddPageLog();

            Children.Add(contentPageA);
            Children.Add(contentPageB);
        }
    }
}
