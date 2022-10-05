using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim.Setup
{
	public class DatesimSetupAmbienceObjectCommon : DatesimSetupAmbienceObject {
		// This is a list of locations, I should be able to auto-generate them
		
		
		
		public override void MakeProxy(){
			isProxy = true;
			collider.enabled = false;
			GameObject.Destroy(gameObject.GetComponent<ErasedByEraser>());
			GameObject.Destroy(gameObject.GetComponent<DragHold2DAmbience>());
			
			
		}
		public override void MakeReal(){
			gameObject.AddComponent<ErasedByEraser>();
			DragHold2DAmbience dr = gameObject.AddComponent<DragHold2DAmbience>();
			
			base.MakeReal();
			if (!isAlive) return;
			
			collider.enabled = true;
			dr.screenBound = false;
			dr.interiorBound = true;
			dr.pressed = true;
			dr.OnInstance();
			
			ar.Fix();
			
		}
		

		
		

	}
}