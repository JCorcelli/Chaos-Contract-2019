using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

namespace SelectionSystem
{
	public abstract class HoverxSelectAbstract :  UpdateBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {

		public string buttonName = "mouse 1";
		
		public bool isActive = false;
		public bool isHovered = false;
		public bool pressed = false;
		
		
		
		protected virtual void Awake() {
			
			
		}
		
		
		
		public void  OnPointerEnter( PointerEventData eventData ) {
			// this isn't registering when two items overlap
			_OnEnter();
				
		}
		public void  Enter() {
			_OnEnter();
		}
		public virtual void  _OnEnter() {
			isHovered = true;
			
			OnEnter();
		}
		public virtual void  OnEnter() {}
		
		public void  OnPointerExit( PointerEventData eventData ) {
			// this is called by disabling it anyway
				
			_OnExit();
		}
		public void Exit() {_OnExit();}
		public void _OnExit() {
			isHovered = false;
			OnExit();
			}
		public virtual void  OnExit() {}
		
		public void  OnPointerDown( PointerEventData eventData ) {
			Press();
		}
		public void Press() {_OnPress();}
		
		public virtual void  OnPress() {}
		
		
		
		protected override void  _OnPress() {
			if (!isHovered) return;
			if (Input.GetButton( buttonName)) 
			{
				Raycast();
			
				pressed = true;
				OnPress();
			}
		}
		
		
		protected RaycastHit _hit;
		public RaycastHit hit {get {return _hit;} set{_hit = value;}}
		protected Ray _ray;
		
		public Ray ray {get {return _ray;} set{_ray = value;}}
		
		protected override void OnDisable(){
			base.OnDisable();
		
			pressed = false;
			_OnExit();
			
		}
		
		public void Raycast() {
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			// set hit / ray values
			
			Physics.Raycast(ray, out _hit, Mathf.Infinity, Camera.main.eventMask);
			
			
		}
		public void Release() {
			// called by external
			if (!(pressed)) return;
			Raycast();
			_OnRelease();
		}
		
		
		protected void  _OnClick( ) {
			
			isActive = !isActive;
			
			OnClick();
			if (isActive)
				OnSelect();
			else
				OnDeselect();
			
		}
		public virtual void  OnClick() {}
		public void  _OnRelease() {
			pressed = false;
				
			if (isHovered)
			{
				_OnClick();
			}
			OnRelease();
		}
		public virtual void  OnRelease() {}
		
		public virtual void  OnSelect() {}
		
		public virtual void  OnDeselect() {}
		
		
		
		protected override void OnUpdate(){
			base.OnUpdate();
			
			if (pressed) // dragging or about to drag, consider drag handler
				DoWhilePressed();
			
			else if (isActive) // clicked, but not pressed
				DoWhileActive();
				
			else if (isHovered) // hovering, low priority
				DoWhileHovered();
				
			if (pressed && !Input.GetButton(buttonName))
				Release(); // this might not check if it's pressed later.
			
		}
		
		public virtual void  DoWhilePressed() {}
		
		public virtual void  DoWhileActive() {}
		
		public virtual void  DoWhileHovered() {}
		
		
	}
}