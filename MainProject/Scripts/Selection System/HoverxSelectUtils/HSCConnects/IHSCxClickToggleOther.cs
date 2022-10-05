using UnityEngine;
using System.Collections;


namespace SelectionSystem.IHSCx
{
	
	
	public class IHSCxClickToggleOther : IHSCxConnect {


		public GameObject other;
		
		protected override void OnEnable() {
			base.OnEnable();
			if (ih == null) return;
			ih.onClick += Call;
		}
		protected override void OnDisable() {
			base.OnDisable();
			ih.onClick -= Call;}
		public void Call(HSCxController caller) {
			if (ih == null) return;
			other.SetActive(!other.activeSelf);
		}
		
		
	}
}