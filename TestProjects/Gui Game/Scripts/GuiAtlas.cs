
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GuiGame
{

		
	public class GuiAtlas 
	{
		// staggered lines are treated as a window would be, windows whose contents are in blocks remain in a raw format
	
		// ^var = "" in atlas to change variables in following blocks
		
	
		public List<GuiBlock> blocks = new List<GuiBlock>(); //, 1 line each, all inserted text
		public List<GuiBlock> atlasVariables = new List<GuiBlock>(); //, 1 line each, all inserted text
		public GuiBlock block;
		public int Count  => blocks.Count; 
		public GuiBlock this[int i]  => blocks[i]; 
		
		public string atlasMods = "";
		public string atlasName = "";
		
		public BText world = new BText();	
		public BText allText = new BText();	
		public BText inputText = new BText();	
		public BText procText;	
		public int[] lineStarts = null;
		public int high => lineStarts.Length;
		public int GetLineWidth(int y) {
			if (y == 0 || y + 1 >= high) return 0; 
			return lineStarts[y+1] - lineStarts[y];
		}
		
		public int atlasStart = 0;
		public int charPos = 0;
		public int linePos = 0;
		public int atlasEnd = 0;
		public char c;
		public bool bound_atlas = false;
		
		public BText s { get => procText ; }
		
		public GuiCollision cursor = new GuiCollision();
		//this is a substring containing the atlas, but it is also containing the rest of the document, maybe I shouldn't do it this way
		public void Generate(string inputText){
			procText = new BText(inputText);
			
			charPos = 0;
			
			allText.Clear();
			
			world.Clear(); // world is now more like a outer-world zone, I guess that soon it will be converted into a larger block or biome
			
			
			
			blocks.Clear(); // blocks are like inner- worlds
			block = null;
			
			
			currentWEND = null;
			
			areaText.Clear();
			windowEnds.Clear();
			windowHorizontal.Clear();
			windowSquare.Clear();
			windowVertical.Clear();
			
			
			GetWindowAtlas();
			
			if (c == '}')
				atlasEnd = charPos - 2;
			else atlasEnd = charPos;
			
			lineCount = 0;
			allText.Append(procText.ToString(0, atlasEnd ));
			
			for (int i = 0 ; i < windowEnds.Count ; i++)
			{
				procText = allText;
				currentWEND = windowEnds[i];
				
				//atlas
				//is this a variable?
				
				_ParseWindow();
				
			}
			
			for (int i = 0 ; i < atlasVariables.Count ; i++)
			{
				blocks.Remove(atlasVariables[i]);
			}
			
		}
		protected virtual void GetWindowAtlas(){
			// back up
			
			
			char linechar = ' ';
			
			linePos = 0;
			lineCount = 0;
			charPos = 0;
			int lastRecord = 0;
			List<int> lineList = new List<int>();
			
			bool endLoop = false;
			bool bound_atlas = false;
			bool comment_on = false;
			
			c = ' ';
			while (charPos < s.Length && c.isWhiteSpace())
			{
				c = s[charPos++];
				world.Append(' ');
			}
			// bind atlas? modify it? single window?
			if (c == '^') 
			{
				int savepos = charPos;
				while (charPos < s.Length && !isWindow())
				{	
					c = s[charPos++];
					world.Append(' ');
					if (c == ':')
					{
						bound_atlas = true;
						break;
					}
					//world.Append(c);
				}
				if (bound_atlas)
				{
					atlasName = s.ToString(savepos, charPos-1 - savepos).Trim();
					c = s[charPos++];
					
					while (charPos < s.Length && !c.isNewLine())
					{
						world.Append(' ');
						atlasMods += c;
						c = s[charPos++];
					}
					charPos--;
				}
			}
			
			if (!bound_atlas)
			{
				charPos = 0;
				c = ' ';
				world.Clear();
			}
			lineList.Add(0);
			lastRecord= charPos;
			// populate the world
			while (!endLoop && charPos < s.Length )
			{
			
				c = s[charPos++];
				
				if (c.isNewLine())
				{
					world.Append(c);
					
					
					if (linechar == ' '){
						linechar = c;
						int len = s.Count(linechar);
						//lineStarts = new int[len + 1];
						//lineStarts[0] = 0;
					}
					
					c = s[charPos++];
					if (c.isNewLine())
					while (charPos < s.Length && c.isNewLine())
					{
						world.Append(c);
						if (c != linechar) 
						{
							++lineCount;
							lineList.Add(charPos);
						}
						c = s[charPos++];
					}
					else
					{
						++lineCount;
						lineList.Add(charPos-1);
					}
					linePos = charPos - 1;
					
				}
				
				
					
				if (isWindow())
				{
					lastRecord = charPos ;
					world.Append('W');
					
					ApplyWordEnd();
				}
				//isWindowMod(c) ||
				
				else if (!(bound_atlas || comment_on 
				|| isAtlasMod() || isWindowMod() 
				|| c.isWhiteSpace() || IsIncompleteParsing())) 
				{
					endLoop = true;
					
					charPos = lastRecord + 2;
					for (int i = lineList.Count-1 ; i > 0 ; i--)
					{
						if (lineList[i] >= charPos)
							lineList.RemoveAt(i);
					}
					world.Replace(world.ToString(),world.ToString(0, lastRecord));
					world.Append(' ');
				}
				else if (!IsWithinParseWindow())
				{
					
					if (bound_atlas && c == '~' && s[charPos] == '^') 
					{
						charPos++;
						endLoop = true;
						
						world.Append("  ");
					}
					else 
					{
						if (c == '#') comment_on = !comment_on;
						else if (c == 'X') comment_on = false;
						
						world.Append(c);
					}
				}
				//else if (comment_on)
				//	world.Append(c);
				else
				{
					lastRecord = charPos ;
					world.Append('W');
				}
				
				
			}
			
			
			// get the atlas modifiers
			
			
			lineStarts = lineList.ToArray();
		}
		
		
		
		public string GetReadDirection(){
			string rd = "";
			int len = atlasMods.Length;
			for (int i = 0 ; i < len && rd.Length < 3 ; i++)
			{
				if ("RDLU".Contains(atlasMods[i]))
					rd += atlasMods[i];
				
			}
			return rd;
		}
		
		//todo:
		//partly done: human reading directions LR UD
		//pending: programmed reading direction
		
		protected string ConnectAtlas(AtlasWord wend)
		{
			return RCast(wend);
		}
		
		
		protected string BottomAtlasMod(AtlasWord wend) => BottomAtlasMod(wend.left, wend.bottom);
		protected string BottomAtlasMod(int left, int bottom)
		{
			
			int pos = ToStringPos(left,bottom + 1);
			if (pos < 0) return "";
			
			BText b = new BText();
			
			for (int y = bottom + 2; pos < world.Length ; y++)
			{
				c = world[pos];
				if (!isWindowMod()) break;
				b.Insert(0,c);
				pos = ToStringPos(left,y);
			}
			
			
			return b.ToString();
		}
		protected string BottomAtlasCast(AtlasWord wend) => BottomAtlasCast(wend.left, wend.bottom, wend.right);
		protected string BottomAtlasCast(int left, int bottom, int right){
			
			int y = bottom;
			BText b = new BText();
			string str = YCast(y);
			
			
			for (int x = left ; x < right + 1 && x < str.Length ; x++)
			
				b.Append(BottomAtlasMod(x, y));
			//for (int x = left ; x < right + 1 && x < str.Length ; x++)
			//
			//	b.Insert(0, BottomAtlasMod(x, y));
			
			return b.ToString();
		}

		protected string TopAtlasMod(AtlasWord wend) => TopAtlasMod(wend.left, wend.top);		
		protected string TopAtlasMod(int left, int top)
		{
			int pos;
			pos = ToStringPos(left,top - 1);
			if (pos < 0) return "";
			BText b = new BText();
			
			for (int y = top - 2; 0 < pos ; y--)
			{
				c = world[pos];
				if (!isWindowMod()) break;
				b.Insert(0,c);
				pos = ToStringPos(left,y);
			}
			
			return b.ToString();
		}
		protected string TopAtlasCast(AtlasWord wend) =>
		TopAtlasCast(wend.left, wend.top, wend.right);
		protected string TopAtlasCast(int left, int top, int right)
		{
			
			int y = top;
			if (y < 0) return "";
			BText b = new BText();
			string str = YCast(y);
			
			// 
			for (int x = left ; x < right + 1 && x < str.Length ; x++)
				b.Append(TopAtlasMod(x,y));
			//for (int x = left ; x < right + 1 && x < str.Length ; x++)
			//	b.Insert(0,TopAtlasMod(x,y));
			
			return b.ToString();
		}
		
		protected string LeftAtlasMod(AtlasWord wend) => LeftAtlasMod(wend.left, wend.top);	
		protected string LeftAtlasMod(int left, int top)
		{
			int pos = ToStringPos(left - 1, top);
			if (pos < 0) return "";
			
			int end = ToStringPos(0,top);
			if (end < 0) end = 0;
			
			BText b = new BText();
			for ( ; end < pos ; pos--)
			{
				c = world[pos];
				if (!isWindowMod()) break;
				b.Insert(0,c);
				
			}
			return b.ToString();
		}
		
		protected string LeftAtlasCast(AtlasWord wend) => LeftAtlasCast(wend.left, wend.top, wend.bottom);
		protected string LeftAtlasCast(int left, int top, int bottom)
		{
			BText b = new BText();
			
			for (int y = top ; y < bottom + 1 ; y++)
				b.Append(LeftAtlasMod(left,y));
			
			//for (int y = top ; y < bottom + 1 ; y++)
			//	b.Insert(0,LeftAtlasMod(left,y));
			
			return b.ToString();
		}

		
		protected string RightAtlasMod(AtlasWord wend) => RightAtlasMod(wend.right, wend.top);	
		protected string RightAtlasMod(int right, int top)
		{
			int pos = ToStringPos(right + 1, top);
			if (pos < 0) return "";
			int end = ToStringPos(0,top);
			if ( end < 0) end = world.Length;
			
			BText b = new BText();
			for ( ; end < pos ; pos++)
			{
				c = world[pos];
				if (!isWindowMod()) break;
				b.Insert(0,c);
				
			}
			return b.ToString();
		}
		
		protected string RightAtlasCast(AtlasWord wend) => RightAtlasCast(wend.right, wend.top, wend.bottom);
		protected string RightAtlasCast(int right, int top, int bottom)
		{
			BText b = new BText();
			
			for (int y = top ; y < bottom + 1 ; y++)
				b.Append(RightAtlasMod(right,y));
			// linedirection up
			//for (int y = top ; y < bottom + 1 ; y++)
			//	b.Insert(0,RightAtlasMod(right,y));
			
			return b.ToString();
		}		
		protected string RCast(AtlasWord wend)
		{
			string readDirection = GetReadDirection();
			// suppose it could be RL for switching direction
			char rd = 'R';
			char linedir = 'D';
			if (readDirection.Length >= 1)
			
				linedir = readDirection[0];
			
			if (readDirection.Length >= 2) 
				rd = readDirection[1];
			
			if ("RL".Contains(linedir) && "RL".Contains(rd)) rd = 'D';
				
			if (wend.high == 1)
			{
					
				if (rd == 'R' || linedir == 'R')
				
					// look left
					return LeftAtlasMod(wend);
				
				
				return RightAtlasMod(wend);
				
				
			}
			else if (wend.wide == 1)
			{
				
				if (rd == 'D' || linedir == 'D')
				{
					
					return TopAtlasMod(wend);
				}
				
				return BottomAtlasMod(wend);
			}
			else
			{
				
				// only read direction matters
				if ("UD".Contains(rd) )
				{
						
					if (rd == 'D' || linedir == 'D')
					{
						
						return TopAtlasCast(wend);
					}
					
					return BottomAtlasCast(wend);
				
				}
				else //RL
				{
					if (rd == 'R' || linedir == 'R')
						return LeftAtlasCast(wend);
					
					return RightAtlasCast(wend);
						
				}
			}
			
		
				
			
		}
			
		protected bool IsTouchingInAtlas(AtlasWord wend, string touchlist){
			
			if (wend.high == 1)
			{
				return (touchlist.Contains(world[wend.begin - 1]) 
				|| touchlist.Contains(world[wend.end + 1]));
			}
			else if (wend.wide == 1)
			{
				int pos = ToStringPos(wend.left,wend.top - 1);
				if (pos >= 0 && touchlist.Contains(world[pos])) return true;
				
				pos = ToStringPos(wend.left,wend.bottom + 1);
				if (pos >= 0 && touchlist.Contains(world[pos])) return true;
			}
			else
			{
				// scan rows and cols
				//top
				GuiCollision col;
				if (YCastRight(wend.left - 1, wend.top - 1, out col, touchlist, wend.wide)) return true;
				
				if (YCastRight(wend.left - 1, wend.bottom + 1, out col, touchlist, wend.wide)) return true;
				
				if (XCastDown(wend.right + 1, wend.top - 1, out col, touchlist, wend.high)) return true;
				
				if (XCastDown(wend.left - 1, wend.top - 1, out col, touchlist, wend.high)) return true;
				
			}
			
			return false;
		}
		

		public string ToString(int x){
			// maybe update all text here, to reflect the atlas and blocks
			return allText.ToString();
		}		

		public bool XCastUp(int castx, int ypos, out GuiCollision col, string collisionLayer, int steps = -1)
		{
			
			col = null;
			if (0 > ypos || ypos >= lineStarts.Length) return false;
			
			int endPos = 0;
			// casting downwards
			int pos;
			for (int y = ypos - 1; y >= endPos ; y--)
			{
				pos = ToStringPos(castx, y);
				if (pos < 0) continue;
				if (collisionLayer.Contains(world[pos]))
				{
					col = new GuiCollision{x = castx, y = y, asChar = world[pos]};
					return true;
				}
				steps--;
				if (steps == 0) break;
			}
			
			return false;
		}
		public bool XCastDown(int castx, int ypos, out GuiCollision col, string collisionLayer, int steps = -1)
		{
			col = null;
			if (0 > ypos || ypos >= lineStarts.Length) return false;
				
			int endPos = lineStarts.Length ;
			
			
			// casting downwards
			int pos;
			for (int y = ypos + 1; y < endPos; y++)
			{
				pos = ToStringPos(castx, y);
				if (pos < 0) continue;
				
				if (collisionLayer.Contains(world[pos]))
				{
					col = new GuiCollision{x = castx, y = y, asChar = world[pos]};
					return true;
				}
				steps--;
				if (steps == 0) break;
			}
			
			return false;
		}
		public bool YCastRight(int xpos, int casty,  out GuiCollision col, string collisionLayer, int steps = -1)
		{
			col = null;
			// going to the right
			if (0 > casty || casty >= lineStarts.Length) return false;
			int pos = 0;
			// horizontal line
			int endPos;
			if (casty+1 >= lineStarts.Length) endPos = world.Length;
			else endPos = lineStarts[casty+1] - 1;
			
			for (int i = lineStarts[casty] + xpos + 1 ; i < endPos ; i++)
			{
				pos = i;
				
				if (collisionLayer.Contains(world[pos]))
				{
					
					col = new GuiCollision{x = pos - lineStarts[casty], y = casty, asChar = world[pos]};
					return true;
				}
				steps--;
				if (steps == 0) break;
			}
			
			return false;
		}

		public bool YCastLeft(int xpos, int casty,  out GuiCollision col, string collisionLayer, int steps = -1)
		{
			col = null;
			// going to the right
			if (0 > casty || casty >= lineStarts.Length) return false;
			int pos = 0;
			// horizontal line
			int endPos;
			if (casty - 1 < 0) endPos = 0;
			else endPos = lineStarts[casty] ;
			
			for (int i = lineStarts[casty] + xpos - 1 ; i >= endPos ; i--)
			{
				pos = i;
				
				if (collisionLayer.Contains(world[pos]))
				{
					col = new GuiCollision{x = pos - lineStarts[casty], y = casty, asChar = world[pos]};
					return true;
				}
				steps--;
				if (steps == 0) break;
			}
			
			return false;
		}


		public (int x, int y) ToXY(int pos)
		{
			int high = this.high;
			int y = 0;
			
			int linepos;
			linepos = lineStarts[y++];
			while (y < high && pos >= linepos) 
				linepos = lineStarts[y++];
				
			y--;
			
			int x = pos - lineStarts[y];
			// passing the horizontal row width
			return (x,y);
		}
		
		public bool MoveCursorOnMod(int movex, int movey) => MoveCursorOnMod(ref cursor, movex, movey);
		
		public bool MoveCursorOnMod(ref GuiCollision cursor, int movex, int movey){
			int x = cursor.x + movex;
			int y = cursor.y + movey;
			int pos = ToStringPos(x, y);
			
			if (pos < 0) return false;
			char c = world[pos];
			if (!isWindowMod(c)) return false;
			
			cursor.asChar = c;
			cursor.x = x;
			cursor.y = y;
			return true;
		}
		
		public bool TapCursor(int x, int y){
			int pos = ToStringPos(x, y);
			
			if (pos < 0) return false;
			
			cursor.asChar = world[pos];
			
			return true;
		}
		public void SetCursor(int x, int y) 
		{
			if (x < 0 ) x = 0;
			if (y < 0 ) y = 0;
			int pos = ToStringPos(x, y);
			int offsetx = x;
			if (pos == -2)
			{
				x = 0;
				pos = ToStringPos(x, y);
			}
			if (pos == -4) 
			{
				y = high -1;
				pos = ToStringPos(x, y);
			}
			cursor.pos = (offsetx,y);
		
		}
		
		public bool TapCursor(){
			
			int pos = ToStringPos(cursor.x, cursor.y);
			
			if (pos == -2)
			{
				cursor.asChar = ' ';
				cursor.hit = null;
				return true;
			}
			if (pos < 0) return false;
			
			cursor.asChar = world[pos];
			
			return true;
		}
		public bool MoveCursor(int movex, int movey) => MoveCursor(ref cursor, movex, movey);
		
		public bool MoveCursor(ref GuiCollision cursor, int movex, int movey){
			int x = cursor.x + movex;
			int y = cursor.y + movey;
			int pos = ToStringPos(x, y);
			
			if (pos < 0) return false;
			
			cursor.asChar = world[pos];
			
			cursor.x = x;
			cursor.y = y;
			return true;
		}
		
		public int ToStringPos(int x, int y)
		{
			if (x < 0) return -1;
			if (y < 0) return -3;
			if (y >= high) return -4;
			
			int wide = GetLineWidth(y);
			if (x >= wide) return -2;
			
			int pos = lineStarts[y] + x;
			// passing the horizontal row width
			return pos;
		}
		public string XCast(int x){
			int pos = x;
			BText verticalLine = new BText();
			for (int y = 0 ; y < lineStarts.Length - 1; y++)
			{
				pos = ToStringPos(x,y);
				if (pos == -2) 
				{
					verticalLine.Append(' ');
					continue;
				}
				if (pos < 0) continue;
				
				verticalLine.Append(world[pos]);
			}
			return verticalLine.ToString();
		}
		public string YCast(int y){
			if (y >= lineStarts.Length || y < 0) return "";
			int pos = lineStarts[y];
			// horizontal line
			if (y+1 >= lineStarts.Length) return world.ToString(pos, world.Length - pos);
			return world.ToString(pos, lineStarts[y+1] - 1 - pos);
		}



	//	
	//LocalToGlobalXY
	//GlobalToLocalXY, something's inserted to world, and world is split... so the position of something is both global and local
	//
	//and
	//
	//Linecast, step over l
		protected bool insideWindow = false;
 		public bool IsAtlasHidden(GuiElement ge ){
			// translate from local to atlas space, ie the text atm
			GuiCollision col;
			bool isHit = false;
			
			isHit = YCastRight(ge.left, ge.top, out col, "X#");
			
			
			isHit = (isHit && col == '#');
			if (isHit)
			{
				// this should cast left
				isHit = YCastLeft(ge.left, ge.top, out col, "X#");
				isHit = (isHit && col == '#');
			}
			if (isHit) return true;
			
			isHit = XCastDown(ge.left, ge.top, out col, "X#");
			isHit = (isHit && col == '#');
			if (isHit)
			{
				// this should cast up, or negative
				isHit = XCastUp(ge.left, ge.top, out col, "X#");
				isHit = (isHit && col == '#');
				
			}
			
			return isHit;
		}
 		protected void ApplyWordEnd(){
			
			int pos = charPos - 1;
				
			AtlasWord w = null;
			if (c == '+')
			{
				insideWindow = true;
				if (currentWEND != null)
				{
					if (currentWEND.key != null) 
					{
						currentWEND.end = pos ;
						currentWEND.endLinePos = linePos ;
						currentWEND.bottom = lineCount ;
						windowEnds.Add(currentWEND);
						windowSquare.Remove(currentWEND);
						
					}
					else
					{
						currentWEND.key = "";
						
						currentWEND.end = pos ;
						currentWEND.endLinePos = linePos ;
						currentWEND.bottom = lineCount ;
					}
					currentWEND = null;
					insideWindow = false;
				}
				else
				{
					bool found = IsWindowSquareBorder();
					
					if (found)
					{
						//currentWEND = w;
						insideWindow = true;
					}
					else
					{	
						currentWEND = new AtlasWord(){begin = pos, linePos = linePos};
						currentWEND.top = lineCount ;
						windowSquare.Add(currentWEND);
					}
					
					
				}
			}
			else if (c == '|')
			{
				if (currentWEND != null)
				{
					currentWEND.end = pos ;
					currentWEND.endLinePos = linePos ;
					currentWEND.bottom = lineCount ;
					windowEnds.Add(currentWEND);
					windowHorizontal.Remove(currentWEND);
					currentWEND = null;
				}
				else
				{	
					// i need to check if this is within the boundary of a large window...
					
					bool cancelled = IsUnderParseSquare();
					
					if (!cancelled)
					{	
						
						
						currentWEND = new AtlasWord(){begin = pos, linePos = linePos};
						currentWEND.top = lineCount ;
						windowHorizontal.Add(currentWEND);
					}
					
				}
				
			}
			else if (c == '-' && currentWEND == null)
			{
				
				bool found = IsUnderParseVertical(true);
				
				
				if (found)
				{	
					w = currentWEND;currentWEND=null;
					w.end = pos ;
					w.endLinePos = linePos;
					w.bottom = lineCount ;
					windowEnds.Add(w);
					windowVertical.Remove(w);
					
				}	
				else
				{
					w = new AtlasWord(){begin = pos, end = pos, linePos = linePos};
					w.top = lineCount ;
					windowVertical.Add(w);
				}
			}
			
			
			//windowEnds.AddRange(windowSquare);
			//windowEnds.AddRange(windowHorizontal);
			//windowEnds.AddRange(windowVertical);
		}
		protected bool IsIncompleteParsing(){
			
			return (currentWEND != null 
			|| windowVertical.Count > 0
 			|| windowSquare.Count > 0);
		}
		
		protected bool IsWithinParseWindow(){
			
			return (currentWEND != null 
			//|| windowHorizontal > 0 
			|| IsUnderParseVertical()
 			|| IsUnderParseSquare());
		}
		
		protected bool IsUnderParseVertical(bool setcur = false){
			if (windowVertical.Count < 1) return false;
			int pos = charPos - 1;
			int left = pos - linePos;
			
			AtlasWord w = null;
			bool found = false;
			for (int i = 0; i < windowVertical.Count && !found ; i++)
			{
				w = windowVertical[i];
				found = ( left - w.left == 0);
				
			}
			if (found && setcur) currentWEND = w;
			return found;
		}
		protected bool IsUnderParseSquare(){
			if (windowSquare.Count < 1) return false;
			int pos = charPos - 1;
	
			bool cancelled = false;
			
			
			
			int nleft = pos - linePos;
			int oleft;
			int oright;
				// equals
			AtlasWord w = null;
			
			for (int i = 0; i < windowSquare.Count && !cancelled ; i++)
			{
				w = windowSquare[i];
				
				
				
				oleft = w.begin - w.linePos;
				oright = w.end - w.endLinePos;
				
				if (oleft == nleft || oright == nleft ) cancelled = true;
				// between
				else if (oleft < nleft && nleft < oright) cancelled = true;
			
			
				
			}
			
			return cancelled;
		}
		protected bool IsWindowSquareBorder(){
			int pos = charPos - 1;
			
			bool found = false;
			AtlasWord w = null;
			for (int i = 0; i < windowSquare.Count && !found ; i++)
			{
				w = windowSquare[i];
				found = ( pos - linePos == w.begin - w.linePos ); //  pos - linepos = indent
				
			}
			if (found) currentWEND = w;
			return found;
		}
	
	
		public bool isWindowMod(char c){
			return  c.isUpper() || c.isDigit() 
			|| !(c.isLetter() || c.isWhiteSpace() || isWindow(c));
		}
		public bool isWindowMod(){
			return isWindowMod(c);
		}
		
		// these mods have a unique behavior in atlas, and behave different in windows
		public bool isAtlasMod(char c){
			return "^#X".Contains(c);
		}
		public bool isAtlasMod(){
			return isAtlasMod(c);
		}
		
		public bool isWindow(char c){
			return @"|+-[]".Contains(""+c);
		}
		public bool isWindow(){
			return isWindow(c);
		}
		
		
		public BText areaText = new BText();
		public AtlasWord currentWEND ;
		
		public List<AtlasWord> windowEnds = new List<AtlasWord>();
		public List<AtlasWord> windowVertical = new List<AtlasWord>();
		public List<AtlasWord> windowHorizontal = new List<AtlasWord>();
		public List<AtlasWord> windowSquare = new List<AtlasWord>();
		
		
/* 
		public virtual void NextBoundary(){
			CheckLineBreaks();
			while (charPos < s.Length &&  (c.isWhiteSpace()) ) 
			{
				c = s[charPos++];
				CheckLineBreaks();
				
			}
		}
		
		*/

		protected void LogError(string er){
			
			
			Debug.LogError("<size=18><color=green>Error: "+ er + " at : " + charPos + "</color></size>", s.sourceText);
			
			 throw new System.Exception("");
			 
		}
		
		public BText definition = new BText();
		public BText streamedText = new BText();
		public bool lineBroken = false;
		
		

		public int recursion = 0;
		protected void _ParseWindow()
		{
			
			//charPos = 0
			
			
			block = new GuiBlock(currentWEND);
			block.atlasWord = currentWEND;
			blocks.Add(block);
			block.atlas = this;
			block.atlasMods = ConnectAtlas(currentWEND);
			definition.Clear();
			int left = 0;
			
			//####################### vars
			
			linePos = currentWEND.linePos;
			lineCount = currentWEND.top;
			
			
			int windowPos = currentWEND.begin;
			int windowIndent = currentWEND.begin - currentWEND.linePos;
			int window_end = currentWEND.end + 1;
			int window_wide = currentWEND.wide; 
			
			int window_high = currentWEND.high ; // + 1?
			// defining block size
			
			
			List<AtlasWord> windows = new List<AtlasWord>();
			List<AtlasWord> qualifiedEnds = new List<AtlasWord>();
			//int tall;
			//int wide;
			
			// the window edge + 1
			bool rightOfIndentedWindow() {
				return charPos >= window_end || charPos - linePos - windowIndent >= window_wide;
				
			}
			bool rightOfWindow() {
				
				return charPos % window_wide == 0;
			}
			AtlasWord activeWord = null;
			
			
			
			// window definition including sub-windows
			
			bool rightof = false;
			//streamedText.Clear();
			
			// sub-windows
			c = ' ';
			charPos = currentWEND.begin;
			
			while (charPos < window_end )
			{
				
				recursion++;
				if (recursion > 5000) 
				{
					LogError("inf recursion, check window indent");
					return;
				}
				rightof = rightOfIndentedWindow();
				
				while (charPos < window_end && !(rightof || c.isNewLine()))
				{
					c = s[charPos ++];
					if (c.isNewLine()) break;
					rightof = rightOfIndentedWindow();
					
					GuiElement ge = new GuiChar(c);
					// this would be global position
					ge.left = charPos - linePos - 1;
					ge.right = ge.left;
					ge.top = lineCount;
					ge.bottom = ge.top;
					char appended;
					
					// more than just a comment-on test, this checks vertical and horizontal pairs of #
					if (IsAtlasHidden(ge)){
						appended = ' ';
						
					}
					else
						appended = c;
					
					block.Append(appended, definition.Length);
					definition.Append(appended);
					//streamedText.Append(appended);
				}
				
				if (!rightof)
				{
					// pad the window, maybe check if it's an enormous window, in which case, use an integer array instead
					
					int ind = charPos - linePos - 1;
					
					if (ind >= windowIndent)
						ind -= windowIndent;
					else ind = 0;
					
					for ( ; ind < window_wide ; ind++)
					{
						
						block.Append(' ', definition.Length);
						definition.Append(' ');
						//streamedText.Append(' ');
					}
					
				}
				
				if (charPos >= window_end )
					continue;
				
				NextLine();
				//streamedText.Append('\n');
				int lineEnd = GetLineEnd();
				
				
				if (charPos + windowIndent < lineEnd)
				{
					charPos += windowIndent;
				}
				
				c = s[charPos - 1];
					
				
			}
			
			charPos = 0;
			
			
			//####################### ^^^
			
			block.GetTitle();
			procText = definition;
			
			//####### here the block definition starts
			
			lineCount = linePos = 0;
			AtlasWord n;
			
			
			int startLine = 0;
			int nextLine = 0;
			void SetLinePos(){
				nextLine = charPos / window_wide;
				
				if (startLine != nextLine) 
				{
					activeWord = null;
					startLine = nextLine;
					lineCount = nextLine;
					linePos = nextLine * window_wide;
				}
			}
			
			
			// here I'm scanning the block
			while (charPos < definition.Length )
			{
				c = s[charPos++];
				
				if ( c == ' ') continue;
				
				SetLinePos();
				
				left = charPos - 1;
			
				n = new AtlasWord(){
				begin = left, 
				linePos = linePos,
				top = lineCount
				};
				
				if (c.isBracket())
				{ // [bracket] extension
					// left is set to charPos
					bool cancelled = false;
					
					ParseBracketReplacement();
					
					SetLinePos();
					n.end = charPos - 1;
					n.endLinePos = linePos;
					n.bottom = lineCount;
					//right = charPos - 1;
					bool newRep = braceText.Length > 0;
					
					
					int nleft = n.left;
					int nright = n.right;
					
					
					AtlasWord qe;
					for (int i = 0 ; i < qualifiedEnds.Count ; i++)
					{
						// not going to work yet because words are on top of eachother
						qe = qualifiedEnds[i];
						int oleft = qe.left;
						int oright = qe.right ;
						// brackets disqualify words if they're not perfectly boxed
						cancelled = false;
						
						bool equality = !newRep && (nleft == oleft && nright == oright);
						
						if (!equality)
						{
							
							// things are out of place, now check if any brackets touch 
							if (  oright == nleft || oleft == nright  || nleft == oleft || nright == oright) {cancelled = true;
							}
							// brackets, even blank ones between cancel previous
							else if (oleft < nleft && nleft < oright) {cancelled = true;}
							
							else if (oleft < nright  && nright < oright){ cancelled = true;}
							// another case where it's larger, and encompasses
							else if (oleft > nleft  && oright < nright) {cancelled = true;}
						}
						
						
						if ( cancelled)
						{
							
	//						Debug.Log("cancelo\n"+s.ToString(0,qualifiedEnds[i].begin + 1));
	//						Debug.Log("cancelo\n"+s.ToString(0,qualifiedEnds[i].end+1));
							
	//						Debug.Log("canceln\n"+s.ToString(0,left + 1 ));
	//						Debug.Log("canceln\n"+s.ToString(0,charPos ));
							qualifiedEnds.RemoveAt(i--);
							
							continue;
						}
						else if (equality)
						{
							// bottom right
							qe.endLinePos = linePos;
							qe.end = charPos - 1;
							qe.bottom = lineCount;
							
							break;
						}
					}
					
					// I leave it out until here, it would just get in the way earlier
					if (newRep){
						n.key = braceText.ToString();
						
						windows.Add(n);
						qualifiedEnds.Add(n);
						
					}
				}

				else if (c == '.')
				{ // ..point.. extension
					
					bool cancelled = false;
					int nleft = n.left;
					
					int dotpos = n.begin;
					
					// dots favor the vertical parent
					for (int i = 0 ; i < qualifiedEnds.Count ; i++)
					{
						// not going to work yet because words are on top of eachother
						
						
						int oleft = qualifiedEnds[i].left;
						int oright = qualifiedEnds[i].right;
						
						cancelled = false;
						// equals
						if (oleft == nleft || oright == nleft ) cancelled = true;
						// between
						else if (oleft < nleft && nleft < oright) cancelled = true;
						
						//else if (oleft < nright  ) cancelled = true;
						
						if (cancelled)
						{
							qualifiedEnds[i].endLinePos = linePos ;
							
							int increase = lineCount - qualifiedEnds[i].bottom;
							qualifiedEnds[i].bottom = lineCount;
							
							// in this instance I manually add height
							qualifiedEnds[i].end += window_wide * increase;
							
							break;
						}
					}
					
					
					if (activeWord != null)
					{
						// the word to the left is cancelled... because dot favors vertical
						if (cancelled) activeWord = null;
						else
						{
							// this is on the same line as a word begin
							activeWord.endLinePos = linePos ;
							activeWord.end = dotpos ;
							activeWord.bottom = lineCount;
							
						}
						
					}
					
				}
				
				else if( c == '"' )
				{ // text window
					
					activeWord = n;
					windows.Add(n);
					qualifiedEnds.Add(n);
					
					c = s[charPos++];
					while (c != '"' && !rightOfWindow())
						c = s[charPos++];
					
					
					if (!(c == '"')) charPos--;
						
					SetLinePos();
					
					
					n.key = s.ToString(left, charPos - left  );
					
					
					
					n.endLinePos = linePos;
					
					n.end = charPos - 1;
					n.bottom = lineCount;
					
				}
				
				
				else if (isWindowMod(c))
				{
					
				}
					
					
				 // consume upper-case and other symbols
				else if( c.isLetter() )
				{ // new word, might not be anything. I need to check the flow direction first.
						
					activeWord = n;
					
					windows.Add(n);
					qualifiedEnds.Add(n);
					
					//if (charPos < definition.Length)
					//	c = s[charPos++];
					while (c.isLetter() && !rightOfWindow())
					{
						recursion++;
						if (recursion > 5000) 
						{
							LogError("inf recursion, check window indent");
							return;
						}
						c = s[charPos++];
					}
					if (!c.isLetter()) charPos--;
						
					SetLinePos();
					
					n.key = s.ToString(left, charPos - left  );
					
					
					
					n.endLinePos = linePos;
					
					n.end = charPos - 1;
					n.bottom = lineCount;
				

				}
				
				
			}
			
			//####################### ^^^
			AtlasWord aw;
			
			for (int i = 0 ; i < windows.Count ; i++)
			{
				aw = windows[i];
				
				block.WindowFromAtlas(aw);
			}
			
			
			//block.GenerateMods();
			//if (block.name != "") 
			//{
			//	Debug.Log(block.ToString());
			//	Debug.Log(block.name);
			//	Debug.Log(block.topleftMod);
			//	Debug.Log(block.toprightMod);
			//	for (int i = 0 ; i < block.mods.Count ; i++)
			//	{
			//		//Debug.Log(block.mods[i].ToMod());
			//		
			//	}
			//}
			
			
			// checking for extraneous atlas modifiers
			if (IsTouchingInAtlas(currentWEND, "^"))
			{
				block.atlasMods += "^";
				atlasVariables.Add(block);
			}
			
			
		}

		public void TrimWord(BText s = null){
			if (s == null) s = word;
			if (s.Length < 1) return;
			
			int pos = s.Length ;
			while (pos > 0 && s[pos-1].isWhiteSpace() )
				pos --;
			s.Remove(pos, s.Length - pos);
		}
				
		public BText word = new BText();
		public BText braceText = new BText();
		public void ParseBracketExtension(){
			braceText.Clear();
			
			c = s[charPos++];
			while (c.isWhiteSpace() || isWindowMod(c)) c = s[charPos++];
			
			while (charPos < s.Length && !(c.isBracket() || c.isWhiteSpace())) 
			{
				braceText.Append(c);
				c = s[charPos++];
			}
			
			while (charPos < s.Length && !c.isBracket())
				c = s[charPos++];
			
			
		}
		public void ParseBracketReplacement(){
			braceText.Clear();
			
			c = s[charPos++];
			while (c.isWhiteSpace()) c = s[charPos++];
			while (charPos < s.Length && !(c == ']')) 
			{
				braceText.Append(c);
				c = s[charPos++];
			}
			
			TrimWord(braceText);
		}
		
		public int lineCount = 0;
		public void NextLine(){
			lineCount++;
			if (lineCount  < lineStarts.Length)
			{
				linePos = charPos = lineStarts[lineCount];
			}
			else
				linePos = charPos = world.Length;
		}
		
		public int GetLineEnd(){
			
			int pos = 0;
			
			if (lineCount + 1 < lineStarts.Length)
			{
				pos = lineStarts[lineCount + 1];
				char pc = s[--pos];
				if (s[--pos] == pc)
					return pos + 1  ;
					
				return pos ;
			}
			else
				return world.Length;
		}
		
		
	}
}