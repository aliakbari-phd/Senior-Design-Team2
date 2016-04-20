using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initializations
            double CorrFact;
            List<double> Timestamps1 = new List<double>();
            List<double> Timestamps2 = new List<double>();
            List<double> Input1 = new List<double>();
            List<double> Input2 = new List<double>();
            int I1Eletracker=0;
            int I2Eletracker=0;
            double I1max=0;
            double I1cont=0;
            int I1Ele=0;
            double I2max=0;
            double I2cont=0;
            int I2Ele=0;
            double TimeI1=0;
            double TimeI2=0;
            int I1CorEle=0;
            int I2CorEle=0;

            //Read in (example here still need to read in to lists)

            Timestamps1.Add(1);
            Timestamps1.Add(2);
            Timestamps1.Add(3);
            Timestamps1.Add(4);
            Timestamps1.Add(5);
            Timestamps1.Add(6);
            Timestamps1.Add(7);
            Timestamps1.Add(8);
            Timestamps2.Add(1);
            Timestamps2.Add(2);
            Timestamps2.Add(3);
            Timestamps2.Add(4);
            Timestamps2.Add(5);
            Timestamps2.Add(6);
            Timestamps2.Add(7);
            Timestamps2.Add(8);
            Input1.Add(1);
            Input1.Add(2);
            Input1.Add(3);
            Input1.Add(4);
            Input1.Add(6);
            Input1.Add(3);
            Input1.Add(2);
            Input1.Add(1);
            Input2.Add(1);
            Input2.Add(2);
            Input2.Add(3);
            Input2.Add(4);
            Input2.Add(5);
            Input2.Add(6);
            Input2.Add(10);
            Input2.Add(5);

            //Math
            //First input Max Ele tracker
            for (int i1iter = 0; i1iter < Input1.Capacity; i1iter++)
            {
                I1cont = Input1[I1Ele];
                if (Math.Abs(I1cont) > I1max)
                {
                    I1max = I1cont;
                    I1Eletracker = I1Ele;
                }
                I1Ele = I1Ele + 1;
            }
            Console.WriteLine("I1 Max is "+I1max);
            Console.WriteLine("The timestamp for I1 at max is " + Timestamps1[I1Eletracker]);
            
            //Second input Max Ele tracker
            for (int i2iter = 0; i2iter < Input2.Capacity; i2iter++)
            {
                I2cont = Input2[I2Ele];
                if (Math.Abs(I2cont) > I2max)
                {
                    I2max = I2cont;
                    I2Eletracker = I2Ele;
                }
                I2Ele = I2Ele + 1;
            }
            Console.WriteLine("I2 Max is "+I2max);
            Console.WriteLine("The timestamp for I2 at max is " + Timestamps2[I2Eletracker]);
            //Now have the elements for each input that the maximum resides in
            //Next will extract timestamps

            TimeI1 = Timestamps1[I1Eletracker];
            TimeI2 = Timestamps2[I2Eletracker];

            //Determines correction factor
            CorrFact = Math.Abs(TimeI1 - TimeI2);
            Console.WriteLine("Correction Factor is "+CorrFact);

            //Cases for sync, 1) TimeI1 longer than TimeI2 2)Time I2 longer than TimeI1
          
            if (TimeI1 > TimeI2)
            {
                Console.WriteLine("The new timestamps for input two are ");
                for (int i = 0; i < Input2.Capacity-1; i++)
                {
                    Timestamps2[I2CorEle] = Timestamps2[I2CorEle] + CorrFact;
                    Console.WriteLine(Timestamps2[I2CorEle]);
                    I2CorEle = I2CorEle + 1;
                }
                Timestamps2[I2CorEle] = Timestamps2[I2CorEle] + CorrFact;
                Console.WriteLine(Timestamps2[I2CorEle]);
                
            }

            else if (TimeI1 < TimeI2)
            {
                Console.WriteLine("The new timestamps for input one are ");
                for (int i2 = 0; i2 < Input1.Capacity-1; i2++)
                {
                    Timestamps1[I1CorEle] = Timestamps1[I1CorEle] + CorrFact;
                    Console.WriteLine(Timestamps1[I1CorEle]);
                    I1CorEle = I1CorEle + 1;
                }
                Timestamps1[I1CorEle] = Timestamps1[I1CorEle] + CorrFact;
                Console.WriteLine(Timestamps1[I1CorEle]);
                
            }

            else
            {
                Console.WriteLine("No correction needed, signals synced.");
            }

            //Read Out after corrections



            Console.ReadLine();
        }
    }
}
