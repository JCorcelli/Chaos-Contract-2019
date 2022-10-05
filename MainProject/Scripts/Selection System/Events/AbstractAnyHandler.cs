using UnityEngine;
using System.Collections;


namespace SelectionSystem
{
	public abstract class AbstractAnyHandler : UpdateBehaviour
	{


		protected static ButtonNames b 
		{
			get {return SelectGlobal.buttons;}
		}
		
		public string triggerName = "";
		
		
		public bool pressed = false;
		
		public string buttonPressed {
			get{return SelectGlobal.button;} 
			set{  SelectGlobal.button = value;}
		}
		
		public bool inclusive = true;
		public bool ctrl_mod = false;
		public bool shift_mod = false;
		public bool alt_mod = false;
		
		
		public bool combo_held = false;
		
		
		
		
		protected override void OnEnable() {
			base.OnEnable();
			
			
			EnableButtons();
			
			
		}
		
		protected override void OnDisable() {
			base.OnDisable();
			
			pressed = false;
			triggerName = "";
			
			DisableButtons();
			
		}
		
		protected override void _NoInput(){
			
			pressed = false;
			combo_held = false;
			triggerName = "";
				
		}
		protected override void OnUpdate() {
			if (pressed) _OnHold();
		}
			
		protected override void _OnPress(){
			// determines if the combo is held now
			
			if (inclusive)
				combo_held = SelectGlobal.GetComboInclusive(ctrl_mod, shift_mod, alt_mod);
			else
				combo_held = SelectGlobal.GetCombo(ctrl_mod, shift_mod, alt_mod);
			
			if (!combo_held) return;
			foreach (string buttonName in b.buttonNames)
			{
				if (Pressing( buttonName) ){
					pressed = true;
					triggerName = buttonName;
					OnPress();
					break;
				}
			}

		}
		protected virtual void OnPress(){}
		
		
		protected virtual void _OnHold(){
			
			if (triggerName == "") return;
			if (Holding(  triggerName) )
				OnHold();
			else
			{
				pressed = false;
				_OnRelease();
			}
				
		}
		
		protected virtual void OnHold(){}
		
		protected virtual void _OnRelease(){OnRelease();}
		protected virtual void OnRelease(){}
	
	}
}