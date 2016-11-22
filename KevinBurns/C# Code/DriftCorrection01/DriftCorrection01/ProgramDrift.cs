using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DriftCorrection01
{
    class ProgramDrift
    {
        static void Main(string[] args)
        {

            List<double> Timestamps1 = new List<double>();
            List<double> Timestamps2 = new List<double>();
            List<double> Input1 = new List<double>();
            List<double> Input2 = new List<double>();
            List<double> PeakValuesHolder1 = new List<double>();
            List<double> PeakTimeHolder1 = new List<double>();
            List<double> TruePeakHolder1 = new List<double>();
            List<double> TruePeakTimeHolder1 = new List<double>();
            List<double> PeakValuesHolder2 = new List<double>();
            List<double> PeakTimeHolder2 = new List<double>();
            List<double> TruePeakHolder2 = new List<double>();
            List<double> TruePeakTimeHolder2 = new List<double>();
            string path1 = @"C:\Users\burns\Desktop\403\MATLAB Codes\test1_vel.txt";
            string path2 = @"C:\Users\burns\Desktop\403\MATLAB Codes\test2_vel.txt";
            int I1Eletracker = 0;
            int I2Eletracker = 0;
            double I1max = 0;
            double I1container = 0;
            int I1Ele = 0;                          // some variables unused
            double I2max = 0;
            double I2cont = 0;
            int I2Ele = 0;
            double TimeI1 = 0;
            double TimeI2 = 0;
            int I1CorEle = 0;
            int I2CorEle = 0;
            double CorrectionFactor;
            double ispeak;
            double syncCorfact;

            using (TextReader reader = File.OpenText(path1))
            {
                string readin1;
                while ((readin1 = reader.ReadLine()) != null)
                {
                    //string text = reader.ReadLine();
                    string[] bits = readin1.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    double x = double.Parse(bits[0]);
                    double y = double.Parse(bits[1]);
                    Console.WriteLine(x);
                    Timestamps1.Add(x);
                    Console.WriteLine(y);
                    Input1.Add(y);
                }
                Console.WriteLine("Press any key to continue");
                System.Console.ReadKey();
            }

            //Math

            for (int i1iter = 0; i1iter < Input1.Count; i1iter++)
            {
                if (Input1[i1iter] > Input1[i1iter - 1] && Input1[i1iter + 1] < Input1[i1iter])
                {
                    ispeak = Input1[i1iter];
                    PeakValuesHolder1.Add(ispeak);
                    PeakTimeHolder1.Add(Timestamps1[i1iter]);
                }

              
                
            }
            for (int peakiter = 0; peakiter < PeakValuesHolder1.Count; peakiter++)
            {
                if (PeakTimeHolder1[peakiter + 1] - PeakTimeHolder1[peakiter] > 0.5)
                {
                    TruePeakHolder1.Add(PeakValuesHolder1[peakiter]);
                    TruePeakTimeHolder1.Add(PeakTimeHolder1[peakiter]);
                }
            }

            //time sync
            syncCorfact = TruePeakTimeHolder1[3] - TruePeakTimeHolder2[3];
            if (syncCorfact < 0)
            {
                for (int synciter = 0; synciter < Timestamps1.Count; synciter++)
                {
                    Timestamps1[synciter] = Timestamps1[synciter] + syncCorfact;
                }
            }

            else if (syncCorfact > 0)
            {
                for (int synciter = 0; synciter < Timestamps2.Count; synciter++)
                {
                    Timestamps2[synciter] = Timestamps2[synciter] + syncCorfact;
                }
            }

        }
    }
}
