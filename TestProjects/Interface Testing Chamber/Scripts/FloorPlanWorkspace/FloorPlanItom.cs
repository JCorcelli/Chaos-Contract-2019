using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace FloorDesigner
{
	public class FloorPlanItom : ConnectHubConnect, IBool, IMessage {
		// This is intended for big applications that need to coordinate actions
		
		protected Image image;
		
		public FloorPlanHub.Channel channel = FloorPlanHub.Channel.Default;
		
		public FloorPlanHub.Enum message =FloorPlanHub.Enum.Itom;
		
		[System.Flags]
		public enum ItomType
		{
			
			Misc 	= 1,
			Wall 	= 1 << 1,
			Room	= 1 << 2
		}
		public ItomType itomType = (ItomType)1;
		
		public bool IsMasked(int other)
		{
			return other.IsMasking((int)itomType); // I thin it's the same either way otherismasked itomismasked
		}
		
		public int GetMessage() { return (int)message;}
		public bool GetBool() { return isVisible;}
		
		protected bool isVisible = false;
		protected RectTransform tRect;
		protected static Vector3 lossyScale; // the whole point is the parent is scaling
		protected float itomMin = 2.5f; // that's 2 pixels
		
		
		protected RectTransform tParent;
		
		protected static float scaleFactor;
		protected static float top 	  ;
		protected static float bottom  ;
		protected static float right ;
		protected static float left 	  ;
		protected static float ptop 	  ;
		protected static float pbottom  ;
		protected static float pright ;
		protected static float pleft 	  ;
		protected static Vector3[] ar = new Vector3[4] 	  ;
		
		
		protected override void OnEnable( ){
			base.OnEnable();
			
			tRect = GetComponent<RectTransform>();
			ItomMask i = tRect.parent.GetComponentInParent<ItomMask>();
			
			if (i == null) 
			{
				Debug.Log("not itom", gameObject);
				
				Object.Destroy(this);
				return;
			}
			
			tParent = i.GetComponent<RectTransform>();
			SetVariables();
			
			
			Connect();
			image = GetComponent<Image>();
			
			
		}
		
		
		
		protected void SetVariables() {
			
			
			scaleFactor = transform.lossyScale.y;
			
			lossyScale = tRect.lossyScale; 
			//pLossyScale = tParent.lossyScale; 
			
		}
		
		
		protected void SetVariablesWorld() {
			
			tRect.GetWorldCorners(ar); // 
			
			bottom = ar[0].y  ;
			left 	= ar[0].x  ;
			
			top 	= ar[2].y ;
			right 	= ar[2].x ;
			
			// the parent
			
			
			tParent.GetWorldCorners(ar); 
			
			pbottom  = ar[0].y ;
			pleft 	= ar[0].x ;
			
			ptop 	= ar[2].y ;
			pright 	= ar[2].x ;
			
		}
		protected void SetWithinMask()
		{
			
			if (bottom > ptop + tRect.rect.height * tRect.lossyScale.y||
			left > pright + tRect.rect.width * tRect.lossyScale.x ||
			right < pleft - tRect.rect.width * tRect.lossyScale.x|| 
			top < pbottom - tRect.rect.height * tRect.lossyScale.y) isVisible = false;
			else
				isVisible = true;
			
			return;
		}
		
		public void ShowItom(bool b) {
			image.enabled = b;
		}
		
		protected void SetVisible() {
			SetVariables();
			SetVariablesWorld();
			//incomplete
			
			isVisible = false;
			SetWithinMask();
			
			
			if (!isVisible) 
			{
				ShowItom(false);
				return;
			}
			// Itom should be within the visible rect mask
			// if not in mask, if visible showItom false, return;
			
			// Itom should be big enough based on its rect
			
			
			if (tRect.rect.width * scaleFactor * lossyScale.x >= itomMin) ShowItom(true);
			else ShowItom(false);
		}
		protected override void OnMessage(int senderEnum, int msgEnum) {
			
			// every time something calls a message, in scope, this will check it
			if (senderEnum != (int)channel) return;
			
			if ( msgEnum == (int)FloorPlanHub.Enum.Ping) 
			{
				
				Connect();
			}
			
			else if ( msgEnum == (int)FloorPlanHub.Enum.ZoomRect || 
			msgEnum == (int)FloorPlanHub.Enum.ResizeHub || 
			msgEnum == (int)FloorPlanHub.Enum.DragHold) 
			{
				
				SetVisible();
			} // issue? I can just be connected, and not take messages
		}
		protected override void OnConnect(Object other) {
			// FloorPlanHub.Enum.ZoomBar, probably don't need it other than to sync
			
		}
		
		

	}
}