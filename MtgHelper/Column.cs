using System;
using System.Collections.Generic;
using System.Drawing;

namespace MtgHelper
{
	public class Column
	{
		readonly Bitmap _b;
		readonly System.Drawing.Point _topleft;
		readonly System.Drawing.Point _topright;
		readonly System.Drawing.Point _maxbottomrightBoundary;

		public Column( Bitmap b, System.Drawing.Point? topleft, System.Drawing.Point? topright, System.Drawing.Point maxbottomrightBoundary )
		{
			_b = b;

			if( topleft == null )
				throw new Exception();
			_topleft = topleft.Value;

			if( topright == null )
			{
				_topright = new System.Drawing.Point( maxbottomrightBoundary.X, _topleft.Y );
			}
			else
				_topright = topright.Value;
			_maxbottomrightBoundary = maxbottomrightBoundary;
		}

		public Card[] GetCards()
		{
			var l = new List<Card>() { Make( _topleft.Y ) };
			bool inCard = true;
			int lastcard = _topleft.Y;
			double cardwidth = _topright.X - _topleft.X;

			for( int y = _topleft.Y; y < _maxbottomrightBoundary.Y; y++ )
			{
				var pix = _b.GetPixel( _topleft.X, y );
				if( inCard )
				{
					if( pix.IsGray() )
					{
						double cardheight = y - lastcard;
						var ratio = cardheight / cardwidth;
						if( ratio < 1.3834 || ratio > 1.397 )
							throw new Exception();
						inCard = false;
					}
				}
				else
				{
					if( !pix.IsGray() )
					{
						lastcard = y;
						l.Add( Make( y ) );
						inCard = true;
					}
				}
			}
			return l.ToArray();
		}

		Card Make( int y )
		{
			return new Card { TopLeft = new System.Drawing.Point( _topleft.X, y ), BottomRight = new System.Drawing.Point( _topright.X, y + 30 ) };
		}
	}
}