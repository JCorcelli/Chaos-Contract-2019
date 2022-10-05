
using UnityEngine;

using UnityEngine.UI;

using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

using Utility.GUI;
using SelectionSystem;



namespace DialogueSystem
{


	
	public class DMediaSimulation  {
		// This will generate a string of text representing the action of accessing storage
		// And this will be a fast method of transporting text
		
		public DText procText ; // original stored text
		
		
		public StringBuilder s {get{return procText.storedText;} set{}} // mutated text
		public char c;
		
		public string mediaType = "Simulation"; // caused by flaws, environment, distortion. Word for word.
		
		
		
		public virtual bool Proc(List<DText> pt)
		{
			for (int i = 0 ; i < pt.Count ; i++)
			{
				for (int ix = 0 ; ix < DUser.all.Count ; ix++)
				{
					DUser.all[i].storage.Store( pt); // that's 100% reference
				}
			}
			
			// Sending information everywhere
			// estimate time to completion
			/* 
				
				2.5% receive references to the source
				17.5% receive references
			
				30% receive a clone
				---
				30% receive a mutated clone
				
				17.5% receive a strong-mutated clone
				
				2.5% receive a clone with altered parameters
				
			*/
			return true;
		}
		
	}
}