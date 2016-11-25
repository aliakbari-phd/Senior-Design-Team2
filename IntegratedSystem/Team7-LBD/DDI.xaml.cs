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

namespace Microsoft.Samples.Kinect.BodyBasics
{
    /// <summary>
    /// Interaction logic for DDI.xaml
    /// </summary>
    public partial class DDI : Window
    {
        public DDI()
        {
            InitializeComponent();
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

            trialBox.Items.Add(MainViewModel.flexAt0Trial1);
            trialBox.Items.Add(MainViewModel.flexAt0Trial2);
            trialBox.Items.Add(MainViewModel.flexAt0Trial3);
            trialBox.Items.Add(MainViewModel.flexAt30LeftTrial1);
            trialBox.Items.Add(MainViewModel.flexAt30LeftTrial2);
            trialBox.Items.Add(MainViewModel.flexAt30LeftTrial3);
            trialBox.Items.Add(MainViewModel.flexAt30RightTrial1);
            trialBox.Items.Add(MainViewModel.flexAt30RightTrial2);
            trialBox.Items.Add(MainViewModel.flexAt30RightTrial3);
            trialBox.Items.Add(MainViewModel.spROMTrial);

            parameterBox.Items.Add(MainViewModel.Angle);
            parameterBox.Items.Add(MainViewModel.angVel);
            parameterBox.Items.Add(MainViewModel.angAccel);
            parameterBox.Items.Add(MainViewModel.angJerk);
        }

        private void GraphDisplayButton_Click(object sender, EventArgs e)
        {
            MainViewModel Graph = new MainViewModel();
            Graph.plotGraph(trialBox.Text, parameterBox.Text);

        }
    }
}
