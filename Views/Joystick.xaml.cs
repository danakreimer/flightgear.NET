using FlightgearSimulator.Utils;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlightgearSimulator.Views
{
    /// <summary>
    /// Interaction logic for Joystick.xaml
    /// </summary>
    public partial class Joystick : UserControl
    {
        private readonly Storyboard knobStoryBoard;
        private bool mouseDown = false;
        private double XInDragStart = 0;
        private double YInDragStart = 0;
        private Point baseCenter;
        private readonly int decimalDigits = 3;

        public event EventHandler Moved;

        public Joystick()
        {
            InitializeComponent();

            // Register mouse events
            Base.MouseMove += Base_MouseMove;
            Knob.MouseDown += Knob_MouseDown;
            Base.MouseUp += Base_MouseUp;

            // Set the knob to the center
            knobPosition.Y = 125;
            knobPosition.X = 125;

            // Get the knob story board from the resources
            knobStoryBoard = Knob.Resources["CenterKnob"] as Storyboard;

            // Initialize the center point
            baseCenter = new Point(125, 125);
        }

        private void MoveKnobToCenter()
        {
            // Start the story board animation
            knobStoryBoard.Begin();

            // Reset the knob to the center
            knobPosition.X = baseCenter.X;
            knobPosition.Y = baseCenter.Y;

            // Trigger the custom Moved event with the range (0, 0)
            Moved(this, new JoystickEventArgs(0, 0));
        }

        private void Base_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mouseDown = false;
            Knob.ReleaseMouseCapture();
            MoveKnobToCenter();
        }

        private void Knob_MouseDown(object sender, MouseButtonEventArgs e)
        {
            knobStoryBoard.Stop();
            mouseDown = true;
            Point mousePositionRelativeToKnob = e.MouseDevice.GetPosition(Knob);
            XInDragStart = KnobBase.Width / 2 - mousePositionRelativeToKnob.X;
            YInDragStart = KnobBase.Height / 2 - mousePositionRelativeToKnob.Y;

            // Capture mouse to catch the mousemove event even if the knob is outside the circle
            Knob.CaptureMouse();
        }

        private void Base_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                Point mousePosition = e.MouseDevice.GetPosition(Base);
                double circleRadius = Base.Width / 2;
                double maxDistanceFromCenter = circleRadius - KnobBase.Width / 2;

                double x = mousePosition.X - KnobBase.Width / 2 + XInDragStart;
                double y = mousePosition.Y - KnobBase.Height / 2 + YInDragStart;

                double deltaXSquared = Math.Pow(x - baseCenter.X, 2);
                double deltaYSquared = Math.Pow(y - baseCenter.Y, 2);
                double distanceFromCenter = Math.Sqrt(deltaXSquared + deltaYSquared);

                if (distanceFromCenter <= maxDistanceFromCenter)
                {
                    knobPosition.X = x;
                    knobPosition.Y = y;
                }
                else
                {
                    // If the distance from the center is greater than the max,
                    // set the x and y to the closest point on the Base circle
                    double deltaXCenter = x - baseCenter.X;
                    double deltaYCenter = y - baseCenter.Y;
                    double deltaXCenterSquared = Math.Pow(deltaXCenter, 2);
                    double deltaYCenterSquared = Math.Pow(deltaYCenter, 2);
                    double distanceFromInnerPoint = Math.Sqrt(deltaXCenterSquared + deltaYCenterSquared);
                    knobPosition.X = baseCenter.X + deltaXCenter / distanceFromInnerPoint * maxDistanceFromCenter;
                    knobPosition.Y = baseCenter.Y + deltaYCenter / distanceFromInnerPoint * maxDistanceFromCenter;
                }

                // Trigger the Moved custom event with the current X and Y converted to a -1 to 1 range
                Moved(this, this.PixelsToRange(knobPosition.X, knobPosition.Y));
            }
        }

        private JoystickEventArgs PixelsToRange(double x, double y)
        {
            double minX = 0;
            double minY = 0;
            double maxX = Base.Width - KnobBase.Width;
            double maxY = Base.Height - KnobBase.Height;
            double newMax = 1;
            double newMin = -1;

            double inRangeX = (((x - minX) * (newMax - newMin)) / (maxX - minX)) + newMin;
            double inRangeY = (((y - minX) * (newMax - newMin)) / (maxY - minY)) + newMin;
            inRangeY *= -1;
            inRangeX = Math.Round(inRangeX, decimalDigits, MidpointRounding.AwayFromZero);
            inRangeY = Math.Round(inRangeY, decimalDigits, MidpointRounding.AwayFromZero);
            return new JoystickEventArgs(inRangeX, inRangeY);
        }
    }
}
