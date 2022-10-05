
using UnityEngine;
using System.Collections;
using SelectionSystem;

namespace Zone
{
	
	public class OpenZoneSwapParent : InZoneAbstract
	{
		// this is supposed to enable selection of things, or be a selectable
		
		public RectTransform whenOn;
		public RectTransform whenOff;
		
		public bool available = false;
		
		public ConnectHubDelegate onSwap;
		
		protected void CheckAvailable(){
			
			changed = (available != hub.inFocus);
			available = hub.inFocus;
		}
		
		protected override void OnEnable() {
			
			base.OnEnable();
			Initialize();
			
		}
		
		protected override void OnDisable(){
			base.OnDisable();
		}
		protected void Initialize(){
			
			TurnOff();
			OnChange();
		}
		
		
		
		protected bool changed = false;
		protected override void OnChange() {
			
			base.OnChange();
			CheckAvailable();
			
			if (available)
				TurnOn(); 
			else if ( gameObject.activeInHierarchy)
				StartCoroutine("NextUpdate");
			
				
		}
		
		protected IEnumerator NextUpdate(){
			if (!changed) yield break;
			yield return null;
			
			TurnOff();
		}
		protected void TurnOn() {
			StopCoroutine("NextUpdate");
			transform.SetParent(whenOn, false);
		}
		
		protected void TurnOff() {
			transform.SetParent(whenOff, false);
		}
		
	}
}