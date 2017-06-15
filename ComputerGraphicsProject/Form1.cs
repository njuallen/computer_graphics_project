using System;
using System.Collections.Generic;
using System.Drawing;
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

        List<DDALine> DDALines = new List<DDALine>();
        List<BresenhamLine> BresenhamLines = new List<BresenhamLine>();
        List<Circle> circles = new List<Circle>();
        List<Ellipse> ellipses = new List<Ellipse>();
        // 填充的块
        List<Block> fill = new List<Block>();

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
        }

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
            }
            else if(mode == "Fill")
            {
                var block = new Block(point);
                fill.Add(block);
            }
            else
            {

            }
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
            return x >= 0 && x <= form_width && y >= 60 && y <= form_height;
        }

		static int form_width = 800;
		static int form_height = 600;

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

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

            len = fill.Count;
            for (var i = 0; i < len; i++)
                fill[i].Draw(e, fillPen);

            // show the one that the user is drawing
            var point = PointToClient(Cursor.Position);
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
                else if(mode == "Circle")
                {
                    int dx = point.X - firstPoint.X;
                    int dy = point.Y - firstPoint.Y;
                    int distance = (int)Math.Sqrt(dx * dx + dy * dy);
                    var circle = new Circle(firstPoint, distance);
                    circle.Draw(e, currPen);
                }
                else if(mode == "Ellipse")
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
                else
                {

                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            isMouseDown = false;
            ClearPointerSelection();
            mode = "DDA";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            isMouseDown = false;
            ClearPointerSelection();
            mode = "Bresenham";
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
            else
            {

            }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            isMouseDown = false;
            ClearPointerSelection();
            mode = "Fill";
        }
    }
}
