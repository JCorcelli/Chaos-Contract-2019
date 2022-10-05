using UnityEngine;
using System.Collections;

namespace TestProject.Cameras
{
	[RequireComponent (typeof (SphereCollider))]
	
	public class Whiskers : MonoBehaviour {
		
	public bool touched = false;
	
	void OnTriggerEnter(){ touched = true; }
		
	}
	
}
	