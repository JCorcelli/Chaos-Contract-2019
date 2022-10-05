
using UnityEngine;
using UnityEngine.Assertions;

using UnityEngine.UI;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using MEC;

using Utility.GUI;
using SelectionSystem;
using System.Text;
using static GuiGame.GuiGameVars;

namespace GuiGame
{


	
	public class LinkHandler {
		/*
			Handles link notation

		Load(GuiDict)
		HandleLink(GuiDict root, string key, bool toLocal = false)
		
		examples
			A[0]<<B
			
			Goes to index 0 of A, then travels down the tree of that dictionary to Fetch B
			
			GLOBAL>F
			Finds the first F and returns it
			
			GLOBAL>>F
			Finds every F and returns a list
			
			GLOBAL[**F]
			Finds everything contining the letter F
		*/
		
		
		public delegate void ParserDelegate();
		
		// text that's being appended
		public BText procText = new BText(); 
		// status variable
		public BText commentText = new BText();
		// placeholder, never used by BParser the current position is charPos
		
		public BText word = new BText(); 
		// second, long operations
		public BText definition = new BText(); 
		
		
		// for single operations, third
		public BText braceText = new BText();
		
		
		
		public int charPos = 0;
		public int linePos = 0;
		public int lineCount = 1;
		
		
		public char c = ' ';
		
		public GuiDict fromDirectory;
		public GuiDict staticDictionary ;
		
		public GuiDict globalVars;
		
		public string target = "";
		
		protected virtual BText s { 
			
			get => procText;
			
		}
		
		
		public virtual void Load(GuiDict g)
		{
			globalVars = g.Root;
			
			globalVars.Add(STATIC);
			staticDictionary = globalVars[STATIC];
			staticDictionary.Instance();
			
		}
		
			
		
		
		public void NextEndParenthesis(){
			braceText.Clear();
			
			int pairs = 1;
			braceText.Append(c);
			while (charPos < s.Length && pairs > 0 )
			{
				c = s[charPos++];
				if (c == ')') pairs --;
				else if (c == '(') pairs ++;
				
				braceText.Append(c);
				
				CheckLineBreaks();
			}
			
			c = s[charPos++];
			
			
		}
		
		public int FindMatchingEndBrace(){
			int pairs = 1;
			int pos = charPos;
			char c ;
			while (pos < s.Length && pairs > 0 )
			{
				c = s[pos++];
				if (c == '}') pairs --;
				else if (c == '{') pairs ++;
				
			}
			
			return pos;
			
		}
		public void NextActionBrace(){
			// perserve #
			NextEndBrace(false);
			
		}
		
		protected char lineChar = ' ';
		public void NextEndBrace(bool skipComment = true){
			// I guess this will be treated specially, so new lines are preserved
			braceText.Clear();
			
			int pairs = 1;
			
			braceText.Append(c);
			
			if (lineChar == ' ' && c.isNewLine()) {lineChar = c;lineCount++;}
			while (charPos < s.Length && pairs > 0 )
			{
				c = s[charPos++];
				if (c == '}') pairs --;
				else if (c == '{') pairs ++;
				
				braceText.Append(c);
				
				if (skipComment) SkipComment();
				if (lineChar == ' ' && c.isNewLine()) {lineChar = c;lineCount++;}
				else if (c == lineChar) lineCount++;
				
			}
			c = s[charPos++];
			
			
		}
		
		
		protected bool lineBroken = false;
		

		public void TrimWord(BText s = null){
			if (s == null) s = word;
			if (s.Length < 1) return;
			
			int pos = s.Length ;
			while (pos > 0 && s[pos-1].isWhiteSpace() )
				pos --;
			s.Remove(pos, s.Length - pos);
		}
		
		public bool isEndChar(){
			return ",;{}()=+-|^[]".Contains(c);
		}
		public bool isStartChar(){
			return ",".Contains(c); 
		}
		
		public virtual void NextBoundary(){
			CheckLineBreaks();
			while (charPos < s.Length &&  (c.isWhiteSpace()) ) 
			{
				c = s[charPos++];
				CheckLineBreaks();
				
			}
		}
		
		
		public virtual void NextWordBlanks(){
			word.Clear();
			
			recursion++;
			if (recursion > 500)
			{
				LogError("");
				return ;
			}
			
			CheckLineBreaks();
			if (isStartChar() || c == ':')
			{
				c = s[charPos++];
				CheckLineBreaks();
			}
			
			bool inquotes = false;
			if (c == '"') inquotes = true;
			else
			while (charPos < s.Length &&  (c.isWhiteSpace()) ) 
			{
				
				c = s[charPos++];
				CheckLineBreaks();
			}
			if (!inquotes && c == '"') inquotes = true;
			
			
			if (isEndChar() || c == ':') 
				return;
			
			
			while (charPos < s.Length && ( inquotes || !(lineBroken || isEndChar() || c == ':') ) )
			{
				word.Append(c);
				c = s[charPos++];
				// toggles "quoted, comma"
				if (c == '"') inquotes = !inquotes;
				
				NextBoundary();
				if (c == '{') lineBroken = false;
			}
			
			TrimWord();
			
		}
		
		
		//this can only handle very specific operators
		public virtual void NextWord(){
			word.Clear();
			
			recursion++;
			if (recursion > 500)
			{
				LogError("recursion");
				return ;
			}
			
			if (c == ';')
			{
				c = s[charPos++];
				CheckLineBreaks();
			}
			
			bool inquotes = false;
			if (c == '"') inquotes = true;
			else if (c == '=')
			{
				// so, take one step, and possibly the word is blank
				
				c = s[charPos++];
				NextBoundary();
				
			}
			else
			while (charPos < s.Length &&  (c.isWhiteSpace() || isStartChar()) ) 
			{
				
				c = s[charPos++];
				CheckLineBreaks();
				
			}
			
			
			if (!inquotes && c == '"') inquotes = true;
			
			lineBroken = false;
			if (isEndChar()) return;
			
			while (charPos < s.Length && ( inquotes || !(lineBroken || isEndChar()) ) )
			{
				word.Append(c);
				c = s[charPos++];
				// toggles "quoted, comma"
				if (c == '"') inquotes = !inquotes;
				CheckLineBreaks();
				
				
			}
			
			
			if (word.Length > 0)
			{
				
			
				if (! lineBroken && c == '[')
				
					ParseAccessor();
					
				
			
				TrimWord();
				
				if ( word[word.Length - 1] == ':')
				{
					c = ':'; charPos--;
					
					word.Remove(word.Length - 1, 1);
				}
				
			}
			
			
			
		}
		
		
		protected void Warn(string er){
			
			
			Debug.Log("<size=14><color=yellow>Warn: "+ er + " at : " + charPos + " line "+ lineCount + "</color></size>", s.sourceText);
			
			 
		}
		
		protected void LogError(string er){
			
			int cp = (charPos - linePos);
			Debug.LogError("<size=18><color=green>Error: "+ er + " at line "+ lineCount + " pos " + cp + "</color></size>", s.sourceText);
			
			 throw new System.Exception("");
			 
		}
		
		public int recursion = 0;
				
		public void ParseBracketReplacement(){
			braceText.Clear();
			
			c = s[charPos++];
			NextBoundary();
			while (charPos < s.Length && !(c == ']')) 
			{
				braceText.Append(c);
				c = s[charPos++];
			}
			
			TrimWord(braceText);
		}
		public void NextEndBracket(){
			while (charPos < s.Length && !(c == ']')) 
			{
				c = s[charPos++];
				CheckLineBreaks();
			}
		}
		public void InsertBracket(){
			
			if (c == '[')
			while (charPos < s.Length && !(c == ']')) 
			{
				word.Append(c);
				c = s[charPos++];
				CheckLineBreaks();
			}
			word.Append(c);
			c = s[charPos++];
			CheckLineBreaks();
		}
		public void ParseAccessor(){
			//lineBroken = false;
			
			//Debug.Log(word.ToString());
			while (charPos < s.Length && !(lineBroken || ";,=}".Contains(c)))
				InsertBracket();
				
			
			
			
		}
		public void ParseBracketVar(){
			
			//if (",:".Contains(c)) c = s[charPos++];
			word.Clear();
			
			int startPos = charPos;
			bool isVariable = false;
			bool NextEndQuote(){
				if (c!= '"') return false;
				
				c = s[charPos++];
				while (charPos < s.Length && c!= '"')
				{
					c = s[charPos++];
					//CheckLineBreaks();
				}
				
				return true;
			}
			
			void NextVar(){
				while (charPos < s.Length && (c.isWhiteSpace() || c.isOperation() || NextEndQuote()))
				{
					
					c = s[charPos++];
				}
			}
			void StripVar(){
				isVariable = false;
				definition.Clear();
				while (charPos < s.Length && (c.isBracket() || c.isWhiteSpace() || c.isOperation() || NextEndQuote())) 
				{
					
					c = s[charPos++];
				}
				while (charPos < s.Length && !(c.isWhiteSpace() || c.isBracket() || c.isOperation() ))
				{
					if (c.isLetter()) isVariable = true;
					
					definition.Append(c);
					
					c = s[charPos++];
					
				}
				
				
			}
			
			// c = [ or bust
			word.Append(c);
			// left
			StripVar();
			if (isVariable) globalVars.Add( VARIABLE_OBJECT, definition.ToString() );
			NextVar();
			
			if (c != ']')
			{
				
				// right
				StripVar();
				if (isVariable) globalVars.Add( VARIABLE_OBJECT, definition.ToString() );
				NextEndBracket();
			}
			// whole bracket
			
			word.Append( s.ToString(startPos, charPos - startPos));
			
			c = s[charPos++]; // or it will be ] forever
			
			
			
		}
		

		public void NextWordVanilla(BText s, ref int charPos, ref int len){
			
		
			word.Clear();
			
				
			recursion++;
			if (recursion > 500)
			{
				LogError("");
				return ;
			}
			
			bool inquotes = false;
			NextBoundary();
			
			if (c == '"') inquotes = true;
			else
			while (charPos < len && c.isWhiteSpace() ) 
			{
				
				c = s[charPos++];
				
			}
			if (!inquotes && c == '"') inquotes = true;
			
			
			if (c.isLetter() || c.isDigit() || " _-+".Contains(c))
			while (charPos < len && ( inquotes ||  (c.isLetter() || c.isDigit() || " _-+".Contains(c)) ) )
			{
				word.Append(c);
				
				c = s[charPos++];
				// toggles "quoted, comma"
				if (c == '"') inquotes = !inquotes;
				
				
				if (c == ' ' && word.ToString() == "is") 
				{
					NextBoundary();
					break;
				}
			}
			else if (c.isOperation() && c != '&' && c != '!' && c != ':')
			while (charPos < len && c.isOperation()) 
			{
				word.Append(c);
				c = s[charPos++];
				
			
			}
			else if (c != '"' && !(c.isLetter() || c.isDigit()))
			{
				char cc = c;
				while (charPos < len && cc == c)
				{
					word.Append(c);
					c = s[charPos++];
					
				
				}
			}
			
			
			
			
			
			TrimWord();

			
		}

		
		
		public void NextLine(){
			while (charPos < s.Length && !(c.isNewLine() || c == '}'))
			{
				c = s[charPos++];
			}
			
		}
		public void AddOneLine(){
			
			if (c.isNewLine())
			{
				lineCount++;
				lineBroken = true;
				
				char lineChar = c;
			
				if (charPos < s.Length)
					c = s[charPos++];
				while (charPos < s.Length && c.isNewLine() && c != lineChar) 
					c = s[charPos++];
				
				linePos = charPos -1;
				Debug.Log(lineCount);
			}
			
			
		}
		public void AddLine(){
			
			if (c.isNewLine())
			{
				lineCount++;
				
				lineBroken = true;
				
				char lineChar = c;
				
				while (charPos < s.Length && c.isNewLine())
				{
					
					c = s[charPos++];
					if (c == lineChar) 
					{
						lineCount++;
						
					}
				}
				
				linePos = charPos -1;
				//Debug.Log(lineCount);
			}
			
			
		}
		
		public void CheckLineBreaks(){
			
			while (charPos < s.Length && (c == '#' || c.isNewLine()))
			{	
				SkipComment();
				AddLine();
			}
			
		}
		public void SkipComment(){
			
			if (c != '#') return;
			int startPos = charPos - 1;
			c = s[charPos++];
			while (charPos < s.Length && !(c.isNewLine() || c == '#')) 
			{
				c = s[charPos++];
			}
			commentText.Append(s.ToString(startPos, charPos - startPos - 1));
			if (c.isNewLine())
				commentText.Append('\n');
		}
		
	
		public GuiDict droot;
		public GuiDict dnext;
		public GuiDict output => dnext;
		public GuiDict dprev;
		public bool create = false;
		
		public DataTable table = new DataTable();
		public int pos;
		public int len;
		public int hyphens =0;
		public char predicate = '%';
		public bool recursive = false;
		public bool reverse = false;
		public bool dostatic = false;
		public bool makeLocal = false;
		public int stringfind = 0;
		public string w;
		public string linkkey;
		
		protected string ws() => w=word.ToString();
		int ampCounter = 0;
		
		public void Clean()
		{
			target = "";
			pos = 0;
			create = false;
			recursion = 0;
			table.Clear();
			hyphens =0;
			predicate = '%';
			recursive = false;
			reverse = false;
			dostatic = false;
			stringfind = 0;
			multithis = performindex;
			ampCounter = 0;
		}
		public void HandleLink(string key, bool toLocal = false)
		{
			HandleLink(fromDirectory, key, toLocal);
		}
		public void HandleLink(GuiDict root, string key, bool toLocal = false)
		{
			Clean();
			droot = root;
			linkkey = key;
			makeLocal = toLocal;
			
			assignDictionary();
		}
		
		
		protected void nwv()
		{
			dprev = dnext;
			nwvNoprev();
		}
		
		protected void nwvNoprev()
		{
			
			NextWordVanilla(definition, ref pos, ref len);
			ws();
			
			
		}
	
		protected void multiparse() {
			if (multithis== null) Debug.LogError("no multi assigned");
			GuiDict save = dnext;
			int savepos = pos;
			GuiDict all = new GuiDict(){key = KEY_ALL};
			for (int i = 0 ; i < save.Count ; i++)
			{
				c = ' ';
				pos = savepos;
				dnext = save[i];
				multithis();
				if (dnext != null) all.Add(dnext);
			}
			dnext = all;
		}
		protected void performindex() 
		{
// ---------// [? 
			if (c == ']')
			{
				c = ' ';
				return;
			}
			nwvNoprev();
			if (c == ' ' && pos == len - 1 )
			{
				
				Debug.Log("error: opening brace missing closing");
				return;
			}
			
			
			//is it *&%?
			checkSpecialString(ref w);
			bool result = false;
			string sleft = w;
			
			string smid = ""; 
			string sright = "";
			
			//[*]
			//[?]
			//[index]
			if (c == ']')
			{
				
				if (pos < len)
					c = definition[pos++];
				else c = ' ';
				
				
				if (stringfind > 0){
					
					if (ampCounter > 0)
					{
						
						goto LastFactor;
					}
					else
						dnext = dnext.FindPartialMatch(sleft, stringfind > 1);
					return;
				}
				
			
				int? ileft = (int?)sleft.asNumber();
				if (ileft != null) 
					dnext = dnext[(int)ileft];
				else
					dnext = dnext[sleft];
				
				
				
				return;
			}
			
// ---------// [? ?
			
			
			nwvNoprev();
			
			
			// [? ?]
			smid = w; 
			sright = "";
			
			if (c == ']') goto LastFactor;
			
			
// ---------// [? ? ?
			nwvNoprev();
			
			
			sright = w;
			
			if (sright.ToLower() == "null") sright = "";
			
			LastFactor:
			
			// [? ?]
			// [? <>= ?]
			// [!&*?* <>= *?*]
			if (c != ']' && c != ' ')
			{
				Debug.Log("unexpected: " + c);
				nwvNoprev();
				
			}
			
			
			
			if (stringfind > 0)
			{
				// should be true
				if (ampCounter > 0)
				{
					
					GuiDict g;
					g = dnext.FindPartialMatch(sleft, stringfind > 1);
					
					result = (g != null && g.Count > 0);
					
				}
			}
		
			if (sleft == "@" || sleft == "INDEX")
			{
				dnext.parent.GetIndexFromDict(dnext);
				
				if (sright == "")
					result = dnext.parent.GetIndexFromDict(dnext) == smid.asNumber();
				
				else
				{
					if (smid == "" || smid.Contains('='))
						result = dnext.parent.GetIndexFromDict(dnext) == sright.asNumber();
					
					if (!result && smid.Length > 0)
					{
						if (smid[0] == '>')
							result = dnext.parent.GetIndexFromDict(dnext) > sright.asNumber();
						else if (smid[0] == '<')
							result = dnext.parent.GetIndexFromDict(dnext) < sright.asNumber();
					}
				}
			}
			else if (sleft == ":")
			{
				Debug.Log($"{sleft}, {smid}, {sright} ");
				int? imid = (int?)smid.asNumber();
				if (imid == null) 
					imid = smid.Length; // not a number, so it's a bunch of wildcards
				
				if (ampCounter >0 )
					dnext = dnext.SliceList(0,(int)imid);
				else
				{
					string newkey = dnext.SliceKey(0,(int)imid);
					dnext = new GuiDict(){key = newkey};
				}
				
				result = true;
			}

			else if (smid.Contains('*'))
			
				result = dnext.key.Contains(sright);
				
			else if (smid == ":")
			{
				Debug.Log($"{sleft}, {smid}, {sright} ");
				
				int? ileft = (int?)sleft.asNumber();
				if (ileft == null) 
					ileft = sleft.Length; // a bunch of random letters
				
				
				int? iright = (int?)sright.asNumber();
				
				if (sright != "" && iright == null) 
					iright = sright.Length; // a bunch of random letters
				
				Debug.Log($"{ileft}, {smid}, {iright} ");
				
				if (ampCounter >0 )
					dnext = dnext.SliceList((int)ileft,iright);
				else
				{
					string newkey = dnext.SliceKey((int)ileft,iright);
					dnext = new GuiDict(){key = newkey};
				}
				
				result = true;
			}
			else if (sright == "")
			{
				// 2factors
				
				// && >= 2
				if (ampCounter > 1)
				for (int i = 0 ; i < dnext.Count ; i++)
				{
					
					result = (bool)table.Compute(dnext[i].Count +sleft + smid ,"");
					
					if (result) break;
				}
				// & >= 2
				else if (ampCounter > 0)
				{
					
					result = (bool)table.Compute(dnext.Count +sleft + smid ,"");
					
				}
					
				else if ("<>=".Contains(sleft[0]) && dnext.key.isNumber())
					result = (bool)table.Compute(dnext.key +sleft + smid ,"");
				
				else if (sleft == "is" || sleft == "=") 
				
					result = (dnext.key == smid);
				
			}
			else if (ampCounter > 0)
			{
				if ("NUM" == sleft)
				for (int i = 0 ; i < dnext.Count ; i++)
				{
					if (!dnext[i].key.isNumber()) continue;
					result = (bool)table.Compute(dnext[i].key  + smid + sright ,"");
					if (result)  break;
				}
					
				else if (new string[]{"'","LENGTH"}.Contains(sleft, true))
				
				for (int i = 0 ; i < dnext.Count ; i++)
				{
					
					result = (bool)table.Compute(dnext[i].key.Length + smid + sright ,"");
					if (result)  break;
				}
				
				else if (new string[]{"COUNT","LIST"}.Contains(sleft, true))
				for (int i = 0 ; i < dnext.Count ; i++)
				{
					
					result = (bool)table.Compute(dnext[i].Count + smid + sright ,"");
					if (result)  break;
				}
				else if (sleft == "is" || sleft == "=")
				for (int i = 0 ; i < dnext.Count ; i++)
				{
					
					result = (dnext[i].key == smid);
					if (result)  break;
				}
				
				
					
					
			}
			else if ("NUM" == sleft)
			{
				if (dnext.key.isNumber())
					result = (bool)table.Compute(dnext.key + smid + sright ,"");
			}
			else if (new string[]{"'","LENGTH"}.Contains(sleft, true))
				result = (bool)table.Compute(dnext.key.Length + smid + sright ,"");
			else if (new string[]{"COUNT","LIST"}.Contains(sleft, true))
				result = (bool)table.Compute(dnext.Count + smid + sright ,"");
					
			
			//Debug.Log($"is {dnext.key} {sleft}{smid}{sright} ? {result}");
			
			// if no reverse and
			// discard only makes sense for multi-select unless the purpose is to delete
			
			if (reverse == result) 
				dnext = null;
			
			nwvNoprev();
			
		}
		
		
		protected void doswitch() 
		{
			// 1 here
			// 2 hub HUB?
			// 3 class GLOBAL?
			// 4 global GAME?
			recursive = false;
			
			if (w[0] == '.')
			{
				hyphens = w.Count('.');
				
				if ( hyphens == 1 )
				{
					if (pos == len)
					{
						if(dnext.key == KEY_ALL)
							target = KEY_FILL;
						else
							target = VARIABLE_ASSIGNMENT;
					}
					
				}
				else
				for (; hyphens > 1 ; hyphens--)
					dnext = dnext.esc();
					
				
				return;
			}
			
			switch (w)
			{
				case "ROOT": goto case "<<<";
				case "GLOBAL": goto case "<<<";
				case ">>": recursive = true;
				goto case ">";
				case ">": 
					//recursive single?
					nwv();
					if (c == ' ')
						break;
					
					dnext = dnext.Find(w, recursive);
					
					break;
					
				case "<": 
					// just the hub
					if (c.isLetter() || c.isDigit())
					{
						nwv();
						dnext = dnext.FindRoot(w);
					}
					else
						dnext = dnext.ThisComponent;
					break;
				case "<<": 
					// recursive backwards
					if (c.isLetter() || c.isDigit())
					{
						nwv();
						dnext=dnext.Fetch(w);
					}
					else
						dnext = dnext.ThisComponent;
					break;
					
				case "<<<": 
					dnext = dnext.Root; 
					break;
				case "**": 
				
					if (c == '.' && pos < len - 1) nwv();
					
					if (c == ' ')
					{
						dnext = dnext.Hierarchy;
						break;
					}
					if ( c == '[')
					{
						dnext = dnext.Hierarchy;
						multithis = performindex;
						multiparse();
						
						break;
					}
					//recursive multiple
					nwv();
					
					
					dnext = dnext.FindAll(w, true);
					
					break;
					
				case "*": 
				
					if (c == '.' || c == '*' || c == ' ' )
					{
						
						if (pos < len - 1) nwv();
						
						if (dnext.key == KEY_ALL)
						{	
							GuiDict d = new GuiDict(){key  = KEY_ALL};
							for (int i = 0 ; i < dnext.Count ; i++)
							
							for (int ii = 0 ; ii < dnext[i].Count ; ii++)
								
								d.Add(dnext[i][ii]);
								
							dnext = d;
						}
						else
						{	
							dnext = dnext.CloneDict();
							dnext.key = KEY_ALL;
						}
						break;
					}
					if ( c == '[')
					{
						multithis = performindex;
						multiparse();
						
						break;
					}
					nwv();
					
					
					dnext = dnext.FetchAll(w);
					
					break;
				case "[":
					if (c == ' ')break;
					
					performindex();
					
					break;
					
				case "]": 
					c=' ';
					Debug.Log("error: closing brace missing opening");
				
					break;
				default: 
					
					dnext=dnext.FetchLocal(w);
					
					if (dnext == null && create )
					{
						if (predicate == '%')
						{
							dnext = new GuiDict(){key = w};
							dnext.SetParent(dprev);
						}
						else
							dnext = new GuiDict(){key = w};
					}
					break;
					
			}
			// list returns will have no parent
			
			if (dnext != null && dnext.parent == null) 
				dnext.parent = dprev.parent;
		}
			
		protected void checkSpecialString(ref string s)
		{
			//!*?
			
			reverse = false;
			stringfind = 0;
			ampCounter = 0;
			
			while (s.Contains(new char[]{'&','!','%','*'}))
			{
				ampCounter+=s.Count('&');
				stringfind+=s.Count('*');
				
				if (s.Contains('!'))
					reverse = true;
					
				
				//if (s.Contains('%'))
				//{
				//	GuiDict refString = dnext.Fetch(s);
				//	s = refString.GetVar();
				//}
				nwvNoprev();
				
			}
			
		}

			
		
		protected ParserDelegate multithis;
		
		public void parse(){
			
			if (dostatic)
				dnext = dprev = staticDictionary;
			else if (makeLocal)
				dnext = dprev = droot.esc();
			else
				dnext = dprev = droot;
			
			definition.Clear();
			definition.Append(linkkey);
			
			if (definition[definition.Length - 1] != ' ')
				definition.Append(' ');
			len = definition.Length;
			
			
			
			c = linkkey[pos];
			if (!("*%&".Contains(c)))
				pos++;
			else c = ' ';
			nwv();
			
			switch (w)
			{
				case "GAME": dnext = game;break;
				case "GLOBAL": dnext = globalVars; break;
				case "ROOT": dnext = globalVars; break;
				default: doswitch();break;
			}
			
			while (pos < len && dnext != null)
			{
				recursion++;
				if (recursion > 500)
				{	
					break;
				}	
				nwv();
				doswitch();
				
			}
			
			
			
			
		}
		public void assignDictionary()
		{
			
			if (linkkey[0] == '*')
				pos = 0;
			else if ("%&".Contains(linkkey[0]))
			{
				predicate = linkkey[0];
				pos = 1;
				
				if (linkkey.Length > 1 && "%&".Contains(linkkey[1]))
				{
					pos = 2;
					create = true;
					if (linkkey.Length > 2 && "%&".Contains(linkkey[2]))
					{
						pos = 3;
						
						// this refers to static
						dostatic = true;
						
						for (int i = 0 ; i < 4 ; i++)
						{
							if (linkkey[i] == '&') 
							{
								create = false;
								break;
							}
						}							
					}
				}
			}
			else
				pos = 0;
				
			parse();
			
			//if (dnext != null)
			//{
			//	Debug.Log(dnext.key + ":count:" + dnext.Count);
			//	if (dnext.Count > 0) Debug.Log(dnext[0].key);
			//}
			
			if (dnext == droot) return;
			
			if(predicate == '&') 
			{
				// even in lists, they are references by default
				
				globalVars.Add("COPIES");
				globalVars["COPIES"].Add(droot);
				
				if (dnext != null)
				{
					dnext = dnext.Clone();
					// this is causinga stack overflow
					if (!dostatic && makeLocal) 
						dnext.SetParent(droot.parent);
				}
				
				
			}
			else //%
			{
				globalVars.Add("LINKS");
				globalVars["LINKS"].Add(droot);
				
				if (dnext != null)
				{
					if (dostatic) {if (dnext.parent == null)dnext.SetParent(droot);}
					else if (makeLocal) 
						droot.parent.Add(dnext);
						
				}
				
			}
			
		}
		
		// maybe convert link?
		
		
		public void GetTarget(string key){
			int len = key.Length;
			target = "";
			if (key[len-1] == '.' && key[len-2] != '.') target = VARIABLE_ASSIGNMENT;
			
			
		}
		public static bool IsLink(string key)
		{
			
			if ("&%".Contains(key[0]) || ( key.Contains(1, '[')) || ( key.Contains(1, '.')) )
			{	
				return true;
			}
			
			int len = key.Length;
			int i;
			if (len < 4) return false;
			
			for ( i = 0 ;; i++)
			{
				if (key[i] != "GAME"[i]) 
					break;
				
				if (i == 3)
					return true;
			}
			
			for ( i = 0 ;; i++)
			{
				if (key[i] != "ROOT"[i]) 
					break;
				if (i == 4)
					return true;
			}
			
			if (len < 6) return false;
			
			for ( i = 0 ;; i++)
			{
				if (key[i] != "GLOBAL"[i]) 
					break;
				if (i == 4)
					return true;
			}
			return false;
		}

		
	}

}
		