// #define MYDEBUG


using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ComputerGraphicsProject
{
    public partial class Form1 : Form
    {
        public Form1()
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
            if (mode == "Pointer")
            {
                // 距离鼠标位置距离小于3的图形被选中
                foreach (var l in DDALines)
                {
                    if (l.Distance(point) <= 3.0)
                    {
                        l.isSelected = true;
                        return;
                    }
                }
                foreach (var l in BresenhamLines)
                {
                    if (l.Distance(point) <= 3.0)
                    {
                        l.isSelected = true;
                        return;
                    }
                }
                foreach (var l in circles)
                {
                    if (l.Distance(point) <= 3.0)
                    {
                        l.isSelected = true;
                        return;
                    }
                }
                foreach (var l in ellipses)
                {
                    if (l.Distance(point) <= 3.0)
                    {
                        l.isSelected = true;
                        return;
                    }
                }
                foreach (var l in polygons)
                {
                    if (l.Distance(point) <= 3.0)
                    {
                        l.isSelected = true;
                        return;
                    }
                }
                foreach (var l in beziers)
                {
                    if (l.Distance(point) <= 3.0)
                    {
                        l.isSelected = true;
                        return;
                    }
                }
                foreach (var l in bsplines)
                {
                    if (l.Distance(point) <= 3.0)
                    {
                        l.isSelected = true;
                        return;
                    }
                }
            }
            else if(mode == "Fill")
            {
                var block = new Block(point);
                fill.Add(block);
            }
            else if(mode == "Polygon" || mode == "Bezier" || mode == "Bspline")
            {
                // 将当前点加入到多边形的顶点序列中
                vertices.Add(point);
            }
            else
            {

            }
        }

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

            var len = DDALines.Count;
            for (var i = 0; i < len; i++)
				if(DDALines[i].isSelected)
					DDALines[i].Draw(e, selectionPen);
				else
					DDALines[i].Draw(e, drawingPen);

            len = BresenhamLines.Count;
            for (var i = 0; i < len; i++)
				if(BresenhamLines[i].isSelected)
					BresenhamLines[i].Draw(e, selectionPen);
				else
					BresenhamLines[i].Draw(e, drawingPen);

            len = circles.Count;
            for (var i = 0; i < len; i++)
				if(circles[i].isSelected)
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

            // show the one that the user is drawing
            // 如果是画多边形，我们就要先把顶点顺序连接
            // 如果是画样条曲线，我们就要先把控制顶点顺序连接
            var point = PointToClient(Cursor.Position);
            if(mode == "Polygon" || mode == "Bezier" || mode == "Bspline")
            {
                len = vertices.Count;
                for(var i = 0; i < len - 1; i++)
                {
                    var line = new BresenhamLine(vertices[i], vertices[i + 1]);
                    line.Draw(e, currPen);
                }
                if(len > 0)
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
            }
        }
   
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            Point point = PointToClient(Cursor.Position);
            isMouseDown = true;
            firstPoint = point;
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
            mode = "Pointer";
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            Refresh();
        }

        static public void DrawPoint(PaintEventArgs e, Pen pen, int x, int y)
        {
            if (x >= 0 && x < form_width && y >= 60 && y < form_height)
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
            return x >= 0 && x < form_width && y >= 60 && y < form_height;
        }

        // 首先先清除上一次选中的图形的信息
        private void ClearPointerSelection()
        {
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
    }
}
