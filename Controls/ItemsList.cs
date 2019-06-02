using System;
using System.Collections;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace smartRestaurant.Controls
{
	public class ItemsList : Control
	{
		private Color backHeaderColor;

		private Color backHeaderSelectedColor;

		private Color backNormalColor;

		private Color backAlterColor;

		private Color backSelectedColor;

		private Color foreHeaderColor;

		private Color foreHeaderSelectedColor;

		private Color foreNormalColor;

		private Color foreAlterColor;

		private Color foreSelectedColor;

		private System.Drawing.Font strikeFont;

		private ItemsList bindList1;

		private ItemsList bindList2;

		private int itemWidth;

		private int itemHeight;

		private int border;

		private int row;

		private Label[] labels;

		private ArrayList items;

		private int selectedIndex;

		private int itemStart;

		private bool pageEnable;

		private bool autoRefresh;

		private ContentAlignment alignment;

		public ContentAlignment Alignment
		{
			get
			{
				return this.alignment;
			}
			set
			{
				this.alignment = value;
			}
		}

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
					this.SetItemValue();
					this.Refresh();
				}
			}
		}

		public Color BackAlterColor
		{
			get
			{
				return this.backAlterColor;
			}
			set
			{
				this.backAlterColor = value;
				if (this.autoRefresh)
				{
					this.SetItemValue();
				}
			}
		}

		public Color BackHeaderColor
		{
			get
			{
				return this.backHeaderColor;
			}
			set
			{
				this.backHeaderColor = value;
				if (this.autoRefresh)
				{
					this.SetItemValue();
				}
			}
		}

		public Color BackHeaderSelectedColor
		{
			get
			{
				return this.backHeaderSelectedColor;
			}
			set
			{
				this.backHeaderSelectedColor = value;
				if (this.autoRefresh)
				{
					this.SetItemValue();
				}
			}
		}

		public Color BackNormalColor
		{
			get
			{
				return this.backNormalColor;
			}
			set
			{
				this.backNormalColor = value;
				if (this.autoRefresh)
				{
					this.SetItemValue();
				}
			}
		}

		public Color BackSelectedColor
		{
			get
			{
				return this.backSelectedColor;
			}
			set
			{
				this.backSelectedColor = value;
				if (this.autoRefresh)
				{
					this.SetItemValue();
				}
			}
		}

		public ItemsList BindList1
		{
			get
			{
				return this.bindList1;
			}
			set
			{
				this.bindList1 = value;
			}
		}

		public ItemsList BindList2
		{
			get
			{
				return this.bindList2;
			}
			set
			{
				this.bindList2 = value;
			}
		}

		public int Border
		{
			get
			{
				return this.border;
			}
			set
			{
				this.border = value;
				if (this.autoRefresh)
				{
					this.BuildItemList();
					this.SetItemValue();
				}
			}
		}

		public bool CanDown
		{
			get
			{
				if (!this.pageEnable)
				{
					return false;
				}
				return this.itemStart + this.row < this.items.Count;
			}
		}

		public bool CanUp
		{
			get
			{
				if (!this.pageEnable)
				{
					return false;
				}
				return this.itemStart > 0;
			}
		}

		public Color ForeAlterColor
		{
			get
			{
				return this.foreAlterColor;
			}
			set
			{
				this.foreAlterColor = value;
				if (this.autoRefresh)
				{
					this.SetItemValue();
				}
			}
		}

		public Color ForeHeaderColor
		{
			get
			{
				return this.foreHeaderColor;
			}
			set
			{
				this.foreHeaderColor = value;
				if (this.autoRefresh)
				{
					this.SetItemValue();
				}
			}
		}

		public Color ForeHeaderSelectedColor
		{
			get
			{
				return this.foreHeaderSelectedColor;
			}
			set
			{
				this.foreHeaderSelectedColor = value;
				if (this.autoRefresh)
				{
					this.SetItemValue();
				}
			}
		}

		public Color ForeNormalColor
		{
			get
			{
				return this.foreNormalColor;
			}
			set
			{
				this.foreNormalColor = value;
				if (this.autoRefresh)
				{
					this.SetItemValue();
				}
			}
		}

		public Color ForeSelectedColor
		{
			get
			{
				return this.foreSelectedColor;
			}
			set
			{
				this.foreSelectedColor = value;
				if (this.autoRefresh)
				{
					this.SetItemValue();
				}
			}
		}

		public int ItemHeight
		{
			get
			{
				return this.itemHeight;
			}
			set
			{
				this.itemHeight = value;
				if (this.autoRefresh)
				{
					this.BuildItemList();
					this.SetItemValue();
				}
			}
		}

		public ArrayList Items
		{
			get
			{
				return this.items;
			}
		}

		public int ItemWidth
		{
			get
			{
				return this.itemWidth;
			}
			set
			{
				this.itemWidth = value;
				if (this.autoRefresh)
				{
					this.BuildItemList();
					this.SetItemValue();
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
				this.BuildItemList();
			}
		}

		public int SelectedIndex
		{
			get
			{
				return this.selectedIndex;
			}
			set
			{
				this.selectedIndex = value;
				if (this.selectedIndex >= this.itemStart + this.row)
				{
					this.itemStart = this.selectedIndex - this.row + 1;
				}
				else if (this.selectedIndex < this.itemStart)
				{
					this.itemStart = this.selectedIndex;
				}
				this.ChangePosition(this.itemStart, this.selectedIndex);
			}
		}

		public ItemsList()
		{
			this.items = new ArrayList();
			this.border = 0;
			this.itemStart = 0;
			this.itemWidth = 320;
			this.itemHeight = 20;
			this.pageEnable = false;
			this.autoRefresh = true;
			object obj = null;
			ItemsList itemsList = (ItemsList)obj;
			this.bindList2 = (ItemsList)obj;
			this.bindList1 = itemsList;
			this.alignment = ContentAlignment.MiddleLeft;
			this.BuildItemList();
		}

		private void BuildItemList()
		{
			if (this.row == 0)
			{
				this.labels = null;
				return;
			}
			base.Controls.Clear();
			Label[] labelArray = this.labels;
			this.labels = new Label[this.row];
			EventHandler eventHandler = new EventHandler(this.Item_Click);
			for (int i = 0; i < this.row; i++)
			{
				if (labelArray == null || i >= (int)labelArray.Length)
				{
					this.labels[i] = new Label();
					this.labels[i].Click += eventHandler;
				}
				else
				{
					this.labels[i] = labelArray[i];
				}
				int num = this.border + i * this.itemHeight;
				this.labels[i].Left = this.border;
				this.labels[i].Top = num;
				this.labels[i].Width = this.itemWidth;
				this.labels[i].Height = this.itemHeight;
				this.labels[i].TextAlign = this.alignment;
				if (i % 2 != 0)
				{
					this.labels[i].ForeColor = this.foreAlterColor;
					this.labels[i].BackColor = this.backAlterColor;
				}
				else
				{
					this.labels[i].ForeColor = this.foreNormalColor;
					this.labels[i].BackColor = this.backNormalColor;
				}
			}
			base.Controls.AddRange(this.labels);
			base.Width = this.itemWidth + this.border * 2;
			base.Height = this.itemHeight * this.row + this.border * 2;
		}

		private void ChangePosition(int itemStart, int selectedIndex)
		{
			if (this.bindList1 != null && this.bindList1.selectedIndex != selectedIndex)
			{
				this.bindList1.itemStart = itemStart;
				this.bindList1.selectedIndex = selectedIndex;
				this.bindList1.SetItemValue();
				this.bindList1.ChangePosition(itemStart, selectedIndex);
			}
			if (this.bindList2 != null && this.bindList2.selectedIndex != selectedIndex)
			{
				this.bindList2.itemStart = itemStart;
				this.bindList2.selectedIndex = selectedIndex;
				this.bindList2.SetItemValue();
				this.bindList2.ChangePosition(itemStart, selectedIndex);
			}
		}

		public void Clear()
		{
			this.items.Clear();
			this.SelectedIndex = -1;
			this.itemStart = 0;
		}

		public void Down(int cnt)
		{
			if (!this.pageEnable)
			{
				return;
			}
			if (this.itemStart + cnt + this.row >= this.items.Count)
			{
				this.itemStart = this.items.Count - this.row;
			}
			else
			{
				this.itemStart += cnt;
			}
			this.SetItemValue();
			if (this.bindList1 != null && this.bindList1.itemStart != this.itemStart)
			{
				this.bindList1.Down(cnt);
			}
			if (this.bindList2 != null && this.bindList2.itemStart != this.itemStart)
			{
				this.bindList2.Down(cnt);
			}
		}

		private void Item_Click(object sender, EventArgs e)
		{
			if (this.items.Count <= 0)
			{
				return;
			}
			if (!(sender is Label))
			{
				return;
			}
			Label label = (Label)sender;
			int num = -1;
			int num1 = 0;
			while (num1 < (int)this.labels.Length)
			{
				if (label != this.labels[num1])
				{
					num1++;
				}
				else
				{
					num = num1;
					break;
				}
			}
			if (num < 0 || this.itemStart + num >= this.items.Count)
			{
				return;
			}
			this.selectedIndex = this.itemStart + num;
			this.SetItemValue();
			this.OnItemClick(new ItemsListEventArgs((DataItem)this.items[this.itemStart + num]));
			this.ChangePosition(this.itemStart, this.selectedIndex);
		}

		protected virtual void OnItemClick(ItemsListEventArgs e)
		{
			if (this.ItemClick != null)
			{
				this.ItemClick(this, e);
			}
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			this.SetItemValue();
			Graphics graphics = pe.Graphics;
			graphics.DrawRectangle(Pens.Black, 0, 0, base.Width - 1, base.Height - 1);
		}

		public void Reset()
		{
			this.itemStart = 0;
			this.pageEnable = false;
			this.autoRefresh = true;
		}

		private void SetItemValue()
		{
			if (this.labels == null)
			{
				return;
			}
			int num = -1;
			if (this.items.Count <= (int)this.labels.Length)
			{
				this.pageEnable = false;
			}
			else
			{
				this.pageEnable = true;
			}
			int num1 = 0;
			for (int i = 0; i < this.items.Count; i++)
			{
				if ((i == this.itemStart || this.itemStart < 0) && num < 0)
				{
					this.itemStart = i;
					num = 0;
				}
				if (num >= (int)this.labels.Length)
				{
					break;
				}
				if (this.items != null && i < this.items.Count && this.items[i] is DataItem)
				{
					DataItem item = (DataItem)this.items[i];
					if (num >= 0)
					{
						this.labels[num].Text = item.Text;
						this.labels[num].Cursor = Cursors.Hand;
						if (!item.Strike)
						{
							this.labels[num].Font = this.Font;
						}
						else
						{
							if (this.strikeFont == null || this.strikeFont.Size != this.Font.Size)
							{
								this.strikeFont = new System.Drawing.Font(this.Font.FontFamily, this.Font.Size, FontStyle.Strikeout, this.Font.Unit);
							}
							this.labels[num].Font = this.strikeFont;
						}
					}
					if (!item.IsHeader)
					{
						if (num >= 0)
						{
							if (this.selectedIndex == i)
							{
								this.labels[num].ForeColor = this.foreSelectedColor;
								this.labels[num].BackColor = this.backSelectedColor;
							}
							else if (num1 % 2 != 0)
							{
								this.labels[num].ForeColor = this.foreAlterColor;
								this.labels[num].BackColor = this.backAlterColor;
							}
							else
							{
								this.labels[num].ForeColor = this.foreNormalColor;
								this.labels[num].BackColor = this.backNormalColor;
							}
						}
						num1++;
					}
					else
					{
						if (num >= 0)
						{
							if (this.selectedIndex != i)
							{
								this.labels[num].ForeColor = this.foreHeaderColor;
								this.labels[num].BackColor = this.backHeaderColor;
							}
							else
							{
								this.labels[num].ForeColor = this.foreHeaderSelectedColor;
								this.labels[num].BackColor = this.backHeaderSelectedColor;
							}
						}
						num1 = 0;
					}
				}
				else if (num >= 0)
				{
					this.labels[num].Text = null;
				}
				if (num >= 0)
				{
					num++;
				}
			}
			if (num < 0)
			{
				if (this.items.Count > 0)
				{
					return;
				}
				num = 0;
			}
			while (num < (int)this.labels.Length)
			{
				this.labels[num].Text = null;
				this.labels[num].Cursor = Cursors.Default;
				if (num1 % 2 != 0)
				{
					this.labels[num].ForeColor = this.foreAlterColor;
					this.labels[num].BackColor = this.backAlterColor;
				}
				else
				{
					this.labels[num].ForeColor = this.foreNormalColor;
					this.labels[num].BackColor = this.backNormalColor;
				}
				num1++;
				num++;
			}
		}

		public void Up(int cnt)
		{
			if (!this.pageEnable)
			{
				return;
			}
			if (this.itemStart - cnt < 0)
			{
				this.itemStart = 0;
			}
			else
			{
				this.itemStart -= cnt;
			}
			this.SetItemValue();
			if (this.bindList1 != null && this.bindList1.itemStart != this.itemStart)
			{
				this.bindList1.Up(cnt);
			}
			if (this.bindList2 != null && this.bindList2.itemStart != this.itemStart)
			{
				this.bindList2.Up(cnt);
			}
		}

		public event ItemsListEventHandler ItemClick;
	}
}