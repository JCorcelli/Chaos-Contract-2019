using UnityEngine;
using System.Collections;

public class BillboardRotate : MonoBehaviour {

	
	// Update is called once per frame
	void LateUpdate () {
		transform.LookAt(Camera.main.transform.position, Vector3.up);
		transform.eulerAngles = Vector3.Scale( transform.eulerAngles, new Vector3(0,1,1));

	}
}
