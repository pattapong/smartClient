using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;


namespace smartRestaurant.Controls
{
	public class ItemsList : Control
	{
		// Fields
		private ContentAlignment alignment;
		private bool autoRefresh = true;
		private Color backAlterColor;
		private Color backHeaderColor;
		private Color backHeaderSelectedColor;
		private Color backNormalColor;
		private Color backSelectedColor;
		private ItemsList bindList1;
		private ItemsList bindList2;
		private int border = 0;
		private Color foreAlterColor;
		private Color foreHeaderColor;
		private Color foreHeaderSelectedColor;
		private Color foreNormalColor;
		private Color foreSelectedColor;
		private int itemHeight = 20;
		private ArrayList items = new ArrayList();
		private int itemStart = 0;
		private int itemWidth = 320;
		private Label[] labels;
		private bool pageEnable = false;
		private int row;
		private int selectedIndex;
		private Font strikeFont;
		public delegate void ItemsListEventHandler(object sender, ItemsListEventArgs e);

		// Events
		public event ItemsListEventHandler ItemClick;

		// Methods
		public ItemsList()
		{
			this.bindList1 = (ItemsList) (this.bindList2 = null);
			this.alignment = ContentAlignment.MiddleLeft;
			this.BuildItemList();
		}

		private void BuildItemList()
		{
			if (this.row == 0)
			{
				this.labels = null;
			}
			else
			{
				base.Controls.Clear();
				Label[] labels = this.labels;
				this.labels = new Label[this.row];
				EventHandler handler = new EventHandler(this.Item_Click);
				for (int i = 0; i < this.row; i++)
				{
					if ((labels != null) && (i < labels.Length))
					{
						this.labels[i] = labels[i];
					}
					else
					{
						this.labels[i] = new Label();
						this.labels[i].Click += handler;
					}
					int num = this.border + (i * this.itemHeight);
					this.labels[i].Left = this.border;
					this.labels[i].Top = num;
					this.labels[i].Width = this.itemWidth;
					this.labels[i].Height = this.itemHeight;
					this.labels[i].TextAlign = this.alignment;
					if ((i % 2) == 0)
					{
						this.labels[i].ForeColor = this.foreNormalColor;
						this.labels[i].BackColor = this.backNormalColor;
					}
					else
					{
						this.labels[i].ForeColor = this.foreAlterColor;
						this.labels[i].BackColor = this.backAlterColor;
					}
				}
				base.Controls.AddRange(this.labels);
				base.Width = this.itemWidth + (this.border * 2);
				base.Height = (this.itemHeight * this.row) + (this.border * 2);
			}
		}

		private void ChangePosition(int itemStart, int selectedIndex)
		{
			if ((this.bindList1 != null) && (this.bindList1.selectedIndex != selectedIndex))
			{
				this.bindList1.itemStart = itemStart;
				this.bindList1.selectedIndex = selectedIndex;
				this.bindList1.SetItemValue();
				this.bindList1.ChangePosition(itemStart, selectedIndex);
			}
			if ((this.bindList2 != null) && (this.bindList2.selectedIndex != selectedIndex))
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
			if (this.pageEnable)
			{
				if (((this.itemStart + cnt) + this.row) < this.items.Count)
				{
					this.itemStart += cnt;
				}
				else
				{
					this.itemStart = this.items.Count - this.row;
				}
				this.SetItemValue();
				if ((this.bindList1 != null) && (this.bindList1.itemStart != this.itemStart))
				{
					this.bindList1.Down(cnt);
				}
				if ((this.bindList2 != null) && (this.bindList2.itemStart != this.itemStart))
				{
					this.bindList2.Down(cnt);
				}
			}
		}

		private void Item_Click(object sender, EventArgs e)
		{
			if ((this.items.Count > 0) && (sender is Label))
			{
				Label label = (Label) sender;
				int num = -1;
				for (int i = 0; i < this.labels.Length; i++)
				{
					if (label == this.labels[i])
					{
						num = i;
						break;
					}
				}
				if ((num >= 0) && ((this.itemStart + num) < this.items.Count))
				{
					this.selectedIndex = this.itemStart + num;
					this.SetItemValue();
					this.OnItemClick(new ItemsListEventArgs((DataItem) this.items[this.itemStart + num]));
					this.ChangePosition(this.itemStart, this.selectedIndex);
				}
			}
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
			pe.Graphics.DrawRectangle(Pens.Black, 0, 0, base.Width - 1, base.Height - 1);
		}

		public void Reset()
		{
			this.itemStart = 0;
			this.pageEnable = false;
			this.autoRefresh = true;
		}

		private void SetItemValue()
		{
			if (this.labels != null)
			{
				int index = -1;
				if (this.items.Count > this.labels.Length)
				{
					this.pageEnable = true;
				}
				else
				{
					this.pageEnable = false;
				}
				int num3 = 0;
				for (int i = 0; i < this.items.Count; i++)
				{
					if (((i == this.itemStart) || (this.itemStart < 0)) && (index < 0))
					{
						this.itemStart = i;
						index = 0;
					}
					if (index >= this.labels.Length)
					{
						break;
					}
					if (((this.items != null) && (i < this.items.Count)) && (this.items[i] is DataItem))
					{
						DataItem item = (DataItem) this.items[i];
						if (index >= 0)
						{
							this.labels[index].Text = item.Text;
							this.labels[index].Cursor = Cursors.Hand;
							if (item.Strike)
							{
								if ((this.strikeFont == null) || (this.strikeFont.Size != this.Font.Size))
								{
									this.strikeFont = new Font(this.Font.FontFamily, this.Font.Size, FontStyle.Strikeout, this.Font.Unit);
								}
								this.labels[index].Font = this.strikeFont;
							}
							else
							{
								this.labels[index].Font = this.Font;
							}
						}
						if (item.IsHeader)
						{
							if (index >= 0)
							{
								if (this.selectedIndex == i)
								{
									this.labels[index].ForeColor = this.foreHeaderSelectedColor;
									this.labels[index].BackColor = this.backHeaderSelectedColor;
								}
								else
								{
									this.labels[index].ForeColor = this.foreHeaderColor;
									this.labels[index].BackColor = this.backHeaderColor;
								}
							}
							num3 = 0;
						}
						else
						{
							if (index >= 0)
							{
								if (this.selectedIndex == i)
								{
									this.labels[index].ForeColor = this.foreSelectedColor;
									this.labels[index].BackColor = this.backSelectedColor;
								}
								else if ((num3 % 2) == 0)
								{
									this.labels[index].ForeColor = this.foreNormalColor;
									this.labels[index].BackColor = this.backNormalColor;
								}
								else
								{
									this.labels[index].ForeColor = this.foreAlterColor;
									this.labels[index].BackColor = this.backAlterColor;
								}
							}
							num3++;
						}
					}
					else if (index >= 0)
					{
						this.labels[index].Text = null;
					}
					if (index >= 0)
					{
						index++;
					}
				}
				if (index < 0)
				{
					if (this.items.Count > 0)
					{
						return;
					}
					index = 0;
				}
				while (index < this.labels.Length)
				{
					this.labels[index].Text = null;
					this.labels[index].Cursor = Cursors.Default;
					if ((num3 % 2) == 0)
					{
						this.labels[index].ForeColor = this.foreNormalColor;
						this.labels[index].BackColor = this.backNormalColor;
					}
					else
					{
						this.labels[index].ForeColor = this.foreAlterColor;
						this.labels[index].BackColor = this.backAlterColor;
					}
					num3++;
					index++;
				}
			}
		}

		public void Up(int cnt)
		{
			if (this.pageEnable)
			{
				if ((this.itemStart - cnt) >= 0)
				{
					this.itemStart -= cnt;
				}
				else
				{
					this.itemStart = 0;
				}
				this.SetItemValue();
				if ((this.bindList1 != null) && (this.bindList1.itemStart != this.itemStart))
				{
					this.bindList1.Up(cnt);
				}
				if ((this.bindList2 != null) && (this.bindList2.itemStart != this.itemStart))
				{
					this.bindList2.Up(cnt);
				}
			}
		}

		// Properties
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
				return (this.pageEnable && ((this.itemStart + this.row) < this.items.Count));
			}
		}

		public bool CanUp
		{
			get
			{
				return (this.pageEnable && (this.itemStart > 0));
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
				if (this.selectedIndex >= (this.itemStart + this.row))
				{
					this.itemStart = (this.selectedIndex - this.row) + 1;
				}
				else if (this.selectedIndex < this.itemStart)
				{
					this.itemStart = this.selectedIndex;
				}
				this.ChangePosition(this.itemStart, this.selectedIndex);
			}
		}
	}
	public delegate void ItemsListEventHandler(object sender, ItemsListEventArgs e);

	public class ItemsListEventArgs : EventArgs
	{
		// Fields
		private DataItem item;

		// Methods
		public ItemsListEventArgs(DataItem item)
		{
			this.item = item;
		}

		// Properties
		public DataItem Item
		{
			get
			{
				return this.item;
			}
		}
	}
}
