using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace ComputerGraphicsProject
{
	class Ellipse : Primitive
	{
		private Point center;
		private int rx, ry;
		public Ellipse(Point c, int x, int y, FormPaint form)
		{
            f = form;
            graphicType = "Ellipse";
			center = c;
			rx = x;
			ry = y;
            points = Points();
		}

        // Use MidPoint ellipse drawing algorithm
		public override List<Point> Points()
		{
			List<Point> l = new List<Point>();
			int x = 0;
			int y = ry;
			// 决策参数p
			int p = ry * ry + rx * rx * (1 - 4 * ry) / 4;

            l.Add(new Point(center.X + x, center.Y + y));
            l.Add(new Point(center.X + x, center.Y - y));

            // 区域1，沿x方向取样
            while (ry * ry * x <= rx * rx * y)
			{
				if (p < 0)
				{
					p += ry * ry * (2 * x + 3);
				}
				else
				{
					p += ry * ry * (2 * x + 3) - 2 * rx * rx * (y - 1);
                    y -= 1;
                }
                x += 1;

                // 画出这个点以及其他三个对称点
                l.Add(new Point(center.X + x, center.Y + y));
                l.Add(new Point(center.X + x, center.Y - y));
                l.Add(new Point(center.X - x, center.Y + y));
                l.Add(new Point(center.X - x, center.Y - y));
            }

            // 我们在进行椭圆放缩时，如果拼命拖动鼠标
            // tmp的值可能会很大，乃至超出Int的表示范围
            // 就会报Exception
            // 此时我们就将原来椭圆上的点返回，从显示上来看，就是用户无法再放大了
            try
            {
                double tmp = ry * ry * (x + 0.5) * (x + 0.5) + rx * rx * (y - 1) * (y - 1) - rx * rx * ry * ry;
                p = Convert.ToInt32(tmp);
            }
            catch (System.Exception)
            {
                return points;
            }
           

            // 区域2，沿y方向取样
            while (y >= 0)
			{
				if(p > 0)
				{
					p += rx * rx * (3 - 2 * y);
				}
				else
				{
                    p += rx * rx * (3 - 2 * y) + 2 * ry * ry * (x + 1);
                    x += 1;
                }
                y -= 1;

                l.Add(new Point(center.X + x, center.Y + y));
                l.Add(new Point(center.X + x, center.Y - y));
                l.Add(new Point(center.X - x, center.Y + y));
                l.Add(new Point(center.X - x, center.Y - y));
            }
			return l;
		}

        public override void Translation(int dx, int dy)
        {
            center.X += dx;
            center.Y += dy;
            var len = points.Count;
            for (var i = 0; i < len; i++)
            {
                var p = new Point(points[i].X + dx, points[i].Y + dy);
                points[i] = p;
            }
        }
        // we do not support rotating ellipse

        public override void Scale(Point c, double scaleFactor)
        {
            center = Helper.Scale(c, center, scaleFactor);
            rx = Convert.ToInt32(rx * scaleFactor);
            ry = Convert.ToInt32(ry * scaleFactor);
            points = Points();
        }
    }
}
