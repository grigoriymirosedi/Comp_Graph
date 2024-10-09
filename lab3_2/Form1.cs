using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3
{
    public partial class Form1 : Form
    {
        Bitmap bm;
        Graphics g;
        List<Point> points;
        const int MAX_COLOR_VALUE = 255;
        Color penColor = Color.Black;
        Pen pen;
        public Form1()
        {
            InitializeComponent();
            bm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bm;
            Clear();

            points = new List<Point>();
            pen = new Pen(penColor);
            g = Graphics.FromImage(bm);
        }
        private void Clear()
        {
            var g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(pictureBox1.BackColor);
            pictureBox1.Image = pictureBox1.Image;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            points.Clear();
            Clear();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                points.Clear();
                Clear();
            }
            else if (radioButton2.Checked)
            {
                points.Clear();
                Clear();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Point point = pictureBox1.PointToClient(Cursor.Position);
            using (Graphics g = pictureBox1.CreateGraphics())
                if (points.Count < 2 && !points.Contains(point))
                {
                    g.FillRectangle(Brushes.Black, point.X, point.Y, 2, 2);
                    points.Add(point);
                }
            if (radioButton1.Checked && (points.Count == 2))
            {
                int x0 = points[points.Count - 2].X;
                int y0 = points[points.Count - 2].Y;
                int x1 = points[points.Count - 1].X;
                int y1 = points[points.Count - 1].Y;
                Bresenham(x0, y0, x1, y1);
                pictureBox1.Image = bm;
            }
            if (radioButton2.Checked && (points.Count == 2))
            {
                int x0 = points[points.Count - 2].X;
                int y0 = points[points.Count - 2].Y;
                int x1 = points[points.Count - 1].X;
                int y1 = points[points.Count - 1].Y;
                Wu(x0, y0, x1, y1);
                pictureBox1.Image = bm;
            }
        }
        private void Bresenham(int x0, int y0, int x1, int y1)
        {
            var dx = x1 - x0;
            var dy = y1 - y0;
            var distX = Math.Abs(dx);
            var distY = Math.Abs(dy);
            var stepX = dx < 0 ? -1 : 1;
            var stepY = dy < 0 ? -1 : 1;
            var dist = distX;
            if (distY >= distX)
            {
                dist = distY;
            }
            var errX = 0;
            var errY = 0;
            var dst = dist + 1;
            while (dst-- > 0)
            {
                bm.SetPixel(x0, y0, Color.Black);

                errX += distX;
                errY += distY;

                if (errX >= dist)
                {
                    errX -= dist;
                    x0 += stepX;
                }
                if (errY >= dist)
                {
                    errY -= dist;
                    y0 += stepY;
                }
            }
        }
        private void Wu(int x0, int y0, int x1, int y1)
        {
            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }

            int dx = x1 - x0;
            int dy = y1 - y0;
            double gradient;
            if (dx == 0)
            {
                gradient = 1;
            }
            else if (dy == 0)
            {
                gradient = 0;
            }
            else
            {
                gradient = dy / (double)dx;
            }

            int step = 1;
            double xi = x0;
            double yi = y0;

            if (Math.Abs(gradient) > 1)
            {
                gradient = 1 / gradient;
                if (gradient < 0)
                {
                    xi = x1;
                    step = -1;
                    Swap(ref y0, ref y1);
                }

                for (yi = y0; yi <= y1; yi += 1)
                {
                    int help;
                    if (gradient < 0)
                    {
                        help = (int)(MAX_COLOR_VALUE * (xi - (int)xi));
                    }
                    else
                    {
                        help = MAX_COLOR_VALUE - (int)(MAX_COLOR_VALUE * (xi - (int)xi));
                    }
                    bm.SetPixel((int)xi, (int)yi, Color.FromArgb(MAX_COLOR_VALUE - help, MAX_COLOR_VALUE - help, MAX_COLOR_VALUE - help));
                    bm.SetPixel((int)xi + step, (int)yi, Color.FromArgb(help, help, help));
                    xi += gradient;
                }
            }
            else
            {
                if (gradient < 0)
                {
                    step = -1;
                }
                for (xi = x0; xi <= x1; xi += 1)
                {
                    int help;
                    if (gradient < 0)
                    {
                        help = (int)(MAX_COLOR_VALUE * (yi - (int)yi));
                    }
                    else
                    {
                        help = MAX_COLOR_VALUE - (int)(MAX_COLOR_VALUE * (yi - (int)yi));
                    }
                    bm.SetPixel((int)xi, (int)yi, Color.FromArgb(MAX_COLOR_VALUE - help, MAX_COLOR_VALUE - help, MAX_COLOR_VALUE - help));
                    bm.SetPixel((int)xi, (int)yi + step, Color.FromArgb(help, help, help));
                    yi += gradient;
                }
            }
        }

        private void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }
    }
}