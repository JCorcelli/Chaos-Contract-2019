using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

namespace SelectionSystem
{
	public class HoverxSelectToggle :  MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler {

		public string buttonName = "mouse 1";
		
		public GameObject onHover;
		public GameObject onClick;
		
		public bool isActive = false;
		public bool isHovered = false;
		public bool pressed = false;
		
		protected void  Start () { 
			onHover.SetActive( false);
			onClick.SetActive( isActive); 
			
			OnStart();
		}
		protected virtual void OnStart(){}
		
		public void  OnPointerEnter( PointerEventData eventData ) {
			isHovered = true;
			if (isActive)
				return;
			onHover.SetActive(true);
			
			OnEnter();
				
		}
		public virtual void  OnEnter() {}
		
		public void  OnPointerExit( PointerEventData eventData ) {
			isHovered = false;
			onHover.SetActive(false);
				
			OnExit();
		}
		
		public virtual void  OnExit() {}
		
		public void  OnPointerDown( PointerEventData eventData ) {
			if (Input.GetButton( buttonName)) 
			{
				pressed = true;
				SelectGlobal.uiSelect = true;
			}
			
		}
		
		protected RaycastHit hit;
		protected Ray ray;
		public void  OnPointerUp( PointerEventData eventData ) {
			if (!(pressed && !Input.GetButton(buttonName))) return;
			pressed = false;
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, Camera.main.eventMask))
			{
				if (isHovered)
					OnClick();
			}
				
		}
		
		protected void  OnClick( ) {
			
			isActive = !isActive;
			onClick.SetActive( isActive);
			
			if (isActive)
				onHover.SetActive(false);
			else
				onHover.SetActive(true);
			
		}
		
	}
}