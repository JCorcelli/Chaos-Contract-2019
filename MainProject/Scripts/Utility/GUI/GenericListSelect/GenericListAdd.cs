using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;


using SelectionSystem;
using SelectionSystem.IHSCx;

namespace Utility.GUI
{
	
	public class GenericListAdd : IHSCxConnect
	{
		// demo / debug class with functions easily copied
		
		public GenericListBuilder builder;
		
		public string stringVar = "Unknown";
		

		protected override void OnEnable() {
			base.OnEnable();
			ih.onClick += OnClick;
			
		}
		protected override void OnDisable() {
			base.OnDisable();
			ih.onClick -= OnClick;
			
		}
		
		public void SetStringFromText(Text s) {
			if (s.text == "") return;
			// string traditionally set from an input field
			stringVar = s.text;
			
			// I could add a find or something?
		}
		
		public bool append = false;

		protected void OnClick(HSCxController caller)
		
		{
			// this is just a demo
			if (append)
				builder.Append(stringVar);
			else
				builder.Add(stringVar);
		}
		
		
	}
		
}