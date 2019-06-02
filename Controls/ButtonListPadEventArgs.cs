using System;

namespace smartRestaurant.Controls
{
	public class ButtonListPadEventArgs : EventArgs
	{
		private ImageButton button;

		private object buttonValue;

		private int index;

		public ImageButton Button
		{
			get
			{
				return this.button;
			}
		}

		public int Index
		{
			get
			{
				return this.index;
			}
		}

		public object ObjectValue
		{
			get
			{
				return this.buttonValue;
			}
		}

		public string Value
		{
			get
			{
				return (string)this.buttonValue;
			}
		}

		public ButtonListPadEventArgs(ImageButton button, int index)
		{
			this.button = button;
			if (button.ObjectValue == null)
			{
				this.buttonValue = null;
			}
			else
			{
				this.buttonValue = button.ObjectValue;
			}
			this.index = index;
		}
	}
}