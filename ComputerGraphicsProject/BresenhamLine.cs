using System;
using System.Collections.Generic;
using System.Drawing;

namespace ComputerGraphicsProject
{
    class BresenhamLine : Line
    {
        public BresenhamLine(Point pa, Point pb) : base(pa, pb)
        {
            points = Points();
        }

        public override List<Point> Points()
        {
            // 注意：我的这个实现要求a.X < b.X
            // 搜易我们先进行交换，以满足要求
            if(b.X < a.X)
            {
                var tmp = b;
                b = a;
                a = tmp;
            }
            List<Point> l = new List<Point>();
            var dx = b.X - a.X;
            var dy = b.Y - a.Y;
            // decide sampling direction
            var sampling_direction = "X";
            // the start and end coordinate on the sampling direction
            var start = 0;
            var end = 0;
            // the start position of the second axis
            var second_axis_start = 0;
            var sampling_delta = 1;
            double k = 0.0;
            if (dx == 0)
            {
                sampling_direction = "Y";
            }
            else
            {
                k = (double)dy / (double)dx;
                if (Math.Abs(k) > 1.0)
                {
                    // choose y direction
                    sampling_direction = "Y";
                }
            }

            // when switch x and y axis
            // we need to make sure dx >= 0
            // start <= end
            // sampling_delta = 1
            if (sampling_direction == "X")
            {
                if (a.X < b.X)
                {
                    start = a.X;
                    end = b.X;
                    second_axis_start = a.Y;
                }
                else
                {
                    start = b.X;
                    end = a.X;
                    second_axis_start = b.Y;
                }
            }
            else
            {
                k = 1.0 / k;
                if (a.Y < b.Y)
                {
                    start = a.Y;
                    end = b.Y;
                    second_axis_start = a.X;
                    dx = b.Y - a.Y;
                    dy = b.X - a.X;
                }
                else
                {
                    start = b.Y;
                    end = a.Y;
                    second_axis_start = b.X;
                    dx = a.Y - b.Y;
                    dy = a.X - b.X;
                }

            }

            // 决策参数p
            var p = 2 * dy - dx;
            while (start != end)
            {
                if (sampling_direction == "X")
                {
                    l.Add(new Point(start, second_axis_start));
                }
                else
                {
                    l.Add(new Point(second_axis_start, start));
                }
                if (k > 0)
                {
                    if (p > 0)
                    {
                        second_axis_start += 1;
                        p += 2 * dy - 2 * dx;

                    }
                    else
                    {
                        p += 2 * dy;
                    }
                }
                else
                {
                    if (p > 0)
                    {
                        // we always choose the upper point when p > 0
                        // when k < 0, y is upper than y - 1
                        p += 2 * dy;
                    }
                    else
                    {
                        second_axis_start -= 1;
                        p += 2 * dy + 2 * dx;
                    }

                }
                start += sampling_delta;
            }
            return l;
        }

    }
}
