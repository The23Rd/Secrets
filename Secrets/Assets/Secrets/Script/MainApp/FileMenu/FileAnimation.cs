using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using DG.Tweening;

namespace Studio23rd.Secrets.Files{
	public class FileAnimation : MonoBehaviour {
		public FileControl m_Controller;

		public CanvasGroup m_CanvasGroup;

		public Transform topArea, bottomArea;

		Vector3 m_TopAreaOriginalPosition, m_BottomAreaOriginalPosition;


		void Start ()
		{
			m_TopAreaOriginalPosition = topArea.localPosition;
			m_BottomAreaOriginalPosition = bottomArea.localPosition;
		}

		public void EnterFilePanelAnimation (System.Action callback, float animationDelay = 0)
		{
			UIReset (true);
			StartCoroutine (EnterFilePanelCoroutine (callback, animationDelay));
		}

		public void QuitFilePanelAnimation (float animationDelay = 0)
		{
			UIReset (false);
			StartCoroutine (QuitFilePanelCoroutine (animationDelay));
		}

		void UIReset (bool enterThisPanel)
		{
			m_CanvasGroup.alpha = enterThisPanel?0:1;
			m_CanvasGroup.interactable = enterThisPanel;
			m_CanvasGroup.blocksRaycasts = enterThisPanel;
			topArea.localPosition = enterThisPanel?m_TopAreaOriginalPosition+new Vector3(0,195,0):m_TopAreaOriginalPosition;
			bottomArea.localPosition = enterThisPanel?m_BottomAreaOriginalPosition+new Vector3(0,-195,0):m_BottomAreaOriginalPosition;
		}
		IEnumerator QuitFilePanelCoroutine (float animationDelay)
		{
			OverlayCanvas.Block = true;
			yield return new WaitForSeconds (animationDelay);
			yield return new WaitForEndOfFrame ();

			topArea.DOLocalMoveY (m_TopAreaOriginalPosition.y+195, 0.5f);
			bottomArea.DOLocalMoveY (m_BottomAreaOriginalPosition.y-195, 0.5f);
			m_CanvasGroup.DOFade (0, 0.3f).SetDelay (0.5f);
		}
		IEnumerator EnterFilePanelCoroutine (System.Action callback, float animationDelay)
		{
			OverlayCanvas.Block = true;
			yield return new WaitForSeconds (animationDelay);
			yield return new WaitForEndOfFrame ();

			m_CanvasGroup.DOFade (1, 0.5f);
			topArea.DOLocalMoveY (m_TopAreaOriginalPosition.y, 1f).SetDelay (0.5f);
			bottomArea.DOLocalMoveY (m_BottomAreaOriginalPosition.y, 1f).SetDelay (0.5f);
			yield return new WaitForSeconds (1.5f);
			OverlayCanvas.Block = false;
			callback ();
		}
	}
}