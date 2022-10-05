using UnityEngine;


using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

using Utility;
using Utility.GUI;
using SelectionSystem;

namespace Zone
{
	public class WindowCanvasHook : StaticHubConnect {
		// This is a basic formulae of connecting with StaticHub
		
		protected string targetName = "";
		protected  TaskbarListener target;
		
		protected Canvas canvas;
		protected GraphicRaycaster raycaster;
		protected CanvasGroup canvasGroup;

		public bool running = false;
		public bool maximized = true;
		public bool focused = false;
		public override void CheckConnected(){
			if (!subscribed) SubscribeHub();
			
			connected = (target != null);
			
			if (connected) return;
			//if (_hook) 
			Ping();
		}
		protected void Init () {
			canvas = AddCanvas.Util(transform);
			raycaster = gameObject.GetComponent<GraphicRaycaster>();
			canvasGroup = gameObject.GetComponent<CanvasGroup>();
			
			
		}
	
		protected override void OnEnable( ){
			base.OnEnable();
			Init ();
			
			CheckConnected();
			
			
		}
		
		
		protected override void OnConnect(object ob) {
			// for hooks
			if (ob.GetType() == typeof(TaskbarListener) )
			{
				target = ((TaskbarListener)ob);
				target.connected = connected = true;
				
				
				onChange += target.OnChange;
				target.onChange += OnChange;
				
				OnChange();
				
			}
		}
		
		public override void OnChange() {
			// behavior
			running = target.running;
			maximized = target.maximized;
			focused = target.focused;
			canvasGroup.interactable = canvasGroup.blocksRaycasts = raycaster.enabled = canvas.enabled = running && maximized;
			if (!canvas.enabled) canvasGroup.alpha = 0f;
			else
				canvasGroup.alpha = 1f;
			if (focused)
				transform.SetSiblingIndex(transform.childCount);
			
		}
		

	}
}