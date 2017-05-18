using System;
using System.Collections.Generic;
using System.Drawing;

namespace ComputerGraphicsProject
{
    class DDALine : Line
    {
        public DDALine(Point pa, Point pb) : base(pa, pb)
        {
        }

        public override List<Point> Points()
        {
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
                    k = 1.0 / k;
                }
            }
            if (sampling_direction == "X")
            {
                start = a.X;
                end = b.X;
                second_axis_start = a.Y;
            }
            else
            {
                start = a.Y;
                end = b.Y;
                second_axis_start = a.X;
            }
            if (start > end)
                sampling_delta = -1;

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
                start += sampling_delta;
                second_axis_start = Convert.ToInt32(second_axis_start + k * sampling_delta);
            }
            return l;
        }

    }
}
