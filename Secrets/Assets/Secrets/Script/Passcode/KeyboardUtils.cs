using UnityEngine;
using System.Collections.Generic;

namespace Studio23rd.Secrets.KeyBoard.Core{
	public class KeyboardUtils {

		public enum KeyValue{
			One,
			Two,
			Three,
			Four,
			Five,
			Six,
			Seven,
			Eight,
			Nine,
			Ten
		}


	}

	[System.Serializable]
	public class KeyChain{
		// Up to 9 keys
		public List<KeyboardUtils.KeyValue> keyChain;
	}
}