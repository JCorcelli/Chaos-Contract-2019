using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

using Utility.GUI;
using SelectionSystem;

namespace Zone
{
	public class TaskbarListener : StaticHubConnect{
		// This is a basic formulae of connecting with StaticHub
		
		public bool running = false;
		public bool focused = false;
		public bool maximized = false;
		public string message = "";
		public TaskbarIconButton button;
		
		public DesktopHub powerHub;
		
		public Color defaultColor;
		public Color focusedColor;
		
		protected Image image;
		public Image hoverImage;
		public override void CheckConnected(){
			if (!subscribed) SubscribeHub();
			
			Connect();
		}
		protected override void OnEnable( ){
			base.OnEnable();
			image = GetComponent<Image>();
			powerHub = GetComponentInParent<DesktopHub>();
			
			powerHub.onMessage += OnMessage;
			
			button = gameObject.GetComponent<TaskbarIconButton>();
			if (button == null)
				button = gameObject.AddComponent<TaskbarIconButton>();
			
			button.target = this;
			button.hoverImage = hoverImage;
			onChange += button.OnChange;
			CheckConnected();
			OnChange();
		}
		
		
		protected void PowerOff(){
			if(!running) return;
			message = "close";
			OnChange();
		}
		public void StartExe(){
			running = true;
			message = "max";
			OnChange();
		}
		
		public bool runAtStart = false;
		protected void OnMessage(int channel, int message) {
			
			if (channel != 0) return;
			if (message == 0) PowerOff();
			else if (message == 1 && runAtStart) StartExe();
		}
		protected override void OnConnect(object ob) {
			
		}
		
		protected void Maximize(){
			
			
			focused = maximized = button.maximized= true;
		}
		protected void Minimize(){
			focused = maximized = button.maximized= false;
			
		}
		
		protected void CloseApp(){
			
			running = focused = button.focused = false;
		}
		
		public override void OnChange() {
			// listener ex.
			
			if(running) 
			{
				
				if (message == "min")
					Minimize();
				else if (message == "max")
					Maximize();
				else if (message == "close")
					CloseApp();
				
				message = "";
			}
			else
			{
				button.focused = false;
			}
			
			if (focused) image.color = focusedColor;
			else
				image.color = defaultColor;
			
			if (onChange != null) onChange();
		}
		

	}
}