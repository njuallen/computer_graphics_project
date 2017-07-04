using System;
using System.Drawing;
using System.Collections.Generic;

namespace ComputerGraphicsProject
{
    class Line : Primitive
    {
        public Point a, b;
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
            vertices.Add(a);
            vertices.Add(b);
        }

        public Line(List<Point> lp)
        {
            if (lp.Count != 2)
                Console.WriteLine("Line: invalid list!");
            var pa = lp[0];
            var pb = lp[1];
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
            vertices.Add(a);
            vertices.Add(b);
        }

        // 求两条直线的交点
        // 其中l1必须是垂直或水平的直线
        // l2不能和l1平行
        public static Point Intersect(Line l1, Line l2)
        {
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

        public override void Trim(Rectangle rect)
		{
			bool aInRect = Helper.IsInRectangle(rect, a);
			bool bInRect = Helper.IsInRectangle(rect, b);

			// 矩形的边界
			var xMax = rect.X + rect.Width;
			var xMin = rect.X;
			var yMax = rect.Y + rect.Height;
			var yMin = rect.Y;

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
                   
                    List<Point> l = new List<Point>();
                    if (Helper.IsInBetween(a, b, leftPoint) && Helper.IsInRectangle(rect, leftPoint))
                    {
                        l.Add(leftPoint);
                    }

                    if (Helper.IsInBetween(a, b, rightPoint) && Helper.IsInRectangle(rect, rightPoint))
                    {
                        l.Add(rightPoint);
                    }

                    if (Helper.IsInBetween(a, b, upperPoint) && Helper.IsInRectangle(rect, upperPoint))
                    {
                        l.Add(upperPoint);
                    }

                    if (Helper.IsInBetween(a, b, lowerPoint) && Helper.IsInRectangle(rect, lowerPoint))
                    {
                        l.Add(lowerPoint);
                    }

                    // 交点个数应该为1
                    if (l.Count != 1)
                        return;
                    outPoint = l[0];
                }

                // 重新确定端点，并重新计算线段上的点
               
				a = inPoint;
				b = outPoint;
                
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

                    List<Point> l = new List<Point>();
                    if (Helper.IsInBetween(a, b, leftPoint) && Helper.IsInRectangle(rect, leftPoint))
                    {
                        l.Add(leftPoint);
                    }

                    if (Helper.IsInBetween(a, b, rightPoint) && Helper.IsInRectangle(rect, rightPoint))
                    {
                        l.Add(rightPoint);
                    }

                    if (Helper.IsInBetween(a, b, upperPoint) && Helper.IsInRectangle(rect, upperPoint))
                    {
                        l.Add(upperPoint);
                    }

                    if (Helper.IsInBetween(a, b, lowerPoint) && Helper.IsInRectangle(rect, lowerPoint))
                    {
                        l.Add(lowerPoint);
                    }

                    // 交点个数应该为2
                    if (l.Count != 2)
                        return;
                    a = l[0];
                    b = l[1];
                }
				points = Points();
			}
            // 更新vertices
            vertices.Clear();
            vertices.Add(a);
            vertices.Add(b);
		}  

    /*
        // 利用矩形rect对线段进行裁剪
        // 如果线段有部分在矩形内部，则只保留那部分
        // 如果线段完全在矩形内部或者完全在矩形外部， 则不变
        // Liang-Barsky Line Clipping algorithm
        // Algorithm come from this link:
        // https://www.cs.helsinki.fi/group/goa/viewing/leikkaus/intro.html
        public override void Trim(Rectangle rect)
        {
            // 矩形的边界
            var xmax = rect.X + rect.Width;
            var xmin = rect.X;
            var ymax = rect.Y + rect.Height;
            var ymin = rect.Y;
            var dx = b.X - a.X;
            var dy = b.Y - a.Y;

            double tmin = 0.0, tmax = 1.0;

            List<double> tvalue = new List<double>();
            // bottom
            tvalue.Add((double)(ymax - a.Y) / dy);
            // top
            tvalue.Add((double)(ymin - a.Y) / dy);
            // left
            tvalue.Add((double)(xmin - a.X) / dx);
            // right
            tvalue.Add((double)(xmax - a.X) / dx);
            // 裁剪框各边法向量的X与Y值
            double[] nvecX = { 0.0, 0.0, -1.0, 1.0 };
            double[] nvecY = { 1.0, -1.0, 0.0, 0.0 };
            for (var i = 0; i < 4; i++)
            {
                if (tvalue[i] >= tmin && tvalue[i] <= tmax)
                {
                    // use inner product to check whether, it's in edge or out edge;
                    var innerProduct = nvecX[i] * dx + nvecY[i] * dy;
                    // entering edge
                    if (innerProduct < 0)
                        tmin = tvalue[i];
                    // exiting edge
                    else if (innerProduct > 0)
                        tmax = tvalue[i];
                }
            }
            if(tmin < tmax)
            {
                // 裁剪完成，更新线段相关信息
                var na = new Point(Convert.ToInt32(a.X + dx * tmin), 
                    Convert.ToInt32(a.Y + dy * tmin));
                var nb = new Point(Convert.ToInt32(a.X + dx * tmax),
                    Convert.ToInt32(a.Y + dy * tmax));
                a = na;
                b = nb;
                points = Points();
                vertices.Clear();
                vertices.Add(a);
                vertices.Add(b);
            }
        }
        */

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
            vertices.Clear();
            vertices.Add(a);
            vertices.Add(b);
        }
        

        public override void Rotate(Point c, double sin, double cos)
        {
            // 对于旋转，我们只旋转端点，其他线段上的点重新计算
            // 因为在旋转过程中由于浮点到整数的大量rounding，线段会很快变形
            a = Helper.Rotate(c, a, sin, cos);
            b = Helper.Rotate(c, b, sin, cos);
            points = Points();
            vertices.Clear();
            vertices.Add(a);
            vertices.Add(b);
        }

        public override void Scale(Point center, double scaleFactor)
        {
            a = Helper.Scale(center, a, scaleFactor);
            b = Helper.Scale(center, b, scaleFactor);
            points = Points();
            vertices.Clear();
            vertices.Add(a);
            vertices.Add(b);
        }
    }
}
