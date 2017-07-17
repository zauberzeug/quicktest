using Xamarin.Forms;

namespace DemoApp
{
    public class ElementDemoPage : ContentPage
    {
        public ElementDemoPage()
        {
            Title = "Element demo";

            Content = new ScrollView {
                Content = new StackLayout {
                    Children = {
                        new SearchBar {
                            AutomationId = "searchbar_automation_id",
                            BackgroundColor = Color.White,
                            HeightRequest = 48, // HACK: https://bugzilla.xamarin.com/show_bug.cgi?id=43975
                        },
                        new DemoButton("Button"),
                        new DemoLabel("Label").WithGestureRecognizer(),
                        new DemoStack(),
                        new DemoGrid(),
                        new ContentView{Content = new DemoLabel("label within ContentView")},
                        new DemoEntry("entry_automation_id", "Placeholder"),
                        new DemoEditor("editor_automation_id", "editor content"),
                        new DemoLabel("Invisible Label").Invisible(),
                        new DemoCountdown(),
                    },
                },
            };

            ToolbarItems.Add(new DemoToolbarItem());
        }
    }
}
