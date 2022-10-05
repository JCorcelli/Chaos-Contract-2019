using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class DebugGui : MonoBehaviour
{
    
	
	
	[ContextMenu("Click me!?")]
	public void SayName()
	{
		
		Debug.Log("name: " + this.name, gameObject);
	}
}
