using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace cg_lab_3_1_b
{
    public partial class Form1 : Form
    {
        private Bitmap canvas; // Основной холст для рисования
        private Bitmap fillPattern; // Изображение для заливки
        private Color boundaryColor = Color.Black; // Цвет границы фигуры
        private List<Point> polygonPoints; // Точки для пользовательской фигуры
        private bool isDrawing = false; // Флаг для отслеживания режима рисования

        public Form1()
        {
            polygonPoints = new List<Point>();
            InitializeComponent();
            canvas = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = canvas;
        }

        // Заливка области изображением
        private void Fill(int x, int y, Color targetColor)
        {
            if (x < 0 || x >= canvas.Width || y < 0 || y >= canvas.Height) return; // Проверка границ холста

            Color currentColor = canvas.GetPixel(x, y);
            if (currentColor != targetColor || currentColor == boundaryColor) return; // Прекращаем, если цвет не совпадает или это граница

            // Создаем стек для обработки
            Stack<Point> pixels = new Stack<Point>();
            pixels.Push(new Point(x, y));

            while (pixels.Count > 0)
            {
                Point pt = pixels.Pop();
                int px = pt.X;
                int py = pt.Y;

                if (px < 0 || px >= canvas.Width || py < 0 || py >= canvas.Height) continue; // Проверка границ
                currentColor = canvas.GetPixel(px, py);

                if (currentColor != targetColor || currentColor == boundaryColor) continue; // Проверка цвета

                // Закрашиваем текущий пиксель
                canvas.SetPixel(px, py, fillPattern.GetPixel(px % fillPattern.Width, py % fillPattern.Height));

                // Добавляем соседние пиксели в стек
                pixels.Push(new Point(px + 1, py));
                pixels.Push(new Point(px - 1, py));
                pixels.Push(new Point(px, py + 1));
                pixels.Push(new Point(px, py - 1));
            }

            pictureBox1.Image = canvas; // Обновляем изображение в PictureBox
            pictureBox1.Invalidate(); // Перерисовываем PictureBox
        }

        // Загрузка изображения для заливки
        private void LoadFillPattern()
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fillPattern = new Bitmap(openFileDialog1.FileName);
            }
        }

        // Отрисовка границы фигуры и отображение её на холсте
        private void DrawPolygonBoundary()
        {
            if (polygonPoints.Count > 1)
            {
                using (Graphics g = Graphics.FromImage(canvas))
                {
                    Pen pen = new Pen(boundaryColor, 2);

                    // Рисуем пользовательскую фигуру на canvas
                    g.DrawLines(pen, polygonPoints.ToArray());

                    pictureBox1.Image = canvas; // Обновляем изображение в PictureBox
                    pictureBox1.Invalidate(); // Перерисовываем PictureBox для отображения изменений
                }
            }
        }

        // Добавление меню для загрузки изображения
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            MenuStrip menuStrip = new MenuStrip();
            ToolStripMenuItem fileMenu = new ToolStripMenuItem("File");
            ToolStripMenuItem loadImageItem = new ToolStripMenuItem("Load Image");

            loadImageItem.Click += (sender, args) => LoadFillPattern();
            fileMenu.DropDownItems.Add(loadImageItem);
            menuStrip.Items.Add(fileMenu);
            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);
        }

        // Обработка события при отпускании мыши
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                polygonPoints.Add(e.Location); // Добавляем последнюю точку
                isDrawing = false;
                DrawPolygonBoundary(); // Рисуем границу после завершения рисования
            }
        }

        // Обработка события при нажатии мыши
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                polygonPoints.Clear(); // Очищаем предыдущие точки фигуры
                polygonPoints.Add(e.Location); // Добавляем первую точку
                isDrawing = true;
            }
        }

        // Обработка движения мыши
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                polygonPoints.Add(e.Location); // Добавляем точку при движении мыши
                DrawPolygonBoundary(); // Отображаем текущую границу фигуры
            }
        }

        // Обработка клика мышью для заливки
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (!isDrawing && polygonPoints.Count > 2 && e.Button == MouseButtons.Right)
            {
                if (fillPattern == null)
                {
                    MessageBox.Show("Пожалуйста, загрузите файл изображения для заливки.");
                    return;
                }

                Color clickedColor = canvas.GetPixel(e.X, e.Y);

                // Если кликнутый цвет не является цветом границы и не является белым (цвет фона), запускаем заливку
                if (clickedColor != boundaryColor && clickedColor != Color.White)
                {
                    Fill(e.X, e.Y, clickedColor);
                }
                else
                {
                    MessageBox.Show("Вы кликнули на цвет границы или цвет фона. Пожалуйста, выберите другую область.");
                }
            }
        }

        private void Form1_ClientSizeChanged(object sender, EventArgs e)
        {
            canvas = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = canvas;
        }
    }
}
