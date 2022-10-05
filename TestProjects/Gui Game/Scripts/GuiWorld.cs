
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GuiGame
{

	public class AtlasWord
	{
		
		public string key;
		public int linePos  ; 
		public int begin  ; 
		public int endLinePos ; 
		public int end ; 
		
		// I'm just going to double the values
		public int top; // start line
		public int left  => begin - linePos; 
		public int bottom; // end line
		public int right => end - endLinePos; 
		public int wide => right - left + 1;  
		public int high  => bottom - top + 1; 
		
		public AtlasWord(){}
		public AtlasWord(GuiBlock block)
		{
			key = block.name;
			
			int left =  block.xpos;
			int right =  left + block.wide - 1;
			top =  block.ypos;
			bottom =  top + block.high - 1;
			begin = block.atlas.ToStringPos(left,top);
			end = block.atlas.ToStringPos(right,bottom);
			linePos = block.atlas.ToStringPos(0,top);
			endLinePos = block.atlas.ToStringPos(0,bottom);
		}

		
	}
	
	public class GuiBlock {
		public GuiBlock(){}
		public GuiBlock(AtlasWord aw)
		{
			name = "";
			xpos = aw.left;
			ypos = aw.top;
			
			Generate(aw.wide, aw.high);
			
		}
		public GuiCollision	cursor = new GuiCollision();
		
		public GuiElement[] elements;
		public char[] chars;
		public int a_wide = 0;
		public int a_high = 0;
		public int wide  => a_wide;
		public int high   => a_high;
		public int a_size = 0;
		
		public int xpos = 0;
		public int ypos = 0;
		public string atlasMods = ""; //ie flow direction
		public string topleftMod = ""; 
		public string toprightMod = "";
		public string name = ""; //ie flow direction
		public GuiAtlas atlas; //ie flow direction
		public AtlasWord atlasWord; //ie flow direction
		// public insertPos
		// public appendPos
		
		public int ihitline = -1;
		public int ihitele = -1;
		public int ihitchar = -1;
		
		public GuiElement hitele ;
		public GuiElement hitchar ;
		
		public List<GuiWindow> windows = new List<GuiWindow>();
		public List<GuiElement> mods = new List<GuiElement>();
		public List<GuiElement> borders = new List<GuiElement>();
		
		// no shift
		
		public void Append(char c, int pos){
			chars[pos] = c;
			
		}
		public char Replace(char c, int x, int y){
			int pos = ToStringPos(x,y);
			if (pos < 0) return ' ';
			char replaced = chars[pos];
			chars[pos] = c;
			return replaced;
		}
		public GuiElement ReplaceElement(GuiElement ele, int x, int y){
			int pos = ToStringPos(x,y);
			if (pos < 0) return null;
			GuiElement replaced = elements[pos];
			elements[pos] = ele;
			return replaced;
		}
		
		public void Clear(){
			// maybe unpack or destroy elements?
			Generate(wide,high);
		}
		public void Generate(string s, int wide, int high){
			//Clear();
			
			Generate(wide,high);
			
			int len = s.Length;
			
			for (int i = 0 ; i < len ; i++)
			{
				chars[i] = s[i];
			}
			
			
		}		
		public void Generate(int wide, int high){
			//Clear();
			
			
			a_high = high;
			a_wide = wide;
			a_size = high * wide;
			
			ycast = new GuiCollision[a_wide];
			xcast = new GuiCollision[a_high];
			chars = new char[a_size];
			elements = new GuiElement[a_size];
			
		}
		
		// getting the elements, no blank spaces
		
		protected GuiCollision[] ycast;
		protected GuiCollision[] xcast;
		
		// local atlas?
		public void WindowFromAtlas(AtlasWord aw){
			GuiWindow window = new GuiWindow(aw);
			
			windows.Add(window);
			
			SetWindow(window);
			
		}
		public void Release(GuiComposite comp){
			// this could be, maybe, holding another element, if not just remove it
			
			GuiElement[] released = comp.Release();
			GuiElement re;
			
			for (int i = 0 ; i < released.Length ; i++)
			{
				re = released[i];
				
				if (re is GuiChar)
				{
					ReplaceElement(null, re.left, re.top);
					continue;
				}
				if (re != null)
				{
					for (int y = re.top ; y < re.bottom; y++)
					for (int x = re.left ; x < re.right ; x++)
					{
						ReplaceElement(re, x, y);
					}
				}
			}
				
		}
		public void SetWindow(GuiWindow window){
			//Release(window as GuiComposite);
			
			GuiElement ele;
			
			
			//char c;
			//GuiComposite mod = null;
			//GuiElement cur;
			for (int y = window.top ; y < window.bottom; y++)
			for (int x = window.left ; x < window.right ; x++)
			{
				ele = ReplaceElement(window, x, y);
				
				if (ele != null)
				{	
					window.Append(ele);
				}
				//else if (y > ypos)
				//{
				//	c = GetChar(x,y);
				//	if (isWindowMod(c))
				//	{
				//		cur = (GuiElement)c;
				//		if (mod == null)
				//		{
				//			(cur.left, cur.top) = (x,y);
				//			mod = new GuiComposite(cur);
				//			window.Append(mod);
				//		}
				//		else 
				//		{	
				//			mod += cur;
				//		}
				//	}
				//	else mod = null;
				//}
				
			}
			
		}
		public string XCast(int xpos)
		{
			BText b = new BText();
			for (int x = xpos ; x < a_size ; x += a_wide)
				b.Append(chars[x]);
			return b.ToString();
		}
		
		public string YCast(int ypos)
		{
			
			int x = ToStringPos(0, ypos);
			if (x < 0) x = 0;
			int endPos = ToStringPos(0, ypos + 1);
			if (endPos < 0) endPos = a_size;
			BText b = new BText();
			for ( ; x < endPos ; x ++)
				b.Append(chars[x]);
				
			return b.ToString();
		}
		
		
 		public bool IsHidden(GuiElement ge ){
			if (ge == '#') return false;
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
		
		
		public bool MoveCursorOnMod(int movex, int movey) => MoveCursorOnMod(ref cursor, movex, movey);
		
		public bool MoveCursorOnMod(ref GuiCollision cursor, int movex, int movey){
			int x = cursor.x + movex;
			int y = cursor.y + movey;
			int pos = ToStringPos(x, y);
			
			if (pos < 0) return false;
			char c = chars[pos];
			
			cursor.asChar = c;
			cursor.hit = elements[pos];
			cursor.x = x;
			cursor.y = y;
			return (isWindowMod(c));
		}
		
		public bool TapCursor(int x, int y){
			int pos = ToStringPos(x, y);
			
			if (pos < 0) return false;
			
			cursor.asChar = chars[pos];
			cursor.hit = elements[pos];
			
			return true;
		}
		public void SetCursor(int x, int y) 
		{
			if (x < 0 ) x = 0;
			if (y < 0 ) y = 0;
			int pos = ToStringPos(x, y);
			
			if (pos == -2)
			{
				x = wide-1;
				pos = ToStringPos(x, y);
			}
			if (pos == -4) 
			{
				y = high -1;
				pos = ToStringPos(x, y);
			}
			cursor.pos = (x,y);
		
		}
		
		public bool TapCursor(){
			
			int pos = ToStringPos(cursor.x, cursor.y);
			
			if (pos < 0) return false;
			
			cursor.asChar = chars[pos];
			cursor.hit = elements[pos];
			
			return true;
		}
		public bool MoveCursor(int movex, int movey) => MoveCursor(ref cursor, movex, movey);
		
		public bool MoveCursor(ref GuiCollision cursor, int movex, int movey){
			int x = cursor.x + movex;
			int y = cursor.y + movey;
			int pos = ToStringPos(x, y);
			
			if (pos < 0) return false;
			
			cursor.asChar = chars[pos];
			cursor.hit = elements[pos];
			cursor.x = x;
			cursor.y = y;
			return true;
		}
		public bool XCastUp(int castx, int ypos,  out GuiCollision col, string collisionLayer, int steps = -1)
		{
			col = new GuiCollision();
			
			int x = ToStringPos(castx, ypos - 1);
			if (x < 0) return false;
			int endPos = 0;
			for ( ; x >= endPos ; x -= a_wide)
			{
				if (collisionLayer.Contains(chars[x]))
				{
					col.asChar = chars[x];
					col.hit = elements[x];
					return true;
				}
				steps--;
				if (steps == 0) break;
			}
			
			return false;
		}
		
		public bool XCastDown(int castx, int ypos,  out GuiCollision col, string collisionLayer, int steps = -1)
		{
			col = new GuiCollision();
			int x = ToStringPos(castx, ypos + 1);
			if (x < 0) return false;
			int endPos = a_size;
			for ( ; x < endPos ; x += a_wide)
			{
				if (collisionLayer.Contains(chars[x]))
				{
					col.asChar = chars[x];
					col.hit = elements[x];
					return true;
				}
				steps--;
				if (steps == 0) break;
			}
			
			return false;
		}
		
		public bool YCastLeft(int xpos, int casty,  out GuiCollision col, string collisionLayer, int steps = -1)
		{
			col = new GuiCollision();
			
			int x = ToStringPos(xpos - 1, casty);
			
			if (x < 0) return false;
			int endPos = ToStringPos(0, casty);
			for ( ; x >= endPos ; x --)
			{
				if (collisionLayer.Contains(chars[x]))
				{
					col.asChar = chars[x];
					col.hit = elements[x];
					return true;
				}
				steps--;
				if (steps == 0) break;
			}
			
			return false;
		}
		
		public bool YCastRight(int xpos, int casty,  out GuiCollision col, string collisionLayer, int steps = -1)
		{
			col = new GuiCollision();
			
			int x = ToStringPos(xpos + 1, casty);
			if (x < 0) return false;
			
			int endPos = ToStringPos(0, casty + 1);
			for ( ; x < endPos ; x ++)
			{
				if (collisionLayer.Contains(chars[x]))
				{
					col.asChar = chars[x];
					col.hit = elements[x];
					return true;
				}
				steps--;
				if (steps == 0) break;
			}
			
			return false;
		}
		
		public GuiElement GetElement(int x, int y){
			
			
			return elements[a_wide * y + x];
		}
		public char GetChar(int x, int y){
			
			
			return chars[a_wide * y + x];
		}
		public (int x, int y) ToXY(int pos){
			
			
			return (pos % a_wide, pos / a_wide);
		}
		public (float x, float y) ToFloat(int x, int y){
			
			float x_frac = (float)x / wide;
			float y_frac = 1 - (float)y / high;
			
			return (x_frac, y_frac);
		}
		public int ToStringPos(int x, int y){
			if (x < 0) return -1;
			if (x >= a_wide) return -2;
			if (y < 0) return -3;
			if (y >= a_high) return -4;
			
			int pos =  a_wide * y + x;
			
			return pos;
		}
		
		public void InsertVertLine(int col){
			
			// shifting right
			int insertPos = 0;
			for (int y = 0 ; y < a_high ; y++){
				
				for (int x = a_wide - 1 ; x > col ; x --)
				{
					insertPos = y * a_wide + x;
					
					chars[insertPos] = chars[insertPos - 1];
				}
			
			}
		}
		public void InsertLine(int row){
			// shifting down
			int insertPos = 0;
			for (int x = 0 ; x < a_wide ; x++){
				
				for (int y = a_high - 1 ; y > row ; y --)
				{
					insertPos = y * a_wide + x;
					
					chars[insertPos] = chars[insertPos - 1];
				}
			
			}
		}
		
		public char Insert(char c, int x , int y){
			// shifting right
			int insertPos = y * a_wide + x;
			char hit = chars[insertPos];
			for (int pos = a_wide - 1 ; pos > x ; pos --)
			{
				insertPos = y * a_wide + pos;
				
				chars[insertPos] = chars[insertPos - 1];
			}
			insertPos = y * a_wide + x;
			chars[insertPos] = c;
			
			return hit;
		}
		public int Length  => a_size;
		
		
		public string ToElementString(){
			if (a_size == 0) return "";
			BText str = new BText();
			
			
			str.Append(elements[0] ?? chars[0]);
			GuiElement ge;
			char c;
			for (int i = 1 ; i < a_size ; i++)
			{
				
				if (i % a_wide == 0) str.Append('\n');
				
				c = (elements[i] ?? chars[i]);
				ge = (GuiElement)c;
				(ge.left, ge.top) = ToXY(i);
				 
				//if (IsHidden(ge))
				//	str.Append(' ');
				//else
				str.Append(ge);
			}
			return str.ToString();
		}
		public override string ToString(){
			if (a_size == 0) return "";
			BText str = new BText();
			
			
			str.Append(chars[0]);
			GuiElement ge;
			
			if (wide > 1)
			for (int i = 1 ; i < a_size ; i++)
			{
				
				if (i % a_wide == 0) str.Append('\n');
				
				ge = (GuiElement)chars[i];
				(ge.left, ge.top) = ToXY(i);
				 
				if (IsHidden(ge))
					str.Append(' ');
				else
					str.Append(chars[i]);
			}
			else
			for (int i = 1 ; i < a_size ; i++)
			{
				
				
				ge = (GuiElement)chars[i];
				(ge.left, ge.top) = ToXY(i);
				 
				if (IsHidden(ge))
					str.Append(' ');
				else
					str.Append(chars[i]);
			}
			return str.ToString();
		}
		public virtual string ToVariable(){
			if (a_size == 0) return "";
			BText str = new BText();
			
			
			str.Append(' ');
			GuiElement ge;
			if (wide > 1)
			for (int i = 1 ; i < a_size ; i++)
			{
				
				if (i % a_wide == 0) str.Append('\n');
				if (isNotVariable(chars[i])) 
				{
					str.Append(' ');
					continue;
				}
				ge = (GuiElement)chars[i];
				(ge.left, ge.top) = ToXY(i);
				 
				if (IsHidden(ge))
					str.Append(' ');
				else
					str.Append(chars[i]);
			}
			else
			for (int i = 1 ; i < a_size ; i++)
			{
				
				if (isNotVariable(chars[i])) 
				{
					str.Append(' ');
					continue;
				}
				ge = (GuiElement)chars[i];
				(ge.left, ge.top) = ToXY(i);
				 
				if (IsHidden(ge))
					str.Append(' ');
				else
					str.Append(chars[i]);
			}
			return str.ToString();
		}
		public bool isNotVariable(char c){
			return @"|+-".Contains(c);
		}	
		public bool isWindow(char c){
			return "|+-[].'\"".Contains(c);
		}			
		public bool isWindowMod(char c){
			if (c == '@') return false;
			return c.isUpper()
			|| !(c.isLetter() || c.isWhiteSpace() || isWindow(c));
		}
		
	
		// this stores the mods, like asterisk
		public void GetTitle(){
			if (a_size == 0) return ;
			
			BText bname = new BText();
			char c = ' ';
			bool hasname = false;
			int i = 1;
			
			// this section is invisible
			for ( ; i < a_size ; i++)
			{
				// only left to right atm
				c = chars[i];
				
				if (
				   "#X ".Contains(c)
				|| !isWindowMod(c)
				|| elements[i] != null) // element ? window
				{
					if (hasname || c.isLetter() || c == '.')
					{
						bname.Append(c);
						hasname = true;
					}
					continue;
				}
				
				if (hasname)
					toprightMod += c;
				else
					topleftMod += c;
				
				
				if (c == ':')
				{
					int pos = bname.Length;
					//trim
					while (pos > 0 && bname[pos-1].isWhiteSpace() )
						pos --;
					
					
					name = bname.ToString(0,pos);
					
					break;
				}
				if (a_wide != 1 && i % a_wide == 0 )
				{
					hasname = false;
					break;
				}
			}
			
		}
		
		public void GenerateMods(){
			
			if (a_size == 0) return ;
			
			mods.Clear();
			GuiElement mod = null;
			GuiElement cur = null;
			
			char c = ' ';
			
			
			int stepx,stepy;
			int linex,liney;
			int startx,starty;
			
			stepx=stepy=linex=liney=startx=starty =0;
			
			string readDirection = atlas.GetReadDirection(); //GetModDirection
			// suppose it could be RL for switching direction
			char rd = 'R';
			char linedir = 'D';
			if (readDirection.Length >= 1)
				linedir = readDirection[0];
			if (readDirection.Length >= 2) 
				rd = readDirection[1];
			
			if ("RL".Contains(linedir) && "RL".Contains(rd)) rd = 'D';
			
			if (linedir == 'D')
				(linex, liney, starty) = (0,1,0);
			else if (linedir == 'U')
				(linex, liney, starty) = (0,-1,high-1);
			else if (linedir == 'R')
				(linex, liney, startx) = (1,0,0);
			else if (linedir == 'L')
				(linex, liney, startx) = (-1,0,wide-1);
			
			
			if (rd == 'D')
				(stepx, stepy, starty) = (0,1,0);
			else if (rd == 'U')
				(stepx, stepy, starty) = (0,-1,high-1);
			else if (rd == 'R')
				(stepx, stepy, startx) = (1,0,0);
			else if (rd == 'L')
				(stepx, stepy, startx) = (-1,0,wide-1);
			
			
			cursor.pos = (startx,starty);
			TapCursor();
			//if (name != "") 
			//{
			//	while (MoveCursor(stepx,stepy))
			//	if (cursor.asChar == ':') break;
			//	
			//	
			//	if (cursor.asChar != ':')
			//		cursor.pos = (startx,starty);
			//}
			//else
			//	cursor.pos = (startx,starty);
			
			for (int i = 0 ; i < a_size ; i++)
			{
				if (!MoveCursor(stepx,stepy))
				{
				
					if (stepx != 0) cursor.x = startx;
					else 			cursor.y = starty;
					
					if (a_wide != 1 ) // won't work
						mod = null;
				
					if (!MoveCursor(linex,liney))
						break;
				}
				
				// only left to right atm
				c = cursor.asChar;
				
				
				if (
				   "#X ".Contains(c)
				|| !isWindowMod(c)) // element ? window
				{
					mod = null;
				
					continue;
				}
				//if (cursor._hit != null)
				//{
				//	mod = cursor.hit;
				//	continue;
				//}
				
				
				if (isWindowMod(c))
				{	
					
					cur = (GuiElement)c;
					if (mod == null)
					{
						(cur.left, cur.top) = cursor.pos;
						mod = new GuiComposite(cur);
						mods.Add(mod);
					}
					else 
					{	
						mod += cur;
						//(mod.right, mod.bottom) = cursor.pos;
						//mod.right++;
						//mod.bottom++;
						
					}
					//elements[i] = cur;
					elements[ToStringPos(cursor.x,cursor.y)] = mod;
				}
			}
			
		
		}
		public void GenerateMods(GuiWindow g){
			GenerateMods();
			ConnectMods(g);
			
			if (g.w > 0) 
			{
				Debug.Log(g);
			}
				
			
		}
		
		protected string GetModDirection(){
			string rd = "";
			int len = atlas.atlasMods.Length;
			for (int i = 0 ; i < len && rd.Length < 3 ; i++)
			{
				if ("RDLU".Contains(atlasMods[i]))
					rd += atlasMods[i];
				
			}
			return rd;
		}
		
protected void LeftWindowMod   (GuiWindow g){
	cursor.pos = (g.left,g.top);
	
	
	if (MoveCursorOnMod(-1, 0))
		g.Append(cursor.hit);
	
	
}
protected void RightWindowMod  (GuiWindow g){
	cursor.pos = (g.right - 1,g.top);
	
	if (MoveCursorOnMod(1, 0))
		g.Append(cursor.hit);
	
	
}
protected void TopWindowMod    (GuiWindow g){
	cursor.pos = (g.left,g.top);
	
	if (MoveCursorOnMod(0, -1))
		g.Append(cursor.hit);
	
	
}
protected void BottomWindowMod (GuiWindow g){
	cursor.pos = (g.left,g.bottom - 1);
	
	if (MoveCursorOnMod(0, 1))
		g.Append(cursor.hit);
	
	
}

protected void TopWindowCast   (GuiWindow g){
	cursor.pos = (g.left,g.top);
	
	for (int x = 0 ; x < g.wide  ; x++)
	{
		if (MoveCursorOnMod(0, -1))
			g.Append(cursor.hit);
		cursor.pos = (cursor.x+1,g.top);
	}
	
	
}
protected void BottomWindowCast(GuiWindow g){
	cursor.pos = (g.left,g.bottom - 1);
	
	for (int x = 0 ; x < g.wide ; x++)
	{
		if (MoveCursorOnMod(0, 1))
			g.Append(cursor.hit);
		cursor.pos = (cursor.x+1,g.bottom - 1);
	}
	
	
}
protected void LeftWindowCast  (GuiWindow g){
	cursor.pos = (g.left,g.top);
	
	for (int y = 0 ; y < g.high ; y++)
	{
		if (MoveCursorOnMod(-1, 0))
			g.Append(cursor.hit);
		cursor.pos = (g.left,cursor.y+1);
	}
	
	
}
protected void RightWindowCast (GuiWindow g){
	cursor.pos = (g.right - 1,g.top);
	
	for (int y = 0 ; y < g.high ; y++)
	{
		if (MoveCursorOnMod(1, 0))
			g.Append(cursor.hit);
			
		cursor.pos = (g.right - 1,cursor.y+1);
	}
	
	
}
protected void InnerWindowCast (GuiWindow g){
	if (g.high == 1) return;
	cursor.pos = (g.left,g.top+1);
	
	TapCursor();
	if (isWindowMod(cursor.asChar))g.Append(cursor.hit);
	for (int i = 0 ; i < g.high * g.wide; i++)
	{
		if (MoveCursorOnMod(1, 0))
			g.Append(cursor.hit);
		
		if (cursor.x % g.right == 0)
		{
			cursor.pos = (g.left, cursor.y + 1);
			
			if (cursor.y % g.bottom == 0) break;
			
			TapCursor();
			if (isWindowMod(cursor.asChar))g.Append(cursor.hit);
		
		}
	}
	
}
		
		protected void ConnectMods(GuiWindow g){
			
			ModCast(g);
			// get inner mods
		}
		protected void ModCast(GuiWindow g){
			
			string readDirection = atlas.GetReadDirection(); //GetModDirection
			// suppose it could be RL for switching direction
			char rd = 'R';
			char linedir = 'D';
			if (readDirection.Length >= 1)
			
				linedir = readDirection[0];
			
			if (readDirection.Length >= 2) 
				rd = readDirection[1];
			
			if ("RL".Contains(linedir) && "RL".Contains(rd)) rd = 'D';
			
			if (high == 1)
			{
					
				if (rd == 'R' || linedir == 'R')
				
					LeftWindowMod(g);
				
				else RightWindowMod(g);
				
				
			}
			else if (wide == 1)
			{
				
				if (rd == 'D' || linedir == 'D')
					
					TopWindowMod(g);
					
				else BottomWindowMod(g);
			}
			else
			{
				
				// only read direction matters
				if ("UD".Contains(rd) )
				{
						
					if (rd == 'D' || linedir == 'D')
						
						TopWindowCast(g);
					
					
					else BottomWindowCast(g);
				
				}
				else //RL
				{
					if (rd == 'R' || linedir == 'R')
						LeftWindowCast(g);
					
					else RightWindowCast(g);
						
				}
			}
			
			InnerWindowCast(g);
		}

		
		public void ApplyMods(){
			if (a_size == 0) return ;
			
			// check if mods are touching windows
			int x;
			int y;
			GuiElement w;
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
				
				x = m.left;
				y = m.top;
				w = GetElement(x, y);
				
				// button?
				
				// hack: * is ambiguous between p-child
				if (m.ToString().Contains('*'))
				{
					// FAIL FAIL FAIL
					
					// xy is relative
					// check for block edge
					if (x < 1 || x > wide-1 || y < 1 || y > high-1)
					{
						// it's a button position
						
						// actually can I reposition it from here?
					}
				}
				
				if (w != null)
				if (w is GuiWindow) 
				{
					// already in window
					
					(w as GuiWindow).Append(m);
					
						
					continue;
					
				}
				
				x = mods[i].right;
				w = GetElement(x, y);
				
				
				if (w != null)
				if (w is GuiWindow) 
				{
					// apply the mod
					
					(w as GuiWindow).Append(m);
					continue;
					
				}
				
			}
			
		}
		
		
	}
	public class GuiElement { 
		
		// scale x, y? nah, let the display handle it
		// uv 1,2,3,4
		// mesh?
		
		
		public Image image; // this may just be a char, but it can be anything
		public int? _left = null;
		public int? _top = null;
		public int left {
			get { 
				if (!_left.HasValue) {
					Debug.Log("attempted to use null int? value");
					return default(int);
				}
				return _left.Value;
			
			}
			set => _left = value;
		}
		public int top {
			get { 
				if (!_top.HasValue) {
					Debug.Log("attempted to use null int? value");
					return default(int);
				}
				return _top.Value;
				
			
			}
			set => _top = value;
		}
		
		public int? _right = null;
		public int? _bottom = null;
		public int right {
			get => _right ?? left + 1;
			set => _right = value;
		}
		public int bottom {
			get => _bottom ?? top + 1;
			set => _bottom = value;
		}
		public int wide => right - left ;
		public int high => bottom - top ;
		public char asChar = 'E';
		public string name = "E.";
		
		
		
		public static implicit operator char(GuiElement d) => d.asChar; 
		public static implicit operator string(GuiElement d) => d.name; 
		public static explicit operator GuiElement(char b) => new GuiChar(b);
		public static explicit operator GuiElement(string b) => new GuiWord(b);
	
		public static explicit operator GuiElement(Image b) => new GuiImage(b);
		
		public static explicit operator GuiElement(GuiElement[] b) => new GuiComposite(b);
		

		public static GuiComposite operator +(GuiElement a, GuiElement[] str){
			GuiComposite ng;
			if (a is GuiComposite)
			{
				ng = a as GuiComposite;
				
				return ng + str;
			}
			else 
			{
				ng = new GuiComposite(a);
			}
			return ng + str;
			
		}
		public static GuiComposite operator +(GuiElement a, GuiElement str){
			GuiComposite ng;
			if (a is GuiComposite)
			{
				ng = a as GuiComposite;
				
				return ng + str;
			}
			else 
			{
				ng = new GuiComposite(a);
			}
			return ng + str;
		}	
		
		public override string ToString(){
			return name;
		}
		public virtual string ToMod(){
			return "";
		}
		public virtual string ToString(int s, int len){
			if (s == 0 && len == name.Length) return name;
			
			BText b = new BText();
			int pos = s;
			for (int i = s ; i < len ; i++)
			{
				pos = (int)Mathf.Repeat( i , name.Length);
				b.Append(name[pos]);
			}
			
			return b.ToString();
			
		}
			
		public virtual char GetChar(){
			return asChar;
		}
		public virtual GuiElement Replace(GuiElement g, int x, int y = 0 ){
			return this;
			
		}
		public virtual GuiElement Replace(int pos, GuiElement g ){
			return this;
			
		}
		public virtual GuiElement GetElement(int x = 0, int y = 0 ){
			return this;
		}
		public virtual char GetChar(int x = 0, int y = 0 ){
			return asChar;
			
		}
		
		public virtual Image GetImage(){ return null; }
		// assumes they're 1x1
		public virtual bool ContainsPoint( int x, int y){
			
			int oleft = x;
			
			int otop = y ;
			
			
			
			bool xparity = (left <= oleft && oleft <= right);
			if (!xparity) return false;
			
			bool yparity =  (top <= otop && otop <= bottom);
			if (!yparity) return false;
			
			return true;
		}
		public virtual bool IsTouching( GuiElement other){
			// this is the verision that would only understand rects, they may overlap or touch at one position, on one side
			int oleft = other.left;
			int oright = other.right;
			int otop = other.top ;
			int obottom = other.bottom;
			
			bottom = this.bottom +1;
			right = this.right+1;
			
			bool xparity = (left <= oright && oleft <= right);
			if (!xparity) return false;
			
			bool yparity =  (top <= obottom && otop <= bottom);
			if (!yparity) return false;
			
			return true;
		}
		
		
		public virtual GuiComposite Insert(int pos, GuiElement g){
			
			GuiComposite ng = new GuiComposite();
			ng += this;
			return ng.Insert(pos, g);
		}


	}
	public class GuiChar : GuiElement{ 
		// this contains some information about modifying a window or creating a guiword
		public GuiChar(char c){
			asChar = c;
			name = c+"";
		}
	
	}
	public class GuiImage : GuiElement {
		// a specific element should display an image, but is a word
		public GuiImage(Image newImg)
		{
			image = newImg;
			name = newImg.name;
			asChar = 'I';
		}
		
	}
	
	
	public class GuiComposite : GuiElement{
		// the contained elements are anything, and should be interpreted seperately
		public GuiElement[] composite = new GuiElement[20];
		
		public int w = 0;
		public int capacity = 20;
		public GuiComposite(){ asChar = '?'; name = "?.";}
		public GuiComposite(GuiElement[] ge):this(){
			if (ge.Length > 0)
			{
				left = ge[0].left;
				top = ge[0].top;
				right = ge[0].right;
				bottom = ge[0].bottom;
			}
			Append(ge);
			
		}
		public GuiComposite(GuiElement ge):this(){
			left = ge.left;
			top = ge.top;
			right = ge.right;
			bottom = ge.bottom;
			Append(ge);
			
		}
		
		public override string ToMod(){
			BText str = new BText();
			for (int i = 0 ; i < w ; i++)
			{
				str.Append(composite[i].ToString());
			}
			return str.ToString();
		}
		public override string ToString(){
			return name + ToMod();
		}
		public override string ToString(int start, int len){
			BText str = new BText(name);
			for (int i = 0 ; i < w ; i++)
			{
				str.Append(composite[i].ToString());
			}
			string savename = name;
			name = str.ToString();
			
			string rename = base.ToString(start, len);
			name = savename;
			return rename;
		}
		
		public override bool ContainsPoint(int x, int y = 0 ){
			// the elements must have their positions set
			int index = IndexOf(x, y);
			
			
			return index > -1;
			
			
		}			
		public override GuiElement GetElement(int x, int y = 0 ){
			// the elements must have their positions set
			int index = IndexOf(x, y);
			if (index > -1 ) return composite[index]; 
			
			
			return this;
			
			
		}		
		public override char GetChar(int x, int y = 0){
			// the elements must have their positions set
			int index = IndexOf(x, y);
			if (index > -1 ) return composite[index].GetChar(x,y); 
			
			return asChar;
			
			
		}
		public override GuiElement Replace(GuiElement g, int x, int y = 0){
			// the elements must have their positions set
			int index = IndexOf(x, y);
			if (index > -1){
				GuiElement ele = composite[index];
				composite[index] = g;
				return ele; 
			}
			
			
			return this;
			
			
		}		
		public override GuiElement Replace(int pos, GuiElement g){
			
			GuiElement repl = composite[pos];
			composite[pos] = g;
			
			return repl;
		}
		public override GuiComposite Insert(int pos, GuiElement g){
			
			CheckCapacity(1);
			w++;
			for (int i = w ; i > pos ; i--)
			{
				composite[i] = composite[i-1];
			}
			composite[pos] = g;
			return this;
		}
		
		public static GuiComposite operator +(GuiComposite a, GuiElement[] str){
			
			a.Append(str);
			return a;
			
		}
		public static GuiComposite operator +(GuiComposite a, GuiElement g){
			a.Append(g);
			return a;
		}	
		
		public virtual void Append(GuiElement[] str){
			
			CheckCapacity(str.Length);
			
			for (int i = 0 ; i < str.Length ; i++)
			{
				if (Contains(str[i])) continue;
				composite[w++] = str[i];
			}
			
			
		}
		public int IndexOf(GuiElement g){
			
			for (int i = 0 ; i < w ; i++)
			{
				Debug.Log(g);
				if (composite[w] == g) return i;
			}
			return -1;
		}
		public bool Contains(GuiElement g){
			for (int i = 0 ; i < w ; i++)
			
				if (composite[i] == g) return true;
			
			return false;
		}
		public virtual void Include(GuiElement g){
			CheckCapacity(1);
			composite[w++] = g;
		}	
		public virtual void Append(GuiElement g){
			if (Contains(g)) return;
			CheckCapacity(1);
			composite[w++] = g;
		}	
		public GuiElement[] Release(){
			GuiElement[] r = new GuiElement[w];
			while (w > 0)
			{
				w--;
				r[w] = composite[w];
				composite[w] = null;
			}
			
			return r;
		}
		public GuiElement Release(int index){
			
			GuiElement r = composite[index];
			
			for (int i = index ; i < w ; i++)
			{
				composite[i] = composite[i+1];
			}
			w--;
			return r;
		}
		
		public virtual int IndexOf(int x, int y = 0){
			
			for (int i = 0 ; i < w ; i++)
				if (composite[i].ContainsPoint(x,y)) return i; 
			
			return -1;
		}

		public void CheckCapacity(int addlen){
			
			if (w+addlen >= capacity)
			{
				capacity = w + addlen + 20;
				GuiElement[] newArray = new GuiElement[capacity];
				for (int i = 0 ; i < w ; i++)
				{
					newArray[i] = composite[i];
				}
				composite = newArray;
				
			}
		}
				
	}
	public class GuiWindow : GuiComposite { 
		// this is simply a window
		// the contained elements char or words, will modify said window
		public GuiWindow( ){ 
			name = "W.";
			asChar = 'W';
		}
		public GuiWindow(AtlasWord aw){
			name = aw.key ?? "W.";
			asChar = 'W';
			
			left = aw.left;
			
			right = aw.right + 1;
			top = aw.top;
			bottom = aw.bottom + 1;
		}
		


		public override int IndexOf(int x, int y = 0)
		{
			int width = right - left ;
			x -= left;
			y -= top;
			int pos = width * y + x ;
			
			return pos;
		}
		public override GuiElement GetElement(int x, int y = 0)
		{
			// assume the chars don't have position values set
			int pos = IndexOf(x, y);
			
			return composite[pos];
				
		}
		



	}
	
	public class GuiWord : GuiComposite {
		// a group of elements should be read as a word, but the contained elements are anything
		
		public GuiWord(){
			name = "W.";
			asChar = 'W';
		}
		public GuiWord(string str){
			name = str;
			asChar = 'W';
			
		}
		
		public GuiWord(GuiChar[] str){
			name = "";
			asChar = 'W';
			
			composite = str;
			name = ToString();
		}
		
		

	}

	
	public class GuiCollision {
		// provides the topmost element and char
		
		public (int,int) pos { get => (x,y) ;set => (x,y) = value; }
		public int x = -1;
		public int y = -1;  
		public GuiElement _hit = null;
		public GuiElement hit {
			get { 
				if (_hit != null) return _hit; 
				
				GuiElement ghit = (GuiElement)asChar ;
				ghit.left = x;
				ghit.top = y;
				return ghit;
			}
			set => _hit = value;
		}
		
		public char asChar = '\0';
		
		
		public static implicit operator char(GuiCollision d) => d.asChar; 
		public static implicit operator GuiElement(GuiCollision d) => d.hit;
	}


	
}
	