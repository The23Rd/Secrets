using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using DG.Tweening;

namespace Studio23rd.Secrets.Core{
	public class Bucket23Animation : MonoBehaviour {
		public CanvasGroup m_CanvasGroup;
		public Image m_Handler;
		public ParticleSystem m_HandlerBloom;
		public Image m_ThickRing01;
		public Image m_ThickRing02;
		public Image m_Ring01;
		public Image m_Ring02;

		public GameObject m_TopArea;
		public GameObject m_BottomArea;

		Vector3 m_TopAreaOriginalPosition;
		Vector3 m_BottomAreaOriginalPosition;
		float m_ThickRingOriginalScale01, m_ThickRingOriginalScale02, m_RingOriginScale01, m_RingOriginScale02;

		void Start ()
		{
			m_TopAreaOriginalPosition = m_TopArea.transform.localPosition;
			m_BottomAreaOriginalPosition = m_BottomArea.transform.localPosition;
			m_ThickRingOriginalScale01 = m_ThickRing01.rectTransform.localScale.x;
			m_ThickRingOriginalScale02 = m_ThickRing02.rectTransform.localScale.y;
			m_RingOriginScale01 = m_Ring01.rectTransform.localScale.x;
			m_RingOriginScale02 = m_Ring02.rectTransform.localScale.y;
		}
		public void Enter (System.Action animationFinishCallback = null)
		{
			ResetAsInit ();
			StartCoroutine (EnterCoroutine (animationFinishCallback));
		}
		IEnumerator EnterCoroutine (System.Action animationFinishCallback)
		{
			yield return new WaitForEndOfFrame ();
			OverlayCanvas.Block = true;

			m_Handler.transform.DOLocalRotate (new Vector3 (0,0,360), 1.5f).SetRelative (true).SetEase (Ease.OutBack);
			yield return new WaitForSeconds (1f);
			DOTween.ToAlpha (()=>m_HandlerBloom.startColor, x=>m_HandlerBloom.startColor = x, 0, 1f);
			m_ThickRing01.transform.DOScale (1.1f, 0.5f).SetEase (Ease.OutBack);
			m_ThickRing02.transform.DOScale (1.15f,0.5f).SetEase (Ease.OutBack).SetDelay (0.2f);
			m_Ring01.transform.DOScale (1.1f, 0.5f).SetEase (Ease.OutBack).SetDelay (0.4f);
			m_Ring02.transform.DOScale (1.15f, 0.5f).SetEase (Ease.OutBack).SetDelay (0.6f);
			yield return new WaitForSeconds (1f);
			m_TopArea.transform.DOLocalMoveY (195, 1f).SetRelative (true);
			m_BottomArea.transform.DOLocalMoveY (-195f, 1f).SetRelative (true);
			m_CanvasGroup.transform.DOScale (1.2f, 1f);
			m_CanvasGroup.DOFade (0, 0.5f).SetDelay (0.5f).OnComplete (()=>{
				gameObject.SetActive (false);
				if (animationFinishCallback != null)	animationFinishCallback ();
			});
		}

		void ResetAsInit ()
		{
			m_CanvasGroup.alpha = 1;
			m_CanvasGroup.transform.localScale = Vector3.one;
			m_Handler.transform.localEulerAngles = Vector3.zero;
			m_HandlerBloom.startColor = new Color (1,1,1, 40/255f);
			m_ThickRing01.transform.localScale = m_ThickRingOriginalScale01* Vector3.one;
			m_ThickRing02.transform.localScale = m_ThickRingOriginalScale02 * Vector3.one;
			m_Ring01.transform.localScale = m_RingOriginScale01 * Vector3.one;
			m_Ring02.transform.localScale = m_RingOriginScale02 * Vector3.one;
			m_TopArea.transform.localPosition = m_TopAreaOriginalPosition;
			m_BottomArea.transform.localPosition = m_BottomAreaOriginalPosition;
		}

		public void Quit ()
		{
			ResetAsFinish ();
			StartCoroutine (QuitCoroutine ());
		}

		void ResetAsFinish ()
		{
			m_CanvasGroup.alpha = 0;
			m_CanvasGroup.transform.localScale = Vector3.one * 1.2f;
			m_Handler.transform.localEulerAngles = Vector3.zero;
			m_HandlerBloom.startColor = new Color (1,1,1,0);
			m_ThickRing01.transform.localScale = Vector3.one * 1.1f;
			m_ThickRing02.transform.localScale = Vector3.one * 1.15f;
			m_Ring01.transform.localScale = Vector3.one * 1.1f;
			m_Ring02.transform.localScale = Vector3.one * 1.15f;
			m_TopArea.transform.localPosition = m_TopAreaOriginalPosition + new Vector3 (0, 195, 0);
			m_BottomArea.transform.localPosition = m_BottomAreaOriginalPosition + new Vector3 (0,-195, 0);
			gameObject.SetActive (true);
		}

		IEnumerator QuitCoroutine ()
		{
			yield return new WaitForEndOfFrame ();
			OverlayCanvas.Block = true;
			gameObject.SetActive (true);

			m_TopArea.transform.DOLocalMoveY (m_TopAreaOriginalPosition.y, 1f);
			m_BottomArea.transform.DOLocalMoveY (m_BottomAreaOriginalPosition.y, 1f);
			m_CanvasGroup.transform.DOScale (1,1);
			m_CanvasGroup.DOFade (1,0.5f).SetDelay (0.5f);
			yield return new WaitForSeconds (1f);
			m_Ring02.transform.DOScale (m_RingOriginScale02, 0.5f).SetEase (Ease.OutBack);
			m_Ring01.transform.DOScale (m_RingOriginScale01, 0.5f).SetEase (Ease.OutBack).SetDelay (0.2f);
			m_ThickRing02.transform.DOScale (m_ThickRingOriginalScale02,0.5f).SetEase (Ease.OutBack).SetDelay (0.4f);
			m_ThickRing01.transform.DOScale (m_ThickRingOriginalScale01, 0.5f).SetEase (Ease.OutBack).SetDelay (0.6f);
			DOTween.ToAlpha (()=>m_HandlerBloom.startColor, x=>m_HandlerBloom.startColor = x, 40/255f, 1f);
			yield return new WaitForSeconds (1f);
			m_Handler.transform.DOLocalRotate (new Vector3 (0,0,-360), 1.5f).SetRelative (true).SetEase (Ease.OutBack).OnComplete (()=>{;
				OverlayCanvas.Block = false;
			});
		}
	}	
}