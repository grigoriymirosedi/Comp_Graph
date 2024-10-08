using System.Drawing;
namespace lab4
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button btnTranslate;
        private System.Windows.Forms.Button btnRotate;
        private System.Windows.Forms.Button btnScale;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TextBox txtDx;
        private System.Windows.Forms.TextBox txtDy;
        private System.Windows.Forms.TextBox txtAngle;
        private System.Windows.Forms.TextBox txtScaleX;
        private System.Windows.Forms.TextBox txtScaleY;
        private System.Windows.Forms.Label lblDx;
        private System.Windows.Forms.Label lblDy;
        private System.Windows.Forms.Label lblAngle;
        private System.Windows.Forms.Label lblScaleX;
        private System.Windows.Forms.Label lblScaleY;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.btnTranslate = new System.Windows.Forms.Button();
            this.btnRotate = new System.Windows.Forms.Button();
            this.btnScale = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.txtDx = new System.Windows.Forms.TextBox();
            this.txtDy = new System.Windows.Forms.TextBox();
            this.txtAngle = new System.Windows.Forms.TextBox();
            this.txtScaleX = new System.Windows.Forms.TextBox();
            this.txtScaleY = new System.Windows.Forms.TextBox();
            this.lblDx = new System.Windows.Forms.Label();
            this.lblDy = new System.Windows.Forms.Label();
            this.lblAngle = new System.Windows.Forms.Label();
            this.lblScaleX = new System.Windows.Forms.Label();
            this.lblScaleY = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.BackColor = System.Drawing.Color.White;
            this.pictureBox.Location = new System.Drawing.Point(12, 12);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(500, 400);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // btnTranslate
            // 
            this.btnTranslate.Location = new System.Drawing.Point(755, 34);
            this.btnTranslate.Name = "btnTranslate";
            this.btnTranslate.Size = new System.Drawing.Size(100, 23);
            this.btnTranslate.TabIndex = 1;
            this.btnTranslate.Text = "Смещение";
            this.btnTranslate.Click += new System.EventHandler(this.btnTranslate_Click);
            // 
            // btnRotate
            // 
            this.btnRotate.Location = new System.Drawing.Point(755, 98);
            this.btnRotate.Name = "btnRotate";
            this.btnRotate.Size = new System.Drawing.Size(100, 23);
            this.btnRotate.TabIndex = 2;
            this.btnRotate.Text = "Поворот";
            this.btnRotate.Click += new System.EventHandler(this.btnRotate_Click);
            // 
            // btnScale
            // 
            this.btnScale.Location = new System.Drawing.Point(755, 158);
            this.btnScale.Name = "btnScale";
            this.btnScale.Size = new System.Drawing.Size(100, 23);
            this.btnScale.TabIndex = 3;
            this.btnScale.Text = "Масштаб";
            this.btnScale.Click += new System.EventHandler(this.btnScale_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(620, 250);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(100, 23);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "Очистить";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // txtDx
            // 
            this.txtDx.Location = new System.Drawing.Point(620, 20);
            this.txtDx.Name = "txtDx";
            this.txtDx.Size = new System.Drawing.Size(100, 20);
            this.txtDx.TabIndex = 5;
            // 
            // txtDy
            // 
            this.txtDy.Location = new System.Drawing.Point(620, 60);
            this.txtDy.Name = "txtDy";
            this.txtDy.Size = new System.Drawing.Size(100, 20);
            this.txtDy.TabIndex = 6;
            // 
            // txtAngle
            // 
            this.txtAngle.Location = new System.Drawing.Point(620, 100);
            this.txtAngle.Name = "txtAngle";
            this.txtAngle.Size = new System.Drawing.Size(100, 20);
            this.txtAngle.TabIndex = 7;
            // 
            // txtScaleX
            // 
            this.txtScaleX.Location = new System.Drawing.Point(620, 140);
            this.txtScaleX.Name = "txtScaleX";
            this.txtScaleX.Size = new System.Drawing.Size(100, 20);
            this.txtScaleX.TabIndex = 8;
            // 
            // txtScaleY
            // 
            this.txtScaleY.Location = new System.Drawing.Point(620, 180);
            this.txtScaleY.Name = "txtScaleY";
            this.txtScaleY.Size = new System.Drawing.Size(100, 20);
            this.txtScaleY.TabIndex = 9;
            // 
            // lblDx
            // 
            this.lblDx.Location = new System.Drawing.Point(599, 20);
            this.lblDx.Name = "lblDx";
            this.lblDx.Size = new System.Drawing.Size(100, 23);
            this.lblDx.TabIndex = 10;
            this.lblDx.Text = "dx:";
            // 
            // lblDy
            // 
            this.lblDy.Location = new System.Drawing.Point(599, 60);
            this.lblDy.Name = "lblDy";
            this.lblDy.Size = new System.Drawing.Size(100, 23);
            this.lblDy.TabIndex = 11;
            this.lblDy.Text = "dy:";
            // 
            // lblAngle
            // 
            this.lblAngle.Location = new System.Drawing.Point(587, 100);
            this.lblAngle.Name = "lblAngle";
            this.lblAngle.Size = new System.Drawing.Size(100, 23);
            this.lblAngle.TabIndex = 12;
            this.lblAngle.Text = "Угол:";
            // 
            // lblScaleX
            // 
            this.lblScaleX.Location = new System.Drawing.Point(553, 140);
            this.lblScaleX.Name = "lblScaleX";
            this.lblScaleX.Size = new System.Drawing.Size(100, 23);
            this.lblScaleX.TabIndex = 13;
            this.lblScaleX.Text = "Масштаб X:";
            // 
            // lblScaleY
            // 
            this.lblScaleY.Location = new System.Drawing.Point(553, 180);
            this.lblScaleY.Name = "lblScaleY";
            this.lblScaleY.Size = new System.Drawing.Size(100, 23);
            this.lblScaleY.TabIndex = 14;
            this.lblScaleY.Text = "Масштаб Y:";
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(984, 761);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.btnTranslate);
            this.Controls.Add(this.btnRotate);
            this.Controls.Add(this.btnScale);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.txtDx);
            this.Controls.Add(this.txtDy);
            this.Controls.Add(this.txtAngle);
            this.Controls.Add(this.txtScaleX);
            this.Controls.Add(this.txtScaleY);
            this.Controls.Add(this.lblDx);
            this.Controls.Add(this.lblDy);
            this.Controls.Add(this.lblAngle);
            this.Controls.Add(this.lblScaleX);
            this.Controls.Add(this.lblScaleY);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
    }
}


