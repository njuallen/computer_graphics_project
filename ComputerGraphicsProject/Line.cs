using System;
using System.Drawing;
using System.Collections.Generic;

namespace ComputerGraphicsProject
{
    class Line : Primitive
    {
        protected Point a, b;
        public Line(Point pa, Point pb)
        {
            // a is on the left, b is on the right
            if (pa.X < pb.X)
            {
                a = pa;
                b = pb;
            }
            else
            {
                a = pb;
                b = pa;
            }
        }

        // 利用矩形rect对线段进行裁剪
        // 如果线段有部分在矩形内部，则只保留那部分
        // 如果线段完全在矩形内部或者完全在矩形外部， 则不变
        public void Trim(Rectangle rect)
		{
			bool aInRect = isInRectangle(rect, a);
			bool bInRect = isInRectangle(rect, b);

			// 矩形的边界
			var xMax = rect.X + rect.Width;
			var xMin = rect.X;
			var yMax = rect.Y + rect.Height;
			var yMin = rect.Y;

            /*
            Console.WriteLine("Trimming");
            Console.WriteLine(string.Format("rect: {0}", rect.ToString()));
            Console.WriteLine(string.Format("a: {0}", a.ToString()));
            Console.WriteLine(string.Format("b: {0}", b.ToString()));
            */

            if (aInRect != bInRect)
			{
				// 线段的一个端点在矩形内部，另一个端点在矩形外部
				Point inPoint;
				Point outPoint;
				if (aInRect)
				{
					inPoint = a;
					outPoint = b;
				}
				else
				{
					inPoint = b;
					outPoint = a;
				}

				// Console.WriteLine(string.Format("inPoint: {0}", inPoint.ToString()));
				// Console.WriteLine(string.Format("outPoint: {0}", outPoint.ToString()));

				// 当线段垂直或水平时，进行特殊处理
				if (inPoint.X == outPoint.X)
				{
					// 线段垂直
					// 求出与矩形边界的交点
					var upperPoint = new Point(inPoint.X, yMin);
					var lowerPoint = new Point(inPoint.X, yMax);
					if (isInBetween(inPoint, outPoint, upperPoint))
						outPoint = upperPoint;
					else
						outPoint = lowerPoint;
				}
				else if (inPoint.Y == outPoint.Y)
				{
					// 线段水平
					// 求出与矩形边界的交点
					var leftPoint = new Point(xMin, inPoint.Y);
					var rightPoint = new Point(xMax, inPoint.Y);
					if (isInBetween(inPoint, outPoint, leftPoint))
						outPoint = leftPoint;
					else
						outPoint = rightPoint;
				}
				else
				{
					double dx = inPoint.X - outPoint.X;
					double dy = inPoint.Y - outPoint.Y;
					double k = dy / dx;
					var leftPoint = new Point(xMin, Convert.ToInt32(inPoint.Y + k * (xMin - inPoint.X)));
					var rightPoint = new Point(xMax, Convert.ToInt32(inPoint.Y + k * (xMax - inPoint.X)));
					var upperPoint = new Point(Convert.ToInt32(inPoint.X + (yMin - inPoint.Y) / k), yMin);
					var lowerPoint = new Point(Convert.ToInt32(inPoint.X + (yMax - inPoint.Y) / k), yMax);
                    /*
					Console.WriteLine(string.Format("leftPoint: {0}", leftPoint.ToString()));
					Console.WriteLine(string.Format("rightPoint: {0}", rightPoint.ToString()));
					Console.WriteLine(string.Format("upperPoint: {0}", upperPoint.ToString()));
					Console.WriteLine(string.Format("lowerPoint: {0}", lowerPoint.ToString()));
                    */
                    List<Point> l = new List<Point>();
                    if (isInBetween(a, b, leftPoint) && isInRectangle(rect, leftPoint))
                    {
                        // Console.WriteLine("left");
                        l.Add(leftPoint);
                    }

                    if (isInBetween(a, b, rightPoint) && isInRectangle(rect, rightPoint))
                    {
                        // Console.WriteLine("right");
                        l.Add(rightPoint);
                    }

                    if (isInBetween(a, b, upperPoint) && isInRectangle(rect, upperPoint))
                    {
                        // Console.WriteLine("upper");
                        l.Add(upperPoint);
                    }

                    if (isInBetween(a, b, lowerPoint) && isInRectangle(rect, lowerPoint))
                    {
                        // Console.WriteLine("lower");
                        l.Add(lowerPoint);
                    }

                    // 交点个数应该为1
                    // Console.WriteLine(string.Format("count: {0}", l.Count));
                    if (l.Count != 1)
                        return;
                    outPoint = l[0];
                    // Console.WriteLine(string.Format("inPoint: {0} outPoint: {1}", inPoint.ToString(), outPoint.ToString()));
                }

                // 重新确定端点，并重新计算线段上的点
               
				a = inPoint;
				b = outPoint;
                
                // Console.WriteLine(string.Format("a: {0} b: {1}", a.ToString(), b.ToString()));
                points = Points();
			}
			else if(!aInRect && !bInRect)
			{
                // 线段两端点都在裁剪框外面
                // 此时线段与矩形要么没有交点，要么就有两个交点
                // 当线段垂直或水平时，进行特殊处理
                // Console.WriteLine("miao");
                if (a.X == b.X)
                {
                    // 线段垂直
                    // 求出与矩形边界的交点
                    if (a.X < xMin || a.X > xMax)
                        return;

                    var upperPoint = new Point(a.X, yMin);
                    var lowerPoint = new Point(a.X, yMax);
                    if (!isInBetween(a, b, upperPoint))
                        return;
                    a = upperPoint;
                    b = lowerPoint;
                }
                else if (a.Y == b.Y)
                {
                    // 线段水平
                    // 求出与矩形边界的交点
                    if (a.Y < yMin || a.Y > yMax)
                        return;
                    var leftPoint = new Point(xMin, a.Y);
                    var rightPoint = new Point(xMax, a.Y);
                    if (!isInBetween(a, b, leftPoint))
                        return;
                    a = leftPoint;
                    b = rightPoint;
                }
                else
                {
                    double dx = a.X - b.X;
                    double dy = a.Y - b.Y;
                    double k = dy / dx;
                    var leftPoint = new Point(xMin, Convert.ToInt32(a.Y + k * (xMin - a.X)));
                    var rightPoint = new Point(xMax, Convert.ToInt32(a.Y + k * (xMax - a.X)));
                    var upperPoint = new Point(Convert.ToInt32(a.X + (yMin - a.Y) / k), yMin);
                    var lowerPoint = new Point(Convert.ToInt32(a.X + (yMax - a.Y) / k), yMax);
                    /*
                    Console.WriteLine(string.Format("leftPoint: {0}", leftPoint.ToString()));
                    Console.WriteLine(string.Format("rightPoint: {0}", rightPoint.ToString()));
                    Console.WriteLine(string.Format("upperPoint: {0}", upperPoint.ToString()));
                    Console.WriteLine(string.Format("lowerPoint: {0}", lowerPoint.ToString()));
                    */

                    List<Point> l = new List<Point>();
                    if (isInBetween(a, b, leftPoint) && isInRectangle(rect, leftPoint))
                    {
                        // Console.WriteLine("left");
                        l.Add(leftPoint);
                    }

                    if (isInBetween(a, b, rightPoint) && isInRectangle(rect, rightPoint))
                    {
                        // Console.WriteLine("right");
                        l.Add(rightPoint);
                    }

                    if (isInBetween(a, b, upperPoint) && isInRectangle(rect, upperPoint))
                    {
                        // Console.WriteLine("upper");
                        l.Add(upperPoint);
                    }

                    if (isInBetween(a, b, lowerPoint) && isInRectangle(rect, lowerPoint))
                    {
                        // Console.WriteLine("lower");
                        l.Add(lowerPoint);
                    }

                    // 交点个数应该为2
                    // Console.WriteLine(string.Format("count: {0}", l.Count));
                    if (l.Count != 2)
                        return;
                    a = l[0];
                    b = l[1];
                    // Console.WriteLine(string.Format("a: {0} b: {1}", a.ToString(), b.ToString()));
                }
				points = Points();
			}
		}

        private bool isInRectangle(Rectangle rect, Point p)
        {
            return p.X >= rect.X && p.X <= rect.X + rect.Width && p.Y >= rect.Y && p.Y <= rect.Y + rect.Height;
        }

        // 判断c是不是在有a，b构成的矩形中或直线上
        private bool isInBetween(Point a, Point b, Point c)
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
