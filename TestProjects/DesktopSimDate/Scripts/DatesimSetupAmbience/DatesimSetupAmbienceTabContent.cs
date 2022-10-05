using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim.Setup
{
	public class DatesimSetupAmbienceTabContent : DatesimSetupDateConnect {
		// This is a list of locations, I should be able to auto-generate them
		
		public DatesimSetupAmbienceHub radioHub;
		
		
		public RectTransform rectTransform;
		public Image image;
		public int relationRequired = 1;
		// maybe more?
		protected int scene = -1;
		
		protected override void OnEnable()
		{
			base.OnEnable();
			// if there were animators I'd turn them on/off?
			
			if (radioHub == null)
			radioHub = GetComponentInParent<DatesimSetupAmbienceHub>();
			if (radioHub == null)
				Debug.Log("no radioHub", gameObject);
		
			
			if (rectTransform == null)
			rectTransform = GetComponent<RectTransform>();
			if (scene < 0) scene = GetComponent<Transform>().GetSiblingIndex();
			if (image == null) image = GetComponent<Image>();
			
			radioHub.onChange -= OnChange;
			radioHub.onChange += OnChange;
			OnChange();
			
			
		}
		
		protected override void OnChange(){
			// if "" == "" don't change it
			//if ((int)radioHub.vars.relation == access) return;
			
			if (gameObject.activeSelf)
			{
				
				if (relationRequired > hub.access)
					gameObject.SetActive(false);
				else if (!radioHub.scenes.Contains(scene)
				&& !radioHub.showAll)
					gameObject.SetActive(false);
			}
			else if ( radioHub.scenes.Contains(scene) 
			|| radioHub.showAll)
			{
				gameObject.SetActive(true);
				
			}
			
				
				
				
			
			
		}
		
		
		
		
		
		

	}
}