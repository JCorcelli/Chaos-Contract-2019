using UnityEngine;
using System.Collections;


public class LookAtFixed	: UpdateBehaviour 
{
	public Transform target;
	
	protected override void OnUpdate()
	{
		transform.LookAt(target);
		
	}
	
	
}
