
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;



namespace DialogueSystem
{
	public partial class DisplayNode {
		public RectTransform widgets;
		public RectTransform icons;
		public DLocalAction _localAction;
		public DLocalAction localAction
		{
			get{ return _localAction;}
			set{
				_localAction = value;
				if (parser != null) parser.localAction = value;
			}
		}
		public DisplayNodeActions dnodeAction;
		
		public virtual void AddAllActions() {
			// naturally assuming it's this type of Memory.action
			// so i'm not sure how it'd handle more boxes
			dnodeAction = new DisplayNodeActions();
			dnodeAction.selected = this;
			dnodeAction.AddAllActions();
			
			localAction = dnodeAction.localAction;
		}
		
		
		
	}
	
	public class DisplayNodeActions
	{
		
		public DLocalAction localAction;
		public DisplayNode selected;
		
		public void AddAllActions() {
			// naturally assuming it's this type of Memory.action
			// so i'm not sure how it'd handle more boxes
			localAction = new DLocalAction();
			
			DAction.Use(localAction.a);
			
			DAction action ;
			action = new DAction("NewLine");
			action.Add(NewLine);
			
			action = new DAction("NewHeader");
			action.Add(NewHeader);
			
			action = new DAction("InsertIcon");
			action.Add(InsertIcon);
			action = new DAction("InsertLeft");
			action.Add(InsertLeft);
			action = new DAction("InsertRight");
			action.Add(InsertRight);
			
			action = new DAction("InsertInline");
			action.Add(InsertInline);
			action = new DAction("Inline");
			action.Add(InsertInline);
			
			action = new DAction("Insert");
			action.Add(Insert);
			
			action = new DAction("Popout");
			action.Add(Popout);
			
			action = new DAction("Skip");
			action.Add(Skip);
			
			action = new DAction("New"); // minimize?
			action.Add(New);
			
			action = new DAction("Clear"); // clear
			action.Add(Clear);
			
			action = new DAction("Close"); // like force quitting zone
			action.Add(Close);
			
			action = new DAction("Open"); // ???
			action.Add(Open);
		}
		
		
// changes	
		
		// This creates a divider. Strictly a visual split.
		public void NewLine (string s) {
			// s is just a short header
			selected.Break();
			if (s != "") NewHeader(s);
		} 
		
		
		// Add Title to text. Defaults to text size.
		public void RemoveHeader (string s){ selected.DestroyTitle();}
		
		public void NewHeader (string title){
			selected.SetTitle(title);
		} 
		
		
		// add prefab 's' of unknown size where an icon would be
		public void InsertIcon (string s ){ 
			UseIcon(s);
			
		
		}
		
		public RectTransform prevWidget;
		public void UseWidget( string s) {
			RectTransform rt = prevWidget = CloneWidget(s);
			if (rt == null) 
			{
				Debug.Log("couldn't find " + s);
				return;
			}
			_UseWidget(rt);
		}
		public void UseWidget( RectTransform t) {
			
			t = prevWidget = CloneWidget(t);
			_UseWidget(t);
		}
			
		public void _UseWidget( RectTransform t) {
			
			// here I could add the widget to a list. although the widget gets destroyed.
		}
		public void RemoveIcon( ) {
			
			if (selected.icon != null) selected.DestroyIcons();
			
		}
		
		public RectTransform defaultIconWidget;
		public RectTransform prevIcon;
		public RectTransform FindIcon( string s) {
			
			return selected.icons.Find(s) as RectTransform;
		}
		public RectTransform CloneIcon(string s) {
			if (s == "") return null;
			
			RectTransform rt = FindIcon(s);
			if (rt == null) return null;
			
			return GameObject.Instantiate(rt) as RectTransform;
		}
		
		public void UseIcon( string s) {
			
			RectTransform rt = prevIcon = CloneIcon(s);
			
			if (rt == null) 
			{
				Debug.Log("missing icon " + s );
				return;
			}
			
			
			_UseIcon(rt);
		}
		
		public void UseIcon( RectTransform t) {
			// this must be some complicated thing that already has an image.
			t = prevIcon = CloneWidget(t);
			_UseIcon(t);
		}
		
		public void RemoveWidget() {
			if (selected.widget != null) selected.DestroyWidgets();
			
		}
		
		public RectTransform FindWidget( string s) {
			
			return selected.widgets.Find(s) as RectTransform;
		}
		public RectTransform CloneWidget(string s) {
			if (s == "") return null;
			
			RectTransform rt = FindWidget(s);
			if (rt == null) return null;
			
			return GameObject.Instantiate(rt) as RectTransform;
		}
		public RectTransform CloneWidget(RectTransform rt ) {
			return GameObject.Instantiate(rt) as RectTransform;
		}
		public void _UseIcon( RectTransform t) {
			// I'm placing an icon in the selected box. set boxPos first.
			
			
			selected.UseIcon(t);
			
			
			if (!t.gameObject.activeSelf) t.gameObject.SetActive(true);
			
		}
		
		
		

		public void InsertLeft (string s ){ 
			
			UseWidget(s);
			selected.InsertLeft(prevWidget);
		} 
		public void InsertRight (string s ){ 
			
			UseWidget(s);
			selected.InsertRight(prevWidget);
		} 
		// insert an anchor with prefab 's' at the current text char position.
		// how is this handled? i'm not sure.
		public RectTransform anchor;
		
		//	public TextMagnet defaultMagnet;
		//	public void InsertInline (string s ){
		//	DisconnectedWidget(rt)
		//	set a magnet that kills it when the magnet dies.
		
		// make an anchored widget that displays child when hovered.
		public void InsertInline (string s ){
			UseWidget(s);
			selected.InsertInline(prevWidget);
		}
		public void InsertInlineAnchor (string s ){
			// req. selected.cursorPos; // idk if it's there yet
			/*
			
			rt = CloneWidget(s);
			if (anchor == null) 
				Debug.Log("where's the anchor?", gameObject);
			else
			{
				UseWidget(anchor, false);
				
				anchor.position = selected.cursorPos;
				
				rt.SetParent( anchor.transform as RectTransform);
				rt.position = anchor.position;
			}
			*/
		} 
		
		// Insert Adds An Element straight into the text. Maybe line breaks are in order.
		public void Insert (string s){
			UseWidget(s);
			selected.Insert(prevWidget);
		} 
		// Display A Simple Popout with prefab s
		public RectTransform popout;
		public RectTransform prevPop;
		public void Popout (string s){ 
			GameObject.Destroy(prevPop);
			prevPop = CloneWidget(popout) as RectTransform;
			//prevPop.postion = 
			prevPop.SetParent(popout.parent as RectTransform, false);
			
			RectTransform rt = CloneWidget(s);
			
			rt.SetParent(prevPop, false);
			rt.position = prevPop.position;
			
			prevPop.gameObject.SetActive(true);
		} 
		
		public RectTransform defaultSkip;
		public void Skip (string s){ 
			// I think this is a parser command too.
			
			// if (s != "") // specific thing?
			//else
			if (defaultSkip == null) 
				Debug.Log("where's the skip?");
			else
				UseWidget(CloneWidget(defaultSkip));
		}
		
		
// bigger changes	
// assuming it affect everything

		// maybe new day?
		public void New (string s){  
			//
		} 
		
		// erase the content of break, for effect
		public void Clear (string s){ 
			//break.erase?
		}
		// this probably means the current memory/dbox not the canvas. think about it.
		public void Close(string s) {
			selected.canvas.enabled = false;
			selected.canvasGroup.interactable = false;
			selected.canvasGroup.blocksRaycasts = false;
			selected.canvasGroup.alpha = 0f;
			
			
		}
		// this would refer to a past memory, which I probably can't access here.
		public void Open(string s) {
			selected.canvas.enabled = true;
			selected.canvasGroup.interactable = true;
			selected.canvasGroup.blocksRaycasts = true;
			selected.canvasGroup.alpha = 1f;
		}


		//public void Split(string p){ pseudo DText as a parallel thread
		//public void Combine(string p){ name of DText as a parallel
		
		
	}
	
}