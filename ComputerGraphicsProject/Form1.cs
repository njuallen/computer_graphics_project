// #define MYDEBUG


using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace ComputerGraphicsProject
{
    public partial class 画图 : Form
    {
        public 画图()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DoubleBuffered = true;
            for (var i = 0; i < form_width; i++)
            {
                var l = new List<bool>();
                for (var j = 0; j < form_height; j++)
                    l.Add(false);
                flag.Add(l);
            }
        }
    
        static int form_width = 800;
        static int form_height = 600;

        List<DDALine> DDALines = new List<DDALine>();
        List<BresenhamLine> BresenhamLines = new List<BresenhamLine>();
        List<Circle> circles = new List<Circle>();
        List<Ellipse> ellipses = new List<Ellipse>();
        List<Polygon> polygons = new List<Polygon>();
        // 填充的块
        List<Block> fill = new List<Block>();
        List<Bezier> beziers = new List<Bezier>();
        List<Bspline> bsplines = new List<Bspline>();

        List<Point> vertices = new List<Point>();

        bool isMouseDown = false;
        Point firstPoint;
        Point prevPoint;

        // 被选中的图形
        Primitive selected = null;

        string mode = "Bresenham";

        // 这个flag标识着屏幕上的某个位置是否有像素点
        // 屏幕大小为form_width * form_height
        static public List<List<bool>> flag = new List<List<bool>>();
        
        // we use Red to draw
        Pen drawingPen = Pens.Red;
        // the graph being drawn is in blue
        Pen currPen = Pens.Blue;
        // the selected graph is in Yellow
        Pen selectionPen = Pens.Yellow;
        // 这是用来进行区域填充的颜色
        Pen fillPen = Pens.Black;

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            ClearPointerSelection();
            var point = PointToClient(Cursor.Position);
            
            if (mode == "Fill")
            {
                var block = new Block(point);
                fill.Add(block);
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

        // 这个变量记录的是指定图形累计被scale了多少
        double totalScaleFactor = 1.0;

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
#if MYDEBUG
            Console.WriteLine("********************");
            var tmp = new BresenhamLine(new Point(455, 228), new Point(366, 259));
            tmp.Draw(e, currPen);
            timer1.Enabled = false;
            return;
#endif

            // 由于要进行重绘，所以屏幕上的点要全部被清空
            // 每一次都进行重新绘制，确实比较慢
            // 尤其是色块，涉及到的点特别多，也就尤其地慢
            // 但我感觉这样就行了，如果要搞增量绘制的话，每次只绘制改变的像素点
            // 就会遇到一个比较麻烦的问题：
            // 图形会重叠，如果这时我把上面的图形给移动了
            // 则颜色要变成下面的图形的颜色
            // 即必须要引入图层
            // 这个就太复杂了，我不准备搞了
            for (var i = 0; i < form_width; i++)
                for (var j = 0; j < form_height; j++)
                    flag[i][j] = false;

            var len = 0;
            // show the one that the user is drawing
            // 如果是画多边形，我们就要先把顶点顺序连接
            // 如果是画样条曲线，我们就要先把控制顶点顺序连接
            var point = PointToClient(Cursor.Position);
            if (mode == "Polygon" || mode == "Bezier" || mode == "Bspline")
            {
                len = vertices.Count;
                for (var i = 0; i < len - 1; i++)
                {
                    var line = new BresenhamLine(vertices[i], vertices[i + 1]);
                    line.Draw(e, currPen);
                }
                if (len > 0)
                {
                    var line = new BresenhamLine(vertices[len - 1], point);
                    line.Draw(e, currPen);
                }
            }

            if (isMouseDown)
            {
                if (mode == "DDA")
                {
                    var line = new DDALine(firstPoint, point);
                    line.Draw(e, currPen);
                }
                else if (mode == "Bresenham")
                {
                    var line = new BresenhamLine(firstPoint, point);
                    line.Draw(e, currPen);
                }
                else if (mode == "Circle")
                {
                    int dx = point.X - firstPoint.X;
                    int dy = point.Y - firstPoint.Y;
                    int distance = (int)Math.Sqrt(dx * dx + dy * dy);
                    var circle = new Circle(firstPoint, distance);
                    circle.Draw(e, currPen);
                }
                else if (mode == "Ellipse")
                {
                    // 第一个点和第二个点构成一个矩形，椭圆是这个矩形的内切椭圆
                    var center = new Point((firstPoint.X + point.X) / 2, (firstPoint.Y + point.Y) / 2);
                    // x轴长度
                    var x = Math.Abs((firstPoint.X - point.X) / 2);
                    // y轴长度
                    var y = Math.Abs((firstPoint.Y - point.Y) / 2);
                    var ellipse = new Ellipse(center, x, y);
                    ellipse.Draw(e, currPen);
                }
                else if (mode == "Trimming")
                {
                    // 裁剪所用的矩形
                    var width = Math.Abs(firstPoint.X - point.X);
                    var height = Math.Abs(firstPoint.Y - point.Y);
                    var minX = Math.Min(firstPoint.X, point.X);
                    var minY = Math.Min(firstPoint.Y, point.Y);
                    Rectangle rect = new Rectangle(minX, minY, width, height);

                    // 画出裁剪所用的那个矩形框
                    Pen trimmingPen = new Pen(Color.GreenYellow, 2);
                    trimmingPen.DashStyle = DashStyle.Dash;
                    e.Graphics.DrawRectangle(trimmingPen, rect);
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
                        selected.Translation(dx, dy);
                    }
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
                                selected.Rotate(firstPoint, sin, cos);
                            }
                        }
                        prevPoint = point;
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
                            selected.Scale(firstPoint, scaleFactor);
                            totalScaleFactor *= scaleFactor;
                        }
                    }
                }
            }
            len = DDALines.Count;
            for (var i = 0; i < len; i++)
                if (DDALines[i].isSelected)
                    DDALines[i].Draw(e, selectionPen);
                else
                    DDALines[i].Draw(e, drawingPen);

            len = BresenhamLines.Count;
            for (var i = 0; i < len; i++)
                if (BresenhamLines[i].isSelected)
                    BresenhamLines[i].Draw(e, selectionPen);
                else
                    BresenhamLines[i].Draw(e, drawingPen);

            len = circles.Count;
            for (var i = 0; i < len; i++)
                if (circles[i].isSelected)
                    circles[i].Draw(e, selectionPen);
                else
                    circles[i].Draw(e, drawingPen);

            len = ellipses.Count;
            for (var i = 0; i < len; i++)
                if (ellipses[i].isSelected)
                    ellipses[i].Draw(e, selectionPen);
                else
                    ellipses[i].Draw(e, drawingPen);

            len = polygons.Count;
            for (var i = 0; i < len; i++)
                if (polygons[i].isSelected)
                    polygons[i].Draw(e, selectionPen);
                else
                    polygons[i].Draw(e, drawingPen);

            len = beziers.Count;
            for (var i = 0; i < len; i++)
                if (beziers[i].isSelected)
                    beziers[i].Draw(e, selectionPen);
                else
                    beziers[i].Draw(e, drawingPen);

            len = bsplines.Count;
            for (var i = 0; i < len; i++)
                if (bsplines[i].isSelected)
                    bsplines[i].Draw(e, selectionPen);
                else
                    bsplines[i].Draw(e, drawingPen);

            len = fill.Count;
            for (var i = 0; i < len; i++)
                fill[i].Draw(e, fillPen);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            Point point = PointToClient(Cursor.Position);
            isMouseDown = true;
            firstPoint = point;
            if(mode == "Translation" || mode == "Rotate" || mode == "Scale")
            {
                // 距离鼠标位置距离小于3的图形被选中
                foreach (var l in DDALines)
                {
                    if (l.Distance(point) <= 3.0)
                    {
                        l.isSelected = true;
                        selected = l;
                        return;
                    }
                }
                foreach (var l in BresenhamLines)
                {
                    if (l.Distance(point) <= 3.0)
                    {
                        l.isSelected = true;
                        selected = l;
                        return;
                    }
                }
                foreach (var l in circles)
                {
                    if (l.Distance(point) <= 3.0)
                    {
                        l.isSelected = true;
                        selected = l;
                        return;
                    }
                }
                foreach (var l in ellipses)
                {
                    if (l.Distance(point) <= 3.0)
                    {
                        l.isSelected = true;
                        selected = l;
                        return;
                    }
                }
                foreach (var l in polygons)
                {
                    if (l.Distance(point) <= 3.0)
                    {
                        l.isSelected = true;
                        selected = l;
                        return;
                    }
                }
                foreach (var l in beziers)
                {
                    if (l.Distance(point) <= 3.0)
                    {
                        l.isSelected = true;
                        selected = l;
                        return;
                    }
                }
                foreach (var l in bsplines)
                {
                    if (l.Distance(point) <= 3.0)
                    {
                        l.isSelected = true;
                        selected = l;
                        return;
                    }
                }
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            Point point = PointToClient(Cursor.Position);
            isMouseDown = false;
         
			if (mode == "DDA")
			{
                DDALines.Add(new DDALine(firstPoint, point));
            }
			else if (mode == "Bresenham")
			{
				BresenhamLines.Add(new BresenhamLine(firstPoint, point));
			}
			else if (mode == "Circle")
			{
				int dx = point.X - firstPoint.X;
				int dy = point.Y - firstPoint.Y;
				int distance = (int)Math.Sqrt(dx * dx + dy * dy);
				circles.Add(new Circle(firstPoint, distance));
			}
			else if(mode == "Ellipse")
			{
				// 第一个点和第二个点构成一个矩形，椭圆是这个矩形的内切椭圆
				var center = new Point((firstPoint.X + point.X) / 2, (firstPoint.Y + point.Y) / 2);
				// x轴长度
				var x = Math.Abs((firstPoint.X - point.X) / 2);
				// y轴长度
				var y = Math.Abs((firstPoint.Y - point.Y) / 2);
				ellipses.Add(new Ellipse(center, x, y));
			}
            else if(mode == "Trimming")
            {
                // 裁剪所用的矩形的大小
                var width = Math.Abs(firstPoint.X - point.X);
                var height = Math.Abs(firstPoint.Y - point.Y);
                var minX = Math.Min(firstPoint.X, point.X);
                var minY = Math.Min(firstPoint.Y, point.Y);
                Rectangle rect = new Rectangle(minX, minY, width, height);
                // 将直线和多边形都裁剪一遍
                var len = DDALines.Count;
                for (var i = 0; i < len; i++)
                    DDALines[i].Trim(rect);

                len = BresenhamLines.Count;
                for (var i = 0; i < len; i++)
                    BresenhamLines[i].Trim(rect);

                len = polygons.Count;
                for (var i = 0; i < len; i++)
                    polygons[i].Trim(rect);
            }
            else if(mode == "Translation" || mode == "Rotate" || mode == "Scale")
            {
                firstPoint.X = -1;
                firstPoint.Y = -1;
                prevPoint.X = -1;
                prevPoint.Y = -1;
                selected = null;
                totalScaleFactor = 1.0;
                ClearPointerSelection();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            isMouseDown = false;
            ClearPointerSelection();
            mode = "Bresenham";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            isMouseDown = false;
            ClearPointerSelection();
            mode = "DDA";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            isMouseDown = false;
            ClearPointerSelection();
            mode = "Circle";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            isMouseDown = false;
            ClearPointerSelection();
            mode = "Ellipse";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            isMouseDown = false;
            ClearPointerSelection();
            // When user clicks button, show the dialog.
            saveFileDialog1.ShowDialog();
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            isMouseDown = false;
            ClearPointerSelection();
            mode = "Fill";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            isMouseDown = false;
            ClearPointerSelection();
            mode = "Polygon";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            isMouseDown = false;
            ClearPointerSelection();
            // 裁剪
            mode = "Trimming";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            isMouseDown = false;
            ClearPointerSelection();
            // Bezier曲线
            mode = "Bezier";
        }

        private void button10_Click(object sender, EventArgs e)
        {
            isMouseDown = false;
            ClearPointerSelection();
            // B样条曲线
            mode = "Bspline";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            isMouseDown = false;
            ClearPointerSelection();
            // 平移
            mode = "Translation";
        }

        private void button12_Click(object sender, EventArgs e)
        {
            isMouseDown = false;
            ClearPointerSelection();
            // 旋转
            mode = "Rotate";
        }

        private void button13_Click(object sender, EventArgs e)
        {
            isMouseDown = false;
            ClearPointerSelection();
            // 缩放
            mode = "Scale";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Refresh();
        }

        static public void DrawPoint(PaintEventArgs e, Pen pen, int x, int y)
        {
            if (x >= 0 && x < form_width && y >= 40 && y < form_height)
            {
                flag[x][y] = true;
                Rectangle rect = new Rectangle(x, y, 1, 1);
                e.Graphics.DrawRectangle(pen, rect);
            }
        }

        // 检查这个点是否在画布上
        // 画布的范围是不包括button的空白位置
        static public bool CheckOnCanvas(int x, int y)
        {
            return x >= 0 && x < form_width && y >= 40 && y < form_height;
        }

        // 首先先清除上一次选中的图形的信息
        private void ClearPointerSelection()
        {
            selected = null;
            foreach (var l in DDALines)
                l.isSelected = false;
            foreach (var l in BresenhamLines)
                l.isSelected = false;
            foreach (var l in circles)
                l.isSelected = false;
            foreach (var l in ellipses)
                l.isSelected = false;
            foreach (var l in polygons)
                l.isSelected = false;
            foreach (var l in beziers)
                l.isSelected = false;
            foreach (var l in bsplines)
                l.isSelected = false;
        }

        private void Form1_DoubleClick(object sender, EventArgs e)
        {
            if(mode == "Polygon")
            {
                // 画多边形是以双击表示结束的
                // 首先先将当前点加入到多边形的顶点序列中，作为最后一个点
                var point = PointToClient(Cursor.Position);
                vertices.Add(point);
                polygons.Add(new Polygon(vertices));
                vertices.Clear();
            }
            else if(mode == "Bezier")
            {
                // 画多边形是以双击表示结束的
                // 首先先将当前点加入到多边形的顶点序列中，作为最后一个点
                var point = PointToClient(Cursor.Position);
                vertices.Add(point);
                beziers.Add(new Bezier(vertices));
                vertices.Clear();
            }
            else if(mode == "Bspline")
            {
                var point = PointToClient(Cursor.Position);
                vertices.Add(point);
                bsplines.Add(new Bspline(vertices));
                vertices.Clear();
            }
        }

        private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Get file name.
            var name = saveFileDialog1.FileName;
            Console.WriteLine(name);

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

        private void 画图_MouseMove(object sender, MouseEventArgs e)
        {
            Refresh();
        }
    }
}
