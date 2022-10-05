using UnityEngine;
using System.Collections;


namespace SelectionSystem.IHSCx
{
	
	
	public class IHSCxConnectCombo : AbstractButtonComboPrecision {
		
		public HSCxController controller {set{ ih = value; } get{ return ih;}
		}
		
		protected HSCxController ih;
		
		protected override void OnEnable(){
			base.OnEnable();
			Connect();
		}
		protected void Connect() {
				
			ih = gameObject.GetComponentInParent<HSCxController>();
			if (ih == null) Debug.LogError(name + "has no HSCxController");
			
		}
		
		 
	}
}