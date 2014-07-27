using System;
using System.Collections.Generic;
using System.Drawing;

namespace MtgHelper
{
	public class Column
	{
		readonly Bitmap _b;
		readonly Point _topleft;
		readonly Point _topright;
		readonly Point _maxbottomrightBoundary;

		public Column( Bitmap b, Point? topleft, Point? topright, Point maxbottomrightBoundary )
		{
			_b = b;

			if( topleft == null )
				throw new Exception();
			_topleft = topleft.Value;

			_topright = topright == null ? new Point( maxbottomrightBoundary.X, _topleft.Y ) : topright.Value;
			_maxbottomrightBoundary = maxbottomrightBoundary;
		}

		public Card[] GetCards()
		{
			double cardwidth = _topright.X - _topleft.X;

			var l = new List<Card> { Make( _topleft.Y, cardwidth ) };
			bool inCard = true;
			int lastcard = _topleft.Y;

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
						l.Add( Make( y, cardwidth ) );
						inCard = true;
					}
				}
			}
			return l.ToArray();
		}

		Card Make( int y, double cardwidth )
		{
			const double frac_height = 0.067;
			const double frac_width = 0.80;
			const double frac_xpad = 0.075;
			const double frac_ypad = 0.068;

			var cardtlX = (int) Math.Floor( _topleft.X + frac_xpad * cardwidth );
			var cardtlY = (int) Math.Floor( y + frac_ypad * cardwidth );
			var cardbrX = (int) Math.Floor( cardtlX + cardwidth * frac_width );
			var cardbrY = (int) Math.Floor( cardtlY + cardwidth * frac_height );
			return new Card( _b )
			{
				TopLeft = new Point( cardtlX, cardtlY ),
				BottomRight = new Point( cardbrX, cardbrY )
			};
		}
	}
}