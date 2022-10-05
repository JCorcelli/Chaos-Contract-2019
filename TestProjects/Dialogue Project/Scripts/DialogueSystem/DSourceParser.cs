
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


	
	public class DSourceParser : DParser {
		// This will be meant to parse... if storage is the brain, this is everything else never touching the brain
		public TextAsset textAsset;
		public DLocalAction localAction;
		protected override StringBuilder s {
			get{
			return streamedText.storedText;}
		}
		public bool useGlobal = false;
		
		
		
		public void Load(){
			charPos = 0;
			running = true;
			done = false;
		}
		public void Stop(){
			running = false;
		}
		
		public override void InvokePrefab()
		{
			// so is this where the magic happens?
			
			DAction.Use(localAction.a);
			DAction.Do(currentPrefab.ToString());
			
			if (useGlobal)
			{
				DAction.Use();
				DAction.Do(currentPrefab.ToString());
			}
		}
		
		
		
		public void CoPrefabs(){
			Timing.KillCoroutines("_CoPrefabs");
			Timing.RunCoroutine(_CoPrefabs());
		}
		public IEnumerator<float> _CoPrefabs()
		
		{
			
			if (s == null) yield break;
			stopwatch.Start();
			
			
			while (charPos < s.Length ) 
			{
				c = s[charPos++];
				
				while (charPos < s.Length && c!= '{') 
				{
					
					if (stopwatch.ElapsedMilliseconds > maxTime) 
					{
						
						stopwatch.Stop();
						
						yield return Timing.WaitForOneFrame;
					}
			
					c = s[charPos++];
				}
				
				int pairs = 1;
				
				currentPrefab.Append( c);
				
				while (pairs > 0 && charPos < s.Length)
				{
					
					c = s[charPos++];
					currentPrefab.Append( c);
					if (c == '}') pairs --;
					else if (c == '{') pairs ++;
					
					if (stopwatch.ElapsedMilliseconds > maxTime)
					{
						stopwatch.Stop();
						yield return Timing.WaitForOneFrame;
						
					}
				}
				
				InvokePrefab();
				
				currentPrefab.Clear() ;
				
				// exit code?
				if (!running) 
				{
					done = true;
					yield break;
				}
				
			}
			
			
			if (charPos >= s.Length ) 
			{

				done = true;
				Debug.LogError("EOF: No call to {source.end}", textAsset);
				stopwatch.Stop();
				
			}
			
			stopwatch.Stop();
			

		}
		public void ImmediatePrefabs()
		{
			
			if (s == null) return;
			stopwatch.Start();
			
			
			while (charPos < s.Length ) 
			{
				c = s[charPos++];
				
				while (c!= '{') 
				{
					if (charPos >= s.Length) 
					{
						done = true;
						Debug.LogError("No call to {source.end}", textAsset);
						stopwatch.Stop();
						return;
					}
					c = s[charPos++];
				}
				
				
				// need a pre-enter setup
				int pairs = 1;
				// it's a prefab
				
				currentPrefab.Append( c);
				while (pairs > 0 && charPos < s.Length)
				{
					c = s[charPos++];
					currentPrefab.Append( c);
					
					if (c == '}') pairs --;
					else if (c == '{') pairs ++;
				}
				InvokePrefab();
				currentPrefab.Clear() ;
				
				
				// exit code?
				if (!running) 
				{
					stopwatch.Stop();
					return;
				}
				
				
			}
			
			
			if (charPos >= s.Length) 
			{
				
				Debug.LogError("EOF. No call to {source.end}", textAsset);

				done = true;
				stopwatch.Stop();
				return;
			}
			

		}
		public void Qualify()
		{
			
			if (s == null) return;
			stopwatch.Start();
			c = s[charPos++];
			
			while (System.Char.IsWhiteSpace(c))
			{
				
				if (charPos >= s.Length ) 
				{
					
					Debug.Log("EOF. Blank.", textAsset);

					done = true;
					stopwatch.Stop();
					
					return;
				}
				c = s[charPos++];
				
			}
			
			if (c == '{') 
			{
				// need a pre-enter setup
				int pairs = 1;
				// it's a prefab
				
				currentPrefab.Append( c);

				while (pairs > 0 && charPos < s.Length)
				{
					c = s[charPos++];
					currentPrefab.Append( c);
					if (c == '}') pairs --;
					else if (c == '{') pairs ++;
					
				}
				InvokePrefab();
				currentPrefab.Clear() ;
				stopwatch.Stop();
				
				
			}
			
			

		}
		
	}
}	