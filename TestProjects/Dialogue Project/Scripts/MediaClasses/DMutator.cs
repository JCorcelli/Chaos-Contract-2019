
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

using Utility.GUI;
using SelectionSystem;


namespace DialogueSystem
{

	public class DMutator {
		/*
			Bound to storage, and environment
			
			1 read it the way you intend to mutate it
			2 don't bother storing extra variables
		*/
		  
		  
		public static Dictionary<string, DMutator> allTypes = new Dictionary<string, DMutator>(){
			["Default"] = new DMutator(), 
			["Concrete"] = new DMutationConcrete()
			};
		public static List<DMutator> all = new List<DMutator>();
		public bool isAlive = true;
		
		public List<WeakReference<DText>> storedText = new List<WeakReference<DText>>(); // basically, everything you need to mess with
		
		
		public string mutatorType = "Default";
		
		public static DMutator New(string s){
			
			if (allTypes.ContainsKey(s))
				return allTypes[s].Clone();
			
			return allTypes["Default"].Clone();
		}
		
		// public[s]*.*Clone[s]*\(
		public virtual DMutator Clone(){ 
			// so, be sure to replace this
			
			// this.MemberwiseClone()
			DMutator dm = Activator.CreateInstance(
			this.GetType()) as DMutator;
			dm.mutatorType = this.mutatorType;
			
			return dm;
		}
		
		
		protected DText temp;
		public virtual void MutateAll(){
			for (int i = 0 ; i < storedText.Count ; i++)
			{
				storedText[i].TryGetTarget(out temp);
				if (temp != null)
				
					Mutate(temp);
				else
					storedText.RemoveAt(i--);
			}
			temp = null;
		}
		public virtual void Mutate(DText text){
			if (text == null) return;
		}
		public virtual void Add(DText text)
		{
			storedText.Add(new WeakReference<DText>(text ));
		}
		
		
		public DMutator()
		{
			if (all == null) return;
			all.Add(this);
		}
		public void OnDestroy()
		{
			if (all == null) return;
			all.Remove(this);
		}
		public void Step(){
			//if (false)
				
			if (!isAlive || storedText == null) return;
					
			MutateAll();
				
		}
	}
	
	public class DMutationConcrete : DMutator {
		// This will be meant to parse... or mutate instantly
		
		public override void Mutate(DText text)
		{
			if (text == null) return;
		}
		public DMutationConcrete():base()
		{
			mutatorType = "Concrete"; 
			
		}
		
	}
	
}