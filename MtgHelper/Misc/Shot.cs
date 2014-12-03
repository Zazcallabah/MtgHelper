using System;
using System.Collections.Generic;
using System.Drawing;

namespace MtgHelper.Misc
{
	public class Shot
	{
		readonly Bitmap _b;
		readonly Point _topleft;
		readonly Point _bottomright;

		public Shot( Bitmap b, ScanArea a ) : this( b, a.TopLeft, a.BottomRight ) { }
		public Shot( Bitmap b )
			: this( b, new Point( 0, 0 ), new Point( b.Width, b.Height ) )
		{
		}

		public Shot( Bitmap b, Point topleft, Point bottomright )
		{
			_b = b;
			_topleft = topleft;
			_bottomright = bottomright;
		}

		public CardImage[] GetCards()
		{

			var c = GetColumns();
			var l = new List<CardImage>();
			foreach( var col in c )
				l.AddRange( col.GetCards() );
			return l.ToArray();
		}

		public Column[] GetColumns()
		{
			var columns = new List<Column>();
			var init = FindColumnStart();
			Point? current = init;
			while( true )
			{
				var start = FindColumnStartSeeded( current );
				if( start == null )
					break;
				var end = FindColumnEnd( start );
				current = end;
				columns.Add( new Column( _b, start, end, _bottomright ) );
			}
			return columns.ToArray();
		}

		Point FindColumnStart()
		{
			for( var x = _topleft.X; x < _bottomright.X; x++ )
				for( var y = _topleft.Y; y < _bottomright.Y; y++ )
				{
					if( !_b.GetPixel( x, y ).IsGray() )
					{
						return new Point( x, y );
					}
				}
			throw new Exception();
		}

		Point? FindColumnStartSeeded( Point? p )
		{
			if( p == null )
				throw new Exception();
			var init = p.Value;
			for( var x = init.X; x < _bottomright.X; x++ )
			{
				if( !_b.GetPixel( x, init.Y ).IsGray() )
					return new Point( x, init.Y );
			}
			return null;
		}

		Point? FindColumnEnd( Point? colstart )
		{
			if( colstart == null )
				throw new Exception();
			var start = colstart.Value;
			int blackcount = 0;
			for( var x = start.X; x < _bottomright.X; x++ )
			{
				var pixel = _b.GetPixel( x, start.Y );
				if( pixel.IsBlack() )
					blackcount++;
				else if( pixel.IsGray() )
				{
					double colwidth = x - start.X;
					var percentblack = blackcount / colwidth;
					if( percentblack < 0.9 )
						throw new Exception();
					return new Point( x, start.Y );
				}
			}
			return null;
		}
	}
}