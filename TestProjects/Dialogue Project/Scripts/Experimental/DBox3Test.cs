
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

using Utility.GUI;
using SelectionSystem;



namespace DialogueSystem
{
	public class DBox3Test : UpdateBehaviour {
		// This is a basic formulae of connecting with StaticHub
		
		public DStream dbox;
		public Text box1;
		public Text box2;
		public Text box3;
		
		
		
		
		protected void Awake()
		{
			
			dbox = new DStream();
			dbox.redparser = new DRedBroken() as DRedParser;
				
			if (!box1 || !box2 || !box3) Debug.LogError("missing some boxes!");
			
		}
		protected override void OnUpdate()
		{
			base.OnUpdate();
			
			if( dbox.procText != null) 
				box1.text = "CODE\n"+dbox.procText.storedText;
			
			
			if (dbox.goalText != null) 
				box2.text = dbox.goalText.storedText.ToString();
			
			box3.text = "streamed text must be parsed now";//dbox.streamedText.storedText.ToString();
			
			
		}



		
	}
}