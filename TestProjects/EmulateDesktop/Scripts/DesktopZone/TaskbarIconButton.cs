using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

using Utility.GUI;
using SelectionSystem;

namespace Zone
{
	public class TaskbarIconButton : SelectAbstract {
		
		public bool focused = false;
		
		public bool maximized = false;
		
		public TaskbarListener target;
		public Canvas canvas;
		
		public Image hoverImage;
		protected override void OnUpdate(){
			base.OnUpdate();
			if (hoverImage == null) return;
			if (isHovered && !pressed)
				hoverImage.enabled = true;
			else
				hoverImage.enabled = false;
		}
		protected override void OnEnable(){
			base.OnEnable();
			
			//canvas = Utility.AddCanvas.Util(transform);
		
		}
		public void OnChange(){
			focused = target.focused;
			maximized = target.maximized;
			gameObject.SetActive(target.running);
		}
		public string initiator = "";
		protected override void OnPress(){
			initiator = buttonPressed;
		}
		protected override void OnClick(){
			
			if (target == null) return;
			
			if (initiator == "mouse 1")
				Calculate();
			
			else if (initiator == "mouse 2")
				Popup();
			
			
			target.OnChange();
		}
		protected void Calculate(){
			focused = target.focused = !focused;
			if (focused) 
			{
				Maximize();
			}
			else
				Minimize();
		}
		
		protected void Maximize(){
			
			target.message = "max";
		}
		protected void Minimize(){
			target.message = "min";
			
		}
		protected void CloseApp(){
			target.message = "close";
			
		}
		
		protected void Popup(){
			focused = true;
			// make widget.
			/*min,max,move,close*/
		}
	}
}
