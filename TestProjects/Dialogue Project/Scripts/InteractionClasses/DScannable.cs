
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

using Utility.GUI;
using SelectionSystem;
using Zone;


namespace DialogueSystem
{
	public class DScannable : SelectAbstract {
		// layer 1: superficial text, included with every scan
		// ie.  measured surface, beauty, prejudice, name or pronoun, and a description of what an object is, (and not what it does, or contains)
		// layer 2: inhereting class content
		
		
		// string replace ops?
		
		public delegate DText DScanDelegate();
		
		protected Dictionary<string, DScanDelegate> _superText = new Dictionary<string, DScanDelegate>() ;
		
		public virtual Dictionary<string, DScanDelegate> superText => _superText; // when someone's appearance is altered this should be altered
		
		public Dictionary<string, DText> GetDetails(){
			var li = new Dictionary<string, DText>();

			foreach(KeyValuePair<string, DScanDelegate> entry in superText)
			{
				if (entry.Value != null)
					li.Add(entry.Key, entry.Value());
			}
			
			
			return (li);
		}
		public void AddDetail(string s, DScanDelegate method)
		{
			superText.Add(s, method);
		}
		public DScannable(){
			AddDetail("Default", Default);
		}
		public static DText GetGeneric(string oname){
			if (oname.Length < 1) return null;
			
			DText dt = new DText();
			dt.DAppend($"This is a {oname}.");
			
			return dt;
		}
		private DText Default(){
			DText dt = new DText();
			dt.DAppend($"This is a {name}.");
			
			return dt;
		}
		
		
		protected override void OnEnable() { 
			base.OnEnable();
		}
		protected override void OnEnter() { // something happens?
		}
		
		protected override void OnExit() { }
			
		protected override void OnClick() { 
			//ZoneGlobal.inZone && 
			if (Input.GetButtonUp("mouse 1")) DSensorInput.Scan(this);
		}
		
		protected override void OnPress() { }
		protected override void OnRelease() { 
		}
		
		protected override void OnSelect() { }
		
		protected override void OnDeselect() { }
		
		
	}
}