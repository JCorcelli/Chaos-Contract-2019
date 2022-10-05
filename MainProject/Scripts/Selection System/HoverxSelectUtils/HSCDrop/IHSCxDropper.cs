using UnityEngine;
using System.Collections;


namespace SelectionSystem.IHSCx
{
	
	
	public class IHSCxDropper : ObjectDropper, ITrigger {
		
		protected SphereCollider thisCol;
		protected virtual void Awake() {
			thisCol = GetComponent<SphereCollider>();
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
			//HoverManager.onEnableTarget += EnterTestOb;
			HoverManager.onDisableTarget += ExitTestOb;
			
			if (dropped)
				droppedThing.transform.position = ih.hit.point;
				
		}
		protected override void OnDisable(){
			
			if (ih == null) return;
			
			ih.onPress -= Press;
			ih.onRelease -= Release;
			
			Cleanup();
			
			if (held)
				Release();
			
			//HoverManager.onEnableTarget -= EnterTestOb;
			HoverManager.onDisableTarget -= ExitTestOb;
		}
		
		
		public virtual void OnTriggerEnter(Collider col) {
			if (col.name.ToLower() == receiverName)
				Enter(col.gameObject);
		}
		public virtual void OnTriggerExit(Collider col) {
			if (col.name.ToLower() == receiverName)
				Exit(col.gameObject);
				
		}
		
		protected void ExitTestOb(GameObject ob) {
			Exit(ob);
			
		}
		
		
		
		protected void EnterTestOb(GameObject ob) {
			EnterTest(ob.GetComponent<SphereCollider>());
		}
		protected void EnterTest(SphereCollider target) {
			float combinedRadius = (thisCol.radius * transform.lossyScale.z) + (target.radius * target.transform.lossyScale.z);
				
			// isTouching
			if (Vector3.Distance(transform.position, target.transform.position) < combinedRadius)
			{
				OnTriggerEnter(target);
					
				foreach (ITrigger a in target.GetComponents<ITrigger>())
					a.OnTriggerEnter(this.thisCol);
			}
		}
		
		protected override void ForceEnter() {
			if (thisCol == null) return;
			count = 0; // not touching anything
			
			foreach (SphereCollider target in HoverManager.dropTargets)
			{
				EnterTest(target);
		
			}
			
			if (count > 0 && dropReady != null)
				dropReady.SetActive(true);
			
		}
		 
	}
}