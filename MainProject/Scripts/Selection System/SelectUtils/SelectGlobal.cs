using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace SelectionSystem  {
	
	
	
	public class SelectGlobal {
		// static / semaphore variables
		public static bool locked = false; //  consider overused, NPC semaphore
		
		public static float dragDistance = 5f;
		public static bool uiSelect = false; // masks selectable, UI semaphore
		
		public static GameObject selected;
		public static Transform selectedTransform;
		public static GameObject prevSelected;
		public static Transform prevSelectedTransform;
		
		public static float rotateOffTimer = 2f; // I think something like shift + right click could set this to -1, right click can set it to the default (2).
		public static float playerInputPatience = 1.5f;
		
		
		public static ButtonNames buttons ;
		
		
		public static string button = "";
		public static bool used = false;
		public static bool ctrl = 	false;
		public static bool shift = 	false;
		public static bool alt = 	false;
		
		public static List<GameObject> focusedList = new List<GameObject>();
		
		public static GameObject focused;
		
		public static bool GetCombo(bool c, bool s, bool a)
		{
			// must match 1:1 in this version
			return (c == ctrl && s == shift && a == alt);
			
		}
		
		public static bool GetComboInclusive(bool c, bool s, bool a)
		{
			// if any buttons are pressed it matches
			return (!c || (c && ctrl) &&
			!s || (s && shift) &&
			!a || (a && alt));
			
		}
		
	}
	
}