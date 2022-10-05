using UnityEngine;

using SelectionSystem.IHSCx;
using SelectionSystem;


namespace Utility.GUI
{
	
	
	public class ObjectDestroyOnClick : SelectAbstract {

		// Use this for initialization
		
		
		public GameObject target;
		
		protected override void OnEnable() {
			base.OnEnable();
			
		
		}
		
		protected override void OnDisable() {
			base.OnDisable();
			
		}
		
		protected override void OnClick()
		{
			
			if (target == null) return;
			GameObject.Destroy(target);
			
		}
		
		
	}
}