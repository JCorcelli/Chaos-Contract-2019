using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace NPC.Strategy
{
	public delegate void SHUB();
	public class StrategyHUB : MonoBehaviour {

	
		// basic functionality for lookup on enables
		protected HashSet<string> _list = new HashSet<string>();
		public HashSet<string> list{get{return _list;} protected set{}}
		
		// recent strings
		public string recentIn = "";
		public string recentOut = "";
		
		// delegates
		
		public SHUB onStart;
		public SHUB onStop;
		
		
		public void Clear(){
			list.Clear();
		}
		public void Add(string nameIn)
		{
			list.Add(nameIn);
			
			recentIn = nameIn;
			if (onStart != null) onStart();
			
			
		}
		public void Remove(string nameIn)
		{
			list.Remove(nameIn);
			
			recentOut = nameIn;
			if (onStop != null) onStop();
			
		}
	}
}