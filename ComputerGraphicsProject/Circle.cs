using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace ComputerGraphicsProject
{
	class Circle
	{
		private Point center;
		int radius;
		public Circle(Point c, int r)
		{
			center = c;
			radius = r;
		}

		public void Draw(PaintEventArgs e)
		{

			int x = 0;
			int y = radius;
			// 决策参数p
			int p = 5 * radius / 4;
			while(x < y)
			{
				// 画出这个点以及其他七个对称点
				Form1.DrawPoint(e, center.X + x, center.Y + y);
				Form1.DrawPoint(e, center.X + x, center.Y - y);
				Form1.DrawPoint(e, center.X - x, center.Y + y);
				Form1.DrawPoint(e, center.X - x, center.Y - y);
				Form1.DrawPoint(e, center.X + y, center.Y + x);
				Form1.DrawPoint(e, center.X + y, center.Y - x);
				Form1.DrawPoint(e, center.X - y, center.Y + x);
				Form1.DrawPoint(e, center.X - y, center.Y - x);

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
		} 
	}
}
