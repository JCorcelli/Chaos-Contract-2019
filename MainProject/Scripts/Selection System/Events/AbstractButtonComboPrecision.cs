using UnityEngine;
using System.Collections;


namespace SelectionSystem
{
	public abstract class AbstractButtonComboPrecision : UpdateBehaviour
	{

		public string buttonName = "mouse 1";
		public string[] buttonCombo = new string[0];
		
		
		public bool combo_held = false;
		public bool bad_combo = false;
		public bool button_held = false;
		
		
		protected override void OnDisable(){
			
			base.OnDisable();
			combo_held = false;
			button_held = false;
			bad_combo = false;
			
		}
		protected override void OnUpdate() {
			// release all buttons, reset
			if (!Input.anyKey) 
			{
				bad_combo = false;
				button_held = false;
				
			}
			else
			// pressing this button might finish the combo
			button_held = Input.GetButton(buttonName);
			
			// check if I pressed any other button before button
			if (!combo_held && Input.anyKeyDown) 	
				ComboBreaker();
			
			// either I finished or.. bad combo
			if (button_held)
				ComboCheck();
			
			
			
			if (combo_held )
			{
				_OnHold();
			}
		}
		
		protected virtual void ComboCheck(){
			// checking combo to make sure all buttons are down
			if (bad_combo || combo_held) return;
			
			foreach (string button in buttonCombo)
			{
				if (!Input.GetButton( button) ){
					combo_held = false;
					bad_combo = true;
					return;
				}
			}
			
			combo_held = true;
			OnPress();
				
		}
		protected void ComboBreaker(){
			// checking combo
			
			if (Input.GetButtonDown(buttonName)) return;
			
			foreach (string button in buttonCombo)
			{
				if (Input.GetButtonDown( button) ){
					return;
				}
			}
			
			
			// something besides one of the combo buttons was pressed, cancels the combo
			bad_combo = true;
		}
		
		protected virtual void OnPress(){}
		
		
			
		protected virtual void _OnHold(){
			
			if (!button_held )
			{
				combo_held = false;
				OnRelease();
			}
			else
				OnHold();
		}
		
		protected virtual void OnHold(){}
		
		protected virtual void OnRelease(){}
	
	}
}