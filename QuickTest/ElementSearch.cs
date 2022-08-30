using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace QuickTest
{
    public static class ElementSearch
    {
        public static List<ElementInfo> Find(this Element element, Predicate<Element> predicate, Predicate<Element> containerPredicate = null)
        {
            var result = new List<ElementInfo>();
            IEnumerable<ElementInfo> empty = new List<ElementInfo>();

            if (element == null) throw new NullReferenceException("cannot search for items in null element");

            if (containerPredicate != null && !containerPredicate.Invoke(element)) return result;

            if (element is Page page) {
                result.AddRange(FindInTitle(page, predicate, containerPredicate) ?? empty);
                result.AddRange(FindInToolbarItems(page, predicate, containerPredicate) ?? empty);
                if (page.Parent is TabbedPage tabbedPage)
                    result.AddRange(FindInTabs(tabbedPage, predicate, containerPredicate));
            } else {
                if (predicate.Invoke(element))
                    result.Add(ElementInfo.FromElement(element));
            }

            result.AddRange((element as ContentPage)?.Content?.Find(predicate, containerPredicate) ?? empty);
            result.AddRange((element as ContentView)?.Content.Find(predicate, containerPredicate) ?? empty);
            result.AddRange((element as ScrollView)?.Content.Find(predicate, containerPredicate) ?? empty);
            result.AddRange((element as Layout<View>)?.Children.ToList().SelectMany(child => child.Find(predicate, containerPredicate)) ?? empty);
            result.AddRange((element as ListView)?.Find(predicate, containerPredicate) ?? empty);
            result.AddRange((element as ViewCell)?.View?.Find(predicate, containerPredicate) ?? empty);

            AddTapGestureRecognizers(element, result);

            return result;
        }

        public static List<ElementInfo> Find(this Element element, string text)
        {
            if (text == null)
                throw new InvalidOperationException("Can't search for (null) text");

            return element.Find(e => e.HasText(text), c => (c as VisualElement)?.IsVisible ?? true);
        }

        public static bool HasText(this Element element, string text)
        {
            return
                (element as Page)?.Title == text ||
                (element as Button)?.Text == text ||
                (element as Label)?.Text == text ||
                (element as Label)?.FormattedText?.ToString() == text ||
                (element as Editor)?.Text == text ||
                (element as Entry)?.Text == text ||
                (element as SearchBar)?.Text == text ||
                (element as Picker)?.Title == text ||
                (element as Picker)?.SelectedItem?.ToString() == text ||
                ((element as Entry)?.Placeholder == text && string.IsNullOrEmpty((element as Entry)?.Text)) ||
                (element as Image)?.Source?.AutomationId == text ||
                (element as TextCell)?.Text == text ||
                (element is Page page && element.Parent is TabbedPage && (page.Title == text || page.AutomationId == text || (page.IconImageSource as FileImageSource)?.File == text)) ||
                (element as ToolbarItem)?.Text == text ||
                element?.AutomationId == text;
        }

        // Title or title view is only visible if there is a navigation bar.
        // In case of (nested) tabbed pages, only the (outermost) tabbed page title or title view is visible.
        static List<ElementInfo> FindInTitle(Page page, Predicate<Element> predicate, Predicate<Element> containerPredicate = null)
        {
            var tabbedParent = page.Parent as TabbedPage;
            if (tabbedParent != null)
                while (tabbedParent.Parent is TabbedPage)
                    tabbedParent = tabbedParent.Parent as TabbedPage;

            if (tabbedParent != null)
                page = tabbedParent;

            var result = new List<ElementInfo>();

            if (containerPredicate != null && !containerPredicate.Invoke(page))
                return result;

            // Title is only visible if inside navigation page
            if (page.FindParent<NavigationPage>() == null)
                return result;

            var titleView = NavigationPage.GetTitleView(page);
            if (titleView != null)
                result.AddRange(titleView.Find(predicate, containerPredicate));
            else if (predicate(page))
                result.Add(ElementInfo.FromElement(page));
            return result;
        }

        // Tabbed pages accumulate toolbar items from themselves and their current page (even in the nested case)
        static List<ElementInfo> FindInToolbarItems(Page page, Predicate<Element> predicate, Predicate<Element> containerPredicate = null)
        {
            var result = new List<ElementInfo>();

            if (containerPredicate != null && !containerPredicate(page))
                return result;

            // Toolbar items are only visible if inside navigation page
            if (page.FindParent<NavigationPage>() == null)
                return result;

            result.AddRange(page.ToolbarItems.ToList().Where(predicate.Invoke).Select(ElementInfo.FromElement));

            var tabbedParent = page.Parent as TabbedPage;
            if (tabbedParent != null)
                result.AddRange(FindInToolbarItems(tabbedParent, predicate, containerPredicate));

            return result;
        }

        static List<ElementInfo> FindInTabs(TabbedPage tabbedPage, Predicate<Element> predicate, Predicate<Element> containerPredicate = null)
        {
            var result = new List<ElementInfo>();

            if (containerPredicate != null && !containerPredicate.Invoke(tabbedPage))
                return result;

            var matchingChildren = tabbedPage.Children.Where(p => predicate(p));

            result.AddRange(matchingChildren.Select(p => new ElementInfo {
                Element = p,
                InvokeTap = () => tabbedPage.CurrentPage = p,
            }));

            if (tabbedPage.Parent is TabbedPage parentTabbedPage)
                result.AddRange(FindInTabs(parentTabbedPage, predicate, containerPredicate));

            return result;
        }

        static void AddTapGestureRecognizers(Element sourceElement, IEnumerable<ElementInfo> result)
        {
            var tapGestureRecognizers = (sourceElement as View)?.GestureRecognizers.OfType<TapGestureRecognizer>().ToList();

            if (tapGestureRecognizers == null || !tapGestureRecognizers.Any())
                return;

            foreach (var info in result.Where(i => i.InvokeTap == null))
                info.InvokeTap = () => tapGestureRecognizers.ForEach(r => r.Invoke("SendTapped", sourceElement));
        }

        public static List<ElementInfo> Find(this ListView listView, Predicate<Element> predicate, Predicate<Element> containerPredicate)
        {
            var result = new List<ElementInfo>();

            result.AddRange(Find(listView.Header, predicate, containerPredicate));
            result.AddRange(Find(listView.Footer, predicate, containerPredicate));

            if (listView.ItemsSource == null)
                return result;

            var groups = ListViewCrawler.GetCellGroups(listView);
            foreach (var group in groups)
                result.AddRange(Find(group, listView, predicate, containerPredicate));

            return result;
        }

        static List<ElementInfo> Find(CellGroup cellGroup, ListView listView, Predicate<Element> predicate, Predicate<Element> containerPredicate)
        {
            var result = new List<ElementInfo>();


            if (cellGroup.Header != null)
                result.AddRange(cellGroup.Header.Find(predicate, containerPredicate));

            foreach (var cell in cellGroup.Content) {
                var info = GetElementInfo(predicate, containerPredicate, cell);
                if (info != null) {
                    if (info.InvokeTap == null)
                        info.InvokeTap = () => listView.Invoke("NotifyRowTapped", cell.GetIndex<ItemsView<Cell>, Cell>(), cell);

                    result.Add(info);
                }
            }
            return result;
        }

        static List<ElementInfo> Find(object stringBindingOrView, Predicate<Element> predicate, Predicate<Element> containerPredicate)
        {
            if (stringBindingOrView is string)
                return new Label { Text = stringBindingOrView.ToString() }.Find(predicate, containerPredicate);
            if (stringBindingOrView is View)
                return (stringBindingOrView as View).Find(predicate, containerPredicate);

            return new List<ElementInfo>();
        }

        static ElementInfo GetElementInfo(Predicate<Element> predicate, Predicate<Element> containerPredicate, object cell)
        {
            if (predicate.Invoke(cell as Cell)) {
                return new ElementInfo { Element = cell as Cell };
            }

            var viewCellResults = (cell as ViewCell)?.View.Find(predicate, containerPredicate);
            if (viewCellResults?.Any() ?? false)
                return viewCellResults.First();

            return null;
        }
    }
}
