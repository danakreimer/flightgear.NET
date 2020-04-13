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
        private readonly Storyboard centerKnob;
        private bool isMouseDownOnKnobBase = false;
        private double startXFromCenter = 0;
        private double startYFromCenter = 0;
        private Point baseCenter;

        public event EventHandler Moved;

        public Joystick()
        {
            InitializeComponent();
            baseCenter = new Point(Base.Width / 2 - KnobBase.Width / 2, Base.Height / 2 - KnobBase.Height / 2);
            Base.MouseMove += Base_MouseMove;
            Knob.MouseDown += Knob_MouseDown;
            Base.MouseUp += Base_MouseUp;
            centerKnob = Knob.Resources["CenterKnob"] as Storyboard;
            knobPosition.Y = 125;
            knobPosition.X = 125;
        }

        private void moveKnobToCenter()
        {
            centerKnob.Begin();
            knobPosition.X = baseCenter.X;
            knobPosition.Y = baseCenter.Y;
            Moved(this, new JoystickEventArgs(0, 0));
        }

        private void Base_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isMouseDownOnKnobBase = false;
            Knob.ReleaseMouseCapture();
            moveKnobToCenter();
        }

        private void Knob_MouseDown(object sender, MouseButtonEventArgs e)
        {
            centerKnob.Stop();
            isMouseDownOnKnobBase = true;
            Point mousePositionRelativeToKnob = e.MouseDevice.GetPosition(Knob);
            startXFromCenter = KnobBase.Width / 2 - mousePositionRelativeToKnob.X;
            startYFromCenter = KnobBase.Height / 2 - mousePositionRelativeToKnob.Y;
            Knob.CaptureMouse();
        }

        private void Base_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDownOnKnobBase)
            {
                Point mousePosition = e.MouseDevice.GetPosition(Base);
                double newX = mousePosition.X - KnobBase.Width / 2 + startXFromCenter;
                double newY = mousePosition.Y - KnobBase.Height / 2 + startYFromCenter;
                double circleRadius = Base.Width / 2;
                double distX = newX - baseCenter.X;
                double distY = newY - baseCenter.Y;

                double result = Math.Sqrt(distX * distX + distY * distY);

                double maxDistanceFromCenter = circleRadius - KnobBase.Width / 2;
                if (result <= maxDistanceFromCenter)
                {
                    knobPosition.X = newX;
                    knobPosition.Y = newY;
                }
                else
                {
                    double vX = newX - baseCenter.X;
                    double vY = newY - baseCenter.Y;
                    double magV = Math.Sqrt(vX * vX + vY * vY);
                    knobPosition.X = baseCenter.X + vX / magV * maxDistanceFromCenter;
                    knobPosition.Y = baseCenter.Y + vY / magV * maxDistanceFromCenter;
                }

                Moved(this, this.pixelsToRange(knobPosition.X, knobPosition.Y));
            }
        }

        private JoystickEventArgs pixelsToRange(double x, double y)
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
            return new JoystickEventArgs(inRangeX, inRangeY);
        }
    }
}
