namespace lab6
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        // Компоненты интерфейса
        private System.Windows.Forms.Button ScaleButton;
        private System.Windows.Forms.Button OffsetButton;
        private System.Windows.Forms.Button RotateXButton;
        private System.Windows.Forms.Button RotateYButton;
        private System.Windows.Forms.Button RotateZButton;
        private System.Windows.Forms.Button ReflectXYButton;
        private System.Windows.Forms.Button ReflectXZButton;
        private System.Windows.Forms.Button ReflectYZButton;
        private System.Windows.Forms.ComboBox ProjectionComboBox;
        private System.Windows.Forms.TextBox ScaleTextBox;
        private System.Windows.Forms.TextBox RotateTextBox;
        private System.Windows.Forms.TextBox OffsetXTextBox;
        private System.Windows.Forms.TextBox OffsetYTextBox;
        private System.Windows.Forms.TextBox OffsetZTextBox;
        private System.Windows.Forms.Button TetrahedronButton;
        private System.Windows.Forms.Button HexahedronButton;
        private System.Windows.Forms.Button OctahedronButton;

        /// <summary>
        /// Освободить используемые ресурсы
        /// </summary>
        /// <param name="disposing">true если управляемый ресурс должен быть удален; иначе false</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Метод для инициализации компонентов интерфейса
        /// </summary>
        private void InitializeComponent()
        {
            this.ScaleButton = new System.Windows.Forms.Button();
            this.OffsetButton = new System.Windows.Forms.Button();
            this.RotateXButton = new System.Windows.Forms.Button();
            this.RotateYButton = new System.Windows.Forms.Button();
            this.RotateZButton = new System.Windows.Forms.Button();
            this.ReflectXYButton = new System.Windows.Forms.Button();
            this.ReflectXZButton = new System.Windows.Forms.Button();
            this.ReflectYZButton = new System.Windows.Forms.Button();
            this.ProjectionComboBox = new System.Windows.Forms.ComboBox();
            this.ScaleTextBox = new System.Windows.Forms.TextBox();
            this.RotateTextBox = new System.Windows.Forms.TextBox();
            this.OffsetXTextBox = new System.Windows.Forms.TextBox();
            this.OffsetYTextBox = new System.Windows.Forms.TextBox();
            this.OffsetZTextBox = new System.Windows.Forms.TextBox();
            this.TetrahedronButton = new System.Windows.Forms.Button();
            this.HexahedronButton = new System.Windows.Forms.Button();
            this.OctahedronButton = new System.Windows.Forms.Button();

            // 
            // ScaleButton
            // 
            this.ScaleButton.Location = new System.Drawing.Point(10, 10);
            this.ScaleButton.Size = new System.Drawing.Size(75, 23);
            this.ScaleButton.Text = "Масштаб";
            this.ScaleButton.Click += new System.EventHandler(this.ScaleButton_Click);

            // 
            // OffsetButton
            // 
            this.OffsetButton.Location = new System.Drawing.Point(10, 40);
            this.OffsetButton.Size = new System.Drawing.Size(75, 23);
            this.OffsetButton.Text = "Смещение";
            this.OffsetButton.Click += new System.EventHandler(this.OffsetButton_Click);

            // 
            // RotateXButton
            // 
            this.RotateXButton.Location = new System.Drawing.Point(10, 70);
            this.RotateXButton.Size = new System.Drawing.Size(75, 23);
            this.RotateXButton.Text = "Поворот X";
            this.RotateXButton.Click += new System.EventHandler(this.RotateXButton_Click);

            // 
            // RotateYButton
            // 
            this.RotateYButton.Location = new System.Drawing.Point(10, 100);
            this.RotateYButton.Size = new System.Drawing.Size(75, 23);
            this.RotateYButton.Text = "Поворот Y";
            this.RotateYButton.Click += new System.EventHandler(this.RotateYButton_Click);

            // 
            // RotateZButton
            // 
            this.RotateZButton.Location = new System.Drawing.Point(10, 130);
            this.RotateZButton.Size = new System.Drawing.Size(75, 23);
            this.RotateZButton.Text = "Поворот Z";
            this.RotateZButton.Click += new System.EventHandler(this.RotateZButton_Click);

            // 
            // ReflectXYButton
            // 
            this.ReflectXYButton.Location = new System.Drawing.Point(10, 160);
            this.ReflectXYButton.Size = new System.Drawing.Size(75, 23);
            this.ReflectXYButton.Text = "Отразить XY";
            this.ReflectXYButton.Click += new System.EventHandler(this.ReflectXYButton_Click);

            // 
            // ReflectXZButton
            // 
            this.ReflectXZButton.Location = new System.Drawing.Point(10, 190);
            this.ReflectXZButton.Size = new System.Drawing.Size(75, 23);
            this.ReflectXZButton.Text = "Отразить XZ";
            this.ReflectXZButton.Click += new System.EventHandler(this.ReflectXZButton_Click);

            // 
            // ReflectYZButton
            // 
            this.ReflectYZButton.Location = new System.Drawing.Point(10, 220);
            this.ReflectYZButton.Size = new System.Drawing.Size(75, 23);
            this.ReflectYZButton.Text = "Отразить YZ";
            this.ReflectYZButton.Click += new System.EventHandler(this.ReflectYZButton_Click);

            // 
            // ProjectionComboBox
            // 
            this.ProjectionComboBox.Location = new System.Drawing.Point(10, 250);
            this.ProjectionComboBox.Size = new System.Drawing.Size(121, 21);
            this.ProjectionComboBox.Items.AddRange(new object[] { "Perspective", "Axonometric" });
            this.ProjectionComboBox.SelectedIndexChanged += new System.EventHandler(this.ProjectionComboBox_SelectedIndexChanged);

            // 
            // ScaleTextBox
            // 
            this.ScaleTextBox.Location = new System.Drawing.Point(100, 10);
            this.ScaleTextBox.Size = new System.Drawing.Size(50, 20);

            // 
            // RotateTextBox
            // 
            this.RotateTextBox.Location = new System.Drawing.Point(100, 40);
            this.RotateTextBox.Size = new System.Drawing.Size(50, 20);

            // 
            // OffsetXTextBox
            // 
            this.OffsetXTextBox.Location = new System.Drawing.Point(100, 70);
            this.OffsetXTextBox.Size = new System.Drawing.Size(50, 20);

            // 
            // OffsetYTextBox
            // 
            this.OffsetYTextBox.Location = new System.Drawing.Point(100, 100);
            this.OffsetYTextBox.Size = new System.Drawing.Size(50, 20);

            // 
            // OffsetZTextBox
            // 
            this.OffsetZTextBox.Location = new System.Drawing.Point(100, 130);
            this.OffsetZTextBox.Size = new System.Drawing.Size(50, 20);

            // 
            // TetrahedronButton
            // 
            this.TetrahedronButton.Location = new System.Drawing.Point(10, 280);
            this.TetrahedronButton.Size = new System.Drawing.Size(75, 23);
            this.TetrahedronButton.Text = "Тетраэдр";
            this.TetrahedronButton.Click += new System.EventHandler(this.TetrahedronButton_Click);

            // 
            // HexahedronButton
            // 
            this.HexahedronButton.Location = new System.Drawing.Point(10, 310);
            this.HexahedronButton.Size = new System.Drawing.Size(75, 23);
            this.HexahedronButton.Text = "Гексаэдр";
            this.HexahedronButton.Click += new System.EventHandler(this.HexahedronButton_Click);

            // 
            // OctahedronButton
            // 
            this.OctahedronButton.Location = new System.Drawing.Point(10, 340);
            this.OctahedronButton.Size = new System.Drawing.Size(75, 23);
            this.OctahedronButton.Text = "Октаэдр";
            this.OctahedronButton.Click += new System.EventHandler(this.OctahedronButton_Click);

            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.ScaleButton);
            this.Controls.Add(this.OffsetButton);
            this.Controls.Add(this.RotateXButton);
            this.Controls.Add(this.RotateYButton);
            this.Controls.Add(this.RotateZButton);
            this.Controls.Add(this.ReflectXYButton);
            this.Controls.Add(this.ReflectXZButton);
            this.Controls.Add(this.ReflectYZButton);
            this.Controls.Add(this.ProjectionComboBox);
            this.Controls.Add(this.ScaleTextBox);
            this.Controls.Add(this.RotateTextBox);
            this.Controls.Add(this.OffsetXTextBox);
            this.Controls.Add(this.OffsetYTextBox);
            this.Controls.Add(this.OffsetZTextBox);
            this.Controls.Add(this.TetrahedronButton);
            this.Controls.Add(this.HexahedronButton);
            this.Controls.Add(this.OctahedronButton);
            this.Text = "Polyhedron Transformations";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
        }
    }
}
