using System.ComponentModel;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows;

namespace MtgHelper
{
	class SaveImageFiles
	{
		readonly ScreenShotEngine _se;

		public SaveImageFiles( ScreenShotEngine se )
		{
			_se = se;
			se.ImageChanged += se_ImageChanged;
		}

		void se_ImageChanged( object sender, ImageChangedEventArgs e )
		{
			if( e.Window == RelevantWindows.Main )
			{
				var i = _se.Images().FirstOrDefault();
				if( i != null )
				{
					i.Image.Save( "tmp_" + System.IO.Path.GetRandomFileName().Substring( 0, 8 ) + ".png", ImageFormat.Png );
				}
			}
		}
	}

	class Controller : INotifyPropertyChanged
	{
		ScreenShotEngine _se;
		string[] _cards;

		public Controller( ScreenShotEngine se )
		{
			_se = se;
			_se.ImageChanged += _se_ImageChanged;
		}

		void _se_ImageChanged( object sender, ImageChangedEventArgs e )
		{
			var i = _se.Images();
			Cards = new[]{
			"imagecount: "+i.Length,
			
			};
		}

		public string[] Cards
		{
			get { return _cards; }
			private set
			{
				_cards = value;
				OnPropertyChanged( "Cards" );
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged( string propertyName )
		{
			var handler = PropertyChanged;
			if( handler != null )
				handler( this, new PropertyChangedEventArgs( propertyName ) );
		}
	}
	/// <summary>
	/// Interaction logic for Detector.xaml
	/// </summary>
	public partial class Detector : Window
	{


		public Detector()
		{
			var se = new ScreenShotEngine();
			DataContext = new Controller( se );
			new SaveImageFiles( se );
			InitializeComponent();
			se.Start();
		}

		//public void Calibrate()
		//{
		//	var controller = new CalibrationController();
		//	var search = new Calibrator { Controller = controller };
		//	var result = search.ShowDialog();
		//	if( !result.HasValue || !result.Value )
		//		Close();

		//	( (Controller) DataContext ).Init( new CalibrationObject( controller ) );
		//}

	}
}
