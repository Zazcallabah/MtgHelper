using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MtgHelper
{
	public class Controller : IDisposable, INotifyPropertyChanged
	{
		Bitmap[] _imgs;
		Thread _capture;
		bool _disposed;
		string _tempfile;
		public Controller()
		{
			_disposed = false;
			//	_capture = new Thread( RunCapture );
			//	_capture.Start();
			var img = System.Drawing.Image.FromFile( @"C:\src\git\MtgHelper\MtgHelper\nozoom.bmp" );
			var b = new Bitmap( img );
			var tl = new System.Drawing.Point( 286, 121 );
			var br = new System.Drawing.Point( 286 + 1049, 121 + 433 );
			var s = new Shot( b, tl, br );
			var c = s.GetCards();
			var iml = new List<Bitmap>();
			foreach( var ca in c )
			{
				iml.Add( ca.Data() );
			}
			_imgs = ( iml.ToArray() );

		}

		public ObservableCollection<ImageSource> Images
		{
			get
			{
				var results = new ObservableCollection<ImageSource>();
				foreach( var card in _imgs )
				{
					var bitmapSource =
				System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
					card.GetHbitmap( System.Drawing.Color.Transparent ),
					IntPtr.Zero,
					new Int32Rect( 0, 0, card.Width, card.Height ),
					null );
					results.Add( bitmapSource );
				}
				return results;
			}
		}
		void RunCapture()
		{
			while( !_disposed )
			{
				ScreenCapture sc = new ScreenCapture();
				// capture entire screen, and save it to a file
				System.Drawing.Image img = sc.CaptureScreen();

				var oldBitmap =
					img as System.Drawing.Bitmap;// ??
				//					new System.Drawing.Bitmap( img );

				//SetImage( oldBitmap );
				Thread.Sleep( 1000 );

			}
		}

		void SetImage( System.Drawing.Bitmap[] oldBitmap )
		{
			Application.Current.Dispatcher.Invoke( (Action) ( delegate
			{


				//				Image = bitmapSource;
				FirePropertyChanged( "Image" );

			} ) );
		}


		public void Dispose()
		{
			_disposed = true;
		}

		public BitmapSource Image
		{
			get;
			private set;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void FirePropertyChanged( string propertyName )
		{
			if( PropertyChanged != null )
				PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
		}
	}
}