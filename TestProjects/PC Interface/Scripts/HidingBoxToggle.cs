using UnityEngine;
using System.Collections;
using SelectionSystem;

namespace Utility.Triggers  
{
	public class HidingBoxToggle : AbstractAnyHandler {

		public string targetName = "PresenceIndicator";
			
		[SerializeField] protected GameObject[] enableThese;
		[SerializeField] protected GameObject[] disableThese;
			
		protected override void OnPress(){
			if (Input.GetButtonDown("use") && touching) 
			{
				Entering();
				
			}
			
		}
		
		void Awake () { 
			foreach( GameObject g in enableThese )
				g.SetActive(false);
				
			foreach( GameObject g in disableThese )
				g.SetActive(true); 
		
		}
		void OnCollisionEnter( Collision col ) {
			if (targetName == col.collider.name) {
				Entering();
				
			}
		}
		void OnTriggerEnter( Collider col ) {
			if (targetName == col.name) {
				Entering();
				
			}
		}
		
		protected bool touching = false;
		protected GameObject hit ;
		protected IEnumerator NoReenter()
		{
			while (touching)
			{
				yield return new WaitForSeconds(.1f);
				if (!hit.activeInHierarchy) Exitting();
			}
			
		}
		protected void Entering (){
			touching = true;
			
				foreach( GameObject g in enableThese )
					g.SetActive(true);
				foreach( GameObject g in disableThese )
					g.SetActive(false);
		}
		protected void Exitting (){
			
			touching = false;
				foreach( GameObject g in enableThese )
					g.SetActive(false);
					
				foreach( GameObject g in disableThese )
					g.SetActive(true);
			
		}
		void OnCollisionExit( Collision col ) {
			if (targetName == col.collider.name) {
				Exitting();
					
				
			}
			
		}
		void OnTriggerExit( Collider col ) {
			if (targetName == col.name) {
				Exitting();
					
				
			}
			
		}
	}
}