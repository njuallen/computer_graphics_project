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

        List<Line> dda_lines = new List<Line>();
        List<Line> bresenham_lines = new List<Line>();
        List<Circle> circles = new List<Circle>();
        List<Ellipse> ellipses = new List<Ellipse>();

        bool is_first_point = true;
        Point first_point;
        string draw = "Bresenham";

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            Point point = PointToClient(Cursor.Position);
            // MessageBox.Show(string.Format("point position: {0}", point));
            if(is_first_point)
            {
                is_first_point = false;
                first_point = point;
                return;
            }
            else
            {
                is_first_point = true;
                if (draw == "DDA")
                {
                    dda_lines.Add(new Line(first_point, point));
                }
                else if(draw == "Bresenham")
                {
                    bresenham_lines.Add(new Line(first_point, point));
                }
                else if(draw == "Circle")
                {
                    int dx = point.X - first_point.X;
                    int dy = point.Y - first_point.Y;
                    int distance = (int)Math.Sqrt(dx * dx + dy * dy);
                    circles.Add(new Circle(first_point, distance));   
                } 
                else
                {
                    // 第一个点和第二个点构成一个矩形，椭圆是这个矩形的内切椭圆
                    var center = new Point((first_point.X + point.X) / 2, (first_point.Y + point.Y) / 2);
                    // x轴长度
                    var x = Math.Abs((first_point.X - point.X) / 2);
                    // y轴长度
                    var y = Math.Abs((first_point.Y - point.Y) / 2);
                    ellipses.Add(new Ellipse(center, x, y));  
                }
            }
        }

        static public void DrawPoint(PaintEventArgs e, int x, int y)
        {
            Rectangle rect = new Rectangle(x, y, 1, 1);
            e.Graphics.DrawRectangle(Pens.Red, rect);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

            var len = dda_lines.Count;
            for (var i = 0; i < len; i++)
                dda_lines[i].DDA(e);

            len = bresenham_lines.Count;
            for (var i = 0; i < len; i++)
                bresenham_lines[i].Bresenham(e);

            len = circles.Count;
            for (var i = 0; i < len; i++)
                circles[i].Draw(e);

            len = ellipses.Count;
            for (var i = 0; i < len; i++)
                ellipses[i].Draw(e);

            // show the one that the user is drawing
            var point = PointToClient(Cursor.Position);
            if (!is_first_point)
            {
                // MessageBox.Show(string.Format("{0} {1}", first_point, point));
                if (draw == "DDA")
                {
                    var line = new Line(first_point, point);
                    line.DDA(e);
                }
                else if (draw == "Bresenham")
                {
                    var line = new Line(first_point, point);
                    line.Bresenham(e);
                }
                else if(draw == "Circle")
                {
                    int dx = point.X - first_point.X;
                    int dy = point.Y - first_point.Y;
                    int distance = (int)Math.Sqrt(dx * dx + dy * dy);
                    var circle = new Circle(first_point, distance);
                    circle.Draw(e);
                }
                else
                {
                    // 第一个点和第二个点构成一个矩形，椭圆是这个矩形的内切椭圆
                    var center = new Point((first_point.X + point.X) / 2, (first_point.Y + point.Y) / 2);
                    // x轴长度
                    var x = Math.Abs((first_point.X - point.X) / 2);
                    // y轴长度
                    var y = Math.Abs((first_point.Y - point.Y) / 2);
                    var ellipse = new Ellipse(center, x, y);
                    ellipse.Draw(e);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            is_first_point = true;
            draw = "DDA";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            is_first_point = true;
            draw = "Bresenham";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            is_first_point = true;
            draw = "Circle";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            is_first_point = true;
            draw = "Ellipse";
        }
    }
}
