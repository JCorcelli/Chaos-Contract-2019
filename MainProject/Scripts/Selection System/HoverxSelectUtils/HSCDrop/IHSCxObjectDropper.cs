using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SelectionSystem;

namespace SelectionSystem.IHSCx
{
	
	
	
	public class ObjectDropper : IHSCxConnect {
		// requires IDrop interface with Enter
		
		
		public GameObject droppedThing;
		public GameObject activateThing;
		public GameObject deactivateThing;
		public GameObject dropReady;
		
		protected List<GameObject> targetList = new List<GameObject>();
		protected GameObject heldBy;
		
		protected Vector3 hitOffset; // anchor
		//protected Vector3 viewOffset;
		protected Vector3 lastMousePosition = Vector3.zero;
		public bool held = false;
		public bool dropped = false;
		public int count = 0;
		
		public string receiverName = "catcher";
		
		new protected Transform transform;
		
		protected virtual void Enter(GameObject t) {
			
			if (targetList.Contains(t)) return;
			targetList.Add(t);
			t.GetComponent<IDrop>().Enter();
			count ++;
			if (!dropped && dropReady != null) 
				dropReady.SetActive(true);
				
		}
		protected virtual void Exit(GameObject t) {
			if (targetList.Contains(t))
				targetList.Remove(t);
			else
				return;
			
			t.GetComponent<IDrop>().Exit();
			count --;
			if (count <= 0)
			{
				count = 0;
				if (dropReady != null) 
					dropReady.SetActive(false);
			}
			// this will require me to check for a component of some type on release
		}
		protected void Cleanup(){
			foreach (GameObject g in targetList) 
			{
				IDrop o = g.GetComponent<IDrop>();
				if (o != null)
				{
					o.Exit();
				}
			}
			
			targetList.Clear();
			count = 0;
		}
		protected void Press(HSCxController caller) { Press(); }
		
		protected virtual void Press() {
			// e.g. pickup
			
			held = true;
			
			
			// cast to check for a pickupTarget... maybe
			
			if (dropped)
			{
				Undrop();
					
			}
			Cleanup();
			ForceEnter();
				
			
		}
		
		protected virtual void Undrop(){
			// this gets called if I'm the pickup target
			// help
			
			if (heldBy != null && heldBy.activeInHierarchy)
			{
				IDrop idrop = heldBy.GetComponent<IDrop>();
				int taken = idrop.PickUp(); // should reduce its held objects by 1.
				
				if (taken > 0)
				{
					
					// this means I'm dropped
					if (activateThing != null) 
						activateThing.SetActive(false);
					
					// this means I'm "available"
					if (deactivateThing != null)	
						deactivateThing.SetActive(true);
					dropped = false;
					
					
				}
			}
			else
			{
				// I take it anyway, but heldby isn't updated
				if (activateThing != null) 
					activateThing.SetActive(false);
				
				// this means I'm "available"
				if (deactivateThing != null)	
					deactivateThing.SetActive(true);
				dropped = false;
				
			}
		
			
		}
		
		protected virtual void ForceEnter(){}
		protected virtual void ForceExit(){}
		
		protected void Release(HSCxController caller) {
			Release();
		}
		protected virtual void Release() {
			// I could call a delegate of the held item queue that
			
			held = false;
			if (count <= 0 ) 
			{
				return; // no longer held, not touching anything, nothing significant changed
			}
			
			int remainder = 1; // currently this is a one object operation
			
			
			foreach (GameObject dropTarget in targetList)
			{
				IDrop idrop = dropTarget.GetComponent<IDrop>();
				if (idrop != null)
				{
					if (droppedThing == null)
						remainder -= idrop.Drop();
					else if (idrop.Drop(droppedThing) > 0)
					{
						remainder --;
					}
					if (remainder < 1) 
					{
						heldBy = dropTarget;
						break; // drop succeeded
					}
					
				}
			}
			
					
			if (remainder < 1) 
			{
				if (droppedThing != null)
				droppedThing.SetActive(false);
				
				dropped = true;
				Cleanup();
				
				// this means I'm dropped
				if (activateThing != null) 
					activateThing.SetActive(true);
				
				// this means I'm "available"
				if (deactivateThing != null)	
					deactivateThing.SetActive(false);
				
				
				// this means I'm hovering
				if (dropReady != null) 
					dropReady.SetActive(false);
			}

		}
		
		
		
		 
	}
}