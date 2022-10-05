using UnityEngine;
using System.Collections;

public class CopyOtherCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Camera>().CopyFrom(Camera.main);
		enabled = false;
	}
	
}
