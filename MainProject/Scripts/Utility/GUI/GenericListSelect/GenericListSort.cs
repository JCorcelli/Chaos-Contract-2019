using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;


using SelectionSystem;
using SelectionSystem.IHSCx;

namespace Utility.GUI
{
	
	public class GenericListSort : IHSCxConnect
	{
		// demo / debug class with functions easily copied
		
		public GenericListBuilder builder;
		
		

		protected override void OnEnable() {
			base.OnEnable();
			ih.onClick += OnClick;
			
		}
		protected override void OnDisable() {
			base.OnDisable();
			ih.onClick -= OnClick;
			
		}
		

		protected void OnClick(HSCxController caller)
		
		{
			builder.Sort();
			
		}
		
		
	}
		
}