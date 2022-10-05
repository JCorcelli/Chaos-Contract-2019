using UnityEngine;

using System.Collections;


namespace Datesim
{
	public class DatesimAppEndStatus : DatesimAppConnect {
		// This is intended for big applications that need to coordinate actions
		
		protected GameObject child;
		protected override void OnEnable( ){
			base.OnEnable();
			child = transform.GetChild(0).gameObject;
			OnChange();
		}
		
		protected override void OnChange()
		{
			
			
			if ( vars.showEndStatus)
				child.SetActive(true);
			else
				child.SetActive(false);
		}

		
		protected override void OnPress()
		{
			//if (child.activeSelf)
			vars.showEndStatus = false;
			vars.OnChange();
		}

	}
}