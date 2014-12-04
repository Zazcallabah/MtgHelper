MtgHelper
=========

Can something please keep track of all those cards?

The idea is to screenshot, isolate card name, ocr, track.





Notes & random thoughts:
-----
We have somewhat reliable detection of card positions for non-complex displays (no hovers, no sudden resize, no overlaps etc.)

Introduce crc store
* serialization: should have card name and list of crcs
* when used inmemory inverts that format into hashmap for quick lookup

Introduce card class
* that holds its position and the base image
* can assert that it in fact contains card (pixelwalk edge and expect black?)
* can calculate crc for relevant parts of image
	* throuh title because that part is the part most often not hidden
	* through title because sometimes card image isnt loaded immediately
	* through card image because font pixels may differ between computers?
* later: support partial cards
* can return new bitmap of only that card (see Cut())
* can return card width

Test by connecting Detector to live app, display crcs for displayed cards, as well as names for known crcs

Introduce web lookup
* scrape gatherer or other sites for data on card, store some data in serialized form?
	* Color for calculating openness
	* price info is also interesting
	* gatherer score possibly, curated score would be good, 
	* possibility of adding own notes

Introduce draft detection:
Very similar to main area detection, but add card count from the number in top left corner
Handle state based on card count
Only detect if card size is min size, to avoid interfering with mouse actions
* selection switches border colors, which will confuse the pixelwalk.


Random thoughts from code
---------------------
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