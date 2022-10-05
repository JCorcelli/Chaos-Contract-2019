using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace FloorDesigner
{
	public class FloorPlanZoomRect : ConnectHubConnect {
		// This is intended for big applications that need to coordinate actions
		
		
		
		protected FloorPlanZoomBar target;
		protected RectTransform tRect;
		protected RectTransform tParent;
		
		protected float scaleFactor;
		protected float top 	  ;
		protected float bottom  ;
		protected float right ;
		protected float left 	  ;
		protected float ptop 	  ;
		protected float pbottom  ;
		protected float pright ;
		protected float pleft 	  ;
		
		protected override void OnEnable( ){
			base.OnEnable();
			tRect = GetComponent<RectTransform>();
			Connect();
			tParent = tRect.parent.GetComponent<RectTransform>();
			
			Resize();
		}
		
		
		protected override void OnMessage(int senderEnum, int msgEnum) {
			if (senderEnum != (int)FloorPlanHub.Channel.Default) return;
			// every time something calls a message, in scope, this will check it
			
			// Direct < -- FloorPlanHub.Enum.ZoomBar
			
			
			if (msgEnum == (int)FloorPlanHub.Enum.ResizeHub)
			{
				
				SetOffset(); // based on parent
				Resize();
				CorrectOffset(); // based on parent
		
				hub.Send( (int)FloorPlanHub.Channel.Default, (int)FloorPlanHub.Enum.ZoomRect);
					
			}
			
		}
		protected override void OnConnect(Object other) {
			// FloorPlanHub.Enum.ZoomBar, probably don't need it other than to sync
			
			GameObject ob = (GameObject)other;
			
			target = ob.GetComponent<FloorPlanZoomBar>();
			
			if (target != null)
				connected = ob;
			else
				Debug.Log("Something wrong with zoomBar", gameObject);
				
		}
		
		protected float zoom = 1f;
		protected float fit = 1f;
		protected Vector3 newScale;
			
		
		protected void SetVariables() {
			
			scaleFactor = transform.lossyScale.y;
			
			localScale = tRect.localScale;
			
		}
		
		protected Vector3[] ar = new Vector3[4];
		protected Vector3 localScale;
		
		protected Vector3 hitOffset;
		protected Vector3 oldPos;
		protected Vector3 truePos;
		protected Vector3 offset;
		protected void SetOffset() {
			// it's zooming from the center of the parent regardless of anchor. Not bad.
			
			// tip:
			// There's another method which uses an anchor point in the rect, and after resizing adjust the difference.
			
			oldPos = tRect.localPosition;
			offset = oldPos / fit / zoom;
			
			
			
		}
		protected void CorrectOffset() {
			truePos = offset * fit * zoom;
			
			tRect.localPosition = truePos;
		}
		protected void SetVariablesRect() {
			
			tRect.GetWorldCorners(ar); // 
			
			bottom = ar[0].y  ;
			left 	= ar[0].x  ;
			
			top 	= bottom + tRect.rect.height * localScale.y * scaleFactor;
			right 	= left + tRect.rect.width * localScale.x * scaleFactor;
			
			// the parent
			
			
			tParent.GetWorldCorners(ar); 
			
			pbottom  = ar[0].y ;
			pleft 	= ar[0].x ;
			
			ptop 	= ar[2].y ;
			pright 	= ar[2].x ;
			
		}
			
		
		protected void Resize() {
			
			if (tRect == null) return;
			SetVariables();
			SetVariablesRect();
			
			
			Rect rect =tRect.parent.GetComponent<RectTransform>().rect;
			
			float bigger = Mathf.Min(rect.width , rect.height) ;
			
			fit = bigger / tRect.rect.width;
			
			localScale.x = localScale.y = fit * zoom;
			
			tRect.localScale = localScale;
			
		}
		public void SetZoom(float f){
			// the goal of this is to zoom around a pivot point
			if (tRect == null) return;
			SetOffset(); // based on parent
			zoom = f;
			Resize();
			CorrectOffset();
			hub.Send((int)FloorPlanHub.Channel.Default, (int)FloorPlanHub.Enum.ZoomRect);
		}
		

	}
}