using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event 


using SelectionSystem;
namespace Zone
{


	public class UISelectMaskOpenZoneSwap : UISelectMask {

	
		// this frame
		// suppress my selectionmanager
		// prevents propagation of pointerdown event
		// suppress buttonhandlers from registering pointer down event
		
		protected OpenZoneSwapParent swapHub;
		
		protected virtual void Swap(){
			
			if (!swapHub.available) Exit();
		}
		protected override void OnEnable() {
			base.OnEnable();
			if (swapHub == null) swapHub = GetComponentInParent<OpenZoneSwapParent>();
			
			if (swapHub == null) 
				Debug.Log("no swapHub", gameObject);
			swapHub.onSwap -= Swap;
			swapHub.onSwap += Swap;
		}
		
		protected override void OnDisable() {
			base.OnDisable();
			Exit();
			swapHub.onSwap -= Swap;
		}

	}
}
