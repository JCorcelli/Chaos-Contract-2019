using UnityEngine;


using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;




namespace DialogueSystem 
{
	
		
	public class DStreamParser : DParser
	{
		// this is actually a Receiver (eg. ear, membrane)
		// A multi-source stream is gathered based on physics
	
		/* // Current Model 
			Sense : receive all of them
			Interval : always on 
			range : anywhere
				requires:
				this position
				target position
				falloff volume NA
				
				
		*/
		public List<DFilter> filters = new List<DFilter>();
		public DLocalAction localAction;
	
		public DStreamParser(DLocalAction receiverActions):base(){
			// make it, then start adding
			
			localAction = receiverActions;
		}
		
		public List<DText> streamList = new List<DText>()	;
		// this could filter them without combining, producing an outstream
		
		protected DText chunkText ; // used in Walk
		

		public bool gathering = false;
		
		
		protected override StringBuilder s { 
			get{return chunkText.storedText;} 
		} // mutated text
		
		
		

		
		public int[] interval = new int[]{0,0};
		
		public int success = 0;
		public float lastTick;
		public void SetStream(float start = 0, float end = -1f){
			GetInterval(start, end);
			
			if (success < 2) return; /// < 1 stutters, fail dupes the entire string
			
			//Debug.Log($"i0:{interval[0]}...i1:{interval[1]}");
			//Debug.Log(hub.streamedText.Count);
			
			streamList.AddRange( hub.streamedText.GetRange(interval[0],interval[1] - interval[0]));
		}
		public void GetInterval(float start = 0, float end = -1f){
			// start time end time
			int list_end = hub.streamedText.Count - 1;
			int list_start = 0;
			
			success = 0;
			
			if (list_end < 0) return;

			if (hub.streamedText[list_end].tstream < start) 
			{
				return;
			}
			
			interval[0] = list_start;
			interval[1] = list_end;
			
			if (end > -1f)
			{
				// end is earlier than start
				if (end - start <= 0f) return ;
				
				for (int i = list_end; i >= 0; i --)
				{
					if (hub.streamedText[i].tstream <= end)
					{
						success++;
						if (list_end + 1< i + 1) break;
						
						list_end = i;
						interval[1] = i + 1;
						break;
					}
				}
			}
			
			
			if (start > 0f)
			for (int i = list_end; i >= 0; i --)
			{
			
				if (hub.streamedText[i].tstream > start)
				{
					success++;
					if (0 > i - 1 ) break ;
					
					interval[0] = i  ;
					break;
				}
			}
			
			
			
		}
		
		
		public virtual void Load()
		{
			mediaType = "Receiver"; 
			lastTick = Time.time;
			
			done = false;
			streamedText = new DText(); // everything's appending to this text?
			streamList = new List<DText>(); 
			
			currentWord .Clear();
			c = ' ';
			running = true;
			inWordPos = 0;
			wordPos = 0;
			charPos = 0;
			
		}
		
		
		public virtual void Next()
		{
			
			if (!running || localAction == null || hub == null ) return;
			
			
			if (lastTick == Time.time) return;
			
			int listPos = streamList.Count;
			float delta = cps * (Time.time - lastTick);
			SetStream(lastTick, Time.time);
			//sets streamList
			lastTick = Time.time;
			if (success < 2) return;
			
			
			// I'm combining into a single unbroken string
			// unfiltered: this will be a hell of a mess
			
			
			stopwatch.Start();
			
			for (int i = listPos; i < streamList.Count && stopwatch.ElapsedMilliseconds < maxTime; i++)
			{
				chunkText = streamList[i];
				
				charPos = 0;
				
				// this only makes sense for information loss
				
				int xlen = filters.Count;
				
				
				if (xlen > 0)
				for (int x = 0 ; x < xlen ; x++)
				{
					filters[x].Interpolate(chunkText,delta);
					//chunkText.isDead = DFilter.dead;
					
					if (DFilter.dead) break;
				}
				
				if (chunkText.isDead) 
				{
					streamList.RemoveAt(i--);
					continue;
				}
				// s.Length might change?
				if (DFilter.captured)
				{
					
					// it seems like all this does is get prefabs
					while ( charPos < s.Length && running   )
					{
						WalkStream();
					}
					chunkText.isRead = true;
				}
				
			}
			stopwatch.Stop();
			
			// alternatively set the positions and let it stream from there
			
		}
		
		public virtual void Stop()
		{
			running = false;
			
		}
			
		
		public virtual void Eject()
		{
			
			streamedText = null;
			streamList = null;
		}
			
		
		public override void InvokePrefab()
		{
			// so is this where the magic happens?
			if (localAction != null)
			{
				DAction.Use(localAction.a);
				DAction.Do(currentPrefab.ToString());
			}
			
			streamedText.DAppend( currentPrefab.ToString());
			
			
		}
		
	}
}