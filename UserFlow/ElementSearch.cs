using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace UserFlow
{
    public static class ElementSearch
    {
        public static List<ElementInfo> Find(this Element element, Predicate<Element> predicate, Predicate<Element> containerPredicate = null)
        {
            var result = new List<ElementInfo>();

            if (containerPredicate != null && !containerPredicate.Invoke(element))
                return result;

            IEnumerable<ElementInfo> empty = new List<ElementInfo>();

            result.AddRange((element as Page)?.ToolbarItems.Where(predicate.Invoke).Select(ElementInfo.FromElement) ?? empty);
            result.AddRange((element as ContentPage)?.Content.Find(predicate, containerPredicate) ?? empty);
            result.AddRange((element as ScrollView)?.Content.Find(predicate, containerPredicate) ?? empty);
            result.AddRange((element as Layout<View>)?.Children.SelectMany(child => child.Find(predicate, containerPredicate)) ?? empty);
            result.AddRange((element as ListView)?.Find(predicate, containerPredicate) ?? empty);

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
                (element as ToolbarItem)?.Text == text ||
                (element as ContentPage)?.Title == text ||
                (element as Button)?.Text == text ||
                (element as Label)?.Text == text ||
                (element as Editor)?.Text == text ||
                (element as Entry)?.Text == text ||
                (element as SearchBar)?.Text == text ||
                ((element as Entry)?.Placeholder == text && string.IsNullOrEmpty((element as Entry)?.Text)) ||
                (element as TextCell)?.Text == text ||
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
            foreach (var item in listView.ItemsSource) {
                var content = listView.ItemTemplate.CreateContent();
                (content as Cell).BindingContext = item;
                if (predicate.Invoke(content as Cell) || ((content as ViewCell)?.View.Find(predicate, containerPredicate).Any() ?? false))
                    result.Add(new ElementInfo {
                        InvokeTap = () => listView.Invoke("NotifyRowTapped", listView.ItemsSource.Cast<object>().ToList().IndexOf(item), null),
                    });
            }
            return result;
        }
    }
}
