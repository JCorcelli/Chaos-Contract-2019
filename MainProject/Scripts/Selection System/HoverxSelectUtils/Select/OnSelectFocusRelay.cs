using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

namespace SelectionSystem
{
	public class OnSelectFocusRelay : SelectNodeAbstract {
		// this sends a signal to a single hub
		
		protected delegate void FocusRelayCallback();
		
		protected OnSelectFocusRelay rh;
		public bool bringToFront = true;
		
		public bool isFocused = false;
		public bool isSelected = false;
		
		
		protected static FocusRelayCallback refocus;
		
		protected override void OnEnable(){
			base.OnEnable();
			refocus += OnRefocus;
			
			ConnectRH();
		}
		protected override void OnDisable(){
			base.OnDisable();
			refocus -= OnRefocus;
			isFocused = false;
			
			
		}
		
		protected void OnRefocus()
		{
			isSelected = false;
			isFocused = false;
		}
			
		protected void ConnectRH() {
			
			rh = transform.parent.GetComponentInParent<OnSelectFocusRelay>();
			
		}
		
		protected void  Focus() {
			if (bringToFront)
				transform.SetSiblingIndex(transform.parent.childCount - 1);
			isFocused = true;
		
			if (rh != null) 
			{
				rh.Focus();
			}
		}
		
		protected override  void  OnPress() { 
			
			// if enabled
			if (refocus != null)
				refocus();
			
			isSelected = true;
			//some select action?
			
			Focus();
			
			
		}
		
		
		
		
		
	}
}