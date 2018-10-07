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

            if (element == null) throw new NullReferenceException("can not search for items in null element");

            if (containerPredicate != null && !containerPredicate.Invoke(element)) return result;

            IEnumerable<ElementInfo> empty = new List<ElementInfo>();

            result.AddRange((element as Page)?.ToolbarItems.ToList().Where(predicate.Invoke).Select(ElementInfo.FromElement) ?? empty);
            result.AddRange((element as ContentPage)?.Content.Find(predicate, containerPredicate) ?? empty);
            result.AddRange((element as ContentView)?.Content.Find(predicate, containerPredicate) ?? empty);
            result.AddRange((element as ScrollView)?.Content.Find(predicate, containerPredicate) ?? empty);
            result.AddRange((element as Layout<View>)?.Children.ToList().SelectMany(child => child.Find(predicate, containerPredicate)) ?? empty);
            result.AddRange((element as ListView)?.Find(predicate, containerPredicate) ?? empty);
            result.AddRange((element as ViewCell)?.View?.Find(predicate, containerPredicate) ?? empty);
            result.AddRange((element as TabbedPage)?.CurrentPage?.Find(predicate, containerPredicate) ?? empty);

            if (predicate.Invoke(element))
                result.Add(ElementInfo.FromElement(element));

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
                ((element.Parent is NavigationPage && (element as Page)?.Title == text)) ||
                (element as Button)?.Text == text ||
                (element as Label)?.Text == text ||
                (element as Label)?.FormattedText?.ToString() == text ||
                (element as Editor)?.Text == text ||
                (element as Entry)?.Text == text ||
                (element as SearchBar)?.Text == text ||
                ((element as Entry)?.Placeholder == text && string.IsNullOrEmpty((element as Entry)?.Text)) ||
                (element as TextCell)?.Text == text ||
                ((element.Parent is TabbedPage && (element.Parent as Page)?.Title == text)) ||
                (((element.Parent as TabbedPage)?.Children.Any(p => p.Title == text) ?? false)) ||
                ((element.FindParent<NavigationPage>() != null && (element as ToolbarItem)?.Text == text)) ||
                element?.AutomationId == text;
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

            if (listView.IsGroupingEnabled)
                foreach (var templatedItems in listView.TemplatedItems.Cast<TemplatedItemsList<ItemsView<Cell>, Cell>>())
                    result.AddRange(templatedItems.Find(listView, predicate, containerPredicate));
            else
                result.AddRange(listView.TemplatedItems.Find(listView, predicate, containerPredicate));

            return result;
        }

        static List<ElementInfo> Find(this TemplatedItemsList<ItemsView<Cell>, Cell> tempatedItems, ListView listView, Predicate<Element> predicate, Predicate<Element> containerPredicate)
        {
            var result = new List<ElementInfo>();


            if (tempatedItems.HeaderContent != null)
                result.AddRange(tempatedItems.HeaderContent.Find(predicate, containerPredicate));

            foreach (var cell in tempatedItems) {
                var element = GetElement(predicate, containerPredicate, cell);
                if (element != null)
                    result.Add(new ElementInfo {
                        InvokeTap = () => listView.Invoke("NotifyRowTapped", cell.GetIndex<ItemsView<Cell>, Cell>(), cell),
                        Element = element,
                    });
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

        static Element GetElement(Predicate<Element> predicate, Predicate<Element> containerPredicate, object cell)
        {
            if (predicate.Invoke(cell as Cell)) {
                return cell as Cell;
            }

            var viewCellResults = (cell as ViewCell)?.View.Find(predicate, containerPredicate);
            if (viewCellResults?.Any() ?? false) {
                return viewCellResults.First().Element;
            }

            return null;
        }
    }
}
