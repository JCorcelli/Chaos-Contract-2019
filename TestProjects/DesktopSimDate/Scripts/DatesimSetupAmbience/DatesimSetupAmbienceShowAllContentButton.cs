using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim.Setup
{
	public class DatesimSetupAmbienceShowAllContentButton : DatesimSetupDateConnect {
		// This is a list of locations, I should be able to auto-generate them
		
		public DatesimSetupAmbienceHub radioHub;
		
		public Sprite[] marks;
		public RectTransform rectTransform;
		protected Image image;
		
		protected override void OnEnable()
		{
			base.OnEnable();
			// if there were animators I'd turn them on/off?
			if (radioHub == null)
			radioHub = GetComponentInParent<DatesimSetupAmbienceHub>();
			if (radioHub == null)
				Debug.Log("no hub", gameObject);
		
			
			if (rectTransform == null)
			rectTransform = GetComponent<RectTransform>();
			if (image == null) image = GetComponent<Image>();
			radioHub.onChange -= OnChange;
			radioHub.onChange += OnChange;
		
			if (image == null) image = GetComponent<Image>();
			
			showAll = radioHub.showAll;
			
			if (!showAll) image.color = Color.clear;
			
			
		}
		
		protected override void OnUpdate()
		{
			base.OnUpdate();
			// name, index
			
			
		}
		
		protected override void OnEnter(){
			
			if (showAll)image.color = Color.red;
		}
		protected override void OnExit(){
			
			if (showAll)image.color = Color.black;
		}
		
		protected bool showAll = false;
		protected override void OnChange()
		{
			// if "" == "" don't change it
			//if ((int)radioHub.vars.relation == access) return;
			if (showAll == radioHub.showAll) return;
			
			showAll = radioHub.showAll;
			if (showAll)
			{
				if (marks.Length > 0)
				{
					int used = (int)Random.Range(0, marks.Length -1 );
					image.sprite = marks[used];
				}
				image.color = Color.black;
			}
			else
			{
				image.color = Color.clear;
				
			}
			
		}
		
		protected override void OnClick()
		{
			
			
			radioHub.showAll = !radioHub.showAll;
			
			radioHub.OnChange();
		}
		
		
		
		
		

	}
}