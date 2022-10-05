using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim
{
	public class DatesimDesktopConnect : SelectAbstract {
		// This is intended for big applications that need to coordinate actions
		
		
		public Image selectedImage;
		public Color pressedColor = Color.white;
		public Color defaultColor = Color.white;
		
		
		
		public int message = 0;
		
		public DatesimVariables vars {get {return hook.vars;}set{}}
		
		public DatesimDesktopHook hook; 
		
		
		protected override void OnEnable( ){
			base.OnEnable();
			if (hook == null)
			{
				hook = GetComponentInParent<DatesimDesktopHook>();
			}
			if (hook == null) {
				Debug.Log("no hook, this broke", gameObject);
				return; 
			}
			
			hook.onChange -= OnChange;
			hook.onChange += OnChange;
			
			
			
		}
		
		protected virtual void OnChange() {
			
		}
		
		
		protected virtual void Destroy( ){
			if (hook == null) return;
			
			hook.onChange -= OnChange;
		}
		
		
		
		protected override void OnPress() {
			
			
			
		}
		

	}
}