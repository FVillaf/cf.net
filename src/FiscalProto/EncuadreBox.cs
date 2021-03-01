using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiscalProto
{
    public class EncuadreBox : Panel
    {
        const int PIN_SIZE = 8;

        int captureMode = 0;

        Point capturePoint;

        public PictureBox ParentPictureBox { get; set; }

        public Action EncuadreChanged;

        public EncuadreBox()
        {
            this.Width = 100;
            this.Height = 100;
            this.MouseDown += EncuadreBox_MouseDown;
            this.MouseUp += EncuadreBox_MouseUp;
            this.MouseMove += EncuadreBox_MouseMove;
        }

        private void EncuadreBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (captureMode == 0) return;
            if (captureMode == 2)
            {
                int newLeft = this.Left + e.X - capturePoint.X;
                int newTop = this.Top + e.Y - capturePoint.Y;
                if (newLeft > 0 && newLeft + this.Width < ParentPictureBox.Right)
                    this.Left = newLeft;
                if (newTop > 0 && newTop + this.Height < ParentPictureBox.Bottom)
                    this.Top = newTop;
            }
            else
            {
                int newWidth = this.Width + e.X - capturePoint.X;
                int newHeigth = this.Height + e.Y - capturePoint.Y;
                if(newWidth > 50 && this.Left + newWidth < ParentPictureBox.Right)
                    this.Width = newWidth;
                if (newHeigth > 50 && this.Top + newHeigth < ParentPictureBox.Bottom)
                    this.Height = newHeigth;
                this.capturePoint = new Point(e.X, e.Y);
            }

            this.Invalidate();
            EncuadreChanged?.Invoke();
        }

        private void EncuadreBox_MouseUp(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
            this.captureMode = 0;
        }

        private void EncuadreBox_MouseDown(object sender, MouseEventArgs e)
        {
            capturePoint = new Point(e.X, e.Y);
            if (e.X >= this.Width - PIN_SIZE && e.Y >= this.Height - PIN_SIZE)
            {
                this.Cursor = Cursors.SizeNWSE;
                this.captureMode = 1;
                return;
            }

            this.Cursor = Cursors.SizeAll;
            this.captureMode = 2;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            int w = Width - 1;
            int h = Height - 1;
            e.Graphics.DrawLine(Pens.Red, 0, 0, w, 0);
            e.Graphics.DrawLine(Pens.Red, w, 0, w, h);
            e.Graphics.DrawLine(Pens.Red, w, h, 0, h);
            e.Graphics.DrawLine(Pens.Red, 0, h, 0, 0);

            if (ParentPictureBox != null && ParentPictureBox.Image != null)
            {
                var targetRect = new Rectangle(
                    this.Left, this.Top,
                    this.Width-2, this.Height-2);
                var bmp = new Bitmap(ParentPictureBox.Image);
                var pixels = bmp.Clone(targetRect, bmp.PixelFormat);
                e.Graphics.DrawImage(pixels, 1, 1);
                e.Graphics.FillRectangle(Brushes.Red, w - PIN_SIZE, h - PIN_SIZE, PIN_SIZE, PIN_SIZE);
                pixels.Dispose();
                bmp.Dispose();
            }
        }
    }
}
