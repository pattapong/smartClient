using smartRestaurant;
using smartRestaurant.Controls;
using smartRestaurant.TableService;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

namespace smartRestaurant.Utils
{
	public class TableForm : Form
	{
		private static int MODE_SINGLE;

		private static int MODE_MULTI;

		private static TableForm instance;

		private GroupPanel TablePanel;

		private IContainer components;

		private ImageButton BtnCancel;

		private ImageList ButtonImgList;

		private ImageButton[] tableButtons;

		private TableInformation[] tableInfo;

		private TableInformation tableSelect = null;

		private TableStatus[] tableStatus;

		private int[] multiSelectID = null;

		private ArrayList multiTemp = null;

		private ImageButton BtnOk;

		private int mode;

		static TableForm()
		{
			TableForm.MODE_SINGLE = 0;
			TableForm.MODE_MULTI = 1;
			TableForm.instance = null;
		}

		public TableForm()
		{
			this.InitializeComponent();
			this.multiTemp = new ArrayList();
		}

		private void AddTableButton()
		{
			ImageButton tableID;
			try
			{
				this.tableStatus = (new smartRestaurant.TableService.TableService()).GetTableStatus();
				Image item = this.ButtonImgList.Images[0];
				Image image = this.ButtonImgList.Images[1];
				bool flag = false;
				int width = 13;
				int height = 48;
				if (this.tableStatus != null && (int)this.tableStatus.Length > 1)
				{
					if (this.tableButtons == null || (int)this.tableButtons.Length != (int)this.tableStatus.Length - 1)
					{
						this.TablePanel.Controls.Clear();
						this.tableButtons = new ImageButton[(int)this.tableStatus.Length - 1];
						flag = true;
					}
					for (int i = 0; i < (int)this.tableStatus.Length - 1; i++)
					{
						tableID = (!flag ? this.tableButtons[i] : new ImageButton()
						{
							Image = item,
							ImageClick = image,
							Left = width,
							Top = height,
							Text = this.tableStatus[i + 1].TableName
						});
						if (this.tableSelect != null && this.tableStatus[i + 1].TableID == this.tableSelect.TableID)
						{
							tableID.ObjectValue = this.tableStatus[i + 1].TableID;
							tableID.Red = 1.75f;
							tableID.Green = 1.75f;
							tableID.Blue = 1f;
						}
						else if (this.multiTemp != null && this.multiTemp.Contains(this.tableInfo[i + 1]))
						{
							tableID.ObjectValue = this.tableStatus[i + 1].TableID;
							this.tableStatus[i + 1].InUse = false;
							tableID.Red = 2f;
							tableID.Green = 2f;
							tableID.Blue = 1f;
						}
						else if (!this.tableStatus[i + 1].InUse)
						{
							tableID.ObjectValue = this.tableStatus[i + 1].TableID;
							tableID.Red = 1f;
							tableID.Green = 1f;
							tableID.Blue = 1f;
						}
						else
						{
							tableID.ObjectValue = -this.tableStatus[i + 1].TableID;
							tableID.Red = 1f;
							tableID.Green = 2f;
							tableID.Blue = 2f;
						}
						if (flag)
						{
							tableID.Click += new EventHandler(this.Table_Click);
							this.TablePanel.Controls.Add(tableID);
							this.tableButtons[i] = tableID;
						}
						width = width + tableID.Width + 10;
						if ((i + 1) % 7 == 0)
						{
							width = 13;
							height = height + tableID.Height + 10;
						}
					}
				}
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.ToString());
			}
		}

		private void BtnCancel_Click(object sender, EventArgs e)
		{
			this.tableSelect = null;
			this.multiSelectID = null;
			base.Close();
		}

		private void BtnOk_Click(object sender, EventArgs e)
		{
			if (this.multiTemp.Count != 0)
			{
				this.multiSelectID = new int[this.multiTemp.Count];
				for (int i = 0; i < this.multiTemp.Count; i++)
				{
					this.multiSelectID[i] = ((TableInformation)this.multiTemp[i]).TableID;
				}
			}
			else
			{
				this.multiSelectID = new int[] { -1 };
			}
			base.Close();
		}

		private void CreateMultiTemp(int[] tableList)
		{
			if (tableList == null || (int)tableList.Length == 0)
			{
				return;
			}
			for (int i = 0; i < (int)tableList.Length; i++)
			{
				int num = 0;
				while (num < (int)this.tableInfo.Length)
				{
					if (tableList[i] != this.tableInfo[num].TableID)
					{
						num++;
					}
					else
					{
						this.multiTemp.Add(this.tableInfo[num]);
						break;
					}
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			ResourceManager resourceManager = new ResourceManager(typeof(TableForm));
			this.TablePanel = new GroupPanel();
			this.BtnCancel = new ImageButton();
			this.ButtonImgList = new ImageList(this.components);
			this.BtnOk = new ImageButton();
			base.SuspendLayout();
			this.TablePanel.BackColor = Color.Transparent;
			this.TablePanel.Caption = "Select Table";
			this.TablePanel.Font = new System.Drawing.Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.TablePanel.Name = "TablePanel";
			this.TablePanel.ShowHeader = true;
			this.TablePanel.Size = new System.Drawing.Size(866, 352);
			this.TablePanel.TabIndex = 3;
			this.BtnCancel.BackColor = Color.Transparent;
			this.BtnCancel.Blue = 1f;
			this.BtnCancel.Cursor = Cursors.Hand;
			this.BtnCancel.Font = new System.Drawing.Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 222);
			this.BtnCancel.Green = 1f;
			this.BtnCancel.Image = (Bitmap)resourceManager.GetObject("BtnCancel.Image");
			this.BtnCancel.ImageClick = (Bitmap)resourceManager.GetObject("BtnCancel.ImageClick");
			this.BtnCancel.ImageClickIndex = 0;
			this.BtnCancel.Location = new Point(736, 360);
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.ObjectValue = null;
			this.BtnCancel.Red = 1f;
			this.BtnCancel.Size = new System.Drawing.Size(110, 60);
			this.BtnCancel.TabIndex = 3;
			this.BtnCancel.Text = "Cancel";
			this.BtnCancel.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnCancel.Click += new EventHandler(this.BtnCancel_Click);
			this.ButtonImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.ButtonImgList.ImageSize = new System.Drawing.Size(110, 60);
			this.ButtonImgList.ImageStream = (ImageListStreamer)resourceManager.GetObject("ButtonImgList.ImageStream");
			this.ButtonImgList.TransparentColor = Color.Transparent;
			this.BtnOk.BackColor = Color.Transparent;
			this.BtnOk.Blue = 1f;
			this.BtnOk.Cursor = Cursors.Hand;
			this.BtnOk.Font = new System.Drawing.Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 222);
			this.BtnOk.Green = 1f;
			this.BtnOk.Image = (Bitmap)resourceManager.GetObject("BtnOk.Image");
			this.BtnOk.ImageClick = (Bitmap)resourceManager.GetObject("BtnOk.ImageClick");
			this.BtnOk.ImageClickIndex = 0;
			this.BtnOk.Location = new Point(600, 360);
			this.BtnOk.Name = "BtnOk";
			this.BtnOk.ObjectValue = null;
			this.BtnOk.Red = 1f;
			this.BtnOk.Size = new System.Drawing.Size(110, 60);
			this.BtnOk.TabIndex = 4;
			this.BtnOk.Text = "Ok";
			this.BtnOk.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnOk.Click += new EventHandler(this.BtnOk_Click);
			this.AutoScaleBaseSize = new System.Drawing.Size(9, 23);
			this.BackColor = Color.White;
			base.ClientSize = new System.Drawing.Size(866, 424);
			Control.ControlCollection controls = base.Controls;
			Control[] btnOk = new Control[] { this.BtnOk, this.TablePanel, this.BtnCancel };
			controls.AddRange(btnOk);
			this.Font = new System.Drawing.Font("Tahoma", 14.25f);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.Name = "TableForm";
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "TableForm";
			base.ResumeLayout(false);
		}

		private void LoadTableInformation()
		{
			smartRestaurant.TableService.TableService tableService = new smartRestaurant.TableService.TableService();
			this.tableInfo = tableService.GetTableList();
			while (this.tableInfo == null)
			{
				if (MessageBox.Show("Can't load table information.", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Hand) != System.Windows.Forms.DialogResult.Cancel)
				{
					this.tableInfo = tableService.GetTableList();
				}
				else
				{
					((MainForm)base.MdiParent).Exit();
				}
			}
		}

		public static TableInformation Show(string PageShow)
		{
			if (TableForm.instance == null)
			{
				TableForm.instance = new TableForm();
			}
			if (PageShow != null)
			{
				TableForm.instance.TablePanel.Caption = PageShow;
			}
			TableForm.instance.BtnOk.Visible = false;
			TableForm.instance.mode = TableForm.MODE_SINGLE;
			TableForm.instance.tableSelect = null;
			TableForm.instance.LoadTableInformation();
			TableForm.instance.AddTableButton();
			TableForm.instance.ShowDialog();
			return TableForm.instance.tableSelect;
		}

		public static int[] ShowMulti(string PageShow, TableInformation mainTable, int[] mergeTable)
		{
			if (TableForm.instance == null)
			{
				TableForm.instance = new TableForm();
			}
			if (PageShow != null)
			{
				TableForm.instance.TablePanel.Caption = PageShow;
			}
			TableForm.instance.BtnOk.Visible = true;
			TableForm.instance.mode = TableForm.MODE_MULTI;
			TableForm.instance.tableSelect = mainTable;
			TableForm.instance.multiTemp.Clear();
			TableForm.instance.LoadTableInformation();
			TableForm.instance.CreateMultiTemp(mergeTable);
			TableForm.instance.AddTableButton();
			TableForm.instance.ShowDialog();
			return TableForm.instance.multiSelectID;
		}

		private void Table_Click(object sender, EventArgs e)
		{
			ImageButton imageButton = (ImageButton)sender;
			int objectValue = (int)imageButton.ObjectValue;
			if (objectValue < 0)
			{
				objectValue = -objectValue;
			}
			for (int i = 0; i < (int)this.tableInfo.Length; i++)
			{
				if (this.tableInfo[i].TableID == objectValue)
				{
					if (this.tableStatus[i].InUse)
					{
						MessageForm.Show("Select Table", "Can't select used table.");
						return;
					}
					if (this.mode == TableForm.MODE_SINGLE)
					{
						this.tableSelect = this.tableInfo[i];
						base.Close();
					}
					else if (this.mode == TableForm.MODE_MULTI)
					{
						if (this.tableInfo[i].TableID == this.tableSelect.TableID)
						{
							return;
						}
						if (this.multiTemp.Contains(this.tableInfo[i]))
						{
							this.multiTemp.Remove(this.tableInfo[i]);
							imageButton.Red = 1f;
							imageButton.Green = 1f;
							imageButton.Blue = 1f;
							return;
						}
						this.multiTemp.Add(this.tableInfo[i]);
						imageButton.Red = 2f;
						imageButton.Green = 2f;
						imageButton.Blue = 1f;
						return;
					}
				}
			}
		}
	}
}