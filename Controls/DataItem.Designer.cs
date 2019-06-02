using System;

namespace smartRestaurant.Controls
{
	public class DataItem
	{
		public string Text;

		public object Value;

		public bool IsHeader;

		public bool Strike;

		public DataItem(string text, object value, bool isHeader)
		{
			this.Text = text;
			this.Value = value;
			this.IsHeader = isHeader;
			this.Strike = false;
		}
	}
}