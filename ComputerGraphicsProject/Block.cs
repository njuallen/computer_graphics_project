using System.Collections.Generic;
using System.Drawing;

namespace ComputerGraphicsProject
{
    // Block表示填充的色块
    class Block : Primitive
    {
        // 从哪个中心点开始的填充
        // 由于我采用的是邻域扩充法，所以需要指明一个中心点
        private Point p;
        public Block(Point c)
        {
            p = c;
            // 此时，一系列点指的是色块内部的点
            points = Points();
        }

        // 以p点为中心开始扩展，填充
        private List<Point> Points()
        {
            {
                List<Point> l = new List<Point>();

                Queue<Point> q = new Queue<Point>();
                Form1.flag[p.X][p.Y] = true;
                q.Enqueue(p);
                while (q.Count > 0)
                {
                    p = q.Dequeue();
                    var x = p.X;
                    var y = p.Y;
                    l.Add(p);
                    // 使用四邻域扩充
                    if (Form1.CheckOnCanvas(x + 1, y) && !Form1.flag[x + 1][y])
                    {
                        Form1.flag[x + 1][y] = true;
                        q.Enqueue(new Point(x + 1, y));
                    }
                    if (Form1.CheckOnCanvas(x - 1, y) && !Form1.flag[x - 1][y])
                    {
                        Form1.flag[x - 1][y] = true;
                        q.Enqueue(new Point(x - 1, y));
                    }
                    if (Form1.CheckOnCanvas(x, y + 1) && !Form1.flag[x][y + 1])
                    {
                        Form1.flag[x][y + 1] = true;
                        q.Enqueue(new Point(x, y + 1));
                    }
                    if (Form1.CheckOnCanvas(x, y - 1) && !Form1.flag[x][y - 1])
                    {
                        Form1.flag[x][y - 1] = true;
                        q.Enqueue(new Point(x, y - 1));
                    }
                }
                return l;
            }
        }
    }
}
