using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class HideBacksideView : MonoBehaviour {

	// Use this for initialization
	private MeshRenderer c_MeshRenderer;
	private void Awake () {
		
		c_MeshRenderer = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		// I need to know if __some 'thing'__ is facing the camera?
		if (Vector3.Dot((Camera.main.transform.position - transform.position).normalized , transform.forward) < 0f)
			c_MeshRenderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
		else
			c_MeshRenderer.shadowCastingMode = ShadowCastingMode.On;
		
		
	}
}
