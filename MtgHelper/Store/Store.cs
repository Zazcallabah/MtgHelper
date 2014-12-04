using System.IO;
using System.Text;

namespace MtgHelper.Store
{
	public class Store
	{
		readonly string _crcFile;
		readonly string _cardFile;
		readonly CrcStore _crc;
		readonly CardStore _card;

		readonly Serializer _serializer = new Serializer();

		public Store( string crcFile, string cardFile )
		{
			_crcFile = crcFile;
			_cardFile = cardFile;
			_crc = _serializer.Deserialize<CrcStore>( File.ReadAllText( crcFile, Encoding.UTF8 ) );
			_card = _serializer.Deserialize<CardStore>( File.ReadAllText( cardFile, Encoding.UTF8 ) );
		}

		public CardInfo Lookup( long crc )
		{
			var index = _crc.GetIndex( crc );
			if( index == null )
				return null;
			return _card.GetCard( index.Value );
		}

		public void Save()
		{
			File.WriteAllText( _crcFile, _serializer.Serialize( _crc ), Encoding.UTF8 );
			File.WriteAllText( _cardFile, _serializer.Serialize( _card ), Encoding.UTF8 );
		}
	}
}