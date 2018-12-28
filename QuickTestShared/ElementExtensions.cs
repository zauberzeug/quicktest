using Xamarin.Forms;
using System;

namespace QuickTest
{
    public static class ElementExtensions
    {
        /// <summary>
        /// Returns first XForms.Element with type T in the view hierarchy (self or ancestor).
        /// </summary>
        public static T FindParent<T>(this Element element) where T : Element
        {
            if (element == null)
                throw new NullReferenceException("can not find parent on null reference");
            do
                element = element.Parent;
            while (!(element is T) && element != null);

            return (T)element;
        }
    }
}
