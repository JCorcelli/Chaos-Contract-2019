using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim.Setup
{
	public class DatesimSetupAmbienceDragZone : DatesimSetupDateConnect {
		
		public RectTransform rectTransform;
		//protected RectTransform scaledTransform;
		
		public MagnetThatGrowsAndToggle eraser;
		
		protected DatesimSetupAmbienceHub dragHub;
		
		public RectTransform dragged;
		
		public bool isDragging = false;
		public bool isGhost = false;
		public bool erasing = false;
		
		protected Vector3 startPosition = new Vector3();
		
		protected override void OnEnable( ){
			base.OnEnable();
			
			buttonName = "mouse 1";
			
			if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
			
			if (dragHub == null)
			dragHub = GetComponentInParent<DatesimSetupAmbienceHub>();
			if (dragHub == null)
				Debug.Log("no hub", gameObject);
			dragHub.onChange -= OnChange;
			dragHub.onChange += OnChange;
			
			
			if (eraser == null) eraser = rectTransform.parent.Find("Eraser").GetComponent<MagnetThatGrowsAndToggle>();
		}
		
		protected override void OnDisable( ){
			base.OnDisable( );
			Drop();
			
		}
		protected override void OnChange(){
			
			if (hub.clearAmbience) Clear();
			else if (!isHovered && dragHub.draggedAmbience != null)
				GhostDrag();
			
			else if (hub.undoAmbience) UndoAll();
		}
		
		protected void UndoAll(){
			dragHub.UndoAll();
			
			hub.undoAmbience = false;
			hub.Preview();
			
		}
		protected void Clear(){
			dragHub.ClearAll();
			hub.clearAmbience = false;
			hub.Preview();
		}
		public void OnDestroy() {
			if (dragged != null) Destroy(dragged.gameObject);
		}
		protected void Drop(){
			if (isGhost && dragged != null) GameObject.Destroy(dragged.gameObject);
			
			isDragging = false;
			dragged = null; // redundant?
		}
		protected void CancelDrag(){
			if (dragged != null) GameObject.Destroy(dragged.gameObject);
			
			isDragging = false;
			dragged = null; // redundant?
		}
		
		protected void GhostDrag(){
			
			if (erasing) 
			{
				dragHub.draggedAmbience = null;
				return;
			}
			isGhost = true;
			if (dragHub.selectedAmbience == null) return;
			
			BeginDrag();
		}
		
		protected void InstantDrag(){
			
			
			if (dragHub.selectedAmbience == null) return;
			
			BeginDrag();
			MakeReal();
		}
		
		protected void MakeReal(){
			if (dragHub.totalAmbience >= dragHub.maxAmbience) 
			{
				CancelDrag();
				return;
			}
			
			isGhost = false;
			dragged.SetParent(rectTransform, false);
			dragged.gameObject.GetComponent<DatesimSetupAmbienceObject>().MakeReal();
			
			
			
			Drop();
		}
		protected void BeginDrag(){
			// make object
			
			isDragging=true;
			dragged = Instantiate(dragHub.selectedAmbience) as RectTransform;
			
			dragged.SetParent(rectTransform.parent, false);
			dragged.SetSiblingIndex(rectTransform.GetSiblingIndex()-1);
			dragHub.draggedAmbience = null;
			
			startPosition = dragged.position = Input.mousePosition;
		}
		protected void Drag(){
			// a per frame update of sprite position
			if (!isDragging) return;
			
			if (!Input.GetButton("mouse 1")) 
				Drop();
			else
				dragged.position = Input.mousePosition;
		}
		
		
		protected override void OnUpdate(){
			base.OnUpdate();
			
			if (Pressing("mouse 2"))
				eraser.Toggle();
			
			if (Pressing("mouse 1"))
			{
				if (erasing) eraser.Cancel();
				InstantDrag();
			}
			
			if (erasing)
			{
				if (Pressing("mouse 1") || Pressing("mouse 2"))
				{ eraser.Cancel();
				}
			}
			erasing = eraser.col.enabled;
			
			if (dragHub.newList)
			{
				// this is a silent "replace"
				hub.ambienceList = dragHub.previewList;
			
			}
			if (erasing || dragHub.totalAmbience >= dragHub.maxAmbience) 
			{
				CancelDrag();
				return;
			}
			
			
			Drag();
			
			
		}
		
		protected override void OnEnter(){
			
			if (isDragging && isGhost) 
			{
				
				MakeReal();
			}
		}
		protected override void OnPress(){
			// make selected object here
			
		}
		
		
	}
}