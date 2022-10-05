
using UnityEngine;
using System.Collections;
using SelectionSystem;

namespace Inventory
{
	
	public class OnTriggerAddItem : MonoBehaviour
	{
		// this is a leaf of the zone hub
		public string itemName = "cube";
		
		public string targetName;
		protected GameObject setTarget;
		
		protected void OnEnable(){
			
			if (setTarget == null) setTarget = gameObject;
		}
		protected void OnTriggerEnter(Collider col)
		{
			if (col.gameObject.name == targetName) 
			{
				setTarget.SetActive(false);
				
				
				InventoryGlobal.items[itemName]++;
				
			}
		}
		
	}
}