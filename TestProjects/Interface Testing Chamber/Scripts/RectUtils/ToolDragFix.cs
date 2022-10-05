using UnityEngine;
using System.Collections;

namespace SelectionSystem
{
	public class ToolDragFix : AbstractButtonHandler {
		// corrects the visual of a drag action
		// responds to mouse position, only while held or pressed
		
		protected Vector3 startPosition = Vector3.zero;
		protected Vector3 scale = Vector3.one;
		protected Vector3 anchorPosition = Vector3.one;
		
		
		public RectTransform toolRect;
		
		protected override void OnDisable() {
			base.OnDisable();
			toolRect.gameObject.SetActive(false);
		}
		
		protected UISelectMask sMask;
		
		protected override void OnEnable() {
			base.OnEnable();
			sMask = GetComponentInParent<UISelectMask>();
			
			if (toolRect == null) toolRect = transform.GetChild(0).GetComponent<RectTransform>();
			
			if (toolRect == null) Debug.Log("tool rect messed up", gameObject);
				
			toolRect.gameObject.SetActive(false);
			
		}
		
		
		protected override void _OnPress(){
			base._OnPress();
			
			
			if (!pressed)  toolRect.gameObject.SetActive(false);
		}
		protected override void OnPress (){
			
			
			if (!sMask.isHovered) 
			{
				pressed = false; return;
			}
			
			
			toolRect.gameObject.SetActive(true);
			
			startPosition = Input.mousePosition;
			
			toolRect.position = startPosition;
			
			toolRect.sizeDelta = Vector3.zero;
		}
		
		protected Vector2 pivot = Vector2.zero;
		
		protected override void OnHold () {
			
			Vector3 currentPosition =  Input.mousePosition;
			
			anchorPosition = toolRect.anchoredPosition3D;
			
			Vector3 direction = (currentPosition - startPosition ).normalized;
			
			float direction_dot_right = Vector3.Dot(toolRect.parent.right, direction);
			
			if ( direction_dot_right < 0)
			{
				// pointing left from parent toolRect's angle
				scale.x = -1f;
				
				pivot.x = 1;
				
				
				
			}
			else
			{
				// pointing right from parent toolRect's angle
				scale.x = 1f;
				
				pivot.x = 0;
				
				
				
			}
			
			float direction_dot_up = Vector3.Dot(toolRect.parent.up, direction);
				
			if ( direction_dot_up < 0)
			{
				// pointing down from parent toolRect's angle
				
				scale.y = -1f;
				
				pivot.y = 1;
				
				
				
			}
			else
			{
				
				scale.y = 1f;
				
				pivot.y = 0;
				
				
				
			}
			
			toolRect.anchoredPosition3D = anchorPosition ;
			
			toolRect.pivot = pivot;
			
			
			Vector3 calcAbs = (currentPosition - startPosition) ;
			
			
			
			
			toolRect.sizeDelta = new Vector3(0f, calcAbs.magnitude /  toolRect.lossyScale.y);
			
			
			float addy = 0f;
			
			if (scale.y < 0) addy = 180;
			
			
			float angle = -scale.x * Vector3.Angle(toolRect.parent.up, calcAbs) ;
			
			angle -= addy;
			
			//if (Input.GetButton("shift"))
			//{
			angle = 90f * Mathf.Round(angle / 90f );
			//}
			
			
			// setting toolRect
			
			Vector3 eulerAngles = toolRect.localEulerAngles;
			
			eulerAngles.z = angle;
			
			toolRect.localEulerAngles = eulerAngles;
			
		}
		
		
	}
}