using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cg_lab3
{
    public partial class Form1 : Form
    {
        private Bitmap bitmap;
        private bool isDrawing = false;
        private Point previousPoint;


        public Form1()
        {
            InitializeComponent();
            bitmap = new Bitmap(this.Width, this.Height);
            pictureBox2.Image = bitmap;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

       

        private void FillLines(int x, int y, Color targetColor, Color fillColor)
        {
            if (x < 0 || x >= bitmap.Width || y < 0 || y >= bitmap.Height)
                return;

            if (bitmap.GetPixel(x, y) != targetColor)
                return;

            int left = x;
            int right = x;

            // Находим левую границу строки для заливки
            while (left > 0 && bitmap.GetPixel(left - 1, y) == targetColor)
            {
                left--;
            }

            // Находим правую границу строки для заливки
            while (right < bitmap.Width - 1 && bitmap.GetPixel(right + 1, y) == targetColor)
            {
                right++;
            }

            // Заполняем строку от левой до правой границы
            for (int i = left; i <= right; i++)
            {
                bitmap.SetPixel(i, y, fillColor);
            }

            // Рекурсивно вызываем для строки выше и строки ниже
            for (int i = left; i <= right; i++)
            {
                if (y > 0 && bitmap.GetPixel(i, y - 1) == targetColor)
                {
                    FillLines(i, y - 1, targetColor, fillColor);
                }

                if (y < bitmap.Height - 1 && bitmap.GetPixel(i, y + 1) == targetColor)
                {
                    FillLines(i, y + 1, targetColor, fillColor);
                }
            }
        }

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDrawing = false;
            }
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDrawing = true;
                previousPoint = e.Location; // Запоминаем начальную точку
            }
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    Pen pen = new Pen(Color.Black, 4);  
                    g.DrawLine(pen, previousPoint, e.Location);  
                    previousPoint = e.Location;  
                }
                pictureBox2.Invalidate(); 
            }
        }

        private void pictureBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Color targetColor = bitmap.GetPixel(e.X, e.Y);
            Color fillColor = Color.DarkGreen;

            if (targetColor != fillColor && targetColor != Color.Black)  
            {
                FillLines(e.X, e.Y, targetColor, fillColor);
                pictureBox2.Invalidate();  // Обновляем изображение
            }
        }
    }
}
