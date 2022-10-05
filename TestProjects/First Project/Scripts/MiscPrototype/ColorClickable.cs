using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace TestProject
{

	public class ColorClickable : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IDeselectHandler, IPointerDownHandler {
		
		public static bool selected = false;
		private ColorTarget[] targets;
		
		void Start(){
			//Selectable sel = gameObject.GetComponent<Selectable>();
			//Navigation customNav = Navigation.defaultNavigation;
			//customNav.mode = Navigation.Mode.Explicit;			
			//sel.navigation = customNav;
			targets = GameObject.FindObjectsOfType(typeof(ColorTarget)) as ColorTarget[];
		}
		public void OnPointerEnter (PointerEventData eventData) 
		{
			if (selected) return;
			foreach (ColorTarget target in targets) {
			target.mat.color = this.GetComponent<Renderer>().material.color ;
			}
		}
		public void OnSelect (BaseEventData eventData) 
		{
			selected = true;
			foreach (ColorTarget target in targets) {
			target.mat.color = this.GetComponent<Renderer>().material.color ;
			}
		}
		public void OnDeselect (BaseEventData eventData) 
		{
			selected = false;
		}
		public void OnPointerDown (PointerEventData eventData) 
		{
			eventData.selectedObject = this.gameObject;
			eventData.Use();
		}
	
	}
}