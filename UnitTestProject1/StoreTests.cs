using MtgHelper.Store;
using NUnit.Framework;

namespace MtgHelperTests
{
	[TestFixture]
	public class StoreTests
	{
		Serializer _serializer = new Serializer();

		[Test]
		public void CanGetCardInfoFromCardIndex()
		{
			CardStore store = _serializer.Deserialize<CardStore>( "{\"Cards\":[{\"Index\":44,\"Name\":\"A Card\"}]}" );
			var info = store.GetCard( 44 );
			Assert.AreEqual( "A Card", info.Name );
		}

		[Test]
		public void CanGetCardIndexFromStore()
		{
			CrcStore store = new CrcStore();
			long crc = 1234;
			int expectedindex = 44;
			store.AddCrc( expectedindex, crc );

			Assert.AreEqual( 44, store.GetIndex( 1234 ) );
		}

		[Test]
		public void CanCreateStoreFromJsonString()
		{
			var str = "{\"Crcs\":[{\"I\":44,\"Crc\":1234}]}";
			var store = _serializer.Deserialize<CrcStore>( str );

			Assert.AreEqual( 44, store.GetIndex( 1234 ) );
		}

	}
}