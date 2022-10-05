using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim
{
	public class DatesimAppConnectLite : UpdateBehaviour {
		// This is intended for big applications that need to coordinate actions
		
		
		
		public DatesimVariables vars;
		
		
		
		protected override void OnEnable( ){
			base.OnEnable();
			
			
			if (vars == null) 
			{
				vars = GetComponentInParent<DatesimVariables>();
			}
			if (vars == null) {
				Debug.Log("no vars, this broke", gameObject);
			}
			else
			{
				vars.onChange += OnChange;
				vars.onConnect += OnConnect;
			}
			
		}
		
		protected virtual void Connect() {vars.Connect(this);}
		protected virtual void OnConnect(Object ob) {}
		protected virtual void OnChange() {}
		
		
		
		protected virtual void Destroy( ){
			if (vars == null) return;
			
			vars.onChange -= OnChange;
			vars.onConnect -= OnConnect;
		}
		
		

	}
}