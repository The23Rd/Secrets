using UnityEngine;
using System.Collections.Generic;
using System;

namespace Studio23rd.Secrets.Data.Core{
	public class DataCore : MonoBehaviour {
		public static DataCore Instance;

		public static Int64 UniqueTimeStamp{
			get{
				string timeStamp = DateTime.UtcNow.ToString ("yyyyMMddHHmmssffff");
				return Int64.Parse(timeStamp);
			}
		}

		public static Core23 Core{
			get{
				return Instance.m_Core;
			}
		}

		void Awake ()
		{
			if (Instance == null)
				Instance = this;

			DontDestroyOnLoad (gameObject);
		}

		Core23 m_Core;

		#region Navigators

		public static File23 NavigateFile (Int64 fileId, int bucketIndex)
		{
			return Instance.m_Core.buckets[bucketIndex].NavigateFile (fileId);
		}

		#endregion

		#region Updators

		public static void UpdateFilePosition (Int64 fileId, int bucketIndex, Vector3 newPosition)
		{
			File23 file = NavigateFile (fileId, bucketIndex);
			if (file != null)
			{
				file.m_UIPositionX = newPosition.x;
				file.m_UIPositionY = newPosition.y;
				SaveCore ();
			}
		}

		#endregion

		public static void SaveCore ()
		{
			BinaryLoadAndSave.Instance.Save<Core23> (Instance.m_Core, SecretDataUtils.DataCoreFileName);
		}

		public static void LoadCore ()
		{
			BinaryLoadAndSave.Instance.Load<Core23> (out Instance.m_Core, SecretDataUtils.DataCoreFileName);
		}

		public static void MakeDefaultCore ()
		{
			Instance.m_Core = new Core23 ();
			Bucket23 bucket = new Bucket23 ();
			bucket.timeIndex = DataCore.Core.NextBucketTimeIndex;
			File23 newFile = new File23 ();
			newFile.id = DataCore.UniqueTimeStamp;
			newFile.m_Title = "Guide";
			newFile.m_Caption = "Welcome to the secret world!";
			bucket.AddFile (newFile);
			DataCore.Core.AddBucket (bucket);
			SaveCore ();
		}

	}

	[System.Serializable]
	public class Core23{
		public List<Bucket23> buckets;

		public int NextBucketTimeIndex{
			get{
				int maxIndex = 0;
				foreach (Bucket23 bucket in buckets)
					maxIndex = Mathf.Max (maxIndex, bucket.timeIndex);
				return (maxIndex + 1);
			}
		}

		#region constructor
		public Core23 (){
			buckets = new List<Bucket23> ();
		}
		#endregion

		public void AddBucket (Bucket23 bucket)
		{
			buckets.Add (bucket);
		}
	}

	[System.Serializable]
	public class Bucket23{
		public int timeIndex;
		public List<File23> files;

		#region constructor
		public Bucket23 ()
		{
			files = new List<File23> ();
		}
		#endregion

		#region Navigator

		public File23 NavigateFile (Int64 fileId)
		{
			foreach (File23 file in files)
			{
				if (file.id == fileId)
					return file;
			}
			return null;
		}

		#endregion

		public void NewFile ()
		{
			AddFile (new File23 ());
		}

		public void AddFile (File23 newFile)
		{
			files.Add (newFile);
		}
	}

	[System.Serializable]
	public class File23{
		public enum Diversifier{
			Note,
			Account,
			Activation
		}
		public Int64 id;
		public string m_Title;
		public string m_Caption;
		public Diversifier m_Division;
		public float m_UIPositionX;
		public float m_UIPositionY;
		public List<File23Field> m_Fields;

		#region constructor
		public File23 ()
		{
			m_Division = Diversifier.Note;
			m_UIPositionX = 0;
			m_UIPositionY = 0;
			m_Fields = new List<File23Field> ();
		}
		#endregion
	}

	public interface File23Field{
		void RenderField ();
	}

	[System.Serializable]
	public class RichTextField : File23Field{

		public string m_Text;
		public float m_Height;

		#region constructor
		public RichTextField ()
		{
			m_Text = string.Empty;
			m_Height = 230f;
		}
		#endregion

		#region File23Field implementation
		public void RenderField ()
		{
			throw new System.NotImplementedException ();
		}
		#endregion
	}
}