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
            // Вычисляем разницы по осям X и Y
            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);

            // Определяем шаги (в каком направлении двигаться по X и Y)
            int stepX = x0 < x1 ? 1 : -1;
            int stepY = y0 < y1 ? 1 : -1;

            // Проверяем, какая ось больше, и меняем роль осей, если наклон большой
            bool steep = dy > dx;
            if (steep)
            {
                // Меняем местами dx и dy, поскольку будем двигаться по Y
                Swap(ref dx, ref dy);
                Swap(ref x0, ref y0);
                Swap(ref x1, ref y1);
                Swap(ref stepX, ref stepY);
            }

            // Инициализируем параметр D
            int D = 2 * dy - dx;
            int y = y0;

            // Цикл по основной оси (теперь это может быть либо X, либо Y, в зависимости от наклона)
            for (int x = x0; x != x1; x += stepX)
            {
                // Рисуем пиксель, учитывая, какая ось является основной
                if (steep)
                {
                    bm.SetPixel(y, x, Color.Black); // Если наклон большой, меняем местами x и y
                }
                else
                {
                    bm.SetPixel(x, y, Color.Black); // Обычная отрисовка для пологих линий
                }

                // Если D больше 0, увеличиваем y на 1 и уменьшаем D
                if (D > 0)
                {
                    y += stepY;
                    D -= 2 * dx;
                }

                // Увеличиваем D на 2 * dy
                D += 2 * dy;
            }
        }


        private void Wu(int x0, int y0, int x1, int y1)
        {
            // Если начальная точка правее конечной, меняем точки местами
            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }

            // Вычисляем разницу по осям X и Y
            int dx = x1 - x0;
            int dy = y1 - y0;
            double gradient;

            // Определяем наклон (градиент) линии
            if (dx == 0)
            {
                // Если dx == 0, линия вертикальная, градиент равен 1
                gradient = 1;
            }
            else if (dy == 0)
            {
                // Если dy == 0, линия горизонтальная, градиент равен 0
                gradient = 0;
            }
            else
            {
                // В остальных случаях вычисляем градиент
                gradient = dy / (double)dx;
            }

            int step = 1; // Шаг для перемещения по пикселям
            double xi = x0; // Начальная координата X
            double yi = y0; // Начальная координата Y

            // Если наклон линии больше 1, то она крутая и мы будем двигаться по оси Y
            if (Math.Abs(gradient) > 1)
            {
                // Меняем ось наклона и инвертируем градиент
                gradient = 1 / gradient;

                // Если градиент отрицательный, начнем с правой точки
                if (gradient < 0)
                {
                    xi = x1; // Начинаем с конечной координаты X
                    step = -1; // Шаг назад для обратного направления
                    Swap(ref y0, ref y1); // Меняем начальные и конечные точки по Y
                }

                // Цикл для отрисовки линии, движемся по Y
                for (yi = y0; yi <= y1; yi += 1)
                {
                    int help;
                    // Определяем интенсивность цвета для антиалиасинга
                    if (gradient < 0)
                    {
                        help = (int)(MAX_COLOR_VALUE * (xi - (int)xi)); // Ближайший к линии пиксель
                    }
                    else
                    {
                        help = MAX_COLOR_VALUE - (int)(MAX_COLOR_VALUE * (xi - (int)xi)); // Более дальний пиксель
                    }

                    // Рисуем основной пиксель с рассчитанной интенсивностью
                    bm.SetPixel((int)xi, (int)yi, Color.FromArgb(MAX_COLOR_VALUE - help, MAX_COLOR_VALUE - help, MAX_COLOR_VALUE - help));
                    // Рисуем соседний пиксель для сглаживания
                    bm.SetPixel((int)xi + step, (int)yi, Color.FromArgb(help, help, help));

                    xi += gradient; // Сдвиг по оси X на основе градиента
                }
            }
            else
            {
                // Если наклон меньше 1, будем двигаться по оси X
                if (gradient < 0)
                {
                    step = -1; // Шаг назад для отрицательного наклона
                }

                // Цикл для отрисовки линии, движемся по X
                for (xi = x0; xi <= x1; xi += 1)
                {
                    int help;
                    // Определяем интенсивность цвета для антиалиасинга
                    if (gradient < 0)
                    {
                        help = (int)(MAX_COLOR_VALUE * (yi - (int)yi)); // Ближайший к линии пиксель
                    }
                    else
                    {
                        help = MAX_COLOR_VALUE - (int)(MAX_COLOR_VALUE * (yi - (int)yi)); // Более дальний пиксель
                    }

                    // Рисуем основной пиксель с рассчитанной интенсивностью
                    bm.SetPixel((int)xi, (int)yi, Color.FromArgb(MAX_COLOR_VALUE - help, MAX_COLOR_VALUE - help, MAX_COLOR_VALUE - help));
                    // Рисуем соседний пиксель для сглаживания
                    bm.SetPixel((int)xi, (int)yi + step, Color.FromArgb(help, help, help));

                    yi += gradient; // Сдвиг по оси Y на основе градиента
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