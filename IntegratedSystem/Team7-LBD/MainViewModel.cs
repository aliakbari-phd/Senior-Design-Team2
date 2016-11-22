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
        public MainViewModel()
        {
            // Create the plot model
            List<float> angularTest = ApplicationState.dataAnalysis.imuData.flexAnglesAt0T1;
            List<float> timeStampsAT = ApplicationState.dataAnalysis.imuData.transposedTSMidAt0T1;
            List<DataPoint> Graph = null;
            this.Title = "Example 2";
            
            for (int x = 0; x < angularTest.Count; ++x)
            {
                Graph.Add(new DataPoint(timeStampsAT[x], angularTest[x]));
            }

            this.Points = Graph;
            // Axes are created automatically if they are not defined

            // Set the Model property, the INotifyPropertyChanged event will make the WPF Plot control update its content
        }

        /// <summary>
        /// Gets the plot model.
        /// </summary>
        public string Title { get; set; }

        public IList<DataPoint> Points { get; set; }
    }
}