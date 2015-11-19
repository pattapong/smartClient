using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;
using smartRestaurant.Data;
using smartRestaurant.Utils;
using smartRestaurant.Controls;
using System.Resources;

namespace smartRestaurant.Business
{
	/// <summary>
	/// Summary description for SalesForm.
	/// </summary>
	public class SalesForm : SmartForm
	{
		// Fields
		private ImageButton BtnBackup;
		private ImageButton BtnDelete;
		private ImageButton BtnExport;
		private ImageButton BtnInvoiceReport;
		private ImageButton BtnMain;
		private ImageButton BtnMaintenance;
		private ImageButton BtnManagerMaintenance;
		private ImageButton BtnPrinterSave;
		private ImageButton BtnPrintInvoiceReport;
		private ImageButton BtnReport;
		private ImageButton BtnSaleReport;
		private ImageButton BtnSumMenuType;
		private ImageButton BtnSumPayment;
		private ImageButton BtnSumTax;
		private ImageList ButtonImgList;
		private ImageList ButtonLiteImgList;
		private bool Changed;
		private IContainer components;
		private DateTimePicker DateExportFrom;
		private DateTimePicker DateExportTo;
		private DateTimePicker DateInvoiceReport;
		private DateTimePicker DateSalesReport;
		private GroupPanel groupPanel1;
		private GroupPanel groupPanel2;
		private GroupPanel groupPanel3;
		private GroupPanel groupPanel4;
		private GroupPanel groupPanel5;
		private GroupPanel groupPanel6;
		private DataGrid InvoiceReportGrid;
		private DataGrid InvoiceSummaryGrid;
		private Label label1;
		private Label label10;
		private Label label11;
		private Label label2;
		private Label label3;
		private Label label4;
		private Label label5;
		private Label label6;
		private Label label7;
		private Label label8;
		private Label label9;
		private ComboBox ListTaxMonth;
		private ComboBox ListTaxYear;
		private ComboBox LstBillPrinter;
		private ComboBox LstKitchenPrinter;
		private ComboBox LstReceiptPrinter;
		private Panel PanelInvoiceReport;
		private Panel PanelMaintenance;
		private Panel PanelManagerMaintenance;
		private Panel PanelReport;
		private Panel PanelSalesReport;
		private DataGrid ReportGrid;
		private DateTimePicker SummaryDate;
		private DataTable Table;
		private DataGrid TotalGrid;

		// Methods
		public SalesForm()
		{
			this.InitializeComponent();
		}

		private void BtnBackup_Click(object sender, EventArgs e)
		{
			new smartRestaurant.BusinessService.BusinessService().BackupDatabase();
			MessageForm.Show("Backup Database", "Backup Database Completed...");
		}

		private void BtnDelete_Click(object sender, EventArgs e)
		{
			new smartRestaurant.BusinessService.BusinessService().DeleteSelected();
			MessageForm.Show("Void Selected", "Void Selected Completed...");
		}

		private void BtnExport_Click(object sender, EventArgs e)
		{
			if (this.DateExportFrom.Value > this.DateExportTo.Value)
			{
				MessageForm.Show("Export Invoice", "Please select from date less or equal to date.");
			}
			else
			{
				smartRestaurant.BusinessService.BusinessService service = new smartRestaurant.BusinessService.BusinessService();
				DataTable table = ReportConverter.Convert(service.ExportInvoice(this.DateExportFrom.Value, this.DateExportTo.Value));
				if (table == null)
				{
					MessageForm.Show("Export Invoice", "No invoice in range to export.");
				}
				else
				{
					DateTime now = DateTime.Now;
					StringBuilder builder = new StringBuilder();
					string description = new smartRestaurant.CheckBillService.CheckBillService().GetDescription("EXPORT_PATH");
					builder.Append(description);
					builder.Append("invoice_");
					builder.Append(now.ToString("yyyyMMddhhmmss"));
					builder.Append(".csv");
					StreamWriter writer = File.CreateText(builder.ToString());
					builder.Length = 0;
					builder.Append("Export Invoice when ");
					builder.Append(now.ToString("dd/MM/yyyy hh:mm:ss"));
					writer.WriteLine(builder.ToString());
					builder.Length = 0;
					for (int i = 0; i < table.Columns.Count; i++)
					{
						if (i > 0)
						{
							builder.Append(",\"");
						}
						else
						{
							builder.Append("\"");
						}
						builder.Append(table.Columns[i].ColumnName);
						builder.Append("\"");
					}
					writer.WriteLine(builder.ToString());
					for (int j = 0; j < table.Rows.Count; j++)
					{
						DataRow row = table.Rows[j];
						builder.Length = 0;
						for (int k = 0; k < row.ItemArray.Length; k++)
						{
							if (k > 0)
							{
								builder.Append(",");
							}
							builder.Append(row[k].ToString());
						}
						writer.WriteLine(builder.ToString());
					}
					writer.Close();
					MessageForm.Show("Export Invoice", "Export Invoice Completed...");
				}
			}
		}

		private void BtnInvoiceReport_Click(object sender, EventArgs e)
		{
			this.UpdateInvoiceReport();
		}

		private void BtnMain_Click(object sender, EventArgs e)
		{
			((MainForm) base.MdiParent).ShowMainMenuForm();
		}

		private void BtnMaintenance_Click(object sender, EventArgs e)
		{
			this.UpdateMaintenance();
		}

		private void BtnManagerMaintenance_Click(object sender, EventArgs e)
		{
			this.UpdateManagerMaintenance();
		}

		private void BtnPrinterSave_Click(object sender, EventArgs e)
		{
			smartRestaurant.BusinessService.BusinessService service = new smartRestaurant.BusinessService.BusinessService();
			service.SetKitchenPrinter(this.LstKitchenPrinter.Text);
			service.SetReceiptPrinter(this.LstReceiptPrinter.Text);
			service.SetBillPrinter(this.LstBillPrinter.Text);
			MessageForm.Show("Save Printer", "Save Printer Successful...");
		}

		private void BtnPrintInvoiceReport_Click(object sender, EventArgs e)
		{
			WaitingForm.Show("Print Invoice Report");
			base.Enabled = false;
			new smartRestaurant.BusinessService.BusinessService().PrintInvoiceReport(this.DateInvoiceReport.Value, ((MainForm) base.MdiParent).User.EmployeeTypeID);
			base.Enabled = true;
			WaitingForm.HideForm();
		}

		private void BtnReport_Click(object sender, EventArgs e)
		{
			this.UpdateReports();
		}

		private void BtnSaleReport_Click(object sender, EventArgs e)
		{
			this.UpdateSalesReport();
		}

		private void BtnSumMenuType_Click(object sender, EventArgs e)
		{
			smartRestaurant.BusinessService.BusinessService service = new smartRestaurant.BusinessService.BusinessService();
			WaitingForm.Show("Print Summary");
			service.PrintSummaryMenuType(this.SummaryDate.Value);
			WaitingForm.HideForm();
		}

		private void BtnSumPayment_Click(object sender, EventArgs e)
		{
			smartRestaurant.BusinessService.BusinessService service = new smartRestaurant.BusinessService.BusinessService();
			WaitingForm.Show("Print Summary");
			service.PrintSummaryReceive(this.SummaryDate.Value);
			WaitingForm.HideForm();
		}

		private void BtnSumTax_Click(object sender, EventArgs e)
		{
			WaitingForm.Show("Print Tax Summary");
			base.Enabled = false;
			new smartRestaurant.BusinessService.BusinessService().PrintTaxSummary((int) this.ListTaxMonth.SelectedItem, (int) this.ListTaxYear.SelectedItem);
			base.Enabled = true;
			WaitingForm.HideForm();
		}

		private void DateInvoiceReport_ValueChanged(object sender, EventArgs e)
		{
			if (!this.Changed)
			{
				this.Changed = true;
				this.DateSalesReport.Value = this.DateInvoiceReport.Value;
				this.SummaryDate.Value = this.DateInvoiceReport.Value;
				this.Changed = false;
				this.UpdateInvoiceReport();
			}
		}

		private void DateSalesReport_ValueChanged(object sender, EventArgs e)
		{
			if (!this.Changed)
			{
				this.Changed = true;
				this.DateInvoiceReport.Value = this.DateSalesReport.Value;
				this.SummaryDate.Value = this.DateSalesReport.Value;
				this.Changed = false;
				this.UpdateSalesReport();
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
			this.components = new Container();
			ResourceManager manager = new ResourceManager(typeof(SalesForm));
			this.BtnMain = new ImageButton();
			this.ButtonImgList = new ImageList(this.components);
			this.BtnSaleReport = new ImageButton();
			this.BtnInvoiceReport = new ImageButton();
			this.PanelSalesReport = new Panel();
			this.DateSalesReport = new DateTimePicker();
			this.TotalGrid = new DataGrid();
			this.ReportGrid = new DataGrid();
			this.PanelInvoiceReport = new Panel();
			this.BtnPrintInvoiceReport = new ImageButton();
			this.ButtonLiteImgList = new ImageList(this.components);
			this.DateInvoiceReport = new DateTimePicker();
			this.InvoiceSummaryGrid = new DataGrid();
			this.InvoiceReportGrid = new DataGrid();
			this.BtnMaintenance = new ImageButton();
			this.PanelMaintenance = new Panel();
			this.groupPanel6 = new GroupPanel();
			this.BtnPrinterSave = new ImageButton();
			this.LstBillPrinter = new ComboBox();
			this.LstReceiptPrinter = new ComboBox();
			this.LstKitchenPrinter = new ComboBox();
			this.label11 = new Label();
			this.label10 = new Label();
			this.label7 = new Label();
			this.groupPanel3 = new GroupPanel();
			this.label5 = new Label();
			this.BtnBackup = new ImageButton();
			this.groupPanel2 = new GroupPanel();
			this.DateExportTo = new DateTimePicker();
			this.label9 = new Label();
			this.label6 = new Label();
			this.BtnExport = new ImageButton();
			this.DateExportFrom = new DateTimePicker();
			this.groupPanel1 = new GroupPanel();
			this.label1 = new Label();
			this.label2 = new Label();
			this.label3 = new Label();
			this.label4 = new Label();
			this.BtnSumMenuType = new ImageButton();
			this.BtnSumPayment = new ImageButton();
			this.SummaryDate = new DateTimePicker();
			this.groupPanel5 = new GroupPanel();
			this.ListTaxYear = new ComboBox();
			this.ListTaxMonth = new ComboBox();
			this.label8 = new Label();
			this.BtnSumTax = new ImageButton();
			this.BtnManagerMaintenance = new ImageButton();
			this.PanelManagerMaintenance = new Panel();
			this.groupPanel4 = new GroupPanel();
			this.BtnDelete = new ImageButton();
			this.BtnReport = new ImageButton();
			this.PanelReport = new Panel();
			this.PanelSalesReport.SuspendLayout();
			this.TotalGrid.BeginInit();
			this.ReportGrid.BeginInit();
			this.PanelInvoiceReport.SuspendLayout();
			this.InvoiceSummaryGrid.BeginInit();
			this.InvoiceReportGrid.BeginInit();
			this.PanelMaintenance.SuspendLayout();
			this.groupPanel6.SuspendLayout();
			this.groupPanel3.SuspendLayout();
			this.groupPanel2.SuspendLayout();
			this.groupPanel1.SuspendLayout();
			this.groupPanel5.SuspendLayout();
			this.PanelManagerMaintenance.SuspendLayout();
			this.groupPanel4.SuspendLayout();
			this.PanelReport.SuspendLayout();
			base.SuspendLayout();
			this.BtnMain.BackColor = Color.Transparent;
			this.BtnMain.Blue = 2f;
			this.BtnMain.Cursor = Cursors.Hand;
			this.BtnMain.Green = 2f;
			this.BtnMain.ImageClick = (Image) manager.GetObject("BtnMain.ImageClick");
			this.BtnMain.ImageClickIndex = 1;
			this.BtnMain.ImageIndex = 0;
			this.BtnMain.ImageList = this.ButtonImgList;
			this.BtnMain.IsLock = false;
			this.BtnMain.Location = new Point(880, 0x2b0);
			this.BtnMain.Name = "BtnMain";
			this.BtnMain.ObjectValue = null;
			this.BtnMain.Red = 1f;
			this.BtnMain.Size = new Size(110, 60);
			this.BtnMain.TabIndex = 1;
			this.BtnMain.Text = "Main";
			this.BtnMain.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnMain.Click += new EventHandler(this.BtnMain_Click);
			this.ButtonImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.ButtonImgList.ImageSize = new Size(110, 60);
			this.ButtonImgList.ImageStream = (ImageListStreamer) manager.GetObject("ButtonImgList.ImageStream");
			this.ButtonImgList.TransparentColor = Color.Transparent;
			this.BtnSaleReport.BackColor = Color.Transparent;
			this.BtnSaleReport.Blue = 2f;
			this.BtnSaleReport.Cursor = Cursors.Hand;
			this.BtnSaleReport.Green = 1f;
			this.BtnSaleReport.ImageClick = null;
			this.BtnSaleReport.ImageClickIndex = 0;
			this.BtnSaleReport.ImageIndex = 0;
			this.BtnSaleReport.ImageList = this.ButtonImgList;
			this.BtnSaleReport.IsLock = false;
			this.BtnSaleReport.Location = new Point(0x18, 0x2b0);
			this.BtnSaleReport.Name = "BtnSaleReport";
			this.BtnSaleReport.ObjectValue = null;
			this.BtnSaleReport.Red = 1f;
			this.BtnSaleReport.Size = new Size(110, 60);
			this.BtnSaleReport.TabIndex = 3;
			this.BtnSaleReport.Text = "Sales";
			this.BtnSaleReport.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnSaleReport.Click += new EventHandler(this.BtnSaleReport_Click);
			this.BtnSaleReport.DoubleClick += new EventHandler(this.BtnSaleReport_Click);
			this.BtnInvoiceReport.BackColor = Color.Transparent;
			this.BtnInvoiceReport.Blue = 2f;
			this.BtnInvoiceReport.Cursor = Cursors.Hand;
			this.BtnInvoiceReport.Green = 1f;
			this.BtnInvoiceReport.ImageClick = (Image) manager.GetObject("BtnInvoiceReport.ImageClick");
			this.BtnInvoiceReport.ImageClickIndex = 1;
			this.BtnInvoiceReport.ImageIndex = 0;
			this.BtnInvoiceReport.ImageList = this.ButtonImgList;
			this.BtnInvoiceReport.IsLock = false;
			this.BtnInvoiceReport.Location = new Point(0x90, 0x2b0);
			this.BtnInvoiceReport.Name = "BtnInvoiceReport";
			this.BtnInvoiceReport.ObjectValue = null;
			this.BtnInvoiceReport.Red = 2f;
			this.BtnInvoiceReport.Size = new Size(110, 60);
			this.BtnInvoiceReport.TabIndex = 4;
			this.BtnInvoiceReport.Text = "Receipt";
			this.BtnInvoiceReport.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnInvoiceReport.Click += new EventHandler(this.BtnInvoiceReport_Click);
			this.BtnInvoiceReport.DoubleClick += new EventHandler(this.BtnInvoiceReport_Click);
			this.PanelSalesReport.BackColor = Color.FromArgb(0xff, 0xff, 0xc0);
			this.PanelSalesReport.Controls.Add(this.DateSalesReport);
			this.PanelSalesReport.Controls.Add(this.TotalGrid);
			this.PanelSalesReport.Controls.Add(this.ReportGrid);
			this.PanelSalesReport.Location = new Point(0x10, 80);
			this.PanelSalesReport.Name = "PanelSalesReport";
			this.PanelSalesReport.Size = new Size(0x3e0, 0x260);
			this.PanelSalesReport.TabIndex = 5;
			this.DateSalesReport.CalendarFont = new Font("Tahoma", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.DateSalesReport.CustomFormat = "";
			this.DateSalesReport.Font = new Font("Tahoma", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.DateSalesReport.Format = DateTimePickerFormat.Short;
			this.DateSalesReport.Location = new Point(0x338, 8);
			this.DateSalesReport.Name = "DateSalesReport";
			this.DateSalesReport.RightToLeft = RightToLeft.No;
			this.DateSalesReport.Size = new Size(0x98, 30);
			this.DateSalesReport.TabIndex = 8;
			this.DateSalesReport.ValueChanged += new EventHandler(this.DateSalesReport_ValueChanged);
			this.TotalGrid.AllowSorting = false;
			this.TotalGrid.AlternatingBackColor = Color.FromArgb(0xc0, 0xff, 0xff);
			this.TotalGrid.BackColor = Color.White;
			this.TotalGrid.BackgroundColor = Color.FromArgb(0xe0, 0xe0, 0xe0);
			this.TotalGrid.BorderStyle = BorderStyle.None;
			this.TotalGrid.CaptionBackColor = Color.Black;
			this.TotalGrid.CaptionForeColor = Color.White;
			this.TotalGrid.CaptionVisible = false;
			this.TotalGrid.ColumnHeadersVisible = false;
			this.TotalGrid.DataMember = "";
			this.TotalGrid.FlatMode = true;
			this.TotalGrid.Font = new Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.TotalGrid.ForeColor = Color.Black;
			this.TotalGrid.GridLineColor = Color.Silver;
			this.TotalGrid.HeaderBackColor = Color.Black;
			this.TotalGrid.HeaderForeColor = Color.White;
			this.TotalGrid.Location = new Point(8, 0x238);
			this.TotalGrid.Name = "TotalGrid";
			this.TotalGrid.PreferredRowHeight = 0x20;
			this.TotalGrid.ReadOnly = true;
			this.TotalGrid.RowHeadersVisible = false;
			this.TotalGrid.SelectionBackColor = Color.Navy;
			this.TotalGrid.Size = new Size(0x3d0, 0x20);
			this.TotalGrid.TabIndex = 4;
			this.ReportGrid.AllowSorting = false;
			this.ReportGrid.AlternatingBackColor = Color.FromArgb(0xc0, 0xff, 0xff);
			this.ReportGrid.BackColor = Color.White;
			this.ReportGrid.BackgroundColor = Color.FromArgb(0xe0, 0xe0, 0xe0);
			this.ReportGrid.BorderStyle = BorderStyle.None;
			this.ReportGrid.CaptionBackColor = Color.Black;
			this.ReportGrid.CaptionForeColor = Color.White;
			this.ReportGrid.CaptionVisible = false;
			this.ReportGrid.DataMember = "";
			this.ReportGrid.FlatMode = true;
			this.ReportGrid.Font = new Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.ReportGrid.ForeColor = Color.Black;
			this.ReportGrid.GridLineColor = Color.Silver;
			this.ReportGrid.HeaderBackColor = Color.Black;
			this.ReportGrid.HeaderForeColor = Color.White;
			this.ReportGrid.Location = new Point(8, 0x30);
			this.ReportGrid.Name = "ReportGrid";
			this.ReportGrid.PreferredRowHeight = 0x20;
			this.ReportGrid.ReadOnly = true;
			this.ReportGrid.RowHeadersVisible = false;
			this.ReportGrid.SelectionBackColor = Color.Navy;
			this.ReportGrid.Size = new Size(0x3d0, 520);
			this.ReportGrid.TabIndex = 3;
			this.PanelInvoiceReport.BackColor = Color.FromArgb(0xc0, 0xff, 0xc0);
			this.PanelInvoiceReport.Controls.Add(this.BtnPrintInvoiceReport);
			this.PanelInvoiceReport.Controls.Add(this.DateInvoiceReport);
			this.PanelInvoiceReport.Controls.Add(this.InvoiceSummaryGrid);
			this.PanelInvoiceReport.Controls.Add(this.InvoiceReportGrid);
			this.PanelInvoiceReport.Location = new Point(0x10, 80);
			this.PanelInvoiceReport.Name = "PanelInvoiceReport";
			this.PanelInvoiceReport.Size = new Size(0x3e0, 0x260);
			this.PanelInvoiceReport.TabIndex = 6;
			this.BtnPrintInvoiceReport.BackColor = Color.Transparent;
			this.BtnPrintInvoiceReport.Blue = 2f;
			this.BtnPrintInvoiceReport.Cursor = Cursors.Hand;
			this.BtnPrintInvoiceReport.Green = 1f;
			this.BtnPrintInvoiceReport.ImageClick = (Image) manager.GetObject("BtnPrintInvoiceReport.ImageClick");
			this.BtnPrintInvoiceReport.ImageClickIndex = 1;
			this.BtnPrintInvoiceReport.ImageIndex = 0;
			this.BtnPrintInvoiceReport.ImageList = this.ButtonLiteImgList;
			this.BtnPrintInvoiceReport.IsLock = false;
			this.BtnPrintInvoiceReport.Location = new Point(8, 5);
			this.BtnPrintInvoiceReport.Name = "BtnPrintInvoiceReport";
			this.BtnPrintInvoiceReport.ObjectValue = null;
			this.BtnPrintInvoiceReport.Red = 1f;
			this.BtnPrintInvoiceReport.Size = new Size(110, 40);
			this.BtnPrintInvoiceReport.TabIndex = 8;
			this.BtnPrintInvoiceReport.Text = "Print List";
			this.BtnPrintInvoiceReport.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnPrintInvoiceReport.Click += new EventHandler(this.BtnPrintInvoiceReport_Click);
			this.ButtonLiteImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.ButtonLiteImgList.ImageSize = new Size(110, 40);
			this.ButtonLiteImgList.ImageStream = (ImageListStreamer) manager.GetObject("ButtonLiteImgList.ImageStream");
			this.ButtonLiteImgList.TransparentColor = Color.Transparent;
			this.DateInvoiceReport.CalendarFont = new Font("Tahoma", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.DateInvoiceReport.CustomFormat = "";
			this.DateInvoiceReport.Font = new Font("Tahoma", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.DateInvoiceReport.Format = DateTimePickerFormat.Short;
			this.DateInvoiceReport.Location = new Point(0x338, 8);
			this.DateInvoiceReport.Name = "DateInvoiceReport";
			this.DateInvoiceReport.RightToLeft = RightToLeft.No;
			this.DateInvoiceReport.Size = new Size(0x98, 30);
			this.DateInvoiceReport.TabIndex = 7;
			this.DateInvoiceReport.ValueChanged += new EventHandler(this.DateInvoiceReport_ValueChanged);
			this.InvoiceSummaryGrid.AllowSorting = false;
			this.InvoiceSummaryGrid.AlternatingBackColor = Color.FromArgb(0xc0, 0xff, 0xff);
			this.InvoiceSummaryGrid.BackColor = Color.White;
			this.InvoiceSummaryGrid.BackgroundColor = Color.FromArgb(0xe0, 0xe0, 0xe0);
			this.InvoiceSummaryGrid.BorderStyle = BorderStyle.None;
			this.InvoiceSummaryGrid.CaptionBackColor = Color.Black;
			this.InvoiceSummaryGrid.CaptionForeColor = Color.White;
			this.InvoiceSummaryGrid.CaptionVisible = false;
			this.InvoiceSummaryGrid.DataMember = "";
			this.InvoiceSummaryGrid.FlatMode = true;
			this.InvoiceSummaryGrid.Font = new Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.InvoiceSummaryGrid.ForeColor = Color.Black;
			this.InvoiceSummaryGrid.GridLineColor = Color.Silver;
			this.InvoiceSummaryGrid.HeaderBackColor = Color.Black;
			this.InvoiceSummaryGrid.HeaderForeColor = Color.White;
			this.InvoiceSummaryGrid.Location = new Point(8, 0x200);
			this.InvoiceSummaryGrid.Name = "InvoiceSummaryGrid";
			this.InvoiceSummaryGrid.PreferredRowHeight = 0x20;
			this.InvoiceSummaryGrid.ReadOnly = true;
			this.InvoiceSummaryGrid.RowHeadersVisible = false;
			this.InvoiceSummaryGrid.SelectionBackColor = Color.Navy;
			this.InvoiceSummaryGrid.Size = new Size(0x3d0, 90);
			this.InvoiceSummaryGrid.TabIndex = 4;
			this.InvoiceReportGrid.AllowSorting = false;
			this.InvoiceReportGrid.AlternatingBackColor = Color.FromArgb(0xc0, 0xff, 0xff);
			this.InvoiceReportGrid.BackColor = Color.White;
			this.InvoiceReportGrid.BackgroundColor = Color.FromArgb(0xe0, 0xe0, 0xe0);
			this.InvoiceReportGrid.BorderStyle = BorderStyle.None;
			this.InvoiceReportGrid.CaptionBackColor = Color.Black;
			this.InvoiceReportGrid.CaptionForeColor = Color.White;
			this.InvoiceReportGrid.CaptionVisible = false;
			this.InvoiceReportGrid.DataMember = "";
			this.InvoiceReportGrid.FlatMode = true;
			this.InvoiceReportGrid.Font = new Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.InvoiceReportGrid.ForeColor = Color.Black;
			this.InvoiceReportGrid.GridLineColor = Color.Silver;
			this.InvoiceReportGrid.HeaderBackColor = Color.Black;
			this.InvoiceReportGrid.HeaderForeColor = Color.White;
			this.InvoiceReportGrid.Location = new Point(8, 0x30);
			this.InvoiceReportGrid.Name = "InvoiceReportGrid";
			this.InvoiceReportGrid.PreferredRowHeight = 0x20;
			this.InvoiceReportGrid.ReadOnly = true;
			this.InvoiceReportGrid.RowHeadersVisible = false;
			this.InvoiceReportGrid.SelectionBackColor = Color.Navy;
			this.InvoiceReportGrid.Size = new Size(0x3d0, 0x1d0);
			this.InvoiceReportGrid.TabIndex = 3;
			this.InvoiceReportGrid.Click += new EventHandler(this.InvoiceReportGrid_Click);
			this.InvoiceReportGrid.DoubleClick += new EventHandler(this.InvoiceReportGrid_Click);
			this.BtnMaintenance.BackColor = Color.Transparent;
			this.BtnMaintenance.Blue = 1f;
			this.BtnMaintenance.Cursor = Cursors.Hand;
			this.BtnMaintenance.Green = 2f;
			this.BtnMaintenance.ImageClick = (Image) manager.GetObject("BtnMaintenance.ImageClick");
			this.BtnMaintenance.ImageClickIndex = 1;
			this.BtnMaintenance.ImageIndex = 0;
			this.BtnMaintenance.ImageList = this.ButtonImgList;
			this.BtnMaintenance.IsLock = false;
			this.BtnMaintenance.Location = new Point(0x180, 0x2b0);
			this.BtnMaintenance.Name = "BtnMaintenance";
			this.BtnMaintenance.ObjectValue = null;
			this.BtnMaintenance.Red = 1f;
			this.BtnMaintenance.Size = new Size(110, 60);
			this.BtnMaintenance.TabIndex = 7;
			this.BtnMaintenance.Text = "Maintenance";
			this.BtnMaintenance.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnMaintenance.Click += new EventHandler(this.BtnMaintenance_Click);
			this.BtnMaintenance.DoubleClick += new EventHandler(this.BtnMaintenance_Click);
			this.PanelMaintenance.BackColor = Color.FromArgb(0xff, 0xc0, 0xff);
			this.PanelMaintenance.Controls.Add(this.groupPanel6);
			this.PanelMaintenance.Controls.Add(this.groupPanel3);
			this.PanelMaintenance.Controls.Add(this.groupPanel2);
			this.PanelMaintenance.Controls.Add(this.groupPanel1);
			this.PanelMaintenance.Location = new Point(0x10, 80);
			this.PanelMaintenance.Name = "PanelMaintenance";
			this.PanelMaintenance.Size = new Size(0x3e0, 0x260);
			this.PanelMaintenance.TabIndex = 8;
			this.groupPanel6.BackColor = Color.Transparent;
			this.groupPanel6.Caption = "Printer";
			this.groupPanel6.Controls.Add(this.BtnPrinterSave);
			this.groupPanel6.Controls.Add(this.LstBillPrinter);
			this.groupPanel6.Controls.Add(this.LstReceiptPrinter);
			this.groupPanel6.Controls.Add(this.LstKitchenPrinter);
			this.groupPanel6.Controls.Add(this.label11);
			this.groupPanel6.Controls.Add(this.label10);
			this.groupPanel6.Controls.Add(this.label7);
			this.groupPanel6.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.groupPanel6.Location = new Point(720, 0x110);
			this.groupPanel6.Name = "groupPanel6";
			this.groupPanel6.ShowHeader = true;
			this.groupPanel6.Size = new Size(0x100, 0x130);
			this.groupPanel6.TabIndex = 13;
			this.BtnPrinterSave.BackColor = Color.Transparent;
			this.BtnPrinterSave.Blue = 2f;
			this.BtnPrinterSave.Cursor = Cursors.Hand;
			this.BtnPrinterSave.Font = new Font("Tahoma", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.BtnPrinterSave.Green = 1f;
			this.BtnPrinterSave.ImageClick = (Image) manager.GetObject("BtnPrinterSave.ImageClick");
			this.BtnPrinterSave.ImageClickIndex = 1;
			this.BtnPrinterSave.ImageIndex = 0;
			this.BtnPrinterSave.ImageList = this.ButtonImgList;
			this.BtnPrinterSave.IsLock = false;
			this.BtnPrinterSave.Location = new Point(80, 0xe8);
			this.BtnPrinterSave.Name = "BtnPrinterSave";
			this.BtnPrinterSave.ObjectValue = null;
			this.BtnPrinterSave.Red = 1f;
			this.BtnPrinterSave.Size = new Size(110, 60);
			this.BtnPrinterSave.TabIndex = 14;
			this.BtnPrinterSave.Text = "Save";
			this.BtnPrinterSave.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnPrinterSave.Click += new EventHandler(this.BtnPrinterSave_Click);
			this.LstBillPrinter.Location = new Point(0x10, 0xc0);
			this.LstBillPrinter.Name = "LstBillPrinter";
			this.LstBillPrinter.Size = new Size(0xe0, 0x1b);
			this.LstBillPrinter.TabIndex = 13;
			this.LstReceiptPrinter.Location = new Point(0x10, 0x80);
			this.LstReceiptPrinter.Name = "LstReceiptPrinter";
			this.LstReceiptPrinter.Size = new Size(0xe0, 0x1b);
			this.LstReceiptPrinter.TabIndex = 12;
			this.LstKitchenPrinter.Location = new Point(0x10, 0x40);
			this.LstKitchenPrinter.Name = "LstKitchenPrinter";
			this.LstKitchenPrinter.Size = new Size(0xe0, 0x1b);
			this.LstKitchenPrinter.TabIndex = 11;
			this.label11.Font = new Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label11.Location = new Point(0x10, 0xa8);
			this.label11.Name = "label11";
			this.label11.Size = new Size(0x70, 0x10);
			this.label11.TabIndex = 10;
			this.label11.Text = "Bill Printer";
			this.label10.Font = new Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label10.Location = new Point(0x10, 0x68);
			this.label10.Name = "label10";
			this.label10.Size = new Size(0x70, 0x10);
			this.label10.TabIndex = 9;
			this.label10.Text = "Receipt Printer";
			this.label7.Font = new Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label7.Location = new Point(0x10, 40);
			this.label7.Name = "label7";
			this.label7.Size = new Size(0x70, 0x10);
			this.label7.TabIndex = 8;
			this.label7.Text = "Kitchen Printer";
			this.groupPanel3.BackColor = Color.Transparent;
			this.groupPanel3.Caption = "Backup Database";
			this.groupPanel3.Controls.Add(this.label5);
			this.groupPanel3.Controls.Add(this.BtnBackup);
			this.groupPanel3.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.groupPanel3.Location = new Point(720, 0x10);
			this.groupPanel3.Name = "groupPanel3";
			this.groupPanel3.ShowHeader = true;
			this.groupPanel3.Size = new Size(0x100, 0xf8);
			this.groupPanel3.TabIndex = 12;
			this.label5.Font = new Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label5.Location = new Point(0x18, 0x40);
			this.label5.Name = "label5";
			this.label5.Size = new Size(0xd8, 40);
			this.label5.TabIndex = 7;
			this.label5.Text = "Click \"Backup\" button for start backup database to disk.";
			this.BtnBackup.BackColor = Color.Transparent;
			this.BtnBackup.Blue = 2f;
			this.BtnBackup.Cursor = Cursors.Hand;
			this.BtnBackup.Font = new Font("Tahoma", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.BtnBackup.Green = 1f;
			this.BtnBackup.ImageClick = (Image) manager.GetObject("BtnBackup.ImageClick");
			this.BtnBackup.ImageClickIndex = 1;
			this.BtnBackup.ImageIndex = 0;
			this.BtnBackup.ImageList = this.ButtonImgList;
			this.BtnBackup.IsLock = false;
			this.BtnBackup.Location = new Point(80, 0xb0);
			this.BtnBackup.Name = "BtnBackup";
			this.BtnBackup.ObjectValue = null;
			this.BtnBackup.Red = 1f;
			this.BtnBackup.Size = new Size(110, 60);
			this.BtnBackup.TabIndex = 6;
			this.BtnBackup.Text = "Backup";
			this.BtnBackup.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnBackup.Click += new EventHandler(this.BtnBackup_Click);
			this.groupPanel2.BackColor = Color.Transparent;
			this.groupPanel2.Caption = "Export Invoice";
			this.groupPanel2.Controls.Add(this.DateExportTo);
			this.groupPanel2.Controls.Add(this.label9);
			this.groupPanel2.Controls.Add(this.label6);
			this.groupPanel2.Controls.Add(this.BtnExport);
			this.groupPanel2.Controls.Add(this.DateExportFrom);
			this.groupPanel2.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.groupPanel2.Location = new Point(0x170, 0x10);
			this.groupPanel2.Name = "groupPanel2";
			this.groupPanel2.ShowHeader = true;
			this.groupPanel2.Size = new Size(0x150, 0xf8);
			this.groupPanel2.TabIndex = 11;
			this.DateExportTo.CalendarFont = new Font("Tahoma", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.DateExportTo.CustomFormat = "";
			this.DateExportTo.Font = new Font("Tahoma", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.DateExportTo.Format = DateTimePickerFormat.Short;
			this.DateExportTo.Location = new Point(0x88, 0x70);
			this.DateExportTo.Name = "DateExportTo";
			this.DateExportTo.RightToLeft = RightToLeft.No;
			this.DateExportTo.Size = new Size(0x98, 30);
			this.DateExportTo.TabIndex = 12;
			this.label9.Font = new Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label9.Location = new Point(0x58, 120);
			this.label9.Name = "label9";
			this.label9.Size = new Size(0x20, 0x10);
			this.label9.TabIndex = 11;
			this.label9.Text = "to";
			this.label6.Font = new Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label6.Location = new Point(0x20, 0x48);
			this.label6.Name = "label6";
			this.label6.Size = new Size(0x58, 0x10);
			this.label6.TabIndex = 8;
			this.label6.Text = "Export from";
			this.BtnExport.BackColor = Color.Transparent;
			this.BtnExport.Blue = 2f;
			this.BtnExport.Cursor = Cursors.Hand;
			this.BtnExport.Font = new Font("Tahoma", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.BtnExport.Green = 1f;
			this.BtnExport.ImageClick = (Image) manager.GetObject("BtnExport.ImageClick");
			this.BtnExport.ImageClickIndex = 1;
			this.BtnExport.ImageIndex = 0;
			this.BtnExport.ImageList = this.ButtonImgList;
			this.BtnExport.IsLock = false;
			this.BtnExport.Location = new Point(0xc0, 0xb0);
			this.BtnExport.Name = "BtnExport";
			this.BtnExport.ObjectValue = null;
			this.BtnExport.Red = 1f;
			this.BtnExport.Size = new Size(110, 60);
			this.BtnExport.TabIndex = 5;
			this.BtnExport.Text = "Export";
			this.BtnExport.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnExport.Click += new EventHandler(this.BtnExport_Click);
			this.DateExportFrom.CalendarFont = new Font("Tahoma", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.DateExportFrom.CustomFormat = "";
			this.DateExportFrom.Font = new Font("Tahoma", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.DateExportFrom.Format = DateTimePickerFormat.Short;
			this.DateExportFrom.Location = new Point(0x88, 0x40);
			this.DateExportFrom.Name = "DateExportFrom";
			this.DateExportFrom.RightToLeft = RightToLeft.No;
			this.DateExportFrom.Size = new Size(0x98, 30);
			this.DateExportFrom.TabIndex = 6;
			this.groupPanel1.BackColor = Color.Transparent;
			this.groupPanel1.Caption = "Print Summary";
			this.groupPanel1.Controls.Add(this.label1);
			this.groupPanel1.Controls.Add(this.label2);
			this.groupPanel1.Controls.Add(this.label3);
			this.groupPanel1.Controls.Add(this.label4);
			this.groupPanel1.Controls.Add(this.BtnSumMenuType);
			this.groupPanel1.Controls.Add(this.BtnSumPayment);
			this.groupPanel1.Controls.Add(this.SummaryDate);
			this.groupPanel1.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.groupPanel1.Location = new Point(0x10, 0x10);
			this.groupPanel1.Name = "groupPanel1";
			this.groupPanel1.ShowHeader = true;
			this.groupPanel1.Size = new Size(0x150, 0xf8);
			this.groupPanel1.TabIndex = 10;
			this.label1.Location = new Point(0x10, 40);
			this.label1.Name = "label1";
			this.label1.Size = new Size(0x40, 0x18);
			this.label1.TabIndex = 7;
			this.label1.Text = "Step 1";
			this.label2.Font = new Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label2.Location = new Point(0x20, 0x48);
			this.label2.Name = "label2";
			this.label2.Size = new Size(0x58, 0x10);
			this.label2.TabIndex = 8;
			this.label2.Text = "Select date";
			this.label3.Location = new Point(0x10, 120);
			this.label3.Name = "label3";
			this.label3.TabIndex = 9;
			this.label3.Text = "Step 2";
			this.label4.Font = new Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label4.Location = new Point(0x20, 0x98);
			this.label4.Name = "label4";
			this.label4.Size = new Size(0xd8, 0x10);
			this.label4.TabIndex = 10;
			this.label4.Text = "Select print summary type";
			this.BtnSumMenuType.BackColor = Color.Transparent;
			this.BtnSumMenuType.Blue = 2f;
			this.BtnSumMenuType.Cursor = Cursors.Hand;
			this.BtnSumMenuType.Font = new Font("Tahoma", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.BtnSumMenuType.Green = 1f;
			this.BtnSumMenuType.ImageClick = (Image) manager.GetObject("BtnSumMenuType.ImageClick");
			this.BtnSumMenuType.ImageClickIndex = 1;
			this.BtnSumMenuType.ImageIndex = 0;
			this.BtnSumMenuType.ImageList = this.ButtonImgList;
			this.BtnSumMenuType.IsLock = false;
			this.BtnSumMenuType.Location = new Point(0x38, 0xb0);
			this.BtnSumMenuType.Name = "BtnSumMenuType";
			this.BtnSumMenuType.ObjectValue = null;
			this.BtnSumMenuType.Red = 1f;
			this.BtnSumMenuType.Size = new Size(110, 60);
			this.BtnSumMenuType.TabIndex = 4;
			this.BtnSumMenuType.Text = "Print Summary by Menu Type";
			this.BtnSumMenuType.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnSumMenuType.Click += new EventHandler(this.BtnSumMenuType_Click);
			this.BtnSumPayment.BackColor = Color.Transparent;
			this.BtnSumPayment.Blue = 2f;
			this.BtnSumPayment.Cursor = Cursors.Hand;
			this.BtnSumPayment.Font = new Font("Tahoma", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.BtnSumPayment.Green = 1f;
			this.BtnSumPayment.ImageClick = (Image) manager.GetObject("BtnSumPayment.ImageClick");
			this.BtnSumPayment.ImageClickIndex = 1;
			this.BtnSumPayment.ImageIndex = 0;
			this.BtnSumPayment.ImageList = this.ButtonImgList;
			this.BtnSumPayment.IsLock = false;
			this.BtnSumPayment.Location = new Point(0xb8, 0xb0);
			this.BtnSumPayment.Name = "BtnSumPayment";
			this.BtnSumPayment.ObjectValue = null;
			this.BtnSumPayment.Red = 1f;
			this.BtnSumPayment.Size = new Size(110, 60);
			this.BtnSumPayment.TabIndex = 5;
			this.BtnSumPayment.Text = "Print Summary by Payment Method";
			this.BtnSumPayment.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnSumPayment.Click += new EventHandler(this.BtnSumPayment_Click);
			this.SummaryDate.CalendarFont = new Font("Tahoma", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.SummaryDate.CustomFormat = "";
			this.SummaryDate.Font = new Font("Tahoma", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.SummaryDate.Format = DateTimePickerFormat.Short;
			this.SummaryDate.Location = new Point(0x88, 0x40);
			this.SummaryDate.Name = "SummaryDate";
			this.SummaryDate.RightToLeft = RightToLeft.No;
			this.SummaryDate.Size = new Size(0x98, 30);
			this.SummaryDate.TabIndex = 6;
			this.SummaryDate.ValueChanged += new EventHandler(this.SummaryDate_ValueChanged);
			this.groupPanel5.BackColor = Color.Transparent;
			this.groupPanel5.Caption = "Print Tax Summary";
			this.groupPanel5.Controls.Add(this.ListTaxYear);
			this.groupPanel5.Controls.Add(this.ListTaxMonth);
			this.groupPanel5.Controls.Add(this.label8);
			this.groupPanel5.Controls.Add(this.BtnSumTax);
			this.groupPanel5.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.groupPanel5.Location = new Point(0x10, 0x10);
			this.groupPanel5.Name = "groupPanel5";
			this.groupPanel5.ShowHeader = true;
			this.groupPanel5.Size = new Size(0x150, 0xf8);
			this.groupPanel5.TabIndex = 13;
			this.ListTaxYear.Location = new Point(0xd8, 0x40);
			this.ListTaxYear.Name = "ListTaxYear";
			this.ListTaxYear.Size = new Size(0x48, 0x1b);
			this.ListTaxYear.TabIndex = 10;
			this.ListTaxYear.Text = "2005";
			this.ListTaxMonth.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" });
			this.ListTaxMonth.Location = new Point(0x98, 0x40);
			this.ListTaxMonth.Name = "ListTaxMonth";
			this.ListTaxMonth.Size = new Size(0x38, 0x1b);
			this.ListTaxMonth.TabIndex = 9;
			this.ListTaxMonth.Text = "1";
			this.label8.Font = new Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label8.Location = new Point(0x20, 0x48);
			this.label8.Name = "label8";
			this.label8.Size = new Size(0x58, 0x10);
			this.label8.TabIndex = 8;
			this.label8.Text = "Select month";
			this.BtnSumTax.BackColor = Color.Transparent;
			this.BtnSumTax.Blue = 2f;
			this.BtnSumTax.Cursor = Cursors.Hand;
			this.BtnSumTax.Font = new Font("Tahoma", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.BtnSumTax.Green = 1f;
			this.BtnSumTax.ImageClick = (Image) manager.GetObject("BtnSumTax.ImageClick");
			this.BtnSumTax.ImageClickIndex = 1;
			this.BtnSumTax.ImageIndex = 0;
			this.BtnSumTax.ImageList = this.ButtonImgList;
			this.BtnSumTax.IsLock = false;
			this.BtnSumTax.Location = new Point(0xb8, 0xb0);
			this.BtnSumTax.Name = "BtnSumTax";
			this.BtnSumTax.ObjectValue = null;
			this.BtnSumTax.Red = 1f;
			this.BtnSumTax.Size = new Size(110, 60);
			this.BtnSumTax.TabIndex = 5;
			this.BtnSumTax.Text = "Print Tax Summary";
			this.BtnSumTax.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnSumTax.Click += new EventHandler(this.BtnSumTax_Click);
			this.BtnManagerMaintenance.BackColor = Color.Transparent;
			this.BtnManagerMaintenance.Blue = 0.5f;
			this.BtnManagerMaintenance.Cursor = Cursors.Hand;
			this.BtnManagerMaintenance.ForeColor = Color.White;
			this.BtnManagerMaintenance.Green = 0.5f;
			this.BtnManagerMaintenance.ImageClick = (Image) manager.GetObject("BtnManagerMaintenance.ImageClick");
			this.BtnManagerMaintenance.ImageClickIndex = 1;
			this.BtnManagerMaintenance.ImageIndex = 0;
			this.BtnManagerMaintenance.ImageList = this.ButtonImgList;
			this.BtnManagerMaintenance.IsLock = false;
			this.BtnManagerMaintenance.Location = new Point(760, 0x2b0);
			this.BtnManagerMaintenance.Name = "BtnManagerMaintenance";
			this.BtnManagerMaintenance.ObjectValue = null;
			this.BtnManagerMaintenance.Red = 0.5f;
			this.BtnManagerMaintenance.Size = new Size(110, 60);
			this.BtnManagerMaintenance.TabIndex = 9;
			this.BtnManagerMaintenance.Text = "Manager Maintenance";
			this.BtnManagerMaintenance.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnManagerMaintenance.Click += new EventHandler(this.BtnManagerMaintenance_Click);
			this.BtnManagerMaintenance.DoubleClick += new EventHandler(this.BtnManagerMaintenance_Click);
			this.PanelManagerMaintenance.BackColor = Color.Gray;
			this.PanelManagerMaintenance.Controls.Add(this.groupPanel4);
			this.PanelManagerMaintenance.Location = new Point(0x10, 80);
			this.PanelManagerMaintenance.Name = "PanelManagerMaintenance";
			this.PanelManagerMaintenance.Size = new Size(0x3e0, 0x260);
			this.PanelManagerMaintenance.TabIndex = 10;
			this.groupPanel4.BackColor = Color.Transparent;
			this.groupPanel4.Caption = "Void Selected";
			this.groupPanel4.Controls.Add(this.BtnDelete);
			this.groupPanel4.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.groupPanel4.Location = new Point(0x10, 0x10);
			this.groupPanel4.Name = "groupPanel4";
			this.groupPanel4.ShowHeader = true;
			this.groupPanel4.Size = new Size(0x150, 0x80);
			this.groupPanel4.TabIndex = 10;
			this.BtnDelete.BackColor = Color.Transparent;
			this.BtnDelete.Blue = 2f;
			this.BtnDelete.Cursor = Cursors.Hand;
			this.BtnDelete.Font = new Font("Tahoma", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.BtnDelete.Green = 2f;
			this.BtnDelete.ImageClick = (Image) manager.GetObject("BtnDelete.ImageClick");
			this.BtnDelete.ImageClickIndex = 1;
			this.BtnDelete.ImageIndex = 0;
			this.BtnDelete.ImageList = this.ButtonImgList;
			this.BtnDelete.IsLock = false;
			this.BtnDelete.Location = new Point(0xd0, 0x30);
			this.BtnDelete.Name = "BtnDelete";
			this.BtnDelete.ObjectValue = null;
			this.BtnDelete.Red = 1f;
			this.BtnDelete.Size = new Size(110, 60);
			this.BtnDelete.TabIndex = 5;
			this.BtnDelete.Text = "Void";
			this.BtnDelete.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnDelete.Click += new EventHandler(this.BtnDelete_Click);
			this.BtnReport.BackColor = Color.Transparent;
			this.BtnReport.Blue = 2f;
			this.BtnReport.Cursor = Cursors.Hand;
			this.BtnReport.Green = 2f;
			this.BtnReport.ImageClick = (Image) manager.GetObject("BtnReport.ImageClick");
			this.BtnReport.ImageClickIndex = 1;
			this.BtnReport.ImageIndex = 0;
			this.BtnReport.ImageList = this.ButtonImgList;
			this.BtnReport.IsLock = false;
			this.BtnReport.Location = new Point(0x108, 0x2b0);
			this.BtnReport.Name = "BtnReport";
			this.BtnReport.ObjectValue = null;
			this.BtnReport.Red = 1f;
			this.BtnReport.Size = new Size(110, 60);
			this.BtnReport.TabIndex = 11;
			this.BtnReport.Text = "Report";
			this.BtnReport.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnReport.Click += new EventHandler(this.BtnReport_Click);
			this.PanelReport.BackColor = Color.FromArgb(0xff, 0xc0, 0xc0);
			this.PanelReport.Controls.Add(this.groupPanel5);
			this.PanelReport.Location = new Point(0x10, 80);
			this.PanelReport.Name = "PanelReport";
			this.PanelReport.Size = new Size(0x3e0, 0x260);
			this.PanelReport.TabIndex = 12;
			this.AutoScaleBaseSize = new Size(6, 15);
			base.ClientSize = new Size(0x3fc, 0x2fc);
			base.Controls.Add(this.BtnReport);
			base.Controls.Add(this.BtnManagerMaintenance);
			base.Controls.Add(this.BtnMaintenance);
			base.Controls.Add(this.BtnInvoiceReport);
			base.Controls.Add(this.BtnSaleReport);
			base.Controls.Add(this.BtnMain);
			base.Controls.Add(this.PanelSalesReport);
			base.Controls.Add(this.PanelManagerMaintenance);
			base.Controls.Add(this.PanelReport);
			base.Controls.Add(this.PanelMaintenance);
			base.Controls.Add(this.PanelInvoiceReport);
			base.Name = "SalesForm";
			this.Text = "SalesForm";
			this.PanelSalesReport.ResumeLayout(false);
			this.TotalGrid.EndInit();
			this.ReportGrid.EndInit();
			this.PanelInvoiceReport.ResumeLayout(false);
			this.InvoiceSummaryGrid.EndInit();
			this.InvoiceReportGrid.EndInit();
			this.PanelMaintenance.ResumeLayout(false);
			this.groupPanel6.ResumeLayout(false);
			this.groupPanel3.ResumeLayout(false);
			this.groupPanel2.ResumeLayout(false);
			this.groupPanel1.ResumeLayout(false);
			this.groupPanel5.ResumeLayout(false);
			this.PanelManagerMaintenance.ResumeLayout(false);
			this.groupPanel4.ResumeLayout(false);
			this.PanelReport.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		private void InvoiceReportGrid_Click(object sender, EventArgs e)
		{
			if ((this.InvoiceReportGrid.CurrentCell.ColumnNumber == 4) && ((MainForm) base.MdiParent).User.IsManager())
			{
				int invoiceID = int.Parse(this.Table.Rows[this.InvoiceReportGrid.CurrentCell.RowNumber]["invoiceid"].ToString());
				bool hidden = !((bool) this.Table.Rows[this.InvoiceReportGrid.CurrentCell.RowNumber]["Selected"]);
				for (int i = 0; i < this.Table.Rows.Count; i++)
				{
					if (this.Table.Rows[i]["invoiceid"].ToString() == invoiceID.ToString())
					{
						this.Table.Rows[i]["Selected"] = hidden;
					}
				}
				new smartRestaurant.BusinessService.BusinessService().UpdateInvoiceHidden(invoiceID, hidden);
				this.UpdateInvoiceSummaryReport();
			}
			else if ((this.InvoiceReportGrid.CurrentCell.ColumnNumber == 5) || ((this.InvoiceReportGrid.CurrentCell.ColumnNumber == 4) && ((MainForm) base.MdiParent).User.IsAuditor()))
			{
				InvoiceViewerForm.Show(int.Parse(this.Table.Rows[this.InvoiceReportGrid.CurrentCell.RowNumber]["invoiceid"].ToString()));
			}
		}

		private void SummaryDate_ValueChanged(object sender, EventArgs e)
		{
			if (!this.Changed)
			{
				this.Changed = true;
				this.DateSalesReport.Value = this.SummaryDate.Value;
				this.DateInvoiceReport.Value = this.SummaryDate.Value;
				this.Changed = false;
			}
		}

		public override void UpdateForm()
		{
			int num;
			if (((MainForm) base.MdiParent).User.IsManager())
			{
				this.BtnManagerMaintenance.Visible = true;
			}
			else
			{
				this.BtnManagerMaintenance.Visible = false;
			}
			this.Changed = true;
			this.DateSalesReport.Value = DateTime.Today;
			this.DateInvoiceReport.Value = DateTime.Today;
			this.SummaryDate.Value = DateTime.Today;
			this.ListTaxMonth.Items.Clear();
			for (num = 1; num <= 12; num++)
			{
				this.ListTaxMonth.Items.Add(num);
			}
			this.ListTaxMonth.SelectedIndex = DateTime.Today.Month - 1;
			this.ListTaxYear.Items.Clear();
			for (num = DateTime.Today.Year - 9; num <= DateTime.Today.Year; num++)
			{
				this.ListTaxYear.Items.Add(num);
			}
			this.ListTaxYear.SelectedIndex = this.ListTaxYear.Items.Count - 1;
			this.Changed = false;
			this.UpdateSalesReport();
		}

		private void UpdateInvoiceReport()
		{
			this.PanelInvoiceReport.BringToFront();
			smartRestaurant.BusinessService.BusinessService service = new smartRestaurant.BusinessService.BusinessService();
			DataTable table = ReportConverter.Convert(service.GetInvoiceReport(this.DateInvoiceReport.Value, ((MainForm) base.MdiParent).User.EmployeeTypeID));
			if (table != null)
			{
				if (((MainForm) base.MdiParent).User.IsManager())
				{
					table.Columns.Add("Selected", typeof(bool));
				}
				table.Columns.Add("View", typeof(bool));
				table.Columns.Add("No.", typeof(string));
				string str = "";
				int num = 1;
				for (int i = 0; i < table.Rows.Count; i++)
				{
					bool flag1 = table.Rows[i]["hidden"].ToString() == "True";
					if (((MainForm) base.MdiParent).User.IsManager())
					{
						table.Rows[i]["Selected"] = table.Rows[i]["hidden"].ToString() == "True";
					}
					table.Rows[i]["View"] = false;
					if (str == table.Rows[i]["invoiceid"].ToString())
					{
						table.Rows[i]["No."] = "";
					}
					else
					{
						table.Rows[i]["No."] = num++.ToString();
						str = table.Rows[i]["invoiceid"].ToString();
					}
				}
				table.Columns.Remove("hidden");
				this.InvoiceReportGrid.DataSource = table;
				DataGridTableStyle style = new DataGridTableStyle();
				style.RowHeadersVisible = false;
				style.PreferredRowHeight = 0x20;
				style.PreferredColumnWidth = (this.InvoiceReportGrid.Width - 0x42) / (table.Columns.Count - 3);
				style.HeaderBackColor = Color.Black;
				style.HeaderForeColor = Color.White;
				style.AllowSorting = false;
				DataGridColumnStyle column = new DataGridTextBoxColumn();
				column.HeaderText = "No.";
				column.MappingName = "No.";
				column.Alignment = HorizontalAlignment.Center;
				column.Width = 50;
				style.GridColumnStyles.Add(column);
				for (int j = 2; j < (table.Columns.Count - 1); j++)
				{
					if (j >= 5)
					{
						column = new DataGridBoolColumn();
					}
					else
					{
						column = new DataGridTextBoxColumn();
					}
					column.HeaderText = table.Columns[j].ColumnName;
					column.MappingName = table.Columns[j].ColumnName;
					column.Alignment = HorizontalAlignment.Center;
					column.ReadOnly = false;
					style.GridColumnStyles.Add(column);
				}
				this.InvoiceReportGrid.TableStyles.Clear();
				this.InvoiceReportGrid.TableStyles.Add(style);
				this.Table = table;
				this.UpdateInvoiceSummaryReport();
			}
		}

		private void UpdateInvoiceSummaryReport()
		{
			smartRestaurant.BusinessService.BusinessService service = new smartRestaurant.BusinessService.BusinessService();
			DataTable table = ReportConverter.Convert(service.GetInvoiceSummaryReport(this.DateInvoiceReport.Value, ((MainForm) base.MdiParent).User.EmployeeTypeID));
			if (table != null)
			{
				for (int i = 0; i < table.Rows.Count; i++)
				{
					for (int k = 1; k < table.Columns.Count; k++)
					{
						table.Rows[i][k] = double.Parse(table.Rows[i][k].ToString()).ToString("N");
					}
				}
				this.InvoiceSummaryGrid.DataSource = table;
				DataGridTableStyle style = new DataGridTableStyle();
				style.RowHeadersVisible = false;
				style.PreferredRowHeight = 0x20;
				style.PreferredColumnWidth = (this.InvoiceSummaryGrid.Width - 0x10) / table.Columns.Count;
				style.HeaderBackColor = Color.Black;
				style.HeaderForeColor = Color.White;
				for (int j = 0; j < table.Columns.Count; j++)
				{
					DataGridColumnStyle column = new DataGridTextBoxColumn();
					if (j == 0)
					{
						column.HeaderText = "";
					}
					else
					{
						column.HeaderText = table.Columns[j].ColumnName;
					}
					column.MappingName = table.Columns[j].ColumnName;
					column.Alignment = HorizontalAlignment.Center;
					style.GridColumnStyles.Add(column);
				}
				this.InvoiceSummaryGrid.TableStyles.Clear();
				this.InvoiceSummaryGrid.TableStyles.Add(style);
			}
		}

		private void UpdateMaintenance()
		{
			this.PanelMaintenance.BringToFront();
			smartRestaurant.BusinessService.BusinessService service = new smartRestaurant.BusinessService.BusinessService();
			string[] installedPrinter = service.GetInstalledPrinter();
			this.LstKitchenPrinter.Items.Clear();
			this.LstKitchenPrinter.Items.AddRange(installedPrinter);
			this.LstKitchenPrinter.Text = service.GetKitchenPrinter();
			this.LstReceiptPrinter.Items.Clear();
			this.LstReceiptPrinter.Items.AddRange(installedPrinter);
			this.LstReceiptPrinter.Text = service.GetReceiptPrinter();
			this.LstBillPrinter.Items.Clear();
			this.LstBillPrinter.Items.AddRange(installedPrinter);
			this.LstBillPrinter.Text = service.GetBillPrinter();
		}

		private void UpdateManagerMaintenance()
		{
			this.PanelManagerMaintenance.BringToFront();
		}

		private void UpdateReports()
		{
			this.PanelReport.BringToFront();
		}

		private void UpdateSalesReport()
		{
			this.PanelSalesReport.BringToFront();
			smartRestaurant.BusinessService.BusinessService service = new smartRestaurant.BusinessService.BusinessService();
			DataTable table = ReportConverter.Convert(service.GetSalesReport(this.DateSalesReport.Value, ((MainForm) base.MdiParent).User.EmployeeTypeID));
			if (table != null)
			{
				double[] numArray = new double[table.Columns.Count - 1];
				for (int i = 0; i < table.Rows.Count; i++)
				{
					for (int num2 = 1; num2 < table.Columns.Count; num2++)
					{
						IntPtr ptr;
						double num3 = double.Parse(table.Rows[i][num2].ToString());
						table.Rows[i][num2] = num3.ToString("N");
						numArray[(int) (ptr = (IntPtr) (num2 - 1))] = numArray[(int) ptr] + num3;
					}
				}
				DataTable table2 = new DataTable();
				for (int j = 0; j < table.Columns.Count; j++)
				{
					table2.Columns.Add(new DataColumn(table.Columns[j].ColumnName));
				}
				DataRow row = table2.NewRow();
				row[0] = "Total";
				for (int k = 1; k < table.Columns.Count; k++)
				{
					row[k] = numArray[k - 1].ToString("N");
				}
				table2.Rows.Add(row);
				this.ReportGrid.DataSource = table;
				DataGridTableStyle style = new DataGridTableStyle();
				style.RowHeadersVisible = false;
				style.PreferredRowHeight = 0x20;
				style.PreferredColumnWidth = (this.ReportGrid.Width - 0x10) / table.Columns.Count;
				style.HeaderBackColor = Color.Black;
				style.HeaderForeColor = Color.White;
				for (int m = 0; m < table.Columns.Count; m++)
				{
					DataGridColumnStyle column = new DataGridTextBoxColumn();
					if (m == 0)
					{
						column.HeaderText = "";
					}
					else
					{
						column.HeaderText = table.Columns[m].ColumnName;
					}
					column.MappingName = table.Columns[m].ColumnName;
					column.Alignment = HorizontalAlignment.Center;
					style.GridColumnStyles.Add(column);
				}
				this.ReportGrid.TableStyles.Clear();
				this.ReportGrid.TableStyles.Add(style);
				this.TotalGrid.DataSource = table2;
				style = new DataGridTableStyle();
				style.RowHeadersVisible = false;
				style.ColumnHeadersVisible = false;
				style.PreferredRowHeight = 0x20;
				style.PreferredColumnWidth = (this.TotalGrid.Width - 0x10) / table2.Columns.Count;
				style.BackColor = Color.DarkRed;
				style.ForeColor = Color.White;
				for (int n = 0; n < table2.Columns.Count; n++)
				{
					DataGridColumnStyle style3 = new DataGridTextBoxColumn();
					style3.HeaderText = "";
					style3.MappingName = table2.Columns[n].ColumnName;
					style3.Alignment = HorizontalAlignment.Center;
					style.GridColumnStyles.Add(style3);
				}
				this.TotalGrid.TableStyles.Clear();
				this.TotalGrid.TableStyles.Add(style);
			}
		}
	}
}
