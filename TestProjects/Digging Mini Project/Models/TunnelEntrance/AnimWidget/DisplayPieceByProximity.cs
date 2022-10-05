using UnityEngine;
using System.Collections;

public class DisplayPieceByProximity : MonoBehaviour {

	
	new protected Transform transform;
	public Transform targetBrush;
	protected int displayed = 0;
	
	protected void Awake() { 
		transform = GetComponent<Transform>();
		
		
		foreach (Transform child in transform)
			child.gameObject.SetActive(false);
	}
	protected void Update () {
		
		Display();
		
		if (displayed == transform.childCount) enabled = false;
	}
	
	public int delayCurrent = 0;
	protected void Display()
	{
		
		float compare = 0f;
		float shortestdistance;
		
		
		
		if (displayed > 0)
		{
			shortestdistance = Vector3.Distance(transform.GetChild(displayed-1).position, targetBrush.position);
		}
		
		else 
		{
			shortestdistance = Vector3.Distance(transform.GetChild(displayed).position, targetBrush.position);
			transform.GetChild(displayed).gameObject.SetActive(true); // 0 is always set visible now
		}
		
		// I have to test before entering loop
		
		if (displayed < transform.childCount - 1)
			compare = Vector3.Distance(transform.GetChild(displayed + 1).position, targetBrush.position);
		
		
		while (displayed < transform.childCount -1 && compare <= shortestdistance) // just the unused ones is fine
		{
			shortestdistance = compare;
			
			if (displayed >= delayCurrent)
				transform.GetChild(displayed - delayCurrent).gameObject.SetActive(true);
			displayed ++;
			
			if (displayed < transform.childCount - 1)
				compare = Vector3.Distance(transform.GetChild(displayed + 1).position, targetBrush.position);
			
		}
		
			
	}
}
