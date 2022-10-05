using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim
{
	public class DatesimAppConnect : SelectAbstract {
		// This is intended for big applications that need to coordinate actions
		
		
		public Image selectedImage;
		public Color pressedColor = Color.white;
		public Color defaultColor = Color.white;
		
		public DatesimHub.Channel channel = DatesimHub.Channel.Option;
		
		public int message = 0;
		
		public DatesimVariables vars;
		
		
		
		protected override void OnEnable( ){
			base.OnEnable();
			if (selectedImage == null)
				
				selectedImage = GetComponentInParent<Image>();
			if (vars == null) 
			{
				vars = GetComponentInParent<DatesimVariables>();
			}
			if (vars == null) {
				Debug.Log("no vars, this broke", gameObject);
				
			}
			else
			{
				
				
				vars.onChange -= OnChange;
				vars.onChange += OnChange;
				vars.onConnect -= OnConnect;
				vars.onConnect += OnConnect;
				
				
			}
			
			
		}
		
		protected virtual void OnChange() {}
		protected virtual void OnConnect(Object ob) {}
		protected virtual void Connect() {vars.Connect(this);}
		
		
		
		protected virtual void Destroy( ){
			if (vars == null) return;
			
			vars.onChange -= OnChange;
		}
		
		
		
		protected override void OnPress() {
			
			
			
		}
		

	}
}