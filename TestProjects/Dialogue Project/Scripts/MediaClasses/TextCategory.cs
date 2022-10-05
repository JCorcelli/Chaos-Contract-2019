
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


	[CreateAssetMenu(fileName = "TextCategory Name", menuName = "ScriptableObjects/TextCategory", order = 1)]
	public class TextCategory : ScriptableObject {

		public TextAsset[] t = new TextAsset[]{};
		
	}
}