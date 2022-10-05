using System;
using UnityEngine;
using System.Collections;

using SelectionSystem;

namespace NPCSystem
{
    public class DesirePhysicalForce : UpdateBehaviour
    {
		
		public Rigidbody offsetThing;
		public bool repel = true;
		public float maxForce = 10f;
		public ForceMode mode = ForceMode.Force;
		protected SphereCollider col;
		protected float radius = 0f; // use a collider or something
		
		
		
		protected void Awake() {
			col = GetComponent<SphereCollider>();
			
			
			
			if (offsetThing == null) offsetThing = gameObject.FindNameXTag("DesireIndicator","Magnets").GetComponent<Rigidbody>();
			
			
			if (offsetThing == null) Debug.Log("need a manually added thing to offset",gameObject);
		}
		
		
		protected override void OnFixedUpdate(){
			
			radius = col.radius; // could change
			
			if (repel)
				offsetThing.AddExplosionForce(maxForce, transform.position, radius, 0, mode);
			else
				//attract
				offsetThing.AddExplosionForce(-maxForce, transform.position, radius, 0, mode);
					
		}
		
		
    }
}
