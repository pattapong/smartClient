using smartRestaurant.Controls;
using smartRestaurant.Data;
using smartRestaurant.Utils;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

namespace smartRestaurant
{
	public class LoginForm : SmartForm
	{
		private const int FIELD_USERID = 0;

		private const int FIELD_PASSWORD = 1;

		private Color backColor;

		private int focusField;

		private string hiddenPassword;

		private NumberPad NumberKeyPad;

		private ImageList NumberImgList;

		private ImageList ButtonImgList;

		private ImageButton BtnExit;

		private ImageButton BtnLogin;

		private Label LblUserID;

		private Label LblPassword;

		private GroupPanel LoginPanel;

		private Label FieldUserID;

		private Label FieldPassword;

		private IContainer components;

		private Label LblPageID;

		private Label LblCopyright;

		public LoginForm()
		{
			this.InitializeComponent();
			this.backColor = this.FieldUserID.BackColor;
			this.UpdateForm();
		}

		private void BtnExit_Click(object sender, EventArgs e)
		{
			((MainForm)base.MdiParent).Exit();
		}

		private void BtnLogin_Click(object sender, EventArgs e)
		{
			int num;
			if (this.FieldUserID.Text == "" || this.FieldPassword.Text == "")
			{
				MessageForm.Show("Login Fail", "Please fill form.");
				return;
			}
			try
			{
				num = int.Parse(this.FieldUserID.Text);
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.ToString());
				return;
			}
			UserProfile userProfile = UserProfile.CheckLogin(num, this.hiddenPassword);
			if (userProfile != null)
			{
				((MainForm)base.MdiParent).User = userProfile;
				((MainForm)base.MdiParent).ShowMainMenuForm();
				return;
			}
			MessageForm.Show("Login Fail", "Your user ID or password wrong.");
			this.FieldPassword.Text = "";
			this.hiddenPassword = "";
			this.focusField = 0;
			this.CheckFocusField();
		}

		private void CheckFocusField()
		{
			switch (this.focusField)
			{
				case 0:
				{
					this.FieldUserID.BackColor = this.backColor;
					this.FieldPassword.BackColor = Color.White;
					return;
				}
				case 1:
				{
					this.FieldUserID.BackColor = Color.White;
					this.FieldPassword.BackColor = this.backColor;
					return;
				}
				default:
				{
					return;
				}
			}
		}

		private void ClearField()
		{
			this.focusField = 0;
			this.FieldUserID.Text = "";
			this.FieldPassword.Text = "";
			this.hiddenPassword = "";
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void FieldPassword_Click(object sender, EventArgs e)
		{
			this.focusField = 1;
			this.CheckFocusField();
		}

		private void FieldUserID_Click(object sender, EventArgs e)
		{
			this.focusField = 0;
			this.CheckFocusField();
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			ResourceManager resourceManager = new ResourceManager(typeof(LoginForm));
			this.NumberKeyPad = new NumberPad();
			this.NumberImgList = new ImageList(this.components);
			this.ButtonImgList = new ImageList(this.components);
			this.BtnLogin = new ImageButton();
			this.BtnExit = new ImageButton();
			this.LoginPanel = new GroupPanel();
			this.FieldPassword = new Label();
			this.LblPassword = new Label();
			this.FieldUserID = new Label();
			this.LblUserID = new Label();
			this.LblPageID = new Label();
			this.LblCopyright = new Label();
			this.LoginPanel.SuspendLayout();
			base.SuspendLayout();
			this.NumberKeyPad.BackColor = Color.White;
			this.NumberKeyPad.Font = new System.Drawing.Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.NumberKeyPad.Image = (Image)resourceManager.GetObject("NumberKeyPad.Image");
			this.NumberKeyPad.ImageClick = (Image)resourceManager.GetObject("NumberKeyPad.ImageClick");
			this.NumberKeyPad.ImageClickIndex = 1;
			this.NumberKeyPad.ImageIndex = 0;
			this.NumberKeyPad.ImageList = this.NumberImgList;
			this.NumberKeyPad.Location = new Point(392, 368);
			this.NumberKeyPad.Name = "NumberKeyPad";
			this.NumberKeyPad.Size = new System.Drawing.Size(226, 255);
			this.NumberKeyPad.TabIndex = 8;
			this.NumberKeyPad.Text = "NumberPadKey";
			this.NumberKeyPad.PadClick += new NumberPadEventHandler(this.NumberKeyPad_PadClick);
			this.NumberImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.NumberImgList.ImageSize = new System.Drawing.Size(72, 60);
			this.NumberImgList.ImageStream = (ImageListStreamer)resourceManager.GetObject("NumberImgList.ImageStream");
			this.NumberImgList.TransparentColor = Color.Transparent;
			this.ButtonImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.ButtonImgList.ImageSize = new System.Drawing.Size(110, 60);
			this.ButtonImgList.ImageStream = (ImageListStreamer)resourceManager.GetObject("ButtonImgList.ImageStream");
			this.ButtonImgList.TransparentColor = Color.Transparent;
			this.BtnLogin.BackColor = Color.Transparent;
			this.BtnLogin.Blue = 2f;
			this.BtnLogin.Cursor = Cursors.Hand;
			this.BtnLogin.Green = 1f;
			this.BtnLogin.ImageClick = (Image)resourceManager.GetObject("BtnLogin.ImageClick");
			this.BtnLogin.ImageClickIndex = 1;
			this.BtnLogin.ImageIndex = 0;
			this.BtnLogin.ImageList = this.ButtonImgList;
			this.BtnLogin.Location = new Point(392, 632);
			this.BtnLogin.Name = "BtnLogin";
			this.BtnLogin.ObjectValue = null;
			this.BtnLogin.Red = 2f;
			this.BtnLogin.Size = new System.Drawing.Size(110, 60);
			this.BtnLogin.TabIndex = 9;
			this.BtnLogin.Text = "Login";
			this.BtnLogin.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnLogin.Click += new EventHandler(this.BtnLogin_Click);
			this.BtnExit.BackColor = Color.Transparent;
			this.BtnExit.Blue = 2f;
			this.BtnExit.Cursor = Cursors.Hand;
			this.BtnExit.Green = 2f;
			this.BtnExit.ImageClick = (Image)resourceManager.GetObject("BtnExit.ImageClick");
			this.BtnExit.ImageClickIndex = 1;
			this.BtnExit.ImageIndex = 0;
			this.BtnExit.ImageList = this.ButtonImgList;
			this.BtnExit.Location = new Point(509, 632);
			this.BtnExit.Name = "BtnExit";
			this.BtnExit.ObjectValue = null;
			this.BtnExit.Red = 1f;
			this.BtnExit.Size = new System.Drawing.Size(110, 60);
			this.BtnExit.TabIndex = 10;
			this.BtnExit.Text = "Exit";
			this.BtnExit.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnExit.Click += new EventHandler(this.BtnExit_Click);
			this.LoginPanel.BackColor = Color.Transparent;
			this.LoginPanel.Caption = "Login";
			this.LoginPanel.Controls.Add(this.FieldPassword);
			this.LoginPanel.Controls.Add(this.LblPassword);
			this.LoginPanel.Controls.Add(this.FieldUserID);
			this.LoginPanel.Controls.Add(this.LblUserID);
			this.LoginPanel.Font = new System.Drawing.Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.LoginPanel.Location = new Point(392, 128);
			this.LoginPanel.Name = "LoginPanel";
			this.LoginPanel.ShowHeader = true;
			this.LoginPanel.Size = new System.Drawing.Size(225, 224);
			this.LoginPanel.TabIndex = 11;
			this.FieldPassword.BackColor = Color.FromArgb(255, 255, 192);
			this.FieldPassword.BorderStyle = BorderStyle.FixedSingle;
			this.FieldPassword.Cursor = Cursors.Hand;
			this.FieldPassword.Location = new Point(8, 168);
			this.FieldPassword.Name = "FieldPassword";
			this.FieldPassword.Size = new System.Drawing.Size(208, 40);
			this.FieldPassword.TabIndex = 42;
			this.FieldPassword.TextAlign = ContentAlignment.MiddleLeft;
			this.FieldPassword.Click += new EventHandler(this.FieldPassword_Click);
			this.LblPassword.Location = new Point(8, 128);
			this.LblPassword.Name = "LblPassword";
			this.LblPassword.Size = new System.Drawing.Size(88, 40);
			this.LblPassword.TabIndex = 41;
			this.LblPassword.Text = "Password";
			this.LblPassword.TextAlign = ContentAlignment.MiddleLeft;
			this.FieldUserID.BackColor = Color.FromArgb(255, 255, 192);
			this.FieldUserID.BorderStyle = BorderStyle.FixedSingle;
			this.FieldUserID.Cursor = Cursors.Hand;
			this.FieldUserID.Location = new Point(8, 80);
			this.FieldUserID.Name = "FieldUserID";
			this.FieldUserID.Size = new System.Drawing.Size(208, 40);
			this.FieldUserID.TabIndex = 40;
			this.FieldUserID.TextAlign = ContentAlignment.MiddleLeft;
			this.FieldUserID.Click += new EventHandler(this.FieldUserID_Click);
			this.LblUserID.Location = new Point(8, 40);
			this.LblUserID.Name = "LblUserID";
			this.LblUserID.Size = new System.Drawing.Size(72, 40);
			this.LblUserID.TabIndex = 39;
			this.LblUserID.Text = "User ID";
			this.LblUserID.TextAlign = ContentAlignment.MiddleLeft;
			this.LblPageID.BackColor = Color.Transparent;
			this.LblPageID.Font = new System.Drawing.Font("Tahoma", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.LblPageID.ForeColor = Color.FromArgb(103, 138, 198);
			this.LblPageID.Location = new Point(912, 752);
			this.LblPageID.Name = "LblPageID";
			this.LblPageID.TabIndex = 33;
			this.LblPageID.Text = "STLI011";
			this.LblPageID.TextAlign = ContentAlignment.TopRight;
			this.LblCopyright.BackColor = Color.Transparent;
			this.LblCopyright.Font = new System.Drawing.Font("Tahoma", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.LblCopyright.ForeColor = Color.FromArgb(103, 138, 198);
			this.LblCopyright.Location = new Point(8, 752);
			this.LblCopyright.Name = "LblCopyright";
			this.LblCopyright.Size = new System.Drawing.Size(280, 16);
			this.LblCopyright.TabIndex = 36;
			this.LblCopyright.Text = "Copyright (c) 2004. All rights reserved.";
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			base.ClientSize = new System.Drawing.Size(1020, 764);
			base.Controls.Add(this.LblCopyright);
			base.Controls.Add(this.LblPageID);
			base.Controls.Add(this.LoginPanel);
			base.Controls.Add(this.BtnExit);
			base.Controls.Add(this.BtnLogin);
			base.Controls.Add(this.NumberKeyPad);
			base.Name = "LoginForm";
			this.Text = "Login";
			this.LoginPanel.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		private void NumberKeyPad_PadClick(object sender, NumberPadEventArgs e)
		{
			if (e.IsNumeric)
			{
				switch (this.focusField)
				{
					case 0:
					{
						Label fieldUserID = this.FieldUserID;
						fieldUserID.Text = string.Concat(fieldUserID.Text, e.Number);
						return;
					}
					case 1:
					{
						Label fieldPassword = this.FieldPassword;
						fieldPassword.Text = string.Concat(fieldPassword.Text, "*");
						LoginForm loginForm = this;
						loginForm.hiddenPassword = string.Concat(loginForm.hiddenPassword, e.Number);
						return;
					}
					default:
					{
						return;
					}
				}
			}
			if (e.IsCancel)
			{
				switch (this.focusField)
				{
					case 0:
					{
						if (this.FieldUserID.Text.Length <= 0)
						{
							break;
						}
						this.FieldUserID.Text = this.FieldUserID.Text.Substring(0, this.FieldUserID.Text.Length - 1);
						return;
					}
					case 1:
					{
						if (this.FieldPassword.Text.Length <= 0)
						{
							break;
						}
						this.FieldPassword.Text = this.FieldPassword.Text.Substring(0, this.FieldPassword.Text.Length - 1);
						this.hiddenPassword = this.hiddenPassword.Substring(0, this.hiddenPassword.Length - 1);
						return;
					}
					default:
					{
						return;
					}
				}
			}
			else if (e.IsEnter)
			{
				switch (this.focusField)
				{
					case 0:
					{
						this.focusField = 1;
						this.CheckFocusField();
						return;
					}
					case 1:
					{
						this.BtnLogin_Click(null, null);
						break;
					}
					default:
					{
						return;
					}
				}
			}
		}

		public override void UpdateForm()
		{
			this.ClearField();
			this.CheckFocusField();
		}
	}
}