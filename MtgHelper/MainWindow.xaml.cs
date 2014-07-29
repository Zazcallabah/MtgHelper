using System.Diagnostics;
using System.Windows;

namespace MtgHelper
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{


		public MainWindow()
		{
			DataContext = new Controller();
			InitializeComponent();
			Calibrate();
		}

		public void Calibrate()
		{
			var controller = new CalibrationController();
			var search = new Calibrator { Controller = controller };
			var result = search.ShowDialog();
			if( !result.HasValue || !result.Value )
				Close();

			( (Controller) DataContext ).Init( new CalibrationObject( controller ) );
		}

		void ButtonBase_OnClick( object sender, RoutedEventArgs e )
		{
			Calibrate();
		}
	}
}
