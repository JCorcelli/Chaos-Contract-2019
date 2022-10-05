
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


	
	public class StaticMediaHub  : StaticHubConnect {
		// this is where all the libraries can be accessed at once, for character knowledge reference
		
		public List<DMediaLibrary> libraries = new List<DMediaLibrary>();
		
		
		// could add some more variables to determine if someone would access this 
		protected DMediaLibrary _selected;
		public DMediaLibrary selected{get{return _selected;}set{_selected = value;}}
		public static StaticMediaHub instance;
		
		public void AddLibrary(DMediaLibrary newLibrary){
			selected = newLibrary;
			DSource.validate = false;
			newLibrary.Build();
			DSource.validate = true;
			OnChange();
			libraries.Add(newLibrary);
			
		}
		
		protected override void OnEnable( ){
			
			
			if (instance != null && instance != this){ 
				Destroy(this);
			
				return;
			}
			
			base.OnEnable();
			instance = this;
			
			
			DSource.validate = false;
			Build();
			DSource.validate = true;
			
			CheckConnected();
			
			
			
		
		}
		public void Build()
		{
			for (int i = 0 ; i < libraries.Count ; i++)
			{
				libraries[i].Build();
				selected = libraries[i];
				OnChange();
			}
		}
		
	}
}