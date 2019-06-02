using System;

namespace smartRestaurant.Controls
{
	public class KeyboardPadEventArgs : EventArgs
	{
		private ImageButton button;

		private char keyChar;

		private int keyCode;

		private string text;

		public ImageButton Button
		{
			get
			{
				return this.button;
			}
		}

		public bool IsControlKey
		{
			get
			{
				if (this.keyCode == KeyboardPad.BUTTON_OK || this.keyCode == KeyboardPad.BUTTON_CANCEL || this.keyCode == KeyboardPad.BUTTON_CAPLOCK || this.keyCode == KeyboardPad.BUTTON_SHIFT)
				{
					return true;
				}
				return this.keyCode == KeyboardPad.BUTTON_BACKSPACE;
			}
		}

		public char KeyChar
		{
			get
			{
				return this.keyChar;
			}
		}

		public int KeyCode
		{
			get
			{
				return this.keyCode;
			}
		}

		public string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				this.text = value;
			}
		}

		public KeyboardPadEventArgs(ImageButton button, char keyChar, int keyCode)
		{
			this.button = button;
			this.keyChar = keyChar;
			this.keyCode = keyCode;
			this.text = null;
		}
	}
}