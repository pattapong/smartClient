using System;

namespace smartRestaurant.Controls
{
	public class NumberPadEventArgs : EventArgs
	{
		private ImageButton button;

		private int number;

		public ImageButton Button
		{
			get
			{
				return this.button;
			}
		}

		public bool IsCancel
		{
			get
			{
				return this.number == NumberPad.BUTTON_CANCEL;
			}
		}

		public bool IsEnter
		{
			get
			{
				return this.number == NumberPad.BUTTON_ENTER;
			}
		}

		public bool IsNumeric
		{
			get
			{
				return this.number < 10;
			}
		}

		public int Number
		{
			get
			{
				return this.number;
			}
		}

		public NumberPadEventArgs(ImageButton button, int number)
		{
			this.button = button;
			this.number = number;
		}
	}
}