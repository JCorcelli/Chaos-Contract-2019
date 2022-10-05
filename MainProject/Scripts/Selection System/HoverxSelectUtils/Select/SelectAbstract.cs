using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

namespace SelectionSystem
{
	public abstract class SelectAbstract :  
	AbstractButtonHandler,  IPointerEnterHandler,
	IPointerUpHandler,  IPointerDownHandler, IPointerExitHandler {
		// tracks which mouse button is pressed with a.b.c.p.
		// optionally check if combo is pressed for script purpose
		

		
		
		public bool isActive = false;
		public bool isHovered = false;
		public bool isHit = false;
		
		
		
		
		public virtual void OnPointerDown( PointerEventData eventData ) {
			//fixed because I forgot the Ionenter
			isHit = true;
				
		}
		public virtual void OnPointerUp( PointerEventData eventData ) {
			//fixed because I forgot the Ionenter
			isHit = false;
				
		}
		public virtual void OnPointerEnter( PointerEventData eventData ) {
			
			_OnEnter();
				
		}
		public void  Enter() {
			_OnEnter();
		}
		protected virtual void  _OnEnter() {
			isHovered = true;
			
			OnEnter();
		}
		protected virtual void  OnEnter() {}
		
		
		public virtual void  OnPointerExit( PointerEventData eventData ) {
			// this is called by disabling it anyway
				
			_OnExit();
		}
		public void Exit() {_OnExit();}
		public void _OnExit() {
			
			isHovered = false;

			
			OnExit();
		}
		protected virtual void  OnExit() {}
		
		
		public void Press() {OnPress();}
		
		protected override void  OnPress() {
			
			
		}
		
		
		
		protected Vector3 startPos = new Vector3();
		
		
		protected override void _OnPress() {
			
			
			if (!isHit) return;
			startPos = Input.mousePosition;
			
			base._OnPress();
			
		}
		
		
		protected override void OnDisable(){
			base.OnDisable();
		
			isHovered = false;
			isHit = false;
			isActive = false;
			pressed = false;
			combo_held = false;
			
			
		}
		public void Release() {
			// called by external
			if (!(pressed)) return;
			
			_OnRelease();
		}
		
		
		protected void  _OnClick( ) {
			
			isActive = !isActive;
			
			if (Vector3.Distance(startPos,Input.mousePosition) > SelectGlobal.dragDistance) return;
				
			OnClick();
			if (isActive)
				OnSelect();
			else
				OnDeselect();
			
		}
		protected virtual void  OnClick() {}
		
		public virtual void  Select() {
			isActive = true;
			OnSelect();
		}
		public virtual void  Deselect() {
			isActive = false;
			OnDeselect();
		}
		protected override void  _OnRelease() {
			base._OnRelease();
			pressed = false;
			
			if (isHovered)
			{
				_OnClick();
			}
			OnRelease();
		}
		protected override void  OnRelease() {
			
		}
		
		protected virtual void  OnSelect() {}
		
		protected virtual void  OnDeselect() {}
		
		
		
		protected override void OnUpdate(){
			base.OnUpdate();
			
				
			
		}
		
		
		
	}
}