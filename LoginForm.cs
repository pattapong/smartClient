using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using smartRestaurant.Data;
using smartRestaurant.Utils;

namespace smartRestaurant
{
	/// <summary>
	/// <b>LoginForm</b> is form for users to login. This is first form that users should use.
	/// </summary>
	public class LoginForm : SmartForm
	{
		/// <summary>
		/// Constanct value for form to know that focus on User ID Text Box.
		/// </summary>
		private Color backColor;
		private smartRestaurant.Controls.ImageButton BtnExit;
		private smartRestaurant.Controls.ImageButton BtnLogin;
		private ImageList ButtonImgList;
		private IContainer components;
		private const int FIELD_PASSWORD = 1;
		private const int FIELD_USERID = 0;
		private Label FieldPassword;
		private Label FieldUserID;
		private int focusField;
		private string hiddenPassword;
		private Label LblCopyright;
		private Label LblPageID;
		private Label LblPassword;
		private Label LblUserID;
		private smartRestaurant.Controls.GroupPanel LoginPanel;
		private ImageList NumberImgList;
		private smartRestaurant.Controls.NumberPad NumberKeyPad;

		public LoginForm()
		{
			this.InitializeComponent();
			this.backColor = this.FieldUserID.BackColor;
			this.UpdateForm();
		}

		private void BtnExit_Click(object sender, EventArgs e)
		{
			((MainForm) base.MdiParent).Exit();
		}

		private void BtnLogin_Click(object sender, EventArgs e)
		{
			if ((this.FieldUserID.Text == "") || (this.FieldPassword.Text == ""))
			{
				MessageForm.Show("Login Fail", "Please fill form.");
			}
			else
			{
				int num;
				try
				{
					num = int.Parse(this.FieldUserID.Text);
				}
				catch (Exception exception)
				{
					MessageBox.Show(exception.ToString());
					return;
				}
				UserProfile profile = UserProfile.CheckLogin(num, this.hiddenPassword);
				if (profile != null)
				{
					((MainForm) base.MdiParent).User = profile;
					((MainForm) base.MdiParent).ShowMainMenuForm();
				}
				else
				{
					MessageForm.Show("Login Fail", "Your user ID or password wrong.");
					this.FieldPassword.Text = "";
					this.hiddenPassword = "";
					this.focusField = 0;
					this.CheckFocusField();
				}
			}
		}

		private void CheckFocusField()
		{
			switch (this.focusField)
			{
				case 0:
					this.FieldUserID.BackColor = this.backColor;
					this.FieldPassword.BackColor = Color.White;
					return;

				case 1:
					this.FieldUserID.BackColor = Color.White;
					this.FieldPassword.BackColor = this.backColor;
					return;
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
			if (disposing && (this.components != null))
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
			this.NumberKeyPad = new smartRestaurant.Controls.NumberPad();
			this.NumberImgList = new System.Windows.Forms.ImageList(this.components);
			this.ButtonImgList = new System.Windows.Forms.ImageList(this.components);
			this.BtnLogin = new smartRestaurant.Controls.ImageButton();
			this.BtnExit = new smartRestaurant.Controls.ImageButton();
			this.LoginPanel = new smartRestaurant.Controls.GroupPanel();
			this.FieldPassword = new System.Windows.Forms.Label();
			this.LblPassword = new System.Windows.Forms.Label();
			this.FieldUserID = new System.Windows.Forms.Label();
			this.LblUserID = new System.Windows.Forms.Label();
			this.LblPageID = new System.Windows.Forms.Label();
			this.LblCopyright = new System.Windows.Forms.Label();
			this.LoginPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// NumberKeyPad
			// 
			this.NumberKeyPad.BackColor = System.Drawing.Color.White;
			this.NumberKeyPad.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.NumberKeyPad.Image = null;
			this.NumberKeyPad.ImageClick = null;
			this.NumberKeyPad.ImageClickIndex = 1;
			this.NumberKeyPad.ImageIndex = 0;
			this.NumberKeyPad.ImageList = this.NumberImgList;
			this.NumberKeyPad.Location = new System.Drawing.Point(392, 368);
			this.NumberKeyPad.Name = "NumberKeyPad";
			this.NumberKeyPad.Size = new System.Drawing.Size(226, 255);
			this.NumberKeyPad.TabIndex = 8;
			this.NumberKeyPad.Text = "NumberPadKey";
			this.NumberKeyPad.PadClick += new smartRestaurant.Controls.NumberPad.NumberPadEventHandler(this.NumberKeyPad_PadClick);
			// 
			// NumberImgList
			// 
			this.NumberImgList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.NumberImgList.ImageSize = new System.Drawing.Size(72, 60);
			this.NumberImgList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// ButtonImgList
			// 
			this.ButtonImgList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.ButtonImgList.ImageSize = new System.Drawing.Size(110, 60);
			this.ButtonImgList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// BtnLogin
			// 
			this.BtnLogin.BackColor = System.Drawing.Color.Transparent;
			this.BtnLogin.Blue = 2F;
			this.BtnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
			this.BtnLogin.Green = 1F;
			this.BtnLogin.ImageClick = null;
			this.BtnLogin.ImageClickIndex = 1;
			this.BtnLogin.ImageList = this.ButtonImgList;
			this.BtnLogin.Location = new System.Drawing.Point(392, 632);
			this.BtnLogin.Name = "BtnLogin";
			this.BtnLogin.ObjectValue = null;
			this.BtnLogin.Red = 2F;
			this.BtnLogin.Size = new System.Drawing.Size(110, 60);
			this.BtnLogin.TabIndex = 9;
			this.BtnLogin.Text = "Login";
			this.BtnLogin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.BtnLogin.Click += new System.EventHandler(this.BtnLogin_Click);
			// 
			// BtnExit
			// 
			this.BtnExit.BackColor = System.Drawing.Color.Transparent;
			this.BtnExit.Blue = 2F;
			this.BtnExit.Cursor = System.Windows.Forms.Cursors.Hand;
			this.BtnExit.Green = 2F;
			this.BtnExit.ImageClick = null;
			this.BtnExit.ImageClickIndex = 1;
			this.BtnExit.ImageList = this.ButtonImgList;
			this.BtnExit.Location = new System.Drawing.Point(509, 632);
			this.BtnExit.Name = "BtnExit";
			this.BtnExit.ObjectValue = null;
			this.BtnExit.Red = 1F;
			this.BtnExit.Size = new System.Drawing.Size(110, 60);
			this.BtnExit.TabIndex = 10;
			this.BtnExit.Text = "Exit";
			this.BtnExit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
			// 
			// LoginPanel
			// 
			this.LoginPanel.BackColor = System.Drawing.Color.Transparent;
			this.LoginPanel.Caption = "Login";
			this.LoginPanel.Controls.Add(this.FieldPassword);
			this.LoginPanel.Controls.Add(this.LblPassword);
			this.LoginPanel.Controls.Add(this.FieldUserID);
			this.LoginPanel.Controls.Add(this.LblUserID);
			this.LoginPanel.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.LoginPanel.Location = new System.Drawing.Point(392, 128);
			this.LoginPanel.Name = "LoginPanel";
			this.LoginPanel.ShowHeader = true;
			this.LoginPanel.Size = new System.Drawing.Size(225, 224);
			this.LoginPanel.TabIndex = 11;
			// 
			// FieldPassword
			// 
			this.FieldPassword.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(192)));
			this.FieldPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.FieldPassword.Cursor = System.Windows.Forms.Cursors.Hand;
			this.FieldPassword.Location = new System.Drawing.Point(8, 168);
			this.FieldPassword.Name = "FieldPassword";
			this.FieldPassword.Size = new System.Drawing.Size(208, 40);
			this.FieldPassword.TabIndex = 42;
			this.FieldPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.FieldPassword.Click += new System.EventHandler(this.FieldPassword_Click);
			// 
			// LblPassword
			// 
			this.LblPassword.Location = new System.Drawing.Point(8, 128);
			this.LblPassword.Name = "LblPassword";
			this.LblPassword.Size = new System.Drawing.Size(88, 40);
			this.LblPassword.TabIndex = 41;
			this.LblPassword.Text = "Password";
			this.LblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// FieldUserID
			// 
			this.FieldUserID.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(192)));
			this.FieldUserID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.FieldUserID.Cursor = System.Windows.Forms.Cursors.Hand;
			this.FieldUserID.Location = new System.Drawing.Point(8, 80);
			this.FieldUserID.Name = "FieldUserID";
			this.FieldUserID.Size = new System.Drawing.Size(208, 40);
			this.FieldUserID.TabIndex = 40;
			this.FieldUserID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.FieldUserID.Click += new System.EventHandler(this.FieldUserID_Click);
			// 
			// LblUserID
			// 
			this.LblUserID.Location = new System.Drawing.Point(8, 40);
			this.LblUserID.Name = "LblUserID";
			this.LblUserID.Size = new System.Drawing.Size(72, 40);
			this.LblUserID.TabIndex = 39;
			this.LblUserID.Text = "User ID";
			this.LblUserID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// LblPageID
			// 
			this.LblPageID.BackColor = System.Drawing.Color.Transparent;
			this.LblPageID.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.LblPageID.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(103)), ((System.Byte)(138)), ((System.Byte)(198)));
			this.LblPageID.Location = new System.Drawing.Point(912, 752);
			this.LblPageID.Name = "LblPageID";
			this.LblPageID.TabIndex = 33;
			this.LblPageID.Text = "STLI011";
			this.LblPageID.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// LblCopyright
			// 
			this.LblCopyright.BackColor = System.Drawing.Color.Transparent;
			this.LblCopyright.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.LblCopyright.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(103)), ((System.Byte)(138)), ((System.Byte)(198)));
			this.LblCopyright.Location = new System.Drawing.Point(8, 752);
			this.LblCopyright.Name = "LblCopyright";
			this.LblCopyright.Size = new System.Drawing.Size(280, 16);
			this.LblCopyright.TabIndex = 36;
			this.LblCopyright.Text = "Copyright (c) 2004. All rights reserved.";
			// 
			// LoginForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.ClientSize = new System.Drawing.Size(1020, 764);
			this.Controls.Add(this.LblCopyright);
			this.Controls.Add(this.LblPageID);
			this.Controls.Add(this.LoginPanel);
			this.Controls.Add(this.BtnExit);
			this.Controls.Add(this.BtnLogin);
			this.Controls.Add(this.NumberKeyPad);
			this.Name = "LoginForm";
			this.Text = "Login";
			this.LoginPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		private void NumberKeyPad_PadClick(object sender, smartRestaurant.Controls.NumberPadEventArgs e)
		{
			if (e.IsNumeric)
			{
				switch (this.focusField)
				{
					case 0:
						this.FieldUserID.Text = this.FieldUserID.Text + e.Number;
						return;

					case 1:
						this.FieldPassword.Text = this.FieldPassword.Text + "*";
						this.hiddenPassword = this.hiddenPassword + e.Number;
						return;
				}
			}
			else if (e.IsCancel)
			{
				switch (this.focusField)
				{
					case 0:
						if (this.FieldUserID.Text.Length > 0)
						{
							this.FieldUserID.Text = this.FieldUserID.Text.Substring(0, this.FieldUserID.Text.Length - 1);
						}
						return;

					case 1:
						if (this.FieldPassword.Text.Length > 0)
						{
							this.FieldPassword.Text = this.FieldPassword.Text.Substring(0, this.FieldPassword.Text.Length - 1);
							this.hiddenPassword = this.hiddenPassword.Substring(0, this.hiddenPassword.Length - 1);
						}
						return;
				}
			}
			else if (e.IsEnter)
			{
				switch (this.focusField)
				{
					case 0:
						this.focusField = 1;
						this.CheckFocusField();
						return;

					case 1:
						this.BtnLogin_Click(null, null);
						return;

					default:
						return;
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
