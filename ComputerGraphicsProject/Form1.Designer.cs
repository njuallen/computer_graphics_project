namespace ComputerGraphicsProject
{
    partial class FormPaint
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPaint));
            this.buttonTrimming = new System.Windows.Forms.Button();
            this.buttonPolygon = new System.Windows.Forms.Button();
            this.buttonFill = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonEllipse = new System.Windows.Forms.Button();
            this.buttonCircle = new System.Windows.Forms.Button();
            this.buttonDDA = new System.Windows.Forms.Button();
            this.buttonBresenham = new System.Windows.Forms.Button();
            this.buttonBezier = new System.Windows.Forms.Button();
            this.buttonBspline = new System.Windows.Forms.Button();
            this.buttonTranslation = new System.Windows.Forms.Button();
            this.buttonRotate = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.buttonScale = new System.Windows.Forms.Button();
            this.buttonClearing = new System.Windows.Forms.Button();
            this.buttonWPF3D = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonTrimming
            // 
            this.buttonTrimming.Image = ((System.Drawing.Image)(resources.GetObject("buttonTrimming.Image")));
            this.buttonTrimming.Location = new System.Drawing.Point(360, 0);
            this.buttonTrimming.Name = "buttonTrimming";
            this.buttonTrimming.Size = new System.Drawing.Size(40, 40);
            this.buttonTrimming.TabIndex = 7;
            this.buttonTrimming.UseVisualStyleBackColor = true;
            this.buttonTrimming.Click += new System.EventHandler(this.buttonTrimming_Click);
            // 
            // buttonPolygon
            // 
            this.buttonPolygon.Image = ((System.Drawing.Image)(resources.GetObject("buttonPolygon.Image")));
            this.buttonPolygon.Location = new System.Drawing.Point(240, 0);
            this.buttonPolygon.Name = "buttonPolygon";
            this.buttonPolygon.Size = new System.Drawing.Size(40, 40);
            this.buttonPolygon.TabIndex = 6;
            this.buttonPolygon.UseVisualStyleBackColor = true;
            this.buttonPolygon.Click += new System.EventHandler(this.buttonPolygon_Click);
            // 
            // buttonFill
            // 
            this.buttonFill.Image = ((System.Drawing.Image)(resources.GetObject("buttonFill.Image")));
            this.buttonFill.Location = new System.Drawing.Point(400, 0);
            this.buttonFill.Name = "buttonFill";
            this.buttonFill.Size = new System.Drawing.Size(40, 40);
            this.buttonFill.TabIndex = 5;
            this.buttonFill.UseVisualStyleBackColor = true;
            this.buttonFill.Click += new System.EventHandler(this.buttonFill_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Image = ((System.Drawing.Image)(resources.GetObject("buttonSave.Image")));
            this.buttonSave.Location = new System.Drawing.Point(0, 0);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(40, 40);
            this.buttonSave.TabIndex = 4;
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonEllipse
            // 
            this.buttonEllipse.Image = ((System.Drawing.Image)(resources.GetObject("buttonEllipse.Image")));
            this.buttonEllipse.Location = new System.Drawing.Point(200, 0);
            this.buttonEllipse.Name = "buttonEllipse";
            this.buttonEllipse.Size = new System.Drawing.Size(40, 40);
            this.buttonEllipse.TabIndex = 3;
            this.buttonEllipse.UseVisualStyleBackColor = true;
            this.buttonEllipse.Click += new System.EventHandler(this.buttonEllipse_Click);
            // 
            // buttonCircle
            // 
            this.buttonCircle.Image = ((System.Drawing.Image)(resources.GetObject("buttonCircle.Image")));
            this.buttonCircle.Location = new System.Drawing.Point(160, 0);
            this.buttonCircle.Name = "buttonCircle";
            this.buttonCircle.Size = new System.Drawing.Size(40, 40);
            this.buttonCircle.TabIndex = 2;
            this.buttonCircle.UseVisualStyleBackColor = true;
            this.buttonCircle.Click += new System.EventHandler(this.buttonCircle_Click);
            // 
            // buttonDDA
            // 
            this.buttonDDA.Image = ((System.Drawing.Image)(resources.GetObject("buttonDDA.Image")));
            this.buttonDDA.Location = new System.Drawing.Point(80, 0);
            this.buttonDDA.Name = "buttonDDA";
            this.buttonDDA.Size = new System.Drawing.Size(40, 40);
            this.buttonDDA.TabIndex = 1;
            this.buttonDDA.UseVisualStyleBackColor = true;
            this.buttonDDA.Click += new System.EventHandler(this.buttonDDA_Click);
            // 
            // buttonBresenham
            // 
            this.buttonBresenham.Cursor = System.Windows.Forms.Cursors.Default;
            this.buttonBresenham.Image = ((System.Drawing.Image)(resources.GetObject("buttonBresenham.Image")));
            this.buttonBresenham.Location = new System.Drawing.Point(120, 0);
            this.buttonBresenham.Name = "buttonBresenham";
            this.buttonBresenham.Size = new System.Drawing.Size(40, 40);
            this.buttonBresenham.TabIndex = 0;
            this.buttonBresenham.UseVisualStyleBackColor = true;
            this.buttonBresenham.Click += new System.EventHandler(this.buttonBresenham_Click);
            // 
            // buttonBezier
            // 
            this.buttonBezier.Image = ((System.Drawing.Image)(resources.GetObject("buttonBezier.Image")));
            this.buttonBezier.Location = new System.Drawing.Point(280, 0);
            this.buttonBezier.Name = "buttonBezier";
            this.buttonBezier.Size = new System.Drawing.Size(40, 40);
            this.buttonBezier.TabIndex = 8;
            this.buttonBezier.UseVisualStyleBackColor = true;
            this.buttonBezier.Click += new System.EventHandler(this.buttonBezier_Click);
            // 
            // buttonBspline
            // 
            this.buttonBspline.Image = ((System.Drawing.Image)(resources.GetObject("buttonBspline.Image")));
            this.buttonBspline.Location = new System.Drawing.Point(320, 0);
            this.buttonBspline.Name = "buttonBspline";
            this.buttonBspline.Size = new System.Drawing.Size(40, 40);
            this.buttonBspline.TabIndex = 9;
            this.buttonBspline.UseVisualStyleBackColor = true;
            this.buttonBspline.Click += new System.EventHandler(this.buttonBspline_Click);
            // 
            // buttonTranslation
            // 
            this.buttonTranslation.Image = ((System.Drawing.Image)(resources.GetObject("buttonTranslation.Image")));
            this.buttonTranslation.Location = new System.Drawing.Point(440, 0);
            this.buttonTranslation.Name = "buttonTranslation";
            this.buttonTranslation.Size = new System.Drawing.Size(40, 40);
            this.buttonTranslation.TabIndex = 10;
            this.buttonTranslation.UseVisualStyleBackColor = true;
            this.buttonTranslation.Click += new System.EventHandler(this.buttonTranslation_Click);
            // 
            // buttonRotate
            // 
            this.buttonRotate.Image = ((System.Drawing.Image)(resources.GetObject("buttonRotate.Image")));
            this.buttonRotate.Location = new System.Drawing.Point(480, 0);
            this.buttonRotate.Name = "buttonRotate";
            this.buttonRotate.Size = new System.Drawing.Size(40, 40);
            this.buttonRotate.TabIndex = 11;
            this.buttonRotate.UseVisualStyleBackColor = true;
            this.buttonRotate.Click += new System.EventHandler(this.buttonRotate_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "bmp";
            this.saveFileDialog1.FileName = "draw";
            this.saveFileDialog1.Filter = "JPEG|*.jpeg|BMP|*.bmp|PNG|*.png";
            this.saveFileDialog1.Title = "保存图片";
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // buttonScale
            // 
            this.buttonScale.Image = ((System.Drawing.Image)(resources.GetObject("buttonScale.Image")));
            this.buttonScale.Location = new System.Drawing.Point(520, 0);
            this.buttonScale.Name = "buttonScale";
            this.buttonScale.Size = new System.Drawing.Size(40, 40);
            this.buttonScale.TabIndex = 12;
            this.buttonScale.UseVisualStyleBackColor = true;
            this.buttonScale.Click += new System.EventHandler(this.buttonScale_Click);
            // 
            // buttonClearing
            // 
            this.buttonClearing.Image = ((System.Drawing.Image)(resources.GetObject("buttonClearing.Image")));
            this.buttonClearing.Location = new System.Drawing.Point(40, 0);
            this.buttonClearing.Name = "buttonClearing";
            this.buttonClearing.Size = new System.Drawing.Size(40, 40);
            this.buttonClearing.TabIndex = 13;
            this.buttonClearing.UseVisualStyleBackColor = true;
            this.buttonClearing.Click += new System.EventHandler(this.buttonClearing_Click);
            // 
            // buttonWPF3D
            // 
            this.buttonWPF3D.Image = ((System.Drawing.Image)(resources.GetObject("buttonWPF3D.Image")));
            this.buttonWPF3D.Location = new System.Drawing.Point(560, 0);
            this.buttonWPF3D.Name = "buttonWPF3D";
            this.buttonWPF3D.Size = new System.Drawing.Size(40, 40);
            this.buttonWPF3D.TabIndex = 14;
            this.buttonWPF3D.UseVisualStyleBackColor = true;
            this.buttonWPF3D.Click += new System.EventHandler(this.buttonWPF3D_Click);
            // 
            // FormPaint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.buttonWPF3D);
            this.Controls.Add(this.buttonClearing);
            this.Controls.Add(this.buttonScale);
            this.Controls.Add(this.buttonRotate);
            this.Controls.Add(this.buttonTranslation);
            this.Controls.Add(this.buttonBspline);
            this.Controls.Add(this.buttonBezier);
            this.Controls.Add(this.buttonTrimming);
            this.Controls.Add(this.buttonPolygon);
            this.Controls.Add(this.buttonFill);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonEllipse);
            this.Controls.Add(this.buttonCircle);
            this.Controls.Add(this.buttonDDA);
            this.Controls.Add(this.buttonBresenham);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPaint";
            this.Text = "画图";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Line_MouseClick);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Line_MouseDoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Line_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Line_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Line_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonBresenham;
        private System.Windows.Forms.Button buttonDDA;
        private System.Windows.Forms.Button buttonCircle;
        private System.Windows.Forms.Button buttonEllipse;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonFill;
        private System.Windows.Forms.Button buttonPolygon;
        private System.Windows.Forms.Button buttonTrimming;
        private System.Windows.Forms.Button buttonBezier;
        private System.Windows.Forms.Button buttonBspline;
        private System.Windows.Forms.Button buttonTranslation;
        private System.Windows.Forms.Button buttonRotate;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button buttonScale;
        private System.Windows.Forms.Button buttonClearing;
        private System.Windows.Forms.Button buttonWPF3D;
    }
}

