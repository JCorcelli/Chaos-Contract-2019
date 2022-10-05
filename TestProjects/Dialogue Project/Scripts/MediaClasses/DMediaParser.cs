
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


using Utility.GUI;
using SelectionSystem;
using System.Text;


namespace DialogueSystem
{


	
	public class DMediaParser : DParser {
		// This will be meant to parse something that left storage, a signal of some kind
		
		public List<DFilter> filters = new List<DFilter>();
		
		public DLocalAction localAction;
	
		public DMediaParser():base(){
			// make it, then start adding
			
		}
		
		public DText procText ; // text that was spoken
		public DText goalText ; // text that was interpreted
		//inherits streamedText; // text that's viewed

		
		protected override StringBuilder s { 
			get{return goalText.storedText;} 
			
		} // mutated text
		
		
		
		
		public virtual void Continue()
		{
			lastTick = Time.time;
			done = false;
			running = true;
		}
		
		public virtual void Load()
		{
			lastTick = Time.time;
			
			done = false;
			currentWord .Clear();
			c = ' ';
			running = true;
			inWordPos = 0;
			wordPos = 0;
			charPos = 0;
			
		}
		
		public float lastTick;
		public virtual void Next()
		{
			
			if (!running ) return;
			if (hub == null || goalText == null || goalText.storedText.Length < 1 )
			{
				Stop();
				return;
			}
			
			
			float deltaTime = Time.time - lastTick;
			
			lastTick = Time.time;
			
			float delta = cps * deltaTime;
			charAdvance += delta;
			
				
			if (charAdvance < 0f) return;
			
			
			int xlen = filters.Count;
			
			// filter: you got poked in the eye!
			if (xlen > 0)
			for (int x = 0 ; x < xlen ; x++)
			{
				filters[x].Interpolate(goalText, delta);
				
				if (DFilter.dead) break;
			}
			


			// I'm setting the streamed text chunk before the loop
			
			streamedText = goalText.Clone("");
			
			streamedText.parent = goalText;
			streamedText.parentIndex = charPos;
			streamedText.EmptyStream();
			
			hub.AddStreamedText(streamedText);
						
			if (DFilter.dead) // it was killed at the source
			{
				Stop();
				return;
			}	
			if (charPos >= s.Length) 
			{
				goalText.isRead = true;
				Stop();
				return;
			}

			stopwatch.Start();
			while (running && charAdvance >= 1f && stopwatch.ElapsedMilliseconds < maxTime)
			{
				
				WalkStream();
				
				if (charPos >= s.Length) 
				{
					goalText.isRead = true;
					Stop();
					return;
				}
			}
			stopwatch.Stop();
			
			// alternatively set the positions and let it stream from there
		}
		public virtual void Stop()
		{
			running = false;
			
			charAdvance = 0f;
		}
			
		
		public virtual void Eject()
		{
			
			procText = goalText = streamedText = null;
		}
		
		
		
		public override void InvokePrefab()
		{
			// so is this where the magic happens?
			
			if (localAction != null)
			{
				DAction.Use(localAction.a);
				DAction.Do(currentPrefab.ToString());
			}
			// check the user's mood
			streamedText.DAppend( currentPrefab);
			
		}
		
	}
}