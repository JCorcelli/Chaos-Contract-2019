
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

using Utility.GUI;
using SelectionSystem;



namespace DialogueSystem
{
		// bytes, powers of 2 or 1 << position
	// enum max 0, 1, 1<< 1 ... 1 << 30 (total 32 values)
	[System.Flags]
	public enum SensorEnum {
		Nothing		= 0,
		Video		= 1 << 0, // visible, infrared, micro, huge
		Audio		= 1 << 1, 
		Chemical  	= 1 << 2, // smell, taste, microbe
		Touch		= 1 << 3, // if wires touch your brain, do they read your mind?
		Psychic		= 1 << 4, 
		Magical		= 1 << 5
		// so, I could always use this to short-circuit skill checks
	}

	public class DSensor {
		// this is either a user or an object
		// this is a first pass before filters

		// BIOLOGICAL // ANALOGUE // SPIRIT is just a difference in terminology
		// modifiers like awake / numb should be filtered
		// it should be possible to sense microscopic objects if you have a very powerful filter
		
		
		
		public static Dictionary<string, DSensor> allTypes = new Dictionary<string, DSensor>()
		{
			["Default"] = new DSensor()
			{
				sensorMask = Convert.ToInt32("1111",2), 
				sensorType = "Basic"
			},
			 
			["Video"] = new DSensor()
			{
				sensorMask = 1, 
				sensorType = "Video" 
			},
			["Audio"] = new DSensor()
			{
				sensorMask = 1 << 1, 
				sensorType = "Audio" 
			},
			["Chemical"] = new DSensor()
			{
				sensorMask = 1 << 2, 
				sensorType = "Chemical" 
			},
			["Touch"] = new DSensor()
			{
				sensorMask = 1 << 3, 
				sensorType = "Touch" 
			},
			["Psychic"] = new DSensor()
			{
				sensorMask = 1 << 4, 
				sensorType = "Psychic" 
			},
			["Magical"] = new DSensor()
			{
				sensorMask = 1 << 5, 
				sensorType = "Magical" 
			},
			
			["Clairvoyant"] = new DSensor()
			{
				sensorMask = Convert.ToInt32("111111",2)
			}

		};
		
		public static DSensor New(string s){
			
			if (allTypes.ContainsKey(s))
				return allTypes[s].Clone();
			
			return allTypes["Default"].Clone();
		}
		public virtual DSensor Clone(){ 
			// so, be sure to replace this
			DSensor dm = Activator.CreateInstance(
			this.GetType()) as DSensor;
			dm.sensorType = this.sensorType;
			
			return dm;
		}
		
		public DMediaProc media = new DMediaProc(){mediaType = "Sensor.in"};
		
		
		// Scan(storage) : sets scanned
		public List<DText> scanned = new List<DText>();
		
		
		/* add to the current stack of text without
		*/
		public void Join(){
			
			for (int i = 0 ; i < scanned.Count ; i++)
			{
				media.Proc(scanned[i]);
			}
		}
		/* push the previous stack out, but not automatically
		*/
		public void Push(){
			media.procList.Clear();
			media.Step(); // calls ending
			for (int i = 0 ; i < scanned.Count ; i++)
			{
				media.Proc(scanned[i]);
			}
		}
		
		public int sensorMask = 0;
		
		public string sensorType = "None";
		
		protected void Scan(DUser st){
			
			
			
			DStorage dt = st.storage; // scan what someone is "using"
			if (dt != null) Scan(dt);
			// not sure how I want this to really work
			// if this gathered some information from each and moved on it might make sense
			DStorage nt;
			for (int i = 0 ; i < st.nstorage.Count ; i++)
			{
				nt = st.nstorage[i];
				if (nt == dt) continue;
				Scan(nt) ;
			}
			
			// after delay?
		}
		protected void Scan(DStorage st){
			
			
			
			DText dt;
			
			// not sure how I want this to really work
			// if this gathered some information from each and moved on it might make sense
			for (int i = 0 ; i < st.storedText.Count ; i++)
			{
				dt = st.storedText[i];
				Scan(dt) ;
			}
			
			// after delay?
			
			
		}
		protected void DoNothing(){}
		
		// this is actually a description
		protected void Scan(DScannable st){
			
			
			// supertext = dict<string,DText>
			var dict = st.GetDetails();
			media.AddFlavor(dict["Default"]);
			var v = dict.Values;
			foreach (DText dt in  v)
			{
				if (dt != null)
				{
					Scan(dt) ;
				}
			}
			
			
		}
		// quality order contain||intersect, hmm, how much info was lost? = spills
		public void Contains( DText t ){
			sensorMask.IsMasking(t.sensorMask);
		}
		
		public bool NoMask( DText t ){
			return (t.sensorMask < 1);
		}
		public int Intersection( DText t ){
			return (sensorMask & t.sensorMask);
		}
		public int Spills( DText t ){
			return (~sensorMask & t.sensorMask);
		}
		
		public void Scan<T>( T t ){
			
			// ADD THE SUPERFICIAL INFO IMMEDIATELY
			
			if (t is DScannable) 	Scan( t as DScannable);
			
			// ADD THE DEEPER CONTENT, IF APPLICABLE 
			if (t is DRealUser) 	Scan( (t as DRealUser).myUser);
			else if (t is DRealStorage) Scan( (t as DRealStorage).storage);
			else if (t is DText) 	Scan( t as DText);
			
			
		}
		public void Scan( DText t ){
			
			if (t == null) return;
			
			if (NoMask(t) || this.Intersection(t) > 0) scanned.Add(t) ; // when do I remove this?
		}
	}
	
}