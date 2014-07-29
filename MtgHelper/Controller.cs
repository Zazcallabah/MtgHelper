using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows;

namespace MtgHelper
{
	public class Controller : IDisposable, INotifyPropertyChanged
	{
		readonly Dictionary<long, Bitmap> _imgs = new Dictionary<long, Bitmap>();
		CalibrationObject _calibration;
		Thread _capture;
		bool _closeSignal;

		public Controller()
		{
			_closeSignal = false;
		}

		public string Text
		{
			get { return _imgs.Count.ToString(); }
		}

		public void Dispose()
		{
			_closeSignal = true;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void Init( CalibrationObject c )
		{
			_calibration = c;
			_capture = new Thread( RunCapture );
			_capture.Start();
		}

		void RunCapture()
		{
			while( !_closeSignal )
			{
				var sc = new ScreenCapture();
				var img = sc.CaptureScreen() as Bitmap;
				var s = new Shot( img, _calibration.Topleft, _calibration.Bottomright );
				var c = s.GetCards();

				foreach( var ca in c )
				{
					var d = ca.Data();
					var hash = d.Crc32();
					if( !_imgs.ContainsKey( hash ) )
					{
						_imgs.Add( hash, d );
						Application.Current.Dispatcher.Invoke(
							delegate
							{
								if( _closeSignal )
									return;
								FirePropertyChanged( "Text" );
							} );
					}
				}

				Wait( 500 );
			}
		}

		void Wait( int ms )
		{
			var ts = TimeSpan.FromMilliseconds( ms );
			var mark = DateTime.Now;
			while( mark + ts > DateTime.Now && !_closeSignal )
				Thread.Sleep( 50 );
		}

		protected virtual void FirePropertyChanged( string propertyName )
		{
			if( PropertyChanged != null )
				PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
		}
	}
}