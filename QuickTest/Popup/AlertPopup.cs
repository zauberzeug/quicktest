using System.Linq;
using Xamarin.Forms.Internals;

namespace QuickTest
{
    public class AlertPopup : Popup
    {
        readonly AlertArguments arguments;

        public AlertPopup(AlertArguments arguments)
        {
            this.arguments = arguments;
        }

        public override bool Contains(string text)
        {
            return arguments.Title == text
                || arguments.Message == text
                || arguments.Cancel == text
                || arguments.Accept == text;
        }

        public override bool Tap(string text)
        {
            if (arguments.Accept == text) {
                arguments.SetResult(true);
                return true;
            } else if (arguments.Cancel == text) {
                arguments.SetResult(false);
                return true;
            } else
                return false;
        }

        public override string Render()
        {
            return $"{arguments.Title}\n{arguments.Message}\n\n{RenderButtons(arguments.Accept, arguments.Cancel)}";
        }

        public override string ToString()
        {
            return $"Alert \"{arguments.Title}\"";
        }

        string RenderButtons(params string[] titles)
        {
            return string.Join(" ", titles.Where(t => t != null).Select(t => $"[{t}]"));
        }
    }
}
