using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Cache;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Point = System.Windows.Point;

namespace MtgHelper
{
	public class CalibrationController : INotifyPropertyChanged
	{
		bool _closeSignal;
		readonly Thread _capture;
		Image _img;

		public CalibrationController()
		{
			_closeSignal = false;
			Done = false;
			_capture = new Thread( RunCapture );
			_capture.Start();
		}

		public Point Down { get; set; }
		public Point Up { get; set; }

		public ImageSource Screen
		{
			get
			{
				lock( _capture )
				{
					var image = new BitmapImage();
					image.BeginInit();
					image.CacheOption = BitmapCacheOption.None;
					image.UriCachePolicy = new RequestCachePolicy( RequestCacheLevel.BypassCache );
					image.CacheOption = BitmapCacheOption.OnLoad;
					image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
					image.UriSource = new Uri( @"screen.png", UriKind.RelativeOrAbsolute );
					image.EndInit();
					return image;
				}
			}
		}

		public bool Done
		{
			get;
			private set;
		}

		public void Close()
		{
			_closeSignal = true;
		}

		void RunCapture()
		{
			while( !_closeSignal )
			{
				var sc = new ScreenCapture();
				_img = sc.CaptureScreen();
				lock( _capture )
				{
					_img.Save( "screen.png", ImageFormat.Png );
				}
				SetImage();
				Wait( 3000 );
			}
		}

		void Wait( int ms )
		{
			var ts = TimeSpan.FromMilliseconds( ms );
			var mark = DateTime.Now;
			while( mark + ts > DateTime.Now && !_closeSignal )
			{
				Thread.Sleep( 50 );
			}
		}

		void SetImage()
		{
			Application.Current.Dispatcher.Invoke( delegate
			{
				if( _closeSignal )
					return;
				FirePropertyChanged( "Screen" );
			} );
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void FirePropertyChanged( string propertyName )
		{
			if( PropertyChanged != null )
				PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
		}

		public void Adjust()
		{
			lock( _capture )
			{
				var u = Up;
				var d = Down;
				var tl = new Point( (int) Math.Min( u.X, d.X ), (int) Math.Min( u.Y, d.Y ) );
				var br = new Point( (int) Math.Max( u.X, d.X ), (int) Math.Max( u.Y, d.Y ) );

				var start = new Point( tl.X, tl.Y );
				var end = new Point( _img.Width, _img.Height );

				var b = (Bitmap) _img;

				// top left diagonal down right
				for( ; tl.X < br.X && tl.Y < br.Y; tl.X++, tl.Y++ )
				{
					if( b.GetPixel( (int) tl.X, (int) tl.Y ).IsGray() )
					{
						break;
					}
				}
				// up
				for( ; tl.X >= start.X; tl.X-- )
				{
					if( !b.GetPixel( (int) tl.X, (int) tl.Y ).IsGray() )
					{
						tl.X++;
						break;
					}
				}
				// left
				for( ; tl.Y >= start.Y; tl.Y-- )
				{
					if( !b.GetPixel( (int) tl.X, (int) tl.Y ).IsGray() )
					{
						tl.Y++;
						break;
					}
				}

				// br right
				for( ; br.X < end.X; br.X++ )
				{
					if( !b.GetPixel( (int) br.X, (int) br.Y ).IsGray() )
					{
						br.X--;
						break;
					}
				}
				// br down
				for( ; br.Y < end.Y; br.Y++ )
				{
					if( !b.GetPixel( (int) br.X, (int) br.Y ).IsGray() )
					{
						br.Y--;
						break;
					}
				}
				Down = tl;
				Up = br;
			}
			Done = true;
			FirePropertyChanged( "Done" );
		}
	}
}