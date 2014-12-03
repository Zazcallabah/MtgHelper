using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MtgHelper.Misc
{
	/*	public class Controller : INotifyPropertyChanged
		{
			readonly ScreenShotEngine _engine;
			ScanArea _sa;
			CardCache _cache;
			string[] _cards;

			public Controller( ScreenShotEngine engine )
			{
				_engine = engine;
				_engine.PropertyChanged += _engine_PropertyChanged;
				_cache = new CardCache();
			}

			void _engine_PropertyChanged( object sender, PropertyChangedEventArgs e )
			{
				if( _sa == null )
				{
					var tmp = ScanAreaFactory.Get( _engine.Image );
					if( tmp == null )
						return;
					_sa = tmp;
				}
				var shot = new Shot( _engine.Image, _sa );
				var newcards = _cache.Parse( shot );
				UpdateCards( newcards );
			}

			void UpdateCards( string[] n )
			{
				if( Cards == null || n.Length != Cards.Length )
				{
					Cards = n;
					return;
				}

				for( int i = 0; i < n.Length; i++ )
				{
					if( n[i] != Cards[i] )
					{
						Cards = n;
						return;
					}
				}
			}

			public string[] Cards
			{
				get { return _cards; }
				private set { _cards = value; FirePropertyChanged( "Cards" ); }
			}

			public event PropertyChangedEventHandler PropertyChanged;

			protected virtual void FirePropertyChanged( string propertyName )
			{
				if( PropertyChanged != null )
					PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
			}
		}*/
}