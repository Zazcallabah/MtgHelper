using System;
using System.Drawing;

namespace MtgHelper.Misc
{
	// easier: only detect if card size is minimum. halt detection at all other times.

	public class ImageParser
	{
		// or better name

		// these are probably very specific.

		// they receive every _new_ image from screenshotengine
		// the window is already identified, but thats it (draft, collection, game, etc)

		// for collection, scrolling is differentiating factor
		// for draft, scroll and new draft pick
		//		draft also has selections

		// scrolling:
		// needs to identify when cards are scrolled in and out of detectable area, and create new card set for those
		// instances

		// scroll+draft:
		// detect scroll separate from draft,
		// detect draft pick by number of cards?

	}

	public class CardNameStore
	{
		//  hold hash map linking long crcs to card name
		// need some handling if hashes collide?

		// string CardName( long crc )

		// void SetForEach( string existing, string newname )

		// void addcrc( string cardname, long crc)

		// void serialize2file() 
		// invert dictionary and store in some format
		// because values cap at ~250, but keys are numerous

		// void deserializefromfile()


	}

	public class CardSet
	{
		// keep strict ordering
		// always detect cards the same way
		// give every detected card an index
		// init a new card set if screen changes, but not if cards resize?
		// or just preemptively collect hashes for all card sizes?

		// constructor takes card[]
		// lookup each card,
		// if card name exists.
		//  add name to array field
		// else
		//  alert missing crc, 
		//  try ocr lookup?
		//  add Card# template name (with unique id) to array field instead


		// field string[] names;

		// void ApplyCards( Cards[] )
		// foreach card,
		//	if crc exists in cardnamestore
		//   assert it is same name as in array
		//    else alert crc mismatch/collision
		//  else add crc using existing name for card
		//  if all cards are mismatches, signal erroneous cardset transition somehow

		// void setcardname( int index )
		//	set name in field array
		//  also set name in card store for each entry with that name
	}
	public class CardInstance
	{
		readonly Bitmap _b;
		public long Crc { get; private set; }

		public CardInstance( Bitmap b, Point tl, Point br )
		{
			_b = b.Cut( tl, br );
			Crc = _b.Crc32( new Rectangle( 0, 0, _b.Width, (int) Math.Floor( _b.Height / 2m ) ) );
		}
	}

	public class CardImage
	{
		readonly Bitmap _b;

		public CardImage( Bitmap b )
		{
			_b = b;
		}

		public Point TopLeft { get; set; }
		public Point BottomRight { get; set; }

		public void Draw()
		{
			_b.DrawRectangle( TopLeft, BottomRight );
		}

		public Bitmap Data()
		{
			var dx = BottomRight.X - TopLeft.X;
			var dy = BottomRight.Y - TopLeft.Y;
			var r = new Rectangle( TopLeft, new Size( dx, dy ) );

			Bitmap bmp = new Bitmap( r.Width * 2, r.Height * 2 );
			using( Graphics g = Graphics.FromImage( bmp ) )
			{
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
				g.DrawImage( _b, 0, 0, r, GraphicsUnit.Pixel );
			}
			bmp.Mask();
			return bmp;
		}
	}
}