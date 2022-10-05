
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;



namespace DialogueSystem
{
	public partial class MemoryBox {
		
		public DLocalAction localAction;
		
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
			NewLine();
			if (s != "") NewHeader(s);
		} 
		
		
		// Add Title to text. Defaults to text size.
		public void RemoveHeader (string s){ RemoveTitle();}
		public void NewHeader (string title){
			UseTitle(title);
		} 
		
		
		// add prefab 's' of unknown size where an icon would be
		public RectTransform widgets;
		public void InsertIcon (string s ){ 
			UseIcon(s);
			
		
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
			Destroy(prevPop);
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
			
			DeactivateAll();
			
		} 
		
		// erase the content of box, for effect
		public void Clear (string s){ 
			ResetBox();
		}
		// this probably means the current memory/dbox not the canvas. think about it.
		public void Close(string s) {
			canvas.enabled = false;
			canvasGroup.interactable = false;
			canvasGroup.blocksRaycasts = false;
			canvasGroup.alpha = 0f;
			
			
		}
		// this would refer to a past memory, which I probably can't access here.
		public void Open(string s) {
			canvas.enabled = true;
			canvasGroup.interactable = true;
			canvasGroup.blocksRaycasts = true;
			canvasGroup.alpha = 1f;
		}

// dtext and code stuff

		//public void Split(string p){ pseudo DText as a parallel thread
		//public void Combine(string p){ name of DText as a parallel
		
	}
}