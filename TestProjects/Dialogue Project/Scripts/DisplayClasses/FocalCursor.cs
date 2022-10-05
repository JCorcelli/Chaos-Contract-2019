using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace SelectionSystem
{
	public class FocalCursor : UpdateBehaviour {
		// basically this is a supercursor class.
		// consider 2D - 3D meld
		
		
		public Image[] recolorThese ;
		public RectTransform highlight ;
		public RectTransform rectTransform;
		public RectTransform defaultParent;
		public RectTransform defaultEffect;
		public Color defaultColor = Color.white;
		public Color color = Color.white;
		
		
		public Vector2 defaultSize; // effect parent, maybe a canvas
		public RectTransform effectOffTransform; // effect parent, maybe a canvas
		protected bool isDefault = false; // default parent
		protected bool visible = true; // default parent
		
		
		// effect is in the center of the body. a rect with any components .
		public RectTransform effect;
		public TextAsset cursorInfo;
		protected string currentEffect = "";
		
		protected IEnumerator Start() { 
			yield return new WaitForSeconds(2f);
			if (!CursorInfo.infoObjectExists)
			{
				Debug.Log("No cursor info instantiated. Add a CursorInfo to get started.", cursorInfo);
			}
		}
		protected override void OnEnable () { 
			base.OnEnable();
			// child is highlight
			CursorInfo.cursorObjectExists = true;
			rectTransform = GetComponent<RectTransform>();
			CursorInfo.defaultCursorSize = defaultSize = highlight.sizeDelta;
			
			if (effectOffTransform == null)
				Debug.Log("rectTransform for turning off effects needed", gameObject);
				
			if (highlight == null)
				Debug.Log("make a highlight already", gameObject);
			
			
			if (recolorThese.Length < 1) Debug.LogError("the list of recolorThese images isn't populated", gameObject);
			
			if (defaultParent == null)
				defaultParent = rectTransform as RectTransform;
			if (defaultEffect == null)
				Debug.Log("make a default effect already", gameObject);
			
			RectTransform child;
			for (int i = 0; i < highlight.childCount; i ++)
			{
				child = highlight.GetChild(i) as RectTransform;
				child.SetParent(effectOffTransform, false);
				
				
			}
			
			Disconnected();
		}
		
		// SetEffect(RectTransform rt) will use the referenced rt, calls RemoveEffect
		// RemoveEffect(); will place rt, if it isn't moved
		
		protected void DefaultColor() {
			SetColor(defaultColor);
		}
		protected void SetColor() {
			color = CursorInfo.cursorColor;
			SetColor(color);
		}
		protected void SetColor(Color c) {
			Image img ;
			for (int i = 0; i < recolorThese.Length; i ++)
			{
				img = recolorThese[i];
				img.color = c;
			}
			
		}
		protected void DefaultEffect() {
			if (currentEffect == defaultEffect.name) return;
			SetEffect(defaultEffect);
			
		}
		protected void SetEffect() {
			if (CursorInfo.cursorEffect == "") return;
			string s = CursorInfo.cursorEffect;
			CursorInfo.cursorEffect = "";
			if (currentEffect == s) return;
			RectTransform newEffect = effectOffTransform.Find(s) as RectTransform;
			if (newEffect == null) return;
			RemoveEffect();
			SetEffect(newEffect);
			
		}
		protected void SetEffect(RectTransform rt) {
			RemoveEffect();
			rt.SetParent(highlight, false);
			rt.position = highlight.position;
			effect = rt;
			
			currentEffect = effect.name;
		}
		protected void RemoveEffect() {
			
			if (effect == null || effect.parent != highlight) return;
			
			
			effect.SetParent(effectOffTransform, false);
			
			
			
		}
		
		
		// vanish effect / animation / mask
		// a mask will probably prevent this from being visible until a new parent is hit
		protected void Reappear( ) {
			
			if (visible)  return;
				
			visible = true;
			Reparent();
		}
		protected void Disappear( ) {
			
			if (!visible)  return;
				
			visible = false;
			highlight.SetParent(effectOffTransform, false);
			CursorInfo.showCursor = false;
			// deactivate animation?
			DefaultPosition();
		}
		
		// called every frame something is controlling cursor
		protected void RogueMove( ) {
				// disconnect, and no parent required
				if (CursorInfo.cursorVanish)
					Disappear();
				if (CursorInfo.cursorDetachable)
					
					UnfocusedMove(); // normal
				
		}
		protected void Retarget( ) {
			
			if (!CursorInfo.currentCursorInfo.isHovered)
			{
				RogueMove( );
				// could follow the mouse cursor anyway
				return;
			}
			
			if (CursorInfo.cursorRefocus) Reparent();
			
			
			// FOCUSED, WORD
			if (CursorInfo.isFocused)
			{
				// use a square highlighter
				FocusedMove();
				
			}
			else
			// UNFOCUSED, HIDDEN BY MASK, OR POINTING AT THE WINDOW
			{
				UnfocusedMove();
				// use a pen or something.
				// maybe even change the user's cursor
			}
			
			
			// activate animation?
		}
		protected void Reparent( ) {
			
			if ( CursorInfo.cursorParent != null ) 
			{
				isDefault = false;
				if (highlight.parent != CursorInfo.cursorParent)	
				{
					highlight.SetParent(CursorInfo.cursorParent, false);
					
				}
				SetColor();
				
				// and set it to appropriate effect
			}
			else
				Disconnected();
			CursorInfo.cursorRefocus = false;
			
		}
		
		// called if nothing's controlling cursor
		protected void DefaultSize( ) {
			highlight.sizeDelta = defaultSize;
		}
		protected void DefaultPosition( ) {
			highlight.position = defaultParent.position; // like a spawnpoint, maybe
		}
		protected void DefaultParent( ) {
			highlight.SetParent(defaultParent, false);
		}
		protected void Disconnected( ) {
			CursorInfo.releaseCursor = false;
			if (!isDefault) 
			{
				isDefault = true;
				DefaultEffect();
				DefaultSize();
				DefaultParent();
				DefaultPosition();
				DefaultColor();
			}
			
		}
		
		
		protected void FocusedMove( ) {
		
			// size
			highlight.sizeDelta =  CursorInfo.cursorSize ;
			
			// position
			highlight.position = CursorInfo.cursorPosition  ;
			
		}
		protected void UnfocusedMove( ) {
			DefaultSize();
			highlight.position = Input.mousePosition; 
		}
		
		protected override void OnLateUpdate( ) {
			base.OnLateUpdate();
			
			// visible or not visible
			if (CursorInfo.releaseCursor)
				Disconnected(); // isDefault = true;
			if (CursorInfo.showCursor)
				
				Reappear(); // visible = true;
			else
				Disappear(); // visible = false;
			
			
			// parent status
			if (CursorInfo.currentCursorInfo == null)
			{
				// Rogue, no parent
				RogueMove();
			}
			// require parent
			else
				
				Retarget();
			
			SetEffect();
		}
		
	}
}