using UnityEngine;
using System.Collections;

public class DisplayPieceByProgress : MonoBehaviour {

	
	public Animator anim;
	new protected Transform transform;
	protected int displayed = 0;
	protected float displayedFrac = 0;
	protected float childFrac = 0;
	
	protected void Awake() { 
		if (anim == null) 
			anim = GetComponentInParent<Animator>();
		if (anim == null) Debug.LogError(name + " there's no animator");
		transform = GetComponent<Transform>();
		
		
		childFrac = 1f / transform.childCount;
		foreach (Transform child in transform)
			child.gameObject.SetActive(false);
	}
	protected void LateUpdate () {
		float nTime = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
		
		Display(nTime);
		
		if (displayed == transform.childCount) enabled = false;
	}
	protected void Display(float percent)
	{
		while (displayedFrac < percent && displayed < transform.childCount)
		
		
		{
			transform.GetChild(displayed).gameObject.SetActive(true);
			displayed ++;
			
			displayedFrac += childFrac;
		}
		// probably going to disable this after if (displayed == children.Count) 
	}
}
