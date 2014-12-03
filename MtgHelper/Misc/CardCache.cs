using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MtgHelper.Misc;

namespace MtgHelper.Misc
{
	public class CardCache
	{
		Dictionary<long, string> _cacheCrc;
		TesseractOcr tess;

		public CardCache()
		{
			_cacheCrc = new Dictionary<long, string>();
			tess = new TesseractOcr( ".\\tesseract.exe" );
			Load();
		}

		void Load()
		{
			if( File.Exists( "crc.dat" ) )
			{
				var lines = File.ReadAllLines( "crc.dat" );
				foreach( var line in lines )
				{
					var split = line.IndexOf( "," );
					var crc = line.Substring( 0, split );
					var name = line.Substring( split + 1 );
					_cacheCrc.Add( Int64.Parse( crc ), name );
				}
			}
		}

		void Save()
		{
			File.WriteAllLines( "crc.dat", _cacheCrc.Select( kvp => string.Format( "{0},{1}", kvp.Key, kvp.Value ) ) );
		}

		public string[] Parse( Shot shot )
		{
			bool save = false;
			var l = new List<string>();
			foreach( var card in shot.GetCards() )
			{
				var data = card.Data();
				var crc = data.Crc32();
				if( !_cacheCrc.ContainsKey( crc ) )
				{
					save = true;
					var name = tess.OCRFromBitmap( data ).Trim();
					_cacheCrc.Add( crc, name );
				}
				l.Add( _cacheCrc[crc] );
			}

			if( save )
				Save();
			return l.ToArray();
		}
	}
}