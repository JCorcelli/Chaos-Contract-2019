using UnityEngine;
using System.Collections;



[ExecuteInEditMode]

public class ImageRectToBounds : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {
		RectTransform r = gameObject.GetComponent<RectTransform>();
		r.offsetMin = new Vector2(0,0);
		r.offsetMax = new Vector2(0,0);
	}
}
