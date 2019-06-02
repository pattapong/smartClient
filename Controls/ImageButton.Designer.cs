using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace smartRestaurant.Controls
{
	public class ImageButton : Label
	{
		private const int DEFAULT_WIDTH = 110;

		private const int DEFAULT_HEIGHT = 60;

		private static StringFormat stringFormat;

		private static Rectangle defaultRect;

		private static RectangleF defaultRectF;

		private static ImageAttributes defaultImageAttr;

		private static System.Drawing.Image lockImage;

		private static System.Drawing.Image markImage;

		private object objectValue;

		private System.Drawing.Image imageNormal;

		private System.Drawing.Image imageClick;

		private int imageClickIndex;

		private string tmpText;

		private float redTone;

		private float greenTone;

		private float blueTone;

		private bool isLock;

		private bool isMark;

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

		public System.Drawing.Image ImageClick
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

		static ImageButton()
		{
			ImageButton.stringFormat = null;
			ImageButton.defaultRect = Rectangle.Empty;
			ImageButton.defaultRectF = RectangleF.Empty;
			ImageButton.defaultImageAttr = null;
			ImageButton.lockImage = null;
			ImageButton.markImage = null;
		}

		public ImageButton()
		{
			if (ImageButton.lockImage == null)
			{
				try
				{
					Assembly entryAssembly = Assembly.GetEntryAssembly();
					ImageButton.lockImage = System.Drawing.Image.FromStream(entryAssembly.GetManifestResourceStream("smartRestaurant.images.key_lock.png"));
					ImageButton.markImage = System.Drawing.Image.FromStream(entryAssembly.GetManifestResourceStream("smartRestaurant.images.key_mark.png"));
				}
				catch (Exception exception)
				{
				}
			}
			float single = 1f;
			float single1 = single;
			this.blueTone = single;
			float single2 = single1;
			single1 = single2;
			this.greenTone = single2;
			this.redTone = single1;
			this.BackColor = Color.Transparent;
			this.Cursor = Cursors.Hand;
			base.Size = new System.Drawing.Size(110, 60);
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
			Rectangle rectangle;
			RectangleF rectangleF;
			ColorMatrix colorMatrix;
			ImageAttributes imageAttribute;
			Graphics graphics = pe.Graphics;
			if (ImageButton.defaultRect == Rectangle.Empty)
			{
				ImageButton.defaultRect = new Rectangle(0, 0, 110, 60);
				ImageButton.defaultRectF = new RectangleF(0f, 0f, 110f, 60f);
			}
			if (base.Width != 110 || base.Height != 60)
			{
				rectangle = new Rectangle(0, 0, base.Width, base.Height);
				rectangleF = new RectangleF(0f, 0f, (float)base.Width, (float)base.Height);
			}
			else
			{
				rectangle = ImageButton.defaultRect;
				rectangleF = ImageButton.defaultRectF;
			}
			if (base.Image != null)
			{
				if (ImageButton.defaultImageAttr == null)
				{
					colorMatrix = new ColorMatrix()
					{
						Matrix00 = 1f,
						Matrix11 = 1f,
						Matrix22 = 1f,
						Matrix33 = 1f,
						Matrix44 = 1f
					};
					ImageButton.defaultImageAttr = new ImageAttributes();
					ImageButton.defaultImageAttr.SetColorMatrix(colorMatrix);
				}
				if (!base.Enabled || this.redTone == 1f && this.greenTone == 1f && this.blueTone == 1f)
				{
					imageAttribute = ImageButton.defaultImageAttr;
				}
				else
				{
					colorMatrix = new ColorMatrix()
					{
						Matrix00 = this.redTone,
						Matrix11 = this.greenTone,
						Matrix22 = this.blueTone,
						Matrix33 = 1f,
						Matrix44 = 1f
					};
					imageAttribute = new ImageAttributes();
					imageAttribute.SetColorMatrix(colorMatrix);
				}
				graphics.DrawImage(base.Image, rectangle, 0, 0, base.Width, base.Height, GraphicsUnit.Pixel, imageAttribute);
			}
			if (this.isLock)
			{
				if (ImageButton.lockImage == null)
				{
					graphics.FillEllipse(Brushes.Red, base.Width - 10, base.Height - 10, 10, 10);
				}
				else
				{
					graphics.DrawImage(ImageButton.lockImage, base.Width - ImageButton.lockImage.Width, base.Height - ImageButton.lockImage.Height, ImageButton.lockImage.Width, ImageButton.lockImage.Height);
				}
			}
			if (this.isMark)
			{
				if (ImageButton.markImage == null)
				{
					graphics.FillEllipse(Brushes.Blue, 0, base.Height - 10, 10, 10);
				}
				else
				{
					graphics.DrawImage(ImageButton.markImage, 0, base.Height - ImageButton.markImage.Height, ImageButton.markImage.Width, ImageButton.markImage.Height);
				}
			}
			if (ImageButton.stringFormat == null)
			{
				ImageButton.stringFormat = new StringFormat()
				{
					Alignment = StringAlignment.Center,
					LineAlignment = StringAlignment.Center
				};
			}
			graphics.DrawString(this.Text, this.Font, (new Pen((base.Enabled ? this.ForeColor : Color.Gray))).Brush, rectangleF, ImageButton.stringFormat);
		}

		private void Repaint()
		{
			this.tmpText = this.Text;
			this.Text = "";
			this.Text = this.tmpText;
		}
	}
}