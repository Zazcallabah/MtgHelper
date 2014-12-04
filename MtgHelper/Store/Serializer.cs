using System.Globalization;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace MtgHelper.Store
{
	public class Serializer
	{
		readonly JsonSerializer _serializer;

		public Serializer()
		{
			_serializer = JsonSerializer.Create();
		}


		public string Serialize( object source )
		{
			var sb = new StringBuilder( 128 );
			var sw = new StringWriter( sb, CultureInfo.InvariantCulture );
			using( var jsonWriter = new JsonTextWriter( sw ) )
			{
				_serializer.Serialize( jsonWriter, source );
			}
			return sw.ToString();
			//			var targetWriter = new StreamWriter( target ) { AutoFlush = true };
			//		targetWriter.Write( sw.ToString() );
		}


		public T Deserialize<T>( string source )
		{
			return _serializer.Deserialize<T>( new JsonTextReader( new StringReader( source ) ) );
		}

	}
}