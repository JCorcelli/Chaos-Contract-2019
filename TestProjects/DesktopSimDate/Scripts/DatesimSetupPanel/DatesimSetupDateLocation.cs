using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim.Setup
{
	public class DatesimSetupDateLocation : DatesimSetupDateConnect {
		// This is a list of locations, I should be able to auto-generate them
		
		[System.Serializable]
		public class RestrictedSprite
		{
			public int relationRequired = 1;
			public Sprite sprite;
		}
		public RestrictedSprite[] bgList;
		
		public RectTransform optionPrefab;
		public RectTransform rectTransform;
		public RectTransform selectedRect;
		public string selected = "";
		
		public bool focused = false; // now I can scroll the selection
		
		protected override void OnEnable()
		{
			base.OnEnable();
			// if there were animators I'd turn them on/off?
			rectTransform = GetComponent<RectTransform>();
			optionPrefab.gameObject.SetActive(false);
			
			GenerateChildren();
			SetOptions();
			
		}
		public void GetInput(RectTransform t)
		{
			// name, index
			selectedRect = t;
			selected = t.name.ToLower();
			
			MoveChildren(selectedRect);
		}
		protected override void OnUpdate()
		{
			base.OnUpdate();
			// name, index
			
			int direction = 0;
			
			if (Input.GetButton("left"))
				direction = -1;
			else if (Input.GetButton("right"))
				direction = 1;
			
			if (direction != 0) 
				MoveChildren(direction);
			
		}
		
		public void MoveChildren(int direction){
			// increment/decrement
			// maybe have it scroll to center first, then select it
			if (selectedRect == null) return;
			int i= selectedRect.GetSiblingIndex();
			
			selectedRect = rectTransform.GetChild(i + direction) as RectTransform;
			
		}
		public void MoveChildren(RectTransform goal){
			// should move towards the select object
		}
		public int access = 1;
		
		
		protected void GenerateChildren(){
			if (rectTransform.childCount > 0) return;
			
			// here I could check for the level
			
			
			RawImage img;
			DatesimSetupDateLocationButton setThis;
			
			Sprite s;
			RectTransform r;
			foreach( RestrictedSprite res in bgList)
			{
				s = res.sprite;
				r = Instantiate(optionPrefab) as RectTransform;
				
				r.name = s.name.Replace("_"," ");
				img = r.GetComponent<RawImage>();
				img.texture = s.texture;
				
				setThis = r.GetComponent<DatesimSetupDateLocationButton>();
				setThis.relationRequired = res.relationRequired;
				
				r.SetParent (rectTransform, false);
				
				r.gameObject.SetActive(true);
			}
		}
		protected void SetOptions(){
			
			List<string> locationOptions = new List<string> ();
			
			Sprite s;
			foreach( RestrictedSprite res in bgList)
			{
				if (res.relationRequired <= access )
				{
					s = res.sprite;
					
					locationOptions.Add(s.name.Replace("_"," "));
				}
			}
			hub.locationOptions = locationOptions.ToArray();
		}
		protected override void OnChange()
		{
			// if "" == "" don't change it
			if ((int)hub.access == access) return;
			access = hub.access;
			SetOptions();
			
		}
		
		
		
		
		
		
		

	}
}