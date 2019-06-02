using System;
using System.Drawing;

namespace smartRestaurant.Controls
{
	public class ButtonItem
	{
		public string Text;

		public Color ForeColor;

		private object itemValue;

		private bool isLock;

		public bool IsLock
		{
			get
			{
				return this.isLock;
			}
			set
			{
				this.isLock = value;
			}
		}

		public object ObjectValue
		{
			get
			{
				return this.itemValue;
			}
			set
			{
				this.itemValue = value;
			}
		}

		public string Value
		{
			get
			{
				return (string)this.itemValue;
			}
			set
			{
				this.itemValue = value;
			}
		}

		public ButtonItem(string text, string value)
		{
			this.Text = text;
			this.itemValue = value;
			this.ForeColor = Color.Black;
			this.IsLock = false;
		}

		public ButtonItem(string text, object value)
		{
			this.Text = text;
			this.itemValue = value;
			this.ForeColor = Color.Black;
			this.IsLock = false;
		}
	}
}