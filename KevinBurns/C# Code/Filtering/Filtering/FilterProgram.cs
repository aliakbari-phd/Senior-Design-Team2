using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Filtering
{
    class FilterProgram
    {
        static void Main(string[] args)
        {
            double[] UnfilteredData = new double[128];
            double[] FilteredData = new double[128];

            UnfilteredData = MathNet.Numerics.Generate.Sinusoidal(400, 20000, 250, 5);
        }
    }
}
