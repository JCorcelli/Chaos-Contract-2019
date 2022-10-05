
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

using Utility.GUI;
using SelectionSystem;



namespace DialogueSystem
{
	public class DActionRegister : MonoBehaviour{
		// the concept of an action can exist before the thing that does the action
		// either I create it from the list here, or it exists
		
		// I can't put flavor text in braces, also actions can't use adjectives yet, idk
		
		
		public string names = "";
		protected void OnEnable()
		{
			DAction.Use(); // short names.. ugh
			DAction.AddKey("#"); // short names.. ugh
			DAction.AddKey("cps"); // short names.. ugh
			DAction.AddKey("source.start"); // short names.. ugh
			DAction.AddKey("source.end"); // short names.. ugh
			
			string[] split = names.Split(new Char[]{' '}, StringSplitOptions.RemoveEmptyEntries );
			
			foreach (string s in split)
			{
				DAction.AddKey(s); // names
			}
		}

	}
}