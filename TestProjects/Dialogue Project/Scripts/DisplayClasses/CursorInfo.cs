using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace SelectionSystem
{
	public class CursorInfo : SelectAbstract {
		// basically a class scattered around into nodes. text. aka CursorNode

		

// STATIC ONLY		
		public static string wordText = "";
		public static bool cursorObjectExists = false;
		public static bool infoObjectExists = false;
		
		public static CursorInfo currentCursorInfo;
		
		public static Color cursorColor = Color.black;
		
		public static Vector2 defaultCursorSize = new Vector2();
		public static Vector2 lineSize = new Vector2();
		public static Vector2 paragraphSize = new Vector2();
		
		public static Vector2 linePosition = new Vector2();
		public static Vector2 paragraphPosition = new Vector2();
		public static Vector2 wordSize = new Vector2();
		public static Vector2 wordPosition = new Vector2();
		
		//##### CURSOR
		public static Vector2 cursorSize = new Vector2();
		public static Vector2 cursorPosition = new Vector2();
		public static bool showCursor = false;
		public static bool cursorDetachable = false; // ignore this info, cursor parent is ignored
		public static bool releaseCursor = false; // ignore this info, cursor parent is ignored
		public static bool cursorVanish = false; // ignore this info, cursor parent is ignored
		public static bool cursorRefocus = false; // ignore this info, cursor parent is ignored
		
		public static RectTransform cursorParent ; // a controlled class cursor uses this
		public static string cursorEffect = "";
		
		public static float scaleFactor = 1f;
		public static float fontHeight  ;
		public static float lineHeight  ;
		public static float lineWidth  ;
		public static Vector3[] fourCorners = new Vector3[4];
// OTHER		

		public RectTransform highlight ;
		public Color color = Color.black ;
		
		public Vector3 mousePos {
			get {return Input.mousePosition;}
		}
		public static bool _isFocused = false;
		public static bool isFocused {
			get{ return _isFocused;}
			set{ _isFocused = value;}
		}
		
		
		
		protected override void OnEnable () { 
			base.OnEnable();
			infoObjectExists = true;
			// child is highlight
		}
		
	}
}