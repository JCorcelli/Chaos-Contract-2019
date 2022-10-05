using UnityEngine;
using System.Collections;

namespace SelectionSystem.IHSCx 
{
	public class IHSCxDropTarget2D : ObjectDropTarget {

			
		protected override void OnEnable(){
			Connect();
			if (ih == null) return;
			
			ih.onPress += Press;
			ih.onClick += Click;
			
			Collider2D col = GetComponent<Collider2D>();
			if (col == null) Debug.Log(name + " has no 2d collider",gameObject);
			else
			{
				HoverManager.dropTargets2D.Add(col);
				if (
				HoverManager.onEnableTarget2D != null)
					HoverManager.onEnableTarget2D(gameObject);
				
			}
			
		}
		protected override void OnDisable(){
			
			if (ih == null) return;
			
			ih.onPress -= Press;
			ih.onClick -= Click;
			
			
			Collider2D col = GetComponent<Collider2D>();
			if (col == null) Debug.Log(name + " has no collider",gameObject);
			else
			{
				if (
				HoverManager.onDisableTarget2D != null)
					HoverManager.onDisableTarget2D(gameObject);
				HoverManager.dropTargets2D.Remove(col);
			}
			hoverCount = 0;
			Exit();
		}
		
		
		
	}
}