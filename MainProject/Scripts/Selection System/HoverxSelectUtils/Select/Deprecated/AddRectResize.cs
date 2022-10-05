using UnityEngine;
using System.Collections;

namespace SelectionSystem
{
	public class AddRectResize : SelectRelayAddAbstract {
		// corrects the visual of a drag action
		// responds to mouse position, only while held or pressed
		
		protected delegate void RectResizeDelegate();
		
		public Vector2 grab = Vector2.zero; // value to be read, in case this is necessary
		
		protected float handleDistance = 5f;
		
		protected RectResizeDelegate handleMethod;
		
		protected Vector3 anchorPosition = Vector3.zero;
		
		
		protected Vector3 currentPosition = Vector3.zero;
		protected Vector3 newPosition = Vector3.zero;
		
		protected Vector3 calcAbs = Vector3.zero;
		
		
		
		public RectTransform tRect;
		
		protected virtual void Awake() {
			if (tRect != null) return;
			tRect = GetComponent<RectTransform>();
			
			
			
		}
		
		protected override void OnRelease (){
			base.OnRelease(); // change mouse?
			
			
			handleMethod = null;
			
		}
		
		protected override void OnPress (){
			base.OnPress(); // change mouse?
			
			
			
			currentPosition = Input.mousePosition ;
			
			
			StartHandle();
			
		}
		
		
		protected virtual void StartHandle(){
			// start anchors
			handleMethod = null;
			
			Vector3[] ar = new Vector3[4];
			tRect.GetWorldCorners(ar); // clockwise starting bottomleft
			float xMin = ar[0].x;
			float xMax = ar[2].x;
			float yMin = ar[0].y;
			float yMax = ar[2].y;
			
			grab.x = 0;
			grab.y = 0;
			
			
			
			anchorPosition = ar[0];
			
			if ( currentPosition.x - xMin < handleDistance)
			{
				handleMethod = XHandle;
				//left
				grab.x = -1;
				
				anchorPosition.x = xMax;
			}
			else if (xMax - currentPosition.x < handleDistance)
			{
				//right
				handleMethod = XHandle;
				grab.x = 1;
			}
				
			if (currentPosition.y - yMin < handleDistance)
			{
				if (!(grab.x.IsZero()))
					handleMethod = BothHandle;
				else
					handleMethod = YHandle;
				//bottom
				grab.y = -1;
				anchorPosition.y = yMax;
			}
			else if (yMax - currentPosition.y < handleDistance)
			{
				if (!(grab.x.IsZero()))
					handleMethod = BothHandle;
				else
					handleMethod = YHandle;
				grab.y = 1;
			}
		}
		
		protected virtual void XHandle(){
			
			// I use currentPosition since it's just mouseposition normally
			currentPosition = tRect.sizeDelta;
			currentPosition.x = calcAbs.x;
			tRect.sizeDelta = currentPosition;
			
			
			currentPosition = tRect.anchoredPosition3D;
			
			currentPosition.x = newPosition.x;
			tRect.anchoredPosition3D = currentPosition;
			
			
			
		}
		
		protected virtual void YHandle(){
			
			currentPosition = tRect.sizeDelta;
			currentPosition.y = calcAbs.y;
			tRect.sizeDelta = currentPosition;
			
			
			currentPosition = tRect.anchoredPosition3D;
			
			currentPosition.y = newPosition.y;
			tRect.anchoredPosition3D = currentPosition;
			
		}
		
		protected virtual void BothHandle(){
			
			tRect.sizeDelta = calcAbs;
			
			tRect.anchoredPosition3D = newPosition;
			
		}
		
		
		
		
		protected override void OnLateUpdate() {
			
			base.OnLateUpdate();
			if (handleMethod == null) return;
			if (ih.used) return;
			ih.used = true;
			
			float scaleFactor = transform.lossyScale.y;
			
			currentPosition = Input.mousePosition ;
			
			newPosition = anchorPosition ;
			
			if ( currentPosition.x < anchorPosition.x)
			{
				
				newPosition.x = currentPosition.x;
			}
			
			
				
			if ( currentPosition.y < anchorPosition.y)
			{
				
				newPosition.y = currentPosition.y;
				
			}
			
			calcAbs = (currentPosition - anchorPosition) ;
			
			calcAbs.x = Mathf.Abs(calcAbs.x);
			calcAbs.y = Mathf.Abs(calcAbs.y);
			
			// and set
			/*
			
			transform.sizeDelta = calcAbs / scaleFactor;
			transform.anchoredPosition3D = newPosition / scaleFactor;
			*/
			
			calcAbs /= scaleFactor;
			newPosition /= scaleFactor;
			
			
			
			if (handleMethod != null) handleMethod();
			
			
		}
		
		
	}
}