using UnityEngine;
using System.Collections;


namespace SelectionSystem
{
	public abstract class AbstractButtonCombo : UpdateBehaviour
	{

		public string[] buttonCombo = new string[]{"mouse 1", "shift"};
		
		protected bool comboPressed = false;
		
		protected void ComboCheck(){
			// checking combo
			foreach (string button in buttonCombo)
			{
				if (!Input.GetButton( button) ){
					if (comboPressed) OnRelease();
					comboPressed = false;
					return;
				}
			}
			if (comboPressed) return; // no repeat
			comboPressed = true;
			OnPress();
				
		}
		protected virtual void OnPress(){}
		
		
		
		protected override void OnUpdate() {
			ComboCheck();
			
			if (comboPressed)
				OnHold();
		}
			
		protected virtual void OnHold(){}
		
		protected virtual void OnRelease(){}
	
	}
}