
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;
using System;
using System.Text;


namespace DialogueSystem
{
	public class MemoryBoxNode : DisplayNode {
		// This is a widget. It plays the running text so this bit of code isn't part of the parent.
		
		public MemoryBoxNode touchedNode;
		
		// this is cut before the end of a larger thread.
		public override void Break(){
			// this box is now a separate section
			gathering = false;
			done = true; // this lets "newline" function, else the entire content ends up in the first and last box
		}
		
		public override void AddAllActions() {
			// override default actions
		}
		
		
		
	}
}