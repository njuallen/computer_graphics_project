using System.Collections.Generic;
using System.Drawing;

namespace ComputerGraphicsProject
{
    // Block表示填充的色块
    class Polygon : Primitive
    {
        // 多边形的几个顶点
        private List<Point> vertices;
        public Polygon(List<Point> lp)
        {
            // make a deep copy of the list
            vertices = lp.ConvertAll(point => new Point(point.X, point.Y));
            // 多边形边界上的点
            points = Points();
        }

        // 以p点为中心开始扩展，填充
        public List<Point> Points()
        {
            List<Point> l = new List<Point>();
            var len = vertices.Count;
            for (var i = 0; i < len; i++)
            {
                var p = new BresenhamLine(vertices[i], vertices[(i + 1) % len]);
                l.AddRange(p.Points());
            }

            return l;

        }
    }
}
