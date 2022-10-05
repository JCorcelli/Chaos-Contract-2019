using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;// Required when using Event data.

using System.Collections;
using SelectionSystem;

namespace Utility.GUI
{
	
	public class GenericListScrollHandler : UpdateBehaviour
	{
		protected RectTransform rectTransform;
		
		// optional delta speed?
		
		protected GroupSelection group;
		public Scrollbar sb;
		public RectTransform cursor;
		public RectTransform listElement;
		
		protected override void OnEnable() {
			rectTransform = GetComponent<RectTransform>();
			group = GetComponentInParent<GroupSelection>();
			if (sb == null) Debug.Log("no scrollbar"); 
			if (group != null)
			{
				
				base.OnEnable();
			}
			
		}
		protected override void OnDisable() {
			if (group != null)
				base.OnDisable();
			
		}
		
		protected Vector3 axis = Vector3.up; // changing this, and some additional programming @Drag could allow it to pan 
		
		public bool dragEnabled = false;
		
		protected GameObject followedTarget;
		protected GameObject prevSelection;

		protected override void OnUpdate() {
			dragEnabled = FillsPanel();
			if (!dragEnabled)
				ToTop();
			
			
			bool hasFocus = group.selected == EventSystem.current.currentSelectedGameObject;
			
			if (group == null || group.selected == null) 
			{
				if (cursor.gameObject.activeSelf)
					cursor.gameObject.SetActive(false);
				return;
			}
			
			if (group.dirty && dragEnabled)
			{
				// I was having trouble getting the menu to follow exactly, so instead it'll check if the selection changed.
				
				// case 1. look for the cursor
				FollowSelectedPosition();
				
			}
			
			ScrollbarToSelectedPosition();
			
			
			if (!hasFocus) return;
			
			dragging = false;
			if (dragEnabled) CheckDrag();
			
			//if (!dragging)
			//{
				
			//		
			//	
			//}
			
			
		}
		
		protected Vector3 lastMousePosition = Vector3.zero;
		
		public float deltaSpeed = 5f;
		
		
		public bool dragging = false;
		protected void CheckDrag() {
			dragging = Input.GetButton("mouse 1");
			
			if (Input.GetButtonDown("mouse 1"))
			{
				lastMousePosition = Input.mousePosition;
				
			
				Drag();
			}
			else if (dragging)
			{				
				Drag();
			}
			
		}
		
		public void ToTop () {
			rectTransform.position = rectTransform.parent.position;
		}
		public void ToBottom () {
			SetScrollPercent(1);
		}
        protected void Drag()
        {
			
			
			
			Vector3 currentPosition = Input.mousePosition;
			Vector3 delta = currentPosition-lastMousePosition;
			
			if (delta.magnitude > deltaSpeed) 
			{
				delta = delta.normalized * deltaSpeed ;
				lastMousePosition += delta;
			}
			else
			{
				lastMousePosition = currentPosition;
				
			}
			
			
			
			
			Vector3 newPos = rectTransform.position + Vector3.Scale(delta, axis);
			
			
			float totalHeight = GetListHeight();
			if (totalHeight.IsZero() ) return;
			float lowLimit = rectTransform.parent.transform.position.y;
			
			float upLimit = totalHeight + lowLimit;
			
			if (lowLimit > newPos.y) 
			{
				newPos.y = lowLimit;
				
			}
			
			else if (upLimit < newPos.y)
			{
				newPos.y = upLimit;
				
			}
			
			rectTransform.position = newPos;
			
			
        }
		
		
		
		public float GetSingleHeight() {
			
			
			float height = listElement.rect.height;
			
			return height * GetComponentInParent<Canvas>().scaleFactor;
		}
		
		public void FollowSelectedPosition() {
			
			
			Transform selected = group.selected.transform;
			
			RectTransform box = rectTransform.parent as RectTransform;
			
			float increment = GetSingleHeight();
			
			// boxHeight requires the pivot for both to be at the bottom
			Vector3 boxHeight = (Vector3.up * box.rect.height)* GetComponentInParent<Canvas>().scaleFactor;
			
			if (selected.position.y > (box.position + boxHeight).y ) rectTransform.position = rectTransform.position - (Vector3.up * increment);
			
			else if (selected.position.y < (box.position ).y) rectTransform.position = rectTransform.position + (Vector3.up * increment);
			
			else 
			{
				group.dirty = false;
			}
			
			// 
			
			
		}
		
		public void ScrollbarToSelectedPosition() {
			
			RectTransform box = rectTransform.parent as RectTransform;
			
			
			
			// float boxHeight = box.rect.height * GetComponentInParent<Canvas>().scaleFactor;
			
			// assuming the pivot is bottom 
			
			float diff = rectTransform.position.y - box.position.y; // bottom left of screen
			
			float hundred = GetListHeight();
			
			if (hundred <= 0f ) 
			{
				if (sb.gameObject.activeSelf) sb.gameObject.SetActive(false);
				
				return;
			}
			
			if (!sb.gameObject.activeSelf) sb.gameObject.SetActive(true);
			
			float frac = diff / hundred;
			
			sb.value = frac;
			
		}
		
		
		public bool FillsPanel() {
			float height = listElement.rect.height;
			
			int fits = (int)(rectTransform.rect.height / height);
			
			int activeChildren = 0;
			
			foreach (Transform child in rectTransform)
			{
				if (child.gameObject.activeSelf) activeChildren ++;
			}
			
			return (activeChildren > fits) ;
		}
		public float GetListHeight() {
			
			// int maxIndex = rectTransform.childCount;
			
			
			int activeChildren = 0;
			
			foreach (Transform child in rectTransform)
			{
				if (child.gameObject.activeSelf) activeChildren ++;
			}
			
			
			float height = listElement.rect.height;
			
			float fits = (float)rectTransform.rect.height / height;
			
			
			float fittedIndex = activeChildren - fits;
			
			
			float totalHeight = fittedIndex * height * GetComponentInParent<Canvas>().scaleFactor;
			
			return totalHeight;
		}
		
		
		public void SetScrollPercent(float frac) {
			
			
			float totalHeight = GetListHeight();
			
			
			float lowLimit = rectTransform.parent.transform.position.y;
			
			float upLimit = totalHeight * frac + lowLimit;
			
			Vector3 newPos = rectTransform.position;
			newPos.y = upLimit;
			
			
			rectTransform.position = newPos;
		}
		
		public void SetScrollPercent(Scrollbar sb) {
			float frac = sb.value;
			SetScrollPercent(frac);
			
			
		}
	}
		
}