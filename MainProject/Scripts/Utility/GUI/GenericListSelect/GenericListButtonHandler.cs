using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;// Required when using Event data.

using SelectionSystem;
using SelectionSystem.IHSCx;
using Utility.Managers;

using System.Collections;

namespace Utility.GUI
{
	[RequireComponent (typeof(HSCxController))]
	public class GenericListButtonHandler : IHSCxConnect
	{
		public Transform cursor;
		public Transform highlight;
		
		public string buttonName = "mouse 1";
		public GroupSelection group;
		
		protected override void OnEnable() {
			base.OnEnable();
			if (group == null) group = GetComponentInParent<GroupSelection>();
			if (group == null) Debug.Log("Looked for group, found nothing", gameObject);
			ih.onEnter += OnEnter;
			ih.onExit += OnExit;
			ih.onPress += OnPress;
			
		}
		protected override void OnDisable() {
			base.OnDisable();
			ih.onEnter -= OnEnter;
			ih.onExit -= OnExit;
			ih.onPress  -= OnPress;
			
		}
		
		protected virtual void OnEnter(HSCxController caller){
			
			highlight.gameObject.SetActive(true);
			highlight.position = this.transform.position;
		}
		protected virtual void OnExit(HSCxController caller){
			highlight.gameObject.SetActive(false);
		}
		protected virtual void OnPress(HSCxController caller)
		{
			if (!cursor.gameObject.activeSelf) cursor.gameObject.SetActive(true);
			cursor.position = this.transform.position;
			group.selected = this.gameObject;
			
		}
		
		protected void OnDestroy() {
			if (group == null) return;
			if (group.selected == this.gameObject) cursor.gameObject.SetActive(false);
		}
		
		protected override void OnLateUpdate() {
			if (EventSystem.current.currentSelectedGameObject == this.gameObject) group.selected = this.gameObject;
			if (group.selected == this.gameObject) 
				cursor.position = this.transform.position;
			
			if (ih.isHovered) 
				highlight.position = this.transform.position;

		}
		
	}
		
}