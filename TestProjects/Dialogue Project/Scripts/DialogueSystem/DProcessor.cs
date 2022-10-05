
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

using Utility.GUI;
using SelectionSystem;


namespace DialogueSystem
{

	public class DProcessor {
		// goes into storage variable
		
		// very simple and robotic sort
		
		public List<DFilter> filters = new List<DFilter>();
		
		
		public static Dictionary<string, DProcessor> allTypes = new Dictionary<string, DProcessor>(){
			["Default" 	] = new DProcessor(), 
			["Organizer"] = new DOrganizer()
		};
		public List<DText> input; 
		
		
		public List<DText> output; // the resulting sort
		
		public string mediaType = "Filter"; // assumes the text is earliest first
		
		public DProcessor(){
			
		}
		
		public static DProcessor New(string s){
			
			if (allTypes.ContainsKey(s))
				return allTypes[s].Clone();
			return allTypes["Default"].Clone();
		}
		
		public virtual DProcessor Clone(){ 
			// so, be sure to replace this
			
			DProcessor dm = Activator.CreateInstance(
			this.GetType()) as DProcessor;
			dm.mediaType = this.mediaType;
			
			
			return dm;
		}
		
		public virtual void Load(List<DText> t) { 
			input = t; 
		}
		public virtual void Eject() { 
			input = output = null;
		}
		public virtual void Exe(List<DText> t = null)
		{
			output = new List<DText>();
			if (t != null) input = t;
			if (input == null || input.Count < 1 ) return;
			
			
			
			DText c;
			DText cat;
			for (int i = 0 ; i < input.Count; i++)
			{
				c = input[i];
				
				cat = c.Clone();
					
				output.Add(cat);
			}
			
			
			Filter();
		}
		public virtual void Filter(){
			DText chunkText;
			float delta = 1f;
			int xlen = filters.Count;
			if (xlen > 0)
			for (int i = 0; i < output.Count; i++)
			{
				chunkText = output[i];
				
				
				DFilter.dead = false;
				for (int x = 0 ; x < xlen ; x++)
				{
					filters[x].Interpolate(chunkText,delta);
					if (DFilter.dead) break;
				}
			}
		}
	}
}