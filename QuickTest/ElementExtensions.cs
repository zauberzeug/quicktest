using System;
using Xamarin.Forms;
namespace QuickTest
{
    public static class ElementExtensions
    {
        /// <summary>
        /// Returns parent XForms.Element with type T.
        /// </summary>
        public static T GetParent<T>(this Element result) where T : Element
        {
            while (!(result is T) && result != null)
                result = result.Parent;

            return (T)result;
        }
    }
}
