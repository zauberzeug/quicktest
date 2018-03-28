using System;
using Xamarin.Forms;
namespace QuickTest
{
    public static class ElementExtensions
    {
        /// <summary>
        /// Returns first XForms.Element with type T in the view hierarchy (self or ancestor).
        /// </summary>
        public static T FindParent<T>(this Element element) where T : Element
        {
            while (!(element is T) && element != null)
                element = element.Parent;

            return (T)element;
        }
    }
}
