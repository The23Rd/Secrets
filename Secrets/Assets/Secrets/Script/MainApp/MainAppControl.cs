using UnityEngine;
using System.Collections;

using Studio23rd.Secrets.Case;
using Studio23rd.Secrets.Data;

namespace Studio23rd.Secrets{
	public class MainAppControl : MonoBehaviour {

		public CaseControl m_CaseControl;

		[SerializeField] Canvas m_Canvas;

		[SerializeField] GameObject[] particleEffects;

		public bool Enable
		{
			get{
				return m_Canvas.enabled;
			}
			set{
				LoadMyCores ();
				m_Canvas.enabled = value;
				if (value)		EnableSelf ();
				else 				DisableSelf ();
			}
		}

		void EnableSelf ()
		{
			foreach (GameObject go in particleEffects)
				go.SetActive (true);
		}
		void DisableSelf ()
		{
			foreach (GameObject go in particleEffects)
				go.SetActive (false);
		}

		// Load all the cores from local file
		void LoadMyCores ()
		{
			// TODO >>>>> not available for now, cause only has one core
		}
	}	
}