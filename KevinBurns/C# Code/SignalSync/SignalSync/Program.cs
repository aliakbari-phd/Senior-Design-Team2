using System;
using System.IO;
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
            int I1Eletracker = 0;
            int I2Eletracker = 0;
            double I1max = 0;
            double I1cont = 0;
            int I1Ele = 0;
            double I2max = 0;
            double I2cont = 0;
            int I2Ele = 0;
            double TimeI1 = 0;
            double TimeI2 = 0;
            int I1CorEle = 0;
            int I2CorEle = 0;
            string path1 = @"C:\Users\burns\Desktop\403\MATLAB Codes\test1_1.txt";
            string path2 = @"C:\Users\burns\Desktop\403\MATLAB Codes\test2_1.txt";
            string line;
            //Read in txt file into lists for manipulation

            /*TextReader rdr = File.OpenText(path);
            while ((line = rdr.ReadLine()) != null)
            {
                string text = rdr.ReadLine();
                /*string[] bits = text.Split(' ');   //Incorrect read-in
                double x = double.Parse(bits[0]);
                double y = double.Parse(bits[1]);
                Console.WriteLine(text);
            }
            rdr.Close();*/

            using (TextReader reader = File.OpenText(path1))
            {
                string readin1;
                while ((readin1 = reader.ReadLine()) != null)
                {
                    //string text = reader.ReadLine();
                    string[] bits = readin1.Split(new [] {' '}, StringSplitOptions.RemoveEmptyEntries);
                    double x = double.Parse(bits[0]);
                    double y = double.Parse(bits[1]);
                    Console.WriteLine(x);
                    Timestamps1.Add(x);
                    Console.WriteLine(y);
                    Input1.Add(y);
                }
                Console.WriteLine("To read in next file press any key");
                System.Console.ReadKey();
            }
            
            using (TextReader reader2 = File.OpenText(path2))
            {
                string readin2;
                while ((readin2 = reader2.ReadLine()) != null)
                {
                   // string text2 = reader2.ReadLine();
                    string[] bits = readin2.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    double x2 = double.Parse(bits[0]);
                    double y2 = double.Parse(bits[1]);
                    Console.WriteLine(x2);
                    Timestamps2.Add(x2);
                    Console.WriteLine(y2);
                    Input2.Add(y2);
                }
                Console.WriteLine("Both Files read in, press any key");
                System.Console.ReadKey();
            }
            

            //dummy test
            /* Timestamps1.Add(1);
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
             Input2.Add(5);  */

            //Math Algorithm
            //First input Max Ele tracker
            for (int i1iter = 0; i1iter < Input1.Count; i1iter++)
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
            for (int i2iter = 0; i2iter < Input2.Count; i2iter++)
            {
                I2cont = Input2[i2iter];
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
            Console.WriteLine("Press any key to see the new timestamps");
            System.Console.ReadKey();

            //Cases for sync, 1) TimeI1 longer than TimeI2 2)Time I2 longer than TimeI1
          
            if (TimeI1 > TimeI2)
            {
                Console.WriteLine("The new timestamps for input two are ");
                for (int i = 0; i < Input2.Count-1; i++)
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
                for (int i2 = 0; i2 < Input1.Count-1; i2++)
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
            //Under construction... reading to data structure from data analysis


            Console.WriteLine("Press any key to exit");
            System.Console.ReadKey();
        }
    }
}
