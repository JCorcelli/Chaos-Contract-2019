using UnityEngine;
using System.Collections;

public class RotateInPlace : MonoBehaviour {

	// Use this for initialization
	public float speed = 1f;
	// Update is called once per frame

	void Update () 
	{
		transform.localEulerAngles += new Vector3(0, speed * Time.deltaTime, 0);
	}

	
}
