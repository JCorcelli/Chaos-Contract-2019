using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StealthSystem;

namespace Dungeon
{
	public class VisibleItemNode : SeeTarget {

	
		protected VisibleItemHub hub;
		protected MeshRenderer rend;
		protected bool roomEnabled = false;
		
		protected void Awake() {
			hub = GetComponentInParent<VisibleItemHub>();
			
		}
		
		public void OnDestroy() {
			// could be damage
			
			
			if (roomEnabled)
			{
				
					
				if (hub!= null) hub.Calc(-1);
					
			}
			
				
		}
		
		
		protected override void OnUpdate () {
			// could be a delayed update, otherwise this will take a lot of resource
			
			base.OnUpdate();
			if (visible)
			{
				if (roomEnabled) return;
				roomEnabled = true;
				
				if (hub!= null) hub.Calc(1);
					
			}
			else
			{
				
				if (!roomEnabled) return;
				roomEnabled = false;
				if (hub!= null) hub.Calc(-1);
			}
		}
	}
}