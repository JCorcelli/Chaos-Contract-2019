using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;



namespace Datesim.Setup
{
	public class DatesimSetupAmbienceObjectTrail : DatesimSetupAmbienceObject {
		// This is a list of locations, I should be able to auto-generate them
		
		
		public Image image;
		public override void MakeProxy(){
			
			
			/*
			isProxy = true;
			collider.enabled = false;
			GameObject.Destroy(gameObject.GetComponent<ErasedByEraser>());
			*/
			
			
		}
		public override void MakeReal(){
			gameObject.AddComponent<ErasedByEraser>();
			image = GetComponent<Image>();
			
			base.MakeReal();
			if (!isAlive) return;
			
			collider.enabled = true;
			
			ar.Fix();
			
			// testing
			
		}
		

		
		

	}
}