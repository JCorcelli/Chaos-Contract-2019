using UnityEngine;


using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using System.Text;



namespace DialogueSystem
{

	
	
	public delegate void DFilterDelegate(DText text);
	
	public class DFilter {
		/*
			filters alter the sounds and sensations of the moment, maybe they should be called moment filters
			
			potentially a mutator, and there's no hard limit to the code that goes into each filter
			
			updates to respond to immediate state
		*/
		
		
		public static Dictionary<string,DFilter> allTypes = new Dictionary<string,DFilter>()
		{
			["Default"]  = new DFilter(),
			["Video.in"] = new DVideoFilterIn(),
			["Audio.in"] = new DAudioFilterIn(),
			["Chemical.in"] = new DChemicalFilterIn(),
			["Touch.in"] = new DTouchFilterIn(),
			["Magic.in"] = new DMagicFilterIn(),
			["Psychic.in"] = new DPsychicFilterIn(),
			["Filter.out"] = new DFilterOutput(),
			["TextFilter.exe"] = new DTextFilter(),
			["WildcardReplace.exe"] = new DWildcardReplace(),
			["AllWordReplace.exe"] = new DAllWordReplace(),
			["UnknownWordReplace.exe"] = new DUnknownWordReplace()
			              
		};                     
		                        
		public static bool captured = true;
		public static bool dead = false;
		
		public string type = "Filter";
		public DFilterDelegate updateRequest; 
		public float lastUpdate = 0f;
		
		protected int[] shiftedPrefabs;
		protected int[] prefabOrigins;
		protected DText originText;
		
		
		protected string[] _prefabs;
		
		protected string[] prefabs{
			// this is loosely concatenated prefabs, split by any separation at all
			
			get{
				if (_prefabs != null) return _prefabs;
				_prefabs = GetJoinedPrefabs();
				
				return _prefabs;
			}
			set{_prefabs = value;}
				
			
		}
		
		
		public static DFilter New(string s){
			if (allTypes.ContainsKey(s))
				return allTypes[s].Clone();
			
			return allTypes["Default"].Clone();
		}
		public virtual DFilter Clone(){ 
			// so, be sure to replace this
			DFilter dm = Activator.CreateInstance(
			this.GetType()) as DFilter;
			dm.type = this.type;
			
			return dm;
		}
		
		protected string[] GetJoinedPrefabs(){
			
				
			if (originText == null) return null ;
			
			prefabOrigins = DParser.GetPrefabEndpoints(originText, individual : true);
			return DParser.ListPrefabsEP(originText, prefabOrigins);
			
		}
		protected string[] GetSplitPrefabs(){
			// this is split by any separation at all
			
				
			if (originText == null) return null ;
			
			prefabOrigins = DParser.GetPrefabEndpoints(originText, individual : true);
			return DParser.ListPrefabsEP(originText, prefabOrigins);
			
		}
		public virtual void Flush()
		{
			shiftedPrefabs = prefabOrigins = null;
			originText = null;
			prefabs = null;
		}
		public virtual void Interpolate(DText dtext, float delta)
		{
			if (updateRequest != null && Time.time - lastUpdate > 0)
				Update(dtext);
			dead = captured = false;
			
			originText = dtext;
			Step(delta);
			
			Filter(dtext);
			Flush();
		}
		
		public void ShiftPrefabs(int pos, int amount){
			
			DParser.ShiftPrefabEndpoints(shiftedPrefabs, //int[]
			pos, //int 
			amount); // +/- int 
		}
		public virtual void Update(DText dtext){
			if (updateRequest != null) updateRequest(dtext);
			lastUpdate = Time.time;
		}
		public virtual void Filter(DText input)  {}
		
		public virtual void Extinguish()   {}
		public virtual void Step(float delta)   {}

	}
	
	// streamparser
	public class DVideoFilterIn : DFilter{
		
		
		
		public DVideoFilterIn(){ type = "Video.in";
		
		}
		// "" Frequently updated
		public Vector3 location  ;
		public Vector3 facing    ;
		public Vector3 motion    ;
		public Vector3 focalPoint ; // if this is not null things get blurrier
		public float relativeSpeed = 0f;
		
		// if the target has information, it needs to face us

		public Vector3 targetLocation  ;
		public Vector3 targetFacing ;
		public Vector3 targetMotion ;
		
		public float targetWindowMin = .25f ;
		public float targetWindowMax = 1f   ;
		// ""###############
		
		public float viewMin = .25f ;
		public float viewMax = 1f   ;
		
		// peripheral, sees things that are moving
		public float peripheral =0.5f;
		public float iperipheral =0f ;
		public float pMin = -.05f    ;
		public float pMax = 1f       ;
		
		
		// vision clarity
		public float falloffMin = 0.01f;
		public float falloffMax = 4400f; // meters, 5' person with perfect vision
		
		
		public float focusMargin = 1f;
		// vision acuity, and peripheral

		public override void Extinguish() {

			iperipheral =  0;

		}

		public override void Step(float delta)
		{
			// delta should be cps * deltaTime

			iperipheral += peripheral *delta;


		}
		public override void Filter(DText dtext)  {

			DFilter.captured = false;
			if (focalPoint != null)
			{	
				if ( Vector3.Distance(targetLocation, focalPoint) < focusMargin)
					DFilter.captured = true;
					return ;
			}

			Vector3 dir = targetLocation - location;
			float fac = Vector3.Dot(facing, dir.normalized);
			if (fac > viewMin & fac < viewMax)
			{
					DFilter.captured = true;
				
				
			}
			Vector3 otherdir =  location - targetLocation;
			float otherfac = Vector3.Dot(targetFacing, otherdir.normalized);
			if (otherfac > targetWindowMin & otherfac < targetWindowMax)
			{
				// captured more info?
				
				
			}

			if (iperipheral >= 1 )
			{

				iperipheral--;
				if(fac > pMin && fac < pMax && relativeSpeed > 0.001f)
					DFilter.captured = true;
				

			}
			
		}
	}

	public class DAudioFilterIn : DFilter {
		
		
		
		public DAudioFilterIn(){type = "Audio.in";}
		// should probably take an array and normalize the volume
		public float volume = -1f;
		public float involume = 1f;

		public override void Step(float delta)  {


		}
		public override void Filter(DText text)  {
			if (involume > volume) 
				DFilter.captured = true;
			else
				DFilter.captured = false;


		}
	}
	
	public class DChemicalFilterIn : DFilter {
		
		public DChemicalFilterIn(){type = "Chemical.in";}
		// ok so this time, a callback in the text can be updating the chemical mask and this parses the info
		
		public int mask = Convert.ToInt32("00111",2);
		public int chem = 0;
		public float captureRate = 0f; // out
		public float spillRate = 0f; // out, this could be a negative impact, eg a failure to contain
		
		public int Intersection( ){
			return (mask & chem);
		}
		public int Spills(   ){
			return (~mask & chem);
		}
		
		public override void Step(float delta)  {


		}
		
		
		public override void Filter(DText t)  {
			int hits = Intersection ();
			string binHits = Convert.ToString(hits, 2);
			int spills = Spills     ();
			string binSpills = Convert.ToString(spills, 2);
			
			int total = Convert.ToString(chem, 2).Count('1');
			
			hits = binHits.Count('1');
			spills = binSpills.Count('1');
			
			captureRate = (float)hits / total;
			spillRate = (float)spills / total;
			if (captureRate >= UnityEngine.Random.value) 
				DFilter.captured = true;
			else
				DFilter.captured = false;


		}
	}
	
	public class DTouchFilterIn : DFilter {
		
		public DTouchFilterIn(){type = "Touch.in";}
		// there may be some information about a touched object
		
		public float sensitivity = 0f;
		public float pressure = 0f;

		public override void Step(float delta)  {


		}
		public override void Filter(DText text)  {
			if (pressure > sensitivity) 
				DFilter.captured = true;
			else
				DFilter.captured = false;

		}
	}
	
	
	public class DMagicFilterIn : DFilter {
		
		public DMagicFilterIn(){type = "Magic.in";}
		// this time I should be given an unfiltered text
		// hypothetical of course
		
		public struct MagicDefense
		{
			public string name;
			public float skill;
		}
		public List<MagicDefense> magic = new  List<MagicDefense>(){
			new MagicDefense{name="Fire",		skill=0.05f},
			new MagicDefense{name="Missile",	skill=0.05f},
			new MagicDefense{name="Uncanny",	skill=0.05f}};
		
		
		public override void Step(float delta)  {


		}
		public override void Filter(DText t)  {
			
			DFilter.captured = false;
			
			string s = t.storedText.ToString();
			for (int i = 0 ; i < magic.Count ; i++)
			{
				if (s.Contains(magic[i].name) && UnityEngine.Random.value < magic[i].skill)
				{
					t.Clear();
					t.DAppend("Your shield against "+magic[i]+" magic activates. So it becomes harmless.");
					DFilter.captured = true;
					break;
				}
			}
			
			


		}
	}
	
	public class DPsychicFilterIn : DFilter {
		
		public DPsychicFilterIn(){type = "Psychic.in";}
		// either read someone's thoughts or they read your thoughts
		float ipsychic = 0f;
		float psychic = .1f;
		
		public override void Step(float delta)  {
			ipsychic += psychic * delta;

		}
		public override void Filter(DText t)  {
			
			DFilter.captured = false;
			if (ipsychic >= 1)
			{
				DFilter.captured = true;
				ipsychic --;
			}

		}
	}
	




	// I guess one generic output fits all
	// redparser, mediaparser
	public class DFilterOutput : DFilter {
		
		public DFilterOutput(){type = "Filter.out";}
		
		public string interference = "";
		public string insert = "";
		public string prefixStream = ""; 
		public string stream = ""; 
		public string interrupt = ""; // remove the rest of the sentence
		
		
		public override void Extinguish() {


		}

		public override void Step(float delta)
		{
			


		}
		public override void Filter(DText dtext)  {

			DFilter.captured = true;
			
			if (insert != "") dtext.parent.DInsert( dtext.parentIndex + (int)dtext.Length,insert); 
			
			if (interrupt  != "") {
				string s = dtext.parent.storedText.ToString();
				int i = s.IndexOf('.', dtext.parentIndex);
				if (i < 0) i =  dtext.storedText.Length - 1;
				
				if (i > dtext.parentIndex + (int)dtext.Length)
					dtext.parent.DRemove(dtext.parentIndex, i - dtext.parentIndex);
				
				dtext.parent.DInsert( dtext.parentIndex + (int)dtext.Length,interrupt); 
				DFilter.captured = false;
				interference = insert =  stream = interrupt = "";
				return;
			}
			
			if (interference != "") 
			{
				dtext.EmptyStream();
				dtext.DAppend(interference);
			}
			
			if (prefixStream != "") dtext.DInsert(0, prefixStream); 
			if (stream != "") dtext.DAppend(stream); 
			
			interference = insert =  stream = interrupt = "";
		}
	}

	
	// processor
	
	// I guess this is a generic filter
	public class DTextFilter : DFilter {
		
		public DTextFilter(){type = "TextFilter.exe";}

		public string user = "";
		public string media = "";

		public string mutator = "";
		public DText text  ;   // could also add quotes

		public DStorage storage ;


		public override void Filter(DText dtext)
		{

			DFilter.captured = false;
			if (user == dtext.currentUser.name
			|| dtext.media == media 
			|| dtext.mutator.mutatorType == mutator 
			|| dtext.storage == storage )
			{
				DFilter.captured = true;
			}



		}
		public override void Extinguish()
		{


		}
		public override void Step(float delta)
		{
			// delta should be cps * deltaTime


		}

	}


	/*
		Random reconstruction
		This gets a list of known words and wildcards and replaces the broken words with real ones of different length and spelling
		
	*/
	public class DWildcardReplace : DFilter {
		
		
		
		public DWildcardReplace(){type = ".exe";}
		// the idea is to auto-replace new words with known words
		public List<string> wordReference;
		public List<string> wordMatches = new List<string>();
		public int lengthThreshold = 3;
		public int missThreshold = 5;
		public string wildcards = @"*@";

		public override void Filter(DText dtext)
		{
			DText s  = dtext;
			

			int len = wildcards.Length;
			int charPos = 0;
			int wordPos = 0;
			int wordEnd = 0;
			
			// iterates wildcards
			
			
			bool isWildCard(char str)
			{
				
				return wildcards.Contains(""+str);
			}
			
			
			
			
			char c;
			
			
			
			StringBuilder sb = new StringBuilder();
			
			List<int> endpoints = new List<int>();
			while (charPos < s.Length) // while not finished
			{
				// charpos at end of loop skips trailing dots, but stops at white space, so all leading dots are captured
				
				
				
				bool hasWildCard = false;
				// the quick parse no regex
				
				while (charPos < s.Length && !hasWildCard)
				{
					endpoints.Clear();
					c = s[charPos++];
					// this will not start with leading punctuation
					while (charPos < s.Length && c.isWhiteSpace())
					{	
						
						c = s[charPos++];
						
						
					}
					wordPos = charPos - 1;
					// this will include punctuation
					while (charPos < s.Length && !c.isWhiteSpace())
					{	
				
						if (c=='{') 
						{	
							endpoints.Add(charPos);
							charPos = DParser.FindPrefabEnd(s, charPos);
							endpoints.Add(charPos);
							continue;
						}
						sb.Append(c);
						if (!hasWildCard) hasWildCard = isWildCard(c);
						c = s[charPos++];
						
							
					}
				}
				if (!hasWildCard ) break;
				
				int trim = 0;
				int ending = sb.Length - 1;
				// this will trim leading punctuation
				if (sb[ending--].isPunctuation()) 
				{
					while (ending >= 0 && sb[ending].isPunctuation())
						ending--;
					
					trim = sb.Length - ending;
					
					sb.Remove(ending, trim);
				}
				
				if (sb.Length == 0) continue; // right here I discard any hyphenated {}...
				
				prefabOrigins = endpoints.ToArray();
				endpoints.Clear();
				wordEnd = charPos - trim - 1;
				// find close matching words and replace only the wildcards
				string oldWord = sb.ToString();
				string newWord = "";
				
				int s1len = sb.Length;
				int missmatches( )
				{
					// count misses, exclude wildcards
					int s2len = newWord.Length;
					int shortest = (s1len < s2len) ? s1len : s2len;
					int miss = 0;
					
					for (int i = 0 ; i < shortest ; i++)
					{
						if (oldWord[i] == newWord[i]
						|| isWildCard(oldWord[i]) 
						|| isWildCard(newWord[i]))
							continue;
						
						miss++;
					}
					return miss;
				}
		
				Func<bool> algorithm = () => (Mathf.Abs((wordEnd - wordPos) - newWord.Length) < lengthThreshold && missmatches() < missThreshold);
				
				Func<int> getrandom = () => (UnityEngine.Random.Range (0, wordMatches.Count));
				
				
				for (int ix = 0 ; ix < wordReference.Count ; ix++)
				{
					newWord = wordReference[ix];
					if(algorithm()) wordMatches.Add(newWord);
					
					//calculate newWord = wordMatches
				}
			
				newWord = "";
				if (wordMatches.Count > 0) 
				{
					newWord = wordMatches[getrandom()];
					int shift = newWord.Length - sb.Length;
					
					shiftedPrefabs = (int[])prefabOrigins.Clone();
					
					int wordLen = wordEnd - wordPos;
					DParser.ShiftPrefabInWord(shiftedPrefabs, wordPos, shift, wordPos, wordLen);
					
					prefabs = DParser.ListPrefabsEP(s, prefabOrigins);
					s.DRemove(wordPos, wordLen);
					
					s.DInsert(wordPos, newWord);
					DParser.RestorePrefabs(s, prefabs, shiftedPrefabs);
					charPos = wordPos + newWord.Length ; // push charpos to edge of word, possibly the end of file
					wordMatches.Clear();
					
				}
				else // no replace
					charPos = wordEnd + 1;
					
				while (charPos < s.Length && s[charPos].isPunctuation())
					charPos++;
					
			}
			
		

		}
		public override void Extinguish()
		{


		}
		public override void Step(float delta)
		{
			// delta should be cps * deltaTime


		}

	}




	/*
		Randomization
		This gets a list of known words and wildcards and replaces the broken words with real ones of different length and spelling
		
	*/
	public class DAllWordReplace : DFilter {
		
		
		public DAllWordReplace(){type = ".exe";}
		// the idea is to auto-replace new words with known words
		public List<string> wordReference;
		public List<string> wordMatches = new List<string>();
		public int lengthThreshold = 3;
		public int missThreshold = 5;
		

		public override void Filter(DText dtext)
		{
			DText s  = dtext;
			
			

			int charPos = 0;
			int wordPos = 0;
			int wordEnd = 0;
			
			// iterates wildcards
			
			
			
			
			
			
			char c;
			
			
			
			StringBuilder sb = new StringBuilder();
			
			List<int> endpoints = new List<int>();
			while (charPos < s.Length) // while not finished
			{
				// charpos at end of loop skips trailing dots, but stops at white space, so all leading dots are captured
				
				
			
				endpoints.Clear();
				c = s[charPos++];
				// this will not start with leading punctuation
				while (charPos < s.Length && c.isWhiteSpace())
				{	
					
					c = s[charPos++];
					
					
				}
				wordPos = charPos - 1;
				// this will include punctuation
				while (charPos < s.Length && !c.isWhiteSpace())
				{	
			
					if (c=='{') 
					{	
						endpoints.Add(charPos);
						charPos = DParser.FindPrefabEnd(s, charPos);
						endpoints.Add(charPos);
						continue;
					}
					sb.Append(c);

					c = s[charPos++];
					
						
				}
			
				if (sb.Length == 0) continue;
				
				
				int trim = 0;
				int ending = sb.Length - 1;
				// this will trim leading punctuation
				if (sb[ending--].isPunctuation()) 
				{
					while (ending >= 0 && sb[ending].isPunctuation())
						ending--;
					
					trim = sb.Length - ending;
					
					sb.Remove(ending, trim);
					
					if (sb.Length == 0) continue;
				}
				
				prefabOrigins = endpoints.ToArray();
				endpoints.Clear();
				wordEnd = charPos - trim - 1;
				// find close matching words and replace them
				string oldWord = sb.ToString();
				string newWord = "";
				
				int s1len = sb.Length;
				int missmatches( )
				{
					// count misses
					int s2len = newWord.Length;
					int shortest = (s1len < s2len) ? s1len : s2len;
					int miss = 0;
					
					for (int i = 0 ; i < shortest ; i++)
					{
						if (oldWord[i] == newWord[i])
							continue;
						
						miss++;
					}
					return miss;
				}
		
				Func<bool> algorithm = () => (Mathf.Abs((wordEnd - wordPos) - newWord.Length) < lengthThreshold && missmatches() < missThreshold);
				
				Func<int> getrandom = () => (UnityEngine.Random.Range (0, wordMatches.Count));
				
				
				for (int ix = 0 ; ix < wordReference.Count ; ix++)
				{
					newWord = wordReference[ix];
					if(algorithm()) wordMatches.Add(newWord);
					
					//calculate newWord = wordMatches
				}
			
				newWord = "";
				if (wordMatches.Count > 0) 
				{
					newWord = wordMatches[getrandom()];
					int shift = newWord.Length - sb.Length;
					
					shiftedPrefabs = (int[])prefabOrigins.Clone();
					
					int wordLen = wordEnd - wordPos;
					DParser.ShiftPrefabInWord(shiftedPrefabs, wordPos, shift, wordPos, wordLen);
					
					prefabs = DParser.ListPrefabsEP(s, prefabOrigins);
					s.DRemove(wordPos, wordLen);
					
					s.DInsert(wordPos, newWord);
					DParser.RestorePrefabs(s, prefabs, shiftedPrefabs);
					charPos = wordPos + newWord.Length ; // push charpos to edge of word, possibly the end of file
					wordMatches.Clear();
				}
				else // no replace
					charPos = wordEnd + 1;
					
				while (charPos < s.Length && s[charPos].isPunctuation())
					charPos++;
					
			}
			
		

		}
		public override void Extinguish()
		{


		}
		public override void Step(float delta)
		{
			// delta should be cps * deltaTime


		}

	}




	/*
		Randomization
		This gets a list of known words and wildcards and replaces the broken words with real ones of different length and spelling
		
	*/
	public class DUnknownWordReplace : DFilter {
		
		public DUnknownWordReplace(){type = ".exe";}
		// the idea is to auto-replace new words with known words
		public List<string> wordReference;
		public List<string> wordMatches = new List<string>();
		public int lengthThreshold = 3;
		public int missThreshold = 5;
		

		public override void Filter(DText dtext)
		{
			DText s  = dtext;
			


			int charPos = 0;
			int wordPos = 0;
			int wordEnd = 0;
			
			// iterates wildcards
			
			
			
			
			
			
			char c;
			
			
			
			StringBuilder sb = new StringBuilder();
			
			List<int> endpoints = new List<int>();
			while (charPos < s.Length) // while not finished
			{
				// charpos at end of loop skips trailing dots, but stops at white space, so all leading dots are captured
				
				// the quick parse no regex
			
				bool knownWord = true;
				while (charPos < s.Length && knownWord)
				{					
					endpoints.Clear();
					c = s[charPos++];	
					// this will not start with leading punctuation
					while (charPos < s.Length && c.isWhiteSpace())
					{	
						
						c = s[charPos++];
						
						
					}
					wordPos = charPos - 1;
					// this will include punctuation
					while (charPos < s.Length && !c.isWhiteSpace())
					{	
				
						if (c=='{') 
						{	
							endpoints.Add(charPos);
							charPos = DParser.FindPrefabEnd(s, charPos);
							endpoints.Add(charPos);
							continue;
						}
						sb.Append(c);

						c = s[charPos++];
						
							
					}
					if (sb.Length == 0) continue;
					knownWord = wordReference.Contains(sb.ToString());
				}
				
				
				int trim = 0;
				int ending = sb.Length - 1;
				// this will trim leading punctuation
				if (sb[ending--].isPunctuation()) 
				{
					while (ending >= 0 && sb[ending].isPunctuation())
						ending--;
					
					trim = sb.Length - ending;
					
					sb.Remove(ending, trim);
					
				}
				if (sb.Length == 0) continue;
				
				prefabOrigins = endpoints.ToArray();
				endpoints.Clear();
				wordEnd = charPos - trim - 1;
				// find close matching words and replace them
				string oldWord = sb.ToString();
				string newWord = "";
				
				int s1len = sb.Length;
				int missmatches( )
				{
					// count misses
					int s2len = newWord.Length;
					int shortest = (s1len < s2len) ? s1len : s2len;
					int miss = 0;
					
					for (int i = 0 ; i < shortest ; i++)
					{
						if (oldWord[i] == newWord[i])
							continue;
						
						miss++;
					}
					return miss;
				}
		
				Func<bool> algorithm = () => (Mathf.Abs((wordEnd - wordPos) - newWord.Length) < lengthThreshold && missmatches() < missThreshold);
				
				Func<int> getrandom = () => (UnityEngine.Random.Range (0, wordMatches.Count));
				
				
				for (int ix = 0 ; ix < wordReference.Count ; ix++)
				{
					newWord = wordReference[ix];
					if(algorithm()) wordMatches.Add(newWord);
					
					//calculate newWord = wordMatches
				}
			
				newWord = "";
				if (wordMatches.Count > 0) 
				{
					newWord = wordMatches[getrandom()];
					int shift = newWord.Length - sb.Length;
					
					shiftedPrefabs = (int[])prefabOrigins.Clone();
					
					int wordLen = wordEnd - wordPos;
					DParser.ShiftPrefabInWord(shiftedPrefabs, wordPos, shift, wordPos, wordLen);
					
					prefabs = DParser.ListPrefabsEP(s, prefabOrigins);
					s.DRemove(wordPos, wordLen);
					
					s.DInsert(wordPos, newWord);
					DParser.RestorePrefabs(s, prefabs, shiftedPrefabs);
					charPos = wordPos + newWord.Length ; // push charpos to edge of word, possibly the end of file
					wordMatches.Clear();
					
				}
				else // no replace
					charPos = wordEnd + 1;
					
				while (charPos < s.Length && s[charPos].isPunctuation())
					charPos++;
					
			}
			
		

		}
		public override void Extinguish()
		{


		}
		public override void Step(float delta)
		{
			// delta should be cps * deltaTime


		}

	}






}