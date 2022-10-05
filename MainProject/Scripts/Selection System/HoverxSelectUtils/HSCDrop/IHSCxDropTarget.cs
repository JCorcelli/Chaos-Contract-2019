using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SelectionSystem.IHSCx 
{
	public class IHSCxDropTarget : ObjectDropTarget {

		
		protected override void OnEnable(){
			Connect();
			if (ih == null) return;
			
			ih.onPress += Press;
			ih.onClick += Click;
			
			SphereCollider col = GetComponent<SphereCollider>();
			if (col == null) Debug.Log(name + " has no collider",gameObject);
			else
			{
				HoverManager.dropTargets.Add(col);
				
				if (
				HoverManager.onEnableTarget != null)
					HoverManager.onEnableTarget(gameObject);
				
			}
			
			
		}
		protected override void OnDisable(){
			
			if (ih == null) return;
			
			ih.onPress -= Press;
			ih.onClick -= Click;
			
			SphereCollider col = GetComponent<SphereCollider>();
			if (col == null) Debug.Log(name + " has no collider",gameObject);
			else
			{
				if (
				HoverManager.onDisableTarget != null)
					HoverManager.onDisableTarget(gameObject);
				HoverManager.dropTargets.Remove(col);
			}
			hoverCount = 0;
			Exit();
		}
		
		
	}
}