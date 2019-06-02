using System;
using System.Collections;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace smartRestaurant.Controls
{
	public class ButtonListPad : Control
	{
		private int row;

		private int column;

		private int padding;

		private System.Windows.Forms.ImageList imageList;

		private System.Drawing.Image image;

		private System.Drawing.Image imageClick;

		private int imageIndex;

		private int imageClickIndex;

		private ImageButton[] buttons;

		private float red;

		private float green;

		private float blue;

		private ArrayList items;

		private int itemStart;

		private bool pageEnable;

		private bool autoRefresh;

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
					for (int i = 0; i < (int)this.buttons.Length; i++)
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
					for (int i = 0; i < (int)this.buttons.Length; i++)
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

		public System.Drawing.Image Image
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
					for (int i = 0; i < (int)this.buttons.Length; i++)
					{
						this.buttons[i].Image = this.image;
					}
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
				if (this.buttons != null)
				{
					for (int i = 0; i < (int)this.buttons.Length; i++)
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
					for (int i = 0; i < (int)this.buttons.Length; i++)
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

		public ButtonListPad()
		{
			float single = 1f;
			float single1 = single;
			this.blue = single;
			float single2 = single1;
			single1 = single2;
			this.green = single2;
			this.red = single1;
			this.items = new ArrayList();
			this.itemStart = 0;
			this.pageEnable = false;
			this.autoRefresh = true;
			this.BuildButtonList();
		}

		private void BuildButtonList()
		{
			int num = this.row * this.column;
			if (num == 0)
			{
				this.buttons = null;
				return;
			}
			base.Controls.Clear();
			ImageButton[] imageButtonArray = this.buttons;
			this.buttons = new ImageButton[num];
			EventHandler eventHandler = new EventHandler(this.Button_Click);
			for (int i = 0; i < num; i++)
			{
				if (imageButtonArray == null || i >= (int)imageButtonArray.Length)
				{
					this.buttons[i] = new ImageButton();
					this.buttons[i].Click += eventHandler;
					this.buttons[i].DoubleClick += eventHandler;
				}
				else
				{
					this.buttons[i] = imageButtonArray[i];
				}
				this.buttons[i].Image = this.image;
				this.buttons[i].ImageClick = this.imageClick;
				this.buttons[i].Width = this.imageClick.Width;
				this.buttons[i].Height = this.imageClick.Height;
				int width = i % this.column * (this.buttons[i].Width + this.padding);
				int height = i / this.column * (this.buttons[i].Height + this.padding);
				this.buttons[i].Left = width;
				this.buttons[i].Top = height;
			}
			base.Controls.AddRange(this.buttons);
			base.Width = (this.buttons[0].Width + this.padding) * this.column - this.padding;
			base.Height = (this.buttons[0].Height + this.padding) * this.row - this.padding;
		}

		private void Button_Click(object sender, EventArgs e)
		{
			if (!(sender is ImageButton))
			{
				return;
			}
			ImageButton imageButton = (ImageButton)sender;
			int num = -1;
			int num1 = 0;
			while (num1 < (int)this.buttons.Length)
			{
				if (imageButton != this.buttons[num1])
				{
					num1++;
				}
				else
				{
					num = num1;
					break;
				}
			}
			if (num < 0)
			{
				return;
			}
			if (this.pageEnable)
			{
				if (num == 0)
				{
					ButtonListPad length = this;
					length.itemStart = length.itemStart - ((int)this.buttons.Length - 2);
					if (this.itemStart < 0)
					{
						this.itemStart = this.items.Count / ((int)this.buttons.Length - 2) * ((int)this.buttons.Length - 2);
					}
					this.OnPageChange(new ButtonListPadEventArgs(imageButton, num));
					return;
				}
				if (num == (int)this.buttons.Length - 1)
				{
					ButtonListPad buttonListPad = this;
					buttonListPad.itemStart = buttonListPad.itemStart + ((int)this.buttons.Length - 2);
					if (this.itemStart >= this.items.Count)
					{
						this.itemStart = 0;
					}
					this.OnPageChange(new ButtonListPadEventArgs(imageButton, num));
					return;
				}
				num = num + (this.itemStart - 1);
			}
			this.OnPadClick(new ButtonListPadEventArgs(imageButton, num));
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
			if (index >= this.itemStart + ((int)this.buttons.Length - 2))
			{
				return -1;
			}
			return index - this.itemStart + 1;
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
			int num;
			int length;
			int num1 = this.itemStart;
			if (this.items == null)
			{
				return;
			}
			if (this.items.Count <= (int)this.buttons.Length)
			{
				num = 0;
				length = (int)this.buttons.Length;
				this.pageEnable = false;
			}
			else
			{
				num = 1;
				length = (int)this.buttons.Length - 1;
				this.buttons[0].Text = "<<";
				this.buttons[0].ObjectValue = null;
				this.buttons[(int)this.buttons.Length - 1].Text = ">>";
				this.buttons[(int)this.buttons.Length - 1].ObjectValue = null;
				this.pageEnable = true;
			}
			for (int i = num; i < length; i++)
			{
				if (this.items == null || num1 >= this.items.Count || !(this.items[num1] is ButtonItem))
				{
					this.buttons[i].Text = null;
					this.buttons[i].ObjectValue = null;
					this.buttons[i].IsLock = false;
				}
				else
				{
					ButtonItem item = (ButtonItem)this.items[num1];
					this.buttons[i].Text = item.Text;
					this.buttons[i].ObjectValue = item.ObjectValue;
					this.buttons[i].ForeColor = item.ForeColor;
					this.buttons[i].IsLock = item.IsLock;
				}
				num1++;
			}
		}

		public void SetLock(int pos, bool isLock)
		{
			if (this.buttons != null && pos < (int)this.buttons.Length)
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
			if (this.buttons != null && pos < (int)this.buttons.Length)
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

		public event ButtonListPadEventHandler PadClick;

		public event ButtonListPadEventHandler PageChange;
	}
}