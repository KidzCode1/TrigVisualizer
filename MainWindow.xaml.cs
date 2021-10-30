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
		public MainWindow()
		{
			InitializeComponent();
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

		double GetOppositeFromThetaAndAdjacent(double theta, double adjacent)
		{
			// TOA
			// adjacent * Tangent of theta = opposite.
			return adjacent * Tan(theta);
		}

		double GetHypotenuseFromThetaAndAdjacent(double theta, double adjacent)
		{
			// hypotenuse = adjacent / cos of theta 
			return adjacent / Cos(theta);
		}

		double GetThetaFromOppositeAndAdjacent(double opposite, double adjacent)
		{
			// TOA 
			// Tangent of theta = Opposite / Adjacent
			// ArcTan(opposite/Adjacent) = theta

			double thetaRadians = Math.Atan2(opposite, adjacent);
			return ToDegrees(thetaRadians);

		}

		private void btnSolve_Click(object sender, RoutedEventArgs e)
		{
			GetKnownValues();
			if (HasValue(adjacent))
			{
				if (HasValue(theta))
				{
					opposite = GetOppositeFromThetaAndAdjacent(theta, adjacent);
					hypotenuse = GetHypotenuseFromThetaAndAdjacent(theta, adjacent);

					tbxOpposite.Text = opposite.ToString();
					tbxHypotenuse.Text = hypotenuse.ToString();
					return;
				}


				if (HasValue(opposite))
				{
					theta = GetThetaFromOppositeAndAdjacent(opposite, adjacent);
					hypotenuse = GetHypotenuseFromThetaAndAdjacent(theta, adjacent);

					tbxHypotenuse.Text = hypotenuse.ToString(); 
					tbxTheta.Text = theta.ToString();
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
					}
				}
				else if (HasValue(adjacent))
				{
					
				}
			}
		}

		private void btnClearAll_Click(object sender, RoutedEventArgs e)
		{
			tbxAdjacent.Text = "";
			tbxOpposite.Text = "";
			tbxHypotenuse.Text = "";
			tbxTheta.Text = "";
		}
	}
}
