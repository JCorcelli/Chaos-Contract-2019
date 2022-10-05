
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

using Utility.GUI;
using SelectionSystem;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;


namespace DialogueSystem
{


	public partial class DSource {
		// origin
		// this is probably something I generate with an file

		public DSourceParser parser;
		public TextAsset _textAsset;
		public TextAsset textAsset
		{
		 	get{ return _textAsset;}
			set{
				_textAsset = value; 
				if (value != null) text = value.ToString();
			}
		}
		
		protected string _text = "";
		public static bool validate = true;
		public string text {
		 	get{ return _text;}
			set{
				string nv = value.Replace("\r", string.Empty);
				_text = nv ;
				
				if (validate) ValidSource(nv, ref _text); 
			}
		}
		public static List<DSource> all = new List<DSource>(); // character info
		public DUser creator; // character info
		public DStorage origin; // basic info
		//public string creationDate
		
		
		public DSource(){
			all.Add(this);
		}
		public void OnDestroy()
		{
			all.Remove(this);
		} 
		
		
		public bool qualified = false;
		public bool isSpawned = false;
		public WeakReference<DText> body ;
		
		public DText Spawn(){
			AddAllActions(); // new local action
			// origin
			// creator
			DText dt = null; 
			qualified = true;
			if (body == null) isSpawned = false; // i have been forgotten?
			for (int i = 0 ; i < all.Count ; i++)
			{
				if (all[i] != this && all[i].textAsset == this.textAsset)
				{
					qualified = all[i].qualified;
					if (!qualified) break;
					
					all[i].body.TryGetTarget(out dt);
					if (dt != null)
					{	
						isSpawned = true;
						dt = dt.Clone();
						dt.isSource = true;
						body = new WeakReference<DText>(dt);// i have not been forgotten
						break;
					}
				}
			}
			
			//qualified for parser?
			
			// remember to declare parser first
			
			DText newText = new DText();
			
			newText.DAppend(text); // sets tstream
			newText.sourceText = this;
			
			newText.isSource = true;
			
			if (!qualified) return newText;
			
			parser = new DSourceParser();
			parser.localAction = localAction;
			parser.useGlobal = false;
			
			parser.streamedText = newText;
			
			if (dt == null)
			{
				qualified = false;
				parser.Qualify();
				
			}
			
			// this creates a lopped off version
			
			if (qualified)
			{
				if (!isSpawned)
				{
					
					
					parser.ImmediatePrefabs();
					
					dt = newText.Clone(parser.streamedText.storedText.ToString(parser.charPos, (int)parser.streamedText.Length - parser.charPos));
					body = new WeakReference<DText>(dt );
				}
				parser.useGlobal = true;
				parser.Load(); // charpos = 0, running = true

				parser.CoPrefabs(); // it can run slow, fast whatever 
				parser = null;// and it will die when it stops
			}
			else dt = newText;
			
			dt.isSource = true;
			isSpawned = true;
			
			return dt; // dt is the body
		}
		
		public string pattern = @"{.*?}";
		public string subStringOut = "";
		public static int totalErrors = 0;
		public static int errors = 0;
		
		public string TrimBraces(string s){
			s = s.Trim();
			s = s.Substring(1, s.Length - 2);
			return s;
		}

		protected bool ValidSource (string s, ref string _text, bool isSubstring = false) {
			
			// so is this where the magic happens?
			if (s.Length < 2) return true;
			if (!isSubstring) errors = 0;
			int pairs = 0;
			int pos =0;
			
			int startPos = 0;
			
			int extraPos = -1;
			int endPos = 0;
			
			
			string mark;
			char c;
			List<string> prefabs = new List<string>();
			List<int> prefabPos = new List<int>();
			
			
			while (pos < s.Length && pos >= 0)
			{
				c = s[pos];
				
				// count brace pairs
				if (c == '{') 
				{
					if (pairs == 0) 
					{
						startPos = pos;
						extraPos = -1 ;
					}
					pairs ++;
				}
				else if (c == '}') 
				{
					endPos = pos;
					pairs --;
					if (pairs == 0) 
					{
						// 0 sum creating a prefab
						
						mark = s.Substring(startPos,pos - startPos+ 1);
						
						prefabs.Add(mark);
						prefabPos.Add(pos);
						
					}
					else if (pairs < 0) break;
					else if (extraPos < 0)
						extraPos = pos;
				}
				pos ++;
			}
			
			// bracket pair failure means there's syntax error
			if (!isSubstring && pairs != 0) 
			{
				int row = 0;
				int cutPos = 0;
				string n;
				errors ++;
				totalErrors ++;
				if (pairs < 0) 
				{
					n = s.Substring(0, endPos);
					
					for (int i = 0; i < n.Length; i++) 
					{
						c = n[i];
						if (c == '\n') row++;
					}
					
					cutPos = (endPos + 5 < s.Length ? 5 : 1);
					row++;
					if (cutPos > 0)
						Debug.Log("<size=18><color=green>Syntax Error@" + row + "Malformed brace pair ..." + s.Substring( endPos , cutPos).Replace("\n","\\n") + "...</color></size>");
					else
						Debug.Log("<size=18><color=green>Syntax Error@" + row + "Malformed brace pair ..."  + s + "...</color></size>");
					
					pos = 0;
				
					while (pos < cutPos)
					{
						n += '@';
						pos ++;
					}
						
					_text = n + s.Substring(n.Length, s.	Length - n.Length).Replace('}','>').Replace('{','<');
						
				}
				else 
				{
					n = s.Substring(0, startPos);
					for (int i = 0; i < n.Length; i++) 
					{
						c = n[i];
						if (c == '\n') row++;
					}
							
					cutPos = (extraPos + 1 - startPos < s.Length ? extraPos + 1 - startPos : extraPos - startPos);
					row++;
					if (cutPos > 0)
						Debug.Log("<size=18><color=green>Syntax Error@" + row + "Malformed brace pair ..." + s.Substring(startPos, cutPos).Replace("\n","\\n") + "...</color></size>");
					else
						Debug.Log("<size=18><color=green>Syntax Error@" + row + "Malformed brace pair ..." + s + "...</color></size>");
						
					pos = 0;
				
					while (pos < cutPos)
					{
						n += '@';
						pos ++;
					}
						
					_text = n + s.Substring(n.Length, s.Length - n.Length).Replace('}','>').Replace('{','<');

									
				}
				return false;
				
			}
			// give up if nothing's there
			if (prefabs.Count < 1) return true;
			
			
			
			string p;
			int cpos;
			// here I treat it like it's a method call
			for( int i = 0; i < prefabs.Count; i ++)
			{
				p = prefabs[i]; // full name
				cpos = prefabPos[i];
				mark = TrimBraces(p); // cut for source
				
				if (!ValidSource( mark, ref _text, true) && !isSubstring)
				{
					
					int row = 0;
					
					// i already replaced it in _text
					for (int x = 0; x < cpos; x++) 
					{
						c = s[x];
						if (c == '\n') row++;
					}
					row ++;
					
					Debug.Log("<color=green><size=23>NestedError@"+ row+" {"+ SplitFirstPrefab(p)[0] + ".. "+ subStringOut + "} </size></color>");
					subStringOut = "";
					
					
				}
			}
			
			
			// check the prefab actions
			
			for (int i = 0; i < prefabs.Count; i++)
				
			{
				p = prefabs[i];
				cpos = prefabPos[i];
				mark = TrimBraces(p);
				
				// here I treat it like it's a file, but I don't perform the same row or replace operations like before
				
				bool v = DAction.ValidAction(mark);				
				if (!v && !isSubstring)					
				{
					int row = 0;
					
					for (int x = 0; x < cpos; x++) 
					{
						c = _text[x];
						if (c == '\n') row++;
					}
					row ++;
					
					Debug.Log("<color=green><size=23>Nope@"+ row+ " "+ p +" isn't a thing.</size></color>");
				
					_text = ReplaceFirst(_text, p, "<"+mark+">"); // future place to add an error action
					
					
					totalErrors ++;
					errors ++;
				}	
				else if (!v)
				{
					totalErrors ++;
					errors ++;
					_text = ReplaceFirst(_text, p, "<"+mark+">"); // future place to add an error action
					subStringOut += p+".. ";
				}
			}
			return (errors < 0);
			
		}
		
		public string[] SplitFirstPrefab(string s, int po = 0)
		{
			if (s.Length < 2) return new string[]{"",""};
			int pos = s.IndexOf("{");
			if (pos < 0)
			{
				return new string[]{"",""};
			}
			return SplitFirst(s.Substring(pos + 1), " ");
		}
		
		public string[] SplitFirst(string text, string search = " ")
		{
			// it's for splitting string prefab param[param param]
		  int pos = text.IndexOf(search);
		  if (pos < 0)
		  {
			return new string[]{text, ""};
		  }
		  return new string[]{text.Substring(0, pos), text.Substring(pos)};
		}
		public string ReplaceFirst(string text, string search, string replace)
		{
		  int pos = text.IndexOf(search);
		  if (pos < 0)
		  {
			return text;
		  }
		  return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
		}



	}
	
}