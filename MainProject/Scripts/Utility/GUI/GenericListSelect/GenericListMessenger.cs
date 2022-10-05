using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;


using SelectionSystem;
using SelectionSystem.IHSCx;

namespace Utility.GUI
{
	
	public class GenericListMessenger : SelectAbstract
	{
		// demo / debug class with functions easily copied
		
		public GenericListBuilder builder;
		public string inputMessage = "Select";
		

		

		protected override void OnClick()
		
		{
			builder.SetMessage(inputMessage);
			
		}
		
		
	}
		
}