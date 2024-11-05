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

        public void Draw(Graphics g, Rectangle clientRect, ProjectionType projection)
        {
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
