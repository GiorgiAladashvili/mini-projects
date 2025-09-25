using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace MasterMind
{
    [Serializable]
    public partial class ColorBall : UserControl
    {
        public int Index;
        public bool IsMainBall;
        private bool NoColor, Selected;
        private Color colDefault = Color.FromArgb(90, 162, 214);
        private Color colNormal = Color.FromArgb(90, 162, 214);
        private Pen p = new Pen(Color.Red , 2.5f);
        private Rectangle rect;

        private Bitmap bmpTemplate;
        private ImageAttributes iaDefault, iaNormal;
        private ColorMatrix cmDefault, cmNormal;

        public Color ColorNormal
        {
            get => colNormal;
            set
            {
                colNormal = value;
                cmNormal = GetTranslationColorMatrix(colDefault, colNormal);
                iaNormal.SetColorMatrix(cmNormal, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                Invalidate();
            }
        }

        public string ColorName
        {
            get
            {
                if (NoColor) return "NoColor";
                else return colNormal.Name;
            }
        }

        public bool IsSelected
        {
            get => Selected;
            set
            {
                Selected = value;
                Invalidate();
            }
        }

        public bool IsEmpty
        {
            get => NoColor;
            set
            {
                NoColor = value;
                Invalidate();
            }
        }

        public ColorBall()
        {
            InitializeComponent();
            Index = 0; IsMainBall = false;
            NoColor = false; Selected = false;
            if (bmpTemplate != null) bmpTemplate.Dispose();
            bmpTemplate = new Bitmap(GetType(), "aqua.png");
            cmDefault = GetTranslationColorMatrix(colDefault, colDefault);
            iaDefault = new ImageAttributes();
            iaDefault.SetColorMatrix(cmDefault, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            cmNormal = GetTranslationColorMatrix(colDefault, colNormal);
            iaNormal = new ImageAttributes();
            iaNormal.SetColorMatrix(cmNormal, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            rect = new Rectangle(ClientRectangle.X + 2, ClientRectangle.Y + 2, ClientRectangle.Width - 4, ClientRectangle.Height - 4);
            if (!NoColor) g.DrawImage(bmpTemplate, ClientRectangle, 0, 0, bmpTemplate.Width, bmpTemplate.Height, GraphicsUnit.Pixel, iaNormal);
            if (Selected) g.DrawEllipse(p, rect);
        }

        /// <summary>
		/// Get ColorMatrix that transform from the origin color to a new color
		/// </summary>
		private ColorMatrix GetTranslationColorMatrix(Color originColor, Color newColor)
        {
            // Image attributes that lighten and desaturate normal buttons
            ColorMatrix cmTrans = new ColorMatrix();

            if (newColor.Equals(originColor)) return cmTrans;

            float fTransRed = (float)newColor.R / (float)(originColor.R + originColor.G + originColor.B);
            float fTransGreen = (float)newColor.G / (float)(originColor.R + originColor.G + originColor.B);
            float fTransBlue = (float)newColor.B / (float)(originColor.R + originColor.G + originColor.B);

            // Translate the Origin Color to New Color
            cmTrans.Matrix00 = fTransRed;
            cmTrans.Matrix10 = fTransRed;
            cmTrans.Matrix20 = fTransRed;

            cmTrans.Matrix01 = fTransGreen;
            cmTrans.Matrix11 = fTransGreen;
            cmTrans.Matrix21 = fTransGreen;

            cmTrans.Matrix02 = fTransBlue;
            cmTrans.Matrix12 = fTransBlue;
            cmTrans.Matrix22 = fTransBlue;

            return cmTrans;
        }

    }
}
