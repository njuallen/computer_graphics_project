using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace ComputerGraphicsProject
{
	class Circle : Primitive
	{
		private Point center;
		private int radius;
		public Circle(Point c, int r)
		{
			center = c;
			radius = r;
		}

		// 将图形上的点的坐标以一个list的形式返回
		public override List<Point> Points()
		{
			int x = 0;
			int y = radius;
			// 决策参数p
			int p = 5 * radius / 4;
			List<Point> l = new List<Point>();
			while(x < y)
			{
				// 这个点以及其他七个对称点
				l.Add(new Point(center.X + x, center.Y + y));
				l.Add(new Point(center.X + x, center.Y - y));
				l.Add(new Point(center.X - x, center.Y + y));
				l.Add(new Point(center.X - x, center.Y - y));
				l.Add(new Point(center.X + y, center.Y + x));
				l.Add(new Point(center.X + y, center.Y - x));
				l.Add(new Point(center.X - y, center.Y + x));
				l.Add(new Point(center.X - y, center.Y - x));

				x += 1;
				if(p < 0)
				{
					p = p + 2 * x + 1;
				}
				else
				{
					p = p + 2 * x + 1 - 2 * (y - 1);
					y = y - 1;
				}
			}
			return l;
		}
	}
}
