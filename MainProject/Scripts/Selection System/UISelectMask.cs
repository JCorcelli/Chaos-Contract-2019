using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event 



namespace SelectionSystem
{


	public class UISelectMask : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler {

	
		// this frame
		// suppress my selectionmanager
		// prevents propagation of pointerdown event
		// suppress buttonhandlers from registering pointer down event
		
		protected virtual void OnEnable() {
			
			SelectionManager.onMask += HoverCheck;
		}
		
		protected virtual void OnDisable() {
			isHovered = false;
			SelectionManager.onMask -= HoverCheck;
		}
		
		protected void HoverCheck(){
			if (isHovered) SelectGlobal.uiSelect = true;
		}
		
		public void  OnPointerEnter( PointerEventData eventData ) {
				
			_OnEnter();
				
		}
		
		public bool isHovered = false;
		public void  Enter() {
			_OnEnter();
		}
		protected virtual void  _OnEnter() {
			isHovered = true;
			
			
		}
		
		
		public void  OnPointerExit( PointerEventData eventData ) {
			// this is called by disabling it anyway
				
			_OnExit();
		}
		public void Exit() {_OnExit();}
		public void _OnExit() {
			isHovered = false;
			
		}
			
		
	}
}
