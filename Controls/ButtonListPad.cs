using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;


namespace smartRestaurant.Controls
{
	public class ButtonListPad : Control
	{
		// Fields
		private bool autoRefresh;
		private float blue;
		private ImageButton[] buttons;
		private int column;
		private float green;
		private Image image;
		private Image imageClick;
		private int imageClickIndex;
		private int imageIndex;
		private ImageList imageList;
		private ArrayList items;
		private int itemStart;
		private int padding;
		private bool pageEnable;
		private float red;
		private int row;
		// Events
		public event ButtonListPadEventHandler PadClick;

		public event ButtonListPadEventHandler PageChange;

		// Methods
		public ButtonListPad()
		{
			this.red = this.green = this.blue = 1f;
			this.items = new ArrayList();
			this.itemStart = 0;
			this.pageEnable = false;
			this.autoRefresh = true;
			this.BuildButtonList();
		}

		private void BuildButtonList()
		{
			int num3 = this.row * this.column;
			if (num3 == 0)
			{
				this.buttons = null;
			}
			else
			{
				base.Controls.Clear();
				ImageButton[] buttons = this.buttons;
				this.buttons = new ImageButton[num3];
				EventHandler handler = new EventHandler(this.Button_Click);
				for (int i = 0; i < num3; i++)
				{
					if ((buttons != null) && (i < buttons.Length))
					{
						this.buttons[i] = buttons[i];
					}
					else
					{
						this.buttons[i] = new ImageButton();
						this.buttons[i].Click += handler;
						this.buttons[i].DoubleClick += handler;
					}
					this.buttons[i].Image = this.image;
					this.buttons[i].ImageClick = this.imageClick;
					this.buttons[i].Width = this.imageClick.Width;
					this.buttons[i].Height = this.imageClick.Height;
					int num = (i % this.column) * (this.buttons[i].Width + this.padding);
					int num2 = (i / this.column) * (this.buttons[i].Height + this.padding);
					this.buttons[i].Left = num;
					this.buttons[i].Top = num2;
				}
				base.Controls.AddRange(this.buttons);
				base.Width = ((this.buttons[0].Width + this.padding) * this.column) - this.padding;
				base.Height = ((this.buttons[0].Height + this.padding) * this.row) - this.padding;
			}
		}

		private void Button_Click(object sender, EventArgs e)
		{
			if (sender is ImageButton)
			{
				ImageButton button = (ImageButton) sender;
				int index = -1;
				for (int i = 0; i < this.buttons.Length; i++)
				{
					if (button == this.buttons[i])
					{
						index = i;
						break;
					}
				}
				if (index >= 0)
				{
					if (this.pageEnable)
					{
						if (index == 0)
						{
							this.itemStart -= this.buttons.Length - 2;
							if (this.itemStart < 0)
							{
								this.itemStart = (this.items.Count / (this.buttons.Length - 2)) * (this.buttons.Length - 2);
							}
							this.OnPageChange(new ButtonListPadEventArgs(button, index));
							return;
						}
						if (index == (this.buttons.Length - 1))
						{
							this.itemStart += this.buttons.Length - 2;
							if (this.itemStart >= this.items.Count)
							{
								this.itemStart = 0;
							}
							this.OnPageChange(new ButtonListPadEventArgs(button, index));
							return;
						}
						index += this.itemStart - 1;
					}
					this.OnPadClick(new ButtonListPadEventArgs(button, index));
				}
			}
		}

		public int GetPosition(int index)
		{
			if (!this.pageEnable)
			{
				return index;
			}
			if (index < this.itemStart)
			{
				return -1;
			}
			if (index >= (this.itemStart + (this.buttons.Length - 2)))
			{
				return -1;
			}
			return ((index - this.itemStart) + 1);
		}

		protected virtual void OnPadClick(ButtonListPadEventArgs e)
		{
			if (this.PadClick != null)
			{
				this.PadClick(this, e);
			}
		}

		protected virtual void OnPageChange(ButtonListPadEventArgs e)
		{
			if (this.PageChange != null)
			{
				this.PageChange(this, e);
			}
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			this.SetButtonValue();
		}

		public void SetButtonValue()
		{
			int itemStart = this.itemStart;
			if (this.items != null)
			{
				int num3;
				int length;
				if (this.items.Count > this.buttons.Length)
				{
					num3 = 1;
					length = this.buttons.Length - 1;
					this.buttons[0].Text = "<<";
					this.buttons[0].ObjectValue = null;
					this.buttons[this.buttons.Length - 1].Text = ">>";
					this.buttons[this.buttons.Length - 1].ObjectValue = null;
					this.pageEnable = true;
				}
				else
				{
					num3 = 0;
					length = this.buttons.Length;
					this.pageEnable = false;
				}
				for (int i = num3; i < length; i++)
				{
					if (((this.items != null) && (itemStart < this.items.Count)) && (this.items[itemStart] is ButtonItem))
					{
						ButtonItem item = (ButtonItem) this.items[itemStart];
						this.buttons[i].Text = item.Text;
						this.buttons[i].ObjectValue = item.ObjectValue;
						this.buttons[i].ForeColor = item.ForeColor;
						this.buttons[i].IsLock = item.IsLock;
					}
					else
					{
						this.buttons[i].Text = null;
						this.buttons[i].ObjectValue = null;
						this.buttons[i].IsLock = false;
					}
					itemStart++;
				}
			}
		}

		public void SetLock(int pos, bool isLock)
		{
			if ((this.buttons != null) && (pos < this.buttons.Length))
			{
				this.buttons[pos].IsLock = isLock;
				if (this.autoRefresh)
				{
					this.buttons[pos].Refresh();
				}
			}
		}

		public void SetMatrix(int pos, float red, float green, float blue)
		{
			if ((this.buttons != null) && (pos < this.buttons.Length))
			{
				this.buttons[pos].Red = red;
				this.buttons[pos].Green = green;
				this.buttons[pos].Blue = blue;
				if (this.autoRefresh)
				{
					this.buttons[pos].Refresh();
				}
			}
		}

		// Properties
		public bool AutoRefresh
		{
			get
			{
				return this.autoRefresh;
			}
			set
			{
				this.autoRefresh = value;
				if (this.autoRefresh)
				{
					this.Refresh();
				}
			}
		}

		public float Blue
		{
			get
			{
				return this.blue;
			}
			set
			{
				this.blue = value;
				if (this.buttons != null)
				{
					for (int i = 0; i < this.buttons.Length; i++)
					{
						this.buttons[i].Blue = this.blue;
					}
					if (this.autoRefresh)
					{
						this.Refresh();
					}
				}
			}
		}

		public ImageButton[] Buttons
		{
			get
			{
				return this.buttons;
			}
		}

		public int Column
		{
			get
			{
				return this.column;
			}
			set
			{
				this.column = value;
				this.BuildButtonList();
			}
		}

		public float Green
		{
			get
			{
				return this.green;
			}
			set
			{
				this.green = value;
				if (this.buttons != null)
				{
					for (int i = 0; i < this.buttons.Length; i++)
					{
						this.buttons[i].Green = this.green;
					}
					if (this.autoRefresh)
					{
						this.Refresh();
					}
				}
			}
		}

		public Image Image
		{
			get
			{
				return this.image;
			}
			set
			{
				this.image = value;
				if (this.buttons != null)
				{
					for (int i = 0; i < this.buttons.Length; i++)
					{
						this.buttons[i].Image = this.image;
					}
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
				if (this.buttons != null)
				{
					for (int i = 0; i < this.buttons.Length; i++)
					{
						this.buttons[i].ImageClick = this.imageClick;
					}
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

		public ArrayList Items
		{
			get
			{
				return this.items;
			}
		}

		public int ItemStart
		{
			get
			{
				return this.itemStart;
			}
			set
			{
				this.itemStart = value;
				if (this.autoRefresh)
				{
					this.Refresh();
				}
			}
		}

		public int Padding
		{
			get
			{
				return this.padding;
			}
			set
			{
				this.padding = value;
				this.BuildButtonList();
			}
		}

		public float Red
		{
			get
			{
				return this.red;
			}
			set
			{
				this.red = value;
				if (this.buttons != null)
				{
					for (int i = 0; i < this.buttons.Length; i++)
					{
						this.buttons[i].Red = this.red;
					}
					if (this.autoRefresh)
					{
						this.Refresh();
					}
				}
			}
		}

		public int Row
		{
			get
			{
				return this.row;
			}
			set
			{
				this.row = value;
				this.BuildButtonList();
			}
		}
	}
	public delegate void ButtonListPadEventHandler(object sender, ButtonListPadEventArgs e);
	public class ButtonListPadEventArgs : System.ComponentModel.Component
	{
		private ImageButton button;
		private object buttonValue;
		private int index;

		// Methods
		public ButtonListPadEventArgs(ImageButton button, int index)
		{
			this.button = button;
			if (button.ObjectValue != null)
			{
				this.buttonValue = button.ObjectValue;
			}
			else
			{
				this.buttonValue = null;
			}
			this.index = index;
		}

		// Properties
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
				return (string) this.buttonValue;
			}
		}
	}


}
