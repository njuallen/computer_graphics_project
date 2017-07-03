using System;
using System.Drawing;
using System.Collections.Generic;

namespace ComputerGraphicsProject
{
    class Bezier : Spline
    {
        public Bezier(List<Point> lp, FormPaint form) : base(lp)
        {
            graphicType = "Bezier";
            f = form;
        }

        // Bezier曲线的De Casteljau算法
        // From : http://give.zju.edu.cn/cgcourse/new/book/8.2.htm
        public override List<Point> GetCurvePoints()
        {
            var result = new List<Point>();
            var samplePoints = 10000;
            double step = 1.0 / samplePoints;
            for(var i = 0; i < samplePoints; i++)
            {
                result.Add(DeCasteljau(step * i));
            }
            result = Helper.RemoveDuplicatedPoint(result);
            return result;
        }

        private Point DeCasteljau(double u)
        {
            var n = vertices.Count;
            var q = new List<Point>();
            foreach (var p in vertices)
                q.Add(p);

            for (var k = 0; k < n; k++)
                for (var i = 0; i < n - k - 1; i++)
                {
                    var p = new Point();
                    p.X = Convert.ToInt32((1.0 - u) * q[i].X + u * q[i + 1].X);
                    p.Y = Convert.ToInt32((1.0 - u) * q[i].Y + u * q[i + 1].Y);
                    q[i] = p;
                }
            return q[0];
        }
    }
}
