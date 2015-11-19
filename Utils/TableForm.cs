using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using smartRestaurant.Controls;
using smartRestaurant.Data;
using smartRestaurant.TableService;
using smartRestaurant.Utils;

namespace smartRestaurant.Utils
{
	/// <summary>
	/// Summary description for CancelForm.
	/// </summary>
	public class TableForm : Form
	{
		// Fields
		private ImageButton BtnCancel;
		private ImageButton BtnOk;
		private ImageList ButtonImgList;
		private IContainer components;
		private static TableForm instance = null;
		private int mode;
		private static int MODE_MULTI = 1;
		private static int MODE_SINGLE = 0;
		private int[] multiSelectID = null;
		private ArrayList multiTemp = null;
		private ImageButton[] tableButtons;
		private TableInformation[] tableInfo;
		private GroupPanel TablePanel;
		private TableInformation tableSelect = null;
		private TableStatus[] tableStatus;

		// Methods
		public TableForm()
		{
			this.InitializeComponent();
			this.multiTemp = new ArrayList();
		}

		private void AddTableButton()
		{
			try
			{
				this.tableStatus = new smartRestaurant.TableService.TableService().GetTableStatus();
				Image image = this.ButtonImgList.Images[0];
				Image image2 = this.ButtonImgList.Images[1];
				bool flag = false;
				int num = 13;
				int num2 = 0x30;
				if ((this.tableStatus != null) && (this.tableStatus.Length > 1))
				{
					if ((this.tableButtons == null) || (this.tableButtons.Length != (this.tableStatus.Length - 1)))
					{
						this.TablePanel.Controls.Clear();
						this.tableButtons = new ImageButton[this.tableStatus.Length - 1];
						flag = true;
					}
					for (int i = 0; i < (this.tableStatus.Length - 1); i++)
					{
						ImageButton button;
						if (flag)
						{
							button = new ImageButton();
							button.Image = image;
							button.ImageClick = image2;
							button.Left = num;
							button.Top = num2;
							button.Text = this.tableStatus[i + 1].TableName;
						}
						else
						{
							button = this.tableButtons[i];
						}
						if ((this.tableSelect != null) && (this.tableStatus[i + 1].TableID == this.tableSelect.TableID))
						{
							button.ObjectValue = this.tableStatus[i + 1].TableID;
							button.Red = 1.75f;
							button.Green = 1.75f;
							button.Blue = 1f;
						}
						else if ((this.multiTemp != null) && this.multiTemp.Contains(this.tableInfo[i + 1]))
						{
							button.ObjectValue = this.tableStatus[i + 1].TableID;
							this.tableStatus[i + 1].InUse = false;
							button.Red = 2f;
							button.Green = 2f;
							button.Blue = 1f;
						}
						else if (this.tableStatus[i + 1].InUse)
						{
							button.ObjectValue = -this.tableStatus[i + 1].TableID;
							button.Red = 1f;
							button.Green = 2f;
							button.Blue = 2f;
						}
						else
						{
							button.ObjectValue = this.tableStatus[i + 1].TableID;
							button.Red = 1f;
							button.Green = 1f;
							button.Blue = 1f;
						}
						if (flag)
						{
							button.Click += new EventHandler(this.Table_Click);
							this.TablePanel.Controls.Add(button);
							this.tableButtons[i] = button;
						}
						num += button.Width + 10;
						if (((i + 1) % 7) == 0)
						{
							num = 13;
							num2 += button.Height + 10;
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
			if (this.multiTemp.Count == 0)
			{
				this.multiSelectID = new int[] { -1 };
			}
			else
			{
				this.multiSelectID = new int[this.multiTemp.Count];
				for (int i = 0; i < this.multiTemp.Count; i++)
				{
					this.multiSelectID[i] = ((TableInformation) this.multiTemp[i]).TableID;
				}
			}
			base.Close();
		}

		private void CreateMultiTemp(int[] tableList)
		{
			if ((tableList != null) && (tableList.Length != 0))
			{
				for (int i = 0; i < tableList.Length; i++)
				{
					for (int j = 0; j < this.tableInfo.Length; j++)
					{
						if (tableList[i] == this.tableInfo[j].TableID)
						{
							this.multiTemp.Add(this.tableInfo[j]);
							break;
						}
					}
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && (this.components != null))
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.TablePanel = new smartRestaurant.Controls.GroupPanel();
			this.BtnCancel = new smartRestaurant.Controls.ImageButton();
			this.ButtonImgList = new System.Windows.Forms.ImageList(this.components);
			this.BtnOk = new smartRestaurant.Controls.ImageButton();
			this.SuspendLayout();
			// 
			// TablePanel
			// 
			this.TablePanel.BackColor = System.Drawing.Color.Transparent;
			this.TablePanel.Caption = "Select Table";
			this.TablePanel.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.TablePanel.Location = new System.Drawing.Point(0, 0);
			this.TablePanel.Name = "TablePanel";
			this.TablePanel.ShowHeader = true;
			this.TablePanel.Size = new System.Drawing.Size(866, 352);
			this.TablePanel.TabIndex = 3;
			// 
			// BtnCancel
			// 
			this.BtnCancel.BackColor = System.Drawing.Color.Transparent;
			this.BtnCancel.Blue = 1F;
			this.BtnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.BtnCancel.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(222)));
			this.BtnCancel.Green = 1F;
			this.BtnCancel.ImageClick = null;
			this.BtnCancel.ImageClickIndex = 0;
			this.BtnCancel.Location = new System.Drawing.Point(736, 360);
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.ObjectValue = null;
			this.BtnCancel.Red = 1F;
			this.BtnCancel.Size = new System.Drawing.Size(110, 60);
			this.BtnCancel.TabIndex = 3;
			this.BtnCancel.Text = "Cancel";
			this.BtnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
			// 
			// ButtonImgList
			// 
			this.ButtonImgList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.ButtonImgList.ImageSize = new System.Drawing.Size(110, 60);
			this.ButtonImgList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// BtnOk
			// 
			this.BtnOk.BackColor = System.Drawing.Color.Transparent;
			this.BtnOk.Blue = 1F;
			this.BtnOk.Cursor = System.Windows.Forms.Cursors.Hand;
			this.BtnOk.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(222)));
			this.BtnOk.Green = 1F;
			this.BtnOk.ImageClick = null;
			this.BtnOk.ImageClickIndex = 0;
			this.BtnOk.Location = new System.Drawing.Point(600, 360);
			this.BtnOk.Name = "BtnOk";
			this.BtnOk.ObjectValue = null;
			this.BtnOk.Red = 1F;
			this.BtnOk.Size = new System.Drawing.Size(110, 60);
			this.BtnOk.TabIndex = 4;
			this.BtnOk.Text = "Ok";
			this.BtnOk.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
			// 
			// TableForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(9, 23);
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(866, 424);
			this.Controls.Add(this.BtnOk);
			this.Controls.Add(this.TablePanel);
			this.Controls.Add(this.BtnCancel);
			this.Font = new System.Drawing.Font("Tahoma", 14.25F);
			this.Name = "TableForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "TableForm";
			this.ResumeLayout(false);

		}

		private void LoadTableInformation()
		{
			smartRestaurant.TableService.TableService service = new smartRestaurant.TableService.TableService();
			this.tableInfo = service.GetTableList();
			while (this.tableInfo == null)
			{
				if (MessageBox.Show("Can't load table information.", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Hand) == DialogResult.Cancel)
				{
					((MainForm) base.MdiParent).Exit();
				}
				else
				{
					this.tableInfo = service.GetTableList();
				}
			}
		}

		public static TableInformation Show(string PageShow)
		{
			if (instance == null)
			{
				instance = new TableForm();
			}
			if (PageShow != null)
			{
				instance.TablePanel.Caption = PageShow;
			}
			instance.BtnOk.Visible = false;
			instance.mode = MODE_SINGLE;
			instance.tableSelect = null;
			instance.LoadTableInformation();
			instance.AddTableButton();
			instance.ShowDialog();
			return instance.tableSelect;
		}

		public static int[] ShowMulti(string PageShow, TableInformation mainTable, int[] mergeTable)
		{
			if (instance == null)
			{
				instance = new TableForm();
			}
			if (PageShow != null)
			{
				instance.TablePanel.Caption = PageShow;
			}
			instance.BtnOk.Visible = true;
			instance.mode = MODE_MULTI;
			instance.tableSelect = mainTable;
			instance.multiTemp.Clear();
			instance.LoadTableInformation();
			instance.CreateMultiTemp(mergeTable);
			instance.AddTableButton();
			instance.ShowDialog();
			return instance.multiSelectID;
		}

		private void Table_Click(object sender, EventArgs e)
		{
			ImageButton button = (ImageButton) sender;
			int objectValue = (int) button.ObjectValue;
			if (objectValue < 0)
			{
				objectValue = -objectValue;
			}
			for (int i = 0; i < this.tableInfo.Length; i++)
			{
				if (this.tableInfo[i].TableID == objectValue)
				{
					if (this.tableStatus[i].InUse)
					{
						MessageForm.Show("Select Table", "Can't select used table.");
						return;
					}
					if (this.mode == MODE_SINGLE)
					{
						this.tableSelect = this.tableInfo[i];
						base.Close();
					}
					else if (this.mode == MODE_MULTI)
					{
						if (this.tableInfo[i].TableID != this.tableSelect.TableID)
						{
							if (this.multiTemp.Contains(this.tableInfo[i]))
							{
								this.multiTemp.Remove(this.tableInfo[i]);
								button.Red = 1f;
								button.Green = 1f;
								button.Blue = 1f;
								return;
							}
							this.multiTemp.Add(this.tableInfo[i]);
							button.Red = 2f;
							button.Green = 2f;
							button.Blue = 1f;
						}
						return;
					}
				}
			}
		}
	}


}
