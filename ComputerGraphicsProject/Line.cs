using System.Drawing;

namespace ComputerGraphicsProject
{
    class Line : Primitive
    {
        protected Point a, b;
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
    }
}
