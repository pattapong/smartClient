using smartRestaurant.Controls;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Resources;
using System.Windows.Forms;

namespace smartRestaurant.Utils
{
	public class CalculatorForm : Form
	{
		private static CalculatorForm instance;

		private ImageList NumberImgList;

		private ImageButton BtnNo1;

		private ImageButton BtnNo3;

		private ImageButton BtnNo2;

		private ImageButton BtnNo5;

		private ImageButton BtnNo6;

		private ImageButton BtnNo4;

		private ImageButton BtnNo8;

		private ImageButton BtnNo9;

		private ImageButton BtnNo7;

		private ImageButton BtnNo0;

		private ImageButton BtnDot;

		private ImageButton BtnEqual;

		private ImageButton BtnMultiple;

		private ImageButton BtnPlus;

		private ImageButton BtnMinus;

		private ImageButton BtnBackspace;

		private ImageButton BtnCurrentClear;

		private ImageButton BtnClear;

		private ImageButton BtnSwitch;

		private ImageButton BtnDivide;

		private Label FieldNumber;

		private IContainer components;

		private char sign;

		private bool showTotal;

		private double currentTotal;

		private double currentNumber;

		private ImageList ButtonLiteImgList;

		private ImageButton BtnClose;

		private double currentDot;

		static CalculatorForm()
		{
			CalculatorForm.instance = null;
		}

		public CalculatorForm()
		{
			this.InitializeComponent();
		}

		private void BtnClose_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		private void BtnFunction_Click(object sender, EventArgs e)
		{
			if (sender == this.BtnMultiple)
			{
				this.Compute();
				this.sign = '*';
				this.ClearCurrent();
			}
			else if (sender == this.BtnDivide)
			{
				this.Compute();
				this.sign = '/';
				this.ClearCurrent();
			}
			else if (sender == this.BtnPlus)
			{
				this.Compute();
				this.sign = '+';
				this.ClearCurrent();
			}
			else if (sender == this.BtnMinus)
			{
				this.Compute();
				this.sign = '-';
				this.ClearCurrent();
			}
			else if (sender == this.BtnEqual)
			{
				this.Compute();
				this.sign = '=';
				this.ClearCurrent();
			}
			else if (sender == this.BtnSwitch)
			{
				if (this.currentNumber != 0 || !(this.FieldNumber.Text != "0"))
				{
					this.currentNumber = -this.currentNumber;
				}
				else
				{
					this.currentTotal = -this.currentTotal;
					this.showTotal = true;
				}
			}
			else if (sender == this.BtnDot && this.currentDot == 1)
			{
				this.currentDot = 10;
				this.showTotal = false;
			}
			else if (sender == this.BtnClear)
			{
				this.ClearAll();
			}
			else if (sender == this.BtnCurrentClear)
			{
				this.ClearCurrent();
			}
			else if (sender == this.BtnBackspace)
			{
				if (this.currentDot <= 10)
				{
					this.currentDot = 1;
					this.currentNumber = (this.currentNumber - (double)((long)this.currentNumber % (long)10)) / 10;
				}
				else
				{
					this.currentDot /= 10;
					this.currentNumber *= this.currentDot;
					this.currentNumber = (this.currentNumber - (double)((long)this.currentNumber % (long)10)) / this.currentDot;
				}
			}
			this.ShowNumber();
		}

		private void BtnNo0_Click(object sender, EventArgs e)
		{
			double num;
			int length = this.currentNumber.ToString().Length;
			if (this.currentDot > 1)
			{
				length++;
			}
			if (length >= 15)
			{
				return;
			}
			if (sender == this.BtnNo0)
			{
				num = 0;
			}
			else if (sender == this.BtnNo1)
			{
				num = 1;
			}
			else if (sender == this.BtnNo2)
			{
				num = 2;
			}
			else if (sender == this.BtnNo3)
			{
				num = 3;
			}
			else if (sender == this.BtnNo4)
			{
				num = 4;
			}
			else if (sender == this.BtnNo5)
			{
				num = 5;
			}
			else if (sender == this.BtnNo6)
			{
				num = 6;
			}
			else if (sender == this.BtnNo7)
			{
				num = 7;
			}
			else if (sender != this.BtnNo8)
			{
				if (sender != this.BtnNo9)
				{
					return;
				}
				num = 9;
			}
			else
			{
				num = 8;
			}
			if (this.currentDot <= 1)
			{
				this.currentNumber = this.currentNumber * 10 + num;
			}
			else
			{
				this.currentNumber = this.currentNumber + num / this.currentDot;
				this.currentDot *= 10;
			}
			this.showTotal = false;
			this.ShowNumber();
		}

		private void ClearAll()
		{
			this.currentTotal = 0;
			this.sign = ' ';
			this.ClearCurrent();
			this.showTotal = true;
		}

		private void ClearCurrent()
		{
			this.currentDot = 1;
			this.currentNumber = 0;
		}

		private void Compute()
		{
			char chr = this.sign;
			switch (chr)
			{
				case '*':
				{
					this.currentTotal *= this.currentNumber;
					break;
				}
				case '+':
				{
					this.currentTotal += this.currentNumber;
					break;
				}
				case ',':
				case '.':
				{
					this.currentTotal = this.currentNumber;
					break;
				}
				case '-':
				{
					this.currentTotal -= this.currentNumber;
					break;
				}
				case '/':
				{
					if (this.currentNumber == 0)
					{
						this.currentTotal = 0;
						break;
					}
					else
					{
						this.currentTotal /= this.currentNumber;
						break;
					}
				}
				default:
				{
					if (chr == '=')
					{
						break;
					}
					goto case '.';
				}
			}
			this.showTotal = true;
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
			ResourceManager resourceManager = new ResourceManager(typeof(CalculatorForm));
			this.NumberImgList = new ImageList(this.components);
			this.BtnNo1 = new ImageButton();
			this.BtnNo3 = new ImageButton();
			this.BtnNo2 = new ImageButton();
			this.BtnNo5 = new ImageButton();
			this.BtnNo6 = new ImageButton();
			this.BtnNo4 = new ImageButton();
			this.BtnNo8 = new ImageButton();
			this.BtnNo9 = new ImageButton();
			this.BtnNo7 = new ImageButton();
			this.BtnNo0 = new ImageButton();
			this.BtnDot = new ImageButton();
			this.BtnEqual = new ImageButton();
			this.BtnMultiple = new ImageButton();
			this.BtnDivide = new ImageButton();
			this.BtnPlus = new ImageButton();
			this.BtnMinus = new ImageButton();
			this.BtnBackspace = new ImageButton();
			this.BtnCurrentClear = new ImageButton();
			this.BtnClear = new ImageButton();
			this.BtnSwitch = new ImageButton();
			this.FieldNumber = new Label();
			this.BtnClose = new ImageButton();
			this.ButtonLiteImgList = new ImageList(this.components);
			base.SuspendLayout();
			this.NumberImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.NumberImgList.ImageSize = new System.Drawing.Size(72, 60);
			this.NumberImgList.ImageStream = (ImageListStreamer)resourceManager.GetObject("NumberImgList.ImageStream");
			this.NumberImgList.TransparentColor = Color.Transparent;
			this.BtnNo1.BackColor = Color.Transparent;
			this.BtnNo1.Blue = 1f;
			this.BtnNo1.Cursor = Cursors.Hand;
			this.BtnNo1.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Point, 222);
			this.BtnNo1.Green = 1f;
			this.BtnNo1.ImageClick = (Image)resourceManager.GetObject("BtnNo1.ImageClick");
			this.BtnNo1.ImageClickIndex = 1;
			this.BtnNo1.ImageIndex = 0;
			this.BtnNo1.ImageList = this.NumberImgList;
			this.BtnNo1.Location = new Point(80, 216);
			this.BtnNo1.Name = "BtnNo1";
			this.BtnNo1.ObjectValue = null;
			this.BtnNo1.Red = 2f;
			this.BtnNo1.Size = new System.Drawing.Size(72, 60);
			this.BtnNo1.TabIndex = 0;
			this.BtnNo1.Text = "1";
			this.BtnNo1.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnNo1.Click += new EventHandler(this.BtnNo0_Click);
			this.BtnNo1.DoubleClick += new EventHandler(this.BtnNo0_Click);
			this.BtnNo3.BackColor = Color.Transparent;
			this.BtnNo3.Blue = 1f;
			this.BtnNo3.Cursor = Cursors.Hand;
			this.BtnNo3.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Point, 222);
			this.BtnNo3.Green = 1f;
			this.BtnNo3.ImageClick = (Image)resourceManager.GetObject("BtnNo3.ImageClick");
			this.BtnNo3.ImageClickIndex = 1;
			this.BtnNo3.ImageIndex = 0;
			this.BtnNo3.ImageList = this.NumberImgList;
			this.BtnNo3.Location = new Point(224, 216);
			this.BtnNo3.Name = "BtnNo3";
			this.BtnNo3.ObjectValue = null;
			this.BtnNo3.Red = 2f;
			this.BtnNo3.Size = new System.Drawing.Size(72, 60);
			this.BtnNo3.TabIndex = 1;
			this.BtnNo3.Text = "3";
			this.BtnNo3.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnNo3.Click += new EventHandler(this.BtnNo0_Click);
			this.BtnNo3.DoubleClick += new EventHandler(this.BtnNo0_Click);
			this.BtnNo2.BackColor = Color.Transparent;
			this.BtnNo2.Blue = 1f;
			this.BtnNo2.Cursor = Cursors.Hand;
			this.BtnNo2.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Point, 222);
			this.BtnNo2.Green = 1f;
			this.BtnNo2.ImageClick = (Image)resourceManager.GetObject("BtnNo2.ImageClick");
			this.BtnNo2.ImageClickIndex = 1;
			this.BtnNo2.ImageIndex = 0;
			this.BtnNo2.ImageList = this.NumberImgList;
			this.BtnNo2.Location = new Point(152, 216);
			this.BtnNo2.Name = "BtnNo2";
			this.BtnNo2.ObjectValue = null;
			this.BtnNo2.Red = 2f;
			this.BtnNo2.Size = new System.Drawing.Size(72, 60);
			this.BtnNo2.TabIndex = 2;
			this.BtnNo2.Text = "2";
			this.BtnNo2.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnNo2.Click += new EventHandler(this.BtnNo0_Click);
			this.BtnNo2.DoubleClick += new EventHandler(this.BtnNo0_Click);
			this.BtnNo5.BackColor = Color.Transparent;
			this.BtnNo5.Blue = 1f;
			this.BtnNo5.Cursor = Cursors.Hand;
			this.BtnNo5.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Point, 222);
			this.BtnNo5.Green = 1f;
			this.BtnNo5.ImageClick = (Image)resourceManager.GetObject("BtnNo5.ImageClick");
			this.BtnNo5.ImageClickIndex = 1;
			this.BtnNo5.ImageIndex = 0;
			this.BtnNo5.ImageList = this.NumberImgList;
			this.BtnNo5.Location = new Point(152, 152);
			this.BtnNo5.Name = "BtnNo5";
			this.BtnNo5.ObjectValue = null;
			this.BtnNo5.Red = 2f;
			this.BtnNo5.Size = new System.Drawing.Size(72, 60);
			this.BtnNo5.TabIndex = 5;
			this.BtnNo5.Text = "5";
			this.BtnNo5.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnNo5.Click += new EventHandler(this.BtnNo0_Click);
			this.BtnNo5.DoubleClick += new EventHandler(this.BtnNo0_Click);
			this.BtnNo6.BackColor = Color.Transparent;
			this.BtnNo6.Blue = 1f;
			this.BtnNo6.Cursor = Cursors.Hand;
			this.BtnNo6.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Point, 222);
			this.BtnNo6.Green = 1f;
			this.BtnNo6.ImageClick = (Image)resourceManager.GetObject("BtnNo6.ImageClick");
			this.BtnNo6.ImageClickIndex = 1;
			this.BtnNo6.ImageIndex = 0;
			this.BtnNo6.ImageList = this.NumberImgList;
			this.BtnNo6.Location = new Point(224, 152);
			this.BtnNo6.Name = "BtnNo6";
			this.BtnNo6.ObjectValue = null;
			this.BtnNo6.Red = 2f;
			this.BtnNo6.Size = new System.Drawing.Size(72, 60);
			this.BtnNo6.TabIndex = 4;
			this.BtnNo6.Text = "6";
			this.BtnNo6.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnNo6.Click += new EventHandler(this.BtnNo0_Click);
			this.BtnNo6.DoubleClick += new EventHandler(this.BtnNo0_Click);
			this.BtnNo4.BackColor = Color.Transparent;
			this.BtnNo4.Blue = 1f;
			this.BtnNo4.Cursor = Cursors.Hand;
			this.BtnNo4.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Point, 222);
			this.BtnNo4.Green = 1f;
			this.BtnNo4.ImageClick = (Image)resourceManager.GetObject("BtnNo4.ImageClick");
			this.BtnNo4.ImageClickIndex = 1;
			this.BtnNo4.ImageIndex = 0;
			this.BtnNo4.ImageList = this.NumberImgList;
			this.BtnNo4.Location = new Point(80, 152);
			this.BtnNo4.Name = "BtnNo4";
			this.BtnNo4.ObjectValue = null;
			this.BtnNo4.Red = 2f;
			this.BtnNo4.Size = new System.Drawing.Size(72, 60);
			this.BtnNo4.TabIndex = 3;
			this.BtnNo4.Text = "4";
			this.BtnNo4.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnNo4.Click += new EventHandler(this.BtnNo0_Click);
			this.BtnNo4.DoubleClick += new EventHandler(this.BtnNo0_Click);
			this.BtnNo8.BackColor = Color.Transparent;
			this.BtnNo8.Blue = 1f;
			this.BtnNo8.Cursor = Cursors.Hand;
			this.BtnNo8.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Point, 222);
			this.BtnNo8.Green = 1f;
			this.BtnNo8.ImageClick = (Image)resourceManager.GetObject("BtnNo8.ImageClick");
			this.BtnNo8.ImageClickIndex = 1;
			this.BtnNo8.ImageIndex = 0;
			this.BtnNo8.ImageList = this.NumberImgList;
			this.BtnNo8.Location = new Point(152, 88);
			this.BtnNo8.Name = "BtnNo8";
			this.BtnNo8.ObjectValue = null;
			this.BtnNo8.Red = 2f;
			this.BtnNo8.Size = new System.Drawing.Size(72, 60);
			this.BtnNo8.TabIndex = 8;
			this.BtnNo8.Text = "8";
			this.BtnNo8.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnNo8.Click += new EventHandler(this.BtnNo0_Click);
			this.BtnNo8.DoubleClick += new EventHandler(this.BtnNo0_Click);
			this.BtnNo9.BackColor = Color.Transparent;
			this.BtnNo9.Blue = 1f;
			this.BtnNo9.Cursor = Cursors.Hand;
			this.BtnNo9.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Point, 222);
			this.BtnNo9.Green = 1f;
			this.BtnNo9.ImageClick = (Image)resourceManager.GetObject("BtnNo9.ImageClick");
			this.BtnNo9.ImageClickIndex = 1;
			this.BtnNo9.ImageIndex = 0;
			this.BtnNo9.ImageList = this.NumberImgList;
			this.BtnNo9.Location = new Point(224, 88);
			this.BtnNo9.Name = "BtnNo9";
			this.BtnNo9.ObjectValue = null;
			this.BtnNo9.Red = 2f;
			this.BtnNo9.Size = new System.Drawing.Size(72, 60);
			this.BtnNo9.TabIndex = 7;
			this.BtnNo9.Text = "9";
			this.BtnNo9.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnNo9.Click += new EventHandler(this.BtnNo0_Click);
			this.BtnNo9.DoubleClick += new EventHandler(this.BtnNo0_Click);
			this.BtnNo7.BackColor = Color.Transparent;
			this.BtnNo7.Blue = 1f;
			this.BtnNo7.Cursor = Cursors.Hand;
			this.BtnNo7.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Point, 222);
			this.BtnNo7.Green = 1f;
			this.BtnNo7.ImageClick = (Image)resourceManager.GetObject("BtnNo7.ImageClick");
			this.BtnNo7.ImageClickIndex = 1;
			this.BtnNo7.ImageIndex = 0;
			this.BtnNo7.ImageList = this.NumberImgList;
			this.BtnNo7.Location = new Point(80, 88);
			this.BtnNo7.Name = "BtnNo7";
			this.BtnNo7.ObjectValue = null;
			this.BtnNo7.Red = 2f;
			this.BtnNo7.Size = new System.Drawing.Size(72, 60);
			this.BtnNo7.TabIndex = 6;
			this.BtnNo7.Text = "7";
			this.BtnNo7.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnNo7.Click += new EventHandler(this.BtnNo0_Click);
			this.BtnNo7.DoubleClick += new EventHandler(this.BtnNo0_Click);
			this.BtnNo0.BackColor = Color.Transparent;
			this.BtnNo0.Blue = 1f;
			this.BtnNo0.Cursor = Cursors.Hand;
			this.BtnNo0.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Point, 222);
			this.BtnNo0.Green = 1f;
			this.BtnNo0.ImageClick = (Image)resourceManager.GetObject("BtnNo0.ImageClick");
			this.BtnNo0.ImageClickIndex = 1;
			this.BtnNo0.ImageIndex = 0;
			this.BtnNo0.ImageList = this.NumberImgList;
			this.BtnNo0.Location = new Point(80, 280);
			this.BtnNo0.Name = "BtnNo0";
			this.BtnNo0.ObjectValue = null;
			this.BtnNo0.Red = 2f;
			this.BtnNo0.Size = new System.Drawing.Size(72, 60);
			this.BtnNo0.TabIndex = 9;
			this.BtnNo0.Text = "0";
			this.BtnNo0.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnNo0.Click += new EventHandler(this.BtnNo0_Click);
			this.BtnNo0.DoubleClick += new EventHandler(this.BtnNo0_Click);
			this.BtnDot.BackColor = Color.Transparent;
			this.BtnDot.Blue = 1f;
			this.BtnDot.Cursor = Cursors.Hand;
			this.BtnDot.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Point, 222);
			this.BtnDot.Green = 2f;
			this.BtnDot.ImageClick = (Image)resourceManager.GetObject("BtnDot.ImageClick");
			this.BtnDot.ImageClickIndex = 1;
			this.BtnDot.ImageIndex = 0;
			this.BtnDot.ImageList = this.NumberImgList;
			this.BtnDot.Location = new Point(152, 280);
			this.BtnDot.Name = "BtnDot";
			this.BtnDot.ObjectValue = null;
			this.BtnDot.Red = 2f;
			this.BtnDot.Size = new System.Drawing.Size(72, 60);
			this.BtnDot.TabIndex = 10;
			this.BtnDot.Text = ".";
			this.BtnDot.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnDot.Click += new EventHandler(this.BtnFunction_Click);
			this.BtnDot.DoubleClick += new EventHandler(this.BtnFunction_Click);
			this.BtnEqual.BackColor = Color.Transparent;
			this.BtnEqual.Blue = 1f;
			this.BtnEqual.Cursor = Cursors.Hand;
			this.BtnEqual.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Point, 222);
			this.BtnEqual.Green = 2f;
			this.BtnEqual.ImageClick = (Image)resourceManager.GetObject("BtnEqual.ImageClick");
			this.BtnEqual.ImageClickIndex = 1;
			this.BtnEqual.ImageIndex = 0;
			this.BtnEqual.ImageList = this.NumberImgList;
			this.BtnEqual.Location = new Point(224, 280);
			this.BtnEqual.Name = "BtnEqual";
			this.BtnEqual.ObjectValue = null;
			this.BtnEqual.Red = 2f;
			this.BtnEqual.Size = new System.Drawing.Size(72, 60);
			this.BtnEqual.TabIndex = 11;
			this.BtnEqual.Text = "=";
			this.BtnEqual.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnEqual.Click += new EventHandler(this.BtnFunction_Click);
			this.BtnEqual.DoubleClick += new EventHandler(this.BtnFunction_Click);
			this.BtnMultiple.BackColor = Color.Transparent;
			this.BtnMultiple.Blue = 1f;
			this.BtnMultiple.Cursor = Cursors.Hand;
			this.BtnMultiple.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Point, 222);
			this.BtnMultiple.Green = 2f;
			this.BtnMultiple.ImageClick = (Image)resourceManager.GetObject("BtnMultiple.ImageClick");
			this.BtnMultiple.ImageClickIndex = 1;
			this.BtnMultiple.ImageIndex = 0;
			this.BtnMultiple.ImageList = this.NumberImgList;
			this.BtnMultiple.Location = new Point(296, 88);
			this.BtnMultiple.Name = "BtnMultiple";
			this.BtnMultiple.ObjectValue = null;
			this.BtnMultiple.Red = 2f;
			this.BtnMultiple.Size = new System.Drawing.Size(72, 60);
			this.BtnMultiple.TabIndex = 12;
			this.BtnMultiple.Text = "x";
			this.BtnMultiple.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnMultiple.Click += new EventHandler(this.BtnFunction_Click);
			this.BtnMultiple.DoubleClick += new EventHandler(this.BtnFunction_Click);
			this.BtnDivide.BackColor = Color.Transparent;
			this.BtnDivide.Blue = 1f;
			this.BtnDivide.Cursor = Cursors.Hand;
			this.BtnDivide.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Point, 222);
			this.BtnDivide.Green = 2f;
			this.BtnDivide.ImageClick = (Image)resourceManager.GetObject("BtnDivide.ImageClick");
			this.BtnDivide.ImageClickIndex = 1;
			this.BtnDivide.ImageIndex = 0;
			this.BtnDivide.ImageList = this.NumberImgList;
			this.BtnDivide.Location = new Point(296, 152);
			this.BtnDivide.Name = "BtnDivide";
			this.BtnDivide.ObjectValue = null;
			this.BtnDivide.Red = 2f;
			this.BtnDivide.Size = new System.Drawing.Size(72, 60);
			this.BtnDivide.TabIndex = 13;
			this.BtnDivide.Text = "/";
			this.BtnDivide.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnDivide.Click += new EventHandler(this.BtnFunction_Click);
			this.BtnDivide.DoubleClick += new EventHandler(this.BtnFunction_Click);
			this.BtnPlus.BackColor = Color.Transparent;
			this.BtnPlus.Blue = 1f;
			this.BtnPlus.Cursor = Cursors.Hand;
			this.BtnPlus.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Point, 222);
			this.BtnPlus.Green = 2f;
			this.BtnPlus.ImageClick = (Image)resourceManager.GetObject("BtnPlus.ImageClick");
			this.BtnPlus.ImageClickIndex = 1;
			this.BtnPlus.ImageIndex = 0;
			this.BtnPlus.ImageList = this.NumberImgList;
			this.BtnPlus.Location = new Point(296, 216);
			this.BtnPlus.Name = "BtnPlus";
			this.BtnPlus.ObjectValue = null;
			this.BtnPlus.Red = 2f;
			this.BtnPlus.Size = new System.Drawing.Size(72, 60);
			this.BtnPlus.TabIndex = 14;
			this.BtnPlus.Text = "+";
			this.BtnPlus.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnPlus.Click += new EventHandler(this.BtnFunction_Click);
			this.BtnPlus.DoubleClick += new EventHandler(this.BtnFunction_Click);
			this.BtnMinus.BackColor = Color.Transparent;
			this.BtnMinus.Blue = 1f;
			this.BtnMinus.Cursor = Cursors.Hand;
			this.BtnMinus.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Point, 222);
			this.BtnMinus.Green = 2f;
			this.BtnMinus.ImageClick = (Image)resourceManager.GetObject("BtnMinus.ImageClick");
			this.BtnMinus.ImageClickIndex = 1;
			this.BtnMinus.ImageIndex = 0;
			this.BtnMinus.ImageList = this.NumberImgList;
			this.BtnMinus.Location = new Point(296, 280);
			this.BtnMinus.Name = "BtnMinus";
			this.BtnMinus.ObjectValue = null;
			this.BtnMinus.Red = 2f;
			this.BtnMinus.Size = new System.Drawing.Size(72, 60);
			this.BtnMinus.TabIndex = 15;
			this.BtnMinus.Text = "-";
			this.BtnMinus.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnMinus.Click += new EventHandler(this.BtnFunction_Click);
			this.BtnMinus.DoubleClick += new EventHandler(this.BtnFunction_Click);
			this.BtnBackspace.BackColor = Color.Transparent;
			this.BtnBackspace.Blue = 2f;
			this.BtnBackspace.Cursor = Cursors.Hand;
			this.BtnBackspace.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Point, 222);
			this.BtnBackspace.Green = 2f;
			this.BtnBackspace.ImageClick = (Image)resourceManager.GetObject("BtnBackspace.ImageClick");
			this.BtnBackspace.ImageClickIndex = 1;
			this.BtnBackspace.ImageIndex = 0;
			this.BtnBackspace.ImageList = this.NumberImgList;
			this.BtnBackspace.Location = new Point(8, 216);
			this.BtnBackspace.Name = "BtnBackspace";
			this.BtnBackspace.ObjectValue = null;
			this.BtnBackspace.Red = 1f;
			this.BtnBackspace.Size = new System.Drawing.Size(72, 60);
			this.BtnBackspace.TabIndex = 19;
			this.BtnBackspace.Text = "<-";
			this.BtnBackspace.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnBackspace.Click += new EventHandler(this.BtnFunction_Click);
			this.BtnBackspace.DoubleClick += new EventHandler(this.BtnFunction_Click);
			this.BtnCurrentClear.BackColor = Color.Transparent;
			this.BtnCurrentClear.Blue = 2f;
			this.BtnCurrentClear.Cursor = Cursors.Hand;
			this.BtnCurrentClear.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Point, 222);
			this.BtnCurrentClear.Green = 2f;
			this.BtnCurrentClear.ImageClick = (Image)resourceManager.GetObject("BtnCurrentClear.ImageClick");
			this.BtnCurrentClear.ImageClickIndex = 1;
			this.BtnCurrentClear.ImageIndex = 0;
			this.BtnCurrentClear.ImageList = this.NumberImgList;
			this.BtnCurrentClear.Location = new Point(8, 88);
			this.BtnCurrentClear.Name = "BtnCurrentClear";
			this.BtnCurrentClear.ObjectValue = null;
			this.BtnCurrentClear.Red = 1f;
			this.BtnCurrentClear.Size = new System.Drawing.Size(72, 60);
			this.BtnCurrentClear.TabIndex = 18;
			this.BtnCurrentClear.Text = "CE";
			this.BtnCurrentClear.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnCurrentClear.Click += new EventHandler(this.BtnFunction_Click);
			this.BtnCurrentClear.DoubleClick += new EventHandler(this.BtnFunction_Click);
			this.BtnClear.BackColor = Color.Transparent;
			this.BtnClear.Blue = 2f;
			this.BtnClear.Cursor = Cursors.Hand;
			this.BtnClear.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Point, 222);
			this.BtnClear.Green = 2f;
			this.BtnClear.ImageClick = (Image)resourceManager.GetObject("BtnClear.ImageClick");
			this.BtnClear.ImageClickIndex = 1;
			this.BtnClear.ImageIndex = 0;
			this.BtnClear.ImageList = this.NumberImgList;
			this.BtnClear.Location = new Point(8, 152);
			this.BtnClear.Name = "BtnClear";
			this.BtnClear.ObjectValue = null;
			this.BtnClear.Red = 1f;
			this.BtnClear.Size = new System.Drawing.Size(72, 60);
			this.BtnClear.TabIndex = 17;
			this.BtnClear.Text = "C";
			this.BtnClear.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnClear.Click += new EventHandler(this.BtnFunction_Click);
			this.BtnClear.DoubleClick += new EventHandler(this.BtnFunction_Click);
			this.BtnSwitch.BackColor = Color.Transparent;
			this.BtnSwitch.Blue = 1f;
			this.BtnSwitch.Cursor = Cursors.Hand;
			this.BtnSwitch.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Point, 222);
			this.BtnSwitch.Green = 2f;
			this.BtnSwitch.ImageClick = (Image)resourceManager.GetObject("BtnSwitch.ImageClick");
			this.BtnSwitch.ImageClickIndex = 1;
			this.BtnSwitch.ImageIndex = 0;
			this.BtnSwitch.ImageList = this.NumberImgList;
			this.BtnSwitch.Location = new Point(8, 280);
			this.BtnSwitch.Name = "BtnSwitch";
			this.BtnSwitch.ObjectValue = null;
			this.BtnSwitch.Red = 2f;
			this.BtnSwitch.Size = new System.Drawing.Size(72, 60);
			this.BtnSwitch.TabIndex = 16;
			this.BtnSwitch.Text = "+/-";
			this.BtnSwitch.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnSwitch.Click += new EventHandler(this.BtnFunction_Click);
			this.BtnSwitch.DoubleClick += new EventHandler(this.BtnFunction_Click);
			this.FieldNumber.BackColor = Color.FromArgb(255, 255, 192);
			this.FieldNumber.BorderStyle = BorderStyle.FixedSingle;
			this.FieldNumber.Cursor = Cursors.Hand;
			this.FieldNumber.Font = new System.Drawing.Font("Tahoma", 20.25f, FontStyle.Regular, GraphicsUnit.Point, 222);
			this.FieldNumber.Location = new Point(8, 40);
			this.FieldNumber.Name = "FieldNumber";
			this.FieldNumber.Size = new System.Drawing.Size(248, 40);
			this.FieldNumber.TabIndex = 41;
			this.FieldNumber.TextAlign = ContentAlignment.MiddleRight;
			this.BtnClose.BackColor = Color.Transparent;
			this.BtnClose.Blue = 2f;
			this.BtnClose.Cursor = Cursors.Hand;
			this.BtnClose.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Point, 222);
			this.BtnClose.Green = 2f;
			this.BtnClose.ImageClick = (Image)resourceManager.GetObject("BtnClose.ImageClick");
			this.BtnClose.ImageClickIndex = 1;
			this.BtnClose.ImageIndex = 0;
			this.BtnClose.ImageList = this.ButtonLiteImgList;
			this.BtnClose.Location = new Point(257, 40);
			this.BtnClose.Name = "BtnClose";
			this.BtnClose.ObjectValue = null;
			this.BtnClose.Red = 1f;
			this.BtnClose.Size = new System.Drawing.Size(112, 40);
			this.BtnClose.TabIndex = 42;
			this.BtnClose.Text = "Close";
			this.BtnClose.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnClose.Click += new EventHandler(this.BtnClose_Click);
			this.ButtonLiteImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.ButtonLiteImgList.ImageSize = new System.Drawing.Size(110, 40);
			this.ButtonLiteImgList.ImageStream = (ImageListStreamer)resourceManager.GetObject("ButtonLiteImgList.ImageStream");
			this.ButtonLiteImgList.TransparentColor = Color.Transparent;
			this.AutoScaleBaseSize = new System.Drawing.Size(9, 20);
			this.BackColor = Color.White;
			base.ClientSize = new System.Drawing.Size(376, 352);
			base.Controls.Add(this.BtnClose);
			base.Controls.Add(this.FieldNumber);
			base.Controls.Add(this.BtnBackspace);
			base.Controls.Add(this.BtnCurrentClear);
			base.Controls.Add(this.BtnClear);
			base.Controls.Add(this.BtnSwitch);
			base.Controls.Add(this.BtnMinus);
			base.Controls.Add(this.BtnPlus);
			base.Controls.Add(this.BtnDivide);
			base.Controls.Add(this.BtnMultiple);
			base.Controls.Add(this.BtnEqual);
			base.Controls.Add(this.BtnDot);
			base.Controls.Add(this.BtnNo0);
			base.Controls.Add(this.BtnNo8);
			base.Controls.Add(this.BtnNo9);
			base.Controls.Add(this.BtnNo7);
			base.Controls.Add(this.BtnNo5);
			base.Controls.Add(this.BtnNo6);
			base.Controls.Add(this.BtnNo4);
			base.Controls.Add(this.BtnNo2);
			base.Controls.Add(this.BtnNo3);
			base.Controls.Add(this.BtnNo1);
			this.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Point, 222);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.Name = "CalculatorForm";
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "Calculator";
			base.ResumeLayout(false);
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			Graphics graphics = pe.Graphics;
			Rectangle rectangle = new Rectangle(0, 0, base.Width, 29);
			LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rectangle, Color.FromArgb(103, 138, 198), Color.White, 90f);
			graphics.FillRectangle(linearGradientBrush, rectangle);
			rectangle = new Rectangle(0, 30, base.Width, base.Height - 30);
			linearGradientBrush = new LinearGradientBrush(rectangle, Color.FromArgb(230, 230, 230), Color.White, 180f);
			graphics.FillRectangle(linearGradientBrush, rectangle);
			Pen pen = new Pen(Color.FromArgb(180, 180, 180));
			graphics.DrawLine(pen, 0, 29, base.Width - 1, 29);
			graphics.DrawRectangle(pen, 0, 0, base.Width - 1, base.Height - 1);
			graphics.DrawString(this.Text, this.Font, Brushes.Black, 15f, 4f);
			base.OnPaint(pe);
		}

		public static void Show(bool clear)
		{
			if (CalculatorForm.instance == null)
			{
				CalculatorForm.instance = new CalculatorForm();
			}
			if (clear)
			{
				CalculatorForm.instance.ClearAll();
				CalculatorForm.instance.ShowDialog();
			}
		}

		private void ShowNumber()
		{
			double num;
			int num1;
			if (!this.showTotal)
			{
				num = this.currentNumber;
				num1 = (this.currentDot <= 1 ? 0 : (int)Math.Log10(this.currentDot) - 1);
			}
			else
			{
				num = this.currentTotal;
				num1 = 0;
				this.showTotal = false;
			}
			if (num1 <= 0)
			{
				this.FieldNumber.Text = num.ToString();
				return;
			}
			string str = "0.";
			for (int i = 0; i < num1; i++)
			{
				str = string.Concat(str, "0");
			}
			this.FieldNumber.Text = num.ToString(str);
		}
	}
}