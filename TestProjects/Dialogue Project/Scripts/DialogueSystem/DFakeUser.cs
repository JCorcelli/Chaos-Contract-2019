
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

using Utility.GUI;
using SelectionSystem;



namespace DialogueSystem
{

	public class DFakeUser : DSuperUser {
		// force a real user to store this or proc
		
		protected override IEnumerator Start(){
			
			yield return base.Start();
			
			//SpawnUser();
			
			
			outStream.Proc(storage.storedText[0]);
			
			
		}
		
		
		protected override void OnLateUpdate(){}
		protected override void OnUpdate()
		{
			
			
			if (storage != null) storage.location = transform.position;
			if (outStream != null) outStream.Step();
			
			
		}
		
		
	}
	
}