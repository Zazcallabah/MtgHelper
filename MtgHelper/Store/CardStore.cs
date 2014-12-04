using System;
using System.Collections.Generic;
using System.Linq;

namespace MtgHelper.Store
{
	public class CardStore
	{
		IDictionary<int, CardInfo> _map = new Dictionary<int, CardInfo>();

		public CardInfo[] Cards
		{
			get
			{
				return _map.Select( k => k.Value ).ToArray();
			}
			set
			{
				_map = new Dictionary<int, CardInfo>();
				foreach( var info in value )
				{
					_map.Add( info.Index, info );
				}
			}
		}

		public void AddCard( CardInfo card )
		{
			if( _map.ContainsKey( card.Index ) )
				throw new ArgumentException();
			_map.Add( card.Index, card );

		}
		public CardInfo GetCard( int index )
		{
			if( _map.ContainsKey( index ) )
				return _map[index];
			return null;

		}
	}
}