using UnityEngine;
using System.Collections;

namespace SelectionSystem
{
	
	public class ResizeHub : SelectRouter {
		// corrects the visual of a drag action
		// responds to mouse position, only while held or pressed
		
		protected delegate void RectResizeDelegate();
		
		public Vector2 sizeMin = new Vector2(20f, 140f);
		
		public Vector2 grab = Vector2.zero; // value to be read, in case this is necessary
		
		protected float handleDistance = 5f;
		
		protected RectResizeDelegate handleMethod;
		
		protected Vector3 anchorPosition = Vector3.zero;
		
		
		protected Vector3 currentPosition = Vector3.zero;
		protected Vector3 newPosition = Vector3.zero;
		
		protected Vector3 calcAbs = Vector3.zero;
		protected Vector2 pivot = Vector2.zero;
		
		
		
		public RectTransform tRect;
		
		protected virtual void Awake() {
			if (tRect != null) return;
			tRect = GetComponent<RectTransform>();
			
			
			
		}
		
		public override void OnRelease (){
			base.OnRelease(); // change mouse?
			
			
			handleMethod = null;
			
			
		}
		
		public override void OnPress (){
			base.OnPress(); // change mouse?
			
			
			
			
			currentPosition = Input.mousePosition ;
			
			
			StartPivot();
			StartHandle();
			
			
			
		}
		
		protected virtual void StartPivot() {
			
			anchorPosition = tRect.anchoredPosition3D;
			
			if (grab.x < -.1f)
			{
				
				if (tRect.pivot.x < .1f)
				{
					anchorPosition.x = anchorPosition.x + tRect.sizeDelta.x;
				}
				pivot.x = 1;
				
				
				
			}
			else if (grab.x > .1f)
			{
				if (tRect.pivot.x > .9f)
				{
					anchorPosition.x = anchorPosition.x - tRect.sizeDelta.x;
				}
				
				pivot.x = 0;
			}

			
			
			if (grab.y < -.1f)
			{
				
				if (tRect.pivot.y < .1f)
				{
					anchorPosition.y = anchorPosition.y + tRect.sizeDelta.y;
				}
				
				pivot.y = 1;
				
				
			}
			else if (grab.y > .1f)
			{
				if (tRect.pivot.y > .9f)
				{
					anchorPosition.y = anchorPosition.y - tRect.sizeDelta.y;
				}
				
				pivot.y = 0;
			}
			
			tRect.anchoredPosition3D = anchorPosition ;
			
			tRect.pivot = pivot;
		}
		
		
		protected virtual void StartHandle(){
			// start anchors
			handleMethod = null;
			
			Vector3[] ar = new Vector3[4];
			tRect.GetWorldCorners(ar); // clockwise starting bottomleft
			//float xMin = ar[0].x;
			
			//float xMin = ar[0].x;
			float xMax = ar[2].x;
			//float yMin = ar[0].y;
			float yMax = ar[2].y;
			
			anchorPosition = ar[0];
			
			
			if ( grab.x < -.1f)
			{
				handleMethod = XHandle;
				//left
				
				anchorPosition.x = xMax;
			}
			else if (grab.x > .1f)
			{
				//right
				handleMethod = XHandle;
				
			}
				
			if (grab.y < -.1f)
			{
				if (!(grab.x.IsZero()))
					handleMethod = BothHandle;
				else
					handleMethod = YHandle;
				//bottom
				
				anchorPosition.y = yMax;
			}
			else if (grab.y > .1f)
			{
				if (!(grab.x.IsZero()))
					handleMethod = BothHandle;
				else
					handleMethod = YHandle;
				
			}
		}
		
		protected virtual void XHandle(){
			
			// I use currentPosition since it's just mouseposition normally
			currentPosition = tRect.sizeDelta;
			currentPosition.x = calcAbs.x;
			
			tRect.sizeDelta = currentPosition;
			
			
			
			
		}
		
		protected virtual void YHandle(){
			
			currentPosition = tRect.sizeDelta;
			currentPosition.y = calcAbs.y;
			tRect.sizeDelta = currentPosition;
			
			
		}
		
		protected virtual void BothHandle(){
			
			tRect.sizeDelta = calcAbs;
			
			
			
		}
		
		
		
		
		protected override void OnLateUpdate() {
			
			base.OnLateUpdate();
			if (handleMethod == null) return;
			
			float scaleFactor = transform.lossyScale.y;
			
			currentPosition = Input.mousePosition  ;
			
			
			// can also be used to keep in rect
			if (currentPosition.x < 0) currentPosition.x = 0;
			else if (currentPosition.x > Screen.width ) currentPosition.x = Screen.width ;
			
			if ( currentPosition.y < 0 ) currentPosition.y = 0; 
			else if (currentPosition.y > Screen.height) currentPosition.y = Screen.height;
			
			
			calcAbs = (currentPosition  - anchorPosition ) ;
			
			calcAbs /= scaleFactor;
			
			calcAbs = Vector2.Scale(grab, calcAbs) ;
			
			
			// catch out of bounds
			
			
			if (calcAbs.x < sizeMin.x) calcAbs.x = sizeMin.x;
			
			
			if (calcAbs.y < sizeMin.y) calcAbs.y = sizeMin.y;
			
			
			if (handleMethod != null) handleMethod();
			
			
		}
		
		
	}
}