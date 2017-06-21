// #define MYDEBUG


using System;
using System.Collections.Generic;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Wpf3D;


namespace ComputerGraphicsProject
{
    public partial class FormPaint : Form
    {
        public FormPaint()
        {
            InitializeComponent();
        }

        // *************************** 不变量 *****************************
        static int form_width = 800;
        static int form_height = 600;
        // 用于绘图的颜色的数量，不包括空白颜色
        static int numColor = 4;

        Color[] colors = { Color.Red, Color.Blue, Color.Yellow, Color.Black };

        int defaultColor = 0;
        int drawingColor = 1;
        int selectionColor = 2;
        public int fillColor = 3;

        // 如果这个像素点上啥都没画，就应该是这个颜色
        Color backgroundColor = Color.Gray;


        // *********************** 变量 *************************************
        public int[,,] screenBuffer = new int[form_width, form_height, numColor];
        Bitmap myCanvas;
        // 我们画出的图形
        List<Primitive> graphics = new List<Primitive>();

        // 当前正在画的直线/圆/椭圆/多边形的边/样条曲线控制边
        Primitive currPrimitive = null;

        // 已经画下来的多边形/样条曲线控制边
        List<Primitive> currPolygon = new List<Primitive>();

        // 当前在画多边形，样条曲线时已经记录下的顶点/控制点
        List<Point> vertices = new List<Point>();

        bool isMouseDown = false;

        Point firstPoint;
        Point prevPoint;

        // 被选中的图形
        Primitive selected = null;

        string mode = "Bresenham";

        // 这个变量记录的是指定图形累计被scale了多少
        double totalScaleFactor = 1.0;

        private void Init()
        {
            myCanvas = new Bitmap(form_width, form_height - 40);
            graphics.Clear();
            InitDraw();
        }

        // 不更新画布，只更新绘图要用的变量
        private void InitDraw()
        {
            currPrimitive = null;
            currPolygon.Clear();
            vertices.Clear();
            isMouseDown = false;
            firstPoint.X = -1;
            prevPoint.X = -1;
            selected = null;
            totalScaleFactor = 1.0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Init();
        }

        // 这个是用来进行完全重绘的
        // 我们平常的简单更新直接调用UpdateScreen就可以了
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // 显示已经画好的图形
            // UpdateScreen();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            Point point = PointToClient(Cursor.Position);
            isMouseDown = true;
            firstPoint = point;
            Console.WriteLine("MouseDown");
            // 这个三个操作需要选中图形
            // 距离鼠标位置距离小于3的图形被选中
            if (mode == "Translation" || mode == "Rotate" || mode == "Scale")
            {
                foreach (var g in graphics)
                {
                    if (g.Distance(point) <= 3.0)
                    {
                        g.isSelected = true;
                        selected = g;
                        // 被选中的图形原先肯定是defaultColor
                        // 图形被选中之后，立马就把这个图形的颜色给改掉
                        selected.UnDraw(defaultColor);
                        selected.Draw(selectionColor);
                        UpdateScreen();
                        return;
                    }
                }
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            Console.WriteLine("MouseUp");
            Point point = PointToClient(Cursor.Position);
            isMouseDown = false;
            if (mode == "DDA" || mode == "Bresenham" || mode == "Circle" ||
                mode == "Ellipse")
            {
                // 直线已经画完了，我们把它从蓝色变成红色
                if (currPrimitive != null)
                {
                    currPrimitive.UnDraw(drawingColor);
                    currPrimitive.Draw(defaultColor);
                    graphics.Add(currPrimitive);
                    currPrimitive = null;
                }
            }
            if (mode == "Polygon" || mode == "Bezier" || mode == "Bspline")
            {
                if (currPrimitive != null)
                {
                    Console.WriteLine("Add to polygon");
                    currPolygon.Add(currPrimitive);
                    currPrimitive = null;
                    return;
                }
            }
            else if (mode == "Trimming")
            {
                if (currPrimitive != null)
                {
                    // 裁剪所用的矩形的大小
                    var x = ((DashedRectangle)currPrimitive).x;
                    var y = ((DashedRectangle)currPrimitive).y;
                    var width = ((DashedRectangle)currPrimitive).width;
                    var height = ((DashedRectangle)currPrimitive).height;
                    Rectangle rect = new Rectangle(x, y, width, height);
                    // 将直线和多边形都裁剪一遍
                    foreach (var g in graphics)
                        if (g.graphicType == "Line" || g.graphicType == "Polygon")
                        {
                            g.UnDraw(defaultColor);
                            g.Trim(rect);
                            g.Draw(defaultColor);
                        }
                    // 将裁剪所用的矩形框给消去
                    currPrimitive.UnDraw(drawingColor);
                    currPrimitive = null;
                }
            }
            else if ((mode == "Translation" || mode == "Rotate" || mode == "Scale") && selected != null)
            {
                // 换回原来的颜色
                firstPoint.X = -1;
                firstPoint.Y = -1;
                prevPoint.X = -1;
                prevPoint.Y = -1;
                selected.UnDraw(selectionColor);
                selected.Draw(defaultColor);
                selected.isSelected = false;
                selected = null;
                totalScaleFactor = 1.0;
            }
            
            UpdateScreen();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            Point point = PointToClient(Cursor.Position);
            // Console.WriteLine("MouseMove");
            // 先把当前图形消去
            if (currPrimitive != null)
                currPrimitive.UnDraw(drawingColor);

            // 将用户正在画的图形显示出来
            if (isMouseDown)
            {
                // 在MouseDown的时候，我们要画的图形有直线、圆、椭圆、裁剪框、平移、旋转、缩放变换
                if (mode == "DDA")
                {
                    // 先把当前这条线给去掉
                    currPrimitive = new DDALine(firstPoint, point, this);
                }
                else if (mode == "Bresenham")
                {
                    currPrimitive = new BresenhamLine(firstPoint, point, this);
                }
                else if (mode == "Circle")
                {
                    int dx = point.X - firstPoint.X;
                    int dy = point.Y - firstPoint.Y;
                    int distance = (int)Math.Sqrt(dx * dx + dy * dy);
                    currPrimitive = new Circle(firstPoint, distance, this);
                }
                else if (mode == "Ellipse")
                {
                    // 第一个点和第二个点构成一个矩形，椭圆是这个矩形的内切椭圆
                    var center = new Point((firstPoint.X + point.X) / 2, (firstPoint.Y + point.Y) / 2);
                    // x轴长度
                    var x = Math.Abs((firstPoint.X - point.X) / 2);
                    // y轴长度
                    var y = Math.Abs((firstPoint.Y - point.Y) / 2);
                    currPrimitive = new Ellipse(center, x, y, this);
                }
                else if (mode == "Trimming")
                {
                    // 裁剪所用的矩形的大小
                    var width = Math.Abs(firstPoint.X - point.X);
                    var height = Math.Abs(firstPoint.Y - point.Y);
                    var minX = Math.Min(firstPoint.X, point.X);
                    var minY = Math.Min(firstPoint.Y, point.Y);
                    currPrimitive = new DashedRectangle(minX, minY, width, height, this);
                }
                else if (mode == "Translation")
                {
                    if (selected != null)
                    {
                        var dx = point.X - firstPoint.X;
                        var dy = point.Y - firstPoint.Y;
                        // 我们平移的量是这个时钟周期内的鼠标的移动量
                        // 这一次移动好之后，图形就到新位置了
                        // 下一次当然要从那个位置开始重新移动
                        firstPoint = point;
                        selected.UnDraw(selectionColor);
                        selected.Translation(dx, dy);
                        selected.Draw(selectionColor);
                        UpdateScreen();
                    }
                    return;
                }
                else if (mode == "Rotate")
                {
                    if (selected != null)
                    {
                        // 我们是如何计算旋转角度的呢？
                        // 鼠标最开始的位置为a
                        // 之后先后记录鼠标位置，连续两次次所在点分别为b, c
                        // 则角度为ac与ab的夹角
                        if (prevPoint.X != -1)
                        {
                            // 两个向量p1与p2
                            // p1即ab(firstPoint->prevPoint)
                            // p2即ac(firstPoint->point)
                            var p1X = prevPoint.X - firstPoint.X;
                            var p1Y = prevPoint.Y - firstPoint.Y;
                            var p2X = point.X - firstPoint.X;
                            var p2Y = point.Y - firstPoint.Y;
                            double dotProduct = p1X * p2X + p1Y * p2Y;
                            // 计算夹角的cos
                            double cos = dotProduct /
                                (Math.Sqrt(p1X * p1X + p1Y * p1Y) * Math.Sqrt(p2X * p2X + p2Y * p2Y));

                            // 如何确定从p1到p2旋转角是正还是负呢？
                            // 来自博客：http://www.cnblogs.com/lancidie/archive/2010/05/05/1728122.html
                            // 关于叉乘符号与向量的角度方向关系，请参考《算法导论》，我只给出结论:
                            // p1* p2 = x1y2 - x2y1 = -p2 * p1
                            // If p1 *p2 is positive, 
                            // then p1 is clockwise from p2 with respect to the origin(0, 0); 
                            // if this cross product is negative, 
                            // then p1 is counterclockwise from p2.
                            var crossProduct = p1X * p2Y - p2X * p1Y;

                            double sin = Math.Sqrt(1 - cos * cos);
                            if (crossProduct < 0)
                                sin = -sin;
                            if (!Double.IsNaN(sin) && !Double.IsNaN(cos))
                            {
                                selected.UnDraw(selectionColor);
                                selected.Rotate(firstPoint, sin, cos);
                                selected.Draw(selectionColor);
                                UpdateScreen();
                            }
                        }
                        prevPoint = point;
                        return;
                    }

                }
                else if (mode == "Scale")
                {
                    if (selected != null)
                    {
                        var dx = point.X - firstPoint.X;
                        var dy = point.Y - firstPoint.Y;

                        // scaleFactor即我们这一次应该在上一次的基础上再scale多少
                        // scaleFactor的度量是以当前所在点到初始点的距离 / 10.0
                        double scaleFactor = Math.Sqrt(dx * dx + dy * dy) / 10.0;
                        // 由于我们之前已经缩放的比例是totalScaleFactor
                        // 所以在此基础上，我们只要再缩放这么多
                        scaleFactor /= totalScaleFactor;
                        if (!Double.IsNaN(scaleFactor) && !Double.IsInfinity(scaleFactor))
                        {
                            selected.UnDraw(selectionColor);
                            selected.Scale(firstPoint, scaleFactor);
                            totalScaleFactor *= scaleFactor;
                            selected.Draw(selectionColor);
                            UpdateScreen();
                        }
                        return;
                    }
                }
                else
                    return;
            }
            else
            {
                // 在MouseUp时候的Move，我们是在画多边形/样条曲线的控制边
                // 我们画的是当前还在动的那条边
                if ((mode == "Polygon" || mode == "Bezier" || mode == "Bspline") && firstPoint.X != -1)
                    currPrimitive = new DDALine(firstPoint, point, this);
                else
                    return;
            }
            // 图形更新完成
            // 重新画出来
            currPrimitive.Draw(drawingColor);
            UpdateScreen();
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            var point = PointToClient(Cursor.Position);
            Console.WriteLine("MouseClick");
            if (mode == "Fill")
            {
                var block = new Block(point, this);
                // 立即画出来
                block.Draw(fillColor);
                graphics.Add(block);
            }
            else if (mode == "Polygon" || mode == "Bezier" || mode == "Bspline")
            {
                // 将当前点加入到多边形的顶点序列中
                vertices.Add(point);
            }
            else
            {

            }
        }

        private void Form1_DoubleClick(object sender, EventArgs e)
        {
            Console.WriteLine("DoubleClick");
            if (mode == "Polygon")
            {
                // 将已经画好的多边形边框给去掉
                foreach (var l in currPolygon)
                    l.UnDraw(drawingColor);
                currPolygon.Clear();
                // 画多边形是以双击表示结束的
                // 首先先将当前点加入到多边形的顶点序列中，作为最后一个点
                var point = PointToClient(Cursor.Position);
                vertices.Add(point);
                var tmp = new Polygon(vertices, this);
                tmp.Draw(defaultColor);
                graphics.Add(tmp);
                vertices.Clear();
                UpdateScreen();
                Console.WriteLine("**********************");
            }
            else if (mode == "Bezier")
            {
                foreach (var l in currPolygon)
                    l.UnDraw(drawingColor);
                currPolygon.Clear();
                // 画多边形是以双击表示结束的
                // 首先先将当前点加入到多边形的顶点序列中，作为最后一个点
                var point = PointToClient(Cursor.Position);
                vertices.Add(point);
                var tmp = new Bezier(vertices, this);
                tmp.Draw(defaultColor);
                graphics.Add(tmp);
                vertices.Clear();
                UpdateScreen();
            }
            else if (mode == "Bspline")
            {
                foreach (var l in currPolygon)
                    l.UnDraw(drawingColor);
                currPolygon.Clear();
                var point = PointToClient(Cursor.Position);
                vertices.Add(point);
                var tmp = new Bspline(vertices, this);
                graphics.Add(tmp);
                tmp.Draw(defaultColor);
                vertices.Clear();
                UpdateScreen();
            }
            // use this to invalidate firstPoint
            firstPoint.X = -1;
        }

        // 保存图片
        private void buttonSave_Click(object sender, EventArgs e)
        {
            isMouseDown = false;
            ClearPointerSelection();
            // When user clicks button, show the dialog.
            saveFileDialog1.ShowDialog();
            InitDraw();
        }

        // 清屏
        private void buttonClearing_Click(object sender, EventArgs e)
        {
            Init();
            mode = "Clearing";
            ResetScreenBuffer();
            // 在进行refresh的时候，屏幕就被清空了
            Refresh();
            InitDraw();
        }

        // DDA
        private void buttonDDA_Click(object sender, EventArgs e)
        {
            isMouseDown = false;
            ClearPointerSelection();
            mode = "DDA";
            InitDraw();
        }

        // Bresenham
        private void buttonBresenham_Click(object sender, EventArgs e)
        {
            isMouseDown = false;
            ClearPointerSelection();
            mode = "Bresenham";
            InitDraw();
        }

        // 圆
        private void buttonCircle_Click(object sender, EventArgs e)
        {
            isMouseDown = false;
            ClearPointerSelection();
            mode = "Circle";
            InitDraw();
        }

        // 椭圆
        private void buttonEllipse_Click(object sender, EventArgs e)
        {
            isMouseDown = false;
            ClearPointerSelection();
            mode = "Ellipse";
            InitDraw();
        }
        
        // 多边形
        private void buttonPolygon_Click(object sender, EventArgs e)
        {
            isMouseDown = false;
            ClearPointerSelection();
            mode = "Polygon";
            InitDraw();
        }

        // Bezier曲线
        private void buttonBezier_Click(object sender, EventArgs e)
        {
            isMouseDown = false;
            ClearPointerSelection();
            mode = "Bezier";
            InitDraw();
        }

        // B样条曲线
        private void buttonBspline_Click(object sender, EventArgs e)
        {
            isMouseDown = false;
            ClearPointerSelection();
            mode = "Bspline";
            InitDraw();
        }

        // 裁剪
        private void buttonTrimming_Click(object sender, EventArgs e)
        {
            isMouseDown = false;
            ClearPointerSelection();
            mode = "Trimming";
            InitDraw();
        }
       
        // 填充
        private void buttonFill_Click(object sender, EventArgs e)
        {
            isMouseDown = false;
            ClearPointerSelection();
            mode = "Fill";
            InitDraw();
        }

        // 平移
        private void buttonTranslation_Click(object sender, EventArgs e)
        {
            isMouseDown = false;
            ClearPointerSelection();
            mode = "Translation";
            InitDraw();
        }
        
        // 旋转
        private void buttonRotate_Click(object sender, EventArgs e)
        {
            isMouseDown = false;
            ClearPointerSelection();
            mode = "Rotate";
            InitDraw();
        }

        // 缩放
        private void buttonScale_Click(object sender, EventArgs e)
        {
            isMouseDown = false;
            ClearPointerSelection();
            mode = "Scale";
            InitDraw();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Wpf3D.MainWindow wpfwindow = new Wpf3D.MainWindow();
            wpfwindow.Show();
        }

        // 清空screenBuffer
        private void ResetScreenBuffer()
        {
            for (var i = 0; i < form_width; i++)
                for (var j = 0; j < form_height; j++)
                    for (var k = 0; k < numColor; k++)
                        screenBuffer[i, j, k] = 0;
        }

        public bool IsEmpty(int x, int y)
        {
            for (var i = 0; i < numColor; i++)
                if (screenBuffer[x, y, i] > 0)
                    return true;
            return false;
        }

        // 更新我们自己维护的屏幕上的一系列像素
        public void UpdateMyCanvas(List<Point> l)
        {
            foreach (var p in l)
            {
                var x = p.X;
                var y = p.Y;
                if (CheckOnCanvas(x, y))
                {
                    Rectangle rect = new Rectangle(x, y, 1, 1);
                    var color = backgroundColor;
                    // drawingColor
                    if (screenBuffer[x, y, 1] > 0)
                        color = colors[1];
                    // selectionColor
                    else if (screenBuffer[x, y, 2] > 0)
                        color = colors[2];
                    // fillColor
                    else if (screenBuffer[x, y, 3] > 0)
                        color = colors[3];
                    else if (screenBuffer[x, y, 0] > 0)
                        color = colors[0];
                    myCanvas.SetPixel(x, y - 40, color);
                }
            }
        }

        public void UpdateScreen()
        {
            Graphics graphics = CreateGraphics();
            graphics.DrawImage(myCanvas, new Rectangle(0, 40, form_width, form_height - 40));
        }

        // 检查这个点是否在画布上
        // 画布的范围是不包括button的空白位置
        public bool CheckOnCanvas(int x, int y)
        {
            return x >= 0 && x < form_width && y >= 40 && y < form_height;
        }

        // 首先先清除上一次选中的图形的信息
        private void ClearPointerSelection()
        {
            selected = null;
            foreach (var g in graphics)
                g.isSelected = false;
        }

        private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Get file name.
            var name = saveFileDialog1.FileName;

            var format = ImageFormat.Jpeg;

            switch (saveFileDialog1.FilterIndex)
            {
                case 1:
                    format = ImageFormat.Jpeg;
                    break;

                case 2:
                    format = ImageFormat.Bmp;
                    break;

                case 3:
                    format = ImageFormat.Png;
                    break;
            }
            if (name != "")
            {
                // 我们截屏不要把第一行的button给截进去
                using (var bmp = new Bitmap(form_width, form_height))
                {
                    DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                    bmp.Save(name, format);
                }
            }
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {

        }

      
    }
}
