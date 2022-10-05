using UnityEngine;
using System.Collections;
using SelectionSystem;

public class IndicatorLine	: AbstractButtonHandler
{
	public Transform target;
	public float distanceMin = 1f;
	public LineRenderer line;
	
	protected GameObject child;
	
	protected virtual void  Start(){
		
		child = line.gameObject;
	}
	
	
	protected override void OnLateUpdate()
	{
		float distance = Vector3.Distance(transform.position, target.position);
		
		if (distance < distanceMin)
		{
			child.SetActive(false);
		}
		else
		{
			Vector3 newPosition = new Vector3(0,0,distance - .06f ) ;
			transform.LookAt(target);
			line.SetPosition(1, newPosition);
			
			
			child.SetActive(true);
		}		
	}
	
	
}
