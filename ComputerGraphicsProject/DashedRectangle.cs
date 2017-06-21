using System.Collections.Generic;
using System.Drawing;

namespace ComputerGraphicsProject
{
    // 边框为虚线的长方形
    // 这个是用来做裁剪框的
    class DashedRectangle : Primitive
    {
        public int x, y, width, height;
        public DashedRectangle(int tx, int ty, int w, int h, FormPaint form)
        {
            f = form;
            graphicType = "DashedRectangle";
            x = tx;
            y = ty;
            width = w;
            height = h;
            points = Points();
        }

        // 以p点为中心开始扩展，填充
        public override List<Point> Points()
        {

            List<Point> l = new List<Point>();
            // 上边框
            for (var i = x; i <= x + width; i++)
                if (i % 2 == 0)
                    l.Add(new Point(i, y));

            // 下边框
            for (var i = x; i <= x + width; i++)
                if (i % 2 == 0)
                    l.Add(new Point(i, y + height));

            // 左边框
            for (var i = y; i <= y + height; i++)
                if (i % 2 == 0)
                    l.Add(new Point(x, i));

            // 右边框
            for (var i = y; i <= y + height; i++)
                if (i % 2 == 0)
                    l.Add(new Point(x + width, i));
            return l;
        }
    }
}