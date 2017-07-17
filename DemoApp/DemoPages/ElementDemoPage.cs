using Xamarin.Forms;

namespace DemoApp
{
    public class ElementDemoPage : ContentPage
    {
        public ElementDemoPage()
        {
            Title = "Element demo";

            var bindableText = new BindableText("initial bound text");

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
                        new DemoLabel().BindTo(bindableText, BindableText.TextProperty, Label.TextProperty),
                        new DemoCountdown(),
                    },
                },
            };

            ToolbarItems.Add(new DemoToolbarItem());
            bindableText.Text = "updated bound text";
        }
    }

    public class BindableText : BindableObject
    {
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(string), null);

        public BindableText(string text)
        {
            Text = text;
        }

        public string Text {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
    }
}
