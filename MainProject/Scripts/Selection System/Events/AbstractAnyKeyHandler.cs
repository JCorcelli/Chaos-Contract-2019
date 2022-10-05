using UnityEngine;
using System.Collections;


namespace SelectionSystem
{
	public abstract class AbstractAnyKeyHandler : UpdateBehaviour
	{

		public string[] buttonNames = new string[]{};
		public string triggerName = "";
		
		
		protected bool pressed = false;
		
		public bool inclusive = true;
		public bool ctrl_mod = false;
		public bool shift_mod = false;
		public bool alt_mod = false;
		
		
		protected bool combo_held = false;
		
		protected override void OnEnable() {
			
			SelectionManager.onPress += _OnPress;
			SelectionManager.onRelease += _NoInput;
		}
		
		protected override void OnDisable() {
			base.OnDisable();
			
			pressed = false;
			triggerName = "";
			SelectionManager.onPress -= _OnPress;
			SelectionManager.onRelease -= _NoInput;
		}
			
		
		protected override void _NoInput(){
			
			pressed = false;
			combo_held = false;
			triggerName = "";
				
		}
		protected override void _OnPress(){
			// determines if the combo is held now
			if (inclusive)
				combo_held = SelectGlobal.GetComboInclusive(ctrl_mod, shift_mod, alt_mod);
			else
				combo_held = SelectGlobal.GetCombo(ctrl_mod, shift_mod, alt_mod);
			
			if (!combo_held) return;
			foreach (string buttonName in buttonNames)
			{
				if (Input.GetKeyDown( buttonName) ){
					pressed = true;
					triggerName = buttonName;
					OnPress();
					break;
				}
			}

		}
		protected virtual void OnPress(){}
		
		
		protected override void OnUpdate() {
			if (pressed) _OnHold();
		}
			
		protected virtual void _OnHold(){
			
			
			if (Input.GetKey(  triggerName) )
				OnHold();
			else
			{
				pressed = false;
				OnRelease();
				triggerName = "";
			}
				
		}
		
		protected virtual void OnHold(){}
		
		protected virtual void OnRelease(){}
	
	}
}