using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim.Setup
{
	public class DatesimSetupAmbienceTabButton : DatesimSetupAmbienceConnect {
		// This is a list of locations, I should be able to auto-generate them
		
		
		
		public Color selectedColor = Color.red;
		public Color defaultColor = Color.clear;
		
		public Image image;
		public int relationRequired = 0;
		// maybe more?
		protected int scene = -1;
		
		protected override void OnEnable()
		{
			base.OnEnable();
			// if there were animators I'd turn them on/off?
		
			
			if (scene < 0) scene = GetComponent<Transform>().GetSiblingIndex();
			if (image == null) 
			{
				image = transform.GetComponent<Image>();
				//defaultColor = image.color;
			}
			
		}
		
		
		public bool used = false;
		protected override void OnUpdate()
		{
			base.OnUpdate();
			// name, index
			
			if (!used)
			{
				if (SelectGlobal.selected == gameObject)
					Use();
			}
			else if (used && SelectGlobal.selected != gameObject)
				used = false;
		}
		
		
		protected override void OnChange()
		{
			// if "" == "" don't change it
			//if ((int)hub.vars.relation == access) return;
			
			used= ambienceHub.scenes.Contains(scene);
			
			if (used && relationRequired < hub.access)
			{
				if (used)image.color = selectedColor;
				else
				image.color = Color.clear;
			}
			else
			{
				used = false;
				image.color = Color.clear;
			}
			
		}
		
		protected override void OnPress()
		{
			
			Use();
		}
		
		protected void Use()
		{
			
			if (relationRequired > hub.access) return;
			
			bool deselect = ambienceHub.scenes.Contains(scene);
			
				
			if (!SelectGlobal.ctrl && !SelectGlobal.shift) 
			{
				ambienceHub.scenes.Clear();
			}
			
			if (deselect)
				ambienceHub.scenes.Remove(scene);
			else
				ambienceHub.scenes.Add(scene);
					
					
			ambienceHub.OnChange();
		}
		
		
		
		
		

	}
}