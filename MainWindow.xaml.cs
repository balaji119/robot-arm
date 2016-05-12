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

namespace RobotProb1 {
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window {
      public MainWindow () {
         InitializeComponent ();
         MakeArms ();
         RepositionArm ();
      }

      void MakeArms () {
         mCanvas.Children.Add (mUIArm1 = new Path () { Data = mArm1, StrokeThickness = 7, Stroke = Brushes.Gray });
         mCanvas.Children.Add (mUIArm2 = new Path () { Data = mArm2, StrokeThickness = 7, Stroke = Brushes.DarkGray });
         mCanvas.Children.Add (mUIPickBoxPointer = new Path () { Data = mPickBoxPointer, Stroke = Brushes.Black });
         mCanvas.Children.Add (mUIPickBoxCircle = new Path () { Data = mEllipse, Stroke = Brushes.Red, Fill = Brushes.Red });
         Canvas.SetLeft (mUIArm1, 200);
         Canvas.SetTop (mUIArm1, 200);
         Canvas.SetLeft (mUIArm2, 200);
         Canvas.SetTop (mUIArm2, 200);
         Canvas.SetLeft (mUIPickBoxPointer, 200);
         Canvas.SetTop (mUIPickBoxPointer, 200);
         Canvas.SetLeft (mUIPickBoxCircle, 200);
         Canvas.SetTop (mUIPickBoxCircle, 200);
      }

      /// <summary>
      /// Calculates the end point of arm1 and end point of pickBox pointer.
      /// We already know the start point of arm-1(Fixed point) and the end point of arm-2(i.e point p).
      /// </summary>
      /// <param name="p">End point of arm-2 (destination point of robot)</param>
      /// <returns>Arm1 end point</returns>
      Point Calaculate (Point p, double arm1Len = 100, double arm2Len = 100) {
         double distance = GetDisBtwPts (p, mArm1StartPt),     // Distace btw arm1-start point to arm2-endpoint.
            angle1 = GetSlopeAngle (mArm1StartPt, p), // Slope of arm 1
            angle2 = Math.Acos ((arm1Len * arm1Len + distance * distance - arm2Len * arm2Len) / (2 * distance * arm1Len)); //Angle between arm1 and line joining arm1Pt1 and point p. 
         p.X = arm1Len * Math.Cos (angle2 + angle1); p.Y = arm1Len * Math.Sin (angle2 + angle1); //Horizondal and vertical component of arm-1 gives the end point of arm-1.
         return p;
      }

      void RepositionArm () {
         if (GetDisBtwPts (mArm1StartPt, mArm2EndPt) < 200) {
            mArm1.StartPoint = mArm1StartPt; 
            mArm1.EndPoint = mArm2.StartPoint = mArm1EndPt; 
            mArm2.EndPoint = mArm2EndPt;
            mPickBoxPointer.StartPoint = mArm2EndPt;
            mPickBoxPointer.EndPoint = new Point (mArm2EndPt.X + 20 * Math.Cos (mPickAngle), mArm2EndPt.Y + 20 * Math.Sin (mPickAngle));
            mEllipse.Center = mArm2EndPt; 
            mEllipse.RadiusX = mEllipse.RadiusY = 10;
         }
      }

      double GetDisBtwPts (Point p1, Point p2) {
         return Math.Sqrt ((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
      }

      double GetSlopeAngle (Point p1, Point p2) {
         return Math.Atan2 ((p2.Y - p1.Y), (p2.X - p1.X));
      }

      void XSlider_ValueChanged (object sender, RoutedPropertyChangedEventArgs<double> e) {
         mXPos = mXSlider.Value;
         mArm1EndPt = Calaculate (mArm2EndPt);
         mArm2EndPt = new Point (mXPos, mYPos);
         RepositionArm ();
      }

      void YSlider_ValueChanged (object sender, RoutedPropertyChangedEventArgs<double> e) {
         mYPos = mYSlider.Value;
         mArm1EndPt = Calaculate (mArm2EndPt);
         mArm2EndPt = new Point (mXPos, mYPos);
         RepositionArm ();
      }

      void Theta_ValueChanged (object sender, RoutedPropertyChangedEventArgs<double> e) {
         mPickAngle = mTheta.Value;
         RepositionArm ();
      }

      double mXPos = 0, mYPos = 0, mPickAngle = 0;// Slider values
      Point mArm1StartPt = new Point (0, 0), mArm1EndPt = new Point (100, 0), mArm2EndPt = new Point (0, 0);
      LineGeometry mArm1 = new LineGeometry (), mArm2 = new LineGeometry (), mPickBoxPointer = new LineGeometry ();
      EllipseGeometry mEllipse = new EllipseGeometry ();
      UIElement mUIPickBoxPointer, mUIArm1, mUIArm2, mUIPickBoxCircle;
   }
}
