using UnityEngine;
using System.Collections;


namespace SelectionSystem.IHSCx
{
	
	
	public class HSCxSpawner : IHSCxConnect {
		
		
		public GameObject spawnedThing;
		
		protected Vector3 hitOffset; // anchor
		//protected Vector3 viewOffset;
		
		public float normalOffset = .1f;
		
		protected SphereCollider thisCol;
		
		new protected Transform transform;
		protected virtual void Awake() {
			
			transform = GetComponent<Transform>();
		}
		
		protected override void OnEnable(){
			Connect();
			if (ih == null) return;
			
			ih.onPress += Press;
			
		}
		protected override void OnDisable(){
			
			if (ih == null) return;
			
			ih.onPress -= Press;
			
		}
		
		
		protected void Press(HSCxController caller) { Press(); }
		
		protected virtual void Press() {
			// e.g. pickup
			
			
			if (ItemManager.holding > 0)
			{
				ItemManager.holding --;
				
				Transform t = Instantiate(spawnedThing).transform;
				
				t.position = ih.hit.point + ih.hit.normal * normalOffset; // plus I should make sure it doesn't fall into the ground
			}
			
		}
		
	}
}