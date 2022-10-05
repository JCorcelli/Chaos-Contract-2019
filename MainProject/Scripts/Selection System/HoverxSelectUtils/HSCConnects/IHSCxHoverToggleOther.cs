using UnityEngine;
using System.Collections;


namespace SelectionSystem.IHSCx
{
	
	
	public class IHSCxHoverToggleOther : IHSCxConnect {


		public GameObject other;
		
		protected override void OnEnable() {
			Connect();
			if (ih == null) return;
			ih.onEnter += Enter;
			ih.onExit += Exit;
		}
		
		protected override void OnDisable() {
			if (ih == null) return;
			ih.onEnter -= Enter;
			ih.onExit -= Exit;
		}
		
		
		protected void Enter(HSCxController caller) {
			
			other.SetActive(true);
		}
		
		protected void Exit(HSCxController caller) {
			
			other.SetActive(false);
		}
		
		
	}
}