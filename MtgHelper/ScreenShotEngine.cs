using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows;

namespace MtgHelper
{
	public enum RelevantWindows
	{
		Main,
		Play,
		Chat,
		Draft,
	}

	public class ImageStore
	{
		public ImageStore( Image bitmap, long crc, RelevantWindows window )
		{
			Image = bitmap;
			Crc = crc;
			Window = window;
		}

		public RelevantWindows Window { get; set; }
		public long Crc { get; set; }
		public Image Image { get; set; }
	}
	public class HandleStore
	{
		public HandleStore( IntPtr handleCandidate, RelevantWindows window )
		{
			Ptr = handleCandidate;
			Window = window;
		}

		public IntPtr Ptr { get; set; }
		public RelevantWindows Window { get; set; }
	}

	public class ScreenShotEngine
	{
		const int WaitPeriod = 1000;
		readonly ScreenCapture _sc;
		readonly Dictionary<long, RelevantWindows> _crcLookup = new Dictionary<long, RelevantWindows>
		{
			{559831290,RelevantWindows.Main}
		};
		readonly List<ImageStore> _images = new List<ImageStore>();
		readonly List<HandleStore> _relevantHandles = new List<HandleStore>();
		Thread _worker;
		bool _continue;


		public ScreenShotEngine()
		{
			_sc = new ScreenCapture();
		}

		public void Start()
		{
			_continue = true;
			_worker = new Thread( Loop );
			_worker.Start();
		}

		void Loop()
		{
			while( _continue )
			{

				var handleCandidate = _sc.GetHandle();
				if( _relevantHandles.All( r => r.Ptr != handleCandidate ) )
				{
					var test = _sc.CaptureScreen( handleCandidate );
					if( test != null )
					{
						//this cut may have to be something different
						var cut = test.Cut( 0, 0, 35, 24 );
						var crc = cut.Crc32();


						if( _crcLookup.ContainsKey( crc ) )
						{
							var window = _crcLookup[crc];
							Test( test, window );
							_relevantHandles.Add( new HandleStore( handleCandidate, window ) );
						}
					}
				}
				var badHandles = new List<HandleStore>();
				foreach( var h in _relevantHandles )
				{
					try
					{
						var img = _sc.CaptureScreen( h.Ptr );
						if( img != null )
							Test( img, h.Window );
					}
					catch( InvalidOperationException )
					{
						badHandles.Add( h );
					}
				}

				foreach( var h in badHandles )
				{
					_relevantHandles.Remove( h );
				}

				Wait( WaitPeriod );
			}
		}

		void Test( Bitmap b, RelevantWindows window )
		{
			var crc = b.Crc32();
			var store = _images.FirstOrDefault( i => i.Window == window );
			if( store != null )
			{
				if( store.Crc != crc )
				{
					store.Crc = crc;
					store.Image = b;
					InvokeImageChanged( window );
				}
			}
			else
			{
				_images.Add( new ImageStore( b, crc, window ) );
				InvokeImageChanged( window );
			}
		}

		void Wait( int ms )
		{
			var ts = TimeSpan.FromMilliseconds( ms );
			var mark = DateTime.Now;
			while( mark + ts > DateTime.Now && _continue )
			{
				Thread.Sleep( 50 );
			}
		}

		public void Stop()
		{
			_continue = false;
		}

		void InvokeImageChanged( RelevantWindows window )
		{
			Application.Current.Dispatcher.Invoke( delegate
			{
				if( !_continue )
					return;
				FireImageChanged( window );
			} );
		}

		public event EventHandler<ImageChangedEventArgs> ImageChanged;

		protected virtual void FireImageChanged( RelevantWindows window )
		{
			if( ImageChanged != null )
				ImageChanged( this, new ImageChangedEventArgs( window ) );
		}
	}

	public class ImageChangedEventArgs
	{
		public ImageChangedEventArgs( RelevantWindows window )
		{
			Window = window;
		}
		public RelevantWindows Window { get; private set; }
	}
}