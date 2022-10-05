
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
	public partial class DisplayNodeParent : UpdateBehaviour {
		// This is a widget. It plays the running text so this bit of code isn't part of the parent.
		public DisplayNode node;
		public DText procText{get{return node.procText;}set{node.procText = value;}}
		public DText goalText{
			get{return node.goalText;}
			set{
				node.goalText = value;
			}
		}
		
		
	}
}