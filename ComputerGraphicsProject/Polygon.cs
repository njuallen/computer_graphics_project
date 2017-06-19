using System;
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
            // 由于我们的多边形在构造时是单击记录一个顶点
            // 最后一个顶点是以双击来记录的
            // 但是这样子经常会导致最后一个点被记录两次，具体原因我还没搞清楚
            // 因此为了避免之后的裁剪出现问题，我们在这里就要清除重复的点
            // make a deep copy of the list
            vertices = Helper.RemoveDuplicatedPoint(lp);
            // 多边形边界上的点
            points = Points();
        }

        // 以p点为中心开始扩展，填充
        public override List<Point> Points()
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

        public void Trim(Rectangle rect)
        {
            // 先使用左侧边框
            // Console.WriteLine("Left");
            TrimWithOneSide(rect, new Line(new Point(rect.X, rect.Y), new Point(rect.X, rect.Bottom)), true);
            // 上侧边框
            // Console.WriteLine("Up");
            TrimWithOneSide(rect, new Line(new Point(rect.X, rect.Y), new Point(rect.Right, rect.Y)), true);
            // 右侧边框
            // Console.WriteLine("Right");
            TrimWithOneSide(rect, new Line(new Point(rect.Right, rect.Y), new Point(rect.Right, rect.Bottom)), false);
            // 下侧边框
            // Console.WriteLine("Down");
            TrimWithOneSide(rect, new Line(new Point(rect.X, rect.Bottom), new Point(rect.Right, rect.Bottom)), false);
            // 重新计算边界上的点
            points = Points();
        }

        // 我们使用的是Sutherland-Hodgman算法
        // 此算法是一次使用矩形框的一边进行裁剪
        private void TrimWithOneSide(Rectangle rect, Line l, bool side)
        {
            List<Point> result = new List<Point>();

            var len = vertices.Count;
            for(var i = 0; i < len; i++)
            {
                var a = vertices[i];
                var b = vertices[(i + 1) % len];
                /*
                Console.WriteLine("*************************************");
                Console.WriteLine(a.ToString());
                Console.WriteLine(b.ToString());
                Console.WriteLine("--------------------------------------");
                foreach (var p in result)
                {
                    Console.WriteLine(p.ToString());
                }
                Console.WriteLine("");
                */

                var aInWindow = Line.isInHalfSpace(l, a, side);
                var bInWindow = Line.isInHalfSpace(l, b, side);
                if (!aInWindow && !bInWindow)
                {
                    // 起始和终止点都在窗口外，则不输出任何点
                    continue;
                }
                else if (aInWindow && bInWindow)
                {
                    // 起始和终止点都在窗口内，则只有终止点输出
                    result.Add(b);
                }
                else if(!aInWindow && bInWindow)
                {
                    // 起始点在外侧，终止点在内测，则输出交点和终止点
                    Point intersection = Line.Intersect(l, new Line(a, b));
                    result.Add(intersection);
                    result.Add(b);
                }
                else
                {
                    // 起始点在内侧，终止点在外测，则输出交点
                    Point intersection = Line.Intersect(l, new Line(a, b));
                    result.Add(intersection);
                }
            }
            vertices = result;
        }

        public override void Translation(int dx, int dy)
        {
            var len = vertices.Count;
            for(var i = 0; i < len; i++)
            {
                var p = new Point(vertices[i].X + dx, vertices[i].Y + dy);
                vertices[i] = p;
            }
            points = Points();
        }
       

        public override void Rotate(Point center, double sin, double cos)
        {
            // 只旋转控制节点
            var len = vertices.Count;
            for (var i = 0; i < len; i++)
            {
                var p = Helper.Rotate(center, vertices[i], sin, cos);
                vertices[i] = p;
            }
            vertices = Helper.RemoveDuplicatedPoint(vertices);
            points = Points();
        }


        public override void Scale(Point center, double scaleFactor)
        {
            var len = vertices.Count;
            for (var i = 0; i < len; i++)
            {
                var p = Helper.Scale(center, vertices[i], scaleFactor);
                vertices[i] = p;
            }
            points = Points();
        }
    }
}