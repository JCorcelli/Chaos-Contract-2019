using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;


namespace SelectionSystem
{
	public delegate void AbstractAnyDelegate();
	public delegate void SelectionDelegate();
	
	
	
	
	public class SelectionManager : MonoBehaviour {
		
		
		
		
		
		public static SelectionDelegate onPress;

		
		public static SelectionDelegate onRelease;
		
		
		public static SelectionDelegate onMask;
		public static SelectionManager instance;
		
		
		protected void Awake () {
			if (instance == null) instance = this;
			else 
			{
				GameObject.Destroy(this);
				return;
			}
			
			SelectGlobal.buttons = Resources.Load("AbstractAnyData") as ButtonNames;
				
		}
		
		
		public static bool anyPressed = false;
		

		public static List<GameObject> focusList = new List<GameObject>();
		public static List<GameObject> focusCanvas = new List<GameObject>();
		public static List<GameObject> usedList;
		
		public static Canvas oc;
		public static GameObject og;
		
		
		public static void HandleModifiers()
		{
			SelectGlobal.ctrl = Input.GetButton("ctrl");
			SelectGlobal.shift = Input.GetButton("shift");
			SelectGlobal.alt = Input.GetButton("alt");
		}
		
		public static void HandleButtons()
		{
			
			foreach (string s in SelectGlobal.buttons.buttonNames)
			{
				if (Input.GetButtonDown(s))
				{
					SelectGlobal.button  = s;
					
					break;
				}
			}
			
		}
		
		
		
		
		public static void Select(GameObject ob){
			if (ob == null) return;
			SelectGlobal.uiSelect = true;
			SelectGlobal.prevSelected = SelectGlobal.selected;
			
			
			SelectGlobal.prevSelectedTransform =SelectGlobal.selectedTransform;
			
			SelectGlobal.selected = ob;
			
			
			
			if (SelectGlobal.selected == null)
				SelectGlobal.selectedTransform = null;
			else
				SelectGlobal.selectedTransform = ob.transform;
			
		}
		public static void Deselect(GameObject ob){
			if (ob == null) return;
			if (SelectGlobal.selected != ob) return;
			Deselect();
		}
			
		public static void Deselect(){
			SelectGlobal.prevSelected = SelectGlobal.selected;
				
			SelectGlobal.uiSelect = false;
			SelectGlobal.prevSelected = SelectGlobal.selected;
			SelectGlobal.selected = null;
			
				
		}
		
		
		protected void Update () {
			// Unity gets an auto interface, but it might be annoying.
			Select( EventSystem.current.currentSelectedGameObject);
			
			
				
			// anything is pressed
			HandleModifiers();
				
			
			
			if (SelectGlobal.locked) return;
			if (Input.anyKeyDown) {
				
				HandleButtons();
				anyPressed = true;
				
				if (onMask != null) onMask(); // checks if I'm over a mask
				
				if (onPress != null)
					onPress();
				
				
			}
			
			
			// nothing is pressed
			else if (anyPressed && !Input.anyKey && onRelease != null) {
				anyPressed = false;
				onRelease();
			}
			
				
			SelectGlobal.uiSelect = false;
		}
			
	}
}