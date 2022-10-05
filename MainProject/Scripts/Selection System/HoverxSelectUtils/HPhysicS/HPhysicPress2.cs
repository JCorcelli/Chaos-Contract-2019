using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace SelectionSystem
{
	
	public class HPhysicPress2 : UpdateBehaviour {
		
		public static HPhysicPress2 instance;
		public static HPhysicDelegate press;
		
		
		public static bool touching = false;
		
		
		new protected Collider2D collider;
		
		public static GameObject current;
		protected List<GameObject> recentCalls = new List<GameObject>();
		
		
		public Collider2D GetCollider(){return collider;}
		protected void Awake()
		{
			if (instance == null) instance = this;
			// otherwise I might leave this to toggle somehow
			
			else
			{
				Debug.Log("Multiple HPhysicPress aren't supported",gameObject);
				GameObject.Destroy(this);
				
			}
			collider = GetComponent<Collider2D>();
			
		}
		/*
		protected override void OnEnable(){
			base.OnEnable();
			if (enter != null) enter();
		}
		protected override void OnDisable(){
			base.OnDisable();
			if (exit != null) exit();
			
		}
		*/
		
		
		public void Touch(GameObject other) {
			if (!recentCalls.Contains(other)) recentCalls.Add(other);
			{
				current = other;
				if (!touching)
					current.SendMessage("Enter");
			
				touching = true;
				
			}
		}
		
		public void Release(GameObject other) {

			if (recentCalls.Contains(other)) {
				// remove
				recentCalls.Remove(other);
			}
			if (recentCalls.Count > 0) {
				// check a list
				
				if (current == other) 
				{
					
					current.SendMessage("Exit", this.collider);
				
					current = recentCalls[recentCalls.Count - 1];
					
					
				}
				
			}
			else 
				touching = false;
			
			if (touching)
				current.SendMessage("Enter", this.collider);
		}
		
		
		protected override void OnUpdate () {
			// supposedly things I hover over will subscribe to my press delegate.
			if (SelectGlobal.uiSelect) return;
			
			if (press != null)
			{
				if (touching && Input.GetButtonDown("mouse 1"))
				{
					current.SendMessage("Press");
				}
			
			}
			
			
		}
		
		
		protected override void OnLateUpdate () {
			// other object turns off
			if (touching && !current.activeInHierarchy) 
			{
				Release(current);
			}
			
			transform.position = Input.mousePosition;
		}
		
	}
}