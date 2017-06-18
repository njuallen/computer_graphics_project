using System;
using System.Drawing;
using System.Collections.Generic;

namespace ComputerGraphicsProject
{
    class Bspline : Spline
    {
        public Bspline(List<Point> lp) : base(lp)
        {
        }

        private List<double> knotVector;
        public override List<Point> GetCurvePoints()
        {
            var result = new List<Point>();
           
            // p ： 阶数
            // 我们选取p的阶数为4
            var p = 4;

            // n + 1: 控制节点的个数
            var n = controlVertices.Count  - 1;
            // m = n + p + 1 来源：http://give.zju.edu.cn/cgcourse/new/book/8.4.htm
            var m = n + p + 1;
            // 我们采用均匀B样条
            // u就是所谓的节点矢量
            knotVector = new List<double>();
            // m + 1： 节点矢量的个数
            for (var i = 0; i < m + 1; i++)
            {
                knotVector.Add(i * 1.0 / m);
            }

            Console.WriteLine("knotVector");
            foreach (var a in knotVector)
                Console.Write(string.Format("{0} ", a));
            Console.WriteLine("");

            Console.WriteLine("controlVertices");
            foreach (var a in controlVertices)
                Console.Write(string.Format("{0} ", a.ToString()));
            Console.WriteLine("");

            Console.WriteLine("n: {0} p: {1} m: {2}", n, p, m);

            var samplePoints = 10000;
            double step = 1.0 / samplePoints;
            for (var i = 0; i < samplePoints; i++)
            {
                // Console.WriteLine("****************");
                var u = step * i;
               
                double x = 0.0, y = 0.0;
                for (var j = 0; j <= n; j++)
                {
                    double weight = CoxdeBoorRecursion(u, j, p);
                    x += weight * controlVertices[j].X;
                    y += weight * controlVertices[j].Y;
                }
                result.Add(new Point(Convert.ToInt32(x), Convert.ToInt32(y)));
            }
            result = Helper.RemoveDuplicatedPoint(result);
            return result;
        }

        
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

        /*
        // using De Boor's Algorithm 
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
                    s = 1;
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

            for (var i = 0; i < h + 1; i++)
            {
                // 由于我们故意将最开始的p个knot设置为区间长度为0的，且从0.0开始
                // 所以我们这边的k一定大于p
                Console.WriteLine(string.Format("k: {0}, p :{1} i:{2}", k, p, i));
                var tmp = new Point(controlVertices[k - p + i].X, controlVertices[k - p + i].Y);
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
            return array[p - s][k - s];
        }
        */
    }

}
