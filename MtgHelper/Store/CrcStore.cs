using System;
using System.Collections.Generic;
using System.Linq;

namespace MtgHelper.Store
{
	public class CrcStore
	{
		IDictionary<long, int> _map = new Dictionary<long, int>();

		public CrcEntry[] Crcs
		{
			get
			{
				return _map.Select( k => new CrcEntry { Crc = k.Key, I = k.Value } ).ToArray();
			}
			set
			{
				_map = new Dictionary<long, int>();
				foreach( var kvp in value )
				{
					_map.Add( kvp.Crc, kvp.I );
				}
			}
		}

		public int? GetIndex( long crc )
		{
			if( _map.ContainsKey( crc ) )
				return _map[crc];
			return null;
		}

		public void AddCrc( int expectedindex, long crc )
		{
			if( _map.ContainsKey( crc ) )
			{
				if( _map[crc] != expectedindex )
					throw new ArgumentException();
			}
			else
			{
				_map.Add( crc, expectedindex );
			}
		}
	}
}