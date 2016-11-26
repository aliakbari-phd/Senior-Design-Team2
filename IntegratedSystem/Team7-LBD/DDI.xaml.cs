using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;

namespace Microsoft.Samples.Kinect.BodyBasics
{
    /// <summary>
    /// Interaction logic for DDI.xaml
    /// </summary>
    public partial class DDI : Window
    {
        public const string flexAt0Trial1 = "Flex At 0 Deg Trial 1";
        public const string flexAt0Trial2 = "Flex At 0 Deg Trial 2";
        public const string flexAt0Trial3 = "Flex At 0 Deg Trial 3";
        public const string flexAt30LeftTrial1 = "Flex At 30 Deg Left Trial 1";
        public const string flexAt30LeftTrial2 = "Flex At 30 Deg Left Trial 2";
        public const string flexAt30LeftTrial3 = "Flex At 30 Deg Left Trial 3";
        public const string flexAt30RightTrial1 = "Flex At 30 Deg Right Trial 1";
        public const string flexAt30RightTrial2 = "Flex At 30 Deg Right Trial 2";
        public const string flexAt30RightTrial3 = "Flex At 30 Deg Right Trial 3";
        public const string spROMTrial = "Sagittal Plane ROM Trial";
        public const string Angle = "Angle";
        public const string angVel = "Angular Velocity";
        public const string angAccel = "Angular Acceleration";
        public const string angJerk = "Angular Jerk";

        public PlotModel DataPlot { get; set; }

        public DDI()
        {
            InitializeComponent();

            DataPlot = new PlotModel { Title = "Angle", LegendTitle = "Angle" };
            DataPlot.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Angle" });
            DataPlot.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Height" });


            patientIDTxt.Text = "PatientID: " + ApplicationState.dataAnalysis.patientID.ToString();
            ageTxt.Text = "Age: " + ApplicationState.dataAnalysis.age.ToString();
            if (ApplicationState.dataAnalysis.gender == true)
            {
                genderTxt.Text = "Gender: " + "Male";
            }
            else
            {
                genderTxt.Text = "Gender: " + "Female";
            }

            severityLBDTxt.Text = "LBD Severity: " + ApplicationState.dataAnalysis.severityLBD.ToString();
            spROM15Txt.Text = "SP ROM15: " + ApplicationState.dataAnalysis.spROM15.ToString();
            spROM30Txt.Text = "SP ROM30: " + ApplicationState.dataAnalysis.spROM30.ToString();
            fpROMTxt.Text = "FP ROM: " + ApplicationState.dataAnalysis.fpROM.ToString();
            peakVelTxt.Text = "Peak Angular Velocity: " + ApplicationState.dataAnalysis.peakFlexAngVelocityAvgAt0.ToString();
            peakAccTxt.Text = "Peak Angular Acceleration: " + ApplicationState.dataAnalysis.peakFlexAngAccelerationAvgAt0.ToString();
            peakJerkTxt.Text = "Peak Angular Jerk: " + ApplicationState.dataAnalysis.peakFlexAngJerkAvgAt0.ToString();
            twistingROMTxt.Text = "Twisting ROM: " + ApplicationState.dataAnalysis.twistingROM.ToString();

            trialBox.Items.Add(flexAt0Trial1);
            trialBox.Items.Add(flexAt0Trial2);
            trialBox.Items.Add(flexAt0Trial3);
            trialBox.Items.Add(flexAt30LeftTrial1);
            trialBox.Items.Add(flexAt30LeftTrial2);
            trialBox.Items.Add(flexAt30LeftTrial3);
            trialBox.Items.Add(flexAt30RightTrial1);
            trialBox.Items.Add(flexAt30RightTrial2);
            trialBox.Items.Add(flexAt30RightTrial3);
            trialBox.Items.Add(spROMTrial);

            parameterBox.Items.Add(Angle);
            parameterBox.Items.Add(angVel);
            parameterBox.Items.Add(angAccel);
            parameterBox.Items.Add(angJerk);
        }

        private void GraphDisplayButton_Click(object sender, EventArgs e)
        {
            FunctionSeries data = new FunctionSeries();
            // Create the plot model
            List<float> angularTest = new List<float>();
            List<float> timeStampsAT = new List<float>();

            DataPlot.LegendPosition = LegendPosition.RightBottom;
            DataPlot.LegendPlacement = LegendPlacement.Outside;
            DataPlot.LegendOrientation = LegendOrientation.Horizontal;

            var Yaxis = new OxyPlot.Axes.LinearAxis();
            OxyPlot.Axes.LinearAxis XAxis = new OxyPlot.Axes.LinearAxis { Position = OxyPlot.Axes.AxisPosition.Bottom, Minimum = 0, Maximum = 100 };

            switch (trialBox.Text)
            {
                case flexAt0Trial1:
                    switch(parameterBox.Text)
                    {
                        case Angle:
                            DataPlot = new PlotModel { Title = "Flex At 0 Deg Trial 0 Angle" };
                            angularTest = ApplicationState.dataAnalysis.imuData.flexAnglesAt0T1;
                            timeStampsAT = ApplicationState.dataAnalysis.imuData.transposedTSMidAt0T1;
                            XAxis.Title = "Time (s)";
                            Yaxis.Title = "Angle (degrees)";
                            break;
                        case angVel:
                            DataPlot = new PlotModel { Title = "Flex At 0 Deg Trial 0 Anglular Velocity" };
                            angularTest = ApplicationState.dataAnalysis.angularFlexVelAt0T1;
                            timeStampsAT = ApplicationState.dataAnalysis.imuData.transposedTSMidAt0T1;
                            XAxis.Title = "Time (s)";
                            Yaxis.Title = "Anglular Velocity (degrees/sec)";
                            break;
                        case angAccel:
                            DataPlot = new PlotModel { Title = "Flex At 0 Deg Trial 0 Anglular Acceleration" };
                            angularTest = ApplicationState.dataAnalysis.angularFlexAccelAt0T1;
                            timeStampsAT = ApplicationState.dataAnalysis.imuData.transposedTSMidAt0T1;
                            XAxis.Title = "Time (s)";
                            Yaxis.Title = "Anglular Acceleration (degrees/sec)";
                            break;
                        case angJerk:
                            DataPlot = new PlotModel { Title = "Flex At 0 Deg Trial 0 Anglular Jerk" };
                            angularTest = ApplicationState.dataAnalysis.angularFlexAccelAt0T1;
                            timeStampsAT = ApplicationState.dataAnalysis.imuData.transposedTSMidAt0T1;
                            XAxis.Title = "Time (s)";
                            Yaxis.Title = "Anglular Jerk (degrees/sec)";
                            break;
                    }
                    break;
            }
            for (int x = 0; x < angularTest.Count; ++x)
            {
                DataPoint dataPoint = new DataPoint(timeStampsAT[x], angularTest[x]);
                data.Points.Add(dataPoint);
            }

            DataPlot.Series.Add(data);
            DataPlot.Axes.Add(Yaxis);
            DataPlot.Axes.Add(XAxis);
            this.plot.Model = DataPlot;
        }
    }
}
