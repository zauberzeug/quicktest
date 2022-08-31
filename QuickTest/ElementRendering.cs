using System;
using System.Linq;
using Xamarin.Forms;

namespace QuickTest
{
    public static class ElementRendering
    {
        public static string Render(this Element element)
        {
            if (!(element as VisualElement)?.IsVisible ?? false)
                return "";

            var result = "· ";// + $"<{element.GetType().Name}> ";

            result += GetTitle(element as ContentPage);

            if (element is Page) {
                var tabbedPage = element.FindParent<TabbedPage>();
                var tabResult = "";
                while (tabbedPage != null) {
                    tabResult = "\n|" + string.Join("|", tabbedPage.Children.Select(p => tabbedPage.CurrentPage == p ? $"> {p.Title} <" : $" {p.Title} ")) + "|" + tabResult;
                    tabbedPage = tabbedPage.FindParent<TabbedPage>();
                }
                result += tabResult;
            }

            result += (element as ContentPage)?.Content?.Render();

            result += RenderContent(element);

            result = "\n" + result.Replace("\n", "\n  ");

            return result;
        }

        static string RenderContent(Element element)
        {
            var result = "";

            var automationId = (element as VisualElement)?.AutomationId;
            if (automationId != null)
                result += $"({automationId}) ";

            result += (element as ContentView)?.Content.Render();
            result += (element as ScrollView)?.Content.Render();
            result += string.Join("", (element as Layout<View>)?.Children.Select(c => c.Render()) ?? new[] { "" });
            result += (element as ListView)?.Render();

            result += (element as Label)?.FormattedText?.ToString() ?? (element as Label)?.Text;
            result += (element as Button)?.Text;
            result += (element as Entry)?.Text;
            result += (element as Editor)?.Text;
            result += (element as SearchBar)?.Text;
            result += (element as Image)?.Source?.AutomationId;

            if (element is Picker) {
                var picker = element as Picker;
                if (picker.SelectedIndex >= 0)
                    result += picker.SelectedItem.ToString();
                else
                    result += picker.Title;
            }

            if (element is Slider)
                result += "--o---- " + (element as Slider).Value;

            result += (element as TextCell)?.Text;
            result += (element as ViewCell)?.View?.Render();

            return result;
        }

        static string GetTitle(ContentPage page)
        {
            var navigation = page?.Navigation;
            if (navigation == null)
                return ""; // without navigation the user can't see the title

            string result = "";
            var titleView = NavigationPage.GetTitleView(page);
            if (titleView != null)
                result += $"*{RenderContent(titleView).Replace("\n", "").Replace("· ", " ").Replace("  ", "")} *";
            else
                result += navigation.NavigationStack.LastOrDefault()?.Title;

            result += " " + string.Join(" ", page.ToolbarItems.Select(t => $"[{t.Text}]"));

            return result;
        }

        public static string Render(this ListView listView)
        {
            var result = "";
            if (listView.ItemsSource == null)
                return result;

            result += Render(listView.Header);

            var groups = ListViewCrawler.GetCellGroups(listView);
            foreach (var group in groups)
                result += Render(group);

            result.TrimEnd('\n');
            result += Render(listView.Footer);

            return result;
        }

        static string Render(CellGroup cellGroup)
        {
            string result = "";

            if (cellGroup.Header != null)
                result += Render(cellGroup.Header).TrimStart('\n') + "\n";

            foreach (var cell in cellGroup.Content)
                result += $"- {(cell as TextCell)?.Text + (cell as ViewCell)?.View.Render().Trim()}\n";

            return result;
        }

        static string Render(object stringBindingOrView)
        {
            if (stringBindingOrView is string)
                return stringBindingOrView.ToString() + "\n";
            if (stringBindingOrView is View)
                return (stringBindingOrView as View).Render().TrimStart('\n') + "\n";

            return stringBindingOrView?.ToString() ?? "";
        }
    }
}
