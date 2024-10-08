﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab4
{
    public partial class Form1 : Form
    {
        private Polygon currentPolygon; // Храним текущий полигон
        private bool isDrawing; // Флаг для режима рисования полигона

        public Form1()
        {
            InitializeComponent();
            currentPolygon = new Polygon();
            isDrawing = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox.BackColor = Color.White;
            pictureBox.Paint += pictureBox_Paint;
            pictureBox.MouseClick += pictureBox_MouseClick;
        }
        public static bool IsPointInPolygon(Point2D point, Polygon polygon)
        {
            bool inside = false;
            int count = polygon.Vertices.Count;

            for (int i = 0, j = count - 1; i < count; j = i++)
            {
                Point2D vi = polygon.Vertices[i];
                Point2D vj = polygon.Vertices[j];

                if ((vi.Y > point.Y) != (vj.Y > point.Y) &&
                    (point.X < (vj.X - vi.X) * (point.Y - vi.Y) / (vj.Y - vi.Y) + vi.X))
                {
                    inside = !inside;
                }
            }
            return inside;
        }
        public static string ClassifyPoint(Point2D p, Point2D a, Point2D b)
        {
            double result = (b.X - a.X) * (p.Y - a.Y) - (b.Y - a.Y) * (p.X - a.X);

            if (result > 0)
                return "Слева";
            else if (result < 0)
                return "Справа";
            else
                return "На линии";
        }
        private void btnTranslate_Click(object sender, EventArgs e)
        {
            double dx = double.Parse(txtDx.Text);
            double dy = double.Parse(txtDy.Text);
            currentPolygon.ApplyTransformation(AffineTransform.Translation(dx, dy));
            pictureBox.Invalidate(); // Перерисовка
        }

        private void btnRotate_Click(object sender, EventArgs e)
        {
            double angle = double.Parse(txtAngle.Text);
            Point2D center = currentPolygon.GetCentroid(); // Центр полигона
            currentPolygon.ApplyTransformation(AffineTransform.Rotation(angle, center));
            pictureBox.Invalidate(); // Обновление PictureBox
        }
        private void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                Point2D point = new Point2D(e.X, e.Y);
                currentPolygon.AddVertex(point);
                pictureBox.Invalidate(); // Обновляем PictureBox для перерисовки
            }
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (currentPolygon.Vertices.Count > 1)
            {
                for (int i = 0; i < currentPolygon.Vertices.Count - 1; i++)
                {
                    Point2D v1 = currentPolygon.Vertices[i];
                    Point2D v2 = currentPolygon.Vertices[i + 1];
                    e.Graphics.DrawLine(Pens.Black, (float)v1.X, (float)v1.Y, (float)v2.X, (float)v2.Y);
                }
                // Замыкаем полигон
                Point2D last = currentPolygon.Vertices.Last();
                Point2D first = currentPolygon.Vertices.First();
                e.Graphics.DrawLine(Pens.Black, (float)last.X, (float)last.Y, (float)first.X, (float)first.Y);
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            currentPolygon = new Polygon();
            pictureBox.Invalidate(); // Обновление PictureBox
        }

        private void btnScale_Click(object sender, EventArgs e)
        {
            double scaleX = double.Parse(txtScaleX.Text);
            double scaleY = double.Parse(txtScaleY.Text);
            Point2D center = currentPolygon.GetCentroid(); // Центр полигона
            currentPolygon.ApplyTransformation(AffineTransform.Scaling(scaleX, scaleY, center));
            pictureBox.Invalidate(); // Обновление PictureBox
        }



    }
    public class Point2D
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Point2D Transform(double[,] matrix)
        {
            double newX = X * matrix[0, 0] + Y * matrix[0, 1] + matrix[0, 2];
            double newY = X * matrix[1, 0] + Y * matrix[1, 1] + matrix[1, 2];
            return new Point2D(newX, newY);
        }
    }
    public class Polygon
    {
        public List<Point2D> Vertices { get; set; }

        public Polygon()
        {
            Vertices = new List<Point2D>();
        }

        // Добавление вершины
        public void AddVertex(Point2D point)
        {
            Vertices.Add(point);
        }

        // Применение трансформации ко всем вершинам полигона
        public void ApplyTransformation(double[,] matrix)
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                Vertices[i] = Vertices[i].Transform(matrix);
            }
        }

        // Найти центр полигона (среднее арифметическое всех вершин)
        public Point2D GetCentroid()
        {
            double sumX = 0, sumY = 0;
            foreach (var v in Vertices)
            {
                sumX += v.X;
                sumY += v.Y;
            }
            return new Point2D(sumX / Vertices.Count, sumY / Vertices.Count);
        }
    }
    public static class AffineTransform
    {
        public static double[,] Translation(double dx, double dy)
        {
            return new double[,]
            {
            { 1, 0, dx },
            { 0, 1, dy },
            { 0, 0, 1 }
            };
        }

        public static double[,] Rotation(double angle, Point2D center)
        {
            double radians = angle * Math.PI / 180.0;
            double cos = Math.Cos(radians);
            double sin = Math.Sin(radians);

            return new double[,]
            {
            { cos, -sin, center.X - center.X * cos + center.Y * sin },
            { sin, cos, center.Y - center.X * sin - center.Y * cos },
            { 0, 0, 1 }
            };
        }

        public static double[,] Scaling(double sx, double sy, Point2D center)
        {
            return new double[,]
            {
            { sx, 0, center.X - sx * center.X },
            { 0, sy, center.Y - sy * center.Y },
            { 0, 0, 1 }
            };
        }
    }
    public class Geometry
    {
        public static Point2D FindIntersection(Point2D p1, Point2D p2, Point2D p3, Point2D p4)
        {
            double d = (p1.X - p2.X) * (p3.Y - p4.Y) - (p1.Y - p2.Y) * (p3.X - p4.X);

            if (d == 0) return null; // Линии параллельны

            double xi = ((p3.X - p4.X) * (p1.X * p2.Y - p1.Y * p2.X) - (p1.X - p2.X) * (p3.X * p4.Y - p3.Y * p4.X)) / d;
            double yi = ((p3.Y - p4.Y) * (p1.X * p2.Y - p1.Y * p2.X) - (p1.Y - p2.Y) * (p3.X * p4.Y - p3.Y * p4.X)) / d;

            if (xi < Math.Min(p1.X, p2.X) || xi > Math.Max(p1.X, p2.X) ||
                xi < Math.Min(p3.X, p4.X) || xi > Math.Max(p3.X, p4.X))
                return null;

            if (yi < Math.Min(p1.Y, p2.Y) || yi > Math.Max(p1.Y, p2.Y) ||
                yi < Math.Min(p3.Y, p4.Y) || yi > Math.Max(p3.Y, p4.Y))
                return null;

            return new Point2D(xi, yi);
        }
    }
    
}