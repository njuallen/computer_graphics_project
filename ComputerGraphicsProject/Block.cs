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
        public Block(Point c, FormPaint form)
        {
            f = form;
            graphicType = "Block";
            p = c;
            // 此时，一系列点指的是色块内部的点
            points = Points();
        }

        // 以p点为中心开始扩展，填充
        public override List<Point> Points()
        {

            List<Point> l = new List<Point>();
            Queue<Point> q = new Queue<Point>();
            f.screenBuffer[p.X, p.Y, f.fillColor]++;
            q.Enqueue(p);
            while (q.Count > 0)
            {
                p = q.Dequeue();
                var x = p.X;
                var y = p.Y;
                l.Add(p);
                // 使用四邻域扩充
                if (f.CheckOnCanvas(x + 1, y) && !f.IsEmpty(x + 1, y))
                {
                    f.screenBuffer[x + 1, y, f.fillColor]++;
                    q.Enqueue(new Point(x + 1, y));
                }
                if (f.CheckOnCanvas(x - 1, y) && !f.IsEmpty(x - 1, y))
                {
                    f.screenBuffer[x - 1, y, f.fillColor]++;
                    q.Enqueue(new Point(x - 1, y));
                }
                if (f.CheckOnCanvas(x, y + 1) && !f.IsEmpty(x, y + 1))
                {
                    f.screenBuffer[x, y + 1, f.fillColor]++;
                    q.Enqueue(new Point(x, y + 1));
                }
                if (f.CheckOnCanvas(x, y - 1) && !f.IsEmpty(x, y - 1))
                {
                    f.screenBuffer[x, y + 1, f.fillColor]++;
                    q.Enqueue(new Point(x, y - 1));
                }
            }
            return l;
        }
    }
}