
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

using Utility.GUI;
using SelectionSystem;


namespace DialogueSystem
{

	public class DOrganizer : DProcessor{
		// goes into storage variable
		
		// very simple and robotic sort
		
		
		public DOrganizer():base(){
			mediaType = "ForwardText";
		}

		public override void Load(List<DText> t) { 
			input = t; 
		}
		public override void Eject() { 
			input = output = null;
		}
		public override void Exe(List<DText> t = null)
		{
			output = new List<DText>();
			if (t != null) input = t;
			if (input == null || input.Count < 1 ) return;
			
			Dictionary<DText,DText> dict = new Dictionary<DText,DText>();
			DText c;
			DText cat;
			for (int i = 0 ; i < input.Count; i++)
			{
				c = input[i];
				
				if (c.parent == null) 
				{
					dict.Add(c, c);
					
					continue;
				}
				if (!dict.ContainsKey(c.parent))
				{
					cat = c.Clone();
					
					dict[c.parent] = cat;
				}
				else 
				{
					cat = dict[c.parent];
					cat.DAppend(c.storedText.ToString());
					
				}
			}
			
			Dictionary<DText,DText>.ValueCollection valueColl =
			dict.Values;
			
			foreach (DText d in valueColl)
			{
				output.Add(d);
			}
			
			
			output.Sort(TimeSort);
		
		
			// and then process / refine the data
			Filter();
			
		}
		public int TimeSort(DText a, DText b)
		{
			if (a.tstream < b.tstream) return -1;
			return 1;
		}
	}
	
	
	
}