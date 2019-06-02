using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;

namespace smartRestaurant.Controls
{
	public class KeyboardPad : Control
	{
		public static int BUTTON_OK;

		public static int BUTTON_CANCEL;

		public static int BUTTON_CAPLOCK;

		public static int BUTTON_ENTER;

		public static int BUTTON_SHIFT;

		public static int BUTTON_SPACE;

		public static int BUTTON_BACKSPACE;

		private string[] keyboardNormalText = new string[] { "~\n`", "!\n1", "@\n2", "#\n3", "$\n4", "%\n5", "^\n6", "&\n7", "*\n8", "(\n9", ")\n0", "_\n-", "+\n=", "|\n\\", "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P", "{\n[", "}\n]", "Cap", "A", "S", "D", "F", "G", "H", "J", "K", "L", ":\n;", "\"\n'", "Enter", "Shift", "Z", "X", "C", "V", "B", "N", "M", "<\n,", ">\n.", "?\n/", "Space", "<-", "OK", "Cancel" };

		private bool isCaplock;

		private bool isShift;

		private Label textLabel;

		private TextBox messageBox;

		private System.Windows.Forms.ImageList imageList;

		private System.Drawing.Image image;

		private System.Drawing.Image imageClick;

		private int imageIndex;

		private int imageClickIndex;

		private ImageButton[] buttons;

		private string title;

		public System.Drawing.Image Image
		{
			get
			{
				return this.image;
			}
			set
			{
				this.image = value;
				for (int i = 0; i < (int)this.buttons.Length; i++)
				{
					this.buttons[i].Image = this.image;
				}
			}
		}

		public System.Drawing.Image ImageClick
		{
			get
			{
				return this.imageClick;
			}
			set
			{
				this.imageClick = value;
				for (int i = 0; i < (int)this.buttons.Length; i++)
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

		public System.Windows.Forms.ImageList ImageList
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
				if (value == null)
				{
					this.messageBox.Text = null;
					this.messageBox.SelectionStart = 0;
				}
				else
				{
					TextBox textBox = this.messageBox;
					char[] chrArray = new char[] { '\n' };
					textBox.Lines = value.Split(chrArray);
					if ((int)this.messageBox.Lines.Length <= 1)
					{
						this.messageBox.SelectionStart = value.Length;
					}
					else
					{
						this.messageBox.SelectionStart = value.Length + (int)this.messageBox.Lines.Length - 1;
					}
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

		static KeyboardPad()
		{
			KeyboardPad.BUTTON_OK = 52;
			KeyboardPad.BUTTON_CANCEL = 53;
			KeyboardPad.BUTTON_CAPLOCK = 26;
			KeyboardPad.BUTTON_ENTER = 38;
			KeyboardPad.BUTTON_SHIFT = 39;
			KeyboardPad.BUTTON_SPACE = 50;
			KeyboardPad.BUTTON_BACKSPACE = 51;
		}

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
				this.textLabel = new Label()
				{
					Top = 0,
					Left = 0,
					Width = base.Width,
					Height = 40,
					Text = this.title,
					BackColor = Color.Black,
					ForeColor = Color.White,
					TextAlign = ContentAlignment.MiddleLeft
				};
			}
			ImageButton[] imageButtonArray = this.buttons;
			this.buttons = new ImageButton[(int)this.keyboardNormalText.Length];
			int num = 0;
			int num1 = 40;
			for (int i = 0; i < (int)this.buttons.Length; i++)
			{
				if (imageButtonArray == null || i >= (int)imageButtonArray.Length)
				{
					this.buttons[i] = new ImageButton();
				}
				else
				{
					this.buttons[i] = imageButtonArray[i];
				}
				if (i < (int)this.keyboardNormalText.Length)
				{
					this.buttons[i].Text = this.keyboardNormalText[i];
					if (i < 13)
					{
						num = i * 72;
						num1 = 100;
					}
					else if (i >= 13 && i < 26)
					{
						num = (i - 13) * 72;
						num1 = 160;
					}
					else if (i >= 26 && i < 39)
					{
						num = (i - 26) * 72;
						num1 = 220;
					}
					else if (i >= 39 && i < 52)
					{
						num = (i - 39) * 72;
						num1 = 280;
					}
					else if (i >= 52)
					{
						num = (i - 52 + 11) * 72;
						num1 = 40;
					}
				}
				this.buttons[i].Location = new Point(num, num1);
				this.buttons[i].Width = 72;
				if (i == KeyboardPad.BUTTON_CAPLOCK || i == KeyboardPad.BUTTON_SHIFT || i == KeyboardPad.BUTTON_BACKSPACE || i == KeyboardPad.BUTTON_ENTER || i == KeyboardPad.BUTTON_SPACE)
				{
					this.buttons[i].Blue = 0.5f;
				}
				else if (i == KeyboardPad.BUTTON_OK || i == KeyboardPad.BUTTON_CANCEL)
				{
					this.buttons[i].Blue = 1.75f;
				}
				else
				{
					this.buttons[i].Blue = 2f;
				}
				if (i == KeyboardPad.BUTTON_OK || i == KeyboardPad.BUTTON_CANCEL)
				{
					this.buttons[i].Red = 1.75f;
				}
				this.buttons[i].ObjectValue = i;
				this.buttons[i].Click += new EventHandler(this.Button_Click);
				this.buttons[i].DoubleClick += new EventHandler(this.Button_Click);
			}
			if (this.messageBox == null)
			{
				this.messageBox = new TextBox()
				{
					BorderStyle = BorderStyle.FixedSingle,
					Top = 40,
					Left = 0,
					Width = base.Width - 144,
					Height = 60,
					Multiline = true,
					AcceptsReturn = true,
					WordWrap = true
				};
			}
			base.Controls.Add(this.textLabel);
			base.Controls.Add(this.messageBox);
			base.Controls.AddRange(this.buttons);
		}

		private void Button_Click(object sender, EventArgs e)
		{
			char chr;
			if (this.buttons != null && (int)this.buttons.Length > 0)
			{
				int objectValue = (int)((ImageButton)sender).ObjectValue;
				if (objectValue == KeyboardPad.BUTTON_SHIFT)
				{
					this.isShift = !this.isShift;
					chr = ' ';
				}
				else if (objectValue == KeyboardPad.BUTTON_CAPLOCK)
				{
					this.isCaplock = !this.isCaplock;
					chr = ' ';
				}
				else if (objectValue == KeyboardPad.BUTTON_OK || objectValue == KeyboardPad.BUTTON_CANCEL)
				{
					chr = ' ';
				}
				else
				{
					if (objectValue == KeyboardPad.BUTTON_BACKSPACE || objectValue == KeyboardPad.BUTTON_SPACE)
					{
						chr = ' ';
					}
					else if (objectValue != KeyboardPad.BUTTON_ENTER)
					{
						string text = this.buttons[objectValue].Text;
						if (text.Length <= 1)
						{
							chr = (this.isShift || this.isCaplock ? text[0] : text.ToLower()[0]);
						}
						else
						{
							chr = (this.isShift || this.isCaplock ? text[0] : text[2]);
						}
					}
					else
					{
						chr = '\n';
					}
					if (this.isShift)
					{
						this.isShift = false;
					}
				}
				this.SetKeyboardLock();
				this.OnPadClick(new KeyboardPadEventArgs(this.buttons[objectValue], chr, objectValue));
			}
		}

		protected virtual void OnPadClick(KeyboardPadEventArgs e)
		{
			StringBuilder stringBuilder = new StringBuilder(this.messageBox.Text);
			int selectionStart = this.messageBox.SelectionStart;
			int num = 1;
			if (!e.IsControlKey)
			{
				if (this.messageBox.SelectionLength > 0)
				{
					stringBuilder.Remove(this.messageBox.SelectionStart, this.messageBox.SelectionLength);
				}
				stringBuilder.Insert(this.messageBox.SelectionStart, e.KeyChar);
				TextBox textBox = this.messageBox;
				string str = stringBuilder.Replace("\r", "").ToString();
				char[] chrArray = new char[] { '\n' };
				textBox.Lines = str.Split(chrArray);
				for (int i = 0; i <= selectionStart && i < stringBuilder.Length; i++)
				{
					if (stringBuilder[i] == '\n')
					{
						num++;
					}
				}
				selectionStart += num;
			}
			else if (e.KeyCode == KeyboardPad.BUTTON_BACKSPACE)
			{
				if (this.messageBox.SelectionLength > 0)
				{
					stringBuilder.Remove(this.messageBox.SelectionStart, this.messageBox.SelectionLength);
					this.messageBox.Text = stringBuilder.ToString();
				}
				else if (this.messageBox.SelectionStart > 0)
				{
					stringBuilder.Remove(this.messageBox.SelectionStart - 1, 1);
					this.messageBox.Text = stringBuilder.ToString();
					selectionStart--;
				}
			}
			this.messageBox.SelectionStart = selectionStart;
			this.messageBox.ScrollToCaret();
			this.messageBox.Focus();
			if (this.PadClick != null)
			{
				if (e.KeyCode == KeyboardPad.BUTTON_OK)
				{
					e.Text = string.Join("\n", this.messageBox.Lines);
				}
				this.PadClick(this, e);
			}
		}

		private void SetKeyboardLock()
		{
			if (!this.isShift)
			{
				this.buttons[KeyboardPad.BUTTON_SHIFT].Green = 1f;
			}
			else
			{
				this.buttons[KeyboardPad.BUTTON_SHIFT].Green = 0.5f;
			}
			if (!this.isCaplock)
			{
				this.buttons[KeyboardPad.BUTTON_CAPLOCK].Green = 1f;
			}
			else
			{
				this.buttons[KeyboardPad.BUTTON_CAPLOCK].Green = 0.5f;
			}
			this.buttons[KeyboardPad.BUTTON_SHIFT].Refresh();
			this.buttons[KeyboardPad.BUTTON_CAPLOCK].Refresh();
		}

		public bool ShowKeyboard()
		{
			base.Visible = true;
			return this.messageBox.Focus();
		}

		public event KeyboardPadEventHandler PadClick;
	}
}