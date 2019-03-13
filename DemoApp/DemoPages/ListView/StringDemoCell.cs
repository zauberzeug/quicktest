using Xamarin.Forms;

namespace DemoApp
{
    public class StringDemoCell : ViewCell
    {
        public StringDemoCell()
        {
            var label = new DemoLabel();
            label.SetBinding(Label.TextProperty, ".");
            View = label;
        }
    }
}