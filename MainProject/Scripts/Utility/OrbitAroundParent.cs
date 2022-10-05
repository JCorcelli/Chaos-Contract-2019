
using UnityEngine;
using System.Collections;


public class OrbitAroundParent : MonoBehaviour 
{
	
	public float speed = 1;

	
	// Update is called once per frame
	void Update () 
	{
		transform.position = OrbitPointAroundPivot(transform.position, Vector3.zero, new Vector3(0, speed * Time.deltaTime, 0));
	}

	public Vector3 OrbitPointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
	{
		Vector3 dir = point - pivot;
		dir = Quaternion.Euler(angles) * dir;
		point = dir + pivot;
		return point;
	}
}
	