
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


	public class DMediaProc : StaticDialogueHubHook {
		// This will complete flavor text
		
		public struct FlavorEnding {
			public int index ;
			public DText text;
		}
		
		public DLocalAction localAction{
			get => (parser != null) ? parser.localAction : null;
			set {
				if (parser == null) parser = new DMediaParser(){ mediaType = "Media" };
				parser.localAction = value;
			}
		}
		public DMediaParser parser;
		public List<DText> procList = new List<DText>(); // original stored text
		public DText mediaText = new DText(); // text that gets pushed into stream immediately
		public FlavorEnding[] endings = new FlavorEnding[]{new FlavorEnding(){ index = -1 }, new FlavorEnding(){ index = -1 }}
			; // text displayed after action stops
		
		public int selected = -1;
		public bool running => selected >= 0;
		public DText s {get{return mediaText;} } // mutated text
		
		
		public string mediaType = "Read"; // caused by flaws, environment, distortion. Word for word.
		
		
		public DMediaProc():base()
		{
			
		}
		
		// untested
		
		public virtual bool AddFlavor(DText startFlavor, DText endFlavor = null, string typeOfAction = "Flavor"){
			
			string s = startFlavor.DToString();
			string e = endFlavor != null ? endFlavor.DToString() : "";
			return AddFlavor(s,e, typeOfAction);
		}
		public virtual bool AddFlavor(string startFlavor, string endFlavor = "", string typeOfAction = "Flavor"){
			
			mediaType = typeOfAction;
			
			s.Clear();
			
			s.DAppend(startFlavor);
			
			// beginning of action
			hub.AddStreamedText(mediaText.Clone());
			s.Clear();
			
			if (endFlavor != "")
			{		
				s.DAppend(endFlavor);
			
				PushEnding();
			}
			return true;
		}
		
		public void EndAll(){
			if (endings[1].index >= 0) 
			{	
				hub.AddStreamedText(endings[1].text.Clone());
				hub.AddStreamedText(endings[0].text.Clone());
			}	
			else if (endings[0].index >= 0) hub.AddStreamedText(endings[0].text.Clone());
			endings[1].index = endings[0].index = -1;
		}
		public void PullEnding(){
			// removes the oldest ending, replaces with newer
			hub.AddStreamedText(endings[0].text.Clone());
			if (endings[1].index >= 0) {
				endings[0].text = endings[1].text;
				endings[1].index = -1;
			}
			else 
			
				endings[0].index = -1;
				
			
			
			
		}
		public void PushEnding(){
			// removes what I'm not currently looking at, replaces it with something new
			if (endings[1].index >= 0) {
				int cutside = Cut(endings[1].index); // the midpoint
				
				if (cutside < 1 ) // left removed, fall left
					endings[0].text = endings[1].text;
					
			
				endings[1].index = procList.Count;
				endings[1].text = mediaText.Clone();
				
				
			}
			
			else if (endings[0].index < 0) 
			{
				endings[0].index = 0;
				endings[0].text = mediaText.Clone();
			}
			
			else
			{
				endings[1].index = procList.Count;
				endings[1].text = mediaText.Clone();
			}
		}
		
		
		
		public virtual bool Proc(DText pt)
		{
			if (pt == null) return false;
			procList.Add(pt);
			return Proc();
			
		}
		public virtual bool Proc(List<DText> pt)
		{
			if (pt.Count < 1)  return false;
			procList.AddRange(pt);
			return Proc();
		}
		public virtual bool Proc( )
		{
			
			
			
			if (  procList.Count < 1) return false;
			// This instantly adds an action
			
			if (selected < 0) 
			{
				selected = 0;
				
				if (parser == null) parser = new DMediaParser();
				parser.procText = procList[0] ;
				parser.goalText = procList[0].Clone() ;
				
				
				parser.Load();
			}
			// Sending information everywhere
			// estimate time to completion
			/* 
				
				2.5% receive references to the source
				17.5% receive references
			
				30% receive a clone
				---
				30% receive a mutated clone
				
				17.5% receive a strong-mutated clone
				
				2.5% receive a clone with altered parameters
				
			*/
			return true;
		}
		
		
		public virtual int Select(DText pt)
		{
			if (procList.Contains(pt))
			{				
				selected = procList.IndexOf(pt);

				if (selected == prev) parser.Continue();
				else
					parser.Load();
				return selected;
			}

			return -1;
		}
		public int prev = -1;
		public virtual void Continue(){
				parser.Continue();
		}
		
		// stop = pause, otherwise call Reset
		public virtual void Reset(){ 
			parser.Load();
		}
		public virtual void Stop(){ 
			parser.Stop();
			selected = -1; 
		}
		public virtual void Cancel()
		{
			EndAll(); // try to end both actions
			procList.Clear();
			parser.Stop();
			parser.Eject();

			prev = selected = -1;
		}
		public int Cut(int index)
		{
			// removes what I'm not currently looking at
			if (selected >= index)
			{	
				procList.RemoveRange(0, index);
				selected -= index;
				
				hub.AddStreamedText(endings[0].text.Clone());
				return 0;
			}
			else 
			{	
				procList.RemoveRange(index, procList.Count - index);
				hub.AddStreamedText(endings[1].text.Clone());
			}
			return 1;
				
		}
		public virtual void Step()
		{
			if (selected < 0) return;
			
			
			
			parser.Next();
		}
	}
}