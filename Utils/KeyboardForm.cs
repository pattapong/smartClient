using smartRestaurant.Controls;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

namespace smartRestaurant.Utils
{
	public class KeyboardForm : Form
	{
		private ImageList NumberImgList;

		private KeyboardPad MessagePad;

		private IContainer components;

		private static KeyboardForm instance;

		private bool dialogResult;

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

		static KeyboardForm()
		{
			KeyboardForm.instance = null;
		}

		public KeyboardForm()
		{
			this.InitializeComponent();
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
			ResourceManager resourceManager = new ResourceManager(typeof(KeyboardForm));
			this.NumberImgList = new ImageList(this.components);
			this.MessagePad = new KeyboardPad();
			base.SuspendLayout();
			this.NumberImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.NumberImgList.ImageSize = new System.Drawing.Size(72, 60);
			this.NumberImgList.ImageStream = (ImageListStreamer)resourceManager.GetObject("NumberImgList.ImageStream");
			this.NumberImgList.TransparentColor = Color.Transparent;
			this.MessagePad.Font = new System.Drawing.Font("Tahoma", 12f);
			this.MessagePad.Image = (Bitmap)resourceManager.GetObject("MessagePad.Image");
			this.MessagePad.ImageClick = (Bitmap)resourceManager.GetObject("MessagePad.ImageClick");
			this.MessagePad.ImageClickIndex = 1;
			this.MessagePad.ImageIndex = 0;
			this.MessagePad.ImageList = this.NumberImgList;
			this.MessagePad.Name = "MessagePad";
			this.MessagePad.Size = new System.Drawing.Size(936, 340);
			this.MessagePad.TabIndex = 43;
			this.MessagePad.Title = null;
			this.MessagePad.PadClick += new KeyboardPadEventHandler(this.MessagePad_PadClick);
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 16);
			this.BackColor = Color.White;
			base.ClientSize = new System.Drawing.Size(936, 340);
			base.Controls.AddRange(new Control[] { this.MessagePad });
			this.Font = new System.Drawing.Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 222);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.Name = "KeyboardForm";
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterParent;
			base.ResumeLayout(false);
		}

		private void MessagePad_PadClick(object sender, KeyboardPadEventArgs e)
		{
			if (e.KeyCode == KeyboardPad.BUTTON_OK)
			{
				this.dialogResult = true;
				base.Close();
				return;
			}
			if (e.KeyCode == KeyboardPad.BUTTON_CANCEL)
			{
				this.dialogResult = false;
				base.Close();
			}
		}

		public static string Show(string title, string text)
		{
			if (KeyboardForm.instance == null)
			{
				KeyboardForm.instance = new KeyboardForm();
			}
			KeyboardForm.instance.Title = title;
			KeyboardForm.instance.Text = text;
			KeyboardForm.instance.MessagePad.Focus();
			KeyboardForm.instance.ShowDialog();
			if (!KeyboardForm.instance.dialogResult)
			{
				return null;
			}
			return KeyboardForm.instance.Text;
		}
	}
}