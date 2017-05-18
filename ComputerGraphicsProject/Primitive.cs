using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ComputerGraphicsProject
{
    // 这是基类，定义了基本图元类型
    class Primitive
    {
        public bool isSelected = false;
        public virtual List<Point> Points()
        {
            var l = new List<Point>();
            return l;
        }

        public virtual void Draw(PaintEventArgs e, Pen pen)
        {
            var l = Points();
            foreach (var p in l)
            {
                Form1.DrawPoint(e, pen, p.X, p.Y);
            }
        }

        // 返回点a到图元的距离
        // 点a到图元的距离被定义为图元上的点到a点的距离的最小值
        public virtual double Distance(Point a)
        {
            // set the initial distance to a very large value
            double minDistance = 100000000000.0;
            var l = Points();
            foreach (var p in l)
            {
                var dx = a.X - p.X;
                var dy = a.Y - p.Y;
                double currDistance = Math.Sqrt(dx * dx + dy * dy);
                if (currDistance < minDistance)
                    minDistance = currDistance;
            }
            return minDistance;
        }
    }
}
