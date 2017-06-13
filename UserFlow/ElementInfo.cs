using System;
using Xamarin.Forms;

namespace UserFlow
{
	public class ElementInfo
	{
		public Element Element;
		public Action InvokeTapGestures;
		public ListView EnclosingListView;
		public int ListViewIndex;

		public static ElementInfo FromElement(Element element)
		{
			return new ElementInfo {
				Element = element,
			};
		}
	}
}
