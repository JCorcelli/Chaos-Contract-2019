using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim.Setup
{
	public class DatesimSetupAmbienceContentSelector : DatesimSetupDateConnect {
		// This is a list of locations, I should be able to auto-generate them
		
		public DatesimSetupAmbienceHub dragHub;
		
		
		public RectTransform rectTransform;
		public Image image;
		public Color defaultColor = Color.clear;
		
		public Color selectedColor = Color.red;
		public int relationRequired = 1;
		// maybe more?
		
		protected override void OnEnable()
		{
			base.OnEnable();
			buttonName = "mouse 1";
			// if there were animators I'd turn them on/off?
			if (dragHub == null)
			dragHub = GetComponentInParent<DatesimSetupAmbienceHub>();
			if (dragHub == null)
				Debug.Log("no hub", gameObject);
		
			
			if (rectTransform == null)
			rectTransform = GetComponent<RectTransform>();
			if (image == null) image = GetComponent<Image>();
			image.color = defaultColor;
			dragHub.onChange -= OnChange;
			dragHub.onChange += OnChange;
		}
		
		protected bool selected = false;
		protected override void OnChange()
		{
			// if "" == "" don't change it
			//if ((int)hub.vars.relation == access) return;
			
			
			if (selected && relationRequired > hub.access)
			{
				dragHub.selected = null;
				dragHub.selectedAmbience = null;
				selected = false;
			}
			
			else if (selected  ) 
			{
				if (dragHub.selected != gameObject ) 
				{
					selected = false;
					image.color = defaultColor;
				}
			
			}
			else
			image.color = defaultColor;
		}
		
		public void SetDragged()
		{
			
			dragHub.draggedAmbience = dragHub.selectedAmbience = rectTransform.GetChild(0) as RectTransform;
			
		}
		protected override void OnUpdate(){
			base.OnUpdate();
			
			if (!selected && SelectGlobal.selected == gameObject)
			{
				dragHub.selected = gameObject;
				SetSelected();
			}
		}
		
		public void SetSelected()
		{
			selected = true;
			image.color = selectedColor;
			dragHub.selectedAmbience = rectTransform.GetChild(0) as RectTransform;

			dragHub.OnChange();

		}
		protected override void OnPress()
		{
			selected = true;
			image.color = selectedColor;
			dragHub.selected = gameObject;
			SetDragged();

			dragHub.OnChange();

		}
		
		
		
		
		
		

	}
}