﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace ComputerGraphicsProject
{
    class Line
    {
        private Point a, b;
        public Line(Point pa, Point pb)
        {
            // a is on the left, b is on the right
            if (pa.X < pb.X)
            {
                a = pa;
                b = pb;
            }
            else
            {
                a = pb;
                b = pa;
            } 
        }

        public void Bresenham(PaintEventArgs e)
        {
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
                if(a.X < b.X)
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
                if(a.Y < b.Y)
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
                    Form1.DrawPoint(e, start, second_axis_start);
                }
                else
                {
                    Form1.DrawPoint(e, second_axis_start, start);
                }
                if(k > 0)
                {
                    if(p > 0)
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
                    if(p > 0)
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
        }

        public void DDA(PaintEventArgs e)
        {
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
                if(Math.Abs(k) > 1.0)
                {
                    // choose y direction
                    sampling_direction = "Y";
                    k = 1.0 / k;
                }
            }
            if(sampling_direction == "X")
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

            while(start != end)
            {
                if(sampling_direction == "X")
                {
                    Form1.DrawPoint(e, start, second_axis_start);
                }
                else
                {
                    Form1.DrawPoint(e, second_axis_start, start);
                }
                start += sampling_delta;
                second_axis_start = Convert.ToInt32(second_axis_start + k * sampling_delta);
            }
        }
       
    }
}
