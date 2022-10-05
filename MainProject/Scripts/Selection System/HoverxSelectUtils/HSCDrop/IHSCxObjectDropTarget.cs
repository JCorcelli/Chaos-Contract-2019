using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SelectionSystem.IHSCx 
{
	public interface IDrop  {

		//TAmtDropped Drop();
		int Drop(int amt);
		int Drop();
		int Drop(GameObject ob);
		
		int PickUp();
		void Enter();
		void Exit();
	}
	
	
	public class ObjectDropTarget : IHSCxConnect, IDrop {
		
		// Use this for initialization
		public bool locked = false;
		public string dropperName = "dropper";
		public GameObject activatedThing;
		public GameObject hoverReady;
		
		protected List<GameObject> holdingList = new List<GameObject>();
		
		public int holding = 0;
		
		public int dropCount = 0;
		public int dropMax = 1;
		
		public int hoverCount = 0;
		
		
		
		public void Enter() {
			hoverCount ++;
			if (hoverReady != null)
				hoverReady.SetActive(true);
			
		}
		public void Exit() {
			hoverCount --;
			if (hoverCount < 1)
			{	
				hoverCount = 0;
				DisableThing();
			}
			
			
		}
		protected void DisableThing()
		{
			if (hoverReady != null)
				hoverReady.SetActive(false);
		}
		
		public virtual int Drop() {return Drop(1);}
		public virtual int Drop(int amtIn) {
			// I suppose it could be full, or there are multiple targets, which exhausted the supply.
			int taken;
			if (dropCount >= dropMax ) return 0;
			
			dropCount += amtIn;
			if (dropCount > dropMax)
			{
				taken = dropCount - dropMax;
				dropCount = dropMax;
			}
			else
				taken = amtIn;
			
			Exit(); // This doesn't get called external?
				
			if ( activatedThing != null)
				
				activatedThing.SetActive(true);// kill
			
			
			return taken;
		}
		
		public virtual int Drop(GameObject ob) {
			
			// returns the amount taken. 1 means it now holds 1 object
			
			// ***I could check if enough were taken to store the object. If not I'd give the space back. The object needs to check the HoverManager either way.
			int dropped = Drop(1);
			
			if (dropped > 0)
			{
				// one object can take multiple spots, so the variable holding is set up
				holdingList.Add(ob);
				//ob.SetActive(false); // setting this false will throw out an enumeration
				holding ++;
			}
			
			return dropped;
		}
		
		protected void Click(HSCxController caller){
			
			int amtTaken = PickUp();
			if (amtTaken > 0) OnPickUp();
		}
		protected void Press(HSCxController caller){
			// maybe a delayed action would be fine, but idon'tlikethis
			/*
			int amtTaken = PickUp();
			if (amtTaken > 0) OnPickUp();*/
		}
		public int PickUp() {
			// something else calls this, and receives the amount picked up, or I click it directly.
			if (locked || dropCount <= 0) return 0;
			
			dropCount --;
			if (dropCount <= 0) 
			{
				dropCount = 0;
				if ( activatedThing != null)
					activatedThing.SetActive(false);// there is nothing "powering" this
				
			}
			// Enter(); // Is this called external?
			return 1;
		}
		
		protected virtual void OnPickUp(){
			GameObject taken = Pop();
			if (taken != null) 
			{
				taken.SetActive(true); // this seems necessary unless there's a manager of some kind
					
				
			}
			
				
		}

		public GameObject Pop(int i){
	
			if (holding == 0) return null;
			GameObject extract = holdingList[i];
			
			holdingList.RemoveAt(i);
			holding --;
			/*
			if (text.Length == 0 || delay > 0) 
			{
				timer = delay; 
			} // maybe a remove delay?
			*/
			
			return extract;	
					
		}
		public GameObject Pop(){
	
			return Pop(0);		
		}
		
		
		
		
	}
}