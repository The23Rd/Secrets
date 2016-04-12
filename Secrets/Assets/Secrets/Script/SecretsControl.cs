using UnityEngine;
using System.Collections;
using Studio23rd.Secrets.Data;

using DG.Tweening;

namespace Studio23rd.Secrets{
	public class SecretsControl : MonoBehaviour {

		public static SecretsControl m_Instance;

		public PasscodeControl m_PasscodeControl;
		public MainAppControl m_AppControl;

		bool started = false;

		void Awake ()
		{
			if (m_Instance == null)
				m_Instance = this;
		}

		IEnumerator Start ()
		{
			if (PlayerPrefs.HasKey ("Dirty"))
			{
				if (!SecretData.KeyChainIsSet ())
				{
					PlayerPrefs.DeleteKey ("Dirty");
					SecretData.CreateDefaultCore ();
					m_PasscodeControl.StartNewPasscodeRecord ();
				}
				else
				{
					SecretData.LoadCore ();
					m_PasscodeControl.StartPasscodeInput ();
				}
			}
			else
			{
				SecretData.CreateDefaultCore ();
				m_PasscodeControl.StartNewPasscodeRecord ();
			}

			SetAppControl (false);

			yield return new WaitForSeconds (1f);
			yield return new WaitForEndOfFrame ();

			OverlayCanvas.Instance.Hide ();
			started = true;
		}


		void OnApplicationPause (bool isPause)
		{
			if (!started)
				return;
			
			if (isPause)
			{
				//	Lock the app
				OverlayCanvas.Instance.Show ().OnComplete (()=>{SetAppControl (false);});
			}
			else
			{
				// Start unlock process
				m_PasscodeControl.StartPasscodeInput ();
				SetAppControl (false);
				OverlayCanvas.Instance.Hide ();
			}
		}

		public void SetAppControl (bool enabled)
		{
			m_AppControl.Enable = enabled;
		}

	}
}
