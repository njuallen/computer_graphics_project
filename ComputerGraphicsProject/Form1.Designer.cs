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
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonClearing = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonEdit = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDDA = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonBresenham = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonCircle = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonEllipse = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPolygon = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonBezier = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonBspline = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonTrimming = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonFill = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonTranslation = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRotate = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonScale = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonWPF3D = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDebug = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "bmp";
            this.saveFileDialog1.FileName = "draw";
            this.saveFileDialog1.Filter = "JPEG|*.jpeg|BMP|*.bmp|PNG|*.png";
            this.saveFileDialog1.Title = "保存图片";
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.Color.Silver;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSave,
            this.toolStripButtonClearing,
            this.toolStripButtonEdit,
            this.toolStripButtonDDA,
            this.toolStripButtonBresenham,
            this.toolStripButtonCircle,
            this.toolStripButtonEllipse,
            this.toolStripButtonPolygon,
            this.toolStripButtonBezier,
            this.toolStripButtonBspline,
            this.toolStripButtonTrimming,
            this.toolStripButtonFill,
            this.toolStripButtonTranslation,
            this.toolStripButtonRotate,
            this.toolStripButtonScale,
            this.toolStripButtonWPF3D,
            this.toolStripButtonDebug});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(784, 40);
            this.toolStrip1.TabIndex = 16;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.AutoSize = false;
            this.toolStripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSave.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSave.Image")));
            this.toolStripButtonSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(40, 40);
            this.toolStripButtonSave.Text = "保存";
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // toolStripButtonClearing
            // 
            this.toolStripButtonClearing.AutoSize = false;
            this.toolStripButtonClearing.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonClearing.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonClearing.Image")));
            this.toolStripButtonClearing.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonClearing.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonClearing.Name = "toolStripButtonClearing";
            this.toolStripButtonClearing.Size = new System.Drawing.Size(40, 40);
            this.toolStripButtonClearing.Text = "清空";
            this.toolStripButtonClearing.Click += new System.EventHandler(this.toolStripButtonClearing_Click);
            // 
            // toolStripButtonEdit
            // 
            this.toolStripButtonEdit.AutoSize = false;
            this.toolStripButtonEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonEdit.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonEdit.Image")));
            this.toolStripButtonEdit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonEdit.Name = "toolStripButtonEdit";
            this.toolStripButtonEdit.Size = new System.Drawing.Size(40, 40);
            this.toolStripButtonEdit.Text = "编辑图形";
            this.toolStripButtonEdit.Click += new System.EventHandler(this.toolStripButtonEdit_Click);
            // 
            // toolStripButtonDDA
            // 
            this.toolStripButtonDDA.AutoSize = false;
            this.toolStripButtonDDA.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDDA.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDDA.Image")));
            this.toolStripButtonDDA.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonDDA.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDDA.Name = "toolStripButtonDDA";
            this.toolStripButtonDDA.Size = new System.Drawing.Size(40, 40);
            this.toolStripButtonDDA.Text = "DDA直线";
            this.toolStripButtonDDA.Click += new System.EventHandler(this.toolStripButtonDDA_Click);
            // 
            // toolStripButtonBresenham
            // 
            this.toolStripButtonBresenham.AutoSize = false;
            this.toolStripButtonBresenham.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonBresenham.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonBresenham.Image")));
            this.toolStripButtonBresenham.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonBresenham.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonBresenham.Name = "toolStripButtonBresenham";
            this.toolStripButtonBresenham.Size = new System.Drawing.Size(40, 40);
            this.toolStripButtonBresenham.Text = "Bresenham直线";
            this.toolStripButtonBresenham.Click += new System.EventHandler(this.toolStripButtonBresenham_Click);
            // 
            // toolStripButtonCircle
            // 
            this.toolStripButtonCircle.AutoSize = false;
            this.toolStripButtonCircle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonCircle.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonCircle.Image")));
            this.toolStripButtonCircle.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonCircle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCircle.Name = "toolStripButtonCircle";
            this.toolStripButtonCircle.Size = new System.Drawing.Size(40, 40);
            this.toolStripButtonCircle.Text = "中点圆";
            this.toolStripButtonCircle.Click += new System.EventHandler(this.toolStripButtonCircle_Click);
            // 
            // toolStripButtonEllipse
            // 
            this.toolStripButtonEllipse.AutoSize = false;
            this.toolStripButtonEllipse.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonEllipse.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonEllipse.Image")));
            this.toolStripButtonEllipse.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonEllipse.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonEllipse.Name = "toolStripButtonEllipse";
            this.toolStripButtonEllipse.Size = new System.Drawing.Size(40, 40);
            this.toolStripButtonEllipse.Text = "中点椭圆";
            this.toolStripButtonEllipse.Click += new System.EventHandler(this.toolStripButtonEllipse_Click);
            // 
            // toolStripButtonPolygon
            // 
            this.toolStripButtonPolygon.AutoSize = false;
            this.toolStripButtonPolygon.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPolygon.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPolygon.Image")));
            this.toolStripButtonPolygon.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonPolygon.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPolygon.Name = "toolStripButtonPolygon";
            this.toolStripButtonPolygon.Size = new System.Drawing.Size(40, 40);
            this.toolStripButtonPolygon.Text = "多边形";
            this.toolStripButtonPolygon.Click += new System.EventHandler(this.toolStripButtonPolygon_Click);
            // 
            // toolStripButtonBezier
            // 
            this.toolStripButtonBezier.AutoSize = false;
            this.toolStripButtonBezier.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonBezier.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonBezier.Image")));
            this.toolStripButtonBezier.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonBezier.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonBezier.Name = "toolStripButtonBezier";
            this.toolStripButtonBezier.Size = new System.Drawing.Size(40, 40);
            this.toolStripButtonBezier.Text = "Bezier曲线";
            this.toolStripButtonBezier.Click += new System.EventHandler(this.toolStripButtonBezier_Click);
            // 
            // toolStripButtonBspline
            // 
            this.toolStripButtonBspline.AutoSize = false;
            this.toolStripButtonBspline.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonBspline.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonBspline.Image")));
            this.toolStripButtonBspline.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonBspline.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonBspline.Name = "toolStripButtonBspline";
            this.toolStripButtonBspline.Size = new System.Drawing.Size(40, 40);
            this.toolStripButtonBspline.Text = "B样条曲线";
            this.toolStripButtonBspline.Click += new System.EventHandler(this.toolStripButtonBspline_Click);
            // 
            // toolStripButtonTrimming
            // 
            this.toolStripButtonTrimming.AutoSize = false;
            this.toolStripButtonTrimming.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonTrimming.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonTrimming.Image")));
            this.toolStripButtonTrimming.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonTrimming.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonTrimming.Name = "toolStripButtonTrimming";
            this.toolStripButtonTrimming.Size = new System.Drawing.Size(40, 40);
            this.toolStripButtonTrimming.Text = "裁剪";
            this.toolStripButtonTrimming.Click += new System.EventHandler(this.toolStripButtonTrimming_Click);
            // 
            // toolStripButtonFill
            // 
            this.toolStripButtonFill.AutoSize = false;
            this.toolStripButtonFill.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFill.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonFill.Image")));
            this.toolStripButtonFill.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonFill.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFill.Name = "toolStripButtonFill";
            this.toolStripButtonFill.Size = new System.Drawing.Size(40, 40);
            this.toolStripButtonFill.Text = "泛滥填充";
            this.toolStripButtonFill.Click += new System.EventHandler(this.toolStripButtonFill_Click);
            // 
            // toolStripButtonTranslation
            // 
            this.toolStripButtonTranslation.AutoSize = false;
            this.toolStripButtonTranslation.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonTranslation.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonTranslation.Image")));
            this.toolStripButtonTranslation.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonTranslation.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonTranslation.Name = "toolStripButtonTranslation";
            this.toolStripButtonTranslation.Size = new System.Drawing.Size(40, 40);
            this.toolStripButtonTranslation.Text = "平移";
            this.toolStripButtonTranslation.Click += new System.EventHandler(this.toolStripButtonTranslation_Click);
            // 
            // toolStripButtonRotate
            // 
            this.toolStripButtonRotate.AutoSize = false;
            this.toolStripButtonRotate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRotate.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRotate.Image")));
            this.toolStripButtonRotate.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonRotate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRotate.Name = "toolStripButtonRotate";
            this.toolStripButtonRotate.Size = new System.Drawing.Size(40, 40);
            this.toolStripButtonRotate.Text = "旋转";
            this.toolStripButtonRotate.Click += new System.EventHandler(this.toolStripButtonRotate_Click);
            // 
            // toolStripButtonScale
            // 
            this.toolStripButtonScale.AutoSize = false;
            this.toolStripButtonScale.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonScale.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonScale.Image")));
            this.toolStripButtonScale.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonScale.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonScale.Name = "toolStripButtonScale";
            this.toolStripButtonScale.Size = new System.Drawing.Size(40, 40);
            this.toolStripButtonScale.Text = "缩放";
            this.toolStripButtonScale.Click += new System.EventHandler(this.toolStripButtonScale_Click);
            // 
            // toolStripButtonWPF3D
            // 
            this.toolStripButtonWPF3D.AutoSize = false;
            this.toolStripButtonWPF3D.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonWPF3D.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonWPF3D.Image")));
            this.toolStripButtonWPF3D.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonWPF3D.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonWPF3D.Name = "toolStripButtonWPF3D";
            this.toolStripButtonWPF3D.Size = new System.Drawing.Size(40, 40);
            this.toolStripButtonWPF3D.Text = "3D六面体";
            this.toolStripButtonWPF3D.Click += new System.EventHandler(this.toolStripButtonWPF3D_Click);
            // 
            // toolStripButtonDebug
            // 
            this.toolStripButtonDebug.AutoSize = false;
            this.toolStripButtonDebug.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDebug.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDebug.Image")));
            this.toolStripButtonDebug.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonDebug.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDebug.Name = "toolStripButtonDebug";
            this.toolStripButtonDebug.Size = new System.Drawing.Size(40, 40);
            this.toolStripButtonDebug.Text = "调试专用按钮";
            this.toolStripButtonDebug.Click += new System.EventHandler(this.toolStripButtonDebug_Click);
            // 
            // FormPaint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Cornsilk;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.toolStrip1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPaint";
            this.Text = "画图";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DefaultMouseEventHandler);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.DefaultMouseEventHandler);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Line_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Line_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Line_MouseUp);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.ToolStripButton toolStripButtonClearing;
        private System.Windows.Forms.ToolStripButton toolStripButtonDDA;
        private System.Windows.Forms.ToolStripButton toolStripButtonBresenham;
        private System.Windows.Forms.ToolStripButton toolStripButtonCircle;
        private System.Windows.Forms.ToolStripButton toolStripButtonEllipse;
        private System.Windows.Forms.ToolStripButton toolStripButtonPolygon;
        private System.Windows.Forms.ToolStripButton toolStripButtonBezier;
        private System.Windows.Forms.ToolStripButton toolStripButtonBspline;
        private System.Windows.Forms.ToolStripButton toolStripButtonTrimming;
        private System.Windows.Forms.ToolStripButton toolStripButtonEdit;
        private System.Windows.Forms.ToolStripButton toolStripButtonTranslation;
        private System.Windows.Forms.ToolStripButton toolStripButtonRotate;
        private System.Windows.Forms.ToolStripButton toolStripButtonScale;
        private System.Windows.Forms.ToolStripButton toolStripButtonWPF3D;
        private System.Windows.Forms.ToolStripButton toolStripButtonDebug;
        private System.Windows.Forms.ToolStripButton toolStripButtonFill;
    }
}

