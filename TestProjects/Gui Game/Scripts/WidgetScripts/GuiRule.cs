
using UnityEngine;
using UnityEngine.Assertions;

using UnityEngine.UI;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using MEC;

using Utility.GUI;
using SelectionSystem;
using System;
using System.Text;
using static GuiGame.GuiGameVars;

namespace GuiGame
{

	public delegate void GuiRuleDelegate();
	
	
	public class M {
		// for params at least, in practice
		
		
		public M(){}
		public M(GuiDict g){
			Load(g);
		}
		
		
		public delegate void VoidMDelegate();
		
		public VoidMDelegate parseAccessor;
		
		public string next => NextWord();
		public string[] input;
		
		public BText s = new BText();
		public int index = 0;
		public string head => mod.i(index, 0).key;
		public string body => mod.i(index, 1).key;
		
		public GuiDict mod;
		public string name => mod.key ?? "*";
		
		public const string _END = "_&0";
		public string END => _END;
		
		public int charPos = 1;
		public int linePos = 0;
		public int lineCount = 1;
		public const string HC = ",;|";
		public const string BC = ",;=+-|]()";
		public string endChars = HC;
		public bool isEndChar() => c.isOperation() || endChars.Contains(c);
		
		protected int step = 1;
		public bool lineBroken = false;
		public bool running = false;
		
		DataTable doMath = new DataTable();
		public bool Math(ref string numbers){
			if (numbers == "") return true;
			try {
				numbers = ""+doMath.Compute(numbers.Trim('"'), "");
			}
			catch {
				return false;
			}
			return true;
		}
		
		public void Clean(){
			charPos = 1;
			linePos = 0;
			recursion = 0;
			lineCount = 1;
			lineBroken = false;
			running = false;
			commentText.Clear();
			definition.Clear();
			word.Clear();
			s.Clear();
		}
		public void Load(GuiDict guimod){
			mod = guimod;
			linkH = GuiRule.linkH;
			linkH.Load(mod);
			linkH.fromDirectory = mod.esc();
			Parse();
		}
		
		public void LogError(string er){
			
			 throw new System.Exception(er);
		}
		
		public void SetAccessor(int i)
		{
			if (i == 1)
				parseAccessor = ParseAccessorLeft;
			else if (i == 2)
				parseAccessor = ParseAccessorRight;
		}
		public string Next(){
			if (!running) return END;
			// step 1, Head
			
			if (s.Length > 0 && charPos < s.Length)
			{
				SetAccessor(1);
				return next;
			}
			
			// step 2, Body
			step++;
			if (step == 2) 
				ParseBody();
			else
				running = false;
			return END;
		}
		
		public void ParseHead(string inString){
			List<string> p = new List<string>();
			
			string v = inString.Trim(new char[]{'(',')'});
			
			input = v.Split(',');
			for (int i = 0 ; i < input.Length ; i++)
			{
				input[i] = input[i].Trim();
			}
			
		}
		public void ParseHead(){
			List<string> p = new List<string>();
			
			_ParseHead();
			string v = Next();
			for ( int count = 0; running && v != END ; count++)
			{
				p.Add( v );
				v = Next();
			}
			
			ParseBody();
			input = p.ToArray();
		}
		public void _ParseHead(){
			// (head)
			Clean();
			step = 1;
			s.Append(head);
			if (!mod.success) 
				return;
			
			endChars = HC;
			running = true;
			c = '(';
			
		}
		
		public void ParseBody(){
			// {body}
			Clean();
			step = 2;
			s.Append(body);
			
			if (!mod.success) 
				return;
			endChars = BC;
			running = true;
			c = '(';
			
		}
		public void Parse() => ParseHead();
		
		
		protected BText commentText = new BText();
		public BText word = new BText();
		protected BText definition = new BText();
		protected int recursion = 0;
		public char c = ' ';
		public virtual void NextBoundary(){
			CheckLineBreaks();
			while (charPos < s.Length &&  (c.isWhiteSpace()) ) 
			{
				c = s[charPos++];
				CheckLineBreaks();
				
			}
		}
		
		public bool isStartChar() => "(,".Contains(c) ;
	
	
		public string GetParenthesis(){
			definition.Clear();
			int pairs = 1;
			
			definition.Append(c);
			while (charPos < s.Length && pairs > 0 )
			{
				
				c = s[charPos++];
				if (c == ')') pairs --;
				else if (c == '(') pairs ++;
				
				definition.Append(c);
				CheckLineBreaks();
			}
			if (c == ')')
			{
				c = s[charPos++];
				CheckLineBreaks();
			}
			return definition.ToString();
		}

		public void SkipParenthesis(){
			int pairs = 1;
			
			while (charPos < s.Length && pairs > 0 )
			{
				
				c = s[charPos++];
				if (c == ')') pairs --;
				else if (c == '(') pairs ++;
				
				CheckLineBreaks();
			}
			
			if (c == ')')
			{
				c = s[charPos++];
				CheckLineBreaks();
			}
			
		}


		public void SkipConditionalBracket(){
			
			recursion++;
			
			if (recursion > 500)
			{
				LogError("");
				return ;
			}
			
			if (c == '[')
			{
				c = s[charPos++];
				CheckLineBreaks();
			}
			
			
			NextBoundary();
			int pairs = 1;
			while (charPos < s.Length && pairs > 0)
			{
				c = s[charPos++];
				
				if (c == '[') pairs++;
				else if (c == ']') pairs--;
				CheckLineBreaks();
			}
			
			if (c == ']')
			{
				c = s[charPos++];
				CheckLineBreaks();
			}
			
			
			
		}
		
		public string ConditionalBracket(){
			if (c == '[')
			{
				c = s[charPos++];
				CheckLineBreaks();
			}
			NextBoundary();
			if (c == ']') return "False";
			definition.Clear();
			recursion++;
			
			if (recursion > 500)
			{
				LogError("");
				return "";
			}
			
			
			int pairs = 1;
			if (c == ']') pairs--;
			while (charPos < s.Length && pairs > 0)
			{
				definition.Append(c);
				c = s[charPos++];
				
				if (c == '[') pairs++;
				else if (c == ']') pairs--;
				CheckLineBreaks();
			}
			
			int savepos = charPos;
			charPos = 0;
			
			BText save = s;
			
			s = definition;
			char savec = c;
			c = ' ';
			s.Append(' ');
			charPos = 0;
			
			string r = Sum();
			
			//Debug.Log("conditional:"+r);
			
			
			charPos = savepos;
			s = save;
			c = savec;
			
			if (c == ']')
			{
				c = s[charPos++];
				CheckLineBreaks();
			}
			
			if (r == "" || r == "0" || r.ToLower() == "false")
				return "False";
			else return "True";
			
			
		}
		public string Peek(){
			
			SaveState();

			SetAccessor(2);
			string s = NextChunk();
			
			charPos = savepos;
			lineCount = saveline;
			
			
			return s;
		}
		public string NextChunk(){
			definition.Clear();
			lineBroken = false;
			
			c = s[charPos++];
			CheckLineBreaks();
			while (charPos < s.Length && !lineBroken && !"(;,".Contains(c))
			{
				definition.Append(c);
				c = s[charPos++];
				CheckLineBreaks();
			}
			return definition.ToString();
		}
		public string NextWord()
		{
			word.Clear();
			
			recursion++;
			if (recursion > 500)
			{
				LogError("");
				return "";
			}
			
			bool inquotes = false;
			NextBoundary();
			if (c == ';')
			{
				c = s[charPos++];
				CheckLineBreaks();
			}
			
			if (c == '"') inquotes = true;
			else if (c == '{')
			while (charPos < s.Length && c!= '}')
			{
				// action, to be interpreted somehow else
				word.Append(c);
				c = s[charPos++];
				CheckLineBreaks();
				
			}
			else
			while (charPos < s.Length &&  (c.isWhiteSpace() || isStartChar()) ) 
			{
				
				c = s[charPos++];
				CheckLineBreaks();
			}
			if (!inquotes && c == '"') inquotes = true;
			
			lineBroken = false;
			
		
			if (c.isOperation())
			
			while (!lineBroken && charPos < s.Length && c.isOperation() ) 
			{
				word.Append(c);
				c = s[charPos++];
				CheckLineBreaks();
			}
			
			else if (c != '"' && !(c.isLetter() || c.isDigit()) && !(c == '\''))
			{
				char cc = c;
				while (!lineBroken && charPos < s.Length && cc == c)
				{
					word.Append(c);
					c = s[charPos++];
					CheckLineBreaks();
				}
			}
			else
			while (charPos < s.Length && ( inquotes || !(lineBroken || isEndChar()) ) )
			{
				word.Append(c);
				c = s[charPos++];
				// toggles "quoted, comma"
				if (c == '"') inquotes = !inquotes;
				CheckLineBreaks();
				
			}
			
			
			if (word.Length > 0 && !lineBroken && LinkHandler.IsLink(word.ToString()))
				
				parseAccessor();
				
			
			
			if (word.Length < 1 && charPos >= s.Length)
			
				return END;
			
			
			TrimWord();

			
			return word.ToString();
		}

		public string NextWordMath()
		{
			word.Clear();
			
			recursion++;
			if (recursion > 500)
			{
				LogError("");
				return "";
			}
			
			bool inquotes = false;
			NextBoundary();
			if (c == ';')
			{
				c = s[charPos++];
				CheckLineBreaks();
			}
			
			if (c == '"') inquotes = true;
			else if (c == '{')
			while (charPos < s.Length && c!= '}')
			{
				// action, to be interpreted somehow else
				word.Append(c);
				c = s[charPos++];
				CheckLineBreaks();
				
			}
			else
			while (charPos < s.Length &&  (c.isWhiteSpace() || isStartChar()) ) 
			{
				
				c = s[charPos++];
				CheckLineBreaks();
			}
			if (!inquotes && c == '"') inquotes = true;
			
			lineBroken = false;
			
		
			if (c.isOperation())
			{
				word.Append(c);
				c = s[charPos++];
				CheckLineBreaks();
				
				if ("-".Contains(word[0]) && !c.isOperation())
				while (charPos < s.Length && ( inquotes || !(lineBroken || isEndChar()) ) )
				{
					word.Append(c);
					c = s[charPos++];
					// toggles "quoted, comma"
					if (c == '"') inquotes = !inquotes;
					CheckLineBreaks();
					
				}
			}
			
			else if (c != '"' && !(c.isLetter() || c.isDigit()) && !(c == '\''))
			{
				char cc = c;
				while (!lineBroken && charPos < s.Length && cc == c)
				{
					word.Append(c);
					c = s[charPos++];
					CheckLineBreaks();
				}
			}
			else
			while (charPos < s.Length && ( inquotes || !(lineBroken || isEndChar()) ) )
			{
				word.Append(c);
				c = s[charPos++];
				// toggles "quoted, comma"
				if (c == '"') inquotes = !inquotes;
				CheckLineBreaks();
				
			}
			
			
			if (word.Length > 0 && !lineBroken && LinkHandler.IsLink(word.ToString()))
				
				parseAccessor();
				
			
			
			if (word.Length < 1 && charPos >= s.Length)
			
				return END;
			
			
			TrimWord();

			
			return word.ToString();
		}

		public void ParseAccessorRight(){
			if ("<>*".Contains(word[0]) )return;
			int pairs = 1;
			
			
			while (charPos < s.Length
			&& (pairs > 0 && !(lineBroken || ";,}%&+-".Contains(c) ))
			|| pairs > 1)
			{
				word.Append(c);
				c = s[charPos++];
				if ( c == '[') pairs++;
				else if ( c == ']') pairs--;
				CheckLineBreaks();
			}
			
			if ("%&]".Contains(c)) {c = ' ';charPos++;NextBoundary();c = s[charPos-1];}
			
			TrimWord();
			
		}
		public void ParseAccessorLeft(){
			int pairs = 1;
			while (charPos < s.Length && !(lineBroken || ";,=}+-".Contains(c)))
			{
				word.Append(c);
				c = s[charPos++];
				if ( c == '[') pairs++;
				else if ( c == ']') pairs--;
				CheckLineBreaks();
			}
				
			
			if (c == '='){charPos--; c = ' ';}
			TrimWord();
			
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
				//lines.Add(linePos);
			}
			
			
		}

		public void NextLine(){
			lineBroken = false;
			while (charPos < s.Length && !(lineBroken || c == '}' || c == ';'))
			{
				c = s[charPos++];
				CheckLineBreaks();
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
		
		public void TrimWord(BText s = null){
			if (s == null) s = word;
			if (s.Length < 1) return;
			
			int pos = s.Length ;
			while (pos > 0 && s[pos-1].isWhiteSpace() )
				pos --;
			s.Remove(pos, s.Length - pos);
		}

		
		public string asString = "";
		public GuiDict g;
		public GuiDict temporary = new GuiDict(true){key = TEMPORARY};
		public LinkHandler linkH;
		
		
		
		public string WhatIsThis(string thisVar)
		{
			if (thisVar == null) return "null";
			if (thisVar.Length == 0) return "empty";
			
			if (thisVar.isNumber()) return "number"; // string needed anyways
			
			asString = thisVar.asString();
			if (asString != "") return "string";
			
			if (thisVar.isOperation()) return "operation";
			
			if (LinkHandler.IsLink(thisVar)) 
				return "variable";
			if (thisVar.isStringLiteral())
			{
				// aka variable
				asString = thisVar;
				return "string";
			}
			
			
			return "not found";
		}
		public GuiDict UseVar(string thisVar)
		{
			
			if (thisVar== "") {g = null; return g;}
		
			Char[] trimChar = new Char[]{'&','%','.'};
			string trimmed = "";
			if (thisVar[0] == '&')
			{
				trimmed = thisVar.Trim(trimChar);
				
				GuiDict t = temporary[trimmed];
				
				if (t != null)
				{
					linkH.GetTarget(thisVar);
			
					g = t;
					//Debug.Log("reuse temp: "+g.key);
					//Debug.Log(g[0]?.key);
				
					return g;
				}
			}
			
			linkH.HandleLink(thisVar.TrimEnd(new char[]{'%','&'}), false);
			g =linkH.output;
			
			
			if (g != null && thisVar[0] == '&')
			{
				//Debug.Log("new temp: "+trimmed);
				//Debug.Log(g[0]?.key);
				temporary.Add(trimmed, g[0]?.key);
				g = temporary[trimmed];
			}
			return g;
		}
		
		protected char savec  ;
		protected BText saves ;
		protected int savepos ;
		protected int saveline;
		
		public void SaveState(){
			savec = c;
			saves = s;
			savepos = charPos;
			saveline = lineCount;
		}
		public void NewState(BText newstate){
			c = ' ';
			s = newstate;
			charPos = 0;
			lineCount = 0;
		}
		public void RevertState(){
			
			c = savec;
			s = saves;
			charPos = savepos;
			lineCount = saveline;
		}
		public string Sum(string n)
		{
			SaveState();
			NewState(new BText(n));
			string r = _Sum();
			RevertState();
			return r;
		}
		public string Sum()
		{
			return _Sum();
		}
		public string _Sum()
		{
			BText right = new BText();
			BText total = new BText();
			SetAccessor(2);
			
			
			string sright;	
			string wit;
			// assuming it's a string
			
			s.Append(' ');
			
			sright = NextWord();
			
			// early exit
			if (lineBroken) return null;
			
			if (sright == "")
				return "";
				
			wit = WhatIsThis(sright);
			
			//1 Check for single
			//Debug.Log(sright);
			if (wit == "variable")
			{
				UseVar(sright);
				
				if (g != null)
				{
				
					if (linkH.target == VARIABLE_ASSIGNMENT)
						sright = g.key;
					else
						sright = g[0]?.key;
					wit = WhatIsThis(sright);
					//Debug.Log("1:right:"+sright);
					//Debug.Log(wit);
				}
				else
					sright = "";
			}
			
			
			if (!(charPos < s.Length && !lineBroken && !";,".Contains(c)))
			{
				if (sright.Length == 0) return "";
				
				if (sright[0] == '\'') return(sright);
				
				return sright.Trim('"');
			}
			if (wit != "null")
				total.Append(sright);
			
			// 2 variable unpacking
			while (charPos < s.Length && !lineBroken && !";,".Contains(c))
			{	
				sright = NextWord();
				wit = WhatIsThis(sright);
				//Debug.Log("2:right:"+sright);
				//Debug.Log("2:wit:"+wit);
				if (wit == "variable")
				{
					UseVar(sright);
					
					if (g != null)
					{
					
						if (linkH.target == VARIABLE_ASSIGNMENT)
							sright = g.key;
						else
							sright = g[0]?.key;
						wit = WhatIsThis(sright);
						//Debug.Log("2:right:"+sright);
						//Debug.Log("2:wit:"+wit);
					}
					else
						sright = "";
				}
				
				if (wit != "null")
					total.Append(sright);
						
				
			}
			
			BText gstring = new BText();
			BText gnumber = new BText();
			
			NewState(total);
			s = total;
			
			s.Append(' ');
			
			
			
			int hasNumber = 0;
			string num;
			string operation = "";
			
			
			// 3 operations
			while (charPos < s.Length )
			{	
				//Debug.Log(sright);
				//Debug.Log(wit);
				sright = NextWordMath();
				
				wit = WhatIsThis(sright);
				
				
				if (wit == "operation")
				{
					operation = sright;
					
				}
				else if (wit == "number")
				{
					
					if (hasNumber == 0 && operation == "+")
					{
						gstring.Append(operation);
					}
					if (hasNumber == 0 && operation == "*")
					{
						// slice operation
						float f = (float)sright.asNumber();
						int fi = (int)f;
						int fin = 0;
						f = f - fi;
						if (f != 0)
							fin = (int)(gstring.Length * f) ;
						
						if (fi > 0)
						for (int i = 0 ; i < fi ; i++)
						
							gnumber.Append(gstring.ToString());
						else
						for (int i = -1 ; i > fi*gstring.Length - 1 ; i--)
						{
							gnumber.Append(""+gstring[(int)Mathf.Repeat(i, gstring.Length)]);
						}
					
						if (fin > 0)
							gnumber.Append(gstring.ToString(0, fin ));
						else if (fin < 0)
						for (int i = -1 ; i > fin -1 ; i--)
						{
							gnumber.Append(""+gstring[(int)Mathf.Repeat(i, gstring.Length)]);
						}
					
						gstring.Clear();
						gstring.Append(gnumber.ToString());
						gnumber.Clear();
						
					}
					else
					{	
						if (hasNumber > 0) gnumber.Append(operation);
						hasNumber++;
						gnumber.Append(sright);
					}
					operation = "";
				}
				else //string
				{
					if (hasNumber > 1)
					{
						// dmath
						num = gnumber.ToString();
						Math(ref num);
						gstring.Append(num);
						gstring.Append(operation);
						
					}
					else if (hasNumber > 0)
					{
						gstring.Append(gnumber.ToString());
						gstring.Append(operation);
						
					}
					
					if (operation == "=")
					{
						// maybe save it?
						if (gstring.ToString() == sright.Trim(new char[]{'\'','"'}))
						{
							return "True";
						}
						else return "False";
					}
					else if (operation == "+")
					{
						// maybe save it?
						if (sright.Length > 0)
						{
							if (sright == "'\"")
								gstring.Append('"');
							else
								gstring.Append(sright.Trim(new char[]{'\'','"'}));
						}
						
					}
					else if (operation == "-")
					{
						// maybe save it?
						if (sright.Length > 0)
						{
							gstring.Remove(sright.Trim(new char[]{'\'','"'}));
						}
						
					}
					else if (sright.Length > 0)
					{
						if (sright == "'\"")
							gstring.Append('"');
						else
							gstring.Append(sright.Trim(new char[]{'\'','"'}));
						
					}
					
					operation = "";
					hasNumber = 0;
					gnumber.Clear();
					
				}
					
			}
				
			if (hasNumber > 1)
			{
				// dmath
				num = gnumber.ToString();
				Math(ref num);
				gstring.Append(num);
			}
			else if (hasNumber > 0)
			{
				gstring.Append(gnumber.ToString());
			}
			// split right side into parenthesis etc
			
			
			return gstring.ToString();
		}
		
	}
	public class GuiRule {
	
		
/*

Extend this from 
	protected override Run() {type your code here}
	
See GuiMod for example.	
	
	INHERITANCE & RULESETS
	Define a class
		Add(string key, GuiRuleDelegate)
		
	This is helpful for reusing the class.
	
	Rules can be combined static or instanced with 
		Start, Clear, or Restart
		Mix(List<GuiRule> or GuiRule, bool overwrite = true) 
			** if false any replacement is discarded
		
		(optional) Demix(string key)
		(optional) Remix(string key, GuiRuleDelegate), overwrite one key
		
		call Mix/Demix multiple times if necessary
		
		Finish or Apply(...)
		
	when finished use
		Apply() instanced
	
	or staticly,
		Apply(Transform, GuiDict)
	
	mix additive?
	Hard-coding interpolation is the only easy way.
	
	Effects? Timing?
	GuiMod script, any effects this can't handle are most likely taken care of there.

*/

		public static GuiRule defaultRule => _defaultRule;
		
		protected static GuiRule _defaultRule = new DefaultRule();
		
		// a set of referenced rules, to be combined
		public static List<GuiRule> ruleCombo = new List<GuiRule>();
		
		
		
		// human readable definitions of the rules 
		public virtual string description => "See Default";
		

		// a local dictionary of methods, to be referenced for rule sets
		
		public static Dictionary<string, GuiRuleDelegate> rules = new Dictionary<string, GuiRuleDelegate>();
		
		public Dictionary<string, GuiRuleDelegate> localRules = new Dictionary<string, GuiRuleDelegate>();
		
		
		
		public Dictionary<string, GuiRuleDelegate> localMethods = new Dictionary<string, GuiRuleDelegate>(){
		};
		
		public Dictionary<string, GuiRuleDelegate> validateMethods = new Dictionary<string, GuiRuleDelegate>(){
		};
		
		
		public Dictionary<string, GuiRuleDelegate> localKeywords = new Dictionary<string, GuiRuleDelegate>(){
		};
		
		public Dictionary<string, GuiRuleDelegate> validateKeywords = new Dictionary<string, GuiRuleDelegate>(){
		};
		
		
		protected static Transform selected;
		protected static GuiDict dict;
		protected static GuiDict mod;
		
		protected static GuiDict param => mod;
		
		
		
		public static void BuildWindowStatic(Transform s, GuiDict d)
			=> defaultRule.BuildWindow(s, d);
			
		// override this with a set of new rules
		protected virtual void BuildWindow()
			=> defaultRule.BuildWindow();
		
		// mitigating boilerplate code
		public void BuildWindow(Transform s, GuiDict d)
		{
			if (s == null || d == null) return;
			mod = null;
			selected = s; dict = d;
			BuildWindow();
			
			selected = null; dict = null; 
		}
			
		
		public static void RunStatic(Transform s, GuiDict d)
			=> defaultRule.Run(s, d);
			
		// override this with a set of new rules
		protected virtual void Run()
			=> defaultRule.Run();
		
		// mitigating boilerplate code
		public void Run(Transform s, GuiDict d)
		{
			if (s == null || d == null) return;
			mod = null;
			selected = s; dict = d;
			Run();
			
			selected = null; dict = null; 
		}
			
		// change to local rules?
		
		public void ReplaceRule(string key, GuiRuleDelegate method){
			
			if (localRules.ContainsKey(key)) 
				localRules[key] = method;
			else localRules.Add(key, method) ;	
			
		}
		public void AddRule(string key, GuiRuleDelegate method) => localRules.Add(key, method) ;

		public void ReplaceMethod(string key, GuiRuleDelegate method){
			
			if (localMethods.ContainsKey(key)) 
				localMethods[key] = method;
			else localMethods.Add(key, method) ;	
			
		}
		public void ReplaceKeyword(string key, GuiRuleDelegate keyword){
			
			if (localKeywords.ContainsKey(key)) 
				localKeywords[key] = keyword;
			else localKeywords.Add(key, keyword) ;	
			
		}
		public void AddKeyword(string key, GuiRuleDelegate validate = null, GuiRuleDelegate execute = null) {
			validateKeywords.Add(key, validate) ;
			localKeywords.Add(key, execute) ;
		}
		public void AddMethod(string key, GuiRuleDelegate validate = null, GuiRuleDelegate execute = null) {
			validateMethods.Add(key, validate) ;
			localMethods.Add(key, execute) ;
		}
			
			
		

		public void RemoveRule(string key) => localRules.Remove(key);
		
		public static void Finish(Transform t, GuiDict d) => Apply(t, d);
		public static void Apply(Transform t, GuiDict d){
			selected = t; dict = d;
			_Apply();
			
			selected = null; dict = null;
		}
		public void Finish() => _Apply();
		public void Apply() => _Apply();
		
		public void UseLocalMethod(string methodName){
			
			// Does a scripted method, dependent on the calling script
			
			GuiDict g = dict[METHOD];
			
			if (g == null) return;
			
			mod = g[methodName];
		
			// calls method and its lambda body
				
			// run a custom method
			if (localMethods.ContainsKey(mod.key)) 
			
			for (int mi = 0 ; mi < mod.Count ; mi++)
			{
				method.index = mi;
				method.Load(mod);
				localMethods[mod.key].Invoke();
			}
			
			
			
			// run a lambda of method?
			RunMethod();
			
			
			
		}
		public void UseLocalKeyword(string key){
			
			// run a method based on keyword name
			if (localKeywords[key] != null) 
				localKeywords[key].Invoke();
			
		}
		
		public GuiDict vars => dict[VARIABLE_OBJECT] ?? dict.Fetch(VARIABLE_OBJECT);
		public GuiDict globalVars => dict.Root[VARIABLE_OBJECT];
		public virtual void RunLambdaType()
		{
			if (mod.Count < 1) return;
			
			for (int i = 0 ; i < mod.Count ; i++)
			{
				method.index = i;
				RunMethod();
			}
			
			method.index = 0;
		}

		public static LinkHandler linkH = new LinkHandler();
		protected GuiDict temporary => method.temporary;
		protected GuiDict fromDirectory { get => linkH.fromDirectory; set => linkH.fromDirectory = value;}
		
		public GuiDict DoRealtimeMethod(GuiRuleDelegate rdelegate, string input){
			
			string[] asave = method.input;
			
			method.ParseHead(input);
			rdelegate.Invoke();
			
			method.input = asave;
			return method.g;
		}
		public GuiDict DoRealtimeMethod(string s, string input){
			
			string[] asave = method.input;
			
			method.ParseHead(input);
			localMethods[s].Invoke();
			
			method.input = asave;
			return method.g;
		}
		public GuiDict DoRealtimeMethod(string s){
			
			return DoRealtimeMethod(s, method.GetParenthesis());
		}
		
		public void RunMethod()
		{
			// sequential comparisons and assignments
			method.Load(mod);
			
			string[] p = method.input;
			
			// useful for validating ?
			
			
			//fromDirectory = mod.esc(); //@..
			temporary.Clear();
			
			string s = "";
			string n()=>s=method.Next();
				
			string sleft ;
			
			string keyTarget = "";
			
			DataTable table = new DataTable();
		
			GuiDict g;
			GuiDict vars;
			
			n();
			while (method.running && s != method.END)
			{
				if ( s == ":" || s == "else" || s == "else:")
				// skip the else and all following
				while (method.running && s != method.END && s != "[")
					n();
					
					
					
				// evaluate, new line?
				while (s == "[")
				{
				
					method.Peek();
					bool success = false;
					if (method.c == '(')
					{
						GuiDict conditionalList = null;
						method.RevertState();
						n();
						if (localMethods.ContainsKey(s))
						{
							conditionalList = DoRealtimeMethod(s);
							//Debug.Log("conditional method: "+conditionalList?.key);
						}
						else
						{
							Debug.Log("no method: "+s);
						}
						
						if (!(conditionalList?.key == null 
						|| conditionalList.key == "False" 
						|| conditionalList.key == "" 
						|| conditionalList.key == "0")) 
							success = true;
							
						n();
					}
					else
					{
						method.RevertState();
						s = method.ConditionalBracket();
						success = (s == "True");
					}
					n();
					
					//Debug.Log(success);
					//absorb; next word
					//Debug.Log(s);
					
					
					
					if (!success)
					{
						// push to next OR or word
						while (s == "[")
						{
							method.SkipConditionalBracket();
							//Gather
							n();
							
						}
							
						// OR corrects the fail
						if ("||".Contains(s))
						{
							n(); // push left brace in
							continue;
						} 
						
						
						// fail skips the statement
						while (method.running && s != method.END && !( s == ":" || s == "else" || s == "else:" || s == "["))
							n(); // push to the next word or something
						
						// lose, must skip the else
						while (method.running && s != method.END && ( s == ":" || s == "else" || s == "else:"))
							n();
						
					}
					// success, skip the rest
					else if ("||".Contains(s))
					{
						n(); // push left brace, or the next word in
						while ("||[".Contains(s))
						{
							//Gather
							method.SkipConditionalBracket();
							//absorb; next word
							n();
						}
					}
					
					if (success)
					if ( s == ":" || s == "else" || s == "else:")
					// skip the else and all following
					while (method.running && s != method.END && s != "[")
						n();
				}
				
				if (!method.running || s.ToLower() == "exit" || s == method.END) 
				
					return;
				
				
				// left bracing item
				if (s.Contains("{")) 
				{
					
					Debug.Log("ACTION:"+s);
					n();
					continue;
				}
				
				// calling another lambda
				if (method.c == '(') 
				{
					if (localMethods.ContainsKey(s))
					{
						DoRealtimeMethod(s);
					}
					else
					{
						Debug.Log("unknown method"+s);
					
					}
					
					n();
					continue; // idk methods
				}
				if (s[0] == '@')
				{
					
					bool relative = (s == "@@");
					
					n();
					
					if (s != "")
					{
						
						method.ParseAccessorLeft();
						s = method.word.ToString();
						
						if (relative)
							linkH.HandleLink(fromDirectory, s, false);
						else
							linkH.HandleLink(mod, s, false);
						
						
						if (linkH.output != null) fromDirectory = linkH.output;
						
					}
						n();
					
					
					continue; // idk methods
				}
				
				sleft = s;
				method.UseVar(sleft);
				g = method.g;
				keyTarget = linkH.target;
				
				// g must be a variable?
				n();
				
				if (g == null) 
				{
					// skip past this
					Debug.Log("null skip (ref):" +sleft);
					
					method.NextLine();
				
					n();	
					continue;
				}
				// after all conditions are met and s != '['
				
				// assignment
				
				vars = fromDirectory.GetLocalVars();
				GuiDict parsedList = null;
				string dothis = "";
				string increment = "";
				switch (s)
				{
					// could be a sum method?
					case "=":
					//{
						if (method.lineBroken)
						{
							g.Clear();
							break;
						}
						s = method.Peek();
						if (method.c == '(') 
						{
							n();
							if (localMethods.ContainsKey(s))
							{
								parsedList = DoRealtimeMethod(s);
							}
							else
							{
								Debug.Log("no method: "+s);
								break;
							}
						}
						else if (method.c == ',')
						{
							//n();
							
							BText bt = new BText();
							
							bt.Append(method.Sum(method.NextChunk()));
							
							while (method.c == ',')
							{
								bt.Append(',');
								bt.Append(method.Sum(method.NextChunk()));
							}
							parsedList = DoRealtimeMethod(MergeList, bt.ToString());
							
						}
						else
						{
							s = method.NextChunk();
							
							parsedList = new GuiDict(){key="total"};
							s = method.Sum(s);
							
							parsedList.Add(s);
							
						}
						
						// = sum method
						
						
						if (keyTarget == VARIABLE_ASSIGNMENT)
						{
							// replace all? insert?
							g.key = parsedList[0].key;
							
							g = g.parent;
						}
						else if (keyTarget == KEY_FILL)
						{
							// replace all? insert?
							
							if (parsedList.Count > 1)
							for (int i = 0 ; i < g.Count && i < parsedList.Count 
							; i++)
								g[i].key = parsedList[i].key;
								
							else
							for (int i = 0 ; i < g.Count ; i++)
								g[i].key = parsedList[0].key;
							
						}
						else if (g.key == KEY_ALL)
						{
							// replace all? insert?
							
							for (int i = 0 ; i < g.Count ; i++)
							{
								g[i] = parsedList.Clone();
								
								g[i].SetParent(g);
								
							}
						}
						else 
						{
							
							g.Clear();
							for (int i = 0 ; i < parsedList.Count ; i++)
								g.Include(parsedList[i].key);
							
						}
						
						break;
					//}
					
					case "+=":
					
					//{
						if (method.lineBroken)
						{
							g.Clear();
							break;
						}
						s = method.Peek();
						
						
						if (method.c == '(') 
						{
							n();
							if (localMethods.ContainsKey(s))
							{
								parsedList = DoRealtimeMethod(s);
							}
							else
							{
								Debug.Log("no method: "+s);
								break;
							}
						}
						else if (method.c == ',')
						{
							//n();
							
							BText bt = new BText();
							
							bt.Append(method.Sum(method.NextChunk()));
							
							while (method.c == ',')
							{
								bt.Append(',');
								bt.Append(method.Sum(method.NextChunk()));
							}
							parsedList = DoRealtimeMethod(MergeList, bt.ToString());
							
						}
						else
						{
							s = method.NextChunk();
							
							parsedList = new GuiDict(){key="total"};
							s = method.Sum(s);
							
							parsedList.Add(s);
							
						}
						
						// = sum method
						
						
						if (keyTarget == VARIABLE_ASSIGNMENT)
						{
							// replace all? insert?
							g.key = method.Sum(g.key + "+" +parsedList[0].key);
							
							g = g.parent;
						}
						else if (keyTarget == KEY_FILL)
						{
							// replace all? insert?
							
							if (parsedList.Count > 1)
							for (int i = 0 ; i < g.Count && i < parsedList.Count 
							; i++)
								g[i].key = method.Sum(g[i].key + "+" +parsedList[i].key);
								
							else
							for (int i = 0 ; i < g.Count ; i++)
								g[i].key = method.Sum(g[i].key + "+" +parsedList[0].key);
							
						}
						else if (g.key == KEY_ALL)
						{
							// replace all? insert?
							
							if (parsedList.Count > 1)
							for (int i = 0 ; i < g.Count ; i++)
								g[i].IncludeList(parsedList.Clone());
								
							else
							for (int i = 0 ; i < g.Count ; i++)
								g[i].key = method.Sum(g[i].key + "+" +parsedList[0].key);
							
						}
						else 
						{

							if (parsedList.Count > 1)
								g.IncludeList(parsedList.Clone());
								
							else
							{
								if (g.Count < 1) g.Add("0");
								for (int i = 0 ; i < g.Count ; i++)
									g[i].key = method.Sum(g[i].key + "+" +parsedList[0].key);
							}
							
						}
						break;
					//}
					
					case "-=":
					
					//{
						if (method.lineBroken)
						{
							g.Clear();
							break;
						}
						s = method.Peek();
						
						
						if (method.c == '(') 
						{
							n();
							if (localMethods.ContainsKey(s))
							{
								parsedList = DoRealtimeMethod(s);
							}
							else
							{
								Debug.Log("no method: "+s);
								break;
							}
						}
						else if (method.c == ',')
						{
							//n();
							
							BText bt = new BText();
							
							bt.Append(method.Sum(method.NextChunk()));
							
							while (method.c == ',')
							{
								bt.Append(',');
								bt.Append(method.Sum(method.NextChunk()));
							}
							parsedList = DoRealtimeMethod(MergeList, bt.ToString());
							
						}
						else
						{
							s = method.NextChunk();
							
							parsedList = new GuiDict(){key="total"};
							s = method.Sum(s);
							
							parsedList.Add(s);
							
						}
						
						// = sum method
						
						
						if (keyTarget == VARIABLE_ASSIGNMENT)
						{
							// replace all? insert?
							g.key = method.Sum(g.key + "-" +parsedList[0].key);
							
							g = g.parent;
						}
						else if (keyTarget == KEY_FILL)
						{
							// replace all? insert?
							if (parsedList.Count > 1)
							for (int i = 0 ; i < g.Count && i < parsedList.Count 
							; i++)
								g[i].key = method.Sum(g[i].key + "-" +parsedList[i].key);
								
							else
							for (int i = 0 ; i < g.Count ; i++)
								g[i].key = method.Sum(g[i].key + "-" +parsedList[0].key);
							
						}
						else if (g.key == KEY_ALL)
						{
							// replace all? insert?
							
							if (parsedList.Count > 1)
							for (int i = 0 ; i < g.Count ; i++)
								g[i].RemoveList(parsedList.Clone());
								
							else
							for (int i = 0 ; i < g.Count ; i++)
								g[i].key = method.Sum(g[i].key + "-" +parsedList[0].key);
							
						}
						else 
						{

							if (parsedList.Count > 1)
								g.RemoveList(parsedList.Clone());
								
							else
							{
								if (g.Count < 1) g.Add("0");
								for (int i = 0 ; i < g.Count ; i++)
									g[i].key = method.Sum(g[i].key + "-" +parsedList[0].key);
							}
							
						}
						break;
					//}
					
						
					case "++":
						increment = "+1";
						goto case "+|-";
					case "--":
						increment = "-1";
						goto case "+|-";
					case "+|-":
						if (keyTarget == VARIABLE_ASSIGNMENT)
						{
							
								
							dothis = g.key + increment;
							if (method.Math(ref dothis))
								g.key = dothis;
							
							g = g.parent;
						}
						else if (keyTarget == KEY_FILL 
						|| g.key == KEY_ALL)
						{
							// replace all? insert?
							
							for (int i = 0 ; i < g.Count ; i++)
							{
								
								dothis = g[i].key + increment;
								if (method.Math(ref dothis))
									g[i].key = dothis;
								
							}
						}
						else
						{
							dothis = g[0].key + dothis;
							if (method.Math(ref dothis))
								g[0].key = dothis;
						}
						break;
					default:
					
						
						Debug.Log("unknown operation:" +s);
						break;
					
				}
				
				n();
					
				// end
				
			}
		}	
		
		public void UseLambda(GuiDict m){
			// execute method
			if (m == null) return;
			mod = m;
			
			dict = m.parent.parent;
			
			RunMethod();
			
			
			
		}
		public void RunAllLambda(GuiDict g){
			// execute all methods declared in scope
			
			if (g == null) return;
			
			foreach (GuiDict m in g.dict)
			{
				mod = m;
				RunLambdaType();
			}
			
			
		}
		protected void UseValidateKeyword(string key){
			
			if (validateKeywords.ContainsKey(key))
			{
				mod = dict;
				validateKeywords[key].Invoke();
			}
				
		}
		protected void UseValidateKeywords(){
			
			GuiDict g = dict.Hierarchy;
			GuiDict m ;
			
			if (g == null) return;
			
			for (int i = 0 ; i < g.Count ; i++)
			{
				m = g[i];
				if (m.parent.key == METHOD) continue;
				if (validateKeywords.ContainsKey(m.key)) 
				{
					mod = m;
					validateKeywords[m.key].Invoke();
				}
			
			}
			
			
		}
		protected void UseValidateMethods(){
			
			GuiDict g = dict.FindAll(METHOD, true);
			if (g?.dict == null) return;
			
			GuiDict m = new GuiDict(){key = "m"};
			foreach (GuiDict allmethods in g.dict)
			{
				for (int i = 0 ; i < allmethods.Count ; i++)
				{
					
					m.Add(allmethods[i]);
					
				}
				
			}
			
			for (int i = 0 ; i < m.Count ; i++)
			if (validateMethods.ContainsKey(m[i].key)) 
			{
				mod = m[i];
				
				for (int mi = 0 ; mi < mod.Count ; mi++)
				{
					method.index = mi;
					method.Load(mod);
					validateMethods[mod.key].Invoke();
				}
			
			
			}
			
			
		}
		protected static void _Apply(){
			// execute all rules contained in mods.dict
			GuiDict g = dict["mods"];
			if (g != null)
			foreach (GuiDict m in g.dict)
			if (rules.ContainsKey(m.key) ) 
			{
				mod = m;
				rules[m.key].Invoke();
			}
			
			
		}
		public static M method = new M();
		
		public static M realtimeMethod = new M();
		
		
		
		
		
		public static void Clear() 	 => Start();
		public static void Restart() => Start();
		public static void Start(){ rules.Clear(); }
		
		public static void Mix(List<GuiRule> g, bool overwrite = true){
			
			foreach (GuiRule r in g)
			{
				Mix(r, overwrite);
			}
		}
		public static void Mix(GuiRule g, bool overwrite = true){
			// last rule is preferred
			foreach (string key in g.localRules.Keys)
			if (rules.ContainsKey(key) )
			{
				if (overwrite)
					rules[key] = g.localRules[key];
			}
			else
				rules.Add(key, g.localRules[key]);
			
			
		}
		
		
		public void Demix(string key) => rules.Remove(key);
		// change to mixed rules?
		public void Remix(string key, GuiRuleDelegate method){
			
			if (rules.ContainsKey(key)) 
				rules[key] = method;
			else rules.Add(key, method) ;	
			
		}
		
		public void MergeSum(){
			
			
			string[] p = method.input;
			
			int len = p.Length;
			
			if (len < 1) 
			{
				method.g = new GuiDict(){key = "Null"};
				return ;
			}
			BText bt = new BText();
			bt.Append(method.Sum(p[0]));
			for (int i = 1 ; i < len ; i++)
			{
				bt.Append(',');
				bt.Append(method.Sum(p[i]));
			}
			
			method.g = DoRealtimeMethod(MergeList, bt.ToString());
			
		}
		public void MergeRaw(){
			
			string[] p = method.input;
			
			int len = p.Length;
			if (len <= 0) return; // doesn't do anything
			
			GuiDict total = new GuiDict(){key = "total"};
			
			
			
			for (int i = 0 ; i < len ; i++)
			
				total.Include(p[i]);
			
			
			method.g = total;
		}
		
		public void MergeList(){
			
			string[] p = method.input;
			string wit ;
			
			
			
			int len = p.Length;
			if (len <= 0) return; // doesn't do anything
			GuiDict r ;
			GuiDict total = new GuiDict(){key = "total"};
			
			
			
			for (int i = 0 ; i < len ; i++)
			{
				wit = method.WhatIsThis(p[i]);
				if (wit == "variable")
				{
					method.UseVar(p[i]);
					r = method.g;
					if (r == null)
						total.Include("");
					else
						total.Merge(r);
				}
			
				else
				{
					total.Include(p[i]);
					continue; 
				}
			}
			
			method.g = total;
		}
		
	}
	
	public class DefaultRule : GuiRule {
		/*
			Default rulebook for GUI
			meant to be inheritable and overriden
			
			key:
			* = button
		
		*/
			
		public override string description => "Basic window.";
		
		public DataTable doMath = new DataTable();
		
		public DefaultRule(){
			// it's in a button or it is a button, which is it?
			AddRule("*", Inside_aButton);
			AddRule("U", ReadUp);
			
			
			// Method(key, call immediately, called with mod);
			AddMethod("Replace", ReplaceKey);
			AddMethod("OnClick", ValidateOnClick, null);
			AddMethod("DEBUG", Debuga);
			
			AddKeyword("MATH", MathVar);
			AddMethod("MATH", MathMethod, MathHead);
			AddMethod("MathString", null, MathString);
			AddMethod("Merge", null, MergeList);
			AddMethod("MergeSum", null, MergeSum);
			AddMethod("GetTrue", null, GetTrue);
			
		}
		protected void GetTrue(){
			
			method.g = new GuiDict(){key = "True"};
		}
		
		// This is called each time a component's being reinterpreted
		protected override void Run()
		{
			// Method parameter validation, if necessary
			UseValidateKeywords();
			UseValidateMethods();
			Start();
			Mix(this);
			// keep mixing
			Apply();
			
		}
		
		protected void Math(ref string s){
			method.Math(ref s);
			
		}
		protected void MathVar(){
			
			int indexof = mod.parent.GetIndexFromDict(mod);
			GuiDict m = mod.parent[indexof+1];
			
			Math(ref m._key);
			
			GuiDict allvars = m.Hierarchy;
			
			if (allvars != null)
			for (int i = 0 ; i < allvars.Count ; i++)
			
				Math(ref allvars[i]._key);
			
			
				
			
		}
		
		protected void MathString(){
			
			string[] p = method.input;
			if (p.Length < 1) return;
			BText b = new BText(method.Sum(p[0]));
			GuiDict g = new GuiDict(){key = "Math"};
			
			for (int i = 1 ; i < p.Length ; i++)
			{
				b.Append(',');
				b.Append(method.Sum(p[i]));
			}
			g.Add(b.ToString());
			method.g = g;
		}
		protected void MathHead(){
			
			string[] p = method.input;
			if (p.Length < 1) return;
			GuiDict g = new GuiDict(){key = "Math"};
			g.Add(method.Sum(p[0]));
			for (int i = 1 ; i < p.Length ; i++)
			{
				g.Include(method.Sum(p[i]));
			}
			method.g = g;
		}
		
		protected void MathMethod(){
			// head?
			
			// body?
			RunMethod();
		}
		public virtual void ValidateOnClick(){
			string[] p = method.input;
			if (p.Length == 1)
			{
				ReplaceMethod("OnClick",OnClick);
			}
			else
				Debug.Log("wrong number of parameters for" +method.name+"(mouse)");
			
		}
		
		public void Debuga()
		{
			Debug.Log("Debug Method Running");
			RunMethod();
		}
		public virtual void OnClick()
		{
			Debug.Log("Click");
			RunMethod();
		}
			
		// Method interpretation
		// var = method.Next()
		public void ReplaceKey(){
			// method is a class mod == method.mod; You're handling GuiDict.
			
			//method.name
			//method.head
			//method.body
			
			// This is default
			// method.ParseHead();
			
			string[] p = method.input;
			string target = p[0];
			
			string newValue = p[1];
			
			// skip extra variables
			// method.ParseBody();
			
			
			// this calls esc, then FindAll, recursive true
			GuiDict vars = mod.esc().FindAll(target, true);
			
			// replace all the keys, although that might not be exactly right yet
			for (int i = 0 ; i < vars.Count ; i++)
			{
				vars[i].key = newValue;
			}
			
		}
		
		// Change direction of line feed
		public void ReadUp(){
			//U
			
		}
		
		// This is...
		public void Inside_aButton()
		{
			
			
			// this is interpreting button position in parent
			// it could just as easily work the other way, moving the child
			
			//if (mod.parent.key == "")
				
			//hacky solutions may be common at this level. is this fine?
			Debug.Log("Mods"+selected.name );
			if (selected.name != WINDOW_ASCII) return;
		
			float x_frac, y_frac ;
			float x, y, w, h ;
			x = float.Parse(mod.i(1).key);
			y = float.Parse(mod.i(2).key);
			
			w = float.Parse(mod.i(3).key);
			h = float.Parse(mod.i(4).key);
			
			if (x == 0)
			x_frac = (x ) / (w - 2);
			else
			x_frac = (x - 1) / (w - 2);
		
			if (y  < h - 1)
			y_frac = (y ) / (h - 2);
			else 
			y_frac = (y - 1 ) / (h - 2);
			
			// why the parent?
			RectTransform t = selected.parent as RectTransform;
			
			RectTransform tParent = t.parent as RectTransform;
			
			t.localPosition = new Vector2(-(x_frac)  * t.rect.width , ( y_frac)  * t.rect.height );
			
			if (y == 0)
			{
				// above
				t.localPosition = new Vector2(t.localPosition.x, t.localPosition.y - tParent.rect.height / 2f);
			}
			else if (y == h - 1)
			{
				// below
				t.localPosition = new Vector2(t.localPosition.x, t.localPosition.y + tParent.rect.height / 2f);
			}	
			else if (x == 0)
			{
				// left
				t.localPosition = new Vector2(t.localPosition.x + tParent.rect.width / 2f, t.localPosition.y );
			}
			else if (x == w - 1)
			{
				// right
				t.localPosition = new Vector2(t.localPosition.x - tParent.rect.width / 2f, t.localPosition.y );
			}
			// else this button isn't on the border
	
		
		
		}
		

		protected void BuildWindowMod(){
			
			GuiDict vdict = dict;
			string key = dict.key;
			
			string strStart = dict[WINDOW_ATLAS]?["start"]?[0].key;
			
			if (strStart== null) return;
			int thisAtlasStart = (int)strStart.asNumber();
			
			strStart = dict["start"]?[0]?.key;
			if (strStart== null) return;
			int thisWordBegin = (int)strStart.asNumber();
			
			//GuiAtlas atlas = bp.ParseWindowFromDict(g);
			bool found = false;
			
			GuiAtlas atlas;
			GuiBlock block = null;
			int wordIndex = 0;
			int atlasIndex;
			BParser bp = BParser.current;
			
			
			for (atlasIndex = 0 ; !found && atlasIndex < bp.worldAtlas.Count ; atlasIndex++)
			{
				
				atlas = bp.worldAtlas[atlasIndex];
				if (atlas.atlasStart == thisAtlasStart)
				{
					
					for (wordIndex = 0 ; !found && wordIndex < atlas.blocks.Count ; wordIndex++)
					{
						block = atlas.blocks[wordIndex];
						
						if (block.atlasWord.begin == thisWordBegin)
						{
							// found this block
							found = true;
						}
					}
				}
			}
			
			if (!found) return;
			
			//####################### Begin parsing sub-windows
			
			
			BText definition = new BText();
			
			int window_wide, window_high;
			
			/* 
			if (block.atlasWord.wide == 1)
			{
				block.atlasWord.high = window_wide;
			}
			else
				block.atlasWord.high = window_high;
			 */
			 
			float  x_frac, y_frac ;
			
			
			
			// i've decided the default behavior will attempt to expand all windows to touch the screen edges, so no more resizing
			
			List<GuiWindow> windows = block.windows;
			if (windows.Count < 1) return;
			window_wide = block.wide;
			window_high = block.high;
			
			
			block.GenerateMods();
			
			
			// I can put mods into a window here
			List<GuiElement> mods = block.mods;
			
			
			GuiElement m;
			int x, y;
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
					
					
					GuiDict.InstanceStack(vdict, "mods", "*", x+"", y+"", window_wide+"", window_high+"");
					
				
				}
				
				
			}
					
			
		}
		protected override void BuildWindow()
		{
			// this gets called for the window, there are small differences but it could be subject to change
			
			if (dict["apply_var"] == null) 
			{
				BuildWindowMod();
				return;
			}
			string name = dict.key;
			
			//this gets called for every block in the window...
			
			GuiDict vdict = dict;
			dict = dict["apply_var"][0];
			string key = dict.key;
			
			
			if (dict.parent.key == REPLACEMENT)
				dict = dict[0].parent;
			else
				dict = dict.parent;
			
			string strStart = dict[WINDOW_ATLAS]?["start"][0].key;
			if (strStart== null) return;
			
			 
			int thisAtlasStart = (int)strStart.asNumber();
			
			// I could check between atlases
			strStart =dict["start"]?[0].key;
			
			if (strStart== null) return;
			
			int thisWordBegin = (int)strStart.asNumber();
			BParser bp = BParser.current;
			
			
			//GuiAtlas atlas = bp.ParseWindowFromDict(g);
			bool found = false;
			
			GuiAtlas atlas;
			GuiBlock block = null;
			int wordIndex = 0;
			int atlasIndex;
			
			for (atlasIndex = 0 ; !found && atlasIndex < bp.worldAtlas.Count ; atlasIndex++)
			{
				
				atlas = bp.worldAtlas[atlasIndex];
				if (atlas.atlasStart == thisAtlasStart)
				{
					
					for (wordIndex = 0 ; !found && wordIndex < atlas.blocks.Count ; wordIndex++)
					{
						block = atlas.blocks[wordIndex];
						
						if (block.atlasWord.begin == thisWordBegin)
						{
							// found this block
							found = true;
						}
					}
				}
			}
			
			if (!found) return;
			
			//####################### Begin parsing sub-windows
			
			
			BText definition = new BText();
			
			int left, window_wide, window_high;
			
			/* 
			if (block.atlasWord.wide == 1)
			{
				block.atlasWord.high = window_wide;
			}
			else
				block.atlasWord.high = window_high;
			 */
			int  top, bottom, right;
			float w_frac, x_frac, y_frac, h_frac ;
			
			
			
			// i've decided the default behavior will attempt to expand all windows to touch the screen edges, so no more resizing
			GuiWindow eword = null;
			List<GuiWindow> windows = block.windows;
			if (windows.Count < 1) return;
			window_wide = block.wide;
			window_high = block.high;
			
			
			// from dictionary?
			string workingKey = "";
			int duplicate_count = 0;
			int count = 0;
			
			GuiDict vdictp = vdict;
			// todo name hack (2)
			if (name == "__include")
				vdictp = vdict.parent;
			for (int i = 0 ; i < vdictp.Count ; i++)
			{
				workingKey = vdictp[i].key;
				
				//todo, cleaning this would take effort (2)
				if (workingKey == "name" || workingKey == "start" || workingKey == "apply_var" || workingKey == "mods" || workingKey == "" ) continue;
				if (count == selected.GetSiblingIndex())
				{
					break;
				}
				
				count++;
				if (workingKey == key)
					duplicate_count++;
			}
			
			// from guiworld
			
			for (int i = 0 ; i < windows.Count ; i++)
			{
				eword = windows[i];
				
				if ( key == eword.name) 
				{
					if (duplicate_count == 0)
						break;
					duplicate_count--;
				}	
				
			}
			
			if ( key != eword.name) return;
			
			left = eword.left ;
			right = eword.right ; // + 1 for char width, and only after, so it doesn't get eaten
			
			
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
			
			
			
			RectTransform t = selected as RectTransform;
			
			t.offsetMin = Vector2.zero;
			t.offsetMax = Vector2.zero;
			t.anchorMin = new Vector2(x_frac, y_frac);
			t.anchorMax = new Vector2(w_frac, h_frac);
			
			
			block.GenerateMods((GuiWindow)eword);
			// I can put mods into a window here
			List<GuiElement> mods = block.mods;
			
			
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
				/* 
				if (m.ToString().Contains('*'))
				{
					x = m.left;
					y = m.top;
					
					
					
					x_frac = (float)x / window_wide;
					y_frac = 1 - (float)y / window_high;
					
					
					GuiDict.InstanceStack(vdict, "mods", "*", x+"", y+"", window_wide+"", window_high+"");
					
				
				}
				 */
				
			}
					
			
			
		}
		
		
	}
	

} 