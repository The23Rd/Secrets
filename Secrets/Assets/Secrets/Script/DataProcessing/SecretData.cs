using UnityEngine;
using System.Collections;
using Studio23rd.Secrets.KeyBoard.Core;
using Studio23rd.Secrets.Data.Core;

namespace Studio23rd.Secrets.Data{
	public class SecretData : MonoBehaviour {

		public static bool KeyChainIsSet ()
		{
			return BinaryLoadAndSave.Instance.CheckFile (SecretDataUtils.KeyChainFileName);
		}

		public static void SaveKeyChain (KeyChain keyChain)
		{
			BinaryLoadAndSave.Instance.Save<KeyChain> (keyChain, SecretDataUtils.KeyChainFileName);
		}

		public static void LoadKeyChain (out KeyChain keychain)
		{
			BinaryLoadAndSave.Instance.Load<KeyChain> (out keychain, SecretDataUtils.KeyChainFileName);
		}

		#region Core Data Interface

		public static Core23 m_Core{
			get{
				return DataCore.Core;
			}
		}

		public static void CreateDefaultCore ()
		{
			DataCore.MakeDefaultCore ();
		}

		public static void SaveCore ()
		{
			DataCore.SaveCore ();
		}

		public static void LoadCore ()
		{
			DataCore.LoadCore ();
		}

		#endregion
	}
}