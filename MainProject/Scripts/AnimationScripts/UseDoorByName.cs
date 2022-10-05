using UnityEngine;
using System.Collections;


namespace Animations
{
	public class UseDoorByName : DoorControl {

		// This assumes an entity with given name will touch it
		
		// action 1: walk over grab door (nobody else can use it), immediately open door
		// action 2: walk away let go of door (door closes)
		
		public string targetName = "PlayerCollider";
		public bool controlling = false;
		
		public int count = 0; 
		

		void OnTriggerEnter( Collider col ) {
			if (targetName == col.name) {
				count ++;
				Activate();
				
			}
		}
		
		void OnTriggerExit( Collider col ) {
			if (targetName == col.name) {
				
				count --;
				if (count <= 0)
				{
					count = 0;
					Deactivate();
				}
				
			}
			
		}
		
		
		public void Activate(){
			// could check if it's active already
			controlling = true;
		}
		
		public void Deactivate(){
			// could check if it's active already
			controlling = false;
		}
		
		protected override void OnEnable() {
			base.OnEnable();
		}
		
		protected override void OnDisable() {
			
			base.OnDisable();
			controlling = false;
		}
		protected override void OnUpdate(){
			
				
			if (controlling) // && opening, not closing
			{
				// opening
				if (!isOpen)
				{
					
					Open();
				}
				
			}
			else if (isOpen)
			{ // add calculation to check if I changed sides of door
				Close();
				
			}
			
			// abstract. not controlling
			// eg. bunny too close! lost control
		}
		
		
	}
}
