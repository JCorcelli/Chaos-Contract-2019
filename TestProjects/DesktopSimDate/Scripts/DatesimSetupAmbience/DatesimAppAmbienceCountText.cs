using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim.Setup
{
	public class DatesimAppAmbienceCountText : DatesimAppConnectText {
		
		public RectTransform rectTransform;
		//protected RectTransform scaledTransform;
		
		public RectTransform dragged;
		
		public List<DatesimSetupAmbienceObject> ambienceList = new List<DatesimSetupAmbienceObject>();
		public List<DatesimSetupAmbienceObject> backup = new List<DatesimSetupAmbienceObject>();
		
		
		protected override void OnEnable( ){
			base.OnEnable();
			if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
			//vars.onChange -= OnChange;
			//vars.onChange += OnChange;
			
		}
		
		
		protected override void OnChange(){
			
			text.text = vars.ambienceTotal + " / 50";
		}
		
		
		
	}
}