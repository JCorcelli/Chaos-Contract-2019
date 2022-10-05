
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using MEC;

using Utility.GUI;
using SelectionSystem;
using System.Text;


namespace DialogueSystem
{


	
	public abstract class DParser {
		// This will be meant to parse... if storage is the brain, this is everything else never touching the brain
		
		public static System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
		public const int maxTime = 10; // ms
		protected StaticDialogueHub hub
		{ get{ return hook.hub; } }
		protected StaticDialogueHubHook hook;
		
		public DParser()
		{
			hook = new StaticDialogueHubHook();
		}
		public DText streamedText; // text that's viewed
		public DText filteredText; // text that's viewed

		
		
		public bool done = false;
		public bool running = false;
		public StringBuilder currentWord = new StringBuilder();
		public StringBuilder currentPrefab = new StringBuilder();
		
		
		
		public int wordPos = 0;
		public int charPos = 0;
		public int inWordPos = 0;
		protected virtual StringBuilder s { 
			get{return null;} 
			
		} // mutated text
		protected virtual StringBuilder procs { 
			get{return s;} 
			
		} // mutated text
		
		public string mediaType = "Default"; // caused by flaws, environment, distortion. Word for word.
		
		public char c;
		
		public float cps = 300f;
		public int filterAllowance = 500;
		
		public float charAdvance = 0f;
		
		
		
		public virtual void InvokePrefab()
		{
			// so is this where the magic happens?
			
			DAction.Use();
			DAction.Do(currentPrefab.ToString());
			// check the user's mood
			streamedText.DAppend( currentPrefab);
			
		}
		
		
		public virtual void WalkWord()
		{
			// take one step
			// if conditions, then foo. May replace word.

			// or just add the character
			
			if (inWordPos < currentWord.Length)
				c = currentWord[inWordPos++];
			
			
			
			streamedText.DAppend( c);

		}
		
		
		
		public void GatherWord()
		{
			
			// This is an instant word grab at wordPos
			// currentWord == ""
			int pairs = 0;
			
			for (int i = wordPos; i < s.Length; i++)
			{
				c = s[i];
				if (c == '{')
				{
					pairs ++;
				}
				else if (pairs != 0)
				{
					if (c == '}') 
					{
						pairs --;
					
						// 0 sum creating a prefab
					}
				}
					
				else if (c.isWhiteSpace())
				{
					return; // end of word I guess, how does it handle names?
				}
				else 
					currentWord.Append( c);
				

			}
			// just for words
			
		}
		
		
		public virtual void WalkStream()
		{
			if (s == null) return;
			c = s[charPos];
			
			charAdvance -= 1f;
			int pairs = 0;
			
			while (c == '{') 
			{
				// need a pre-enter setup
				// it's a prefab
				
				charPos++;
				currentPrefab.Append( c);
				
				pairs = 1;
				while (pairs > 0 && charPos < s.Length)
				{
					
					c = s[charPos++];
					currentPrefab.Append( c);
					
					if (c == '}') pairs --;
					else if (c == '{') pairs ++;
					
				}
			
				InvokePrefab();
				
				currentPrefab.Clear() ;
				
				if (charPos >= s.Length) 
				{
					return;
				}
				
				c = s[charPos];
				
			}
			
			if (c.isWhiteSpace() )
			{
				// outside a prefab, words. word barrier
				streamedText.DAppend( c);


				currentWord.Clear();
			}
			else
			{
				if (currentWord.Length < 1)
				{
					// word init""
					wordPos = charPos;
					inWordPos = 0;
					GatherWord();
					
				}
				WalkWord(); // some reason it needs to allow ""?
			}
				
			
			
			charPos++;
			

		}
		
		
		public int discardPos= 0;
		public int dcPos	 = 0;
		
		
		public static void DiscardPrefabsEP(DText s, int[] endpoints){
			int length = endpoints.Length;
			
			for (int i = length - 1 ; i >= 0; i -= 2)
				// right to left so there's some fidelity
				s.DRemove(endpoints[i - 1], endpoints[i] - endpoints[i - 1]);
			
		}
		public static string[] ListPrefabsEP(DText s, int[] endpoints){
			int length = endpoints.Length;
			
			int insertPos = length / 2;
			string[] prefabs = new string[insertPos--];
			for (int i = length - 1 ; i >= 0; i -= 2)
			{
				// right to left so there's some fidelity
				prefabs[insertPos--] = s.DToString(endpoints[i - 1], endpoints[i] - endpoints[i - 1]);
			}
			return prefabs;
		}
		
		
		
		public static void ShiftPrefabInWord(int[] endpoints, int pos, int amount, int wordPos, int wordLength){
			if (wordLength <= 0) return;
			int length = endpoints.Length;
			float p;
			for (int i = length - 1 ; i >= 0; )
			{
				
				// check if I'm shifting left past word boundary
				p = (float)endpoints[i] - wordPos;
				if (p == 0) continue;
				p /= wordLength;
				if (Mathf.Sign(amount) < 0) p+= 1f;  // at least subtext_shift must be added or subtracted in order to move this position
				
				endpoints[i--] += amount * (int)p;
				
				endpoints[i--] += amount * (int)p;
				
			}
			
		}
		public static void ShiftPrefabEndpoints(int[] endpoints, int pos, int amount){
			
			int length = endpoints.Length;
			
			for (int i = length - 1 ; i >= 0; )
			{
				if (endpoints[i] < pos) break;
				
				// check if I'm shifting left past word boundary
				int maxneg = pos - endpoints[i - 1];
				if (amount < maxneg)
				{
					endpoints[i--] += maxneg;
					endpoints[i--] += maxneg;
				}
				else
				{		
					endpoints[i--] += amount;
					endpoints[i--] += amount;
				}
			}
			
		}
		
		// this is hypothetical...
		public static int[] GetWordAroundPrefabs(DText s, int[] ep){
			int length = ep.Length;
			List<int> wordep = new List<int>();
			char c ;
			
			int pos;
			int wordL;
			int wordR;
			for (int i = 0 ; i < length ; i += 2)
			{
				pos = ep[i++];
				c = s[pos--];
				while ( !c.isWhiteSpace()  
				&& c != '}')
				{
					// f (c == '}') break; // not sure
					
					c = s[pos--];
				}
				
				
				wordL = pos + 1;
				wordep.Add(pos - 1);
				pos = ep[i++];
				c = s[pos++];
				while (!c.isWhiteSpace()
				&& c != '{')
				{
					
					c = s[pos++];
				}
				
				
				wordR = pos - 1;
				
				
				// maybe the word is entirely on the left
				if (wordR == ep[i - 1]) 
					wordR = ep[i - 2];
				
				// maybe the word is entirely on the right
				if (wordL == ep[i - 2] && wordL != wordR)
					wordL = ep[i - 1];
				
				// if there's no word then the difference resolves to zero meaning 
				wordep.Add(wordL);
				wordep.Add(wordR);
			}
			return wordep.ToArray();
		}
		
		public static void RestorePrefabs(DText s, string[] prefabs, int[] ep_new){
			int length = ep_new.Length;
			int p = 0;
			for (int i = 0 ; i < length ; i += 2)
				// right to left so there's some fidelity
				s.DInsert( ep_new[i], prefabs[p++]);
			
		}
		
		public static void RestorePrefabs(DText s, DText originText, int[] ep, int[] ep_new){
			int length = ep_new.Length;
			
			string prefab;
			for (int i = 0 ; i < length ; i += 2)
			{	
				prefab = originText.DToString( ep[i], ep[i + 1] - ep[i ]);
				// right to left so there's some fidelity
				s.DInsert(ep_new[i], prefab);
			}
			
		}
		public static int[] GetPrefabEndpoints(DText unaltered, int discardPos = 0, DText filteredText = null, bool individual = false){
			stopwatch.Start();
			List<int> li = new List<int>();
			char c;
			bool f = (filteredText != null);
			while (discardPos < unaltered.Length)
			{
			
				c = unaltered[discardPos++];
				while  (c != '{' && discardPos < unaltered.Length)
				{
					if (f) filteredText.DAppend(c);
					c = unaltered[discardPos++];
					
				}
				
				li.Add(discardPos - 1); // start
				
				discardPos = FindPrefabEnd(unaltered, discardPos, individual);
				li.Add(discardPos - 1); // endpoint
				
			}
				
			stopwatch.Stop();
			return li.ToArray();
		}
			
		
		public static int FindPrefabEnd(DText s, int discardPos, bool individual = false){
			
			DParser.stopwatch.Start();
			
			char c = '{';
			int pairs = 0;
			
			void pairloop(){
				// need a pre-enter setup
				pairs = 1;
				while (pairs > 0 && discardPos < s.Length)
				{
					c = s[discardPos++];
					if (c == '}') pairs --;
					else if (c == '{') pairs ++;
					
				}
				if ( discardPos < s.Length)	c = s[discardPos++];
			}
			
			if (individual) pairloop();
			else while (c == '{') 
			{
				pairloop();
			}
		
			
			DParser.stopwatch.Stop();
			return discardPos;
		}
		
		
		
		
		public IEnumerator<float> DiscardPrefabs()
		{
			if (procs == null) yield break;
			stopwatch.Start();
			
			char c;
			
			
			// c != s discardPos on entry, do not assign c
		
		
			
		
			while (discardPos < procs.Length)
			{
			
				c = procs[discardPos++];
				while  (c != '{' && discardPos < procs.Length)
				{
					filteredText.DAppend(c);
					c = procs[discardPos++];
						
						
					if (discardPos - dcPos > filterAllowance || stopwatch.ElapsedMilliseconds > maxTime) 
					{
						stopwatch.Stop();
						dcPos = discardPos;
						yield return Timing.WaitForOneFrame;
						
						if (procs == null) yield break;
					}
					
				}
				
				
				int pairs = 0;
				
				while (c == '{') 
				{
					// need a pre-enter setup
					pairs = 1;
					while (pairs > 0 && discardPos < procs.Length)
					{
						c = procs[discardPos++];
						if (c == '}') pairs --;
						else if (c == '{') pairs ++;
						
						
						
						if (discardPos - dcPos > filterAllowance || stopwatch.ElapsedMilliseconds > maxTime) 
						{
							stopwatch.Stop();
							dcPos = discardPos;
							yield return Timing.WaitForOneFrame;
							
							if (procs == null) yield break;
						}
					}

				}
				
			}
				
			stopwatch.Stop();

		}
	
	}
}	