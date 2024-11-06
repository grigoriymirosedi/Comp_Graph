using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace lab6
{
    public partial class Form1 : Form
    {
        private Polyhedron polyhedron;
        private ProjectionType projection = ProjectionType.Perspective;

        public Form1()
        {
            InitializeComponent();
            polyhedron = new Polyhedron();
            polyhedron.CreateTetrahedron(); // По умолчанию отображаем тетраэдр
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            polyhedron.Draw(e.Graphics, ClientRectangle, projection);
            if (float.TryParse(x1TextBox.Text, out float x1) && float.TryParse(y1TextBox.Text, out float y1) && float.TryParse(z1TextBox.Text, out float z1) &&
                float.TryParse(x2TextBox.Text, out float x2) && float.TryParse(y2TextBox.Text, out float y2) && float.TryParse(z2TextBox.Text, out float z2) &&
                float.TryParse(RotateTextBox.Text, out float angle))
            {
                Point3D point1 = new Point3D(x1, y1, z1);
                Point3D point2 = new Point3D(x2, y2, z2);
                polyhedron.DrawLine(e.Graphics, point1, point2, ClientRectangle, projection);
            }
        }

        private void ScaleButton_Click(object sender, EventArgs e)
        {
            if (float.TryParse(ScaleTextBox.Text, out float factor))
            {
                polyhedron.Scale(factor);
                Invalidate();
            }
        }

        private void OffsetButton_Click(object sender, EventArgs e)
        {
            if (float.TryParse(OffsetXTextBox.Text, out float dx) &&
                float.TryParse(OffsetYTextBox.Text, out float dy) &&
                float.TryParse(OffsetZTextBox.Text, out float dz))
            {
                polyhedron.Offset(dx, dy, dz);
                Invalidate();
            }
        }

        private void RotateXButton_Click(object sender, EventArgs e)
        {
            if (float.TryParse(RotateTextBox.Text, out float angle))
            {
                polyhedron.RotateAroundAxis(Axis.X, angle);
                Invalidate();
            }
        }

        private void RotateYButton_Click(object sender, EventArgs e)
        {
            if (float.TryParse(RotateTextBox.Text, out float angle))
            {
                polyhedron.RotateAroundAxis(Axis.Y, angle);
                Invalidate();
            }
        }

        private void RotateZButton_Click(object sender, EventArgs e)
        {
            if (float.TryParse(RotateTextBox.Text, out float angle))
            {
                polyhedron.RotateAroundAxis(Axis.Z, angle);
                Invalidate();
            }
        }

        private void ReflectXYButton_Click(object sender, EventArgs e)
        {
            polyhedron.Reflect(Axis.Z);
            Invalidate();
        }

        private void ReflectXZButton_Click(object sender, EventArgs e)
        {
            polyhedron.Reflect(Axis.Y);
            Invalidate();
        }

        private void ReflectYZButton_Click(object sender, EventArgs e)
        {
            polyhedron.Reflect(Axis.X);
            Invalidate();
        }

        private void ProjectionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProjectionComboBox.SelectedItem.ToString() == "Perspective")
                projection = ProjectionType.Perspective;
            else
                projection = ProjectionType.Axonometric;

            Invalidate();
        }

        private void TetrahedronButton_Click(object sender, EventArgs e)
        {
            polyhedron.CreateTetrahedron();
            Invalidate();
        }

        private void HexahedronButton_Click(object sender, EventArgs e)
        {
            polyhedron.CreateHexahedron();
            Invalidate();
        }

        private void OctahedronButton_Click(object sender, EventArgs e)
        {
            polyhedron.CreateOctahedron();
            Invalidate();
        }

        private void RotateAroundAxisCenterButton_Click(object sender, EventArgs e)
        {
            if (axisComboBox.Text == "") return;
            if (Enum.TryParse(axisComboBox.SelectedItem.ToString(), out Axis axis) && float.TryParse(RotateTextBox.Text, out float angle))
            {
                polyhedron.RotateAroundAxisCenter(axis, angle);
                Invalidate();
            }
        }

        private void RotateAroundLineButton_Click(object sender, EventArgs e)
        {
            if (float.TryParse(x1TextBox.Text, out float x1) && float.TryParse(y1TextBox.Text, out float y1) && float.TryParse(z1TextBox.Text, out float z1) &&
                float.TryParse(x2TextBox.Text, out float x2) && float.TryParse(y2TextBox.Text, out float y2) && float.TryParse(z2TextBox.Text, out float z2) &&
                float.TryParse(RotateTextBox.Text, out float angle))
            {
                Point3D point1 = new Point3D(x1, y1, z1);
                Point3D point2 = new Point3D(x2, y2, z2);
                polyhedron.RotateAroundLine(point1, point2, angle);
                Invalidate();
            }
        }
    }

    public class Point3D
    {
        public float X, Y, Z;

        public Point3D(float x, float y, float z)
        {
            X = x; Y = y; Z = z;
        }

        public Point3D Transform(float[,] matrix)
        {
            float newX = X * matrix[0, 0] + Y * matrix[0, 1] + Z * matrix[0, 2] + matrix[0, 3];
            float newY = X * matrix[1, 0] + Y * matrix[1, 1] + Z * matrix[1, 2] + matrix[1, 3];
            float newZ = X * matrix[2, 0] + Y * matrix[2, 1] + Z * matrix[2, 2] + matrix[2, 3];
            return new Point3D(newX, newY, newZ);
        }
    }

    public class Polygon
    {
        public List<Point3D> Points;

        public Polygon(List<Point3D> points)
        {
            Points = points;
        }
    }

    public class Polyhedron
    {
        private List<Polygon> faces = new List<Polygon>();

        public void CreateTetrahedron()
        {
            Point3D p1 = new Point3D(1, 1, 1);
            Point3D p2 = new Point3D(-1, -1, 1);
            Point3D p3 = new Point3D(-1, 1, -1);
            Point3D p4 = new Point3D(1, -1, -1);

            faces.Clear();
            faces.Add(new Polygon(new List<Point3D> { p1, p2, p3 }));
            faces.Add(new Polygon(new List<Point3D> { p1, p2, p4 }));
            faces.Add(new Polygon(new List<Point3D> { p1, p3, p4 }));
            faces.Add(new Polygon(new List<Point3D> { p2, p3, p4 }));
        }

        public void CreateHexahedron()
        {
            Point3D p1 = new Point3D(-1, -1, -1);
            Point3D p2 = new Point3D(1, -1, -1);
            Point3D p3 = new Point3D(1, 1, -1);
            Point3D p4 = new Point3D(-1, 1, -1);
            Point3D p5 = new Point3D(-1, -1, 1);
            Point3D p6 = new Point3D(1, -1, 1);
            Point3D p7 = new Point3D(1, 1, 1);
            Point3D p8 = new Point3D(-1, 1, 1);

            faces.Clear();
            faces.Add(new Polygon(new List<Point3D> { p1, p2, p3, p4 }));
            faces.Add(new Polygon(new List<Point3D> { p5, p6, p7, p8 }));
            faces.Add(new Polygon(new List<Point3D> { p1, p2, p6, p5 }));
            faces.Add(new Polygon(new List<Point3D> { p2, p3, p7, p6 }));
            faces.Add(new Polygon(new List<Point3D> { p3, p4, p8, p7 }));
            faces.Add(new Polygon(new List<Point3D> { p4, p1, p5, p8 }));
        }

        public void CreateOctahedron()
        {
            Point3D p1 = new Point3D(0, 0, 1);
            Point3D p2 = new Point3D(1, 0, 0);
            Point3D p3 = new Point3D(0, 1, 0);
            Point3D p4 = new Point3D(-1, 0, 0);
            Point3D p5 = new Point3D(0, -1, 0);
            Point3D p6 = new Point3D(0, 0, -1);

            faces.Clear();
            faces.Add(new Polygon(new List<Point3D> { p1, p2, p3 }));
            faces.Add(new Polygon(new List<Point3D> { p1, p3, p4 }));
            faces.Add(new Polygon(new List<Point3D> { p1, p4, p5 }));
            faces.Add(new Polygon(new List<Point3D> { p1, p5, p2 }));
            faces.Add(new Polygon(new List<Point3D> { p6, p3, p2 }));
            faces.Add(new Polygon(new List<Point3D> { p6, p4, p3 }));
            faces.Add(new Polygon(new List<Point3D> { p6, p5, p4 }));
            faces.Add(new Polygon(new List<Point3D> { p6, p2, p5 }));
        }

        public void DrawAxes(Graphics g, Rectangle clientRect, ProjectionType projection)
        {
            // Ось X — красная, Y — зелёная, Z — синяя
            Point3D origin = new Point3D(0, 0, 0);
            Point3D xEnd = new Point3D(2, 0, 0);
            Point3D yEnd = new Point3D(0, 2, 0);
            Point3D zEnd = new Point3D(0, 0, 2);

            // Преобразуем точки для отображения в выбранной проекции
            Point origin2D = ProjectTo2D(origin, clientRect, projection);
            Point xEnd2D = ProjectTo2D(xEnd, clientRect, projection);
            Point yEnd2D = ProjectTo2D(yEnd, clientRect, projection);
            Point zEnd2D = ProjectTo2D(zEnd, clientRect, projection);

            // Рисуем оси
            g.DrawLine(Pens.Red, origin2D, xEnd2D);    // Ось X
            g.DrawLine(Pens.Green, origin2D, yEnd2D);  // Ось Y
            g.DrawLine(Pens.Blue, origin2D, zEnd2D);   // Ось Z
        }

        private Point ProjectTo2D(Point3D point, Rectangle clientRect, ProjectionType projection)
        {
            if (projection == ProjectionType.Perspective)
            {
                float scale = 300 / (point.Z + 5);
                int x = (int)(clientRect.Width / 2 + point.X * scale);
                int y = (int)(clientRect.Height / 2 - point.Y * scale);
                return new Point(x, y);
            }
            else if (projection == ProjectionType.Axonometric)
            {
                int x = (int)(clientRect.Width / 2 + point.X * 40 - point.Z * 20);
                int y = (int)(clientRect.Height / 2 - point.Y * 40 - point.Z * 10);
                return new Point(x, y);
            }
            return Point.Empty;
        }

        public void DrawLine(Graphics g, Point3D p1, Point3D p2, Rectangle clientRect, ProjectionType projection)
        {
            // Преобразуем 3D точки в 2D с учетом проекции
            Point p1_2D = ProjectTo2D(p1, clientRect, projection);
            Point p2_2D = ProjectTo2D(p2, clientRect, projection);

            // Рисуем линию между этими точками
            g.DrawLine(Pens.Black, p1_2D, p2_2D);
        }

        public void Draw(Graphics g, Rectangle clientRect, ProjectionType projection)
        {
            DrawAxes(g, clientRect, projection);


            foreach (var face in faces)
            {
                Point[] points = new Point[face.Points.Count];
                for (int i = 0; i < face.Points.Count; i++)
                {
                    Point3D point = face.Points[i];

                    if (projection == ProjectionType.Perspective)
                    {
                        float scale = 300 / (point.Z + 5);
                        int x = (int)(clientRect.Width / 2 + point.X * scale);
                        int y = (int)(clientRect.Height / 2 - point.Y * scale);
                        points[i] = new Point(x, y);
                    }
                    else if (projection == ProjectionType.Axonometric)
                    {
                        int x = (int)(clientRect.Width / 2 + point.X * 40 - point.Z * 20);
                        int y = (int)(clientRect.Height / 2 - point.Y * 40 - point.Z * 10);
                        points[i] = new Point(x, y);
                    }
                }
                g.DrawPolygon(Pens.Black, points);
            }
        }

        public void Scale(float factor)
        {
            // Матрица масштабирования
            float[,] scaleMatrix = {
                { factor, 0, 0, 0 },
                { 0, factor, 0, 0 },
                { 0, 0, factor, 0 },
                { 0, 0, 0, 1 }
            };
            Transform(scaleMatrix);
        }

        public void Offset(float dx, float dy, float dz)
        {
            float[,] offsetMatrix = {
                { 1, 0, 0, dx },
                { 0, 1, 0, dy },
                { 0, 0, 1, dz },
                { 0, 0, 0, 1 }
            };
            Transform(offsetMatrix);
        }

        public void RotateAroundAxis(Axis axis, float angle)
        {
            float radians = angle * (float)Math.PI / 180;
            float cos = (float)Math.Cos(radians);
            float sin = (float)Math.Sin(radians);

            float[,] rotationMatrix;
            if (axis == Axis.X)
            {
                rotationMatrix = new float[,] {
                    { 1, 0, 0, 0 },
                    { 0, cos, -sin, 0 },
                    { 0, sin, cos, 0 },
                    { 0, 0, 0, 1 }
                };
            }
            else if (axis == Axis.Y)
            {
                rotationMatrix = new float[,] {
                    { cos, 0, sin, 0 },
                    { 0, 1, 0, 0 },
                    { -sin, 0, cos, 0 },
                    { 0, 0, 0, 1 }
                };
            }
            else
            {
                rotationMatrix = new float[,] {
                    { cos, -sin, 0, 0 },
                    { sin, cos, 0, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 1 }
                };
            }
            Transform(rotationMatrix);
        }

        public void RotateAroundAxisCenter(Axis axis, float angle)
        {
            // Сначала находим центр многогранника
            Point3D center = GetCenter();

            // Перемещаем многогранник так, чтобы его центр оказался в начале координат
            Offset(-center.X, -center.Y, -center.Z);

            // Выполняем вращение вокруг оси
            RotateAroundAxis(axis, angle);

            // Возвращаем многогранник на место
            Offset(center.X, center.Y, center.Z);
        }

        private Point3D GetCenter()
        {
            float x = 0, y = 0, z = 0;
            int pointCount = 0;
            foreach (var face in faces)
            {
                foreach (var point in face.Points)
                {
                    x += point.X;
                    y += point.Y;
                    z += point.Z;
                    pointCount++;
                }
            }
            return new Point3D(x / pointCount, y / pointCount, z / pointCount);
        }

        public void RotateAroundLine(Point3D point1, Point3D point2, float angle)
        {
            // 1. Находим вектор направления прямой
            float dx = point2.X - point1.X;
            float dy = point2.Y - point1.Y;
            float dz = point2.Z - point1.Z;

            // 2. Нормализуем вектор направления прямой
            float length = (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
            dx /= length;
            dy /= length;
            dz /= length;

            // 3. Перемещаем многогранник так, чтобы точка point1 стала в начале координат
            Offset(-point1.X, -point1.Y, -point1.Z);

            // 4. Выполняем вращение вокруг оси
            float radians = angle * (float)Math.PI / 180;
            float cos = (float)Math.Cos(radians);
            float sin = (float)Math.Sin(radians);

            // Создаем матрицу для вращения вокруг произвольной оси
            float[,] rotationMatrix = new float[,]
            {
        { cos + dx * dx * (1 - cos), dx * dy * (1 - cos) - dz * sin, dx * dz * (1 - cos) + dy * sin, 0 },
        { dy * dx * (1 - cos) + dz * sin, cos + dy * dy * (1 - cos), dy * dz * (1 - cos) - dx * sin, 0 },
        { dz * dx * (1 - cos) - dy * sin, dz * dy * (1 - cos) + dx * sin, cos + dz * dz * (1 - cos), 0 },
        { 0, 0, 0, 1 }
            };

            Transform(rotationMatrix);

            // 5. Возвращаем многогранник обратно
            Offset(point1.X, point1.Y, point1.Z);
        }

        public void Reflect(Axis axis)
        {
            float[,] reflectMatrix;
            if (axis == Axis.X)
            {
                reflectMatrix = new float[,] {
                    { -1, 0, 0, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 1 }
                };
            }
            else if (axis == Axis.Y)
            {
                reflectMatrix = new float[,] {
                    { 1, 0, 0, 0 },
                    { 0, -1, 0, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 1 }
                };
            }
            else
            {
                reflectMatrix = new float[,] {
                    { 1, 0, 0, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 0, -1, 0 },
                    { 0, 0, 0, 1 }
                };
            }
            Transform(reflectMatrix);
        }

        private void Transform(float[,] matrix)
        {
            for (int i = 0; i < faces.Count; i++)
            {
                for (int j = 0; j < faces[i].Points.Count; j++)
                {
                    faces[i].Points[j] = faces[i].Points[j].Transform(matrix);
                }
            }
        }
    }

    public enum Axis { X, Y, Z }
    public enum ProjectionType { Perspective, Axonometric }
}
