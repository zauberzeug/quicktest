using System;
using Xamarin.Forms;

namespace QuickTest
{
    public class ElementInfo
    {
        public Element Element;
        public Action InvokeTap;

        public static ElementInfo FromElement(Element element)
        {
            return new ElementInfo {
                Element = element,
            };
        }
    }
}
