using UnityEngine;
using System.Collections;


public class LookAtLate	: UpdateBehaviour 
{
	public Transform target;
	
	protected override void OnLateUpdate()
	{
		transform.LookAt(target);
		
	}
	
	
}
