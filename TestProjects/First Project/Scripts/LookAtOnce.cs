using UnityEngine;
using System.Collections;


public class LookAtOnce	: MonoBehaviour 
{
	public Transform target;
	
	void Awake()
	{
		transform.LookAt(target);
		
	}
	
	
}
