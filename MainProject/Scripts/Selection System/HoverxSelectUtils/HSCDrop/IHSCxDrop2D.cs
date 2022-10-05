using UnityEngine;
using System.Collections;


namespace SelectionSystem.IHSCx
{
	
	
	public class IHSCxDrop2D : ObjectDropper, ITrigger2D {
		
		
		
		protected Collider2D thisCol;
		protected virtual void Awake() {
			thisCol = GetComponent<Collider2D>();
			transform = GetComponent<Transform>();
			
		}
		
		protected override void OnEnable(){
			Connect();
			if (ih == null) return;
			
			ih.onPress += Press;
			ih.onRelease += Release;
			
			// dropped = false;
			// ForceEnter();
			// check mouse?
			// Press();
			HoverManager.onDisableTarget2D += ExitTestOb;
			//HoverManager.onEnableTarget2D += EnterTestOb;
			if (dropped && droppedThing != null)
				droppedThing.transform.position = Input.mousePosition;
		}
		protected override void OnDisable(){
			
			if (ih == null) return;
			
			ih.onPress -= Press;
			ih.onRelease -= Release;
			
			
			Cleanup();
			
			if (held)
				Release();
			
			HoverManager.onDisableTarget2D -= ExitTestOb;
			//HoverManager.onEnableTarget2D -= EnterTestOb;
			
		}
		
		
		public virtual void OnTriggerEnter2D(Collider2D col) {
			if (col.name.ToLower() == receiverName)
				Enter(col.gameObject);
		}
		public virtual void OnTriggerExit2D(Collider2D col) {
			if (col.name.ToLower() == receiverName)
				Exit(col.gameObject);
				
		}
		
		
		protected void ExitTestOb(GameObject ob){
			Exit(ob);
		}
		protected void ExitTest(Collider2D target){
			if (thisCol.IsTouching(target))
			{
				OnTriggerExit2D(target);
				
				// the other game object
				foreach (ITrigger2D a in target.GetComponents<ITrigger2D>())
					a.OnTriggerExit2D(this.thisCol);
			
			}
		}
				
		
		protected void EnterTestOb(GameObject ob) {
			TriggerTest(ob.GetComponent<Collider2D>());
		}
		protected void TriggerTest(Collider2D target)
		{
			if (thisCol.IsTouching(target))
			{
				OnTriggerEnter2D(target);
					
				foreach (ITrigger2D a in target.GetComponents<ITrigger2D>())
					a.OnTriggerEnter2D(this.thisCol);
			}
		}
		
		protected override void ForceEnter() {
			count = 0; // not touching anything
			
			foreach (Collider2D target in HoverManager.dropTargets2D)
			{
				TriggerTest (target);
		
			}
			
			if (count > 0)
				dropReady.SetActive(true);
			
		}
	
		 
	}
}