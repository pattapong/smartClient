using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Windows.Forms;
using System.Reflection;

namespace smartRestaurant.Controls
{
	/// <summary>
	/// Summary description for CustomControl1.
	/// </summary>
	public class ImageButton : Label
	{
		// Fields
		private float blueTone;
		private const int DEFAULT_HEIGHT = 60;
		private const int DEFAULT_WIDTH = 110;
		private static ImageAttributes defaultImageAttr = null;
		private static Rectangle defaultRect = Rectangle.Empty;
		private static RectangleF defaultRectF = RectangleF.Empty;
		private float greenTone;
		private Image imageClick;
		private int imageClickIndex;
		private Image imageNormal;
		private bool isLock;
		private bool isMark;
		private static Image lockImage = null;
		private static Image markImage = null;
		private object objectValue;
		private float redTone;
		private static StringFormat stringFormat = null;
		private string tmpText;

		// Methods
		public ImageButton()
		{
			if (lockImage == null)
			{
				try
				{
					Assembly entryAssembly = Assembly.GetEntryAssembly();
					lockImage = Image.FromStream(entryAssembly.GetManifestResourceStream("smartRestaurant.images.key_lock.png"));
					markImage = Image.FromStream(entryAssembly.GetManifestResourceStream("smartRestaurant.images.key_mark.png"));
				}
				catch (Exception)
				{
				}
			}
			this.redTone = this.greenTone = this.blueTone = 1f;
			this.BackColor = Color.Transparent;
			this.Cursor = Cursors.Hand;
			base.Size = new Size(110, 60);
			this.TextAlign = ContentAlignment.MiddleCenter;
			this.IsLock = false;
			this.IsMark = false;
			base.MouseDown += new MouseEventHandler(this.Button_MouseDown);
			base.MouseUp += new MouseEventHandler(this.Button_MouseUp);
		}

		private void Button_MouseDown(object sender, MouseEventArgs e)
		{
			if (this.imageClick != null)
			{
				this.imageNormal = base.Image;
				base.Image = this.imageClick;
			}
		}

		private void Button_MouseUp(object sender, MouseEventArgs e)
		{
			if (this.imageNormal != null)
			{
				base.Image = this.imageNormal;
			}
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			Rectangle defaultRect;
			RectangleF defaultRectF;
			Graphics graphics = pe.Graphics;
			if (ImageButton.defaultRect == Rectangle.Empty)
			{
				ImageButton.defaultRect = new Rectangle(0, 0, 110, 60);
				ImageButton.defaultRectF = new RectangleF(0f, 0f, 110f, 60f);
			}
			if ((base.Width == 110) && (base.Height == 60))
			{
				defaultRect = ImageButton.defaultRect;
				defaultRectF = ImageButton.defaultRectF;
			}
			else
			{
				defaultRect = new Rectangle(0, 0, base.Width, base.Height);
				defaultRectF = new RectangleF(0f, 0f, (float) base.Width, (float) base.Height);
			}
			if (base.Image != null)
			{
				ColorMatrix matrix;
				ImageAttributes defaultImageAttr;
				if (ImageButton.defaultImageAttr == null)
				{
					matrix = new ColorMatrix();
					matrix.Matrix00 = 1f;
					matrix.Matrix11 = 1f;
					matrix.Matrix22 = 1f;
					matrix.Matrix33 = 1f;
					matrix.Matrix44 = 1f;
					ImageButton.defaultImageAttr = new ImageAttributes();
					ImageButton.defaultImageAttr.SetColorMatrix(matrix);
				}
				if (base.Enabled && (((this.redTone != 1f) || (this.greenTone != 1f)) || (this.blueTone != 1f)))
				{
					matrix = new ColorMatrix();
					matrix.Matrix00 = this.redTone;
					matrix.Matrix11 = this.greenTone;
					matrix.Matrix22 = this.blueTone;
					matrix.Matrix33 = 1f;
					matrix.Matrix44 = 1f;
					defaultImageAttr = new ImageAttributes();
					defaultImageAttr.SetColorMatrix(matrix);
				}
				else
				{
					defaultImageAttr = ImageButton.defaultImageAttr;
				}
				graphics.DrawImage(base.Image, defaultRect, 0, 0, base.Width, base.Height, GraphicsUnit.Pixel, defaultImageAttr);
			}
			if (this.isLock)
			{
				if (lockImage != null)
				{
					graphics.DrawImage(lockImage, base.Width - lockImage.Width, base.Height - lockImage.Height, lockImage.Width, lockImage.Height);
				}
				else
				{
					graphics.FillEllipse(Brushes.Red, base.Width - 10, base.Height - 10, 10, 10);
				}
			}
			if (this.isMark)
			{
				if (markImage != null)
				{
					graphics.DrawImage(markImage, 0, base.Height - markImage.Height, markImage.Width, markImage.Height);
				}
				else
				{
					graphics.FillEllipse(Brushes.Blue, 0, base.Height - 10, 10, 10);
				}
			}
			if (stringFormat == null)
			{
				stringFormat = new StringFormat();
				stringFormat.Alignment = StringAlignment.Center;
				stringFormat.LineAlignment = StringAlignment.Center;
			}
			graphics.DrawString(this.Text, this.Font, new Pen(base.Enabled ? this.ForeColor : Color.Gray).Brush, defaultRectF, stringFormat);
		}

		private void Repaint()
		{
			this.tmpText = this.Text;
			this.Text = "";
			this.Text = this.tmpText;
		}

		// Properties
		public float Blue
		{
			get
			{
				return this.blueTone;
			}
			set
			{
				this.blueTone = value;
				this.Repaint();
			}
		}

		public float Green
		{
			get
			{
				return this.greenTone;
			}
			set
			{
				this.greenTone = value;
				this.Repaint();
			}
		}

		public Image ImageClick
		{
			get
			{
				return this.imageClick;
			}
			set
			{
				this.imageClick = value;
			}
		}

		public int ImageClickIndex
		{
			get
			{
				return this.imageClickIndex;
			}
			set
			{
				this.imageClickIndex = value;
				if (base.ImageList != null)
				{
					this.imageClick = base.ImageList.Images[this.ImageClickIndex];
				}
			}
		}

		public bool IsLock
		{
			get
			{
				return this.isLock;
			}
			set
			{
				this.isLock = value;
			}
		}

		public bool IsMark
		{
			get
			{
				return this.isMark;
			}
			set
			{
				this.isMark = value;
			}
		}

		public object ObjectValue
		{
			get
			{
				return this.objectValue;
			}
			set
			{
				this.objectValue = value;
			}
		}

		public float Red
		{
			get
			{
				return this.redTone;
			}
			set
			{
				this.redTone = value;
				this.Repaint();
			}
		}
	}

 

}
