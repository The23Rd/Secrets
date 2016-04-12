using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Studio23rd.Secrets.KeyBoard.Core;

using DG.Tweening;

namespace Studio23rd.Secrets.KeyBoard{
	public class Key : MonoBehaviour {

		public KeyboardUtils.KeyValue m_KeyValue;

//		[SerializeField] ParticleSystem m_SqureIlluminator;
//		[SerializeField] ParticleSystem m_RadarScanner;
		[SerializeField] Image m_KeyEffect;

		[SerializeField] Button m_KeyButton;
		bool selected;

		public void SetColor (Color color)
		{
			m_KeyEffect.color = color;
		}

		public void ReActivateKey ()
		{
			if (!selected)
				return;
			
			m_KeyEffect.material.DOFloat (1.3f, "_AlphaVanishGate", 1f).OnComplete (()=>{
				selected = false;
			});
		}

		public void ActivateKey (System.Action<KeyboardUtils.KeyValue> onClickEvent)
		{
			selected = false;
			m_KeyButton.onClick.RemoveAllListeners ();

			// Clone the image material
			Material mat = Instantiate (m_KeyEffect.material);
			m_KeyEffect.material = mat;

			m_KeyButton.onClick.AddListener (()=>{
				if (selected)
					return;

				selected = true;
				onClickEvent (m_KeyValue);

				// EnableEffect;
				m_KeyEffect.material.DOFloat (-0.1f, "_AlphaVanishGate", 1f);
			});
			ResetUI ();
		}

		void ResetUI ()
		{
			m_KeyEffect.material.SetFloat ("_AlphaVanishGate", 1.3f);
			selected = false;
		}

		Vector4 RectTransformToWorldBounds (RectTransform mRect)
		{
			Vector3[] corners = new Vector3[4];
			mRect.GetWorldCorners (corners);
			Vector3 topLeft = corners[0];
			Vector3 bottomRight = corners[2];
			return new Vector4 (topLeft.x, bottomRight.x, topLeft.y, bottomRight.y);
		}
	}
}