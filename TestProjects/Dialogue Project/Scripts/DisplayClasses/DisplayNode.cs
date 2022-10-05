
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;
using System;
using System.Text;


namespace DialogueSystem
{
	public partial class DisplayNode : UpdateBehaviour {
		// This is a widget. It plays the running text so this bit of code isn't part of the parent.
		
		public Canvas canvas ;
		public CanvasGroup canvasGroup ;
		public RectTransform rectTransform;
		protected CanvasRenderer bodyRenderer;
		public DisplayParser parser;
		
// Stuff related to string positions

		public int i_paragraph {get{return paragraphPos;}}
		public int paragraphPos = 0;
		public List<int> paragraphs = new List<int>();
		
		public int i_line {get{return lineStartPos;}}
		public List<int> lines = new List<int>();
		
		// basically if word == wordpos and collecting then it's probably not done
		public int wordPos = 0;
		public int i_word      {get{return wordPos;}}
		public List<int> words = new List<int>();
		
		// based on list of paragraphs
		public void GetParagraph(int pos)
		{
			wordText.Clear();
			
			if (paragraphs.Count <= pos) return ;
			
			
			GetQuoteAt(paragraphs[pos], '\n');
			
		}
		// this would be a better search if it were a delimited
		public void GetQuoteAt(int pos, char d  = '\"')
		{
			wordText.Clear( );
			
			if (words.Count < pos) return ;
			
			
			char c = s[pos];
			
			wordText.Append( c);
			for (int i = pos + 1 ; i < s.Length ; i ++)
			{
				c = s[i];
				
				if ( c == d)
					return ;
				
				wordText.Append( c);
			}
			wordText.Clear(); // no quote no word
			return ;
		}
		
		// word based on list of words
		
		// optional:
		// check if it's a quote, a name
		// search for result
		public void GetWord(int pos)
		{
			
			wordText.Clear();
			if (words.Count < 1 || pos >= words.Count ) return ;
			
			GetWordAt(words[pos] );
		}
		
		// this sets based on character positions. Easy when I know how long a word is.
		
		public List<UIVertex> uiSelection = new List<UIVertex>();
		public void ClearSelection()
		{
			
			uiSelection.Clear();
		}
		public void SetSelection(int startPos, int endPos)
		{
			if (endPos > s.Length) endPos= s.Length ;
			UIVertex[] verts ;
			GetCut(startPos, endPos, out verts);
			
			if (verts.Length < 1) return;
			
			float prevBottomLine = -1f;
			float prevX = -1f;
			float bottomLine ;
			float topLine ;
			float lower ;
			
			
			for (
			int i = 0,  pos = startPos; 
			i < verts.Length; 
			i+=4, pos+=1 )
			{
				// this would be cheaper if I checked if start and finish were different lines
				lower = GetMarkUnder(s[pos]);
				bottomLine = verts[i].position.y + lower;
				
				if (i > 1 && (prevBottomLine - bottomLine).IsZero())
				{
					// moves them left
					verts[i].position.x = prevX;
					verts[i+1].position.x = prevX;
				}
				else
					prevBottomLine = bottomLine;
				// I can compare this bottom the the prev one and combine the x if they're ==
				
				verts[i].position.y = bottomLine;
				verts[i+3].position.y = bottomLine;
				
				
				topLine = verts[i].position.y + fontHeight;
				
				verts[i+1].position.y = topLine;
				verts[i+2].position.y = topLine;
				prevX = verts[i+3].position.x;
			}
			uiSelection.Clear();
			uiSelection.AddRange(verts);
			dirtySelection = true;
		}
		public void GetWordAt(int pos, bool spaces = true)
		{
			
			wordText.Clear();
			if (s.Length < pos) return ;
			
			
			char c;
			
			for (int i = pos ; i < s.Length ; i ++)
			{
				c = s[i];
				
				if ((System.Char.IsWhiteSpace(c)  ))
				{
					if (c == '\n') return ;
					else if (spaces) 
					{
						wordText.Append( c);
						return;
					}
					
				}
				
				wordText.Append( c);
			}
			return;
		}
		
		// gets line based on list of lines
		public string GetLine(int pos)
		{
			if (lines.Count < 1 || lines.Count <= pos) return "";
			
			int n = pos + 1;
			int npos;
			if (lines.Count < n) npos = s.Length;
			else
				npos = lines[n];
			
			return GetLineAt(lines[pos], npos);
		}
		public string GetLineAt(int start, int end)
		{
			
			if (s.Length < start) return "";
			if (s.Length < end) 
				end = s.Length;
			
			return s.ToString(start, end - start);
			
			
			
		}
		
		public int i_char    {get{return charPos - continuePos;}}
		
		// Calculating vertex positions based on string results. basically start pos, end pos
		public UIVertex[] GetVertices(int start, int length)
		{
			if (start + length >= uiCharVerts.Count) length =  uiCharVerts.Count - length - 1; // based on 0 index. length needs to be a position
			
			return uiCharVerts.GetRange(start, start + length).ToArray();
		}
		
		
		// point is a pre-reversed position
		
		public Rect hitParagraph;
		public int iHitParagraph;
		
		public Rect hitLine;
		public int iHitLine;
		
		public Rect hitWord;
		public int iHitWord;
		
		public Rect hitChar;
		public int iHitChar;
		public Vector2 editPos = new Vector2();
		
		public bool hitAny = false;
		
		public bool CastPoint( Vector2 worldPoint)
		{
			
			hitAny = false;
			if (s.Length < 1) 
			{
				 // should reset edit position
				SetEditPos(-1, -1f);
				return false;
			}
			
			for (int i = 0; !hitAny && i < paragraphs.Count; i ++)
				hitAny = CastParagraph(i, worldPoint);
			
			if (hitAny) CastLine(iHitLine, worldPoint);
			
			return hitAny; //if! UnsetEdit
		}
		public void CastLine(int pos, Vector2 worldPoint)
		{
			// pass the line in?
			Vector2 correctPoint = worldPoint
			;
			
			
			correctPoint = body.InverseTransformPoint(correctPoint);
			
			HitLineLocal(pos, correctPoint.x);
		}
		
		
		public void SetHitChar(int pos)
		{
			iHitChar = pos;
			
		}
		public void SetHitWord(int pos)
		{
			iHitWord = pos;
		}
		protected void SetEditPos(int pos, float point)
		{
			
			if (s.Length < 1) // fix?
			{
				editPos.Set(0, 0);
				hitChar = new Rect(new Vector2(0,0), new Vector2(0,0));
				return;
			}
			else if (pos < 0)
				pos = 0;
			
			else if (pos >= i_char)
			{
				pos = i_char - 1;
			}
			
			iHitChar = pos;
			int uipos = pos * 4;
			
			
			float left ;
			if (pos > 0 && pos > lines[iHitLine])
				left = uiCharVerts[uipos - 1].position.x;
			else
				left = uiCharVerts[uipos].position.x;
			float right = uiCharVerts[uipos + 3].position.x;
			
			
			if (Mathf.Abs(left - point ) < Mathf.Abs(right - point))
				editPos.Set(left, hitLine.y + lineHeight - fontHeight / 2f);
			else
				editPos.Set(right,hitLine.y + lineHeight - fontHeight / 2f);
				
			float bottom = uiCharVerts[uipos].position.y - medianAscent;
			float top = uiCharVerts[uipos + 1].position.y;
			
			hitChar = new Rect(new Vector2(left ,bottom ), new Vector2(right - left,top - bottom));
		}
		
		
		public StringBuilder wordText = new StringBuilder();
		public void HitLineLocal(int linePos, float point)
		{
			
			if (s.Length < 1 || linePos < 0 ) 
			{
				// preceding or zero lines
				SetEditPos(-1, point);
				return;
			}
			
			// may be blank
			if (linePos >= lines.Count || lines[linePos] >= s.Length) 
			{
				// trailing
				SetEditPos(i_char, point);
				return;
			}
			
			if (s[lines[linePos]] == '\n')
			{
				ClearSelection();
				SetEditPos(lines[linePos], point);
				return;
			}
			
			
			int vertex = lines[linePos] * 4 + 3 ;
			
			if (uiCharVerts.Count < 1 || vertex >= uiCharVerts.Count)
			{
				return; // meeh// fix?
			}
			
			float right = uiCharVerts[vertex].position.x;
			bool hit = (point < right);
			
			int nchar = lines[linePos];
			if (!hit)
			{
				int lineEnd; // the last character on this line, or end of string
				int n = linePos + 1;
				if (n < lines.Count) 
				{
					lineEnd = lines[n] ; 
				}
				else
					lineEnd = charPos - continuePos ;
					
				
				while ( vertex < uiCharVerts.Count && nchar < lineEnd ) 
				{
					right = uiCharVerts[vertex].position.x;
					
					hit = (point < right);
					if (hit) break;
					nchar ++;
					// I need to track what word it's in.
					vertex += 4;
					
				}
				
					
			} // if!hit
			
			// nchar needs to be the actual character.
			SetEditPos(nchar, point);
			
			// WORD from character?
			
			
			if (words.Count < 1 || iHitChar < 0) 
			{
				return;
				
			}
			
			int wordPos = 0;
			
			// here if "iHitChar >" means the left of a word results in prev word ">=" results in word on line.
			while ( wordPos < words.Count 
			&& iHitChar >= words[wordPos] )
			{
				
				wordPos ++;
			}
			wordPos --;
			
			// should probably figure out where the edit is instead
			
			if (wordPos < 0) wordPos = 0;
			
			int wordStart = words[wordPos];
			
			if (wordStart >= charPos - continuePos)
			{
				// guess this is impossible?
				iHitWord = words.Count - 1;
				hitWord = new Rect(new Vector2(0,0), new Vector2(0,0));
				return;
			}
			//get word from string
			
			GetWord(wordPos); // set wordText
			//make rect from the line
			int wordEnd ;
			if (wordText.Length > 0)
				wordEnd = wordStart + wordText.Length ;
			else
				wordEnd = wordStart;
			
			SetSelection(wordStart, wordEnd );
			float left = uiCharVerts[wordStart* 4 +1 ].position.x;
			right = uiCharVerts[wordEnd* 4 - 2].position.x;
			
			
			// x based on characters, y based on hitLine
			
			iHitWord = wordPos;
			hitWord = new Rect(new Vector2(left,hitLine.y), new Vector2(right - left,hitLine.height));
			
			
		}
			
		public bool CastParagraph(int pos, Vector2 worldPoint)
		{
			if (paragraphs.Count < 1 || pos < 0 || pos >= paragraphs.Count  )
			{				
				
				return false; // I should definitely set them to something
			}
			
			Vector2 correctPoint = worldPoint
			;
			
			// anchor at topleft
			
			
			// position, rotate, and scale it (/= scaleFactor)
			correctPoint = body.InverseTransformPoint(correctPoint);
			
			// correctPoint.y > 0
			// the first visible line
			
			return HitParagraphLocal(pos, correctPoint);
		}
		public void UnsetEdit()
		{
			
			// as big as the container
			float height = body.sizeDelta.y;
			float width = textWidth;
			Vector2 rmin = new Vector2(0, 0);
			Vector2 rmax = new Vector2(width, height);
			hitParagraph = new Rect(rmin, rmax);
			iHitParagraph = -1;
			
			// a line with the heigh of one row
			
			rmax.y = endLineHeight;
			hitLine = new Rect(rmin, rmax);
			iHitLine = -1;
			
			// a word... random width
			rmax.x = 15f;
			hitWord = new Rect(rmin, rmax);
			iHitWord = -1;
			
			// a letter... random width
			
			rmax.x = 1f;
			hitChar = new Rect(rmin, rmax);
			iHitChar = -1;
			
			// editPos. zero
			editPos = Vector2.zero;
				
		}
		public bool HitParagraphLocal(int pos, Vector2 point)
		{
			Vector2 rmin = new Vector2();
			Vector2 rmax = new Vector2();
			
			bool hit = false;
			
			int spos = paragraphs[pos ]; 
			
			// this is the line it starts at
			int sline = 0;
			

			while (sline  < lines.Count && lines[sline ] < spos) 
			{
				
				sline ++;
			}
			
			// width is based on the container width, not character position
			
			float width = textWidth;
			
			
			// top of first line
			// collision entry check. is the pointer above the first line?
			
			
			float top = 0 - sline * lineHeight ; // hack?
			
			if (top < point.y) return false;
			
			// bottom of first line
			int nline = sline ;
			float bottom = top - endLineHeight; 
			
			hit = (bottom < point.y);
			if (point.y > 0 ) 
			{
				
				return true;
			}
			if (hit) 
			{
				// LINE
				rmin.Set(0,  bottom );
				rmax.Set(width,  endLineHeight);
			
				hitLine = new Rect(rmin, rmax); // thi can't work. I need local-position, size
				iHitLine = sline ;
				
			}
				
				
			int npos; // the last character of this paragraph, or a placeholder
			int n = pos + 1;
			if (n < paragraphs.Count) 
			{
				npos = paragraphs[n] ;
			}
			else
				npos = charPos - continuePos;
			
			while (nline < lines.Count && lines[nline] < npos) 
			{
				// WHOLE PARAGRAPH
				
				if (!hit) 
				{
					hit = (bottom < point.y);
					if (hit) 
					{
						// FIRST SUCCESSFUL LINE
						rmin.Set(0,  bottom  );
						rmax.Set(width,  endLineHeight);
					
						hitLine = new Rect(rmin, rmax);
						
						iHitLine = nline;
					}
				}
				bottom -= lineHeight;
				nline ++;	
			}
			
			
			if (hit)
			{
				// PARAGRAPH vars
				
				rmin.Set(0,  bottom );
				rmax = new Vector2(width,  top - bottom - endLineHeight);
			
				iHitParagraph = pos;
				hitParagraph = new Rect(rmin, rmax);
			}
			
			
			
			return hit;
		}
		
		
		
		// if letters are taller than blanks, the letter height needs adding
		public float endLineHeight
		{get{
			float h;
			if (fontHeight > lineHeight)
			{
				// edge height
				h = fontHeight -lineHeight;
			}
			else h = fontHeight;
			
			return h ;
			}
		}
		
		// the white space past the bottom of body
		public float trimmedLineHeight
		{get{
			
			float h = 0;
			if (fontHeight < lineHeight)
			{
				h = lineHeight - fontHeight;
			}
			
			return h;
			}
		}
		public Vector2 nodeSize
		{get{
			
			if (s.Length < 1) return new Vector2(
			textWidth + 6, 0f);
			
			float h = endLineHeight;
			float hv = contentHeight;
			
			
			return new Vector2(
			textWidth + 6,
			(hv + h ) - body.offsetMax.y + 6   // padding for all 4 sides
			);
		}}
//###		
		public void AppendChar(char c)
		{
			// added to s, and physically alters size
			_AppendMark(c);
			
			for (int i = 0; i < 4; i ++)
			{
				uiCharVerts.Add(_uiMarks[i]);
				
			}
			// this gets added to the calculated chars
			s.Append(c);
			
			
			Text t = bodytext;
			characterInfo = new CharacterInfo();
			font.GetCharacterInfo(c, out characterInfo, t.fontSize);
			
			float advance = characterInfo.advance;
			
			wrapPos += advance; // not true indent
			
		}
		
		protected UIVertex[] _uiMarks = new UIVertex[4];
		public void AppendMark(char c)
		{
			
			_AppendMark(c);
			for (int i = 0; i < 4; i ++)
			{
				
				uiVerts.Add(_uiMarks[i]);
				
			}
			
		}
		// this is a pretty generic way to write text into neat rows (the neat way)
		public float GetMarkUnder(char c)
		{
			
			characterInfo = new CharacterInfo();
			
			
			Text t = bodytext;
			font.GetCharacterInfo(c, out characterInfo, t.fontSize);
			
			float offset = characterInfo.minY;
			
			float ypos =  -offset - medianAscent ;
			// BL
			
			
			return ypos;
		}
		CharacterInfo characterInfo;
		
		
		public void RefreshMarks()
		{
			if (recent.Length < 1) return;
			dirty = true; 
			
			UIVertex ui;
			char c = '-'; // it's a mark
			Text t = bodytext;
			font.GetCharacterInfo(c, out characterInfo, t.fontSize);
			
			int pos = 0;
			while (pos < uiVerts.Count)
			{
				
				ui = uiVerts[pos + 0];
				ui.uv0 = characterInfo.uvBottomLeft;
				uiVerts[pos + 0] = ui;
				ui = uiVerts[pos + 3];
				ui.uv0 = characterInfo.uvBottomRight;
				uiVerts[pos + 3] = ui;
				ui = uiVerts[pos + 1];
				ui.uv0 = characterInfo.uvTopLeft;
				uiVerts[pos + 1] = ui;
				ui = uiVerts[pos + 2];
				ui.uv0 = characterInfo.uvTopRight;
				uiVerts[pos + 2] = ui;
				
				pos += 4;
			}
			
		}
		public void RefreshChars()
		{
			if (s.Length < 1) return;
			dirty = true; 
			Text t = bodytext;
			
			UIVertex ui;
			char c;
			int pos = 0;
			
			
			for (int i = 0 ; i < s.Length ; i++)
			{
				c = s[i];
				font.GetCharacterInfo(c, out characterInfo, t.fontSize);
				
				ui = uiCharVerts[pos + 0];
				ui.uv0 = characterInfo.uvBottomLeft;
				uiCharVerts[pos + 0] = ui;
				ui = uiCharVerts[pos + 3];
				ui.uv0 = characterInfo.uvBottomRight;
				uiCharVerts[pos + 3] = ui;
				ui = uiCharVerts[pos + 1];
				ui.uv0 = characterInfo.uvTopLeft;
				uiCharVerts[pos + 1] = ui;
				ui = uiCharVerts[pos + 2];
				ui.uv0 = characterInfo.uvTopRight;
				uiCharVerts[pos + 2] = ui;
				
				pos += 4;
			}
			
		}
		public void _AppendMark(char c)
		{
			dirty = true; // changes vertices, all 'var s' characters get 4 vertices so it syncs with string position
			
			characterInfo = new CharacterInfo();
			
			
			Text t = bodytext;
			font.GetCharacterInfo(c, out characterInfo, t.fontSize);
			
			
			Vector2 max = new Vector2(characterInfo.maxX , characterInfo.maxY);
			Vector2 min = new Vector2(characterInfo.minX , characterInfo.minY);
			
			float advance = characterInfo.advance;
			if (c == ' ') max.x = advance;
			
			UIVertex newVertex ;
			
			float xpos = wrapPos - bodyIndent ;
			
			float ypos = -textHeight - fontHeight + medianAscent ;
			// BL
			newVertex = UIVertex.simpleVert;
			newVertex.color = t.color;
			newVertex.position = new Vector3(xpos + min.x, ypos + min.y,body.position.z);
			newVertex.uv0 = characterInfo.uvBottomLeft;
			_uiMarks[0] = newVertex;
			
			// BR
			newVertex = UIVertex.simpleVert;
			newVertex.color = t.color;
			newVertex.position = new Vector3(xpos + max.x, ypos + min.y,body.position.z);
			newVertex.uv0 = characterInfo.uvBottomRight;
			_uiMarks[3] = newVertex;
			
			//TL
			newVertex = UIVertex.simpleVert;
			newVertex.color = t.color;
			newVertex.position = new Vector3(xpos + min.x, ypos + max.y,body.position.z);
			newVertex.uv0 = characterInfo.uvTopLeft;
			_uiMarks[1] = newVertex;
			//TR
			newVertex = UIVertex.simpleVert;
			newVertex.color = t.color;
			newVertex.position = new Vector3(xpos + max.x, ypos + max.y ,body.position.z);
			newVertex.uv0 = characterInfo.uvTopRight;
			_uiMarks[2] = newVertex;
			
			
		}
		
		public RectTransform lineBreak ;
		public RectTransform currentLine ;
		public List<RectTransform> linebreaks = new List<RectTransform>();
		public virtual void Break(){
			// insert a line break / new header replaces line break
			if (lineBreak == null) Debug.LogError("no linebreak", this);
			else
			{
				currentLine = GameObject.Instantiate(lineBreak);
				linebreaks.Add(currentLine);
			}
		}
		
		public int lineStartPos = 0;
		
		public void NewLine()
		{
			lineStartPos = charPos - continuePos  + 1 ;
			lines.Add(lineStartPos);
			
			trueIndent = bodyIndent;
			rowPos ++;
			textHeight += lineHeight ;
			
			CheckLeftMargin(); // indents amt
			CheckRightMargin(); // cancels wrapwidth
			
			wrapPos = trueIndent; // this is probably replaced soon anyway
			if (textHeight > contentHeight) contentHeight = textHeight;
		}
		public void Insert(RectTransform rt)
		{
			NewLine();
			
			rt.SetParent(widget, false);
			rt.localPosition = _insert;
			
			float bottom = _insert.y - rt.sizeDelta.y ; // textheight has scalefactor
			while(textHeight < bottom)
				NewLine();
			
		}
		public Vector2 _insert
		{
			get{
				
			return new Vector2(trueIndent -bodyIndent, -textHeight);
			}
		}
		
		
		public void InsertInline(RectTransform rt)
		{
			if (!rt) return;
			rt.SetParent(widget, false);
			
			
			
			// set world position
			Vector2 newPos = insertInline ;
			Vector2 size =  rt.sizeDelta ;
			newPos.y -=  size.y / 2f ;
			//newPos.x += size.x / 2f ;
			rt.localPosition = newPos;
			
			float amt = size.x;
			wrapPos += amt; // now it's not the true indent
			
			
			if (wrapPos  > maxWidth) textWidth = maxWidth;
			else if (wrapPos  > textWidth) textWidth = wrapPos;
			if (wrapPos > wrapWidth) NewLine();
		}
		
		// this would be the 'local cursor position'
		public Vector2 insertInline
		{get{
			cursorPos.Set(wrapPos, -textHeight);
			return cursorPos  ;
			}
		}
		
		protected float leftInsertBottom = 0;
		protected float leftInsertIndent = 0;
		protected float rightInsertBottom = 0;
		protected float rightInsertIndent = 0;
		// right indent is part of wrapwidth
		protected bool doLeftCheck = false;
		protected bool doRightCheck = false;
		protected void CheckRightMargin()
		{
			if (!doRightCheck) return;
			
			
			if (rightInsertBottom >  -textHeight  - trimmedLineHeight )
			{
				doRightCheck = false;
				wrapWidth = defaultWrapWidth - bodyIndent;
				maxWidth = defaultMaxWidth;
				rightInsertIndent = 0;
			}
			
			
		}
		protected void CheckLeftMargin()
		{
			if (!doLeftCheck) return;
			if (indentAt == rowPos) return;
			
			if (leftInsertBottom >  -textHeight - trimmedLineHeight ) 
			{
				doLeftCheck = false;
				leftInsertIndent = 0f;
			}
			
			indentAt = rowPos ;
			
			trueIndent += leftInsertIndent;
			
		}
		
		public void InsertLeft(RectTransform rt)
		{
			rt.SetParent(widget, false);
			
			rt.anchorMin = anchorTopLeft; // 0x = left, 1y = top
			rt.anchorMax = anchorTopLeft; 
			
			
			Vector2 newPos = insertLeft;
			
			
			
			Vector2 size = rt.sizeDelta ;
			newPos.x += size.x / 2f + leftInsertIndent;
			newPos.y -= size.y / 2f;
			rt.anchoredPosition = newPos;
			
			
			//rt.offsetMin = newPos;
			//rt.offsetMax = newPos + size;
			
			
			doLeftCheck = true;
			leftInsertIndent += size.x;
			trueIndent += size.x;
			leftInsertBottom = newPos.y - size.y / 2f;
			
			wrapPos += size.x ; // if I move this first, it messes up
			MoveLine();
			
			if (leftInsertBottom < -contentHeight) contentHeight = -leftInsertBottom;
			
			
			if (wrapPos > wrapWidth) NewLine();
		}
		
		
		public Vector2 anchorTopLeft= new Vector2(0,1); 
		protected Vector2 _insertLeft= new Vector2(); 
		public Vector2 insertLeft
		{get{
			_insertLeft.Set(0, _insert.y);
		return _insertLeft;
		}}
		public void InsertRight(RectTransform rt)
		{
			rt.SetParent(widget, false);
			rt.anchorMin = anchorTopLeft; // 0x = left, 1y = top
			rt.anchorMax = anchorTopLeft; // when the parent resizes, this will remain in the far right
			
			Vector2 newPos = insertRight;
			
			Vector2 size = rt.sizeDelta ;
			
			newPos.x -= size.x / 2f + rightInsertIndent;
			newPos.y -= size.y / 2f;
			rt.anchoredPosition = newPos;
			
			//rt.offsetMin = newPos;
			//rt.offsetMax = newPos + size;
			
			
			
			doRightCheck = true;
			
			
			wrapWidth -= size.x ; // this is where the normal wrap occurs right?
			maxWidth -= size.x;
			
			rightInsertIndent += size.x;
			rightInsertBottom = newPos.y - size.y / 2f;
			
			WrapCut(); // quickly check if it's past the margin and cut the current line
			
			//(on the off chance this is huge it may have to insert to content height)
			if (rightInsertBottom < -contentHeight) contentHeight = -rightInsertBottom;
			
			
			if (wrapPos > wrapWidth) NewLine();
		}
		public Vector2 anchorTopRight= new Vector2(1,1); 
		
		
		protected Vector2 _insertRight= new Vector2(); 
		
		public Vector2 insertRight
		{get{
			
			_insertRight.Set(textWidth, _insert.y );
			return _insertRight;
		}}
		
		
		public DText procText {
			get{return parser.procText;}
			set{parser.procText = value;}
		}
		public DText goalText {
			get{return parser.goalText;}
			set{
				parser.goalText = value;
				
			}
		}
		public DText streamedText{
			
			get{return parser.streamedText;}
		}
		
		
		//text
		public Text headtext;
		public Text bodytext;
		// transf
		public RectTransform head;
		public RectTransform body;
		public RectTransform bodyframe;
		public RectTransform headframe;
		// widget img
		public RectTransform icon;
		
		public RectTransform widget; // so basically I make this the parent, but the widgets can be in any position. done deal.
		
		public StringBuilder recent {get{ 
			if (streamedText != null)
				return streamed;
			else
				return new StringBuilder();
			}
		}
		
		// this is basically the entire conversation split or joined.
		public DText filteredText {get{ return parser.filteredText;}}
		
		public StringBuilder future {get{ 
			if (goalText != null)
				return parser.filteredText.storedText;
			else
				return new StringBuilder();
			}
		}
		
		public bool isInit = false;
		public bool anchored = true;
		public bool visible = true;
		
		
		protected void DestroyChildren(Transform p)
		{
			foreach (Transform child in p)
			{
				GameObject.Destroy(child.gameObject);
			}
		}
		public float scaleFactor {
			get{
				if (body.lossyScale.y > 0.01f)
				return body.lossyScale.y;
			
				return 1f;
			}
		}
		
		protected float iconIndent ;
		
		
		public void SetLayout()
		{
			
			
			Text t = bodytext;
			
			font.RequestCharactersInTexture(AllFontChars.value, t.fontSize, t.fontStyle);
			wrapPos = trueIndent;
			
				
				
			fontHeight = bodytext.fontSize; // for letter height only
			
			// font ascent is probably not what I think and this will only work... for horizontal text?
			medianAscent = font.ascent / 2f;
			
			lineHeight = fontHeight *bodytext.lineSpacing;
			
				
		}
		protected float medianAscent= 0f;
		
		// first excuse to rebuild the entire text
		protected int indentAt = -1;
		protected float trueIndent = -1;
		protected float bodyIndent = 11;
		
		protected bool useIcon = true;
		protected void UseIconIndent()
		{
			
			// use the same methods as insert left
			
			iconIndent = icon.sizeDelta.x ;
			
			
			doLeftCheck = true;
			leftInsertIndent += iconIndent;
			trueIndent += iconIndent;
			
			if (leftInsertBottom > -iconIndent) leftInsertBottom = -icon.sizeDelta.y - body.offsetMax.y;
			
			
			wrapPos += iconIndent ; // if I move this first, it messes up
			MoveLine();
			
			if (leftInsertBottom < -contentHeight) contentHeight = -leftInsertBottom;
			
			
			if (wrapPos > wrapWidth) NewLine();
		}
		public void UseIcon(RectTransform rt)
		{
			// this could mean I have to redo the entire layout
			if (icon == null) 
			{
				return;
			}
			DestroyChildren(icon);
			
			
			rt.anchorMin = icon.anchorMin;
			rt.anchorMax = icon.anchorMax;
			
			
			rt.SetParent(icon, false);
			
			// this is determined by the pivot though
			Vector2 size = icon.sizeDelta;
			Vector2 newPos = new Vector2();  // the _insert position is at the top
			newPos.y -= size.y ;
			
			rt.offsetMin = newPos;
			rt.offsetMax = newPos + size;
			
			//rt.anchoredPosition = newPos;
			
			
			useIcon = true;
			UseIconIndent();
			newOffset.Set(trueIndent, headframe.offsetMin.y);
			headframe.offsetMin = newOffset;
			//s = iconIndent + s;
		}
		public void DestroyIcons()
		{
			
			useIcon = false;
			if (icon == null) return;
			DestroyChildren(icon);
			newOffset.Set(0f, headframe.offsetMin.y);
			
			headframe.offsetMin = newOffset;
			//s =  s.Substring(iconIndent.length; // idk
		}
		public void DestroyWidgets()
		{
			
			if (icon == null) return;
			DestroyChildren(widget);
		}
		
		protected Vector2 newOffset = new Vector2();
		public void UseTitle()
		{
			if (!headframe.gameObject.activeSelf) headframe.gameObject.SetActive(true);
			
			newOffset.Set(body.offsetMax.x, -28f);
			body.offsetMax = newOffset;
		}
		public void SetTitle(string s)
		{
			
			if (head == null) return;
			headtext.text = s;
			UseTitle();
		}
		public void DestroyTitle()
		{
			
			if (head == null) return;
			headtext.text = "";
			if (headframe.gameObject.activeSelf) 
				headframe.gameObject.SetActive(false);
			
			newOffset.Set(body.offsetMax.x, -3f);
			body.offsetMax = newOffset;
			
		}
		
		
		Material renderMat;
		
		public void Load()
		{
			lastTick = Time.time;
			
			if (isInit) return;
			Clear(); // I wasn't calling play, and this initializes list variables
			if(localAction == null) AddAllActions(); // #included
			if(parser == null) 
			{
				parser = new DisplayParser(localAction);
			
				parser.Load();
			}
			
			isInit = true;
			gathering = false;
			done = true;
			
			
			
			
			if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
			maxWidth = defaultMaxWidth;
			wrapWidth = defaultWrapWidth - bodyIndent; // = body.sizeDelta.x; // 6 for pad, 8 for left margin. or I could enter this in inspector
			
			if (head == null) head= rectTransform.Find("Head") as RectTransform;
			if (headtext == null) headtext= head.GetComponent<Text>();
			
			if (icon == null) icon= rectTransform.Find("Icon") as RectTransform;
			
			if (body == null) body= rectTransform.Find("Body") as RectTransform;
			if (bodytext == null) bodytext= body.GetComponent<Text>();
			
			renderMat = Instantiate(bodytext.materialForRendering);
			if (font == null) font = bodytext.font;
			Font.textureRebuilt += OnRebuild;
			
			
			bodytext.text = "";
			if (bodytext.enabled) bodytext.enabled = false;
			if (bodyRenderer== null) bodyRenderer= bodytext.gameObject.GetComponent<CanvasRenderer>();
			
			if (bodyframe == null) bodyframe=body.parent as RectTransform;
			if (headframe == null) headframe=head.parent as RectTransform;
			
			if (canvas == null) canvas = GetComponentInParent<Canvas>();
			if (canvasGroup == null) canvasGroup = GetComponentInParent<CanvasGroup>();
			
			widget = body;
			wrapPos = trueIndent = bodyIndent; 
			SetLayout();
			
			
			
		}
		
		protected int maxViable = 250; // really I should count the rows of the text
		protected int maxRows= 24;
		protected int rowPos= 0;
		public int prevPos = -1;
		public int charPos = 0;
		protected int continuePos = 0; // for smart calculations...
		protected float cps = 500f;
		
		protected float charAdvance = 0f;
		
		protected StringBuilder s = new StringBuilder();

		protected StringBuilder streamed 
		{
			get{ 
			return streamedText.storedText;
			}
		}
		protected StringBuilder sFuture 
		{
			get{ return future; }
		}
		public bool done = false;
		public bool gathering = false;
		
		public void Continue()
		{
			
			
			
			continuePos = charPos ;
			parser.Continue();
		}
		
		//public void Replay(){} // I'm going to need this to rebuild everything eventually, with the parser's help
		public void Play()
		{
			Clear();
			done = false;
			gathering = true;
			
			
			
			
		}
		protected void CleanLists()
		{
			uiVerts.Clear();
			uiCharVerts.Clear();
			
			paragraphs.Clear(  );
			paragraphs.Add( 0 );
			lines.Clear(  );
			lines.Add( 0 );
			words.Clear(  );
			
			
		}
		public void Clear()
		{
			UnsetEdit();
			charAdvance = 0f;
			charPos = 0;
			s.Clear();
			
			CleanLists();
			
			dirty = true;
			contentHeight = textHeight = 0f;
			textWidth = 0f;
			rowPos = 1;
			indentAt = -1;
			continuePos = 0;
		}
		// this would be potentially missing a change in user, icon, or header
		// this is for trying to connect things?
		public void PlayUnsafe()
		{
			done = false;
			gathering = true;
			
			// doesn't look like play unsafe will work with the text render behavior very well here
			charPos = 0;
			continuePos = -s.Length;
			// bodytext.text = s;
			
		}
		
		
		
		protected float wrapWidth = 281f ; // how wide before wrap
		protected float defaultWrapWidth = 366f; // how wide before wrap
		protected float defaultMaxWidth = 449f ; // 
		protected float maxWidth = 329f ; // how wide before I have to wrap? 7f is added for the -
		protected bool pastEdge = false; 
		protected bool pastCap = false; 
		public float wrapPos = 0f;
		protected float textWidth = 0f;
		protected float textHeight = 0f;
		protected float contentHeight = 0f;
		public float lineHeight = 0f;
		public float fontHeight = 0f;
		protected Vector2 cursorPos = new Vector2();
		
		
		protected void WrapEarly()
		{
			if (s.Length < 1) 
				return;
			char c = s[s.Length - 1];
			if (c == '\n') return;
			
			
			if (!(System.Char.IsWhiteSpace(c)  ))
				return;
			
			pastEdge = false;
			
			
			Text t = bodytext;
			
			characterInfo = new CharacterInfo();

			float fPos = wrapPos;
			
			//needs goal text for predicting future, or proc text to read mind
			
			for (int i = charPos + 1;i < sFuture.Length && !pastEdge;i++)
			{
				c = sFuture[i];
				
				if ((System.Char.IsWhiteSpace(c)  )) break;
			
				font.GetCharacterInfo(c, out characterInfo, t.fontSize);
				
				fPos += characterInfo.advance;
				
				pastEdge = fPos > wrapWidth ;
				
			}
			
			
			if (pastEdge)
				NewLine();
			
		}
		protected void WrapCut()
		{
			// incomplete...
			// here I need to find out what needs to be removed, but I just assume
			Text t = bodytext;
			
			characterInfo = new CharacterInfo();
			
			// I'm checking if a dash puts it over the edge
			
			char c = '-';
		
			float advance ;
			if (font.GetCharacterInfo(c, out characterInfo, t.fontSize))
				advance = characterInfo.advance;
			else
				advance = 7f;
			
			if (wrapPos + advance > wrapWidth)
			{
				int fin = charPos - continuePos ; // guess I don't know where fin is
				
				// here I could set up a method to walk vertices from the right
				int rcut = fin-3 ;
				
				// gather the verts
				UIVertex[] verts ;
				GetCut(rcut, fin , out verts);
				
				// new, this supposedly adds a dash where it just cut
				if (verts.Length > 0)
				{
					float cutWidth = verts[verts.Length - 1].position.x - verts[0].position.x;
					wrapPos -= cutWidth;
					AppendMark('-'); 
				}
				NewLine();
				lines[lines.Count - 1] -= 4;
				
				// reposition the verts
				float lower = GetMarkUnder(s[rcut]);
				float bottomLine = verts[0].position.y + lower;
				
				MoveCut(bottomLine, verts );
				
				ReplaceCut(rcut, fin, verts);
				//wrapPos = trueIndent; // supposedly that reset it
				wrapPos = verts[verts.Length - 1].position.x + bodyIndent;
			}
		}
		
		public void MoveLine( )
		{
			// i obviously need to define 
			
			int firstChar = lineStartPos;
			if (s.Length - 1 < lineStartPos) return;

			if (firstChar >= charPos - continuePos ) return;
			
			char c = s[firstChar];
			
			
			while ((System.Char.IsWhiteSpace(c)  ) && firstChar + 1< charPos - continuePos)
			{
				// seems like a strange thing to do
				firstChar ++;
				c = s[firstChar];
			}
			
				
			
			MoveSequence(firstChar , charPos - continuePos);
		}
		
		// this version takes integers and moves them around
		public void MoveSequence( int startChar, int endChar )
		{
			
			UIVertex[] verts ;
			GetCut(startChar, endChar , out verts);
			float lower = GetMarkUnder(s[startChar]);
			float bottomLine = verts[0].position.y + lower;
			
			MoveCut(bottomLine, verts );
			ReplaceCut(startChar, endChar, verts);
		}
		
		public void MoveSequence( int startChar, int endChar, Vector2 newPosition, Vector2 pivot = new Vector2())
		{
			
			UIVertex[] verts ;
			GetCut(startChar, endChar , out verts);
			
			// the bottom left side 
			int maxIndex = verts.Length - 1;
			float left = verts[0].position.x;
			
			float width = verts[maxIndex].position.x - left;
			
			width *= pivot.x;
			float lower = GetMarkUnder(s[startChar]);
			float bottom = verts[0].position.y + lower;
			
			
			lower = GetMarkUnder(s[endChar]);
			float height = verts[maxIndex].position.y + lower - bottom + lineHeight;
			
			height *= pivot.y;
			
			Vector2 fromPosition = new Vector2(left + width,  bottom + height);
			
			
			MoveCut(verts, fromPosition, newPosition);
			ReplaceCut(startChar, endChar, verts);
		}
		public void MoveCut(float bottomLine, UIVertex[] verts)
		{
			
			// a line replace operation
			cursorPos.Set(trueIndent - bodyIndent, -textHeight);
			
			// the bottom left side 
			
			Vector2 fromPosition = new Vector2(verts[0].position.x,  bottomLine + fontHeight);
			
			MoveCut(verts, fromPosition, cursorPos);
		}
		public void MoveCut( UIVertex[] verts, Vector2 fromPosition, Vector2 newPosition)
		{
			if (verts.Length < 1) return;
			float diffx = newPosition.x - fromPosition.x ; 
			float diffy = newPosition.y  - fromPosition.y ;
			for (int i = 0 ; i < verts.Length ; i ++ )
			{
				
				verts[i].position.x += diffx;
				verts[i].position.y += diffy;
			}
			
		}
		public void GetCut(int startPos, int endPos, out UIVertex[] verts)
		{
			// adds the vertices
			
			int startVertex = startPos * 4;
			int endVertex = endPos  * 4 ;
			
			verts = new UIVertex[ endVertex - startVertex];
			
			
			int count = 0;
			int len = verts.Length;
			for (int i = startVertex ; i < endVertex && i < uiCharVerts.Count && count < len; i ++ )
			{
				
				verts[count] 
				= uiCharVerts[i];
				
				
				count ++;
			}
				
		}
		public void ReplaceCut(int startPos, int endPos, UIVertex[] verts)
		{
			// adds the vertices
			
			int startVertex = startPos * 4;
			int endVertex = endPos  * 4 ;
			
			int count = 0;
			for (int i = startVertex ; i < endVertex && i < uiCharVerts.Count && count < verts.Length; i ++ )
			{
				uiCharVerts[i] = verts[count];
				count ++;
			}
			
			dirty = true; // changes vertices

		}
		
		protected void WrapLate()
		{
			// doesn't work because more than one character per
			if ( s.Length < 1) 
			{
				return;
			}
			char c = s[s.Length - 1];
			if (c == '\n' ) return;
			
			
			// the current x cursor position should be wrapPos
			
			Text t = bodytext;
			
			characterInfo = new CharacterInfo();
			
			// I'm checking if a dash puts it over the edge
			
			c = '-';
		
			float advance ;
			if (font.GetCharacterInfo(c, out characterInfo, t.fontSize))
				advance = characterInfo.advance;
			else
				advance = 7f;
			
			// don't change wrappos (the cursorpos)
			
			if (wrapPos + advance > textWidth) textWidth= wrapPos + advance;
			
			pastEdge = wrapPos + advance > wrapWidth;
			pastCap = wrapPos + advance > maxWidth ;
			
			if (pastCap && maxWidth > textWidth) textWidth = maxWidth;
				
			c = s[s.Length - 1];
			if (pastEdge 
			&& (System.Char.IsWhiteSpace(c)  ) )
				NewLine();
			else if (pastCap ) 
			{
				
				AppendMark('-'); // doesn't change the position
				
				NewLine();
			}
			
		}
		
		protected char prevc = ' '; // space 
		protected float lastTick = 0f;
		protected void ShowText()
		{
			
			
			charAdvance += cps * (Time.time - lastTick);
			
			lastTick = Time.time;
			char c;
			
			
			// initial check
			int recursions = 0;
			parser.running = true;
			done = parser.done = false;
			while (!done && charAdvance >= 1f)
			{
				parser.Next();
				
				// either a prefab, or eof
				if (charPos >= recent.Length ) 
				{
					// this can also be set 'done' externally
					 if (parser.done || !parser.running)done = true;
					// if (charPos > 0) charPos--;
					recursions++;
					
					if (recursions > 5000) 
					{	
				
						Debug.LogError("inf. recursion while parsing?", this);
						return;
					}
					continue;
				}
				c = recent[charPos];
				
				if ((System.Char.IsWhiteSpace(prevc)  ) 
				
				&& !
				(System.Char.IsWhiteSpace(c)  ))
				{
					wordPos = charPos - continuePos ;
					
					words.Add(wordPos);

					// this means last word is very frequently not pointing at anything 
				}
				
				if (c == '\n')
				{
					AppendChar(c);
					paragraphPos = charPos - continuePos;
					paragraphs.Add(  paragraphPos );
					NewLine();
				}
				else 
				{
					AppendChar(c);
					
					// if not psychic
					WrapLate();
				}
					
				prevc = c;
					
				//if (predict) WrapEarly(); // I can read minds
				
				
				charAdvance -= 1f;
				charPos ++; 
				
			}
			
			
			// also the user name/icon could be determined if there is one
			
			//rowPos = s.Split('\n').Length ;
			
			
			
			//if (rowPos >= maxRows)
			//{
			//	int startPos = s.IndexOf('\n', rowPos - maxRows) + 1;
			//	
			//	s = s.Substring(startPos, s.Length - startPos);
			//	
			//	
			//}
			//
			
			
			
			
			//if (s.Length <= maxViable)
			//{
			//	bodytext.text = s;
			//}
			//else
			//{
			//	bodytext.text = s.Substring(s.Length - maxViable, maxViable);
			//}
		}

		
		
		// this is the out mesh, and I could technically append to it if the mesh didn't have to be formed every frame.
		protected Mesh mesh;
		protected Font font;
		List<UIVertex> uiVerts = new List<UIVertex>();
		List<UIVertex> uiCharVerts = new List<UIVertex>();
		
		// if the text changes this should be called. and it should do something useful.
		
		public bool dirty = false;
		public bool dirtySelection = false;
		
		protected List<UIVertex> allVerts = new List<UIVertex>();
		protected void RefreshTextRenderer()
		{
			
			if (!dirty) return; 
			dirty = false;
			//font.RequestCharactersInTexture(AllFontChars.value, bodytext.fontSize, bodytext.fontStyle);
			//bodytext= body.GetComponent<Text>();
			//font = bodytext.font;
			
			// TEXT
			
			// param 2 : makes it textured
			
			bodyRenderer.SetMaterial(renderMat, font.material.mainTexture); 
			
			// show
			allVerts.Clear();
			allVerts.AddRange(uiCharVerts);
			allVerts.AddRange(uiVerts);
			
			
			
			
			
			
			if (allVerts.Count >= 65534) 
				return; // else this freezes it
			if (allVerts.Count < 1) 
				 // blank.
				bodyRenderer.Clear();
			else
			{
				
				// mesh
				Vector3[] newVertices = new Vector3[allVerts.Count];
				Vector2[] newUV = new Vector2[allVerts.Count];
				Color[] newColors = new Color[allVerts.Count];
				int triCount = (int)(allVerts.Count * 1.5);
				int[] newTriangles = new int[triCount];
				
				
				
				for ( int i = 0; i < allVerts.Count; i ++)
				{
					newVertices[i] = allVerts[i].position;
					newUV[i] = allVerts[i].uv0;
					newColors[i] = allVerts[i].color;
					
				}
				
				int v = 0;
				for ( int i = 0; i < triCount; i += 6)
				{
					newTriangles[i] =   v+0; 
					newTriangles[i+1] = v+3; 
					newTriangles[i+2] = v+1; 
					newTriangles[i+3] = v+3; 
					newTriangles[i+4] = v+1; 
					newTriangles[i+5] = v+2; 
					
					v += 4;
				}
				
				if (!mesh) mesh = new Mesh();
				else mesh.Clear();
				
				mesh.vertices = newVertices;
				mesh.uv = newUV;
				mesh.colors = newColors;
				mesh.triangles = newTriangles;
				mesh.RecalculateNormals();
				
				bodyRenderer.SetMesh( mesh );
				
			}
			
			// but if I want to modify the vertices later this won't work.
		}
		
		protected void RefreshTextVars()
		{
			


			RefreshTextRenderer();
			if (s.Length < 1) return;
			
			// necessary
			
			
			
// this doesn't seem to work

			
			cursorPos.Set(wrapPos, -textHeight);
			
			//font = bodytext.font;
			fontHeight = bodytext.fontSize; // for letter height only
				
			if (textHeight > contentHeight) contentHeight = textHeight;
			
			if (wrapPos  > textWidth) textWidth= wrapPos ;
			
			newOffset.Set(textWidth - 6, contentHeight + endLineHeight);
			body.sizeDelta = newOffset;
		}
		public virtual void Step()
		{
			ShowText();
			
			RefreshTextVars();
			
			
			
		}
		public void OnRebuild(Font changedFont){
				
			if (changedFont != font)
				return;
			
			// seems like I need to rebuild the entire mesh. meaning now I need to store another range of characters
			
			
			RefreshChars();
			RefreshMarks();
			
			
			RefreshTextVars();
		}
		void OnDestroy()
		{
			Font.textureRebuilt -= OnRebuild;
		}
		protected override void OnEnable(){
			base.OnEnable();
			if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
			
		}
		protected override void OnUpdate(){
			base.OnUpdate();
			
			font.RequestCharactersInTexture(AllFontChars.value, bodytext.fontSize, bodytext.fontStyle);
		}
	}
}