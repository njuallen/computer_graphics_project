using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace ComputerGraphicsProject
{
    class Ellipse
    {
        private Point center;
        private int rx, ry;
        public Ellipse(Point c, int x, int y)
        {
            center = c;
            rx = x;
            ry = y;
        }

        public void Draw(PaintEventArgs e)
        {

            int x = 0;
            int y = ry;
            // 决策参数p
            int p = ry * ry - rx * rx * ry + rx * rx / 4;
            while (2 * ry * ry * x < 2 * rx * rx * y)
            {
                // 画出这个点以及其他三个对称点
                Form1.DrawPoint(e, center.X + x, center.Y + y);
                Form1.DrawPoint(e, center.X + x, center.Y - y);
                Form1.DrawPoint(e, center.X - x, center.Y + y);
                Form1.DrawPoint(e, center.X - x, center.Y - y);
       
                x += 1;
                if (p < 0)
                {
                    p = p + 2 * ry * ry * x + ry * ry;
                }
                else
                {
                    y = y - 1;
                    p = p + 2 * ry * ry * x - 2 * rx * rx * y + ry * ry;
                }
            }

            p = Convert.ToInt32(ry * ry * (rx + 0.5) * (rx + 0.5) + rx * rx * (ry - 1) - rx * rx * ry * ry);

            while(y >= 0)
            {
                Form1.DrawPoint(e, center.X + x, center.Y + y);
                Form1.DrawPoint(e, center.X + x, center.Y - y);
                Form1.DrawPoint(e, center.X - x, center.Y + y);
                Form1.DrawPoint(e, center.X - x, center.Y - y);

                y -= 1;
                if(p > 0)
                {
                    p = p - 2 * rx * rx * y + rx * rx;
                }
                else
                {
                    x += 1;
                    p = p + 2 * ry * ry * x - 2 * rx * rx * y + rx * rx;
                }
            }
        }
    }
}
