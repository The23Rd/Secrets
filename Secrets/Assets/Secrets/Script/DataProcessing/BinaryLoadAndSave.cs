using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Studio23rd.Secrets.Data{
	public class BinaryLoadAndSave : MonoBehaviour {

		public static BinaryLoadAndSave Instance;

		void Awake(){
			if (Instance == null)
				Instance = this;
			else if (Instance != this)
			{
				Destroy (gameObject);
				return;
			}
			CreateDirectory();
			DontDestroyOnLoad (gameObject);
		}

		void CreateDirectory(){
	#if UNITY_EDITOR
			string basePath = Application.dataPath+"/../";
	#else
			string basePath = Application.persistentDataPath + "/";
	#endif
			basePath += "file";
			if (!Directory.Exists (basePath))
				Directory.CreateDirectory (basePath);
		}

		string GetFilePath (string fileName)
		{
			string finalPath = string.Empty;
			#if UNITY_EDITOR
			finalPath = Application.dataPath + "/../file" + "/" + fileName;
			#else
			finalPath = Application.persistentDataPath + "/file" + "/" + fileName;
			#endif
			return finalPath;

		}

		public void Save<T>(T serializedClass, string fileName){
			BinaryFormatter bf = new BinaryFormatter();

			string finalPath = GetFilePath (fileName);

			FileStream file = File.Open(finalPath, FileMode.OpenOrCreate);
			bf.Serialize(file,serializedClass);
			file.Close();

		}

		public bool Load<T>(out T serializedClass, string fileName){
			string finalPath = GetFilePath (fileName);

			if(File.Exists(finalPath)){
				BinaryFormatter bf = new BinaryFormatter();
				FileStream file = File.Open(finalPath,FileMode.Open);
				serializedClass = (T)bf.Deserialize(file);
				file.Close();
				return true;
			}else{
				Debug.LogError("File doesnt exist: "+finalPath);
				serializedClass = default(T);
				return false;
			}
		}

		public bool CheckFile(string fileName){
			string finalPath = GetFilePath (fileName);
			return File.Exists(finalPath);
		}
	}
}