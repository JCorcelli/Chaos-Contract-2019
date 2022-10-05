using UnityEngine;
using System.Collections;

using SelectionSystem.IHSCx;

namespace SelectionSystem.Magnets
{
	
	public class DimensionChangeSelector : IHSCxConnect
	{
		// These will all use the same button, so I can group their method calls.


		public GameObject deactivate;
		public GameObject activate;
		public GameObject move;
		public GameObject move2D;
		
		protected void Awake() {
			if (deactivate == null && activate == null) Debug.Log(name + " Please add something to activate/deactivate",gameObject);
			
		}
		
		public bool waitForOneRelease = false;
		protected bool waiting = false;
		protected override void OnEnable() {
			base.OnEnable();
			//if (waitForOneRelease && //DimensionComboManager.d_combo_held) waiting = true;
			//DimensionComboManager.onPress 	+= OnPress;
			//DimensionComboManager.onHold 	+= OnHold;
			//DimensionComboManager.onRelease += OnRelease;
			
		}
		protected override void OnDisable() {
			base.OnDisable();
			//DimensionComboManager.onPress -= OnPress;
			//DimensionComboManager.onHold -= OnHold;
			//DimensionComboManager.onRelease -= OnRelease;
			
		}
		
		public bool requirePress = false;
		public bool requireHover = false;
		
		public bool doOnPress = false;
		public bool doOnHold = true;
		public bool doOnRelease = false;
		
		public bool passingController= true;
		protected void OnPress(){
			if (waiting) return;
			if (!doOnPress) return;
			if ( requirePress && !ih.pressed) return;
			
			changeDimension();
			}
		protected void OnHold(){
			if (waiting) return;
			if (!doOnHold) return;
			if ( requirePress && !ih.pressed) return;
			if ( requireHover && !ih.isHovered) return;
			
			changeDimension();
		}
		protected void OnRelease(){
			if (waiting)
			{
				//if ( !//DimensionComboManager.d_combo_held)
				//{
					waiting = false;
				//}
				return;
			}
			
			if (!doOnRelease) return;
			if ( requireHover && !ih.isHovered) return;
			// I could check if I'm blocked first
			
			changeDimension();
			
		}
		
		protected float offset = .2f;
		protected void changeDimension() {
			
			
			if (deactivate != null)
				deactivate.SetActive(false);

			if (activate != null)
				activate.SetActive(true);
			
			if (move != null)
			{
				ih.Raycast();
				move.transform.position = ih.hit.point + ih.hit.normal * offset;
			}
			if (move2D != null)
				move2D.transform.position = Input.mousePosition ;
			if (passingController)
			{
				// if I can send the press parameters from my ih to the other that might be preferable.
				
				HSCxController ih = activate.GetComponentInParent<HSCxController>();
				
				if (ih != null)
					ih.Press();
			}
			
		}
		

	
	}
}