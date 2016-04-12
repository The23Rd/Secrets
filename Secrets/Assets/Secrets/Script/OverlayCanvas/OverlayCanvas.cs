using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using DG.Tweening;

namespace Studio23rd.Secrets{
	public class OverlayCanvas : MonoBehaviour {

		public static OverlayCanvas Instance;

		public static bool Block{
			set{
				Instance.m_OverlayImage.raycastTarget = value;
			}
		}

		public Image m_OverlayImage;

		void Awake ()
		{
			if (Instance == null)
				Instance = this;
			else if (Instance != this)
				Destroy (gameObject);

			m_OverlayImage.material = Instantiate (m_OverlayImage.material);
			m_OverlayImage.material.SetFloat ("_AlphaVanishGate", -0.1f);
			DontDestroyOnLoad (gameObject);
		}

		public Tweener Show ()
		{
			m_OverlayImage.raycastTarget = true;
			return m_OverlayImage.material.DOFloat (-0.1f, "_AlphaVanishGate", 2f);
		}

		public Tweener Hide ()
		{
			return m_OverlayImage.material.DOFloat (1.3f, "_AlphaVanishGate", 2f).OnComplete (()=>{m_OverlayImage.raycastTarget = false;});
		}
	}
}