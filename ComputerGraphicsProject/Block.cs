using System.Collections.Generic;
using System.Drawing;

namespace ComputerGraphicsProject
{
    // Block表示填充的色块
    // 本质上就是一系列点的集合
    class Block : Primitive
    {
        public Block(List<Point> l, FormPaint form)
        {
            f = form;
            graphicType = "Block";
            points = l;
        }
    }
}