// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents the view-model for the main window.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.Samples.Kinect.BodyBasics
{
    using System;
    using System.Collections.Generic;
    using OxyPlot;
    using OxyPlot.Series;

    /// <summary>
    /// Represents the view-model for the main window.
    /// </summary>
    public class MainViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel" /> class.
        /// </summary>

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

        public List<DataPoint> Graph;

        public MainViewModel()
        {

            // Set the Model property, the INotifyPropertyChanged event will make the WPF Plot control update its content
        }

        public void plotGraph(string trialName, string parameterName)
        {
            Graph = new List<DataPoint>();
            Graph.Clear();
            if (trialName == flexAt0Trial1)
            {
                switch (parameterName)
                {
                    case Angle:
                        // Create the plot model
                        List<float> angularTest = ApplicationState.dataAnalysis.imuData.flexAnglesAt0T1;
                        List<float> timeStampsAT = ApplicationState.dataAnalysis.imuData.transposedTSMidAt0T1;
                        this.Title = "Trial 1 Flex Angle At 0";

                        for (int x = 0; x < angularTest.Count; ++x)
                        {
                            DataPoint newPoint = new DataPoint(timeStampsAT[x], angularTest[x]);
                            Graph.Add(newPoint);
                        }

                        this.Points = Graph;
                        break;
                    case angVel:

                        break;
                    case angAccel:

                        break;
                    case angJerk:

                        break;
                }
            }
            else if (trialName == flexAt0Trial2)
            {
                switch (parameterName)
                {
                    case Angle:
                        // Create the plot model
                        List<float> angularTest = ApplicationState.dataAnalysis.imuData.flexAnglesAt0T2;
                        List<float> timeStampsAT = ApplicationState.dataAnalysis.imuData.transposedTSMidAt0T2;
                        this.Title = "Trial 2 Flex Angle At 0";

                        for (int x = 0; x < angularTest.Count; ++x)
                        {
                            DataPoint newPoint = new DataPoint(timeStampsAT[x], angularTest[x]);
                            Graph.Add(newPoint);
                        }

                        this.Points = Graph;
                        break;
                    case angVel:

                        break;
                    case angAccel:

                        break;
                    case angJerk:

                        break;
                }
            }
            else if (trialName == flexAt0Trial3)
            {
                switch (parameterName)
                {
                    case Angle:
                        // Create the plot model
                        List<float> angularTest = ApplicationState.dataAnalysis.imuData.flexAnglesAt0T3;
                        List<float> timeStampsAT = ApplicationState.dataAnalysis.imuData.transposedTSMidAt0T3;
                        this.Title = "Trial 3 Flex Angle At 0";

                        for (int x = 0; x < angularTest.Count; ++x)
                        {
                            DataPoint newPoint = new DataPoint(timeStampsAT[x], angularTest[x]);
                            Graph.Add(newPoint);
                        }

                        this.Points = Graph;
                        break;
                    case angVel:

                        break;
                    case angAccel:

                        break;
                    case angJerk:

                        break;
                }
            }
            else if (trialName == flexAt30LeftTrial1)
            {
                switch (parameterName)
                {
                    case Angle:
                        // Create the plot model
                        List<float> angularTest = ApplicationState.dataAnalysis.imuData.flexAnglesAt0T3;
                        List<float> timeStampsAT = ApplicationState.dataAnalysis.imuData.transposedTSMidAt0T3;
                        this.Title = "Trial 1 Flex Angle At 30 Left";

                        for (int x = 0; x < angularTest.Count; ++x)
                        {
                            DataPoint newPoint = new DataPoint(timeStampsAT[x], angularTest[x]);
                            Graph.Add(newPoint);
                        }

                        this.Points = Graph;
                        break;
                    case angVel:

                        break;
                    case angAccel:

                        break;
                    case angJerk:

                        break;
                }
            }
            else if (trialName == flexAt30LeftTrial2)
            {
                switch (parameterName)
                {
                    case Angle:
                        // Create the plot model
                        List<float> angularTest = ApplicationState.dataAnalysis.imuData.flexAnglesAt0T3;
                        List<float> timeStampsAT = ApplicationState.dataAnalysis.imuData.transposedTSMidAt0T3;
                        this.Title = "Trial 2 Flex Angle At 30 Left";

                        for (int x = 0; x < angularTest.Count; ++x)
                        {
                            DataPoint newPoint = new DataPoint(timeStampsAT[x], angularTest[x]);
                            Graph.Add(newPoint);
                        }

                        this.Points = Graph;
                        break;
                    case angVel:

                        break;
                    case angAccel:

                        break;
                    case angJerk:

                        break;
                }
            }
            else if (trialName == flexAt30LeftTrial3)
            {
                switch (parameterName)
                {
                    case Angle:
                        // Create the plot model
                        List<float> angularTest = ApplicationState.dataAnalysis.imuData.flexAnglesAt0T3;
                        List<float> timeStampsAT = ApplicationState.dataAnalysis.imuData.transposedTSMidAt0T3;
                        this.Title = "Trial 3 Flex Angle At 30 Left";

                        for (int x = 0; x < angularTest.Count; ++x)
                        {
                            DataPoint newPoint = new DataPoint(timeStampsAT[x], angularTest[x]);
                            Graph.Add(newPoint);
                        }

                        this.Points = Graph;
                        break;
                    case angVel:

                        break;
                    case angAccel:

                        break;
                    case angJerk:

                        break;
                }
            }
            else if (trialName == flexAt30RightTrial1)
            {
                switch (parameterName)
                {
                    case Angle:
                        // Create the plot model
                        List<float> angularTest = ApplicationState.dataAnalysis.imuData.flexAnglesAt0T3;
                        List<float> timeStampsAT = ApplicationState.dataAnalysis.imuData.transposedTSMidAt0T3;
                        this.Title = "Trial 1 Flex Angle At 30 Right";

                        for (int x = 0; x < angularTest.Count; ++x)
                        {
                            DataPoint newPoint = new DataPoint(timeStampsAT[x], angularTest[x]);
                            Graph.Add(newPoint);
                        }

                        this.Points = Graph;
                        break;
                    case angVel:

                        break;
                    case angAccel:

                        break;
                    case angJerk:

                        break;
                }
            }
            else if (trialName == flexAt30RightTrial2)
            {
                switch (parameterName)
                {
                    case Angle:
                        // Create the plot model
                        List<float> angularTest = ApplicationState.dataAnalysis.imuData.flexAnglesAt0T3;
                        List<float> timeStampsAT = ApplicationState.dataAnalysis.imuData.transposedTSMidAt0T3;
                        this.Title = "Trial 2 Flex Angle At 30 Right";

                        for (int x = 0; x < angularTest.Count; ++x)
                        {
                            DataPoint newPoint = new DataPoint(timeStampsAT[x], angularTest[x]);
                            Graph.Add(newPoint);
                        }

                        this.Points = Graph;
                        break;
                    case angVel:

                        break;
                    case angAccel:

                        break;
                    case angJerk:

                        break;
                }
            }
            else if (trialName == flexAt30RightTrial3)
            {
                switch (parameterName)
                {
                    case Angle:
                        // Create the plot model
                        List<float> angularTest = ApplicationState.dataAnalysis.imuData.flexAnglesAt0T3;
                        List<float> timeStampsAT = ApplicationState.dataAnalysis.imuData.transposedTSMidAt0T3;
                        this.Title = "Trial 3 Flex Angle At 30 Right";

                        for (int x = 0; x < angularTest.Count; ++x)
                        {
                            DataPoint newPoint = new DataPoint(timeStampsAT[x], angularTest[x]);
                            Graph.Add(newPoint);
                        }

                        this.Points = Graph;
                        break;
                    case angVel:

                        break;
                    case angAccel:

                        break;
                    case angJerk:

                        break;
                }
            }
            else if (trialName == spROMTrial)
            {
                switch (parameterName)
                {
                    case Angle:
                        // Create the plot model
                        List<float> angularTest = ApplicationState.dataAnalysis.imuData.flexAnglesAt0T3;
                        List<float> timeStampsAT = ApplicationState.dataAnalysis.imuData.transposedTSMidAt0T3;
                        this.Title = "Sagittal Angle";

                        for (int x = 0; x < angularTest.Count; ++x)
                        {
                            DataPoint newPoint = new DataPoint(timeStampsAT[x], angularTest[x]);
                            Graph.Add(newPoint);
                        }

                        this.Points = Graph;
                        break;
                    case angVel:

                        break;
                    case angAccel:

                        break;
                    case angJerk:

                        break;
                }
            }
        }

        public string Title { get; set; }

        public IList<DataPoint> Points { get; set; }
    }
}