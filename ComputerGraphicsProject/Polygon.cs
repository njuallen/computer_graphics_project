using System;
using System.Collections.Generic;
using System.Drawing;

namespace ComputerGraphicsProject
{
    // Block表示填充的色块
    class Polygon : Primitive
    {
        // 多边形的几个顶点
        public Polygon(List<Point> lp, FormPaint form)
        {
            f = form;
            graphicType = "Polygon";
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
                var p = new BresenhamLine(vertices[i], vertices[(i + 1) % len], f);
                l.AddRange(p.Points());
            }
            return l;
        }

        public override void Trim(Rectangle rect)
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

        // 对此多边形进行扫描线求交，返回此多边形内部的点坐标
        // Naive implementation，使用了简单的逐边求交，
        // 没有使用复杂的数据结构，没有使用整数增量运算求交
        // 虽然在边界上有若干像素点或者在边界上有一小行的像素可能没填充
        // 但整体表现还是正确的！
        public List<Point> ScanLineFill()
        {
            // 首先要求出多边形的ymin及ymax，确定扫描线范围
            var ymin = FormPaint.form_height;
            var ymax = 0;

            foreach (var p in vertices)
            {
                if (p.Y > ymax)
                    ymax = p.Y;
                if (p.Y < ymin)
                    ymin = p.Y;
            }
            List<Point> ret = new List<Point>();
            for (var y = ymin; y <= ymax; y++)
                ret.AddRange(ScanLineFillOneLine(y));
            return ret;
        }

        class Intersection
        {
            // 与扫描线的交点的横坐标
            public int x;
            // 与扫描线相交的边的Id
            public int edgeId;
            // 相交类型
            public int intersectType;
            public Intersection(int a, int id, int type)
            {
                x = a;
                edgeId = id;
                intersectType = type;
            }
        }

        // 扫描线求交：一行
        // 返回值为y的扫描线处理多边形时，扫过的内部点
        private List<Point> ScanLineFillOneLine(int y)
        {
            // 首先是要按顺时针顺序，依次求各边与扫描线的交点
            List<Point> ret = new List<Point>();
            List<Intersection> l = new List<Intersection>();
            var nvertices = vertices.Count;
            for (var i = 0; i < nvertices; i++)
            {
                int type = 0;
                var p = ScanLineIntersect(y, vertices[i], vertices[(i + 1) % nvertices], ref type);
                switch (type)
                {
                    case 1:
                    case -1:
                        // 过上端点或下端点，可能是奇点
                        // 检查当前边与上一条边及当前扫描线的交点会不会是奇点
                        int prevEdgeId = (i - 1) % nvertices;
                        int nextEdgeId = (i + 1) % nvertices;
                        int prevIndex = FindEdgeIntersection(l, prevEdgeId);
                        int nextIndex = FindEdgeIntersection(l, nextEdgeId);
                        if (prevIndex >= 0 && l[prevIndex].x == p
                            && l[prevIndex].intersectType != 2)
                        {
                            // 两条边位于同侧
                            // 当前点记录进去
                            // 两个交点都要变成普通交点
                            if (l[prevIndex].intersectType == type)
                                l.Add(new Intersection(p, i, 2));
                            l[prevIndex].intersectType = 2;
                        }
                        else if (nextIndex >= 0 && l[nextIndex].x == p
                            && l[nextIndex].intersectType != 2)
                        {
                            // 两条边位于同侧
                            // 当前点记录进去
                            // 两个交点都要变成普通节点
                            if (l[nextIndex].intersectType == type)
                                l.Add(new Intersection(p, i, 2));
                            l[nextIndex].intersectType = 2;
                        }
                        else
                            l.Add(new Intersection(p, i, type));
                        break;
                    case 2:
                        // 有交点，但不是端点
                        // 直接添加进去
                        l.Add(new Intersection(p, i, type));
                        break;
                    default:
                        // 无交点
                        break;
                }
            }
            // 最后保留下来的交点
            // 我们仅保留普通交点
            var lp = new List<int>();
            foreach (var p in l)
                if (p.intersectType == 2)
                    lp.Add(p.x);
            lp.Sort();
            if (lp.Count % 2 != 0)
            {
                Console.WriteLine("Error");
                return ret;
            }

            if(lp.Count > 0)
            {
                var n = lp.Count;
                n /= 2;
                for (var i = 0; i < n; i++)
                    for (var j = lp[2 * i]; j < lp[2 * i + 1]; j++)
                        ret.Add(new Point(j, y));
            }
            return ret;
        }

        // 从l中找到id为edgeId的边的intersection记录
        // 并返回那条记录的下标
        // 如果找不到，就返回-1
        private int FindEdgeIntersection(List<Intersection> l, int edgeId)
        {
            int n = l.Count;
            for (int i = 0; i < n; i++)
                if (l[i].edgeId == edgeId)
                    return i;
            return -1;
        }

        // 多边形一条边的两个端点分别为a与b
        // 扫描线高度为y
        // 通过type参数，告诉调用者是以何种形式相交的
        // type == 1，过上端点
        // type == -1，过下端点
        // type == 0，无交点
        // type == 2，有交点，但不是端点
        // 返回交点的横坐标
        private int ScanLineIntersect(int y, Point a, Point b, ref int type)
        {
            // 因为我们的多边形在构造时，就把相同坐标的点给去除了
            // 所以进来的这两个点坐标肯定不同
            // 水平线不必处理，就当成无交点即可
            if(a.Y == b.Y || y > Math.Max(a.Y, b.Y) 
                || y < Math.Min(a.Y, b.Y))
            {
                type = 0;
                return 0;
            }

            Point up = b, down = a;
            if(a.Y > b.Y)
            {
                up = a;
                down = b;
            }

            // 交于上端点
            if(y == up.Y)
            {
                type = 1;
                return up.X;
            }

            // 交于下端点
            if(y == down.Y)
            {
                type = -1;
                return down.X;
            }

            // 不交于端点，交于线段中间的某个点
            type = 2;
            var dy = up.Y - down.Y;
            var dx = up.X - down.X;
            var x = 0;
            if (dx == 0)
                x = up.X;
            else
                x = Convert.ToInt32((double)(y - down.Y) * dx / dy + down.X);
            return x;
        }
    }
}