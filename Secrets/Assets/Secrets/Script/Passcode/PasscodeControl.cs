using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Studio23rd.Secrets.KeyBoard;
using Studio23rd.Secrets.KeyBoard.Core;
using Studio23rd.Secrets.Data;

using DG.Tweening;

namespace Studio23rd.Secrets{
	public class PasscodeControl : MonoBehaviour {

		public Color m_SetPasscodeColor;
		public Color m_SelectPasscodeColor;

		public Key[] m_PasscodeKey;

		public Image[] m_VisualKeyIndicator;

		[SerializeField] Canvas m_Canvas;
		[SerializeField] Button m_ResetButton;
		[SerializeField] Button m_ConfirmButton;

		KeyChain keyChain;

		[SerializeField] List<KeyboardUtils.KeyValue> sequence;
		int keyChainIndicator;
		bool locked;

		#region New Passcode Record

		public void StartNewPasscodeRecord ()
		{

			keyChain = new KeyChain ();
			keyChain.keyChain = new List<KeyboardUtils.KeyValue> ();
			foreach (Key k in m_PasscodeKey)
			{
				k.ActivateKey (SetupKeyOnPressed);
				k.SetColor (m_SetPasscodeColor);
			}

			foreach (Image img in m_VisualKeyIndicator)
			{
				Material mat = Instantiate (img.material);
				img.material = mat;
			}
			m_ResetButton.image.material = Instantiate (m_ResetButton.image.material);
			m_ConfirmButton.image.material = Instantiate (m_ConfirmButton.image.material);
		}

		void SetupKeyOnPressed (KeyboardUtils.KeyValue keyValue)
		{
			keyChain.keyChain.Add (keyValue);
			Image img = m_VisualKeyIndicator[keyChain.keyChain.Count - 1];
			img.gameObject.SetActive (true);
			img.material.DOFloat (0, "_AlphaVanishGate", 1f);

			if (keyChain.keyChain.Count >= 1)
				EnableResetButton ();
			if (keyChain.keyChain.Count >= 4)
				EnableConfirmButton ();
		}

		void EnableResetButton ()
		{
			if (m_ResetButton.interactable) 
				return;
			
			m_ResetButton.interactable = true;
			m_ResetButton.image.material.DOFloat (-0.1f, "_AlphaVanishGate", 1f);
		}

		void EnableConfirmButton ()
		{
			if (m_ConfirmButton.interactable)
				return;

			m_ConfirmButton.interactable = true;
			m_ConfirmButton.image.material.DOFloat (-0.1f, "_AlphaVanishGate", 1f);
		}





		#endregion

		#region Button message  >>>>>>>>  Reset, Confirm

		public void ResetRecordProcess ()
		{
			OverlayCanvas.Block = true;
			m_ResetButton.interactable = false;
			m_ConfirmButton.interactable = false;
			keyChain.keyChain.Clear ();
			m_ResetButton.image.material.DOFloat (1.3f, "_AlphaVanishGate", 1f).OnComplete (()=>{OverlayCanvas.Block = false;});
			m_ConfirmButton.image.material.DOFloat (1.3f, "_AlphaVanishGate", 1f);
			foreach (Key k in m_PasscodeKey)
				k.ReActivateKey ();

			foreach (Image img in m_VisualKeyIndicator)
			{
				img.material.DOFloat (1f, "_AlphaVanishGate", 1f);
			}
		}

		public void ConfirmRecordProcess ()
		{
			SecretData.SaveKeyChain (keyChain);
			PlayerPrefs.SetInt ("Dirty", 1);
			StartMainApp ();
		}

		#endregion

		#region Passcode Input

		public void StartPasscodeInput ()
		{
			m_Canvas.enabled = true;
			m_ResetButton.interactable = false;
			m_ConfirmButton.interactable = false;
			m_ResetButton.image.material.SetFloat ("_AlphaVanishGate", 1.3f);
			m_ConfirmButton.image.material.SetFloat ("_AlphaVanishGate", 1.3f);
			SecretData.LoadKeyChain (out keyChain);
			sequence = new List<KeyboardUtils.KeyValue> (keyChain.keyChain);

			keyChainIndicator = 0;

			foreach (Key k in m_PasscodeKey)
			{
				k.ActivateKey (InputKeyOnPressed);
				k.SetColor (m_SelectPasscodeColor);
			}
		}



		void InputKeyOnPressed (KeyboardUtils.KeyValue keyValue)
		{
			if (keyValue == sequence[0] && !locked)
				sequence.RemoveAt (0);
			else
				locked = true;

			if (sequence.Count == 0 && !locked)
			{
				StartMainApp ();
				return;
			}
			keyChainIndicator ++;
			if (keyChainIndicator >= m_PasscodeKey.Length)
			{
				// TODO ------- Out of length, then punish the user
				Debug.Log ("Ran out of keys. Your will be PUNISHED!");
				StartMainApp ();
			}
		}


		#endregion

		#region Start Main App

		void StartMainApp ()
		{
			OverlayCanvas.Instance.Show ().OnComplete (()=>{
				m_Canvas.enabled = false;
				SecretsControl.m_Instance.SetAppControl (true);
				OverlayCanvas.Instance.Hide ();
			});
		}

		#endregion
	}
}
