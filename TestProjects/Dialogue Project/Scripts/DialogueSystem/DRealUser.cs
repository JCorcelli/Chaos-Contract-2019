
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

using Utility.GUI;
using SelectionSystem;



namespace DialogueSystem
{

	public class DRealUser : DSuperUser {
		public bool isPlayer = true;
		
		protected override void SpawnUser()
		
		{
			base.SpawnUser();
			
			if (isPlayer) DUser.selected = myUser;
			
			inStream.Load();
			
			
			
		}
		
		
		protected override void OnLateUpdate()
		{
			
			if (inStream != null) 
			{
				
				inStream.Next();
				
				storage.Store(inStream.streamList);
			}
			
		}
		protected override void OnUpdate()
		{
			
			// carrying? storage.location = transform.position;
			myUser.location  = transform.position;
			
			// there's a 'used' storage, and (pending) personal storage
			
			
			if (storage != null) 
			{
				storage.location = transform.position;
				
				
				// inStream.streamedText.EmptyStream(); // streamedText is garbage
				
				
				if (display != null && storage.storedText.Count > 0) {
					
					display.goalText = storage.storedText[0];
					
				}
			}
			
			if (outStream != null) outStream.Step();
			
		}
		
	}
	
}