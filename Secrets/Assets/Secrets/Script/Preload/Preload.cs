using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace Studio23rd.Secrets{
	public class Preload : MonoBehaviour {
		public float splashPeriod = 3f;
		public UnityEngine.UI.Image m_OverlayImage;

		IEnumerator Start () {
			yield return new WaitForEndOfFrame ();
			m_OverlayImage.material.DOFloat (1.3f, "_AlphaVanishGate", 2f);
			yield return new WaitForSeconds (2f);

			yield return new WaitForSeconds (splashPeriod);

			m_OverlayImage.material.DOFloat (-0.1f, "_AlphaVanishGate", 2f).OnComplete (()=>{
				UnityEngine.SceneManagement.SceneManager.LoadScene (1);
			});
		}
	}
}