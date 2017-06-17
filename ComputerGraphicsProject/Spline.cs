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
                var line = new BresenhamLine(controlVertices[i], controlVertices[i + 1]);
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
    }
}
