using UnityEngine;
using UnityEngine.UI;

using System.Collections;


namespace SelectionSystem
{
	
	
	public class DragHold2DIconGhost : SelectAbstract {
		// for dragging an object as close to the edge as I can, with a ghost
		
		public Transform heldThing;
		
		
		public RectTransform ghostOnScreen;
		public RectTransform rectBeingDragged;
		protected Vector3 lastValidPosition = new Vector3();
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
			
			if (ghostOnScreen == null ) 
			{
			ghostOnScreen = heldThing.GetChild(0).GetComponent<RectTransform>();
				lastValidPosition = ghostOnScreen.position;
			}
			tParent = heldThing.parent.GetComponent<RectTransform>();
			
			
			scaleFactor = GetComponentInParent<CanvasScaler>().scaleFactor;
		}
		
		protected void EnableGhost(){
			if (ghostEnabled) return;
			
			if(Vector3.Distance(lastMousePosition, Input.mousePosition) < 8f * scaleFactor ) 
			{
				
				return;
			}
			rectBeingDragged = Instantiate(ghostOnScreen) as RectTransform;
			CanvasGroup ri = rectBeingDragged.GetComponent<CanvasGroup>();
			ri.alpha = .5f;
			rectBeingDragged.SetParent(ghostOnScreen.parent, false);
			
			ghostEnabled = true;
		}
		public bool ghostEnabled = false;
		protected override void OnPress() {
			base.OnPress();
			if (ghostEnabled) return;
			SelectGlobal.uiSelect = true;
			
			lastMousePosition = Input.mousePosition;
			
			
			hitOffset =   heldThing.position - lastMousePosition; // from its center, my offset
			
			
		}
		protected override void OnRelease() {
			base.OnRelease();
			if (! ghostEnabled) return;
			heldThing.position = lastValidPosition;
			Destroy(rectBeingDragged.gameObject);
			rectBeingDragged = null;
			ghostEnabled = false;
		}
		
		
		public void SetVariables() {
			// per frame
			
			scaleFactor = GetComponentInParent<CanvasScaler>().scaleFactor;
			
			localScale = ghostOnScreen.localScale;
			
			
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
			
			Vector3 anchoredPosition = lastMousePosition;
			
			
			
			// need it to not be real
			if(rectBeingDragged!= null) rectBeingDragged.position = anchoredPosition + hitOffset ;
			
			
			//if (Vector3.Distance(anchoredPosition, heldThing.position ) > 0f)
			
		}
		
		public void UpdateBounds() {
			
			// basic drag functions
			
			// avoiding offscreen 
			if (interiorBound) 
			{
				
				SetVariablesRect();
				KeepInRect();
				//KeepToRectEdge();
				
			}
			else if (screenBound) KeepOnScreen(); 
			
			// when within another rect, and it matters
			
		}
		protected override void OnUpdate() {
			
			base.OnUpdate();
			
			if ( !pressed) return;
			SetVariables();
			EnableGhost();	
			
			if ( !ghostEnabled) return;
			
			SetDrag();
			UpdateBounds();
		}
		protected Vector3 localScale;
		
		protected Vector3[] ar = new Vector3[4];
		Vector3 adjustPosition = Vector3.zero;
		protected void SetVariablesScreen() {
			
			// bottomleft circles clockwise
			
			// scalefactor is hopefully correct, it really depends on if the canvas scales
			
			rectBeingDragged.GetWorldCorners(ar); // 
			
			bottom = ar[0].y ;
			left 	= ar[0].x ;
			
			top 	= ar[2].y ;
			right 	= ar[2].x ;
		}
		protected RectTransform tParent;
		protected void SetVariablesRect() {
			
			rectBeingDragged.GetWorldCorners(ar); // 
			
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
		
			rectBeingDragged.position += adjustPosition;
			
			
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
			
			
			if (tParent.rect.height > rectBeingDragged.rect.height * localScale.y)
			newPosition.y = adjustPosition.y;
				
			if (tParent.rect.width > rectBeingDragged.rect.width * localScale.x)
			newPosition.x = adjustPosition.x;
			
			//rectBeingDragged.position += newPosition ;
			
			
			//if (adjustPosition == Vector3.zero)
			lastValidPosition = rectBeingDragged.position + adjustPosition;
				
		}
		
		

		 
	}
}