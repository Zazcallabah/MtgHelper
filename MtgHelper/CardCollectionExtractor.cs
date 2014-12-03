using System;
using System.Collections.Generic;
using System.Drawing;

namespace MtgHelper
{
	public class CardCollectionExtractor
	{
		readonly Bitmap _b;
		readonly ImageArea _area;

		public CardCollectionExtractor( Bitmap b, ImageArea area )
		{
			_b = b;
			_area = area;
		}

		Point FindTopLeftCorner( Point start )
		{
			return FindCorner( start, 1, 1 );
		}
		Point FindTopRightCorner( Point start )
		{
			return FindCorner( start, -1, 1 );
		}
		Point FindBottomLeftCorner( Point start )
		{
			return FindCorner( start, 1, -1 );
		}
		Point FindBottomRightCorner( Point start )
		{
			return FindCorner( start, -1, -1 );
		}

		Point FindCorner( Point start, int xFactor, int yFactor )
		{
			var init = new Point( start.X, start.Y + 1 );
			var p = TraverseXWhileBlack( init, -1 * xFactor );
			p = TraverseYWhileBlack( p, -1 * yFactor );
			p.Y += yFactor * 20;
			p = TraverseXWhileBlack( p, -1 * xFactor );
			p = TraverseYWhileBlack( p, -1 * yFactor );

			var x = p.X;

			p = TraverseDiagonalWhileBlack( p, xFactor, yFactor * -1 );

			p.X += xFactor * 20;
			p = TraverseYWhileBlack( p, -1 * yFactor );

			var y = p.Y;
			return new Point( x, y );
		}
		Point TraverseXWhileNotBlack( Point start, int direction = 1 )
		{
			var x = start.X;
			while( !_b.GetPixel( x, start.Y ).IsBlack() )
			{
				x += direction;
			}
			x -= direction;
			return new Point( x, start.Y );
		}

		Point TraverseYWhileNotBlack( Point start, int direction = 1 )
		{
			var y = start.Y;
			while( !_b.GetPixel( start.X, y ).IsBlack() )
			{
				y += direction;
			}
			y -= direction;
			return new Point( start.X, y );
		}
		Point TraverseXWhileBlack( Point start, int direction = 1 )
		{
			var x = start.X;
			while( _b.GetPixel( x, start.Y ).IsBlack() )
			{
				x += direction;
			}
			x -= direction;
			return new Point( x, start.Y );
		}

		Point TraverseYWhileBlack( Point start, int direction = 1 )
		{
			var y = start.Y;
			while( _b.GetPixel( start.X, y ).IsBlack() )
			{
				y += direction;
			}
			y -= direction;
			return new Point( start.X, y );
		}

		Point TraverseDiagonalWhileNotBlack( Point start, int xDirection = 1, int yDirection = 1 )
		{
			var x = start.X;
			var y = start.Y;
			while( !_b.GetPixel( x, y ).IsBlack() )
			{
				x += xDirection;
				y += yDirection;
			}
			y -= yDirection;
			x -= xDirection;
			return new Point( x, y );
		}
		Point TraverseDiagonalWhileBlack( Point start, int xDirection = 1, int yDirection = 1 )
		{
			var x = start.X;
			var y = start.Y;
			while( _b.GetPixel( x, y ).IsBlack() )
			{
				x += xDirection;
				y += yDirection;
			}
			y -= yDirection;
			x -= xDirection;
			return new Point( x, y );
		}

		public List<Bitmap> Cards()
		{
			var seed = new Point( _area.Topleft.X + 1, _area.Topleft.Y + 1 );
			seed = TraverseYWhileBlack( seed );
			seed.Y++;

			var test = TraverseXWhileNotBlack( seed );
			if( test.X < seed.X + 30 )
			{
				var tmp = new Point( test.X + 2, seed.Y );
				tmp = TraverseYWhileBlack( tmp );
				tmp.Y++;
				tmp = TraverseYWhileNotBlack( tmp );
				seed.Y = tmp.Y + 2;
			}

			seed = TraverseDiagonalWhileNotBlack( seed );
			seed.X++;
			seed.Y++;

			var cc1 = FindTopLeftCorner( seed );

			seed.X++;
			seed.Y++;

			var trstep = TraverseYWhileBlack( seed, -1 );
			var tr = TraverseXWhileBlack( trstep );

			var blstep = TraverseXWhileBlack( seed, -1 );
			var bl = TraverseYWhileBlack( blstep );

			var cctr = FindTopRightCorner( tr );
			var ccbl = FindBottomLeftCorner( bl );

			var dX = cctr.X - cc1.X + 1;
			var dY = ccbl.Y - cc1.Y + 1;

			var pX = TraverseXWhileNotBlack( new Point( cctr.X + 1, cctr.Y + ( dY / 2 ) ) );
			var pY = TraverseYWhileNotBlack( new Point( ccbl.X + ( dX / 2 ), ccbl.Y + 1 ) );
			var paddingX = pX.X - cctr.X;
			var paddingY = pY.Y - ccbl.Y;
			if( paddingX > 10 )
				paddingX = 10;
			if( paddingY > 10 )
				paddingY = 10;

			double totalX = _area.Bottomright.X - cc1.X;
			double totalY = _area.Bottomright.Y - cc1.Y;

			var colcount = (int) Math.Floor( totalX / ( dX + paddingX ) );
			var rowcount = (int) Math.Floor( totalY / ( dY + paddingY ) );

			var list = new List<Bitmap>();
			for( var r = 0; r < rowcount; r++ )
				for( var c = 0; c < colcount; c++ )
				{
					var cardtopleft = new Point( cc1.X + c * ( dX + paddingX ), cc1.Y + r * ( dY + paddingY ) );
					var cardbottomright = new Point( cardtopleft.X + dX, cardtopleft.Y + dY );

					var testpoint = new Point( cardtopleft.X, cardtopleft.Y + dY / 2 );
					var result = TraverseXWhileNotBlack( testpoint );
					if( result.X - testpoint.X < dX )
						list.Add( _b.Cut( cardtopleft, cardbottomright ) );
				}

			return list;
		}
	}
}