using System;
using System.Drawing;
using System.Collections.Generic;


namespace ComputerGraphicsProject
{
    class Helper
    {
        public static bool IsInRectangle(Rectangle rect, Point p)
        {
            return p.X >= rect.X && p.X <= rect.X + rect.Width && p.Y >= rect.Y && p.Y <= rect.Y + rect.Height;
        }

        // 判断c是不是在有a，b构成的矩形中或直线上
        public static bool IsInBetween(Point a, Point b, Point c)
        {
            // Console.WriteLine(string.Format("a: {0} b: {1} c: {2}", a.ToString(), b.ToString(), c.ToString()));
            var maxX = Math.Max(a.X, b.X);
            var minX = Math.Min(a.X, b.X);
            var maxY = Math.Max(a.Y, b.Y);
            var minY = Math.Min(a.Y, b.Y);
            return c.X >= minX && c.X <= maxX && c.Y >= minY && c.Y <= maxY;
        }

        // 我们将lp中的点按照顺序加入到一个新的list中
        // 并确保新的list中没有重复点
        // 返回新的list
        public static List<Point> RemoveDuplicatedPoint(List<Point> lp)
        {
            var result = new List<Point>();
            foreach (var p in lp)
            {
                bool found = false;
                foreach (var v in result)
                {
                    if (v.X == p.X && v.Y == p.Y)
                    {
                        found = true;
                        break;
                    }
                }
                // 这样子，我们确保多边形的顶点没有重复
                if (!found)
                    result.Add(p);
            }
            return result;
        }

        public static List<Point> CopyList(List<Point> lp)
        {
            var result = new List<Point>();
            foreach (var p in lp)
                result.Add(new Point(p.X, p.Y));
            return result;
        }

        public static double PointDistance(Point a, Point b)
        {
            var dx = a.X - b.X;
            var dy = a.Y - b.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        // 求出点a相对于点center旋转b度之后的位置
        public static Point Rotate(Point center, Point a, double sin, double cos)
        {
            // 先转到原点系中
            var x = a.X - center.X;
            var y = a.Y - center.Y;
            double newX = x * cos - y * sin;
            double newY = x * sin + y * cos;
            return new Point(Convert.ToInt32(center.X + newX), Convert.ToInt32(center.Y + newY));
        }

        public static Point Scale(Point center, Point a, double scaleFactor)
        {
            // 先转到原点系中
            var x = a.X - center.X;
            var y = a.Y - center.Y;
            double newX = x * scaleFactor;
            double newY = y * scaleFactor;
            return new Point(Convert.ToInt32(center.X + newX), Convert.ToInt32(center.Y + newY));
        }
    }
}
