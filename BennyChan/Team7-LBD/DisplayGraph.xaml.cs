using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Graph
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class DisplayGraph : Window
    {
        public DisplayGraph()
        {
            InitializeComponent();
        }

        // The data.
        private PointCollection[] DataPoints = new PointCollection[2];
        private Brush[] DataBrushes = { Brushes.Red, Brushes.Blue, Brushes.Green };

        // To mark a clicked point.
        private Ellipse DataEllipse = null;
        private Label DataLabel = null;

        // Draw a simple graph.
        // Draw a simple graph.
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            double wxmin = -10;
            double wxmax = 100;
            double wymin = -1;
            double wymax = 11;
            const double xstep = 10;
            const double ystep = 1;

            const double dmargin = 10;
            double dxmin = dmargin;
            double dxmax = canGraph.Width - dmargin;
            double dymin = dmargin;
            double dymax = canGraph.Height - dmargin;

            // Prepare the transformation matrices.
            PrepareTransformations(
                wxmin, wxmax, wymin, wymax,
                dxmin, dxmax, dymax, dymin);

            // Get the tic mark lengths.
            Point p0 = DtoW(new Point(0, 0));
            Point p1 = DtoW(new Point(5, 5));
            double xtic = p1.X - p0.X;
            double ytic = p1.Y - p0.Y;

            // Make the X axis.
            GeometryGroup xaxis_geom = new GeometryGroup();
            p0 = new Point(wxmin, 0);
            p1 = new Point(wxmax, 0);
            xaxis_geom.Children.Add(new LineGeometry(WtoD(p0), WtoD(p1)));

            for (double x = xstep; x <= wxmax - xstep; x += xstep)
            {
                // Add the tic mark.
                Point tic0 = WtoD(new Point(x, -ytic));
                Point tic1 = WtoD(new Point(x, ytic));
                xaxis_geom.Children.Add(new LineGeometry(tic0, tic1));

                // Label the tic mark's X coordinate.
                DrawText(canGraph, x.ToString(),
                    new Point(tic0.X, tic0.Y + 5), 0, 12,
                    HorizontalAlignment.Center,
                    VerticalAlignment.Top);
            }

            Path xaxis_path = new Path();
            xaxis_path.StrokeThickness = 1;
            xaxis_path.Stroke = Brushes.Black;
            xaxis_path.Data = xaxis_geom;

            canGraph.Children.Add(xaxis_path);

            // Make the Y axis.
            GeometryGroup yaxis_geom = new GeometryGroup();
            p0 = new Point(0, wymin);
            p1 = new Point(0, wymax);
            xaxis_geom.Children.Add(new LineGeometry(WtoD(p0), WtoD(p1)));

            for (double y = ystep; y <= wymax - ystep; y += ystep)
            {
                // Add the tic mark.
                Point tic0 = WtoD(new Point(-xtic, y));
                Point tic1 = WtoD(new Point(xtic, y));
                xaxis_geom.Children.Add(new LineGeometry(tic0, tic1));

                // Label the tic mark's Y coordinate.
                DrawText(canGraph, y.ToString(),
                    new Point(tic0.X - 10, tic0.Y), -90, 12,
                    HorizontalAlignment.Center,
                    VerticalAlignment.Center);
            }

            Path yaxis_path = new Path();
            yaxis_path.StrokeThickness = 1;
            yaxis_path.Stroke = Brushes.Black;
            yaxis_path.Data = yaxis_geom;

            canGraph.Children.Add(yaxis_path);

            // Make some data sets.
            Random rand = new Random();
            for (int data_set = 0; data_set < 2; data_set++)
            {
                double last_y = rand.Next(2, 7);

                DataPoints[data_set] = new PointCollection();
                for (double x = 0; x <= 100; x += 10)
                {
                    last_y += rand.Next(-10, 10) / 10.0;
                    if (last_y < 0) last_y = 0;
                    if (last_y > 10) last_y = 10;
                    Point p = new Point(x, last_y);
                    DataPoints[data_set].Add(WtoD(p));
                }

                Polyline polyline = new Polyline();
                polyline.StrokeThickness = 1;
                polyline.Stroke = DataBrushes[data_set];
                polyline.Points = DataPoints[data_set];

                canGraph.Children.Add(polyline);
            }

            // Make a title
            Point title_location = WtoD(new Point(50, 10));
            DrawText(canGraph, "Graph",
                title_location, 0, 20,
                HorizontalAlignment.Center,
                VerticalAlignment.Top);
        }

        // Prepare values for perform transformations.
        private Matrix WtoDMatrix, DtoWMatrix;
        private void PrepareTransformations(
            double wxmin, double wxmax, double wymin, double wymax,
            double dxmin, double dxmax, double dymin, double dymax)
        {
            // Make WtoD.
            WtoDMatrix = Matrix.Identity;
            WtoDMatrix.Translate(-wxmin, -wymin);

            double xscale = (dxmax - dxmin) / (wxmax - wxmin);
            double yscale = (dymax - dymin) / (wymax - wymin);
            WtoDMatrix.Scale(xscale, yscale);

            WtoDMatrix.Translate(dxmin, dymin);

            // Make DtoW.
            DtoWMatrix = WtoDMatrix;
            DtoWMatrix.Invert();
        }

        // Transform a point from world to device coordinates.
        private Point WtoD(Point point)
        {
            return WtoDMatrix.Transform(point);
        }

        // Transform a point from device to world coordinates.
        private Point DtoW(Point point)
        {
            return DtoWMatrix.Transform(point);
        }

        // Position a label at the indicated point.
        private void DrawText(Canvas can, string text,
            Point location, double angle, double font_size,
            HorizontalAlignment halign, VerticalAlignment valign)
        {
            // Make the label.
            Label label = new Label();
            label.Content = text;
            label.FontSize = font_size;
            can.Children.Add(label);

            // Rotate if desired.
            if (angle != 0) label.LayoutTransform = new RotateTransform(angle);

            // Position the label.
            label.Measure(new Size(double.MaxValue, double.MaxValue));

            double x = location.X;
            if (halign == HorizontalAlignment.Center)
                x -= label.DesiredSize.Width / 2;
            else if (halign == HorizontalAlignment.Right)
                x -= label.DesiredSize.Width;
            Canvas.SetLeft(label, x);

            double y = location.Y;
            if (valign == VerticalAlignment.Center)
                y -= label.DesiredSize.Height / 2;
            else if (valign == VerticalAlignment.Bottom)
                y -= label.DesiredSize.Height;
            Canvas.SetTop(label, y);
        }

        // See if the mouse is over a data point.
        private void canGraph_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // Find the data point at the mouse's location.
            Point mouse_location = e.GetPosition(canGraph);
            int data_set, point_number;
            FindDataPoint(mouse_location, out data_set, out point_number);
            if (data_set < 0) return;
            Point data_point = DataPoints[data_set][point_number];

            // Make the data ellipse if we haven't already.
            if (DataEllipse == null)
            {
                DataEllipse = new Ellipse();
                DataEllipse.Fill = null;
                DataEllipse.StrokeThickness = 1;
                DataEllipse.Width = 7;
                DataEllipse.Height = 7;
                canGraph.Children.Add(DataEllipse);
            }

            // Color and position the ellipse.
            DataEllipse.Stroke = DataBrushes[data_set];
            Canvas.SetLeft(DataEllipse, data_point.X - 2);
            Canvas.SetTop(DataEllipse, data_point.Y - 2);

            // Make the data label if we haven't already.
            if (DataLabel == null)
            {
                DataLabel = new Label();
                DataLabel.FontSize = 12;
                canGraph.Children.Add(DataLabel);
            }

            // Convert the data values back into world coordinates.
            Point world_point = DtoW(data_point);

            // Set the data label's text and position it.
            DataLabel.Content = "(" +
                world_point.X.ToString("0.0") + ", " +
                world_point.Y.ToString("0.0") + ")";
            DataLabel.Measure(new Size(double.MaxValue, double.MaxValue));
            Canvas.SetLeft(DataLabel, data_point.X + 4);
            Canvas.SetTop(DataLabel, data_point.Y - DataLabel.DesiredSize.Height);
        }

        // Change the mouse cursor appropriately.
        private void canGraph_MouseMove(object sender, MouseEventArgs e)
        {
            // Find the data point at the mouse's location.
            Point mouse_location = e.GetPosition(canGraph);
            int data_set, point_number;
            FindDataPoint(mouse_location, out data_set, out point_number);

            // Display the appropriate cursor.
            if (data_set < 0)
                canGraph.Cursor = null;
            else
                canGraph.Cursor = Cursors.UpArrow;
        }

        // Find the data point at this device coordinate location.
        // Return data_set = -1 if there is no point at this location.
        private void FindDataPoint(Point location, out int data_set, out int point_number)
        {
            // Check each data set.
            for (data_set = 0; data_set < DataPoints.Length; data_set++)
            {
                // Check this data set.
                for (point_number = 0;
                    point_number < DataPoints[data_set].Count;
                    point_number++)
                {
                    // See how far the location is from the data point.
                    Point data_point = DataPoints[data_set][point_number];
                    Vector vector = location - data_point;
                    double dist = vector.Length;
                    if (dist < 2) return;
                }
            }

            // We didn't find a point at this location.
            data_set = -1;
            point_number = -1;
        }
    }
}
