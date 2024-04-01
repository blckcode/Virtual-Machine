using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace VirtualMachine
{
    public class CustomButton : Button
    {
        //fields
        private int borderSize = 0;
        private int borderRadius = 35;
        private Color bordercolor = Color.RoyalBlue;

        //Constructor
        public CustomButton()
        {
            this.FlatStyle = FlatStyle.Popup;
            this.FlatAppearance.BorderSize = 0;
            this.Size = new Size(150, 40);
            this.BackColor = Color.RoyalBlue;
            this.ForeColor = Color.RoyalBlue;
        }

        //Methods
        private GraphicsPath GetFigurePath(RectangleF rect, float raduis)
        {
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(rect.X, rect.Y, raduis, raduis, 180, 90);
            path.AddArc(rect.Width - raduis, rect.Y, raduis, raduis, 270, 90);
            path.AddArc(rect.Width - raduis, rect.Height - raduis, raduis, raduis, 0, 90);
            path.AddArc(rect.X, rect.Height - raduis, raduis, raduis, 90, 90);
            path.CloseFigure();

            return path;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            RectangleF rectSurface = new RectangleF(0, 0, this.Width, this.Height);
            RectangleF rectBoder = new RectangleF(1, 1, this.Width - 0.8F, this.Height - 1);

            if (borderRadius > 0)
            {
                using (GraphicsPath pathSurface = GetFigurePath(rectSurface, borderRadius))
                using (GraphicsPath pathBorder = GetFigurePath(rectBoder, borderRadius - 1F))
                using (Pen penSurface = new Pen(this.Parent.BackColor, 2))
                using (Pen penBorder = new Pen(bordercolor, borderSize))
                {
                    penBorder.Alignment = PenAlignment.Inset;
                    this.Region = new Region(pathSurface);
                    pevent.Graphics.DrawPath(penSurface, pathBorder);
                    if (borderSize >= 0)
                    {
                        pevent.Graphics.DrawPath(penBorder, pathBorder);
                    }
                }
            }
            else
            {
                this.Region = new Region(rectSurface);
                if (borderSize > 0)
                {
                    using (Pen penBorder = new Pen(bordercolor, borderSize))
                    {
                        penBorder.Alignment = PenAlignment.Inset;
                        pevent.Graphics.DrawRectangle(penBorder, 0, 0, this.Width - 1, this.Height - 1);
                    }
                }
            }
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.Parent.BackColorChanged += new EventHandler(Container_BackColorChanged);
        }

        private void Container_BackColorChanged(object sender, EventArgs e)
        {
            if (this.DesignMode)
                this.Invalidate();
        }
    }
}
