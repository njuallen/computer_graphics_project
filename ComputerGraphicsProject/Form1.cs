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

        List<Line> lines = new List<Line>();

        bool is_first_point = true;
        Point first_point;

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
                lines.Add(new Line(first_point, point));
                is_first_point = true;
            }
        }

        static public void DrawPoint(PaintEventArgs e, int x, int y)
        {
            Rectangle rect = new Rectangle(x, y, 1, 1);
            e.Graphics.DrawRectangle(Pens.Red, rect);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            var len = lines.Count;
            for (var i = 0; i < len; i++)
                lines[i].DDA(e);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Refresh();
        }
    }
}
