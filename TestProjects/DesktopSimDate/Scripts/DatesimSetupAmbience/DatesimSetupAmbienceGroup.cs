using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim.Setup
{
	public class DatesimSetupAmbienceGroup : DatesimSetupAmbienceConnect {
		// This is a list of locations, I should be able to auto-generate them
		
		public Image image;
		public Color defaultColor = Color.white;
		
		public Color selectedColor = Color.red;
		public int relationRequired = 0;
		
		protected override void OnEnable()
		{
			base.OnEnable();
			if (image == null)
			{
				image = GetComponent<Image>();
				defaultColor = image.color;
			}
		}
		
		
			
			
		public void RandomPick(){
			
			int sel = (int)Random.Range(0, transform.childCount - .1f);
			
			Transform t = transform.GetChild(sel);
			DatesimSetupAmbienceContentSelector con = t.GetComponent<DatesimSetupAmbienceContentSelector>();
			
			if (con == null) return;
			con.SetSelected();
		}
		public void SetDragged()
		{
			
			RandomPick();
			if (ambienceHub.selectedAmbience  == null) return;
			ambienceHub.draggedAmbience =
			ambienceHub.selectedAmbience;
		}
		
		public bool selected = false;

		protected override void OnUpdate(){
			base.OnUpdate();
			if (selected  ) 
			{
				if (ambienceHub.selected != gameObject ) 
				{
					selected = false;
					image.color = defaultColor;
				}
			
				else if (Input.GetButtonUp("mouse 1"))
					RandomPick();
				
					
			}
			else if (SelectGlobal.selected == gameObject)
			{
				image.color = selectedColor;
				selected = true;
				ambienceHub.selected = gameObject;
				RandomPick();
			}
		}
		
		protected override void OnChange()
		{
			
			if (selected && relationRequired > hub.access)
			{
				ambienceHub.selected = null;
				selected = false;
			}
		}
		protected override void OnPress()
		{
			selected = true;
			image.color = selectedColor;
			ambienceHub.selected = gameObject;
			SetDragged();
			ambienceHub.OnChange();
			
		}
		
		
		
		

	}
}