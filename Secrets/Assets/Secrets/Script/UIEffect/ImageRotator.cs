using UnityEngine;
using System.Collections;

public class ImageRotator : MonoBehaviour {

	public float rotateSpeed;

	public bool Enable = true;


	void Update ()
	{
		if (Enable)
		{
			Quaternion rotation = Quaternion.Euler ( new Vector3 (0, 0, rotateSpeed * Time.deltaTime));
			transform.localRotation *= rotation;
		}
	}
}
