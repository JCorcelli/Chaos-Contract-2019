
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


	
	public class BParser {
		/*
			make dictionary ... interpret
			
			1. make dictionary
				values in dictionary are anything
				
			2. variables, hierarchies, and pointers are easily stored
			
			3. world, atlas, and windows are made of quads
			
			4. pass dictionary to interpreter
			
		*/
		
		
		public delegate void ParserDelegate();
		
		public static BParser current;
		// text that's being appended
		public BText procText = new BText(); 
		// status variable
		public BText commentText = new BText();
		public static BText stackText = new BText();
		// placeholder, never used by BParser the current position is charPos
		
		public BText streamedText = new BText(); 
		public void SetStream(){
			streamedText.Clear();
			streamedText.Append(s.ToString(0, charPos));
		}
		// primary, stepping across 'words'
		public BText word = new BText(); 
		// second, long operations
		public BText definition = new BText(); 
		
		
		// for single operations, third
		public BText braceText = new BText();
		
		
		
		// for limitations in time wasted
		public const int maxTime = 5; // ms
		public const int charAllowance = 55000;
		public static int charCount = 0;
		// for effect
		public float cps = 30000f;
		public float charAdvance = 0f;
		public int charPos = 0;
		public int linePos = 0;
		public int lineCount = 1;
		public bool running = false;
		public bool done = false;
		public char c = ' ';
		
		public GuiDict staticDictionary ;
		
		public GuiDict dictionary ;
		
		public GuiDict accessLinks ;
		public string headKey = "";
		public string staticKey = "";
		
		public GuiDict vars ;
		//public GuiDict replacementHub ;
		public int replacementScope = 0;
		
		public GuiDict globalVars;
		
		
		protected virtual BText s { 
			
			get => procText;
			
		}
		
		
		public float lastTick;
		
		public virtual void Load(TextAsset text)
		{
			procText= new BText(text);
			Load();
		}
		
		public virtual void Eject(){
			
			stackArray = null;
			key_stackArray = null;
			pull_stackArray = null;
			running = false;
		}
		public virtual void Load()
		{
			worldAtlas = new List<GuiAtlas>();
			commentText = new BText();
			
			lastTick = Time.time;
			
			done = false;
			
			c = ' ';
			running = true;
			
			charPos = 0;
			lineCount = 1;
			stackArray = new GuiDict[100];
			key_stackArray = new string[100];
			pull_stackArray = new int[100];
			stackPos = 0;
			
			accessLinks = new GuiDict(){} ;
		}
		
		public virtual void Stop(){ running = false;}
		public virtual void Step(){
			
			if (!running || procText == null) return;
			charAdvance = cps * (Time.deltaTime - lastTick);
			Walk();
		}
		
		public virtual void Walk(){
			charCount = 0;
			
			while (charCount < charAllowance && running && charAdvance >= 1 && charPos < s.Length) 	
				WalkStream();
			
			
			if (charPos >= s.Length) done = true;
		}
		public virtual void WalkStream()
		{
			if (s == null) return;
			c = s[charPos++];
			
			charAdvance -= 1f;
			
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
			
			if (c == ')') 
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
		
		public virtual void ParseBracketList(){
			// activator or operation
			word.Clear();
			definition.Clear();
			if (",:".Contains(c)) c = s[charPos++];
			lineBroken = false;
			NextBoundary();
			
			if (",:".Contains(c) || lineBroken) return;
			
			
			
			
			while (charPos < s.Length && c == '[' && !(lineBroken) )
			{
				ParseBracketVar();
				
				if (word.Length > 0) vars.Add(word.ToString());
				
				NextBoundary();
			}
			
			
			NextBoundary();
			
			
		}
		public void TrimWord(BText s = null){
			if (s == null) s = word;
			if (s.Length < 1) return;
			
			int pos = s.Length;
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
			if (recursion > 5000)
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
			if (recursion > 5000)
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
			
			TrimWord();
			
			if (word.Length > 0)
			{
				
			
				if (! lineBroken && c == '[')
				
					ParseAccessor();
					
				
			
				
				if ( word[word.Length - 1] == ':')
				{
					c = ':'; charPos--;
					
					word.Remove(word.Length - 1, 1);
				}
				
			}
			
			
			
		}
		
		public void AddReplacementStack(string key)
		{
			if (key.Length == 0) return;
			
			// [key] should be key as a variable, and the key passed in is just word anyway.
			
			string dName = "";
			
			
			GuiDict fetchvar = vars.Fetch(key);
			if (fetchvar != null)
				dName = fetchvar[0]?.key;
			if (dName != null && dName != COMPONENT_OBJECT && dName.Length > 0)
			{
			
				
				bool dupe = vars.Contains(key);
				
				
				vars.Include(key);
				
				if (!dupe)
				{
				// it's a variable and has the key
				
				// key in another location, which might be named key. Is it a problem?
				InstanceStack(vars, REPLACEMENT, key, "rename", dName);
				vars[REPLACEMENT][key].Add(vars[key]);
				
				}
			}
			else
			{
				// everything's in here
				bool dupe = globalVars.Contains(key);
				
				vars.Include(key);
				if (!dupe)
				{
				InstanceStack(globalVars,REPLACEMENT, key);
				
				
				globalVars[REPLACEMENT][key].Add(vars[key]);
				}
			}
			
			addReplacementStack = false;
			
		}
		public int hierarchyScope = -1;
		public bool ParseVarAssignment() 
		{
			// c = '='
			string w;
			string ws() => w = word.ToString();
			bool delimited = false;
			int unstack = 0;
			
			if (word.Length < 1)
			{
				// ' ' = var
				NextWord();
				StackVar();
				return true;
			}
			
			// var = ?
			
			string key = word.ToString();
			
	
			if (hierarchyScope > -1 && stackPos >= hierarchyScope ) 
			{
				vars.Include(key);
				
				if (addReplacementStack)
				{	
			
					
					AddReplacementStack(key);
					
					
					
				}
			}
			else{
				vars.Include(VARIABLE_OBJECT, key);
				if (addReplacementStack)
				{	
					PushStack(VARIABLE_OBJECT);
					
					AddReplacementStack(key);
					PullStack();
					
					
				}
			}
			
			
			//if (key == "REPLACEMENT" ){
			//	
			//	replacementScope = stackPos;
			//	replacementHub = vars[VARIABLE_OBJECT][key];
			//}
			if (addReplacementStack)
			{	
				PushStack(VARIABLE_OBJECT);
				
				AddReplacementStack(key);
				PullStack();
				
				
			}
			// have to save position before the first word assignment here, or I can't restor the word placement
				
			
			c = s[charPos++];
			
			
			
			// this would be the right side of the assignment
			
			NextBoundary();
			
			if (lineBroken) return true;
			
			int savePos = charPos;
			int saveLine = lineCount;
			NextWordBlanks();
			
			
			// var = #newline seems to dnext this up
			
			if (",:".Contains(c))
			{
				// = def, ...?
				
				bool isComponent = false;
				bool isItComponent() => isComponent = isComponent ||  c == ':' || (word.Length > 0 && c == '{') ;
				
				isItComponent();
				
				NextWordBlanks();
				
				isItComponent();
					
				
				// now reset
				charPos = savePos - 1;
				lineCount = saveLine;
				c = ',';
				if (isComponent)
					NextWordBlanks();
				else
					NextWord();
								

				
				// = Compt, Nam, [brak] or {def}
				if (isComponent) return ParseVariableComponentObject(key);
				
				// = def, def[,def...]
				
				// let the main loop handle it
			}
			else
			{
				
				charPos = savePos - 1;
				lineCount = saveLine;
				c = ',';
				NextWord();
			}
			
			// normal, v = def[,def]
			PushStack(VARIABLE_OBJECT, key);
			
			StackVar();
			
			void AddVar()
			{
				// two commas in a row, escapes variable assignment
					
				if (word.Length < 1)
				{
					if (c == '{')
					{
						// action inside object
						NextActionBrace();
						vars.Add(ACTION);
						
						vars[ACTION].Include(braceText.ToString());
						lineBroken = false;
						NextBoundary();
					}
					else if (c == '[') 
					while (charPos < s.Length && c == '[') 
					{
						// this is like another markup
						
						ParseBracketVar();
						
						if (word.Length > 0) 
							vars.Include(word.ToString());
						NextBoundary();
					}
					else
						lineBroken = true;
				}
				else if (c == '=' )
				{
					if (delimited)
					{
						bool invar = (vars.parent.key == VARIABLE_OBJECT);
						PullStack();
						
						if (invar)
						{
							vars[VARIABLE_OBJECT].Include(ws());
							PushStack(VARIABLE_OBJECT, w);
						}
						else
						{
							vars.Include(ws());
							PushStack(w);
						}
							
						delimited = false;
					}
					else
					{
						PushStack(ws());
						unstack++;
					}
					
					AddLinks(vars);
					
					c = s[charPos++];
					
				}
				else if (c == '{')
				{
					ParseAnon();
					if (c == ',')
						charPos--;
				}
				else if (c == '(')
				{
					ParseMethod();
					if (c == ',')
						charPos--;
				}
				else
				{
					if (delimited)
					{	
						PullStack();
						
						unstack--;
						delimited = false;
					}
					
					vars.Include(ws());
					
					AddLinks(vars.FindLast(w));
					
				}
				if (c == ';') lineBroken = true;
				else if (c == ',')
				{
					delimited = false;
					c = s[charPos++];
					
						// this technicaly unstacks
				
					while(c == ',')
					{
						
						if (unstack > 1)
						{	
							PullStack();
							
							unstack--;
						}
						else 
						{
							delimited = true;
							while(c == ',')
								c = s[charPos++];
							
							break;
						}
						
						c = s[charPos++];
						
					}
					
					
				}
				
			}
			
			void StackVar()
			{
				
				AddVar();
				
				while (charPos < s.Length && !( c == '}' || lineBroken )){
					
					NextWord();
					AddVar();
				}
				if (unstack > 0)
				{
					for ( ; unstack > 0; unstack--)
						PullStack();
				
					//Debug.Log(vars.parent.DebugVariableTree("",""));
					
				}
			}
			
			return PullStack();
		}
		
		public bool ParseHead(){
			if (s == null) 
			{
				LogError("Text is null. Nothing to parse");
				return false;
			}
			
			// specific syntax is expected
			NextWord();
			if (charPos >= s.Length || c == '}') return true; // end of file
			
			if (c != ',')
				{
				LogError("Syntax error ',' expected, got '" + c +"'");
				return false;
			}
			string classKey = word.ToString();
			
			NextWord();
			if (word.Length <= 0)
			{
				LogError("incorrect definition, ending parse:" );
				return false;
			}
			staticKey = headKey = word.ToString();
			
			GuiGameVars.game.Add(classKey);
			dictionary = GuiGameVars.game[classKey];
				
			
			dictionary.Instance(headKey);
			
			vars = globalVars = dictionary[staticKey];
			
			globalVars.Add(STATIC);
			staticDictionary = globalVars[STATIC];
			staticDictionary.Instance();
			
			NextBoundary();
			if (c == '{') return true;
			
			PushStack(ACTIVATOR);
			ParseBracketList();
			PullStack();
			
			
			if (c == '{') return true;
			
			PushStack(OPERATION);
			ParseBracketList();
			PullStack();
			
			if (c != '{') 
			{	
				LogError("Should be"+classKey+","+headKey+",[activator],[operation] { <body> }");
				return false;
			}
			
			return true;
		}
		
		protected void Warn(string er){
			
			
			Debug.Log("<size=14><color=yellow>Warn: "+ er + " at : " + charPos + " line "+ lineCount + "</color></size>, object stack:"+stackText.ToString(), s.sourceText);
			
			 
		}
		
		protected void LogError(string er){
			
			int cp = (charPos - linePos);
			Debug.LogError("<size=18><color=green>Error: "+ er + " at line "+ lineCount + " pos " + cp + "</color></size>, object stack:"+stackText.ToString(), s.sourceText);
			
			 throw new System.Exception("");
			 
		}
		
		public int stack = 0;
		public int recursion = 0;
		
		public bool ParseVariableComponentObject(string key){
			// parse anonymous and component braced object
			
			PushStack( VARIABLE_OBJECT, key);
			
			bool b = ParseComponentObject();
			
			PullStack();
			return b;
		}
		
		public static GuiDict[] stackArray;
		public static string[] key_stackArray;
		public static int[] pull_stackArray;
		public static int stackPos = 0;
		
		public void InstanceStack(GuiDict dict, params string[] key){
			
			int len = key.Length;
			GuiDict droot = dict;
			droot.Instance();
			for (int i = 0 ; i < len ; i++)
			{
				
				if (hierarchyScope > -1 && stackPos >= hierarchyScope && key[i] == VARIABLE_OBJECT) 
				{
					continue;
					
				}
				
				droot.Instance(key[i]);
				droot = droot.FindLast(key[i]);
			}
			
		}
		
		public bool PushStack(string key){
			
			if (hierarchyScope > -1 && stackPos >= hierarchyScope && key == VARIABLE_OBJECT) 
			{
				pull_stackArray[++stackPos] = 0;
				return true;
				
			}
			bool b = _PushStack(key);
			pull_stackArray[stackPos] = 1;
			
			return b;
		}
		
		public bool PushStack(params string[] key){
			
			int len = key.Length;
			int truelen = len;
			bool b = true;
			for (int i = 0 ; i < len ; i++)
			{
				if (hierarchyScope > -1 && stackPos >= hierarchyScope && key[i] == VARIABLE_OBJECT) 
				{
					truelen--;
					continue;
					
				}
				b = _PushStack(key[i]) && b;
			}
			
			pull_stackArray[stackPos] = truelen;
			
			return b;
		}
		public bool _PushStack(string key){
			
			
			stackText.Append(key);
			stackText.Append("/");
			
			stackArray[stackPos] = dictionary;
			key_stackArray[stackPos] = headKey;
			dictionary = dictionary.FindLast(headKey);
			headKey = key;
			
			
			dictionary.Instance(headKey);
			
			// can't handle including, so this relies on being obvious
			
			stackPos ++;
			if (stackPos > 500)
			{
				LogError("stack overflow: > 500 deep.");
				return false;
			}
			vars = dictionary.FindLast(headKey);
			return true;
		}
		
		
		public bool PullStack(){
			// need this to repeat
				
				
			
			int len = pull_stackArray[stackPos];
			
			bool b = true;
			for (int i = 0 ; i < len ; i++)
			{
				b = _PullStack() && b;
			}
			//Debug.Log(stackText.ToString());
			vars = dictionary.FindLast(headKey);
			//if (stackPos < replacementScope) replacementHub = null;
			
			if (stackPos < hierarchyScope) 
				hierarchyScope = -1;
			
			return b;
		}
		public bool _PullStack(){
			int len = headKey.Length + 1;
			
			
			stackText.Remove(stackText.Length - len, len);
			stackPos --;
			
			dictionary = stackArray[stackPos];
			headKey = key_stackArray[stackPos];
			
			
			if (dictionary == null || headKey == null) return false;
			
			
			return true;
		}
			
		public bool ParseObject(){
			// parse anonymous and component braced object
						
			int startLine = lineCount;
			
			c = s[charPos++];
			
			bool b = true;
			
			while (charPos < s.Length && !( c == '}'))
			{

				recursion ++;
				if (recursion > 5000)
				{
					LogError("inf recursion char:" + c + " :last word:"+ word.ToString());
					return false;
				}
				b = ParseDefinition();
			}
			if (c == '}')
			{
				c = s[charPos++];
				CheckLineBreaks();
			}
			else
				LogError("object, opening brace at "+startLine+" missing closing brace");
			
		
					
			return b;
			
		}
		
		
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
		
		public bool ParseComponentObject(){
			// Game Object
			recursion++;
			if (recursion > 5000) 
			{	
				LogError("");
				return false;
			}
			string obType = word.ToString();
			
			
			vars.Add(COMPONENT_OBJECT,obType);
			
			
			if (c != ':')
				NextWordBlanks();
			else 
			{
				word.Clear();
				//if (s[charPos - 1] == ':')
				c = ',';
			}
			
			string objectName = "";
			
			//shorthand
			// Component,... #names the object "Component"
			
			
			if (word.Length < 1)
			{
				
				objectName= obType;
				// Component, <end of line>
				vars[COMPONENT_OBJECT][obType].Include(objectName);
				
				
				if (!",:{".Contains(c))
				{
					
					// proper end
					return true;
				}
				
				else if (!"{};,:".Contains(c))
				{
					LogError("Error parsing component: "+obType+",'" + c +"...");
					return false;
				}
				
			}
			else
			{	
				objectName= word.ToString();
				vars[COMPONENT_OBJECT][obType].Include( objectName);
			}
			
			// make v = this
			if (lineBroken || c == ';') return true;
			
			
			bool ParseStack()
			{
				return (ParseObject() & PullStack());
			}
			PushStack(COMPONENT_OBJECT, obType, objectName);
			
			if (c == '{') 
			{
				return ParseStack();
			}
			
			
			
			
			if (lineBroken || c == ';')  return ParseStack();
			
			// make v = this.activator
			// activator, format [is_on = 1]
			
			PushStack(ACTIVATOR);
			ParseBracketList();
			if (vars.Count < 1) vars.NullifyQuick();
			PullStack();
			
			
			
			if (c == '{' ) return ParseStack();
			else if (lineBroken || c == ';') return PullStack();
			
			
			
			// operation, format [is_on += 1]
			PushStack(OPERATION);
			ParseBracketList();
			if (vars.Count < 1) vars.NullifyQuick();
			PullStack();
			
			if (c == '{' ) return ParseStack();
			if (!(lineBroken || c == ';'))
				LogError("Error parsing "+COMPONENT_OBJECT+"/"+obType+"/"+objectName+"/ use semicolons if this is an object containing a list.");
				
			
			return PullStack();
			
			
			
		}
		
		
		public void AddLinks(GuiDict droot)
		{
			if ("&%*<>".Contains(droot.key[0]) || droot.key.Contains(1, '[') )
			{
				accessLinks.Include(droot);
				return;
			}
			
			int len = droot.key.Length;
			if (len < 4) return;
			int i;
			for ( i = 0 ;; i++)
			{
				if (droot.key[i] != "GAME"[i]) 
					break;
				
				if (i == 3)
					{accessLinks.Include(droot);return;}
			}
			
			for ( i = 0 ;; i++)
			{
				if (droot.key[i] != "ROOT"[i]) 
					break;
				if (i == 4)
					{accessLinks.Include(droot);return;}
			}
			
			if (len <6 ) return;
			for ( i = 0 ;; i++)
			{
				if (droot.key[i] != "GLOBAL"[i]) 
					break;
				if (i == 4)
					{accessLinks.Include(droot);return;}
			}
		}
		
		
		public bool ParseAnon(){
			
			string key = word.ToString();
			if (addReplacementStack)
			{	
				PushStack(ANON_OBJECT);
				AddReplacementStack(key);
				PullStack();
				
			}	
			
			PushStack(ANON_OBJECT, key);
			
			
			
			int startPos = lineCount;
			bool b = ParseObject() & PullStack();
			
			
			return b;
		}
		
		
		
		public bool isWindow(char c){
			return "|+-^".Contains(c);
		}
		public bool isWindow(){
			return isWindow(c);
		}
		
			
		

		
		public GuiBlock currentBlock;
		public List<GuiAtlas> worldAtlas;
		
		protected char savec  ;
		protected BText saves ;
		protected int savepos ;
		protected int saveline;
		//protected BText savetext;
		
		public void SaveState(){
			savec = c;
			saves = procText;
			savepos = charPos;
			saveline = lineCount;
			//savetext= stackText;
		}
		public void NewState(BText newstate){
			c = ' ';
			procText = newstate;
			charPos = 0;
			lineCount = 0;
		}
		public void RevertState(){
			
			c = savec;
			procText = saves;
			charPos = savepos;
			lineCount = saveline;
			//stackText
		}
		
		
		
		// this version will return the atlas from start position
		public GuiAtlas GetAtlas(int i){
			if ( worldAtlas.Count <= i) return null;
			
			return worldAtlas[i];
		}
		// this version will parse the atlas from start position
		public GuiAtlas ParseWindowFromDict(GuiDict g)
		{
			GuiAtlas atlas = new GuiAtlas();
			vars = g.parent;
			
			atlas.Generate(s.ToString((int)g["start"].key.asNumber()));
			
			int trueline = lineCount;
			
			BText temp = new BText();
			
			for (int i = 0 ; i < atlas.atlasVariables.Count ; i++)
			{
				GuiBlock gb = atlas.atlasVariables[i];
				temp.Clear();
				temp.Append(gb.ToVariable()+" "); //2 spaces req.
				NewState(temp);
				Debug.Log($"Line {trueline + gb.ypos}, pos {gb.xpos}");
				while (charPos < s.Length)
					ParseDefinition();
			}
			
			for (int i = 0 ; i < atlas.Count ; i++)
			{
				currentBlock = atlas[i];
				if (atlas.atlasVariables.Contains(currentBlock)) continue;
				//atlas
				
				_ParseWindow();
				
			}
			
			return atlas;
		}
		public void ParseWindow(){
			// backtrack
			
			int atlasStart = linePos;
			
			
			GuiAtlas atlas = new GuiAtlas();
			// include or add?
			worldAtlas.Add(atlas);
			
			
			
			// adds the atlas position to the dict (for editing it or referencing it?)
			atlas.atlasStart = atlasStart;
			atlas.Generate(s.ToString(atlasStart));
			
			//Debug.Log("input\n"+atlas.allText.ToString());
			//Debug.Log("world\n"+atlas.world.ToString());
			
			//debug
			int trueline = lineCount;
			SaveState();
			
			BText temp = new BText();
			for (int i = 0 ; i < atlas.atlasVariables.Count ; i++)
			{
				GuiBlock gb = atlas.atlasVariables[i];
				temp.Clear();
				temp.Append(gb.ToVariable()+" ");
				NewState(temp);
				//Debug.Log($"Line {trueline + gb.ypos}, pos {gb.xpos}");
				while (charPos < s.Length)
					ParseDefinition();
			}
			
			RevertState();
			
			//Debug.Log(s.ToString(charPos, 20));
			charPos = atlasStart + atlas.atlasEnd;
			
			lineCount += atlas.lineStarts.Length - 1;
			
			//Debug.Log(s.ToString(charPos - 20, 20));
			c = s[charPos++];
			
		
			// should I add mods now?
			
			char savec = c;
			for (int i = 0 ; i < atlas.Count ; i++)
			{
				currentBlock = atlas[i];
				if (atlas.atlasVariables.Contains(currentBlock)) continue;
				//atlas
				
				vars.Include(WINDOW_ASCII);
				PushStack(WINDOW_ASCII);
				
				vars.Include( WINDOW_ATLAS);
				GuiDict w_atl = vars.FindLast(WINDOW_ATLAS);
				InstanceStack(w_atl, "start", ""+atlasStart);
				InstanceStack(w_atl, "end", ""+(atlas.atlasEnd));
				InstanceStack(w_atl, "name",atlas.atlasName);
				
				_GetWindow();
				
				PullStack();
				
			}
			// adds the window to the dict
			
			c = savec;
			
			
			NextBoundary();
		}
		
			
		protected void _GetWindow(){
			
			//vars.Include(WINDOW_ASCII);
			//PushStack(WINDOW_ASCII);
				
			GuiBlock block = currentBlock;
			List<GuiWindow> windows = block.windows;
			string key = "";
			InstanceStack(vars, "name",block.name);
			InstanceStack(vars, "start",""+block.atlasWord.begin);
			for (int i = 0 ; i < windows.Count ; i++)
			{
				key = windows[i];
				AddReplacementStack(key);
				//vars.Include(key);
			}
			//PullStack();
		}
		protected void _ParseWindow()
		{
			
			//charPos = 0
			
			
			definition.Clear();
			string key;
			int left, window_wide, window_high;
			
			//####################### vars
			
			
			//####################### ^^^
			
			
			/* 
			if (currentWEND.wide == 1)
			{
				currentWEND.high = window_wide;
			}
			else
				currentWEND.high = window_high;
			
			 */
			int  top, bottom, right;
			float w_frac, x_frac, y_frac, h_frac ;
			
			
			
			// i've decided the default behavior will attempt to expand all windows to touch the screen edges, so no more resizing
			GuiElement eword;
			List<GuiWindow> windows = currentBlock.windows;
			
			
			window_wide = currentBlock.wide;
			window_high = currentBlock.high;
			InstanceStack(vars, "name",currentBlock.name);
			for (int i = 0 ; i < windows.Count ; i++)
			{
				eword = windows[i];
				key = eword;
				
				// do I need to do this?
				//int ki = 1;
				string name = key;
				//while (vars.Contains(key))
				//	key = name + "." + (ki++);
				
				AddReplacementStack(key);
				
				
				left = eword.left ;
				right = eword.right ; // + 1 for char width, and only after, so it doesn't get eaten
				
				
				
				// math
				top = eword.top;
				
				bottom = eword.bottom ;
				
				
				left--;
				right--;
				
				window_wide -= 2;
				if (window_high > 1){
					window_high -= 2;
					top--;
					bottom--;
				}
				x_frac = (float)left / window_wide;
				h_frac = 1 - (float)top / window_high;
				w_frac = (float)right / window_wide;
				y_frac = 1 - (float)bottom / window_high;
				
				left++;
				right++;
				window_wide += 2;
				if (window_high > 1){
					window_high += 2;
					top++;
					bottom++;
				}
				
				
				InstanceStack(vars, key, "left", ""+x_frac);
				
				InstanceStack(vars, key, "top", ""+y_frac);
				InstanceStack(vars, key, "right", ""+w_frac);
				InstanceStack(vars, key, "bottom", ""+h_frac);
				
			
			}
			
			// I can put mods into a window here
			List<GuiElement> mods = currentBlock.mods;
			
			int x;
			int y;
			
			GuiElement m;
			
				
			for (int i = 0 ; i < mods.Count ; i++)
			{
				// so, really I should check what type of mod it is first
				m = mods[i];
				
				// situational mods
				if ("#X".Contains(m))
					continue;
				
				// "Mod>" or default >
				// left - right window binds
				
				
				// button?
				if (m.ToString().Contains('*'))
				{
					
					x = m.left;
					y = m.top;
					
					
					
					x_frac = (float)x / window_wide;
					y_frac = 1 - (float)y / window_high;
					
					
					InstanceStack(vars, "mods", "*", x+"", y+"", window_wide+"", window_high+"");
					
				
				}
				
				
			}
			
			
		}
		
		public bool addReplacementStack = false;
		public bool ParseDefinition(){
			recursion++;
			if (recursion > 5000)
			{
				LogError("recursion");
				return false;
			}
			
			
			NextWord(); // this is the only place , isn't consumed so far
			
			
			addReplacementStack = false;
			if ( c == '[')
			{
				addReplacementStack = true;
				// special [placeholder] variable
				
				// finish the parse
				ParseBracketReplacement();
				c = s[charPos++];
				word.Clear();
				word.Append(braceText.ToString());
				NextBoundary();
				
			}
			
			if (":,".Contains(c) )
			{
				
				// Component, Name
				// Component,{}
				// Component,:{}
				// Component,:[act],[op]{}
				
				
				return ParseComponentObject();
				
				
			}
			else if (c == '=')
			{
				// defined variable
				
				ParseVarAssignment();
				
			
			}
			else if ( c == '{')
			{
				
				if (word.Length < 1)
				{
					// action inside object, may be used multiple times
					NextActionBrace();
					vars.Add(ACTION);
					vars[ACTION].Include(braceText.ToString());
				}
				else
				{	
					// anonymous object definition
					ParseAnon();
					if (c == ',')
						charPos--;
				}
			}
			
			else if (c == '(' )
			{
				ParseMethod();
			}
			else if (word.Length > 0)
			{
				// variable, may be included more than once like a physical inventory
				
				
				string ws = word.ToString();
				
				if (ws == "NOVAR")
				{
					hierarchyScope = stackPos ;
					vars.Include(ws);
				}
				else
				{
					NextBoundary();
					if (c == '{')
					{
						ParseAnon();
						if (c == ',')
							charPos--;
					}
					else
					{
				
						if (hierarchyScope > -1 && stackPos >= hierarchyScope ) 
						{
							vars.Include(ws);
							
							AddLinks(vars.FindLast(ws));
						}
						else
						{	
							vars.Include(VARIABLE_OBJECT, ws);
							AddLinks(vars[VARIABLE_OBJECT].FindLast(ws));
						}
		
						if (addReplacementStack)
						{	
							PushStack(VARIABLE_OBJECT);
							AddReplacementStack(ws);
							PullStack();
						}
					}
				}
				
			}
			else if ( isWindow())
			{
				//seems like I'm parsing ascii
				ParseWindow();
				
			}
			
			
			return true;
		}
		
		public void ParseMethod(){
		
			if (addReplacementStack)
			{	
				PushStack(METHOD);
				AddReplacementStack(word.ToString());
				PullStack();
			}	
			// method
			NextEndParenthesis();
			string method = word.ToString(); // of course the params have to be interpreted
			string head = braceText.ToString();
			vars.Add(METHOD,method);
			GuiDict v = vars[METHOD][method];
			v.Include(head);
			v = v[v.Count - 1];
			
			NextBoundary();
			if (c == '{')
			{
				NextEndBrace();
				// method shouldn't be defined more than once in most cases
				v.Add( braceText.ToString());
			}
		}
		public virtual bool ParseBody(){
			
			
			int startLine = lineCount;
			
			bool success = true;
			int endPoint = FindMatchingEndBrace();
			if (endPoint >= s.Length)
			{
				LogError("Error parsing brace pair");
				return false;
			}
			while (charPos < endPoint && success)
			{
				
				success = ParseDefinition();
				
			}
			
			
			return success;
		}
		

		public virtual void Parse(){
			// ### head, it's just the dictionary setup
			
			
			bool b = true;
			while (charPos < s.Length && b)
			{
				
				b = ParseHead();
				
				if (charPos >= s.Length || c == '}') break;
				c = s[charPos++]; // it should be { before this
				// #### body
				b = b && ParseBody();
				
			}
			done = true;
			
			HandleLinks();
			Stop();
			if (b) Debug.Log($"<color=yellow>{GuiDict.keyCount} keys added</color>");
				
			else if (charPos >= s.Length) LogError("EOF no ending brace");
		}
		
		
		public void NextLine(){
			while (charPos < s.Length && !(c.isNewLine() || c == '}' || c == ';'))
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
		
		
		
		protected LinkHandler linkHandle = new LinkHandler();
		public void HandleLinks()
		{
			linkHandle.Load(vars);
			for (int i = 0 ; i < accessLinks.Count ; i++)
			{
				linkHandle.HandleLink(accessLinks[i], accessLinks[i].key, true);
			}
		}
		
		//public void HandleLinks()
		//{
		//	
		//	for (int i = 0 ; i < accessLinks.Count ; i++)
		//	{
		//		HandleLink( accessLinks[i].key);
		//	}
		//}
		
		
	}

}
		