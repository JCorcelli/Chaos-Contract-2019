using UnityEngine;
using UnityEngine.UI;

using System.Collections;


namespace SelectionSystem
{
	
	
	public class DragHold2D : SelectAbstract {
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
			Vector3 currentPosition = Input.mousePosition;
			Vector3 delta = currentPosition-lastMousePosition;
			delta /= scaleFactor;
			
			if (delta.magnitude > deltaSpeed) 
				lastMousePosition += delta.normalized * deltaSpeed * scaleFactor;
			else
				lastMousePosition = currentPosition;
			
			
			
			
			heldThing.position = lastMousePosition + hitOffset ;
			
		}
		
		public void UpdateBounds() {
			
			// basic drag functions
			
			// avoiding offscreen 
			if (interiorBound) 
			{
				SetVariablesRect();
				KeepInRect();
				KeepToRectEdge();
			}
			else if (screenBound) KeepOnScreen(); 
			
			// when within another rect, and it matters
			
		}
		protected override void OnLateUpdate() {
			
			if (!pressed ) 
			{
				SetVariables();
				UpdateBounds();
				return;
			}
			//if (!pressed) return;
			SetVariables();
			SetDrag();
			UpdateBounds();
		}
		protected Vector3 localScale;
		
		protected Vector3[] ar = new Vector3[4];
		Vector3 adjustPosition = Vector3.zero;
		protected void SetVariablesScreen() {
			
			// bottomleft circles clockwise
			
			// scalefactor is hopefully correct, it really depends on if the canvas scales
			
			rectKeepOnScreen.GetWorldCorners(ar); // 
			
			bottom = ar[0].y ;
			left 	= ar[0].x ;
			
			top 	= ar[2].y ;
			right 	= ar[2].x ;
		}
		protected RectTransform tParent;
		protected void SetVariablesRect() {
			
			rectKeepOnScreen.GetWorldCorners(ar); // 
			
			bottom = ar[0].y  ;
			left 	= ar[0].x  ;
			
			top 	= ar[2].y ;
			right 	= ar[2].x ;
			
			
			tParent.GetWorldCorners(ar); // 
			
			pbottom  = ar[0].y ;
			pleft 	= ar[0].x ;
			
			ptop 	= ar[2].y ;
			pright 	= ar[2].x ;
			
		}
		
		protected void KeepOnScreen(){
			
			adjustPosition = Vector3.zero;
			
			SetVariablesScreen();
			
			// bottomleft circles clockwise
			
			// scalefactor is hopefully correct, it really depends on if the canvas scales
			
			
			if (top > Screen.height)
			adjustPosition.y = Screen.height - top;
			else if (bottom < 0)
			adjustPosition.y = 0 - bottom;
		
			if (left < 0)
			adjustPosition.x = 0 - left;
			else if (right > Screen.width)
			adjustPosition.x = Screen.width - right ;
			
			if (screenBound)
			rectKeepOnScreen.position += adjustPosition ;
			
		}
		protected void KeepInRect(){
			
			
			
			adjustPosition = Vector3.zero;
			
			
			
			
			
			
			// would have same effect as keep in screen, i think?
			
			if (top > ptop)
			adjustPosition.y = ptop - top;
			else if (bottom < pbottom)
			adjustPosition.y = pbottom - bottom;
		
	
	
			if (left < pleft)
			adjustPosition.x = pleft - left;
			else if (right > pright)
			adjustPosition.x = pright - right ;
			
			//no overflow
		
			Vector3 newPosition = Vector3.zero;
			
			
			if (tParent.rect.height > rectKeepOnScreen.rect.height * localScale.y)
			newPosition.y = adjustPosition.y;
				
			if (tParent.rect.width > rectKeepOnScreen.rect.width * localScale.x)
			newPosition.x = adjustPosition.x;
			
			rectKeepOnScreen.position += newPosition ;
				
		}
		
		protected void KeepToRectEdge(){
			
			if (adjustPosition == Vector3.zero) return;
			
			bool bx = (adjustPosition.x != 0);
			bool by = (adjustPosition.y != 0);
				
			
			adjustPosition = Vector3.zero;
			
			
			
			
			
			if (by &&  tParent.rect.height < rectKeepOnScreen.rect.height * localScale.y)
			{
				if (top < ptop)
				adjustPosition.y = ptop - top;
				else if (bottom > pbottom)
				adjustPosition.y = pbottom - bottom;
			}
		
			if (bx && tParent.rect.width < rectKeepOnScreen.rect.width * localScale.x)
			{
				if (left > pleft)
				adjustPosition.x = pleft - left;
				else if (right < pright)
				adjustPosition.x = pright - right ;
			}
			
			
			rectKeepOnScreen.position += adjustPosition ;
		}
		 
	}
}