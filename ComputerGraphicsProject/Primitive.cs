// #define MYDEBUG

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
        // 这个图元边界上的点
        protected List<Point> points;

        public virtual void Draw(PaintEventArgs e, Pen pen)
        {
#if MYDEBUG
            Console.WriteLine("*************************************");
#endif
            foreach (var p in points)
            {
#if MYDEBUG
                Console.WriteLine(p.ToString());
#endif
                Form1.DrawPoint(e, pen, p.X, p.Y);
            }
#if MYDEBUG
            Console.WriteLine("*************************************");
#endif
        }

        public virtual void Translation(int dx, int dy)
        {
            Console.WriteLine("Function Translation not implemented!");
        }

        public virtual void Rotate(Point center, double sin, double cos)
        {
            Console.WriteLine("Function Rotate not implemented!");
        }

        public virtual List<Point> Points()
        {
            return new List<Point>();
        }
        // 返回点a到图元的距离
        // 点a到图元的距离被定义为图元上的点到a点的距离的最小值
        public virtual double Distance(Point a)
        {
            // set the initial distance to a very large value
            double minDistance = 100000000000.0;
            foreach (var p in points)
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
