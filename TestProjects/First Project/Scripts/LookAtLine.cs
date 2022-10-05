using UnityEngine;
using System.Collections;

[RequireComponent (typeof (LineRenderer))]
public class LookAtLine	: MonoBehaviour 
{
	public Transform target;
	private LineRenderer line;
	
	void Start() {
		line = gameObject.GetComponent<LineRenderer>();
	}
	void Update()
	{
		line.SetPosition(0, transform.position);
		line.SetPosition(1, target.position);
		
	}
	
	
}
