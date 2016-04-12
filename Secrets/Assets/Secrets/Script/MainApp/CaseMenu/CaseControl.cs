using UnityEngine;
using System.Collections;
using Studio23rd.Secrets.Data;
using Studio23rd.Secrets.Files;

namespace Studio23rd.Secrets.Case{
	public class CaseControl : MonoBehaviour {

		public FileControl m_FileControl;

		public CaseAnimation m_AnimationComponent;

		public ParticleSystem[] m_RelatedParticleEffects;


		#region Case Control Button Message
		public void EnterCurrentCase ()
		{
			// For now just pass the first bucket we have
			m_FileControl.Bucket = SecretData.m_Core.buckets[0];

			m_AnimationComponent.Enter (TurnOffEffects);
		}

		public void QuitFromInsideCase ()
		{
			TurnOnEffects ();
			m_AnimationComponent.Quit ();
		}
		#endregion
	

		void TurnOffEffects ()
		{
			foreach (ParticleSystem ps in m_RelatedParticleEffects)
				ps.gameObject.SetActive (false);
		}

		void TurnOnEffects ()
		{
			foreach (ParticleSystem ps in m_RelatedParticleEffects)
				ps.gameObject.SetActive (true);
		}
	}	
}