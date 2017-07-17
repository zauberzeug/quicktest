using Xamarin.Forms;

namespace DemoApp
{
    public class BindingDemoPage : ContentPage
    {
        public BindingDemoPage()
        {
            Title = "Binding demo";

            var bindableText = new BindableText("initial bound text");

            Content = new ScrollView {
                Content = new StackLayout {
                    Children = {
                        new DemoLabel().BindTo(bindableText, BindableText.TextProperty, Label.TextProperty),
                    },
                },
            };

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
