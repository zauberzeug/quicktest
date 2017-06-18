using System;
using Xamarin.Forms;

namespace DemoApp
{
    public static class BinableObjectExtensions {
        
        public static T BindTo<T>(this T target, BindableObject source,
                              BindableProperty sourceProperty, BindableProperty targetProperty,
                              BindingMode mode = BindingMode.Default, IValueConverter converter = null, string stringFormat = null)
        where T : BindableObject {

            if (target.BindingContext != null && target.BindingContext != source)
                Console.WriteLine("Incompatible binding source! (was {0}, will be {1})", target.BindingContext, source);

            target.BindingContext = source;
            target.SetBinding(targetProperty, sourceProperty.PropertyName, mode, converter, stringFormat);
            return target;
        }
    }
}
