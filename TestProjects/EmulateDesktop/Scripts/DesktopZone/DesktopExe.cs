using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

using System.Collections;
using System.Collections.Generic;
using System;

using Utility.GUI;
using SelectionSystem;

namespace Zone
{
	public class DesktopExe : SelectAbstract {
		// This is a basic formulae of connecting with StaticHub
		public TaskExeHook runexe;
		public bool focused = false;
		
		public Image image;
		public Color defaultColor = new Color(1f,1f,1f,0f);
		public Color hoveredColor = new Color(1f,1f,1f,0.25f);
		public Color focusedColor = new Color(1f,1f,1f,0.4f);
		
		protected string initiator = "";
		protected Vector3 dragStartPos = new Vector3();
		protected override void OnEnable(){
			base.OnEnable();
			
			
			runexe = gameObject.GetComponent<TaskExeHook>();
			if (runexe == null)
			runexe = gameObject.AddComponent<TaskExeHook>();
			
			image = gameObject.GetComponent<Image>();
			
		}
		
		public float clickTime = 0f;
		protected override void OnUpdate(){
			base.OnUpdate();
			clickTime -= Time.deltaTime;
			clickTime = (clickTime > 0f)? clickTime : 0f;
			
			if (Input.GetButtonDown("submit"))
			{
				OnPressEnter();
				return;
			}
			
			if (!isHovered)
			{
				if (Input.GetButtonDown("mouse 1")||Input.GetButtonDown("mouse 2")) 
				{
					focused = false;
					
					clickTime = 0f;
				}
			
			}
			else if (pressed)
			{
				if (Vector3.Distance(dragStartPos, Input.mousePosition) > 5f) clickTime = 0f;
			}
			if (focused)
				image.color = focusedColor;
			else if (isHovered)
				image.color = hoveredColor;
			else
				image.color = defaultColor;
			
		}
		
		
		
			
		protected void OnDoubleClick(){
			
			// double click
			
			runexe.StartExe();
			focused = false;
		}
		protected void OnPressEnter(){
			
			// double click
			if (!focused) return;
			runexe.StartExe();
			focused = false;
		}
		protected void Secondary(){
			// right click 
			focused = true;
			clickTime = 0f;
		}
		protected void Primary(){
			bool notRight = false;
			if (!focused){focused = true; notRight= true;}
			if (clickTime < .01f) {clickTime= 2f; notRight = true; }
			
			if (notRight) return;
			
			clickTime = 0f;
			
			OnDoubleClick();
			
		}
		
		protected override void OnPress(){
			// or double click or something
			if (initiator != buttonPressed) 
				clickTime = 0f;
			initiator = buttonPressed;
			dragStartPos = Input.mousePosition;
			if (Pressing("mouse 1")) Primary();
			else if (Pressing("mouse 2")) Secondary();
			
			
		}

	}
}