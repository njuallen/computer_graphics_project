using System;
using System.Drawing;
using System.Collections.Generic;

namespace ComputerGraphicsProject
{
    // 样条曲线的基类
    class Spline : Primitive
    {
        // 一个样条曲线图形包含两个部分
        // 控制多边形 + 生成的样条曲线
        // 控制多边形上的顶点（包含开始结束端点及其他控制顶点
        protected List<Point> controlVertices;
        // 控制多边形上的点
        protected List<Point> controlPolygonPoints;
        // 生成的样条曲线上的点
        protected List<Point> curvePoints;
        // 输入control vertices
        public Spline(List<Point> lp)
        {
            graphicType = "Spline";
            controlVertices = Helper.RemoveDuplicatedPoint(lp);
            points = Points();
        }

        public virtual List<Point> GetCurvePoints()
        {
            return new List<Point>();
        }

        public override List<Point> Points()
        {
            var len = controlVertices.Count;
            controlPolygonPoints = new List<Point>();
            for (var i = 0; i < len - 1; i++)
            {
                var line = new BresenhamLine(controlVertices[i], controlVertices[i + 1], f);
                controlPolygonPoints.AddRange(line.Points());
            }
            curvePoints = GetCurvePoints();
            var result = new List<Point>();
            // 手动将两个list合并成一个新的list
            foreach (var p in controlVertices)
                result.Add(p);
            foreach (var p in controlPolygonPoints)
                result.Add(p);
            foreach (var p in curvePoints)
                result.Add(p);
            return result;
        }

        public override void Translation(int dx, int dy)
        {
            var len = controlVertices.Count;
            for (var i = 0; i < len; i++)
            {
                var p = new Point(controlVertices[i].X + dx, controlVertices[i].Y + dy);
                controlVertices[i] = p;
            }

            len = controlPolygonPoints.Count;
            for (var i = 0; i < len; i++)
            {
                var p = new Point(controlPolygonPoints[i].X + dx, controlPolygonPoints[i].Y + dy);
                controlPolygonPoints[i] = p;
            }

            len = curvePoints.Count;
            for (var i = 0; i < len; i++)
            {
                var p = new Point(curvePoints[i].X + dx, curvePoints[i].Y + dy);
                curvePoints[i] = p;
            }

            len = points.Count;
            for (var i = 0; i < len; i++)
            {
                var p = new Point(points[i].X + dx, points[i].Y + dy);
                points[i] = p;
            }
        }

        public override void Rotate(Point center, double sin, double cos)
        {
            // 只旋转控制节点
            var len = controlVertices.Count;
            for (var i = 0; i < len; i++)
            {
                var p = Helper.Rotate(center, controlVertices[i], sin, cos);
                controlVertices[i] = p;
            }
            controlVertices = Helper.RemoveDuplicatedPoint(controlVertices);
            points = Points();
        }

        public override void Scale(Point center, double scaleFactor)
        {
            var len = controlVertices.Count;
            for (var i = 0; i < len; i++)
            {
                var p = Helper.Scale(center, controlVertices[i], scaleFactor);
                controlVertices[i] = p;
            }
            points = Points();
        }
    }
}
