
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


	[CreateAssetMenu(fileName = "LibrarySource Name", menuName = "ScriptableObjects/LibrarySource", order = 1)]
	public class DLibrarySource : ScriptableObject {

		public TextCategory[] categories = new TextCategory[]{};
		
	}
}