using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using smartRestaurant.Controls;

namespace smartRestaurant.Utils
{
	/// <summary>
	/// Summary description for KeyboardForm.
	/// </summary>
	public class KeyboardForm : Form
	{
		// Fields
		private IContainer components;
		private bool dialogResult;
		private static KeyboardForm instance = null;
		private KeyboardPad MessagePad;
		private ImageList NumberImgList;

		// Methods
		public KeyboardForm()
		{
			this.InitializeComponent();
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
			this.NumberImgList = new System.Windows.Forms.ImageList(this.components);
			this.MessagePad = new smartRestaurant.Controls.KeyboardPad();
			this.SuspendLayout();
			// 
			// NumberImgList
			// 
			this.NumberImgList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.NumberImgList.ImageSize = new System.Drawing.Size(72, 60);
			this.NumberImgList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// MessagePad
			// 
			this.MessagePad.Font = new System.Drawing.Font("Tahoma", 12F);
			this.MessagePad.Image = null;
			this.MessagePad.ImageClick = null;
			this.MessagePad.ImageClickIndex = 1;
			this.MessagePad.ImageIndex = 0;
			this.MessagePad.ImageList = this.NumberImgList;
			this.MessagePad.Location = new System.Drawing.Point(0, 0);
			this.MessagePad.Name = "MessagePad";
			this.MessagePad.Size = new System.Drawing.Size(936, 340);
			this.MessagePad.TabIndex = 43;
			this.MessagePad.Title = null;
			this.MessagePad.PadClick += new smartRestaurant.Controls.KeyboardPad.KeyboardPadEventHandler(this.MessagePad_PadClick);
			// 
			// KeyboardForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 16);
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(936, 340);
			this.Controls.Add(this.MessagePad);
			this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(222)));
			this.Name = "KeyboardForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.ResumeLayout(false);

		}

		private void MessagePad_PadClick(object sender, KeyboardPadEventArgs e)
		{
			if (e.KeyCode == KeyboardPad.BUTTON_OK)
			{
				this.dialogResult = true;
				base.Close();
			}
			else if (e.KeyCode == KeyboardPad.BUTTON_CANCEL)
			{
				this.dialogResult = false;
				base.Close();
			}
		}

		public static string Show(string title, string text)
		{
			if (instance == null)
			{
				instance = new KeyboardForm();
			}
			instance.Title = title;
			instance.Text = text;
			instance.MessagePad.Focus();
			instance.ShowDialog();
			if (instance.dialogResult)
			{
				return instance.Text;
			}
			return null;
		}

		// Properties
		public override string Text
		{
			get
			{
				if (this.MessagePad == null)
				{
					return null;
				}
				return this.MessagePad.Text;
			}
			set
			{
				this.MessagePad.Text = value;
			}
		}

		public string Title
		{
			get
			{
				if (this.MessagePad == null)
				{
					return null;
				}
				return this.MessagePad.Title;
			}
			set
			{
				this.MessagePad.Title = value;
			}
		}
	}


}
