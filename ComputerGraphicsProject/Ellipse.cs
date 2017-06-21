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

		public override List<Point> Points()
		{
			List<Point> l = new List<Point>();
			int x = 0;
			int y = ry;
			// 决策参数p
			int p = ry * ry - rx * rx * ry + rx * rx / 4;
            // 区域1，沿x方向取样
            /*
            Console.WriteLine("****************************");
            Console.WriteLine(string.Format("({0}, {1})", rx, ry));
            */

            // 我认为这里的这个判断条件可能有问题，导致循环多走了好多轮，点都画到椭圆外面去了
            while (ry * ry * x < rx * rx * y)
			{
            
                /*    
                Console.WriteLine("-------------------------");
                Console.WriteLine(string.Format("({0}, {1})", x, y));
                */

                // 画出这个点以及其他三个对称点
                l.Add(new Point(center.X + x, center.Y + y));
				l.Add(new Point(center.X + x, center.Y - y));
				l.Add(new Point(center.X - x, center.Y + y));
				l.Add(new Point(center.X - x, center.Y - y));
				
				if (p < 0)
				{
					p += ry * ry * (1 + 2 * x);
				}
				else
				{
					p += ry * ry * (1 + 2 * x) + rx * rx * (1 - 2 * y);
                    y -= 1;
                }
                x += 1;
            }

			p = Convert.ToInt32(ry * ry * (x + 0.5) * (x + 0.5) + rx * rx * (y - 1) * (y - 1) - rx * rx * ry * ry);
            // Console.WriteLine(string.Format("[{0}, {1}]", x, y));

            // 区域2，沿y方向取样
            while (y >= 0)
			{
                
                /*
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>");
                Console.WriteLine(string.Format("({0}, {1})", x, y));
                */

                l.Add(new Point(center.X + x, center.Y + y));
				l.Add(new Point(center.X + x, center.Y - y));
				l.Add(new Point(center.X - x, center.Y + y));
				l.Add(new Point(center.X - x, center.Y - y));
               

				if(p > 0)
				{
					p += rx * rx * (1 - 2 * y);
				}
				else
				{
                    p += rx * rx * (1 - 2 * y) + ry * ry * (1 + 2 * x);
                    x += 1;
                }
                y -= 1;
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
