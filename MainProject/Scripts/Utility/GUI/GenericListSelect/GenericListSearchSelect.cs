using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;


using SelectionSystem;
using SelectionSystem.IHSCx;

namespace Utility.GUI
{
	
	public class GenericListSearchSelect : IHSCxConnect
	{
		// demo / debug class with functions easily copied
		
		public GenericListBuilder builder;
		
		public string prevVar = "";
		public string stringVar = "";
		
		public InputField text;

		protected override void OnEnable() {
			base.OnEnable();
			ih.onClick += OnClick;
			
		}
		protected override void OnDisable() {
			base.OnDisable();
			ih.onClick -= OnClick;
			
		}
		
		protected override void OnUpdate(){
			base.OnUpdate();
			stringVar = text.text;
			if (stringVar == prevVar) return;
			
			prevVar = stringVar;
			
			if (stringVar == "")
				builder.SearchReset();
			// this is just a demo
			else
				builder.SearchSelected(stringVar);
		}
		

		protected void OnClick(HSCxController caller)
		
		{
			
			text.text = "";
			
		}
		
		
	}
		
}