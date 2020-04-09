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
        public static readonly DependencyProperty XProperty = DependencyProperty.Register("X", typeof(double), typeof(Joystick), 
            new FrameworkPropertyMetadata(125.0, FrameworkPropertyMetadataOptions.AffectsRender, onXChanged));
        public static readonly DependencyProperty YProperty = DependencyProperty.Register("Y", typeof(double), typeof(Joystick),
            new FrameworkPropertyMetadata(125.0, FrameworkPropertyMetadataOptions.AffectsRender, onYChanged));

        public event EventHandler Moved;

        private static void onXChanged(DependencyObject JS, DependencyPropertyChangedEventArgs eventArgs)
        {
            (JS as Joystick).knobPosition.X = (double) eventArgs.NewValue;
        }

        private static void onYChanged(DependencyObject JS, DependencyPropertyChangedEventArgs eventArgs)
        {
            (JS as Joystick).knobPosition.Y = (double)eventArgs.NewValue;
        }

        public double X
        {
            get { return Convert.ToDouble(GetValue(XProperty)); }
            set
            {
                SetValue(XProperty, value);
                knobPosition.X = X;
            }
        }

        public double Y
        {
            get { return Convert.ToDouble(GetValue(YProperty)); }
            set
            {
                SetValue(YProperty, value);
                knobPosition.Y = Y;
            }
        }

        private readonly Storyboard centerKnob;
        private bool isMouseDownOnKnobBase = false;
        private double startXFromCenter = 0;
        private double startYFromCenter = 0;
        private Point baseCenter;

        public Joystick()
        {
            InitializeComponent();
            baseCenter = new Point(Base.Width / 2 - KnobBase.Width / 2, Base.Height / 2 - KnobBase.Height / 2);
            Base.MouseMove += Base_MouseMove;
            Knob.MouseDown += Knob_MouseDown;
            Base.MouseUp += Base_MouseUp;
            centerKnob = Knob.Resources["CenterKnob"] as Storyboard;
            X = Convert.ToDouble(GetValue(XProperty));
            Y = Convert.ToDouble(GetValue(YProperty));
        }

        private void moveKnobToCenter()
        {
            centerKnob.Begin();
            X = baseCenter.X;
            Y = baseCenter.Y;
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
                    X = newX;
                    Y = newY;
                }
                else
                {
                    double vX = newX - baseCenter.X;
                    double vY = newY - baseCenter.Y;
                    double magV = Math.Sqrt(vX * vX + vY * vY);
                    X = baseCenter.X + vX / magV * maxDistanceFromCenter;
                    Y = baseCenter.Y + vY / magV * maxDistanceFromCenter;
                }

                double convertedX = convertPixelsToRange(X);
                double convertedY = convertPixelsToRange(Y);
                Moved(this, new JoystickEventArgs(convertedX, convertedY));
            }
        }

        private double convertPixelsToRange(double pixelPosition)
        {
            // TODO: convert by formula
            if (pixelPosition == Base.Width / 2 - KnobBase.Width / 2)
            {
                return 0;
            }

            return 1;
        }

        private void CenterKnob_Completed(object sender, EventArgs e)
        {
        }
    }
}
