using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim
{
	public class DatesimAppRelationText : DatesimAppConnectText {
		// This is intended for big applications that need to coordinate actions
		
		
		
		public float displayRelation = 0f;
		
		protected override void OnChange() {
			base.OnChange();
			
			displayRelation = vars.relation;
			text.text = "Relation : Level " + displayRelation.ToString();
		}
		
		
		

	}
}