using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace smartRestaurant.Controls
{
	public class KeyboardPad : Control
	{
		// Fields
		public static int BUTTON_BACKSPACE = 0x33;
		public static int BUTTON_CANCEL = 0x35;
		public static int BUTTON_CAPLOCK = 0x1a;
		public static int BUTTON_ENTER = 0x26;
		public static int BUTTON_OK = 0x34;
		public static int BUTTON_SHIFT = 0x27;
		public static int BUTTON_SPACE = 50;
		private ImageButton[] buttons;
		private Image image;
		private Image imageClick;
		private int imageClickIndex;
		private int imageIndex;
		private ImageList imageList;
		private bool isCaplock;
		private bool isShift;
		private string[] keyboardNormalText = new string[] { 
															   "~\n`", "!\n1", "@\n2", "#\n3", "$\n4", "%\n5", "^\n6", "&\n7", "*\n8", "(\n9", ")\n0", "_\n-", "+\n=", "|\n\\", "Q", "W", 
															   "E", "R", "T", "Y", "U", "I", "O", "P", "{\n[", "}\n]", "Cap", "A", "S", "D", "F", "G", 
															   "H", "J", "K", "L", ":\n;", "\"\n'", "Enter", "Shift", "Z", "X", "C", "V", "B", "N", "M", "<\n,", 
															   ">\n.", "?\n/", "Space", "<-", "OK", "Cancel"
														   };
		private TextBox messageBox;
		private Label textLabel;
		private string title;
		public delegate void KeyboardPadEventHandler(object sender, KeyboardPadEventArgs e);

		// Events
		public event KeyboardPadEventHandler PadClick;

		// Methods
		public KeyboardPad()
		{
			base.Width = 0x3a8;
			base.Height = 340;
			this.isCaplock = this.isShift = false;
			this.BuildKeyboardPad();
		}

		private void BuildKeyboardPad()
		{
			base.Controls.Clear();
			if (this.textLabel == null)
			{
				this.textLabel = new Label();
				this.textLabel.Top = 0;
				this.textLabel.Left = 0;
				this.textLabel.Width = base.Width;
				this.textLabel.Height = 40;
				this.textLabel.Text = this.title;
				this.textLabel.BackColor = Color.Black;
				this.textLabel.ForeColor = Color.White;
				this.textLabel.TextAlign = ContentAlignment.MiddleLeft;
			}
			ImageButton[] buttons = this.buttons;
			this.buttons = new ImageButton[this.keyboardNormalText.Length];
			int x = 0;
			int y = 40;
			for (int i = 0; i < this.buttons.Length; i++)
			{
				if ((buttons != null) && (i < buttons.Length))
				{
					this.buttons[i] = buttons[i];
				}
				else
				{
					this.buttons[i] = new ImageButton();
				}
				if (i < this.keyboardNormalText.Length)
				{
					this.buttons[i].Text = this.keyboardNormalText[i];
					if (i < 13)
					{
						x = i * 0x48;
						y = 100;
					}
					else if ((i >= 13) && (i < 0x1a))
					{
						x = (i - 13) * 0x48;
						y = 160;
					}
					else if ((i >= 0x1a) && (i < 0x27))
					{
						x = (i - 0x1a) * 0x48;
						y = 220;
					}
					else if ((i >= 0x27) && (i < 0x34))
					{
						x = (i - 0x27) * 0x48;
						y = 280;
					}
					else if (i >= 0x34)
					{
						x = ((i - 0x34) + 11) * 0x48;
						y = 40;
					}
				}
				this.buttons[i].Location = new Point(x, y);
				this.buttons[i].Width = 0x48;
				if (((i == BUTTON_CAPLOCK) || (i == BUTTON_SHIFT)) || (((i == BUTTON_BACKSPACE) || (i == BUTTON_ENTER)) || (i == BUTTON_SPACE)))
				{
					this.buttons[i].Blue = 0.5f;
				}
				else if ((i == BUTTON_OK) || (i == BUTTON_CANCEL))
				{
					this.buttons[i].Blue = 1.75f;
				}
				else
				{
					this.buttons[i].Blue = 2f;
				}
				if ((i == BUTTON_OK) || (i == BUTTON_CANCEL))
				{
					this.buttons[i].Red = 1.75f;
				}
				this.buttons[i].ObjectValue = i;
				this.buttons[i].Click += new EventHandler(this.Button_Click);
				this.buttons[i].DoubleClick += new EventHandler(this.Button_Click);
			}
			if (this.messageBox == null)
			{
				this.messageBox = new TextBox();
				this.messageBox.BorderStyle = BorderStyle.FixedSingle;
				this.messageBox.Top = 40;
				this.messageBox.Left = 0;
				this.messageBox.Width = base.Width - 0x90;
				this.messageBox.Height = 60;
				this.messageBox.Multiline = true;
				this.messageBox.AcceptsReturn = true;
				this.messageBox.WordWrap = true;
			}
			base.Controls.Add(this.textLabel);
			base.Controls.Add(this.messageBox);
			base.Controls.AddRange(this.buttons);
		}

		private void Button_Click(object sender, EventArgs e)
		{
			if ((this.buttons != null) && (this.buttons.Length > 0))
			{
				char ch;
				int objectValue = (int) ((ImageButton) sender).ObjectValue;
				if (objectValue == BUTTON_SHIFT)
				{
					this.isShift = !this.isShift;
					ch = ' ';
				}
				else if (objectValue == BUTTON_CAPLOCK)
				{
					this.isCaplock = !this.isCaplock;
					ch = ' ';
				}
				else if ((objectValue == BUTTON_OK) || (objectValue == BUTTON_CANCEL))
				{
					ch = ' ';
				}
				else
				{
					if ((objectValue == BUTTON_BACKSPACE) || (objectValue == BUTTON_SPACE))
					{
						ch = ' ';
					}
					else if (objectValue == BUTTON_ENTER)
					{
						ch = '\n';
					}
					else
					{
						string text = this.buttons[objectValue].Text;
						if (text.Length > 1)
						{
							if (this.isShift || this.isCaplock)
							{
								ch = text[0];
							}
							else
							{
								ch = text[2];
							}
						}
						else if (this.isShift || this.isCaplock)
						{
							ch = text[0];
						}
						else
						{
							ch = text.ToLower()[0];
						}
					}
					if (this.isShift)
					{
						this.isShift = false;
					}
				}
				this.SetKeyboardLock();
				this.OnPadClick(new KeyboardPadEventArgs(this.buttons[objectValue], ch, objectValue));
			}
		}

		protected virtual void OnPadClick(KeyboardPadEventArgs e)
		{
			StringBuilder builder = new StringBuilder(this.messageBox.Text);
			int selectionStart = this.messageBox.SelectionStart;
			int num2 = 1;
			if (!e.IsControlKey)
			{
				if (this.messageBox.SelectionLength > 0)
				{
					builder.Remove(this.messageBox.SelectionStart, this.messageBox.SelectionLength);
				}
				builder.Insert(this.messageBox.SelectionStart, e.KeyChar);
				this.messageBox.Lines = builder.Replace("\r", "").ToString().Split(new char[] { '\n' });
				for (int i = 0; (i <= selectionStart) && (i < builder.Length); i++)
				{
					if (builder[i] == '\n')
					{
						num2++;
					}
				}
				selectionStart += num2;
			}
			else if (e.KeyCode == BUTTON_BACKSPACE)
			{
				if (this.messageBox.SelectionLength > 0)
				{
					builder.Remove(this.messageBox.SelectionStart, this.messageBox.SelectionLength);
					this.messageBox.Text = builder.ToString();
				}
				else if (this.messageBox.SelectionStart > 0)
				{
					builder.Remove(this.messageBox.SelectionStart - 1, 1);
					this.messageBox.Text = builder.ToString();
					selectionStart--;
				}
			}
			this.messageBox.SelectionStart = selectionStart;
			this.messageBox.ScrollToCaret();
			this.messageBox.Focus();
			if (this.PadClick != null)
			{
				if (e.KeyCode == BUTTON_OK)
				{
					e.Text = string.Join("\n", this.messageBox.Lines);
				}
				this.PadClick(this, e);
			}
		}

		private void SetKeyboardLock()
		{
			if (this.isShift)
			{
				this.buttons[BUTTON_SHIFT].Green = 0.5f;
			}
			else
			{
				this.buttons[BUTTON_SHIFT].Green = 1f;
			}
			if (this.isCaplock)
			{
				this.buttons[BUTTON_CAPLOCK].Green = 0.5f;
			}
			else
			{
				this.buttons[BUTTON_CAPLOCK].Green = 1f;
			}
			this.buttons[BUTTON_SHIFT].Refresh();
			this.buttons[BUTTON_CAPLOCK].Refresh();
		}

		public bool ShowKeyboard()
		{
			base.Visible = true;
			return this.messageBox.Focus();
		}

		// Properties
		public Image Image
		{
			get
			{
				return this.image;
			}
			set
			{
				this.image = value;
				for (int i = 0; i < this.buttons.Length; i++)
				{
					this.buttons[i].Image = this.image;
				}
			}
		}

		public Image ImageClick
		{
			get
			{
				return this.imageClick;
			}
			set
			{
				this.imageClick = value;
				for (int i = 0; i < this.buttons.Length; i++)
				{
					this.buttons[i].ImageClick = this.imageClick;
				}
			}
		}

		public int ImageClickIndex
		{
			get
			{
				return this.imageClickIndex;
			}
			set
			{
				this.imageClickIndex = value;
				if (this.imageList != null)
				{
					this.ImageClick = this.imageList.Images[this.imageClickIndex];
				}
			}
		}

		public int ImageIndex
		{
			get
			{
				return this.imageIndex;
			}
			set
			{
				this.imageIndex = value;
				if (this.imageList != null)
				{
					this.Image = this.imageList.Images[this.imageIndex];
				}
			}
		}

		public ImageList ImageList
		{
			get
			{
				return this.imageList;
			}
			set
			{
				this.imageList = value;
			}
		}

		public override string Text
		{
			get
			{
				return string.Join("\n", this.messageBox.Lines);
			}
			set
			{
				if (value != null)
				{
					this.messageBox.Lines = value.Split(new char[] { '\n' });
					if (this.messageBox.Lines.Length > 1)
					{
						this.messageBox.SelectionStart = (value.Length + this.messageBox.Lines.Length) - 1;
					}
					else
					{
						this.messageBox.SelectionStart = value.Length;
					}
				}
				else
				{
					this.messageBox.Text = null;
					this.messageBox.SelectionStart = 0;
				}
				this.messageBox.SelectionLength = 0;
			}
		}

		public string Title
		{
			get
			{
				return this.title;
			}
			set
			{
				this.title = value;
				this.textLabel.Text = this.title;
			}
		}
	}
	public delegate void KeyboardPadEventHandler(object sender, KeyboardPadEventArgs e);

 

	public class KeyboardPadEventArgs : EventArgs
	{
		// Fields
		private ImageButton button;
		private char keyChar;
		private int keyCode;
		private string text;


		// Methods
		public KeyboardPadEventArgs(ImageButton button, char keyChar, int keyCode)
		{
			this.button = button;
			this.keyChar = keyChar;
			this.keyCode = keyCode;
			this.text = null;
		}

		// Properties
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
				if (((this.keyCode != KeyboardPad.BUTTON_OK) && (this.keyCode != KeyboardPad.BUTTON_CANCEL)) && ((this.keyCode != KeyboardPad.BUTTON_CAPLOCK) && (this.keyCode != KeyboardPad.BUTTON_SHIFT)))
				{
					return (this.keyCode == KeyboardPad.BUTTON_BACKSPACE);
				}
				return true;
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
	}
}
