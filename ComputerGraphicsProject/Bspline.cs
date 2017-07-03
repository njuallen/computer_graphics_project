using System;
using System.Drawing;
using System.Collections.Generic;

namespace ComputerGraphicsProject
{
    class Bspline : Spline
    {
        public Bspline(List<Point> lp, FormPaint form) : base(lp)
        {
            graphicType = "Bspline";
            f = form;
        }

        private List<double> knotVector;

        public override List<Point> GetCurvePoints()
		{
			var result = new List<Point>();

			var numPoints = vertices.Count;
			var samplePoints = 1000;
			var numSegments = numPoints - 3;

			for(int start_cv = 0, j = 0;j < numSegments; j++, start_cv++) 
			{ 
				// for each section of curve, draw samplePoints number of divisions 
				for(int i = 0;i != samplePoints; i++) { 
					// use the parametric time value 0 to 1 for this curve 
					// segment. 
					double t = (double)i / samplePoints; 
					// the t value inverted 
					double it = 1.0 - t;
					// calculate blending functions for cubic bspline 
					double b0 = it * it * it / 6.0; 
					double b1 = (3 * t * t * t - 6 * t * t + 4) / 6.0; 
					double b2 = (-3 * t * t * t + 3 * t * t + 3 * t + 1) / 6.0; 
					double b3 = t * t * t / 6.0; 
					// calculate the x,y and z of the curve point 
					double x = b0 * GetPoint(start_cv + 0).X + 
						b1 * GetPoint(start_cv + 1).X + 
						b2 * GetPoint(start_cv + 2).X + 
						b3 * GetPoint(start_cv + 3).X; 
					double y = b0 * GetPoint(start_cv + 0).Y + 
						b1 * GetPoint(start_cv + 1).Y + 
						b2 * GetPoint(start_cv + 2).Y + 
						b3 * GetPoint(start_cv + 3).Y ; 
					// specify the point 
					result.Add(new Point(Convert.ToInt32(x), Convert.ToInt32(y)));
				} 
			} 
			return result;
		}


        /*
        // Cox-de Boor recursion formula
        private double CoxdeBoorRecursion(double u, int i, int p)
        {
            double result = 0.0;
            // base case
            if (p == 0)
            {
                if (u >= knotVector[i] && u < knotVector[i + 1])
                    result = 1.0;
                else
                    result = 0.0;
            }
            else
            {
                result = ((u - knotVector[i]) / (knotVector[i + p] - knotVector[i]) *
                    CoxdeBoorRecursion(u, i, p - 1)) +
                    ((knotVector[i + p + 1] - u) / (knotVector[i + p + 1] - knotVector[i + 1]) *
                    CoxdeBoorRecursion(u, i + 1, p - 1));
            }
            // Console.WriteLine(string.Format("N({0}, {1}, {2:F8}) = {3:F8}", i, p, u, result));
            return result;
        }
        */

        // 获取第i个control vertex
        private Point GetPoint(int i)
        {
            var len = vertices.Count;
            if (i < 0)
                return vertices[0];
            if (i >= len)
                return vertices[len - 1];
            return vertices[i];
        }

        /*
        // using De Boor's Algorithm
        // Buggy 
        private Point DeBoor(double u, int p)
        {
            var len = knotVector.Count;
            var s = 0;
            var k = 0;
            for (var i = 0; i < len; i++)
            {
                if (u == knotVector[i])
                {
                    k = i;
                    s = 0;
                    break;
                }
                else if (u > knotVector[i] && u < knotVector[i + 1])
                {
                    //检查u是否落在这个knot的区间内
                    k = i;
                    s = 0;
                    break;
                }
                else
                    continue;
            }
            var h = p - s;

            List<List<Point>> array = new List<List<Point>>();
            for (var i = 0; i < h + 1; i++)
            {
                var l = new List<Point>();
                for (var j = 0; j < h + 1; j++)
                    l.Add(new Point());
                array.Add(l);
            }

            // Console.WriteLine("k: {0}, p :{1} h:{2} s:{3}", k, p, h, s);

            for (var i = 0; i < h + 1; i++)
            {
                // 由于我们故意将最开始的p个knot设置为区间长度为0的，且从0.0开始
                // 所以我们这边的k一定大于p
                var tmp = new Point(GetPoint(k - p + i).X, GetPoint(k - p + i).Y);
                array[0][i] = tmp;
            }

            for(var r = 1; r <= h; r++)
                for(var i = r; i <= h; i++)
                {
                    double weight = (u - knotVector[i]) / (knotVector[i + p - r + 1] + knotVector[i]);
                    double x = 0.0, y = 0.0;
                    x += (1 - weight) * array[r - 1][i - 1].X;
                    y += (1 - weight) * array[r - 1][i - 1].Y;
                    x += weight * array[r - 1][i].X;
                    y += weight * array[r - 1][i].Y;
                    array[r][i] = new Point(Convert.ToInt32(x), Convert.ToInt32(y));
                }
            Console.WriteLine("k: {0}, p :{1} h:{2} s:{3}", k, p, h, s);
            return array[p - s][k - s];
        }
        */
    }

}
