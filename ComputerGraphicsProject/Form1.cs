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
        }

        List<DDALine> DDALines = new List<DDALine>();
        List<BresenhamLine> BresenhamLines = new List<BresenhamLine>();
        List<Circle> circles = new List<Circle>();
        List<Ellipse> ellipses = new List<Ellipse>();

        bool isMouseDown = false;
        Point firstPoint;
        string mode = "Bresenham";
        
        // we use Red to draw
        Pen drawingPen = Pens.Red;
        // the graph being drawn is in blue
        Pen currPen = Pens.Blue;
        // the selected graph is in Yellow
        Pen selectionPen = Pens.Yellow;

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
        }

        static public void DrawPoint(PaintEventArgs e, Pen pen, int x, int y)
        {
            Rectangle rect = new Rectangle(x, y, 1, 1);
            e.Graphics.DrawRectangle(pen, rect);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
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
    }
}
