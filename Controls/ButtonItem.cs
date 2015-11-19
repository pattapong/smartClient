using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;


namespace smartRestaurant.Controls
{
	public class ButtonItem : Control
	{
		// Fields
		public Color ForeColor;
		private bool isLock;
		private object itemValue;
		public string Text;

		// Methods
		public ButtonItem(string text, object value)
		{
			this.Text = text;
			this.itemValue = value;
			this.ForeColor = Color.Black;
			this.IsLock = false;
		}

		public ButtonItem(string text, string value)
		{
			this.Text = text;
			this.itemValue = value;
			this.ForeColor = Color.Black;
			this.IsLock = false;
		}

		// Properties
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
				return (string) this.itemValue;
			}
			set
			{
				this.itemValue = value;
			}
		}
	}
}
