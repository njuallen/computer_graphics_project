using System;
using System.Drawing;


namespace ComputerGraphicsProject
{
    class Helper
    {
        public static bool isInRectangle(Rectangle rect, Point p)
        {
            return p.X >= rect.X && p.X <= rect.X + rect.Width && p.Y >= rect.Y && p.Y <= rect.Y + rect.Height;
        }

        // 判断c是不是在有a，b构成的矩形中或直线上
        public static bool isInBetween(Point a, Point b, Point c)
        {
            // Console.WriteLine(string.Format("a: {0} b: {1} c: {2}", a.ToString(), b.ToString(), c.ToString()));
            var maxX = Math.Max(a.X, b.X);
            var minX = Math.Min(a.X, b.X);
            var maxY = Math.Max(a.Y, b.Y);
            var minY = Math.Min(a.Y, b.Y);
            return c.X >= minX && c.X <= maxX && c.Y >= minY && c.Y <= maxY;
        }
    }
}
