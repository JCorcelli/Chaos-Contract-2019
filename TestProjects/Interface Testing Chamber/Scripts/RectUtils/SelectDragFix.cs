using UnityEngine;
using System.Collections;

using UnityEngine.EventSystems;

namespace SelectionSystem
{
	public class SelectDragFix : AbstractButtonComboPrecision  {
		// corrects the visual of a drag action
		// responds to mouse position, only while held or pressed
		
		protected Vector3 startPosition = Vector3.zero;
		protected Vector3 localPosition = Vector3.zero;
		protected Vector3 anchorPosition;
		
		protected Vector3 scale = Vector3.one;
		
		protected Vector2 anchorMax = Vector2.zero;
		protected Vector2 anchorMin = Vector2.zero;
		
		protected RectTransform rectTransform;
		protected RectTransform south;
		protected RectTransform west;
		protected RectTransform north;
		protected RectTransform east;
		
		protected void Awake() {
			
			rectTransform = GetComponent<RectTransform>();
			

			south = rectTransform.Find("SLine").GetComponent<RectTransform>();

			west = rectTransform.Find("WLine").GetComponent<RectTransform>();
			
			north = rectTransform.Find("NLine").GetComponent<RectTransform>();
			
			east = rectTransform.Find("ELine").GetComponent<RectTransform>();
			
			if (north == null) Debug.Log("Can't find line", gameObject);
			if (south == null) Debug.Log("Can't find line", gameObject);
			if (east == null)  Debug.Log("Can't find line", gameObject);
			if (west == null)  Debug.Log("Can't find line", gameObject);
			
		}
		
		
		protected UISelectMask sMask;
		
		protected override void OnEnable() {
			base.OnEnable();
			sMask = GetComponentInParent<UISelectMask>();
		}
		
		protected bool pressed = false;
		protected override void OnRelease() {
			pressed = false;
		}
		protected override void OnPress(){
			if (sMask.isHovered) 
			{
				pressed = true;
			}
			else
			{
				pressed = false;
				return;
			}
			
			startPosition = Input.mousePosition;
			
			rectTransform.position = startPosition;
			
			rectTransform.sizeDelta = Vector3.zero;
		}
		
		protected Vector2 pivot = Vector2.zero;
		
		protected override void OnHold () {
			if (!pressed) return;
			
			Vector3 currentPosition =  Input.mousePosition;
			
			anchorPosition = rectTransform.anchoredPosition3D;
			
			Vector3 calcAbs = (currentPosition - startPosition) ;
			
			if (calcAbs.magnitude < 2f) return;
			
			if ( currentPosition.x < startPosition.x)
			{
				scale.x = -1f;
				
				pivot.x = 1;
			}
			else
			{
				scale.x = 1f;
				
				pivot.x = 0;
				
			}
			
				
			if ( currentPosition.y < startPosition.y)
			{
				
				
				scale.y = -1f;
				
				
				
				pivot.y = 1;
			}
			else
			{
				
				scale.y = 1f;
				
				pivot.y = 0;
			}
			
			rectTransform.anchoredPosition3D = anchorPosition ;
			
			rectTransform.pivot = pivot;
			
			SetLines();
			// in this setup this is just like setting size in rectransform
			
			
			calcAbs.x = Mathf.Abs(calcAbs.x);
			calcAbs.y = Mathf.Abs(calcAbs.y);
			rectTransform.sizeDelta = calcAbs /  rectTransform.lossyScale.y;
			//rectTransform.SizeDelta = Vector3.Scale(rectTransform.lossyScale);
			// rectTransform.position = startPosition;
			
			// this'll make it stand up
			Vector3 eulerAngles = rectTransform.eulerAngles;
			
			eulerAngles.z = 0f;
			
			rectTransform.eulerAngles = eulerAngles;
		}
		
		protected void SetLines(){
			// fix the dots!
			
			
			
			if (north != null) north.localScale = new Vector3(scale.x, 1f, 1f);
			
			if (south != null) south.localScale = new Vector3(scale.x, 1f, 1f);
			
			if (east != null) east.localScale = new Vector3(1f, scale.y, 1f);
			if (west != null) west.localScale = new Vector3(1f, scale.y, 1f);
		}
	}
}