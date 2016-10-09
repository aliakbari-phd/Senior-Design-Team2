//------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.Samples.Kinect.BodyBasics
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Microsoft.Kinect;
    using System.IO.Ports;
    using System.Data;
    using System.Collections.Generic;
    using System.Threading;
    using System.Windows.Threading;
    using System.Windows.Forms;

    /// <summary>
    /// Interaction logic for MainWindow
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {


        /// <summary>
        /// Radius of drawn hand circles
        /// </summary>
        private const double HandSize = 30;

        /// <summary>
        /// Thickness of drawn joint lines
        /// </summary>
        private const double JointThickness = 3;

        /// <summary>
        /// Thickness of clip edge rectangles
        /// </summary>
        private const double ClipBoundsThickness = 10;

        /// <summary>
        /// Constant for clamping Z values of camera space points from being negative
        /// </summary>
        private const float InferredZPositionClamp = 0.1f;

        /// <summary>
        /// Brush used for drawing hands that are currently tracked as closed
        /// </summary>
        private readonly Brush handClosedBrush = new SolidColorBrush(Color.FromArgb(128, 255, 0, 0));

        /// <summary>
        /// Brush used for drawing hands that are currently tracked as opened
        /// </summary>
        private readonly Brush handOpenBrush = new SolidColorBrush(Color.FromArgb(128, 0, 255, 0));

        /// <summary>
        /// Brush used for drawing hands that are currently tracked as in lasso (pointer) position
        /// </summary>
        private readonly Brush handLassoBrush = new SolidColorBrush(Color.FromArgb(128, 0, 0, 255));

        /// <summary>
        /// Brush used for drawing joints that are currently tracked
        /// </summary>
        private readonly Brush trackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));

        /// <summary>
        /// Brush used for drawing joints that are currently inferred
        /// </summary>        
        private readonly Brush inferredJointBrush = Brushes.Yellow;

        /// <summary>
        /// Pen used for drawing bones that are currently inferred
        /// </summary>        
        private readonly Pen inferredBonePen = new Pen(Brushes.Gray, 1);

        /// <summary>
        /// Drawing group for body rendering output
        /// </summary>
        private DrawingGroup drawingGroup;

        /// <summary>
        /// Drawing image that we will display
        /// </summary>
        private DrawingImage imageSource;

        private SerialPort serialPort1 = new SerialPort();
        private SerialPort serialPort2 = new SerialPort();
        private SerialPort serialPort3 = new SerialPort();
        private SerialPort serialPort4 = new SerialPort();

        private byte[] RxPkt1 = new byte[50];
        private byte[] RxPkt2 = new byte[50];
        private byte[] RxPkt3 = new byte[50];
        private byte[] RxPkt4 = new byte[50];

        private int stop = 0;
        private int keystart = 0;
        private int kinect_start = 0;
        //private string move = "Kicking";

        /*private static FileStream fs_kinect = new FileStream("C:/Users/Jian/01. Personal research/01. Sensor location calibration using kinect/data_collection/Jian/Kinect.txt", FileMode.Create);
        private StreamWriter sw1 = new StreamWriter(fs_kinect);
        private static FileStream fs_sensor1 = new FileStream("C:/Users/Jian/01. Personal research/01. Sensor location calibration using kinect/data_collection/Jian/Sensor1.txt", FileMode.Create);
        private StreamWriter sw2 = new StreamWriter(fs_sensor1);
        private static FileStream fs_sensor2 = new FileStream("C:/Users/Jian/01. Personal research/01. Sensor location calibration using kinect/data_collection/Jian/Sensor2.txt", FileMode.Create);
        private StreamWriter sw3 = new StreamWriter(fs_sensor2);
        private static FileStream fs_sensor3 = new FileStream("C:/Users/Jian/01. Personal research/01. Sensor location calibration using kinect/data_collection/Jian/Sensor3.txt", FileMode.Create);
        private StreamWriter sw4 = new StreamWriter(fs_sensor3);
        private static FileStream fs_sensor4 = new FileStream("C:/Users/Jian/01. Personal research/01. Sensor location calibration using kinect/data_collection/Jian/Sensor4.txt", FileMode.Create);
        private StreamWriter sw5 = new StreamWriter(fs_sensor4);*/

        private static FileStream fs_kinect_spinebase;
        private StreamWriter spineBaseSW;
        private static FileStream fs_kinect_spinemid;
        private StreamWriter spinemidSW;
        private static FileStream fs_kinect_spineshoulder;
        private StreamWriter spineshoulderSW;
        private static FileStream fs_kinect_rightshoulder;
        private StreamWriter rightshoulderSW;
        private static FileStream fs_kinect_sagittalangle;
        private StreamWriter sagittalangleSW;
        private static FileStream fs_kinect_flexangle;
        private StreamWriter flexangleSW;
        private static FileStream fs_sensor1;
        private StreamWriter sw2;
        private static FileStream fs_sensor2;
        private StreamWriter sw3;
        private static FileStream fs_sensor3;
        private StreamWriter sw4;
        private static FileStream fs_sensor4;
        private StreamWriter sw5;

        //private long timestamp = 0;
        private int bytesRead1 = 0;
        private int bytesRead2 = 0;
        private int bytesRead3 = 0;
        private int bytesRead4 = 0;

        private Queue<byte> data1 = new Queue<byte>(); //Queue structure to store all the bytes received
        private byte[] buffer1 = new byte[4096]; //Buffer to hold data received in serial port

        private Queue<byte> data2 = new Queue<byte>(); //Queue structure to store all the bytes received
        private byte[] buffer2 = new byte[4096]; //Buffer to hold data received in serial port

        private Queue<byte> data3 = new Queue<byte>(); //Queue structure to store all the bytes received
        private byte[] buffer3 = new byte[4096]; //Buffer to hold data received in serial port

        private Queue<byte> data4 = new Queue<byte>(); //Queue structure to store all the bytes received
        private byte[] buffer4 = new byte[4096]; //Buffer to hold data received in serial port

        private int threadCount = 4;
        Thread[] ProcessSensorData;

        /// <summary>
        /// Active Kinect sensor
        /// </summary>
        private KinectSensor kinectSensor = null;

        /// <summary>
        /// Coordinate mapper to map one type of point to another
        /// </summary>
        private CoordinateMapper coordinateMapper = null;

        /// <summary>
        /// Reader for body frames
        /// </summary>
        private BodyFrameReader bodyFrameReader = null;

        /// <summary>
        /// Array for the bodies
        /// </summary>
        private Body[] bodies = null;

        /// <summary>
        /// definition of bones
        /// </summary>
        private List<Tuple<JointType, JointType>> bones;

        /// <summary>
        /// Width of display (depth space)
        /// </summary>
        private int displayWidth;

        /// <summary>
        /// Height of display (depth space)
        /// </summary>
        private int displayHeight;

        /// <summary>
        /// List of colors for each body tracked
        /// </summary>
        private List<Pen> bodyColors;

        /// <summary>
        /// Current status text to display
        /// </summary>
        private string statusText = null;
        private SensorData sensorData = new SensorData();
        private DispatcherTimer Timer;
        private int time = 15;

        private DataAnalysis dataAnalysis;
        private KinectFeedback kinectFeedback = new KinectFeedback();

        void timer_Tick(object sender, EventArgs e)
        {
            if (time > 0)
            {
                if (time <= 6)
                {
                    TBCountDown.Foreground = Brushes.Red;
                }
                else
                {
                    TBCountDown.Foreground = Brushes.Black;
                }
                time--;
                TBCountDown.Text = string.Format("0{0}:{1}", time / 60, time % 60);
            }
            else
            {
                kinect_start = 0;
                stop = 1;
                spineBaseSW.Close();
                spinemidSW.Close();
                spineshoulderSW.Close();
                rightshoulderSW.Close();
                sagittalangleSW.Close();
                flexangleSW.Close();
                sw2.Close();
                sw3.Close();

                fs_kinect_spinebase.Close();
                fs_kinect_spinemid.Close();
                fs_kinect_spineshoulder.Close();
                fs_kinect_rightshoulder.Close();
                fs_kinect_sagittalangle.Close();
                fs_kinect_flexangle.Close();
                fs_sensor1.Close();
                fs_sensor2.Close();

                kinectFeedback.initialPosRS.Clear();
                kinectFeedback.initialPosSM.Clear();
                kinectFeedback.initialPosSB.Clear();
                kinectFeedback.sagittalAngle.Clear();
                kinectFeedback.flexAngle.Clear();
                kinectFeedback.isInitial = true;

                button1.IsEnabled = true;
                ButtonStop.IsEnabled = false;
                TBCountDown.Text = string.Format("0{0}:{1}", time / 60, time % 60);
                Timer.Stop();
                time = 15;
            }
        }

        private void BrowseFolderButton_Click(object sender, EventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();

            comboBox3.Text = dialog.SelectedPath;
            BrowseFolderButton.IsEnabled = true;
        }

    /// <summary>
    /// Initializes a new instance of the MainWindow class.
    /// </summary>
    public MainWindow()
        {
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Tick += timer_Tick;

            // one sensor is currently supported
            this.kinectSensor = KinectSensor.GetDefault();

            // get the coordinate mapper
            this.coordinateMapper = this.kinectSensor.CoordinateMapper;

            // get the depth (display) extents
            FrameDescription frameDescription = this.kinectSensor.DepthFrameSource.FrameDescription;

            // get size of joint space
            this.displayWidth = frameDescription.Width;
            this.displayHeight = frameDescription.Height;

            // open the reader for the body frames
            this.bodyFrameReader = this.kinectSensor.BodyFrameSource.OpenReader();

            // a bone defined as a line between two joints
            this.bones = new List<Tuple<JointType, JointType>>();

            // Torso
            this.bones.Add(new Tuple<JointType, JointType>(JointType.Head, JointType.Neck));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.Neck, JointType.SpineShoulder));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.SpineMid));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineMid, JointType.SpineBase));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.ShoulderRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.ShoulderLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineBase, JointType.HipRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineBase, JointType.HipLeft));

            // Right Arm
            this.bones.Add(new Tuple<JointType, JointType>(JointType.ShoulderRight, JointType.ElbowRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.ElbowRight, JointType.WristRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.WristRight, JointType.HandRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.HandRight, JointType.HandTipRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.WristRight, JointType.ThumbRight));

            // Left Arm
            this.bones.Add(new Tuple<JointType, JointType>(JointType.ShoulderLeft, JointType.ElbowLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.ElbowLeft, JointType.WristLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.WristLeft, JointType.HandLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.HandLeft, JointType.HandTipLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.WristLeft, JointType.ThumbLeft));

            // Right Leg
            this.bones.Add(new Tuple<JointType, JointType>(JointType.HipRight, JointType.KneeRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.KneeRight, JointType.AnkleRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.AnkleRight, JointType.FootRight));

            // Left Leg
            this.bones.Add(new Tuple<JointType, JointType>(JointType.HipLeft, JointType.KneeLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.KneeLeft, JointType.AnkleLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.AnkleLeft, JointType.FootLeft));

            // populate body colors, one for each BodyIndex
            this.bodyColors = new List<Pen>();

            this.bodyColors.Add(new Pen(Brushes.Red, 6));
            this.bodyColors.Add(new Pen(Brushes.Orange, 6));
            this.bodyColors.Add(new Pen(Brushes.Green, 6));
            this.bodyColors.Add(new Pen(Brushes.Blue, 6));
            this.bodyColors.Add(new Pen(Brushes.Indigo, 6));
            this.bodyColors.Add(new Pen(Brushes.Violet, 6));

            // set IsAvailableChanged event notifier
            this.kinectSensor.IsAvailableChanged += this.Sensor_IsAvailableChanged;

            // open the sensor
            this.kinectSensor.Open();

            // set the status text
            this.StatusText = this.kinectSensor.IsAvailable ? Properties.Resources.RunningStatusText
                                                            : Properties.Resources.NoSensorStatusText;

            // Create the drawing group we'll use for drawing
            this.drawingGroup = new DrawingGroup();

            // Create an image source that we can use in our image control
            this.imageSource = new DrawingImage(this.drawingGroup);

            // use the window object as the view model in this simple example
            this.DataContext = this;

            // initialize the components (controls) of the window
            this.InitializeComponent();

            ProcessSensorData = new Thread[threadCount];
            (ProcessSensorData[0] = new Thread(Processing1)).Start();
            (ProcessSensorData[1] = new Thread(Processing2)).Start();
            (ProcessSensorData[2] = new Thread(Processing3)).Start();
            (ProcessSensorData[3] = new Thread(Processing4)).Start();
        }

        /// <summary>
        /// INotifyPropertyChangedPropertyChanged event to allow window controls to bind to changeable data
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the bitmap to display
        /// </summary>
        public ImageSource ImageSource
        {
            get
            {
                return this.imageSource;
            }
        }

        /// <summary>
        /// Gets or sets the current status text to display
        /// </summary>
        public string StatusText
        {
            get
            {
                return this.statusText;
            }

            set
            {
                if (this.statusText != value)
                {
                    this.statusText = value;

                    // notify any bound elements that the text has changed
                    if (this.PropertyChanged != null)
                    {
                        this.PropertyChanged(this, new PropertyChangedEventArgs("StatusText"));
                    }
                }
            }
        }

        /// <summary>
        /// Execute start up tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.bodyFrameReader != null)
            {
                this.bodyFrameReader.FrameArrived += this.Reader_FrameArrived;
            }
        }

        /// <summary>
        /// Execute shutdown tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (this.bodyFrameReader != null)
            {
                // BodyFrameReader is IDisposable
                this.bodyFrameReader.Dispose();
                this.bodyFrameReader = null;
            }

            if (this.kinectSensor != null)
            {
                this.kinectSensor.Close();
                this.kinectSensor = null;
            }
        }

        /// <summary>
        /// Handles the body frame data arriving from the sensor
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        /// 
        List<List<float>> kinectData = default(List<List<float>>);
        private void Reader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            bool dataReceived = false;

            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    if (this.bodies == null)
                    {
                        this.bodies = new Body[bodyFrame.BodyCount];
                    }

                    // The first time GetAndRefreshBodyData is called, Kinect will allocate each Body in the array.
                    // As long as those body objects are not disposed and not set to null in the array,
                    // those body objects will be re-used.
                    bodyFrame.GetAndRefreshBodyData(this.bodies);
                    dataReceived = true;
                }
            }
            if (kinect_start != 0)
            {
                Timer.Start();
                foreach (Body body in bodies)
                {
                    if (body.IsTracked == true)
                    {
                        DateTime datenow = DateTime.Now;
                        int hour = datenow.Hour;
                        int minute = datenow.Minute;
                        int second = datenow.Second;
                        int millisecond = datenow.Millisecond;
                        int timestamp = hour * 3600 * 1000 + minute * 60 * 1000 + second * 1000 + millisecond;
                        List<float> spineBaseData = new List<float>();
                        List<float> spineMidData = new List<float>();
                        List<float> spineShoulderData = new List<float>();
                        List<float> rightShoulderData = new List<float>();
                        List<List<float>> kinectData = new List<List<float>>();

                        if(kinectFeedback.isInitial == true)
                        {
                            kinectFeedback.initialPosRS.Add(body.Joints[JointType.ShoulderRight].Position.X);
                            kinectFeedback.initialPosSS.Add(body.Joints[JointType.SpineShoulder].Position.X);
                            kinectFeedback.initialPosSM.Add(body.Joints[JointType.SpineMid].Position.X);
                            kinectFeedback.initialPosSB.Add(body.Joints[JointType.SpineBase].Position.X);
                            kinectFeedback.initialPosRS.Add(body.Joints[JointType.ShoulderRight].Position.Y);
                            kinectFeedback.initialPosSS.Add(body.Joints[JointType.SpineShoulder].Position.Y);
                            kinectFeedback.initialPosSM.Add(body.Joints[JointType.SpineMid].Position.Y);
                            kinectFeedback.initialPosSB.Add(body.Joints[JointType.SpineBase].Position.Y);
                            kinectFeedback.initialPosRS.Add(body.Joints[JointType.ShoulderRight].Position.Z);
                            kinectFeedback.initialPosSS.Add(body.Joints[JointType.SpineShoulder].Position.Z);
                            kinectFeedback.initialPosSM.Add(body.Joints[JointType.SpineMid].Position.Z);
                            kinectFeedback.initialPosSB.Add(body.Joints[JointType.SpineBase].Position.Z);
                            kinectFeedback.isInitial = false;
                        }

                        else
                        {
                            List<float> rightShoulderPos = new List<float>();
                            List<float> spineMidPos = new List<float>();
                            rightShoulderPos.Add(body.Joints[JointType.ShoulderRight].Position.X);
                            spineMidPos.Add(body.Joints[JointType.SpineMid].Position.X);
                            rightShoulderPos.Add(body.Joints[JointType.ShoulderRight].Position.Y);
                            spineMidPos.Add(body.Joints[JointType.SpineMid].Position.Y);
                            rightShoulderPos.Add(body.Joints[JointType.ShoulderRight].Position.Z);
                            spineMidPos.Add(body.Joints[JointType.SpineMid].Position.Z);
                            kinectFeedback.CalcSagittalAngleWithRespectToInitialPos(rightShoulderPos);
                            kinectFeedback.CalcFlexAngleWithRespectToInitialPos(spineMidPos);
                            sagittalAngle.Text = kinectFeedback.sagittalAngleTxt;
                            flexAngle.Text = kinectFeedback.flexAngleTxt;
                        }

                        //Collect Spine Base Data
                        spineBaseData.Add(body.Joints[JointType.SpineBase].Position.X);
                        spineBaseData.Add(body.Joints[JointType.SpineBase].Position.Y);
                        spineBaseData.Add(body.Joints[JointType.SpineBase].Position.Z);
                        if(body.Joints[JointType.SpineBase].TrackingState == TrackingState.Tracked)
                        {
                            spineBaseData.Add(1);
                        }
                        else
                        {
                            spineBaseData.Add(0);
                        }
                        spineBaseData.Add(timestamp);

                        //Collect Spine Mid Data
                        spineMidData.Add(body.Joints[JointType.SpineMid].Position.X);
                        spineMidData.Add(body.Joints[JointType.SpineMid].Position.Y);
                        spineMidData.Add(body.Joints[JointType.SpineMid].Position.Z);
                        if (body.Joints[JointType.SpineMid].TrackingState == TrackingState.Tracked)
                        {
                            spineMidData.Add(1);
                        }
                        else
                        {
                            spineMidData.Add(0);
                        }
                        spineMidData.Add(timestamp);

                        //Collect Spine Shoulder Data
                        spineShoulderData.Add(body.Joints[JointType.SpineShoulder].Position.X);
                        spineShoulderData.Add(body.Joints[JointType.SpineShoulder].Position.Y);
                        spineShoulderData.Add(body.Joints[JointType.SpineShoulder].Position.Z);
                        if (body.Joints[JointType.SpineShoulder].TrackingState == TrackingState.Tracked)
                        {
                            spineShoulderData.Add(1);
                        }
                        else
                        {
                            spineShoulderData.Add(0);
                        }
                        spineShoulderData.Add(timestamp);

                        //Collect Right Shoulder Data
                        rightShoulderData.Add(body.Joints[JointType.ShoulderRight].Position.X);
                        rightShoulderData.Add(body.Joints[JointType.ShoulderRight].Position.Y);
                        rightShoulderData.Add(body.Joints[JointType.ShoulderRight].Position.Z);
                        if (body.Joints[JointType.ShoulderRight].TrackingState == TrackingState.Tracked)
                        {
                            rightShoulderData.Add(1);
                        }
                        else
                        {
                            rightShoulderData.Add(0);
                        }
                        rightShoulderData.Add(timestamp);

                        //Collect lists of kinect data
                        kinectData.Add(spineBaseData);
                        kinectData.Add(spineMidData);
                        kinectData.Add(spineShoulderData);
                        kinectData.Add(rightShoulderData);

                        //Write Kinect Joint Data to text files
                        spineBaseSW.WriteLine(body.Joints[JointType.SpineBase].Position.X + " " + body.Joints[JointType.SpineBase].Position.Y + " " + body.Joints[JointType.SpineBase].Position.Z + " " + body.Joints[JointType.SpineBase].TrackingState + " " + timestamp);
                        spinemidSW.WriteLine(body.Joints[JointType.SpineMid].Position.X + " " + body.Joints[JointType.SpineMid].Position.Y + " " + body.Joints[JointType.SpineMid].Position.Z + " " + body.Joints[JointType.SpineMid].TrackingState + " " + timestamp);
                        spineshoulderSW.WriteLine(body.Joints[JointType.SpineShoulder].Position.X + " " + body.Joints[JointType.SpineShoulder].Position.Y + " " + body.Joints[JointType.SpineShoulder].Position.Z + " " + body.Joints[JointType.SpineShoulder].TrackingState + " " + timestamp);
                        rightshoulderSW.WriteLine(body.Joints[JointType.ShoulderRight].Position.X + " " + body.Joints[JointType.ShoulderRight].Position.Y + " " + body.Joints[JointType.ShoulderRight].Position.Z + " " + body.Joints[JointType.ShoulderRight].TrackingState + " " + timestamp);
                        sagittalangleSW.WriteLine(kinectFeedback.currentSagittalAngle + " " + timestamp);
                        flexangleSW.WriteLine(kinectFeedback.currentFlexAngle + " " + timestamp);
                    }
                }
            }
            if (dataReceived)
            {
                using (DrawingContext dc = this.drawingGroup.Open())
                {
                    // Draw a transparent background to set the render size
                    dc.DrawRectangle(Brushes.Black, null, new Rect(0.0, 0.0, this.displayWidth, this.displayHeight));

                    int penIndex = 0;
                    foreach (Body body in this.bodies)
                    {
                        Pen drawPen = this.bodyColors[penIndex++];

                        if (body.IsTracked)
                        {
                            this.DrawClippedEdges(body, dc);

                            IReadOnlyDictionary<JointType, Joint> joints = body.Joints;

                            // convert the joint points to depth (display) space
                            Dictionary<JointType, Point> jointPoints = new Dictionary<JointType, Point>();

                            foreach (JointType jointType in joints.Keys)
                            {
                                // sometimes the depth(Z) of an inferred joint may show as negative
                                // clamp down to 0.1f to prevent coordinatemapper from returning (-Infinity, -Infinity)
                                CameraSpacePoint position = joints[jointType].Position;
                                if (position.Z < 0)
                                {
                                    position.Z = InferredZPositionClamp;
                                }

                                DepthSpacePoint depthSpacePoint = this.coordinateMapper.MapCameraPointToDepthSpace(position);
                                jointPoints[jointType] = new Point(depthSpacePoint.X, depthSpacePoint.Y);
                            }

                            this.DrawBody(joints, jointPoints, dc, drawPen);

                            this.DrawHand(body.HandLeftState, jointPoints[JointType.HandLeft], dc);
                            this.DrawHand(body.HandRightState, jointPoints[JointType.HandRight], dc);
                        }
                    }

                    // prevent drawing outside of our render area
                    this.drawingGroup.ClipGeometry = new RectangleGeometry(new Rect(0.0, 0.0, this.displayWidth, this.displayHeight));
                }
            }
        }

        /// <summary>
        /// Draws a body
        /// </summary>
        /// <param name="joints">joints to draw</param>
        /// <param name="jointPoints">translated positions of joints to draw</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        /// <param name="drawingPen">specifies color to draw a specific body</param>
        private void DrawBody(IReadOnlyDictionary<JointType, Joint> joints, IDictionary<JointType, Point> jointPoints, DrawingContext drawingContext, Pen drawingPen)
        {
            // Draw the bones
            foreach (var bone in this.bones)
            {
                this.DrawBone(joints, jointPoints, bone.Item1, bone.Item2, drawingContext, drawingPen);
            }

            // Draw the joints
            foreach (JointType jointType in joints.Keys)
            {
                Brush drawBrush = null;

                TrackingState trackingState = joints[jointType].TrackingState;

                if (trackingState == TrackingState.Tracked)
                {
                    drawBrush = this.trackedJointBrush;
                }
                else if (trackingState == TrackingState.Inferred)
                {
                    drawBrush = this.inferredJointBrush;
                }

                if (drawBrush != null)
                {
                    drawingContext.DrawEllipse(drawBrush, null, jointPoints[jointType], JointThickness, JointThickness);
                }
            }
        }

        /// <summary>
        /// Draws one bone of a body (joint to joint)
        /// </summary>
        /// <param name="joints">joints to draw</param>
        /// <param name="jointPoints">translated positions of joints to draw</param>
        /// <param name="jointType0">first joint of bone to draw</param>
        /// <param name="jointType1">second joint of bone to draw</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        /// /// <param name="drawingPen">specifies color to draw a specific bone</param>
        private void DrawBone(IReadOnlyDictionary<JointType, Joint> joints, IDictionary<JointType, Point> jointPoints, JointType jointType0, JointType jointType1, DrawingContext drawingContext, Pen drawingPen)
        {
            Joint joint0 = joints[jointType0];
            Joint joint1 = joints[jointType1];

            // If we can't find either of these joints, exit
            if (joint0.TrackingState == TrackingState.NotTracked ||
                joint1.TrackingState == TrackingState.NotTracked)
            {
                return;
            }

            // We assume all drawn bones are inferred unless BOTH joints are tracked
            Pen drawPen = this.inferredBonePen;
            if ((joint0.TrackingState == TrackingState.Tracked) && (joint1.TrackingState == TrackingState.Tracked))
            {
                drawPen = drawingPen;
            }

            drawingContext.DrawLine(drawPen, jointPoints[jointType0], jointPoints[jointType1]);
        }

        /// <summary>
        /// Draws a hand symbol if the hand is tracked: red circle = closed, green circle = opened; blue circle = lasso
        /// </summary>
        /// <param name="handState">state of the hand</param>
        /// <param name="handPosition">position of the hand</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        private void DrawHand(HandState handState, Point handPosition, DrawingContext drawingContext)
        {
            switch (handState)
            {
                case HandState.Closed:
                    drawingContext.DrawEllipse(this.handClosedBrush, null, handPosition, HandSize, HandSize);
                    break;

                case HandState.Open:
                    drawingContext.DrawEllipse(this.handOpenBrush, null, handPosition, HandSize, HandSize);
                    break;

                case HandState.Lasso:
                    drawingContext.DrawEllipse(this.handLassoBrush, null, handPosition, HandSize, HandSize);
                    break;
            }
        }

        /// <summary>
        /// Draws indicators to show which edges are clipping body data
        /// </summary>
        /// <param name="body">body to draw clipping information for</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        private void DrawClippedEdges(Body body, DrawingContext drawingContext)
        {
            FrameEdges clippedEdges = body.ClippedEdges;

            if (clippedEdges.HasFlag(FrameEdges.Bottom))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, this.displayHeight - ClipBoundsThickness, this.displayWidth, ClipBoundsThickness));
            }

            if (clippedEdges.HasFlag(FrameEdges.Top))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, 0, this.displayWidth, ClipBoundsThickness));
            }

            if (clippedEdges.HasFlag(FrameEdges.Left))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, 0, ClipBoundsThickness, this.displayHeight));
            }

            if (clippedEdges.HasFlag(FrameEdges.Right))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(this.displayWidth - ClipBoundsThickness, 0, ClipBoundsThickness, this.displayHeight));
            }
        }

        /// <summary>
        /// Handles the event which the sensor becomes unavailable (E.g. paused, closed, unplugged).
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Sensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
        {
            // on failure, set the status text
            this.StatusText = this.kinectSensor.IsAvailable ? Properties.Resources.RunningStatusText
                                                            : Properties.Resources.SensorNotAvailableStatusText;
        }

        //  start create files for Kinect and sensor and start to record data
        private void button1_Click(object sender, RoutedEventArgs e)
        {

            string trial = comboBox4.Text;
            
            //string fpath = "C:/Users/BennyChan/Desktop/";
            string fpath = comboBox3.Text.Replace("\\","/") + "/" + trial + "/";
            if (String.IsNullOrEmpty(fpath))
            {
                fpath = Directory.GetCurrentDirectory().Replace("\\","/") + "/SD01/" + trial + "/";
            }

            string[] dirs = Directory.GetFiles(fpath);
            int num = dirs.Length;
            fs_kinect_spinebase = new FileStream(string.Concat(fpath,"spinebase"+ trial +".txt"), FileMode.Create);
            spineBaseSW = new StreamWriter(fs_kinect_spinebase);

            fs_kinect_spinemid = new FileStream(string.Concat(fpath, "spinemid" + trial + ".txt"), FileMode.Create);
            spinemidSW = new StreamWriter(fs_kinect_spinemid);

            fs_kinect_spineshoulder = new FileStream(string.Concat(fpath, "spineshoulder" + trial + ".txt"), FileMode.Create);
            spineshoulderSW = new StreamWriter(fs_kinect_spineshoulder);

            fs_kinect_rightshoulder = new FileStream(string.Concat(fpath, "rightshoulder" + trial + ".txt"), FileMode.Create);
            rightshoulderSW = new StreamWriter(fs_kinect_rightshoulder);

            fs_kinect_sagittalangle = new FileStream(string.Concat(fpath, "sagittalangle" + trial + ".txt"), FileMode.Create);
            sagittalangleSW = new StreamWriter(fs_kinect_sagittalangle);

            fs_kinect_flexangle = new FileStream(string.Concat(fpath, "flexangle" + trial + ".txt"), FileMode.Create);
            flexangleSW = new StreamWriter(fs_kinect_flexangle);

            fs_sensor1 = new FileStream(string.Concat(fpath, "IMUmid" + trial + ".txt"), FileMode.Create);
            sw2 = new StreamWriter(fs_sensor1);

            fs_sensor2 = new FileStream(string.Concat(fpath, "IMUbase" + trial + ".txt"), FileMode.Create);
            sw3 = new StreamWriter(fs_sensor2);

            kinect_start = 1;
            button1.IsEnabled = false;
            ButtonStop.IsEnabled = true;
        }


        private void button3_Click(object sender, RoutedEventArgs e)
        {
            kinect_start = 0;
            stop = 1;
            spineBaseSW.Close();
            spinemidSW.Close();
            spineshoulderSW.Close();
            rightshoulderSW.Close();
            sagittalangleSW.Close();
            flexangleSW.Close();
            sw2.Close();
            sw3.Close();

            fs_kinect_spinebase.Close();
            fs_kinect_spinemid.Close();
            fs_kinect_spineshoulder.Close();
            fs_kinect_rightshoulder.Close();
            fs_kinect_sagittalangle.Close();
            fs_kinect_flexangle.Close();
            fs_sensor1.Close();
            fs_sensor2.Close();

            kinectFeedback.initialPosRS.Clear();
            kinectFeedback.initialPosSM.Clear();
            kinectFeedback.initialPosSB.Clear();
            kinectFeedback.sagittalAngle.Clear();
            kinectFeedback.flexAngle.Clear();
            kinectFeedback.isInitial = true;

            Timer.Stop();
            time = 0;
            button1.IsEnabled = true;
            ButtonStop.IsEnabled = false;
            time = 15;
        }

        private void DataReceivedHandler1(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
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

        private void DataReceivedHandler2(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {

            bytesRead2 = serialPort2.Read(buffer2, 0, buffer2.Length);

            lock (data2)
            {

                for (int i = 0; i < bytesRead2; i++)
                {
                    data2.Enqueue(buffer2[i]);
                }

                Monitor.Pulse(data2);
            }

        }

        private void DataReceivedHandler3(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {

            bytesRead3 = serialPort3.Read(buffer3, 0, buffer3.Length);

            lock (data3)
            {

                for (int i = 0; i < bytesRead3; i++)
                {
                    data3.Enqueue(buffer3[i]);
                }

                Monitor.Pulse(data3);
            }

        }

        private void DataReceivedHandler4(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {

            bytesRead4 = serialPort4.Read(buffer4, 0, buffer4.Length);

            lock (data4)
            {

                for (int i = 0; i < bytesRead4; i++)
                {
                    data4.Enqueue(buffer4[i]);
                }

                Monitor.Pulse(data4);
            }

        }

        List<List<Int16>> wearableData = default(List<List<Int16>>);

        private void Processing1()
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
                    if (kinect_start != 0)
                    {
                        DateTime datenow = DateTime.Now;
                        List<Int16> wearable = default(List<Int16>);
                        int hour = datenow.Hour;
                        int minute = datenow.Minute;
                        int second = datenow.Second;
                        int millisecond = datenow.Millisecond;
                        int timestampS = hour * 3600 * 1000 + minute * 60 * 1000 + second * 1000 + millisecond;
                        wearable.Add(BitConverter.ToInt16(convert, 22));
                        wearable.Add(BitConverter.ToInt16(convert, 20));
                        wearable.Add(BitConverter.ToInt16(convert, 18));
                        wearable.Add(BitConverter.ToInt16(convert, 16));
                        wearable.Add(BitConverter.ToInt16(convert, 14));
                        wearable.Add(BitConverter.ToInt16(convert, 12));
                        wearable.Add(BitConverter.ToInt16(convert, 10));
                        wearable.Add(BitConverter.ToInt16(convert, 8));
                        wearable.Add(BitConverter.ToInt16(convert, 6));
                        wearable.Add((Int16)(timestampS));
                        wearableData.Add(wearable);
                        sw2.WriteLine(BitConverter.ToInt16(convert, 22) + " " + BitConverter.ToInt16(convert, 20) + " " + BitConverter.ToInt16(convert, 18) + " " + BitConverter.ToInt16(convert, 16) + " " + BitConverter.ToInt16(convert, 14) + " " + BitConverter.ToInt16(convert, 12) + " " + BitConverter.ToInt16(convert, 10) + " " + BitConverter.ToInt16(convert, 8) + " " + BitConverter.ToInt16(convert, 6) + " " + timestampS);
                    }
                }
            }
        }

        private void Processing2()
        {
            while (true)
            {
                lock (data2)
                {
                    while (data2.Count < 50)
                    {
                        Monitor.Wait(data2);
                    }


                    int i = 1;
                    while (i == 1)
                    {
                        RxPkt2[0] = data2.Dequeue();
                        if (RxPkt2[0] == 16)
                        {
                            RxPkt2[1] = data2.Dequeue();
                            if (RxPkt2[1] == 1)
                            {
                                int j = 1;
                                int k = 2;
                                while (j == 1)
                                {
                                    RxPkt2[k] = data2.Dequeue();
                                    if (RxPkt2[k] == 16)
                                    {
                                        RxPkt2[k + 1] = data2.Dequeue();
                                        if (RxPkt2[k + 1] == 4)
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
                        convert[i] = RxPkt2[i];
                    }
                    Array.Reverse(convert);//operating system is little endians while the package is big endians, reverse the array.
                    if (kinect_start != 0)
                    {
                        DateTime datenow = DateTime.Now;
                        int hour = datenow.Hour;
                        int minute = datenow.Minute;
                        int second = datenow.Second;
                        int millisecond = datenow.Millisecond;
                        int timestampS2 = hour * 3600 * 1000 + minute * 60 * 1000 + second * 1000 + millisecond;
                        sw3.WriteLine(BitConverter.ToInt16(convert, 22) + " " + BitConverter.ToInt16(convert, 20) + " " + BitConverter.ToInt16(convert, 18) + " " + BitConverter.ToInt16(convert, 16) + " " + BitConverter.ToInt16(convert, 14) + " " + BitConverter.ToInt16(convert, 12) + " " + BitConverter.ToInt16(convert, 10) + " " + BitConverter.ToInt16(convert, 8) + " " + BitConverter.ToInt16(convert, 6) + " " + timestampS2);
                    }
                }
            }
        }

        private void Processing3()
        {
            while (true)
            {
                lock (data3)
                {
                    while (data3.Count < 50)
                    {
                        Monitor.Wait(data3);
                    }


                    int i = 1;
                    while (i == 1)
                    {
                        RxPkt3[0] = data3.Dequeue();
                        if (RxPkt3[0] == 16)
                        {
                            RxPkt3[1] = data3.Dequeue();
                            if (RxPkt3[1] == 1)
                            {
                                int j = 1;
                                int k = 2;
                                while (j == 1)
                                {
                                    RxPkt3[k] = data3.Dequeue();
                                    if (RxPkt3[k] == 16)
                                    {
                                        RxPkt3[k + 1] = data3.Dequeue();
                                        if (RxPkt3[k + 1] == 4)
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
                        convert[i] = RxPkt3[i];
                    }
                    Array.Reverse(convert);//operating system is little endians while the package is big endians, reverse the array.
                    if (kinect_start != 0)
                    {
                        DateTime datenow = DateTime.Now;
                        int hour = datenow.Hour;
                        int minute = datenow.Minute;
                        int second = datenow.Second;
                        int millisecond = datenow.Millisecond;
                        int timestampS3 = hour * 3600 * 1000 + minute * 60 * 1000 + second * 1000 + millisecond;
                        sw4.WriteLine(BitConverter.ToInt16(convert, 22) + " " + BitConverter.ToInt16(convert, 20) + " " + BitConverter.ToInt16(convert, 18) + " " + BitConverter.ToInt16(convert, 16) + " " + BitConverter.ToInt16(convert, 14) + " " + BitConverter.ToInt16(convert, 12) + " " + BitConverter.ToInt16(convert, 10) + " " + BitConverter.ToInt16(convert, 8) + " " + BitConverter.ToInt16(convert, 6) + " " + timestampS3);
                    }
                }
            }
        }


        private void Processing4()
        {
            while (true)
            {
                lock (data4)
                {
                    while (data4.Count < 50)
                    {
                        Monitor.Wait(data4);
                    }


                    int i = 1;
                    while (i == 1)
                    {
                        RxPkt4[0] = data4.Dequeue();
                        if (RxPkt4[0] == 16)
                        {
                            RxPkt4[1] = data4.Dequeue();
                            if (RxPkt4[1] == 1)
                            {
                                int j = 1;
                                int k = 2;
                                while (j == 1)
                                {
                                    RxPkt4[k] = data4.Dequeue();
                                    if (RxPkt4[k] == 16)
                                    {
                                        RxPkt4[k + 1] = data4.Dequeue();
                                        if (RxPkt4[k + 1] == 4)
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
                        convert[i] = RxPkt4[i];
                    }
                    Array.Reverse(convert);//operating system is little endians while the package is big endians, reverse the array.
                    if (kinect_start != 0)
                    {
                        DateTime datenow = DateTime.Now;
                        int hour = datenow.Hour;
                        int minute = datenow.Minute;
                        int second = datenow.Second;
                        int millisecond = datenow.Millisecond;
                        int timestampS4 = hour * 3600 * 1000 + minute * 60 * 1000 + second * 1000 + millisecond;
                        sw5.WriteLine(BitConverter.ToInt16(convert, 22) + " " + BitConverter.ToInt16(convert, 20) + " " + BitConverter.ToInt16(convert, 18) + " " + BitConverter.ToInt16(convert, 16) + " " + BitConverter.ToInt16(convert, 14) + " " + BitConverter.ToInt16(convert, 12) + " " + BitConverter.ToInt16(convert, 10) + " " + BitConverter.ToInt16(convert, 8) + " " + BitConverter.ToInt16(convert, 6) + " " + timestampS4);
                    }
                }
            }
        }
        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (keystart == 0 && e.Key == System.Windows.Input.Key.PageDown)
            {
                kinect_start = 1;
                button1.IsEnabled = false;
                keystart = 1;
                ButtonStop.IsEnabled = true;
            }

            else if (keystart == 1 && e.Key == System.Windows.Input.Key.PageDown)
            {
                kinect_start = 0;
                keystart = 0;
                stop = 1;
                spineBaseSW.Close();
                spinemidSW.Close();
                spineshoulderSW.Close();
                rightshoulderSW.Close();
                sagittalangleSW.Close();
                flexangleSW.Close();
                sw2.Close();
                sw3.Close();
                sw4.Close();
                sw5.Close();
                fs_kinect_spinebase.Close();
                fs_kinect_spinemid.Close();
                fs_kinect_spineshoulder.Close();
                fs_kinect_rightshoulder.Close();
                fs_kinect_sagittalangle.Close();
                fs_kinect_flexangle.Close();
                fs_sensor1.Close();
                fs_sensor2.Close();
                fs_sensor3.Close();
                fs_sensor4.Close();
                ButtonStop.IsEnabled = false;
                button1.IsEnabled = true;
            }
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {

            if (comboBox1.Text != "")
            {
                serialPort1.PortName = "COM" + comboBox1.Text;
                serialPort1.BaudRate = 115200;
                serialPort1.Parity = Parity.None;
                serialPort1.StopBits = StopBits.One;
                serialPort1.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler1);
                serialPort1.Open();
            }

            if (comboBox2.Text != "")
            {
                serialPort2.PortName = "COM" + comboBox2.Text;
                serialPort2.BaudRate = 115200;
                serialPort2.Parity = Parity.None;
                serialPort2.StopBits = StopBits.One;
                serialPort2.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler2);
                serialPort2.Open();
            }

            button4.IsEnabled = false;
        }


    }



}
