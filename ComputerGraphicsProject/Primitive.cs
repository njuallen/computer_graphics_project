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

        // 当前用来作图的form
        protected FormPaint f;
        
        // 指明这个图形的类型
        public string graphicType;

        // 把这个图形从界面上抹去
        public virtual void UnDraw(int penIndex)
        {
            foreach (var p in points)
                if (f.CheckOnCanvas(p.X, p.Y))
                    f.screenBuffer[p.X, p.Y, penIndex]--;
            f.UpdateMyCanvas(points);
        }

        public virtual void Draw(int penIndex)
        {
            // 我们画就直接画在最顶层
            foreach (var p in points)
                if (f.CheckOnCanvas(p.X, p.Y))
                    f.screenBuffer[p.X, p.Y, penIndex]++;
            f.UpdateMyCanvas(points);
        }

        public virtual void Translation(int dx, int dy)
        {
            Console.WriteLine("Function Translation not implemented!");
        }

        public virtual void Rotate(Point center, double sin, double cos)
        {
            Console.WriteLine("Function Rotate not implemented!");
        }

        public virtual void Scale(Point center, double scaleFactor)
        {
            Console.WriteLine("Function Scale not implemented!");
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

        public virtual void Trim(Rectangle rect)
        {

        }
    }
}
