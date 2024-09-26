using System;
using System.Drawing;
using System.Windows.Forms;

namespace GradientTriangle
{
    public partial class Form1 : Form
    {
        // Координаты вершин треугольника
        private PointF[] trianglePoints = new PointF[3];
        // Цвета для каждой вершины
        private Color[] triangleColors = new Color[3];
        // Переменные для перетаскивания вершин
        private bool isDragging = false;
        private int draggedPointIndex = -1;

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;  // Для избежания мерцания при перерисовке

            // Инициализация вершин треугольника
            trianglePoints[0] = new PointF(100, 100);
            trianglePoints[1] = new PointF(300, 100);
            trianglePoints[2] = new PointF(200, 300);

            // Инициализация цветов вершин
            triangleColors[0] = Color.Red;
            triangleColors[1] = Color.Green;
            triangleColors[2] = Color.Blue;

            // Подписываемся на события мыши
            this.MouseDown += Form1_MouseDown;
            this.MouseMove += Form1_MouseMove;
            this.MouseUp += Form1_MouseUp;
            this.Paint += Form1_Paint;
        }

        // Проверка на попадание в вершину треугольника
        private bool IsPointInSquare(PointF point, PointF vertex, float size = 10)
        {
            return Math.Abs(point.X - vertex.X) < size && Math.Abs(point.Y - vertex.Y) < size;
        }

        // Обработчик нажатия на мышь
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < trianglePoints.Length; i++)
            {
                if (IsPointInSquare(e.Location, trianglePoints[i]))
                {
                    isDragging = true;
                    draggedPointIndex = i;
                    break;
                }
            }
        }

        // Обработчик перемещения мыши
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && draggedPointIndex != -1)
            {
                // Перемещаем выбранную вершину
                trianglePoints[draggedPointIndex] = e.Location;
                this.Invalidate();  // Перерисовка формы
            }
        }

        // Обработчик отпускания кнопки мыши
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
            draggedPointIndex = -1;
            this.Invalidate();  // Перерисовка формы с новым положением
        }

        // Обработчик перерисовки
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Отрисовка градиента внутри треугольника
            FillGradientTriangle(g, trianglePoints, triangleColors);

            // Отрисовка вершин треугольника в виде квадратов для перемещения
            foreach (PointF point in trianglePoints)
            {
                g.FillRectangle(Brushes.Black, point.X - 5, point.Y - 5, 10, 10);
            }
        }

        // Функция для заливки треугольника с градиентом
        private void FillGradientTriangle(Graphics g, PointF[] points, Color[] colors)
        {
            Bitmap bitmap = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            using (Graphics bmpGraphics = Graphics.FromImage(bitmap))
            {
                // Используем интерполяцию для градиента
                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        PointF p = new PointF(x, y);
                        if (IsPointInTriangle(p, points))
                        {
                            Color interpolatedColor = InterpolateColor(p, points, colors);
                            bitmap.SetPixel(x, y, interpolatedColor);
                        }
                    }
                }
            }

            g.DrawImage(bitmap, 0, 0);
        }

        // Функция проверки, находится ли точка внутри треугольника
        private bool IsPointInTriangle(PointF p, PointF[] triangle)
        {
            float area = TriangleArea(triangle[0], triangle[1], triangle[2]);
            float area1 = TriangleArea(p, triangle[1], triangle[2]);
            float area2 = TriangleArea(triangle[0], p, triangle[2]);
            float area3 = TriangleArea(triangle[0], triangle[1], p);

            return Math.Abs(area - (area1 + area2 + area3)) < 0.01;
        }

        // Вычисление площади треугольника
        private float TriangleArea(PointF p1, PointF p2, PointF p3)
        {
            return Math.Abs((p1.X * (p2.Y - p3.Y) + p2.X * (p3.Y - p1.Y) + p3.X * (p1.Y - p2.Y)) / 2.0f);
        }

        // Функция интерполяции цвета в зависимости от положения точки внутри треугольника
        private Color InterpolateColor(PointF p, PointF[] points, Color[] colors)
        {
            float totalArea = TriangleArea(points[0], points[1], points[2]);

            float w1 = TriangleArea(p, points[1], points[2]) / totalArea;
            float w2 = TriangleArea(p, points[0], points[2]) / totalArea;
            float w3 = TriangleArea(p, points[0], points[1]) / totalArea;

            int r = (int)(w1 * colors[0].R + w2 * colors[1].R + w3 * colors[2].R);
            int g = (int)(w1 * colors[0].G + w2 * colors[1].G + w3 * colors[2].G);
            int b = (int)(w1 * colors[0].B + w2 * colors[1].B + w3 * colors[2].B);

            return Color.FromArgb(r, g, b);
        }
    }
}
