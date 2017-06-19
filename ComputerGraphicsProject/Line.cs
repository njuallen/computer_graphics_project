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
            graphicType = "Line";
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

        // 求两条直线的交点
        // 其中l1必须是垂直或水平的直线
        // l2不能和l1平行
        public static Point Intersect(Line l1, Line l2)
        {
            // Console.WriteLine("Intersect");
            if (l1.a.X == l1.b.X)
            {
                // l1是垂直线段
                double k = (l2.b.Y - l2.a.Y) / (l2.b.X - l2.a.X);
                return new Point(l1.a.X, Convert.ToInt32(l2.a.Y + k * (l1.a.X - l2.a.X)));
            }
            else if (l1.a.Y == l1.b.Y)
            {
                // l1是水平线段
                // l2是垂直线段
               
                /*
                Console.WriteLine(l2.a.ToString());
                Console.WriteLine(l2.b.ToString());
               */

                if (l2.a.X == l2.b.X)
                {
                    return new Point(l2.a.X, l1.a.Y);
                }
                else
                {
                    // 做整数除法的时候，务必要转成double再算啊
                    // 不然只要dy的绝对值小于dx，整数除法除出来肯定是0
                    // 下面再把值为0的k放在分母上，很快就除0错了！
                    double k = (double)(l2.b.Y - l2.a.Y) / (l2.b.X - l2.a.X);
                    // Console.WriteLine(string.Format("k: {0}", k));
                    return new Point(Convert.ToInt32((l1.a.Y - l2.a.Y) / k + l2.a.X), l1.a.Y);
                }
            }
            else
            {
                return new Point(-1, -1);
            }
        }

        // 判断p是否在直线l1的某一侧
        // 直线l1必须为垂直或水平
        // side指明是哪一侧，side为true，则说明是坐标增加的侧
        // 例如对于垂直的直线，则为直线的右侧
        // 若side为false，则说明是坐标减小的侧
        // 例如对于水平的直线，就是直线的上侧
        // 若此点恰好位于直线上，对于这个怎么处理，我还没想好
        public static bool isInHalfSpace(Line l1, Point p, bool side)
        {
            if (l1.a.X == l1.b.X)
            {
                // 垂直线段
                if(side)
                {
                    // 右侧
                    return p.X > l1.a.X;
                }
                else
                {
                    // 左侧
                    return p.X < l1.a.X;
                }
            }
            else if (l1.a.Y == l1.b.Y)
            {
                // 水平线段
                if (side)
                {
                    // 下侧
                    return p.Y > l1.a.Y;
                }
                else
                {
                    // 上侧
                    return p.Y < l1.a.Y;
                }

            }
            else
            {
                return false;
            }
        }

        // 利用矩形rect对线段进行裁剪
        // 如果线段有部分在矩形内部，则只保留那部分
        // 如果线段完全在矩形内部或者完全在矩形外部， 则不变
        public override void Trim(Rectangle rect)
		{
			bool aInRect = Helper.IsInRectangle(rect, a);
			bool bInRect = Helper.IsInRectangle(rect, b);

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
					if (Helper.IsInBetween(inPoint, outPoint, upperPoint))
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
					if (Helper.IsInBetween(inPoint, outPoint, leftPoint))
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
                    if (Helper.IsInBetween(a, b, leftPoint) && Helper.IsInRectangle(rect, leftPoint))
                    {
                        // Console.WriteLine("left");
                        l.Add(leftPoint);
                    }

                    if (Helper.IsInBetween(a, b, rightPoint) && Helper.IsInRectangle(rect, rightPoint))
                    {
                        // Console.WriteLine("right");
                        l.Add(rightPoint);
                    }

                    if (Helper.IsInBetween(a, b, upperPoint) && Helper.IsInRectangle(rect, upperPoint))
                    {
                        // Console.WriteLine("upper");
                        l.Add(upperPoint);
                    }

                    if (Helper.IsInBetween(a, b, lowerPoint) && Helper.IsInRectangle(rect, lowerPoint))
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
                    if (!Helper.IsInBetween(a, b, upperPoint))
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
                    if (!Helper.IsInBetween(a, b, leftPoint))
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
                    if (Helper.IsInBetween(a, b, leftPoint) && Helper.IsInRectangle(rect, leftPoint))
                    {
                        // Console.WriteLine("left");
                        l.Add(leftPoint);
                    }

                    if (Helper.IsInBetween(a, b, rightPoint) && Helper.IsInRectangle(rect, rightPoint))
                    {
                        // Console.WriteLine("right");
                        l.Add(rightPoint);
                    }

                    if (Helper.IsInBetween(a, b, upperPoint) && Helper.IsInRectangle(rect, upperPoint))
                    {
                        // Console.WriteLine("upper");
                        l.Add(upperPoint);
                    }

                    if (Helper.IsInBetween(a, b, lowerPoint) && Helper.IsInRectangle(rect, lowerPoint))
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

        public override void Translation(int dx, int dy)
        {
            a.X += dx;
            a.Y += dy;
            b.X += dx;
            b.Y += dy;
            var len = points.Count;
            for (var i = 0; i < len; i++)
            {
                var p = new Point(points[i].X + dx, points[i].Y + dy);
                points[i] = p;
            }
        }

        public override void Rotate(Point c, double sin, double cos)
        {
            // 对于旋转，我们只旋转端点，其他线段上的点重新计算
            // 因为在旋转过程中由于浮点到整数的大量rounding，线段会很快变形
            a = Helper.Rotate(c, a, sin, cos);
            b = Helper.Rotate(c, b, sin, cos);
            points = Points();
        }

        public override void Scale(Point center, double scaleFactor)
        {
            a = Helper.Scale(center, a, scaleFactor);
            b = Helper.Scale(center, b, scaleFactor);
            points = Points();
        }
    }
}
