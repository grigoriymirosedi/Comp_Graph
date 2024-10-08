using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace lab_3_1_c
{
    public partial class Form1 : Form
    {
        private Bitmap canvas;  // Хранит изображение, на котором рисует пользователь
        private Point? lastPoint = null;  // Последняя нарисованная точка
        private Color boundaryColor = Color.DarkGreen;  // Цвет границы

        public Form1()
        {
            InitializeComponent();
            canvas = new Bitmap(ClientSize.Width, ClientSize.Height);
        }

        // Обработка нажатия мыши (начало рисования границы)
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                lastPoint = e.Location;
            }
        }

        // Обработка движения мыши (рисование границы)
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && lastPoint.HasValue)
            {
                using (Graphics g = Graphics.FromImage(canvas))
                {
                    g.DrawLine(new Pen(boundaryColor, 3), lastPoint.Value, e.Location);
                }
                lastPoint = e.Location;
                this.Invalidate();  // Обновляем отображение формы
            }
        }

        // Обработка отпускания мыши (завершение рисования)
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                lastPoint = null;
            }
        }

        // Обработка нажатия кнопки для поиска границы
        private void buttonFindBoundary_Click(object sender, EventArgs e)
        {
           
        }

        // Поиск начальной точки границы (любой пиксель с цветом границы)
        private Point FindStartingPoint()
        {
            for (int y = 0; y < canvas.Height; y++)
            {
                for (int x = 0; x < canvas.Width; x++)
                {
                    if (canvas.GetPixel(x, y).ToArgb() == boundaryColor.ToArgb())
                    {
                        return new Point(x, y);
                    }
                }
            }
            return Point.Empty;  // Если граница не найдена
        }

        // Поиск границы методом DFS с использованием восьмисвязного поиска
        private List<Point> FindBoundary(Point startPoint)
        {
            List<Point> boundaryPoints = new List<Point>();
            Stack<Point> stack = new Stack<Point>();
            bool[,] visited = new bool[canvas.Width, canvas.Height];

            // Направления для восьмисвязного обхода
            Point[] directions = {
                new Point(0, -1), new Point(0, 1), new Point(-1, 0), new Point(1, 0),
                new Point(-1, -1), new Point(1, -1), new Point(-1, 1), new Point(1, 1)
            };

            stack.Push(startPoint);
            visited[startPoint.X, startPoint.Y] = true;

            while (stack.Count > 0)
            {
                Point currentPoint = stack.Pop();
                boundaryPoints.Add(currentPoint);

                foreach (var dir in directions)
                {
                    Point neighbor = new Point(currentPoint.X + dir.X, currentPoint.Y + dir.Y);

                    if (IsValid(neighbor, visited))
                    {
                        Color neighborColor = canvas.GetPixel(neighbor.X, neighbor.Y);
                        if (neighborColor.ToArgb() == boundaryColor.ToArgb())
                        {
                            stack.Push(neighbor);
                            visited[neighbor.X, neighbor.Y] = true;
                        }
                    }
                }
            }

            return boundaryPoints;
        }

        // Проверяем, находится ли точка в пределах изображения и не посещалась ли она
        private bool IsValid(Point point, bool[,] visited)
        {
            return point.X >= 0 && point.X < canvas.Width &&
                   point.Y >= 0 && point.Y < canvas.Height &&
                   !visited[point.X, point.Y];
        }

        // Прорисовка найденной границы
        private void DrawFoundBoundary(List<Point> boundaryPoints)
        {
            using (Graphics g = Graphics.FromImage(canvas))
            {
                foreach (var point in boundaryPoints)
                {
                    g.FillRectangle(Brushes.Blue, point.X, point.Y, 1, 1);  // Рисуем границу синим цветом
                }
            }
            this.Invalidate();  // Обновляем отображение формы
        }

        // Отрисовка формы
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawImage(canvas, 0, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Point startPoint = FindStartingPoint();

            if (startPoint == Point.Empty)
            {
                MessageBox.Show("Граница не найдена!");
                return;
            }

            List<Point> boundaryPoints = FindBoundary(startPoint);
            DrawFoundBoundary(boundaryPoints);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
