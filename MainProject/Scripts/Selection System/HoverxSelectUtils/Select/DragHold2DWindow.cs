using UnityEngine;
using UnityEngine.UI;

using System.Collections;


namespace SelectionSystem
{
	
	
	public class DragHold2DWindow : SelectAbstract {
		// pick screen or interior
		// edge, overflow, or both
		
		//public Vector2 offscreenLimit = new Vector2(20f, 20f);
		
		public Transform heldThing;
		
		
		public RectTransform rectKeepOnScreen;
		
		protected Vector3 hitOffset; // anchor
		//protected Vector3 viewOffset;
		protected Vector3 lastMousePosition = Vector3.zero;
		public float deltaSpeed = 50f;
		
		public bool screenBound = true;
		public bool interiorBound = false;
		
		
		
		protected float scaleFactor;
		protected float top 	  ;
		protected float bottom  ;
		protected float right ;
		protected float left 	  ;
		protected float ptop 	  ;
		protected float pbottom  ;
		protected float pright ;
		protected float pleft 	  ;
		
		protected override void OnEnable(){
			base.OnEnable();
			if (heldThing == null ) heldThing = this.transform;
			
			tParent = heldThing.parent.GetComponent<RectTransform>();
			
			if (rectKeepOnScreen == null ) 
			{
				rectKeepOnScreen = heldThing.GetComponent<RectTransform>();
			}
		}
		
		protected override void OnPress() {
			base.OnPress();
			SelectGlobal.uiSelect = true;
			
			lastMousePosition = Input.mousePosition;
			
			
			hitOffset =   heldThing.position - lastMousePosition; // from its center, my offset
			
			
		}
		
		
		public void SetVariables() {
			// per frame
			scaleFactor = GetComponentInParent<CanvasScaler>().scaleFactor;
			
			localScale = rectKeepOnScreen.localScale;
			
			
		}
		
		protected void SetDrag() {
			
			// per frame
			
			
			
			Vector3 delta = currentPosition-lastMousePosition;
			delta /= scaleFactor;
			
			if (delta.magnitude > deltaSpeed) 
				lastMousePosition += delta.normalized * deltaSpeed * scaleFactor;
			else
				lastMousePosition = currentPosition;
			
			
			Vector3 anchoredPosition = lastMousePosition;
			
			
			heldThing.position = anchoredPosition + hitOffset ;
			
			
			
		}
		
		public void UpdateBounds() {
			
			// basic drag functions
			
			// avoiding offscreen 
			// when within another rect, and it matters
			if (interiorBound) 
			{
				SetVariablesRect();
				KeepInRect();
				
				return;
			}
			else if (screenBound) KeepOnScreen(); 
			
			
		}
		
		protected override void OnLateUpdate() {
			
			base.OnLateUpdate();
			if (!pressed) return;
			SetVariables();
			UpdateBounds();
			SetDrag();
		}
		protected Vector3 localScale;
		protected Vector3 currentPosition;
		
		protected Vector3[] ar = new Vector3[4];
		protected void SetVariablesScreen() {
			
			// bottomleft circles clockwise
			
			// scalefactor is hopefully correct, it really depends on if the canvas scales
			
			currentPosition = Input.mousePosition;
			
			bottom = currentPosition.y  ;
			left 	=currentPosition.x  ;
			
			top 	= currentPosition.y ;
			right 	= currentPosition.x ;
		}
		protected RectTransform tParent;
		protected void SetVariablesRect() {
			currentPosition = Input.mousePosition;
			
			bottom = currentPosition.y  ;
			left 	=currentPosition.x  ;
			
			top 	= currentPosition.y ;
			right 	= currentPosition.x ;
			
			
			tParent.GetWorldCorners(ar); // 
			
			pbottom  = ar[0].y ;
			pleft 	= ar[0].x ;
			
			ptop 	= ar[2].y ;
			pright 	= ar[2].x ;
			
		}
		
		protected void KeepOnScreen(){
			
			
			SetVariablesScreen();
			
			
			if (top > Screen.height) currentPosition.y = Screen.height;
				
			else if (bottom < 0)
				currentPosition.y = 0;
			if (left < 0)
				currentPosition.x = 0;
			else if (right > Screen.width)
				currentPosition.x = 0;
			
			
			
		}
		protected void KeepInRect(){
			
			
			
			
			if (top > ptop) 
				currentPosition.y = ptop;
				
			else if (bottom < pbottom)
				currentPosition.y = pbottom;
			if (left < pleft)
				currentPosition.x = pleft;
			else if (right > pright)
				currentPosition.x = pright;
			
			
				
		}
		
		
	}
}