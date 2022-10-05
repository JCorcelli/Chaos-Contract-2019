using UnityEngine;
using System.Collections;
using SelectionSystem;

public class IndicatorLine2D	: AbstractButtonHandler
{
	
	public Transform target;
	protected Transform camTransform;
	public RectTransform target2D;
	protected float distanceMin = .2f;
	protected float distanceMax = 2f;
	public LineRenderer line;
	
	protected Vector3 targetWorld;
	protected GameObject child;
	protected Plane plane = new Plane(Vector3.up, Vector3.zero);
	protected Ray ray;
	protected float dist = 0f;
	
	protected virtual void  Start(){
		
		child = line.gameObject;
		camTransform = Camera.main.transform;
	}
	
	
	protected override void OnLateUpdate()
	{
		
		ray = Camera.main.ScreenPointToRay(target2D.position);
		
		dist = 0f;
		
		
		plane.SetNormalAndPosition(-camTransform.forward, target.position);
		
		plane.Raycast(ray, out dist);
		targetWorld = ray.GetPoint(dist);
		
		
			
		float distance = Vector3.Distance(targetWorld, target.position);
		
		if (distance < distanceMin)
		{
			if (child.activeSelf) child.SetActive(false);
		}
		else
		{
			if (distance > distanceMax)
				distance = distanceMax;
			Vector3 newPosition = new Vector3(0,0,distance - distanceMin  ) ;
			transform.position = target.position ;
			
			transform.LookAt(targetWorld);
			transform.Translate(Vector3.forward *distanceMin);
			
			line.SetPosition(1, newPosition);
			
			
			if(!child.activeSelf) child.SetActive(true);
		}		
	}
	
	
}
