using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;
using System.IO.Ports;
using System.IO;

namespace x_IMU_IMU_and_AHRS_Algorithms
{
    class Program
    {
        /// <summary>
        /// Algorithm object.
        /// </summary>
        static AHRS.MadgwickAHRS AHRS = new AHRS.MadgwickAHRS(1f / 256f, 0.3f);
       static AHRS.MahonyAHRS AHRS1 = new AHRS.MahonyAHRS(1f / 256f, 5f);
        static SerialPort serialPort1 = new SerialPort();
        private static Queue<byte> data1 = new Queue<byte>(); //Queue structure to store all the bytes received
        private static byte[] buffer1 = new byte[4096]; //Buffer to hold data received in serial port
        static Thread[] ProcessSensorData;
        private static byte[] RxPkt1 = new byte[50];
        private static int bytesRead1 = 0;

        static uint localCount = 0;
	    static uint newTime;
	    static uint thisRTime = 0;
	    static uint thisRPacket = 0;
	    static uint lastRTime = 0;
	    static uint lastRPacket = 0;
	    static uint firstRTime = 0;
	    static uint firstRPacket = 0;
	    //unsigned char testbuf[100];
	    static uint overflowsTime = 0;
	    static uint lastTimerValue = 0;
	    static uint timerOffsetFromOverflow = 0;
	    static uint offsetCombined = 0;
	    static uint overflowsPacket = 0;
        static float time1 = 0;


        static byte[] readBuffer = new byte[24];
        static Form_3Dcuboid form_3DcuboidA = new Form_3Dcuboid();
        static Form_3Dcuboid form_3DcuboidB = new Form_3Dcuboid(new string[] { "Form_3Dcuboid/RightInv.png", "Form_3Dcuboid/LeftInv.png", "Form_3Dcuboid/BackInv.png", "Form_3Dcuboid/FrontInv.png", "Form_3Dcuboid/TopInv.png", "Form_3Dcuboid/BottomInv.png" });
        /// <summary>
        /// Main method.
        /// </summary>
        /// <param name="args">
        /// Unused.
        /// </param>
        public static void Main(string[] args)
        {
            Console.WriteLine(Assembly.GetExecutingAssembly().GetName().Name + " " + Assembly.GetExecutingAssembly().GetName().Version.Major.ToString() + "." + Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString());
            try
            {
                // Connect to IMU
                
                Console.WriteLine("Searching for IMU...");

                Console.WriteLine("IMU connected successfully");

                // Show 3D cuboid forms
                Console.WriteLine("Showing 3D Cuboid forms...");
                
               // Form_3Dcuboid form_3DcuboidB = new Form_3Dcuboid(new string[] { "Form_3Dcuboid/RightInv.png", "Form_3Dcuboid/LeftInv.png", "Form_3Dcuboid/BackInv.png", "Form_3Dcuboid/FrontInv.png", "Form_3Dcuboid/TopInv.png", "Form_3Dcuboid/BottomInv.png" });
               // The next 8 lines display the cude and render the cubes
                form_3DcuboidA.Text += " Mahony";
                form_3DcuboidB.Text += " Madgwick";
                BackgroundWorker backgroundWorkerA = new BackgroundWorker();
                BackgroundWorker backgroundWorkerB = new BackgroundWorker();
                backgroundWorkerA.DoWork += new DoWorkEventHandler(delegate { form_3DcuboidA.ShowDialog(); });
                backgroundWorkerB.DoWork += new DoWorkEventHandler(delegate { form_3DcuboidB.ShowDialog(); });
                backgroundWorkerA.RunWorkerAsync();
                backgroundWorkerB.RunWorkerAsync();

                // The following 6 lines is to configure the serial port and open the port.
                serialPort1.PortName = "COM9";
                serialPort1.BaudRate = 115200;
                serialPort1.Parity = Parity.None;
                serialPort1.StopBits = StopBits.One;
                serialPort1.Open();
                serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);

                // The following two lines open an new thread to processing the received data. 
                ProcessSensorData = new Thread[1];
                (ProcessSensorData[0] = new Thread(Processing1)).Start();


                Console.WriteLine("Algorithm running in IMU mode.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
           
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
           // sw.Close();
        }

        static private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {

            bytesRead1 = serialPort1.Read(buffer1, 0, buffer1.Length);

            lock (data1)
            {

                for (int i = 0; i < bytesRead1; i++)
                {
                    data1.Enqueue(buffer1[i]);
                }

                Monitor.Pulse(data1);
            }

        }


        private static void Processing1()
        {
            while (true)
            {
                lock (data1)
                {
                    while (data1.Count < 50)
                    {
                        Monitor.Wait(data1);
                    }


                    int i = 1;
                    while (i == 1)
                    {
                        RxPkt1[0] = data1.Dequeue();
                        if (RxPkt1[0] == 16)
                        {
                            RxPkt1[1] = data1.Dequeue();
                            if (RxPkt1[1] == 1)
                            {
                                int j = 1;
                                int k = 2;
                                while (j == 1)
                                {
                                    RxPkt1[k] = data1.Dequeue();
                                    if (RxPkt1[k] == 16)
                                    {
                                        RxPkt1[k + 1] = data1.Dequeue();
                                        if (RxPkt1[k + 1] == 4)
                                        {
                                            j = 0;
                                            i = 0;
                                        }
                                    }
                                    k++;
                                }
                            }
                        }
                    }
                    byte[] convert = new byte[26];
                    for (i = 0; i < 26; i++)
                    {
                        convert[i] = RxPkt1[i];
                    }
                    Array.Reverse(convert);//operating system is little endians while the package is big endians, reverse the array.

                    float[] Accelerometer = new float[3];
                    float[] Gyroscope = new float[3];
                    float[] Mag = new float[3];

                    Accelerometer[0] = (float)(BitConverter.ToInt16(convert, 22)) / 4096;
                    Accelerometer[1] = (float)(BitConverter.ToInt16(convert, 20)) / 4096;
                    Accelerometer[2] = (float)(BitConverter.ToInt16(convert, 18)) / 4096;
                    Gyroscope[0] = (float)(BitConverter.ToInt16(convert, 16)) / (float)32.75;
                    Gyroscope[1] = (float)(BitConverter.ToInt16(convert, 14)) / (float)32.75;
                    Gyroscope[2] = (float)(BitConverter.ToInt16(convert, 12)) / (float)32.75;

                    if (Gyroscope[0] < 3 && Gyroscope[0] > -3) Gyroscope[0] = 0;
                    if (Gyroscope[1] < 3 && Gyroscope[1] > -3) Gyroscope[1] = 0;
                    if (Gyroscope[2] < 3 && Gyroscope[2] > -3) Gyroscope[2] = 0;

                    thisRTime = toMs(RxPkt1[23], RxPkt1[24], timerOffsetFromOverflow);
                    if (thisRTime < lastRTime)
                    {
                        overflowsTime += lastRTime;
                        timerOffsetFromOverflow = 65536 - lastTimerValue;
                        offsetCombined =(uint) ((RxPkt1[23] << 8) + RxPkt1[24] + timerOffsetFromOverflow);
                        thisRTime = toMs(RxPkt1[23], RxPkt1[24], timerOffsetFromOverflow);
                    }

                    lastRTime = thisRTime;
                    lastTimerValue = (uint)((RxPkt1[23] << 8) + RxPkt1[24]);
                    time1 = (thisRTime + overflowsTime - firstRTime)/1000;

                    form_3DcuboidA.RotationMatrix = ConvertToRotateMatrix(AHRS1.Quaternion);
                    form_3DcuboidB.RotationMatrix = ConvertToRotateMatrix(AHRS.Quaternion);

                    // these two lines call the filtering algorithm.
                    AHRS1.Update(-deg2rad(Gyroscope[0]), deg2rad(Gyroscope[1]), -deg2rad(Gyroscope[2]), -Accelerometer[0], Accelerometer[1], -Accelerometer[2], time1);
                    AHRS.Update(-deg2rad(Gyroscope[0]), deg2rad(Gyroscope[1]), -deg2rad(Gyroscope[2]), -Accelerometer[0], Accelerometer[1], -Accelerometer[2], time1);

        
                }
            }
        }


        static uint toMs(Byte a,Byte b, uint overflowTime){
	        //unsigned int myInt = (overflowTime + (a<<8) + b)*1000/500000 ; ///1024 ;  
	        float myFlt = ((float)((overflowTime + (a<<8) + b)*1000))/500000.0f;
	        uint myInt = (uint)(myFlt+0.5f);
	        return myInt;
        }

        /// <summary>
        /// xIMUserial CalInertialAndMagneticDataReceived event to update algorithm in AHRS  mode.
        /// </summary>
       
        static float[] ConvertToRotateMatrix(float []qutenion)
        {
            float[] Qutenion = new float[4];
            Qutenion[0] = qutenion[0];
            Qutenion[1] = -qutenion[1];
            Qutenion[2] = -qutenion[2];
            Qutenion[3] = -qutenion[3];
            float[] RotationMatrix = new float[9];
            RotationMatrix[0] = 2 * Qutenion[0] * Qutenion[0] - 1 + 2 * Qutenion[1] * Qutenion[1];
            RotationMatrix[1] = 2 * (Qutenion[1] * Qutenion[2] + Qutenion[0] * Qutenion[3]);
            RotationMatrix[2] = 2 * (Qutenion[1] * Qutenion[3] - Qutenion[0] * Qutenion[2]);
            RotationMatrix[3] = 2 * (Qutenion[1] * Qutenion[2] - Qutenion[0] * Qutenion[3]);
            RotationMatrix[4] = 2 * Qutenion[0] * Qutenion[0] - 1 + 2 * Qutenion[2] * Qutenion[2];
            RotationMatrix[5] = 2 * (Qutenion[2] * Qutenion[3] + Qutenion[0] * Qutenion[1]);
            RotationMatrix[6] = 2 * (Qutenion[1] * Qutenion[3] + Qutenion[0] * Qutenion[2]);
            RotationMatrix[7] = 2 * (Qutenion[2] * Qutenion[3] - Qutenion[0] * Qutenion[1]);
            RotationMatrix[8] = 2 * Qutenion[0] * Qutenion[0] - 1 + 2 * Qutenion[3] * Qutenion[3];
            return RotationMatrix;

        }

        static void ConvertToEulerAngles(float[] qutenion)
        {
            float[] Qutenion = new float[4];
            Qutenion[0] = qutenion[0];
            Qutenion[1] = -qutenion[1];
            Qutenion[2] = -qutenion[2];
            Qutenion[3] = -qutenion[3];
            double phi, theta, psi;
            phi = Math.Atan2(2 * (Qutenion[2] * Qutenion[3] - Qutenion[0] * Qutenion[1]), (2 * Qutenion[0] * Qutenion[0] + 2 * Qutenion[3] * Qutenion[3] - 1));
            psi = Math.Atan2(2 * (Qutenion[1] * Qutenion[2] - Qutenion[0] * Qutenion[3]), (2 * Qutenion[0] * Qutenion[0] + 2 * Qutenion[1] * Qutenion[1] - 1));
            theta = -Math.Atan(2 * (Qutenion[1] * Qutenion[3] + Qutenion[0] * Qutenion[2]) / Math.Sqrt(1 - 4 * (Qutenion[1] * Qutenion[3] + Qutenion[0] * Qutenion[2]) * (Qutenion[1] * Qutenion[3] + Qutenion[0] * Qutenion[2])));
            phi = phi * 57.3;
            psi = psi * 57.3;
            theta = theta * 57.3;
            string text = phi + " " + psi + " " + theta;
           // sw.WriteLine(text);
        }

        static float[] QuaternionMultiply(float[] q1, float[] q2)
        {
            	float[] q3 = new float[4]; float q_norm;
	            q3[0] = q2[0] * q1[0] - q2[1]*q1[1] - q2[2]*q1[2] - q2[3]*q1[3];
	            q3[1] = q2[0] * q1[1] + q2[1]*q1[0] - q2[2]*q1[3] + q2[3]*q1[2];
	            q3[2] = q2[0] * q1[2] + q2[1]*q1[3] + q2[2]*q1[0] - q2[3]*q1[1];
	            q3[3] = q2[0] * q1[3] - q2[1]*q1[2] + q2[2]*q1[1] + q2[3]*q1[0];
	            q_norm = (float)Math.Sqrt (q3[0]*q3[0] + q3[1]*q3[1] + q3[2]*q3[2] + q3[3]*q3[3]);
	            q3[0] = q3[0] / q_norm;
	            q3[1] = q3[1] / q_norm;
	            q3[2] = q3[2] /q_norm;
	            q3[3] = q3[3] / q_norm;
	//fprintf(fp,"q3 is as: %f ,%f ,%f ,%f\n",q3[0],q3[1],q3[2],q3[3]);
	            return q3;
        }

        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        /// <param name="degrees">
        /// Angular quantity in degrees.
        /// </param>
        /// <returns>
        /// Angular quantity in radians.
        /// </returns>
        static float deg2rad(float degrees)
        {
            return (float)(Math.PI / 180) * degrees;
        }
    }
}