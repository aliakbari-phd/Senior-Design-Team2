using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic.PowerPacks;

namespace LineDrawProject
{
    class Program
    {
        static void Main(string[] args)
        {
            ShapeContainer canvas = new ShapeContainer();
            LineShape theLine = new LineShape();
            canvas.Parent = this;
            theLine.Parent = canvas;
            theLine.StartPoint = new System.Drawing.Point(0, 0);
            theLine.EndPoint = new System.Drawing.Point(640, 480);
        }
    }
}
