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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TrigVisualizer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		const double DBL_GraphLeft = 800;
		public MainWindow()
		{
			InitializeComponent();
			DrawSinFunction();
			DrawCosFunction();
			DrawSinPlusCosFunction();
		}

		void AddDot(double x, double y, Color color)
		{
			Ellipse ellipse = new Ellipse();
			const double diameter = 5;
			const double radius = diameter / 2.0;
			ellipse.Width = diameter;
			ellipse.Height = diameter;
			ellipse.Fill = new SolidColorBrush(color);
			Canvas.SetLeft(ellipse, x + DBL_GraphLeft - radius);
			Canvas.SetTop(ellipse, y - radius);
			cvsMain.Children.Add(ellipse);
		}

		void DrawSinFunction()
		{
			for (double x = 0.1; x < 90; x += 0.1)  // 0.1 to 89.9
			{
				double y = 200 + 400 * (1 - Sin(x));  // Expected to be between 0 and 1
				double multiplier = 400 / 90;
				AddDot(x * multiplier, y, Colors.Red);
			}
		}

		void DrawCosFunction()
		{
			for (double x = 0.1; x < 90; x += 0.1)  // 0.1 to 89.9
			{
				double y = 200 + 400 * (1 - Cos(x));  // Expected to be between 0 and 1
				double multiplier = 400 / 90;
				AddDot(x * multiplier, y, Colors.Blue);
			}
		}

		void DrawSinPlusCosFunction()
		{
			for (double x = 0.1; x < 90; x += 0.1)  // 0.1 to 89.9
			{
				double y = -200 + 400 * (2 - (Cos(x) + Sin(x)));  // Expected to be between 0 and 1
				double multiplier = 400 / 90;
				AddDot(x * multiplier, y, Colors.Purple);
			}
		}

		void DrawTheta()
		{
			Line line = new Line();
			double multiplier = 400 / 90;
			double xOffset = Canvas.GetLeft(brdPreview);
			line.X1 = DBL_GraphLeft + theta * multiplier - xOffset;
			line.X2 = DBL_GraphLeft + theta * multiplier - xOffset;
			line.Y1 = 0;
			line.Y2 = 800;  // I know magic numbers stink, but time is shortening.
			
			line.Stroke = new SolidColorBrush(Colors.Black);
			cvsPreview.Children.Add(line);
		}

		bool changingManually;

		private void tbxOpposite_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (changingManually)
				return;
		}

		private void tbxHypotenuse_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (changingManually)
				return;
		}

		private void tbxTheta_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (changingManually)
				return;
		}

		private void tbxAdjacent_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (changingManually)
				return;
		}

		double adjacent;
		double opposite;
		double hypotenuse;
		double theta;

		void GetKnownValues()
		{
			ParseValue(tbxAdjacent, ref adjacent);
			ParseValue(tbxOpposite, ref opposite);
			ParseValue(tbxHypotenuse, ref hypotenuse);
			ParseValue(tbxTheta, ref theta);
		}

		// ref allows me to change numbers inside the method and see those changes outside.
		private void ParseValue(TextBox textBox, ref double part)
		{
			if (!double.TryParse(textBox.Text, out part))
				part = -1;  // We are using this like a secret code to indicate that the part has no value.
		}

		bool HasValue(double part)
		{
			if (part == -1)
				return false;
			return true;
		}

		private static double ToDegrees(double radians)
		{
			return radians * 180 / Math.PI;
		}

		double ToRadians(double degrees)
		{
			return Math.PI * degrees / 180;
		}

		double Tan(double degrees)
		{
			return Math.Tan(ToRadians(degrees));
		}

		double Cos(double degrees)
		{
			return Math.Cos(ToRadians(degrees));
		}

		double Sin(double degrees)
		{
			return Math.Sin(ToRadians(degrees));
		}

		double GetOppositeFromThetaAndAdjacent(double thetaDegrees, double adjacent)
		{
			// TOA
			// adjacent * Tangent of theta = opposite.
			return adjacent * Tan(thetaDegrees);
		}

		double GetOppositeFromThetaAndHypotenuse(double thetaDegrees, double hypotenuse)
		{
			// Opposite = Sine(theta) * Hypotenuse			
			// SOH Sine(theta) = Opposite over Hypotenuse
			return Sin(thetaDegrees) * hypotenuse;

		}

		double GetAdjacentFromThetaAndHypotenuse(double thetaDegrees, double hypotenuse)
		{
			// CAH Cosine(theta) = Adjacent over Hypotenuse
			// Adjacent = Cosine(theta) * Hypotenuse
			return Cos(thetaDegrees) * hypotenuse;
		}

		double GetHypotenuseFromThetaAndAdjacent(double thetaDegrees, double adjacent)
		{
			// hypotenuse = adjacent / cos of theta 
			return adjacent / Cos(thetaDegrees);
		}

		/// <summary>
		/// Gets the hypotenuse from the theta and the opposite.
		/// </summary>
		/// <param name="thetaDegrees">The angle of theta, in degrees.</param>
		/// <param name="opposite">The length of the opposite side (across from theta)</param>
		double GetHypotenuseFromThetaAndOpposite(double thetaDegrees, double opposite)
		{
			// hypotenuse = opposite / sin of theta 
			return opposite / Sin(thetaDegrees);
		}
		double GetThetaFromOppositeAndAdjacent(double opposite, double adjacent)
		{
			// TOA 
			// Tangent of theta = Opposite / Adjacent
			// ArcTan(opposite/Adjacent) = theta

			double thetaRadians = Math.Atan2(opposite, adjacent);
			return ToDegrees(thetaRadians);

		}

		double GetThetaFromOppositeAndHypotenuse(double opposite, double hypotenuse)
		{
			// SOH Sine(theta) = Opposite over Hypotenuse
			// ArcSin(opposite/hypotenuse) = theta

			double thetaRadians = Math.Asin(opposite / hypotenuse);
			return ToDegrees(thetaRadians);

		}

		double GetThetaFromAdjacentAndHypotenuse(double adjacent, double hypotenuse)
		{
			// CAH Cosine(theta) = Adjacent over Hypotenuse
			// ArcCosine(adjacent/hypotenuse) = theta

			double thetaRadians = Math.Acos(adjacent / hypotenuse);
			return ToDegrees(thetaRadians);
		}

		void DrawPreviewTriangle()
		{
			//brdPreview.ActualHeight;
			const double margin = 10;
			double scaleFactorHorizontal = (brdPreview.ActualWidth - 2 * margin) / adjacent;
			double scaleFactorVertical = (brdPreview.ActualHeight - 2 * margin) / opposite;

			double scaleFactorToUse = Math.Min(scaleFactorHorizontal, scaleFactorVertical);
			Point bottomLeft = new Point(margin + 0, margin + opposite * scaleFactorToUse);
			Point bottomRight = new Point(margin + adjacent * scaleFactorToUse, margin + opposite * scaleFactorToUse);
			Point topRight = new Point(margin + adjacent * scaleFactorToUse, margin);

			Polygon triangle = new Polygon();
			triangle.Points.Add(bottomLeft);
			triangle.Points.Add(bottomRight);
			triangle.Points.Add(topRight);
			triangle.FillRule = FillRule.Nonzero;
			triangle.Fill = new SolidColorBrush(Color.FromArgb(93, 46, 133, 255));


			cvsPreview.Children.Clear();
			cvsPreview.Children.Add(triangle);
			AddPoint(bottomLeft);
			AddPoint(bottomRight);
			AddPoint(topRight);

			DrawTheta();
		}

		private void AddPoint(Point point)
		{
			Ellipse ellipse = new Ellipse();
			const double diameter = 5;
			const double radius = diameter / 2.0;
			ellipse.Width = diameter;
			ellipse.Height = diameter;
			ellipse.Fill = new SolidColorBrush(Colors.Red);
			Canvas.SetLeft(ellipse, point.X - radius);
			Canvas.SetTop(ellipse, point.Y - radius);
			cvsPreview.Children.Add(ellipse);
		}

		private void btnSolve_Click(object sender, RoutedEventArgs e)
		{
			Solve();
		}

		private void Solve()
		{
			tbDiscovery.Visibility = Visibility.Hidden;
			GetKnownValues();
			if (HasValue(adjacent))
			{
				if (HasValue(theta))
				{
					opposite = GetOppositeFromThetaAndAdjacent(theta, adjacent);
					hypotenuse = GetHypotenuseFromThetaAndAdjacent(theta, adjacent);

					tbxOpposite.Text = opposite.ToString();
					tbxHypotenuse.Text = hypotenuse.ToString();
					DrawPreviewTriangle();
					return;
				}


				if (HasValue(opposite))
				{
					theta = GetThetaFromOppositeAndAdjacent(opposite, adjacent);
					hypotenuse = GetHypotenuseFromThetaAndAdjacent(theta, adjacent);

					tbxHypotenuse.Text = hypotenuse.ToString();
					tbxTheta.Text = theta.ToString();
					DrawPreviewTriangle();
					return;
				}

			}


			if (HasValue(opposite))
			{
				if (HasValue(theta))
				{
					hypotenuse = GetHypotenuseFromThetaAndOpposite(theta, opposite);
					adjacent = GetAdjacentFromThetaAndHypotenuse(theta, hypotenuse);

					tbxAdjacent.Text = adjacent.ToString();
					tbxHypotenuse.Text = hypotenuse.ToString();
					DrawPreviewTriangle();
					return;
				}
			}


			if (HasValue(hypotenuse))
			{
				if (HasValue(opposite))
				{
					if (opposite >= hypotenuse)
					{
						tbStatus.Text = "Hypotenuse must be the longest side";
						return;
					}
					theta = GetThetaFromOppositeAndHypotenuse(opposite, hypotenuse);
					adjacent = GetAdjacentFromThetaAndHypotenuse(theta, hypotenuse);

					tbxAdjacent.Text = adjacent.ToString();
					tbxTheta.Text = theta.ToString();
					DrawPreviewTriangle();
					return;
				}
				else if (HasValue(adjacent))
				{
					if (adjacent >= hypotenuse)
					{
						tbStatus.Text = "Hypotenuse must be the longest side";
						return;
					}
					theta = GetThetaFromAdjacentAndHypotenuse(adjacent, hypotenuse);
					opposite = GetOppositeFromThetaAndAdjacent(theta, adjacent);

					tbxOpposite.Text = opposite.ToString();
					tbxTheta.Text = theta.ToString();
					DrawPreviewTriangle();
					return;
				}
				else if (HasValue(theta))
				{
					if (theta >= 90 || theta <= 0)
					{
						tbStatus.Text = "Theta must be between 0 and 90 degrees.";
						return;
					}
					adjacent = GetAdjacentFromThetaAndHypotenuse(theta, hypotenuse);
					opposite = GetOppositeFromThetaAndHypotenuse(theta, hypotenuse);

					tbxOpposite.Text = opposite.ToString();
					tbxAdjacent.Text = adjacent.ToString();
					DrawPreviewTriangle();
					return;
				}
			}
			tbDiscovery.Visibility = Visibility.Visible;
		}

		private void btnClearAll_Click(object sender, RoutedEventArgs e)
		{
			tbxAdjacent.Text = "";
			tbxOpposite.Text = "";
			tbxHypotenuse.Text = "";
			tbxTheta.Text = "";
		}

		private void sldTheta_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			if (tbxTheta == null || sldTheta == null)
				return;
			tbxTheta.Text = sldTheta.Value.ToString();
			Solve();
		}
	}
}
