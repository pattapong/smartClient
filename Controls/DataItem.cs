using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace smartRestaurant.Controls
{
	/// <summary>
	/// Summary description for DataItem.
	/// </summary>
	public class DataItem  : Control
	{
		// Fields
		public bool IsHeader;
		public bool Strike;
		public string Text;
		public object Value;

		// Methods
		public DataItem(string text, object value, bool isHeader)
		{
			this.Text = text;
			this.Value = value;
			this.IsHeader = isHeader;
			this.Strike = false;
		}
	}

 

}
