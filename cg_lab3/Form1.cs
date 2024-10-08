using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3
{
    public partial class Form1 : Form
    {

        Graphics _graphics;
        Bitmap bitmap;
        Point[] point = new Point[3];
        Color[] colors = new Color[3];
        int index = 0;
        Pen p = new Pen(Color.Red, 1);

        public Form1()
        {
            InitializeComponent();
            bitmap = new Bitmap(pic.Width, pic.Height);
            _graphics = Graphics.FromImage(bitmap);
            _graphics.Clear(Color.White);
            pic.Image = bitmap;
        }

        private void pic_MouseClick(object sender, MouseEventArgs e)
        {
            if (index < 2)
            {
                point[index] = e.Location;
                colors[index] = p.Color;
                index++;
                _graphics.DrawRectangle(p, e.Location.X, e.Location.Y, 1, 1);
                pic.Refresh();
            }
            else
            {
                index = 0;
                point[2] = e.Location;
                colors[2] = p.Color;
                _graphics.DrawRectangle(p, e.Location.X, e.Location.Y, 1, 1);

                DrawTriangle();
                pic.Refresh();
            }
        }

        private void DrawTriangle()
        {

            // Сортировка точек по y-координате для корректной работы
            if (point[1].Y < point[0].Y) Swap(0, 1);
            if (point[2].Y < point[0].Y) Swap(0, 2);
            if (point[2].Y < point[1].Y) Swap(1, 2);

            int top_y = point[0].Y;

            while (top_y < point[1].Y)
            {
                float Xleft, Xright;
                Xleft = InterpolateX(point[0], point[1], point[2], top_y, true, false);
                Xright = InterpolateX(point[0], point[1], point[2], top_y, false, false);

                // Интерполяция цвета на левом и правом краях
                Color Cleft = InterpolateColor(point[0], point[1], colors[0], colors[1], top_y);
                Color Cright = InterpolateColor(point[0], point[2], colors[0], colors[2], top_y);

                if (Xleft > Xright)
                {
                    (Xleft, Xright) = (Xright, Xleft);
                    (Cleft, Cright) = (Cright, Cleft);
                }

                // Интерполяция по x между Cleft и Cright
                for (float x = Xleft; x <= Xright; x++)
                {
                    float t = (x - Xleft) / (Xright - Xleft); // Нормализованный коэффициент для интерполяции
                    Color interpolatedColor = InterpolateColor(Cleft, Cright, t);

                    p.Color = interpolatedColor;
                    _graphics.DrawRectangle(p, x, top_y, 1, 1);
                }
                top_y++;
            }
            while (top_y < point[2].Y)
            {
                float Xleft, Xright;
                Xleft = InterpolateX(point[0], point[1], point[2], top_y, true, true);
                Xright = InterpolateX(point[0], point[1], point[2], top_y, false, true);

                // Интерполяция цвета на левом и правом краях
                Color Cleft = InterpolateColor(point[1], point[2], colors[1], colors[2], top_y);
                Color Cright = InterpolateColor(point[0], point[2], colors[0], colors[2], top_y);

                if (Xleft > Xright)
                {
                    (Xleft, Xright) = (Xright, Xleft);
                    (Cleft, Cright) = (Cright, Cleft);
                }

                // Интерполяция по x между Cleft и Cright
                for (float x = Xleft; x <= Xright; x++)
                {
                    float t = (x - Xleft) / (Xright - Xleft); // Нормализованный коэффициент для интерполяции
                    Color interpolatedColor = InterpolateColor(Cleft, Cright, t);

                    p.Color = interpolatedColor;
                    _graphics.DrawRectangle(p, x, top_y, 1, 1);
                }
                top_y++;
            }
        }

        // Линейная интерполяция для нахождения X на отрезке для данного y
        private float InterpolateX(Point p0, Point p1, Point p2, float y, bool isLeft, bool afterMid)
        {
            if (!afterMid)
            {
                if (isLeft)
                {

                    if (p0.Y == p1.Y) return p0.X;
                    return p0.X + (p1.X - p0.X) * (y - p0.Y) / (p1.Y - p0.Y);

                }
                else
                {


                    if (p0.Y == p2.Y) return p0.X;
                    return p0.X + (p2.X - p0.X) * (y - p0.Y) / (p2.Y - p0.Y);

                }
            }
            else
            {
                if (isLeft)
                {

                    if (p1.Y == p2.Y) return p1.X;
                    return p1.X + (p2.X - p1.X) * (y - p1.Y) / (p2.Y - p1.Y);

                }
                else
                {


                    if (p0.Y == p2.Y) return p0.X;
                    return p0.X + (p2.X - p0.X) * (y - p0.Y) / (p2.Y - p0.Y);

                }
            }
        }

        // Линейная интерполяция цвета по оси y
        private Color InterpolateColor(Point p0, Point p1, Color c0, Color c1, float y)
        {
            if (p0.Y == p1.Y) return c0; // Защита от деления на 0
            float t = (y - p0.Y) / (p1.Y - p0.Y);
            return InterpolateColor(c0, c1, t);
        }

        // Линейная интерполяция между двумя цветами по t (от 0 до 1)
        private Color InterpolateColor(Color c1, Color c2, float t)
        {
            int r = (int)Math.Round(c1.R + (c2.R - c1.R) * t);
            int g = (int)Math.Round(c1.G + (c2.G - c1.G) * t);
            int b = (int)Math.Round(c1.B + (c2.B - c1.B) * t);

            // Ограничение значений цветов в диапазоне от 0 до 255
            r = Clamp(r, 0, 255);
            g = Clamp(g, 0, 255);
            b = Clamp(b, 0, 255);

            return Color.FromArgb(r, g, b);
        }

        private int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        // Вспомогательная функция для обмена точками и цветами
        private void Swap(int i, int j)
        {
            Point tempPoint = point[i];
            point[i] = point[j];
            point[j] = tempPoint;

            Color tempColor = colors[i];
            colors[i] = colors[j];
            colors[j] = tempColor;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Button button = (sender as Button);
            {
                p.Color = button.BackColor;
            }
        }

        private void Erase_Click(object sender, EventArgs e)
        {
            _graphics.Clear(Color.White);
            index = 0;
            pic.Refresh();
        }
    }
}
