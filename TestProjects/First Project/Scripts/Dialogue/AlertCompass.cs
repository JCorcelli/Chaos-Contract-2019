using UnityEngine;
using System.Collections;


public class AlertCompass : MonoBehaviour {
	public Transform alertOrigin;
	
	public RectTransform imageTransform;
	

	
	private Vector3 p;
	private Vector3 p2;
	private Vector3 q;
	
	private Vector3	r;
	private Vector3	s;
	
	private float t;
	private float u;
	
	
	
	
	private float slope;
	private float pad; // half the image size
	private UnityEngine.UI.Image image;
	
	public Color opaque = new Color(1,1,1,1);
	public Color translucent = new Color(1,1,1,.6f);
	public Vector2 fullScale =  new Vector2( 100, 100); 
	public Vector2 edgeScale =  new Vector2( 50, 50); 
	public Vector2 offset =  new Vector2( 0, 0); 
	
	private float screenScalar;
	private int currentScreenHeight;
	
	private Vector3 bottomEdgea;
	private Vector3 bottomEdgeb;
	
	private Vector3 topEdgea;
	private Vector3 topEdgeb;
	
	private Vector3 leftEdgea;
	private Vector3 leftEdgeb;
	
	private Vector3 rightEdgea;
	private Vector3 rightEdgeb;
	
	
	
	void Start(){
	
		image = imageTransform.gameObject.GetComponent<UnityEngine.UI.Image>();
		currentScreenHeight = Screen.height;
		SetMargins();
	}
	
	public void SetMargins() {
		// screenedges
		screenScalar = (float)(Screen.height / 380f);
		pad = 25 * screenScalar;
		//bottom
		bottomEdgea = new Vector3(pad, pad, 0);
		bottomEdgeb = new Vector3(Screen.width - pad*2, 0, 0);
		
		
		// top
		topEdgea = new Vector3(pad, Screen.height - pad, 0);
		topEdgeb = new Vector3(Screen.width - pad*2, 0, 0);
		
		
		// left
		leftEdgea = new Vector3(pad, pad, 0);
		leftEdgeb = new Vector3(0, Screen.height - pad*2, 0);
		
		// right
		rightEdgea = new Vector3(Screen.width - pad, pad, 0);
		rightEdgeb = new Vector3(0, Screen.height - pad*2, 0);
		
	}
	
	
	void LateUpdate () {
		// center of screen in 3D space
		Vector3 origin = Camera.main.transform.position + Camera.main.transform.forward * 100;
		
		if (currentScreenHeight != Screen.height) SetMargins();
		
		
		// check if it is facing us and on screen already
		if ( -Vector3.Dot((Camera.main.transform.position - alertOrigin.position).normalized , Camera.main.transform.forward) > 0 && Camera.main.rect.Contains(Camera.main.WorldToViewportPoint(alertOrigin.position))) 
		{
			imageTransform.anchoredPosition = Camera.main.WorldToScreenPoint(alertOrigin.position);
			imageTransform.sizeDelta = fullScale * screenScalar;
			image.color = translucent;
			return;
		}
		
		imageTransform.sizeDelta = edgeScale * screenScalar;
		image.color = opaque;
		
		// we draw a line going to the alert position in screen space
		
		p = Camera.main.WorldToScreenPoint(origin);
		p2 = Camera.main.WorldToScreenPoint(Vector3.MoveTowards(origin, alertOrigin.position, 90f));
		r = (p2 - p).normalized*1e10f;
		// Debug.Log((p2 - p).normalized);
		
		// bottom
		q = bottomEdgea;
		s = bottomEdgeb;
		
		if (SetCompassPosition()) return;
		
		// top
		q = topEdgea;
		s = topEdgeb;
		
		if (SetCompassPosition()) return;
		
		// left
		q = leftEdgea;
		s = leftEdgeb;
	
		if (SetCompassPosition()) return;
		
		// right
		q = rightEdgea;
		s = rightEdgeb;
		
		if (SetCompassPosition()) return;
		
	}	
	bool SetCompassPosition() {
		
		// check if parallel
		/*
		if (Cross(r, s) .IsZero() && Cross((q-p), r) .IsZero())
		{
			// colinear, point on screen
			imageTransform.anchoredPosition = Camera.main.WorldToScreenPoint(alertOrigin.position);  
			return true;
		}
		else if (Cross(r, s) .IsZero() && Cross((q - p), r) != 0)
			// parallel non-intersecting, fail
			return false;
		*/
		
		t = Cross((q - p), s) / Cross(r, s);
		u = Cross((q - p), r) / Cross(r, s);
		
		// check if intersecting
		if (!Cross(r, s).IsZero() && (0 <=u && u <=1) && (0 <=t && t <= 1))
		{
			
			imageTransform.anchoredPosition = p + t*r;
			return true;
		}
		// non-intersecting, fail
		return false;
		
		//#### imageTransform.anchoredPosition = finalPosition;
			
			

		// optimize by stopping at line intersection
		
		
	}

    public float Cross(Vector3 v, Vector3 w)
		{
			return v.x * w.y - v.y * w.x;
		}
}
