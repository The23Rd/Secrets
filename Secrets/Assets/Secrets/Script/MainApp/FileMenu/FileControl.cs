using UnityEngine;
using System.Collections.Generic;

using Studio23rd.Secrets.Data.Core;

namespace Studio23rd.Secrets.Files{
	public class FileControl : MonoBehaviour {

		public FileAnimation m_AnimationComponent;

		public Transform m_FileContent;

		public Transform m_FileSheetPrefab;

		public Vector2 m_FileSheetSummonBoxSize;

		public ParticleSystem[] m_RelatedParticleEffects;

		Bucket23 m_Bucket;

		List<FileSheet> m_FileSheet = new List<FileSheet> ();

		public Vector3 RandomPosition{
			get{
				Vector3 toReturn = Vector3.zero;
				toReturn.x = Random.Range (transform.position.x-m_FileSheetSummonBoxSize.x, transform.position.x+m_FileSheetSummonBoxSize.x);
				toReturn.y = Random.Range (transform.position.y-m_FileSheetSummonBoxSize.y, transform.position.y+m_FileSheetSummonBoxSize.y);
				toReturn = m_FileContent.InverseTransformPoint (toReturn);
				toReturn.z = 0;
				return toReturn;
			}
		}

		public Bucket23 Bucket{
			get{
				return m_Bucket;
			}
			set{
				m_Bucket = value;
			}
		}

		#region Enter/Quit Panel Button Messages

		public void EnterFilePanel ()
		{
			m_AnimationComponent.EnterFilePanelAnimation (Init, 2f);
		}

		public void QuitFilePanel ()
		{
			TurnOffAllEffects ();
			HideSheets ();
			m_AnimationComponent.QuitFilePanelAnimation (0.7f);
		}

		#endregion

		public void Init ()
		{
			TurnOnAllEffects ();
			ReloadAllSheets ();
		}

		void ReloadAllSheets ()
		{
			ClearAllSheets ();

			for (int i=0; i<m_Bucket.files.Count; i++)
			{
				File23 thisFile = m_Bucket.files[i];
				Transform newSheet = Instantiate (m_FileSheetPrefab);
				FileSheet script = newSheet.GetComponent <FileSheet> ();
				newSheet.SetParent (m_FileContent, false);
				newSheet.localPosition = new Vector3 (thisFile.m_UIPositionX, thisFile.m_UIPositionY, 0);
				script.SetFile (thisFile, 0).SetAnimationDelay (i*0.25f);
				script.onMoveToFront = BringSheetToFront;
				m_FileSheet.Add (script);
			}

			//	Show files out
			foreach (FileSheet fs in m_FileSheet)
				fs.StartAnimation ();
		}

		void ClearAllSheets ()
		{
			foreach (FileSheet sheet in m_FileSheet)
			{
				Destroy (sheet.gameObject);
			}
			m_FileSheet.Clear ();
		}

		#region Particle Effect helpers
		void TurnOnAllEffects ()
		{
			foreach (ParticleSystem ps in m_RelatedParticleEffects)
				ps.gameObject.SetActive (true);
		}
		void TurnOffAllEffects ()
		{
			foreach (ParticleSystem ps in m_RelatedParticleEffects)
				ps.gameObject.SetActive (false);
		}

		#endregion

		void HideSheets ()
		{
			foreach (FileSheet fs in m_FileSheet)
				fs.FadeFile ();
		}


		#region Button Messages >>>>>>>>> By FileSheet script

		void BringSheetToFront (FileSheet sheet)
		{
			sheet.transform.SetAsLastSibling ();
		}

		#endregion

		void OnDrawGizmosSelected ()
		{
			Gizmos.DrawWireCube (transform.position, new Vector3 (m_FileSheetSummonBoxSize.x, m_FileSheetSummonBoxSize.y, 0));
		}
	}
}