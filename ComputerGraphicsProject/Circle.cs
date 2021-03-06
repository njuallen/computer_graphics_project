﻿using System;
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
		public Circle(Point c, int r, FormPaint form)
		{
            f = form;
            graphicType = "Circle";
			center = c;
			radius = r;
            points = Points();
        }

		// 将图形上的点的坐标以一个list的形式返回
		public override List<Point> Points()
		{
			int x = 0;
			int y = radius;
			// 决策参数p
			double p = 5 * radius / 4;
			List<Point> l = new List<Point>();
			while(x <= y)
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
	
				if(p < 0)
				{
					p = p + 2 * x + 1;
				}
				else
				{
					p = p + 2 * x - 2 * y + 2;
					y = y - 1;
				}
                x += 1;
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

        public override void Rotate(Point c, double sin, double cos)
        {
            center = Helper.Rotate(c, center, sin, cos);
            points = Points();
        }


        public override void Scale(Point c, double scaleFactor)
        {
            center = Helper.Scale(c, center, scaleFactor);
            radius = Convert.ToInt32(radius * scaleFactor);
            points = Points();
        }
    }
}
