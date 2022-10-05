
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


using Utility.GUI;
using SelectionSystem;
using System.Text;
using MEC;

namespace DialogueSystem
{


	
	public class DisplayParser : DParser {
		// This will be meant to parse something that left storage, a signal of some kind
		
		
		public DLocalAction localAction;
	
		public DisplayParser(DLocalAction displayedActions):base(){
			// make it, then start adding
			
			localAction = displayedActions;
		}
		
		
		
		
		public DText _procText ; 
		public DText procText 
		{
			get { return _procText; }
			set{
				
				if (_procText == value) return;
				_procText = value;
				
				ResetFilteredText();
				UpdateFilteredText();
				
			}
		}
		public DText _goalText ;
		public DText goalText 
		{
			get { return _goalText; }
			set{
				_goalText = value;
				if ( value != null ) 
				{
					if (value.parent != null)
						procText = value.parent;
					else
						procText = value;
					
				}
			}
		}
				
		public float tfilter = 0f;
		protected void ResetFilteredText(){
			discardPos = 0;
			dcPos = 0;
			if (filteredText == null ) return;
			filteredText.EmptyStream(); 
		
		}
		protected void UpdateFilteredText(){
			if (procText == null) return;
			if (filteredText == null ) filteredText = procText.Clone("");
			
			//CoroutineHandle handle; 
			
			//handle = Timing.RunCoroutineSingleton(DiscardPrefabs), handle);
			Timing.KillCoroutines("DiscardPrefabs");
			Timing.RunCoroutine(DiscardPrefabs());
				
			tfilter = Time.time;
		}
		
		protected override StringBuilder procs { 
			get{
				if (procText != null)
					return procText.storedText;
				else
					return null;
			
			} 
			
		} // mutated text
		protected override StringBuilder s { 
			get{return goalText.storedText;} 
			
		} // mutated text
		
		
		
		
		public virtual void Continue()
		{
			
			lastTick = Time.time;
			running = true;
			
			
		}
		public virtual void Load()
		{
			mediaType = "PlayerMedia";
			lastTick = Time.time;
			done = false;
			currentWord .Clear();
			c = ' ';
			running = true;
			inWordPos = 0;
			wordPos = 0;
			charPos = 0;
			
			streamedText = new DText();
			filteredText = new DText();
			
		}
		
		public float lastTick;
		public virtual void Next()
		{
			
			if (!running ) return;
			
			lastTick = Time.time;
			if ( goalText == null || goalText.storedText.Length < 1 )
			{
				ResetFilteredText();
				Stop();
				return;
			}
			
			
			
			if (charPos >= s.Length) 
			{
				done = true;
				goalText.isRead = true;
				Stop();
				return;
			}

			if ( procText.tmodified > tfilter)
			{
				UpdateFilteredText();
			}
			charAdvance = 1f;
			WalkStream(); // charPos++, charAdvance = 0
			
			
			if (charPos >= s.Length) 
			{
				done = true;
				goalText.isRead = true;
				Stop();
				return;
			}
			
			
			// alternatively set the positions and let it stream from there
		}
		public virtual void Stop()
		{
			running = false;
		}
			
		
		public virtual void Eject()
		{
			
			procText = goalText = streamedText = filteredText = null;
		}
		
		
		public bool showPrefab = false;
		public override void InvokePrefab()
		{
			// so is this where the magic happens?
			
			if (localAction != null)
			{
				DAction.Use(localAction.a);
				DAction.Do(currentPrefab.ToString());
			}
			// check the user's mood
			// not displayed: prefab , unless debug mode?
			if (showPrefab) streamedText.DAppend( currentPrefab);
			
		}
		
		
	}
}