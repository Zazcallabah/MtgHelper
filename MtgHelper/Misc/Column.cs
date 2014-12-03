using System;
using System.Collections.Generic;
using System.Drawing;

namespace MtgHelper.Misc
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

		public CardImage[] GetCards()
		{
			double cardwidth = _topright.X - _topleft.X;

			var l = new List<CardImage> { Make( _topleft.Y, cardwidth ) };
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

		CardImage Make( int y, double cardwidth )
		{
			const double fracHeight = 0.067;
			const double fracWidth = 0.80;
			const double fracXpad = 0.075;
			const double fracYpad = 0.072;

			var cardtlX = (int) Math.Floor( _topleft.X + fracXpad * cardwidth );
			var cardtlY = (int) Math.Floor( y + fracYpad * cardwidth );
			var cardbrX = (int) Math.Floor( cardtlX + cardwidth * fracWidth );
			var cardbrY = (int) Math.Floor( cardtlY + cardwidth * fracHeight );
			return new CardImage( _b )
			{
				TopLeft = new Point( cardtlX, cardtlY ),
				BottomRight = new Point( cardbrX, cardbrY )
			};
		}
	}
}