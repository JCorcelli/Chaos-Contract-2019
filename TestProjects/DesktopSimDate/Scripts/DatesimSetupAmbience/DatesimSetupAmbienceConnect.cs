using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim.Setup
{
	public class DatesimSetupAmbienceConnect : DatesimSetupDateConnect {
		// This is a list of locations, I should be able to auto-generate them
		
		// has desktophook
		public DatesimSetupAmbienceHub ambienceHub;
		public RectTransform rectTransform;
		
		protected override void OnEnable()
		{
			base.OnEnable();
			if (rectTransform == null)
			rectTransform = GetComponent<RectTransform>();
		
			if (ambienceHub == null)
			ambienceHub = GetComponentInParent<DatesimSetupAmbienceHub>();
			if (ambienceHub == null)
				Debug.Log("no hub", gameObject);
		
		
			ambienceHub.onChange -= OnChange;
			ambienceHub.onChange += OnChange;
		}
		public virtual void OnDestroy()
		{
			if (ambienceHub == null) return;
			
			ambienceHub.onChange -= OnChange;
			
		}
		
		protected override void OnChange()
		{
			
		}
		
		
		
		
		

	}
}