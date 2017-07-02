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

        string mode = "Bresenham";

        private void Init()
        {
            myCanvas = new Bitmap(form_width, form_height - 40);
            graphics.Clear();
            InitHandlers();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Init();
        }


        // 当窗口被阻挡后重新显示时，我们希望窗口里的内容不要消失
        // 因此我们必须要再这个Paint里重绘
        // 我们重绘就是将我们维护的buffer中的内容重新显示到屏幕上
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // 值得注意的是，再Form第一次load时
            // 没必要重绘，因为此时窗口啥都没有
            // 如果进行重绘，反而会明显拖慢控件的绘制速度
            // 使得界面加载变卡
            UpdateScreen();
        }
       

        #region Line/Circle/Ellipse
        // 直线/圆/椭圆的绘制操作都一样，所以只要用相同的Mouse Event Handler

        bool Line_isMouseDown = false;
        Point Line_firstPoint;
        Primitive currLine = null;

        // 初始化绘制直线所需的环境
        // 环境包括变量及Mouse Event Handler
        private void InitLine()
        {
            Line_isMouseDown = false;
            currLine = null;
            SetMouseUpEventHandler(new MouseEventHandler(Line_MouseUp));
            SetMouseDownEventHandler(new MouseEventHandler(Line_MouseDown));
            SetMouseMoveEventHandler(new MouseEventHandler(Line_MouseMove));
            SetMouseClickEventHandler(new MouseEventHandler(Line_MouseClick));
            SetMouseDoubleClickEventHandler(new MouseEventHandler(Line_MouseDoubleClick));
        }

        private void Line_MouseDown(object sender, MouseEventArgs e)
        {
            // 我们在鼠标左键按下时，记录初始位置
            if (e.Button == MouseButtons.Left)
            {
                Line_isMouseDown = true;
                Line_firstPoint = PointToClient(Cursor.Position);
            }
        }

        private void Line_MouseUp(object sender, MouseEventArgs e)
        {
            // 鼠标左键抬起时，即完成绘图
            if (e.Button == MouseButtons.Left)
            {
                // 值得注意的是，我们实际的构造currLine以及绘制是再Line_MouseMove中完成的
                // 如果只是按下松开鼠标左键，而没有移动鼠标，则currLine仍为null
                // 直线已经画完了，我们把它从蓝色变成红色
                if (currLine != null)
                {
                    currLine.UnDraw(drawingColor);
                    currLine.Draw(defaultColor);
                    graphics.Add(currLine);
                    UpdateScreen();
                }
                // 将相关变量重新设为初始值，准备下一次画线
                Line_isMouseDown = false;
                currLine = null;
            }
        }

        private void Line_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && Line_isMouseDown)
            {
                Point point = PointToClient(Cursor.Position);
                // 先把当前图形消去
                if (currLine != null)
                    currLine.UnDraw(drawingColor);

                // 将用户正在画的直线显示出来
                if (mode == "DDA")
                {
                    currLine = new DDALine(Line_firstPoint, point, this);
                }
                else if (mode == "Bresenham")
                {
                    currLine = new BresenhamLine(Line_firstPoint, point, this);
                }
                else if (mode == "Circle")
                {
                    int dx = point.X - Line_firstPoint.X;
                    int dy = point.Y - Line_firstPoint.Y;
                    int distance = (int)Math.Sqrt(dx * dx + dy * dy);
                    currLine = new Circle(Line_firstPoint, distance, this);
                }
                else if (mode == "Ellipse")
                {
                    // 第一个点和第二个点构成一个矩形，椭圆是这个矩形的内切椭圆
                    var center = new Point((Line_firstPoint.X + point.X) / 2, (Line_firstPoint.Y + point.Y) / 2);
                    // x轴长度
                    var x = Math.Abs((Line_firstPoint.X - point.X) / 2);
                    // y轴长度
                    var y = Math.Abs((Line_firstPoint.Y - point.Y) / 2);
                    currLine = new Ellipse(center, x, y, this);
                }
                else
                {
                    Console.WriteLine("Should not reach here");
                }
                currLine.Draw(drawingColor);
                UpdateScreen();
            }
        }

        private void Line_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void Line_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        private void buttonDDA_Click(object sender, EventArgs e)
        {
            mode = "DDA";
            InitLine();
        }

        // Bresenham
        private void buttonBresenham_Click(object sender, EventArgs e)
        {
            mode = "Bresenham";
            InitLine();
        }

        // 圆
        private void buttonCircle_Click(object sender, EventArgs e)
        {
            mode = "Circle";
            InitLine();
        }

        // 椭圆
        private void buttonEllipse_Click(object sender, EventArgs e)
        {
            mode = "Ellipse";
            InitLine();
        }
        #endregion

        #region Polygon/Bezier/Bspline

        // 多边形及样条曲线的绘制操作类似，都需要连续画多个点
        // 绘制方式，单击绘制每一个点，再最后一个点双击结束

        // 表示是否已经单击定下了多边形的第一个点
        bool Polygon_isClicked = false;
        // 绘制时的上一个顶点
        Point Polygon_prevPoint;
        // 当前正在绘制的边
        Primitive Polygon_currEdge = null;
        // 多边形的顶点
        List<Point> Polygon_vertices = new List<Point>();
        // 多边形的边
        List<Primitive> Polygon_edges = new List<Primitive>();

        // 初始化绘制直线所需的环境
        // 环境包括变量及Mouse Event Handler
        private void InitPolygon()
        {
            Polygon_isClicked = false;
            Polygon_currEdge = null;
            Polygon_vertices.Clear();
            Polygon_edges.Clear();
            SetMouseUpEventHandler(new MouseEventHandler(Polygon_MouseUp));
            SetMouseDownEventHandler(new MouseEventHandler(Polygon_MouseDown));
            SetMouseMoveEventHandler(new MouseEventHandler(Polygon_MouseMove));
            SetMouseClickEventHandler(new MouseEventHandler(Polygon_MouseClick));
            SetMouseDoubleClickEventHandler(new MouseEventHandler(Polygon_MouseDoubleClick));
        }

        private void Polygon_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void Polygon_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void Polygon_MouseMove(object sender, MouseEventArgs e)
        {
            // 我们正在拖动鼠标，画多边形的一条线
            if (Polygon_isClicked)
            {
                Point point = PointToClient(Cursor.Position);
                // 先把当前这条线消去
                if (Polygon_currEdge != null)
                    Polygon_currEdge.UnDraw(drawingColor);
                Polygon_currEdge = new DDALine(Polygon_prevPoint, point, this);
                Polygon_currEdge.Draw(drawingColor);
                UpdateScreen();
            }
        }

        private void Polygon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point point = PointToClient(Cursor.Position);
                Polygon_prevPoint = point;
                if (!Polygon_isClicked)
                {
                    // 这是多边形的第一个顶点
                    Polygon_isClicked = true;
                    // 第一个点总归是要加进去的
                    Polygon_vertices.Add(point);
                }
                else
                {
                    if (Polygon_currEdge != null)
                    {
                        // 因为Polygon_currEdge对象的实际构造是在MouseMove handler中完成的
                        // Polygon_currEdge不为null，说明鼠标在在两次单击期间移动了位置
                        // 也只有此时才需要将点加进去
                        Polygon_vertices.Add(point);
                        // 将当前的边加入到多边形的边集合中
                        Polygon_edges.Add(Polygon_currEdge);
                        // 重新开始绘制边
                        Polygon_currEdge = null;
                    }
                }
            }
        }

        private void Polygon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // 在这里加上判断，保证用户直接双击不会对我们造成影响
            if (e.Button == MouseButtons.Left && Polygon_vertices.Count > 0 && Polygon_edges.Count > 0)
            {
                // 将已经画好的多边形边框给去掉
                foreach (var l in Polygon_edges)
                    l.UnDraw(drawingColor);
                Primitive tmp;
                if (mode == "Polygon")
                    tmp = new Polygon(Polygon_vertices, this);
                else if (mode == "Bezier")
                    tmp = new Bezier(Polygon_vertices, this);
                else
                    tmp = new Bspline(Polygon_vertices, this);
                tmp.Draw(defaultColor);
                graphics.Add(tmp);
                UpdateScreen();
            }
            Polygon_isClicked = false;
            Polygon_currEdge = null;
            Polygon_vertices.Clear();
            Polygon_edges.Clear();
        }

        // 多边形
        private void buttonPolygon_Click(object sender, EventArgs e)
        {
            mode = "Polygon";
            InitPolygon();
        }

        // Bezier曲线
        private void buttonBezier_Click(object sender, EventArgs e)
        {
            mode = "Bezier";
            InitPolygon();
        }

        // B样条曲线
        private void buttonBspline_Click(object sender, EventArgs e)
        {
            mode = "Bspline";
            InitPolygon();
        }
        #endregion

        #region Transformation:Translate/Rotate/Scale
        // 基本变换操作，操作动作都差不多
        // 都是按下鼠标左键选中图形，然后再拖动鼠标，图形随之改变
        // 松开鼠标左键，操作就完成了
        bool Transformation_isMouseDown = false;
        Point Transformation_firstPoint, Transformation_prevPoint;
        double Transformation_totalScaleFactor = 1.0;
        // 被选中的图形
        Primitive Transformation_selected;

        private void InitTransformation()
        {
            Transformation_isMouseDown = false;
            Transformation_selected = null;
            Transformation_firstPoint.X = -1;
            Transformation_firstPoint.Y = -1;
            Transformation_prevPoint.X = -1;
            Transformation_prevPoint.Y = -1;
            Transformation_totalScaleFactor = 1.0;
            SetMouseUpEventHandler(new MouseEventHandler(Transformation_MouseUp));
            SetMouseDownEventHandler(new MouseEventHandler(Transformation_MouseDown));
            SetMouseMoveEventHandler(new MouseEventHandler(Transformation_MouseMove));
            SetMouseClickEventHandler(new MouseEventHandler(Transformation_MouseClick));
            SetMouseDoubleClickEventHandler(new MouseEventHandler(Transformation_MouseDoubleClick));
        }

        private void Transformation_MouseDown(object sender, MouseEventArgs e)
        {
            // 我们在鼠标左键按下时，记录初始位置
            if (e.Button == MouseButtons.Left)
            {
                Transformation_isMouseDown = true;
                Point point = PointToClient(Cursor.Position);
                Transformation_firstPoint = point;
                // 这个三个操作需要选中图形
                // 距离鼠标位置距离小于3的图形被选中
                foreach (var g in graphics)
                {
                    if (g.Distance(point) <= 3.0)
                    {
                        g.isSelected = true;
                        Transformation_selected = g;
                        // 被选中的图形要变色
                        // 因此我们首先把它现在的颜色给去除掉
                        // 然后把它拿新颜色重绘一遍
                        Transformation_selected.UnDraw(defaultColor);
                        Transformation_selected.Draw(selectionColor);
                        UpdateScreen();
                        return;
                    }
                }
            }
        }

        private void Transformation_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Transformation_selected != null)
                {
                    // 变换完成后，我们将其换成原来的颜色
                    Transformation_selected.UnDraw(selectionColor);
                    Transformation_selected.Draw(defaultColor);
                    Transformation_selected.isSelected = false;
                    UpdateScreen();
                }
                // 重新初始化相关变量，以为下一次操作做好准备
                Transformation_isMouseDown = false;
                Transformation_selected = null;
                Transformation_firstPoint.X = -1;
                Transformation_firstPoint.Y = -1;
                Transformation_prevPoint.X = -1;
                Transformation_prevPoint.Y = -1;
                Transformation_totalScaleFactor = 1.0;
            }
        }

        private void Transformation_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && Transformation_isMouseDown
                && Transformation_selected != null)
            {
                Point point = PointToClient(Cursor.Position);
                #region Translation
                if (mode == "Translation")
                {
                    var dx = point.X - Transformation_firstPoint.X;
                    var dy = point.Y - Transformation_firstPoint.Y;
                    // 我们平移的量是两次MouseMove Event之间的鼠标的移动量
                    // 这一次移动好之后，图形就到新位置了
                    // 下一次移动的向量当然要从那个位置开始重新移动
                    Transformation_firstPoint = point;
                    Transformation_selected.UnDraw(selectionColor);
                    Transformation_selected.Translation(dx, dy);
                    Transformation_selected.Draw(selectionColor);
                    UpdateScreen();
                }
                #endregion
                #region Rotate
                else if (mode == "Rotate")
                {
                    // 我们是如何计算旋转角度的呢？
                    // 鼠标最开始的位置为a
                    // 之后先后记录鼠标位置，连续两次次所在点分别为b, c
                    // 则角度为ac与ab的夹角
                    if (Transformation_prevPoint.X != -1)
                    {
                        // 两个向量p1与p2
                        // p1即ab(firstPoint->prevPoint)
                        // p2即ac(firstPoint->point)
                        var p1X = Transformation_prevPoint.X - Transformation_firstPoint.X;
                        var p1Y = Transformation_prevPoint.Y - Transformation_firstPoint.Y;
                        var p2X = point.X - Transformation_firstPoint.X;
                        var p2Y = point.Y - Transformation_firstPoint.Y;
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
                            Transformation_selected.UnDraw(selectionColor);
                            Transformation_selected.Rotate(Transformation_firstPoint, sin, cos);
                            Transformation_selected.Draw(selectionColor);
                            UpdateScreen();
                        }
                    }
                    Transformation_prevPoint = point;
                }
                #endregion
                #region Scale
                else if (mode == "Scale")
                {
                    var dx = point.X - Transformation_firstPoint.X;
                    var dy = point.Y - Transformation_firstPoint.Y;

                    // scaleFactor即我们这一次应该在上一次的基础上再scale多少
                    // scaleFactor的度量是以当前所在点到初始点的距离 / 10.0
                    double scaleFactor = Math.Sqrt(dx * dx + dy * dy) / 10.0;
                    // 由于我们之前已经缩放的比例是totalScaleFactor
                    // 所以在此基础上，我们只要再缩放这么多
                    scaleFactor /= Transformation_totalScaleFactor;
                    if (!Double.IsNaN(scaleFactor) && !Double.IsInfinity(scaleFactor))
                    {
                        Transformation_selected.UnDraw(selectionColor);
                        Transformation_selected.Scale(Transformation_firstPoint, scaleFactor);
                        Transformation_totalScaleFactor *= scaleFactor;
                        Transformation_selected.Draw(selectionColor);
                        UpdateScreen();
                    }
                }
                #endregion
            }
        }

        private void Transformation_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void Transformation_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        // 平移
        private void buttonTranslation_Click(object sender, EventArgs e)
        {
            mode = "Translation";
            InitTransformation();
        }

        // 旋转
        private void buttonRotate_Click(object sender, EventArgs e)
        {
            mode = "Rotate";
            InitTransformation();
        }

        // 缩放
        private void buttonScale_Click(object sender, EventArgs e)
        {
            mode = "Scale";
            InitTransformation();
        }
        #endregion

        #region Fill

        // 填充的操作模式很简单，单击确定一个点/图形
        // 并从那个点开始泛滥填充或对那个多边形进行扫描线填充

        private void InitFill()
        {
            SetMouseUpEventHandler(new MouseEventHandler(Fill_MouseUp));
            SetMouseDownEventHandler(new MouseEventHandler(Fill_MouseDown));
            SetMouseMoveEventHandler(new MouseEventHandler(Fill_MouseMove));
            SetMouseClickEventHandler(new MouseEventHandler(Fill_MouseClick));
            SetMouseDoubleClickEventHandler(new MouseEventHandler(Fill_MouseDoubleClick));
        }

        private void Fill_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void Fill_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void Fill_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void Fill_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var point = PointToClient(Cursor.Position);
                var block = new Block(point, this);
                graphics.Add(block);
                // 立即画出来
                block.Draw(fillColor);
                UpdateScreen();
            }
        }

        private void Fill_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        // 填充
        private void buttonFill_Click(object sender, EventArgs e)
        {
            mode = "Fill";
            InitFill();
        }
        #endregion

        #region Trimming
        // 直线/圆/椭圆的绘制操作都一样，所以只要用相同的Mouse Event Handler

        bool Trimming_isMouseDown = false;
        Point Trimming_firstPoint;
        // 用于裁剪的裁剪框
        Primitive Trimming_rectangle = null;

        // 初始化绘制直线所需的环境
        // 环境包括变量及Mouse Event Handler
        private void InitTrimming()
        {
            Trimming_isMouseDown = false;
            Trimming_rectangle = null;
            SetMouseUpEventHandler(new MouseEventHandler(Trimming_MouseUp));
            SetMouseDownEventHandler(new MouseEventHandler(Trimming_MouseDown));
            SetMouseMoveEventHandler(new MouseEventHandler(Trimming_MouseMove));
            SetMouseClickEventHandler(new MouseEventHandler(Trimming_MouseClick));
            SetMouseDoubleClickEventHandler(new MouseEventHandler(Trimming_MouseDoubleClick));
        }

        private void Trimming_MouseDown(object sender, MouseEventArgs e)
        {
            // 我们在鼠标左键按下时，记录初始位置
            if (e.Button == MouseButtons.Left)
            {
                Trimming_isMouseDown = true;
                Trimming_firstPoint = PointToClient(Cursor.Position);
            }
        }

        private void Trimming_MouseUp(object sender, MouseEventArgs e)
        {
            // 鼠标左键抬起时，即完成绘图
            if (e.Button == MouseButtons.Left && Trimming_rectangle != null)
            {
                // 将裁剪所用的矩形框给消去
                Trimming_rectangle.UnDraw(drawingColor);
                // 裁剪所用的矩形的大小
                var x = ((DashedRectangle)Trimming_rectangle).x;
                var y = ((DashedRectangle)Trimming_rectangle).y;
                var width = ((DashedRectangle)Trimming_rectangle).width;
                var height = ((DashedRectangle)Trimming_rectangle).height;
                Rectangle rect = new Rectangle(x, y, width, height);
                // 将直线和多边形都裁剪一遍
                foreach (var g in graphics)
                    if (g.graphicType == "Line" || g.graphicType == "Polygon")
                    {
                        g.UnDraw(defaultColor);
                        g.Trim(rect);
                        g.Draw(defaultColor);
                    }
                UpdateScreen();
            }
            Trimming_isMouseDown = false;
            Trimming_rectangle = null;
        }

        private void Trimming_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && Trimming_isMouseDown)
            {
                Point point = PointToClient(Cursor.Position);

                // 先把当前的裁剪框给消去
                if (Trimming_rectangle != null)
                    Trimming_rectangle.UnDraw(drawingColor);

                // 鼠标移动之后，裁剪框发生了变化，我们需要重新绘制
                var width = Math.Abs(Trimming_firstPoint.X - point.X);
                var height = Math.Abs(Trimming_firstPoint.Y - point.Y);
                var minX = Math.Min(Trimming_firstPoint.X, point.X);
                var minY = Math.Min(Trimming_firstPoint.Y, point.Y);
                Trimming_rectangle = new DashedRectangle(minX, minY, width, height, this);
                Trimming_rectangle.Draw(drawingColor);
                UpdateScreen();
            }
        }

        private void Trimming_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void Trimming_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        // 裁剪
        private void buttonTrimming_Click(object sender, EventArgs e)
        {
            mode = "Trimming";
            InitTrimming();
        }
        #endregion

        #region Save/Clear/WPF3D
        // 这些都使用DefaultMouseHandler
        // 保存图片
        private void buttonSave_Click(object sender, EventArgs e)
        {
            mode = "Saving";
            UseDefaultMouseHandler();
            // When user clicks button, show the dialog.
            saveFileDialog1.ShowDialog();
        }

        // 清屏
        private void buttonClearing_Click(object sender, EventArgs e)
        {
            mode = "Clearing";
            UseDefaultMouseHandler();
            Init();
            ResetScreenBuffer();
            // 在进行refresh的时候，屏幕就被清空了
            Refresh();
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

        // WPF3D
        private void buttonWPF3D_Click(object sender, EventArgs e)
        {
            UseDefaultMouseHandler();
            Wpf3D.MainWindow wpfwindow = new Wpf3D.MainWindow();
            wpfwindow.Show();
        }

        #endregion

        #region ScreenBuffer
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
        #endregion

        #region  Set Mouse Event Handler Wrapper

        // 默认的MouseHandler
        private void DefaultMouseEventHandler(object sender, EventArgs e)
        {
            // do nothing
        }

        // 当前的MouseEvent
        MouseEventHandler currMouseUpEventHandler = null;
        MouseEventHandler currMouseDownEventHandler = null;
        MouseEventHandler currMouseMoveEventHandler = null;
        MouseEventHandler currMouseClickEventHandler = null;
        MouseEventHandler currMouseDoubleClickEventHandler = null;

        private void InitHandlers()
        {
            currMouseUpEventHandler = new MouseEventHandler(Line_MouseUp);
            currMouseDownEventHandler = new MouseEventHandler(Line_MouseDown);
            currMouseMoveEventHandler = new MouseEventHandler(Line_MouseMove);
            currMouseClickEventHandler = new MouseEventHandler(Line_MouseClick);
            currMouseDoubleClickEventHandler = new MouseEventHandler(Line_MouseDoubleClick);
        }

        private void SetMouseUpEventHandler(MouseEventHandler handler)
        {
            // unregister the old handler
            MouseUp -= currMouseUpEventHandler;
            // register the new handler
            MouseUp += handler;
            currMouseUpEventHandler = handler;
        }

        private void SetMouseDownEventHandler(MouseEventHandler handler)
        {
            // unregister the old handler
            MouseDown -= currMouseDownEventHandler;
            // register the new handler
            MouseDown += handler;
            currMouseDownEventHandler = handler;
        }

        private void SetMouseMoveEventHandler(MouseEventHandler handler)
        {
            // unregister the old handler
            MouseMove -= currMouseMoveEventHandler;
            // register the new handler
            MouseMove += handler;
            currMouseMoveEventHandler = handler;
        }

        private void SetMouseClickEventHandler(MouseEventHandler handler)
        {
            // unregister the old handler
            MouseClick -= currMouseClickEventHandler;
            // register the new handler
            MouseClick += handler;
            currMouseClickEventHandler = handler;
        }

        private void SetMouseDoubleClickEventHandler(MouseEventHandler handler)
        {
            // unregister the old handler
            MouseDoubleClick -= currMouseDoubleClickEventHandler;
            // register the new handler
            MouseDoubleClick += handler;
            currMouseDoubleClickEventHandler = handler;
        }

        // 将所有的MouseEvent Handler设置为Default
        private void UseDefaultMouseHandler()
        {
            SetMouseUpEventHandler(new MouseEventHandler(DefaultMouseEventHandler));
            SetMouseDownEventHandler(new MouseEventHandler(DefaultMouseEventHandler));
            SetMouseMoveEventHandler(new MouseEventHandler(DefaultMouseEventHandler));
            SetMouseClickEventHandler(new MouseEventHandler(DefaultMouseEventHandler));
            SetMouseDoubleClickEventHandler(new MouseEventHandler(DefaultMouseEventHandler));
        }
        #endregion
    }
}
