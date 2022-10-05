
using UnityEngine;
using System.Collections;
using SelectionSystem;

namespace Utility.Triggers 
{
	
	public class OnTriggerSetActive : MonoBehaviour
	{
		// this is a leaf of the zone hub
		public bool setActiveTo = false;
		public string targetName;
		public GameObject setTarget;
		
		protected void OnEnable(){
			
			if (setTarget == null) setTarget = gameObject;
		}
		protected void OnTriggerEnter(Collider col)
		{
			if (col.gameObject.name == targetName) setTarget.SetActive(setActiveTo);
		}
		
	}
}