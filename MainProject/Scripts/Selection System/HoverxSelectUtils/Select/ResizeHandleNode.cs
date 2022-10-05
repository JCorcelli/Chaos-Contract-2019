using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

namespace SelectionSystem
{
	public class ResizeHandleNode : SelectNodeConnect {
		// this sends a signal to a single hub
		
		
		protected ResizeHub rh;
		
		protected override void OnEnable(){
			base.OnEnable();
			ConnectRH();
		}
		protected void ConnectRH() {
			
			rh = gameObject.GetComponentInParent<ResizeHub>();
			
			
		}
		
		public Vector2 grab = Vector2.zero;
		protected override  void  OnPress() { 
			rh.grab = grab;
			rh.OnPress(); 
		}
		protected override  void  OnRelease() { rh.OnRelease(); }
		
		
		
		
	}
}