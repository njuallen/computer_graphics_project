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

        List<Point> points = new List<Point>();

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            Point point = PointToClient(Cursor.Position);
            points.Add(point);
            // MessageBox.Show(point.ToString());
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            var len = points.Count;
            // MessageBox.Show(string.Format("len:{0}\n", len));
            for (var i = 0; i < len / 2; i++)
                e.Graphics.DrawLine(Pens.Red, points[2 * i], points[2 * i + 1]);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Refresh();
        }
    }
}
