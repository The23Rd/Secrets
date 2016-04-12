using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;

using Studio23rd.Secrets.Data.Core;
using DG.Tweening;

namespace Studio23rd.Secrets.Files{
	public class FileSheet : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

		public static FileSheet m_DraggedFileSheet;

		public Text m_TitleText;

		public Image m_Background;

		public System.Action<FileSheet> onMoveToFront;

		[SerializeField] CanvasGroup m_CanvasGroup;
		[SerializeField] Button m_MoveToFrontButton;

		float m_AnimationDelay;
		Material m_ImageVanishMat;
		File23 m_File;

		#region Set File & Animation
		public FileSheet SetFile (File23 file, int bucketIndex)
		{
			m_File = DataCore.NavigateFile (file.id, bucketIndex);
			return this;
		}

		public FileSheet SetAnimationDelay (float animationDelay = 0f)
		{
			UIReset ();
			m_AnimationDelay = animationDelay;
			return this;
		}

		public void StartAnimation ()
		{
			StartCoroutine (UIAnimation (m_AnimationDelay));
		}

		void UIReset ()
		{
			m_ImageVanishMat = Instantiate (m_Background.material);
			m_Background.material = m_ImageVanishMat;
			m_ImageVanishMat.SetFloat ("_AlphaVanishGate", 1.3f);
			m_TitleText.color = new Color (m_TitleText.color.r, m_TitleText.color.g, m_TitleText.color.b, 0);
			m_MoveToFrontButton.interactable = false;
			m_MoveToFrontButton.onClick.RemoveAllListeners ();
			gameObject.SetActive (true);
		}

		IEnumerator UIAnimation (float animationDelay)
		{
			yield return new WaitForSeconds (0.15f+animationDelay);
			yield return new WaitForEndOfFrame ();

			m_ImageVanishMat.DOFloat (-0.1f, "_AlphaVanishGate", 1f);
			m_TitleText.DOFade (1,0.5f).OnComplete (()=>{
				m_MoveToFrontButton.interactable = true;
				m_MoveToFrontButton.onClick.AddListener (()=>{
					if (onMoveToFront != null)
						onMoveToFront (this);
				});
			});
		}

		public void FadeFile (float animationDelay = 0)
		{
			StartCoroutine (UIFadeAnimation (animationDelay));
		}

		IEnumerator UIFadeAnimation (float animationDelay)
		{
			m_MoveToFrontButton.interactable = false;
			yield return new WaitForSeconds (animationDelay);
			yield return new WaitForEndOfFrame ();

			m_TitleText.DOFade (0, 0.3f);
			m_ImageVanishMat.DOFloat (1.3f, "_AlphaVanishGate", 0.7f).SetDelay (0.3f);
		}
		#endregion

		#region IBeginDragHandler implementation

		public void OnBeginDrag (PointerEventData eventData)
		{
			if (m_DraggedFileSheet != null)		return;

			m_MoveToFrontButton.interactable = false;
			m_CanvasGroup.blocksRaycasts = false;
			transform.SetAsLastSibling ();
			m_DraggedFileSheet = this;
		}

		#endregion

		#region IDragHandler implementation

		public void OnDrag (PointerEventData eventData)
		{
			if (m_DraggedFileSheet != this)		return;

			transform.position = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		}

		#endregion

		#region IEndDragHandler implementation

		public void OnEndDrag (PointerEventData eventData)
		{
			if (m_DraggedFileSheet != this)		return;

			transform.position = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			m_CanvasGroup.blocksRaycasts = true;
			m_DraggedFileSheet = null;
			//TODO >>>>>> Update File Data Position, Set bucket timeindex later
			DataCore.UpdateFilePosition (m_File.id, 0, transform.localPosition);
		}

		#endregion


	}
}