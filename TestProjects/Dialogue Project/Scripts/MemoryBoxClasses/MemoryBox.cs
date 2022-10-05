
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;



namespace DialogueSystem
{
	public partial class MemoryBox : DisplayNodeParent {
		// This is a widget. An important one, but a widget no less.
		/*
		WHY THIS IS HERE
			This is used because information is received.
			
			converse: information sent too
			capture: this is the main function

		DISPLAY STUFF HAPPENING
			the boxes aren't thread safe
			the icon and title isn't parsed
			
			DIALOGUES
			//////
			memory node

			1 start blank, scroll at deliberate speed
			2 new card

			
			
		OPTIONS / WIDGETS
			the widgets can be in any position.
	
		*/
		
		public MemoryBoxNode newMemory; // instance
		public static MemoryBox memory; // this
		public RectTransform rectTransform;
		
		
		public Transform offGroup;
		// each "Memory" of all spawned memories
		protected List<MemoryBoxNode> boxList = new List<MemoryBoxNode>(); 
		// each docked memory I need to manage over time
		protected List<MemoryBoxNode> anchoredList = new List<MemoryBoxNode>(); 
		
		// each memory in scene, that could be dragged
		
		protected List<MemoryBoxNode> detachedList = new List<MemoryBoxNode>(); 
		
		
		
		// active/ inactive?
		
		
		public MemoryBoxNode _selected;
		public MemoryBoxNode selected
		{
			get{return _selected;}
			set{node = _selected = value;}
		}
		
		
		public int boxPos = 0;
		protected int maxBoxes = 25;
		public int boxesActive = 0;
		
		protected bool safeThread = true;
		protected Canvas canvas ;
		protected CanvasGroup canvasGroup ;
		
		protected override void OnDisable()
		{
			base.OnDisable();
		}
		
		
		protected override void OnEnable()
		{
			//initialize
			base.OnEnable();
			
			if (canvas == null) canvas = GetComponentInParent<Canvas>();
			if (canvasGroup == null) canvasGroup = GetComponentInParent<CanvasGroup>();
			if (memory == null) memory = this;
			else return;
			
			if (rectTransform == null) rectTransform= GetComponent<RectTransform>();
			boxList.Clear();
			
			AddAllActions();
			
			foreach (Transform t in rectTransform)
			{ // better yet, copy it n times as it gets stronger
				
				MemoryBoxNode m = t.gameObject.GetComponent<MemoryBoxNode>();
				
				m.localAction = localAction;
				m.Load();
				boxList.Add(m);
				
			}
			
			newMemory.canvas = canvas;
			newMemory.canvasGroup = canvasGroup;
			newMemory.name = "Memory";
			newMemory.localAction = localAction;
			newMemory.gameObject.SetActive(true);
			newSize = newMemory.rectTransform.sizeDelta;
			if( boxList.Count < maxBoxes)
			while ( boxList.Count < maxBoxes)
			{
				
				selected = NewMemory();
				
				// these two don't get set properly
				
				selected.Load();
				
				boxList.Add(selected);
			}
			
			newMemory.gameObject.SetActive(false);
			DeactivateAll();
			
			
			
		}

//### THIS SETS THE VISIBLE MEMORIES		
		
		public void RemoveWidget() {
			if (selected.widget != null) selected.DestroyWidgets();
			
		}
		
		public RectTransform FindWidget( string s) {
			
			return widgets.Find(s) as RectTransform;
		}
		public RectTransform CloneWidget(string s) {
			if (s == "") return null;
			
			RectTransform rt = FindWidget(s);
			if (rt == null) return null;
			
			return Instantiate(rt) as RectTransform;
		}
		public MemoryBoxNode NewMemory() {
			
			MemoryBoxNode mb = Instantiate(newMemory) as MemoryBoxNode;
			
			return mb;
		}
		public RectTransform CloneWidget(RectTransform rt ) {
			return Instantiate(rt) as RectTransform;
		}
		
		public RectTransform prevWidget;
		public void UseWidget( string s) {
			RectTransform rt = prevWidget = CloneWidget(s);
			if (rt == null) 
			{
				Debug.Log("couldn't find " + s, gameObject);
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
		public RectTransform icons;
		public RectTransform defaultIconWidget;
		public RectTransform prevIcon;
		public RectTransform FindIcon( string s) {
			
			return icons.Find(s) as RectTransform;
		}
		public RectTransform CloneIcon(string s) {
			if (s == "") return null;
			
			RectTransform rt = FindIcon(s);
			if (rt == null) return null;
			
			return Instantiate(rt) as RectTransform;
		}
		public void UseIcon( string s) {
			
			RectTransform rt = prevIcon = CloneIcon(s);
			
			if (rt == null) 
			{
				Debug.Log("missing icon " + s , gameObject);
				return;
			}
			
			
			_UseIcon(rt);
		}
		
		public void UseIcon( RectTransform t) {
			// this must be some complicated thing that already has an image.
			t = prevIcon = CloneWidget(t);
			_UseIcon(t);
		}
		
		public void _UseIcon( RectTransform t) {
			// I'm placing an icon in the selected box. set boxPos first.
			
			
			selected.UseIcon(t);
			
			
			if (!t.gameObject.activeSelf) t.gameObject.SetActive(true);
			
		}
		
		public void RemoveTitle() {
			
			
			selected.DestroyTitle();
		}
		public void UseTitle(string s) {
			
			selected.SetTitle(s);
		}
		public void RemoveText() {
			
			Text text = selected.bodytext ;
			if (text == null) return;
			text.text = "";
		}
		public void UseText( string s) {
			
			Text text = selected.bodytext ;
			if (text == null) return;
			
			text.text = s;
		}
		
		// something was entered into the list.
		protected void Anchor() {
			selected.anchored = true;
			anchoredList.Add(selected);
			detachedList.Remove(selected);
			
		}
		protected void Detach() {
			selected.anchored = false;
			anchoredList.Remove(selected);
			detachedList.Add(selected);
		}
		protected void SetVisible(bool b = true) {
			
			selected.visible = selected.anchored = b;
			if (b)
			{
				anchoredList.Add(selected);
			}
			else
			{
				anchoredList.Remove(selected);
				detachedList.Remove(selected);
			}
			
		}
		protected void PlayBox() {
			
			if (memory.safeThread)
			{
				
				selected.Play();
				
			}
			else
				selected.PlayUnsafe(); // there is a merge with a random line break, and the counter is reset
		}
		protected void ShowBox() {
			// try to use a DStream, new listed object
			
			// 1
			
			// find an available box. or make one.
			
			
			if ( FindFirstAvail())
			{
				ResetBox();
				boxesActive ++;
			}
			else
			{
				// use the first box because there are none?
				boxPos = 0;
				Select();
				ResetBox();
				
			}
			
			// 2 get the parser running
			selected.localAction = localAction;
			selected.widgets = widgets;
			selected.icons = icons;
			selected.Load();
			
			// 3
			// it looped, or it didn't.
			if (!selected.gameObject.activeSelf) selected.gameObject.SetActive(true);
			
			if (!selected.visible)
			{
				selected.rectTransform.SetParent(rectTransform,false); // in front
				selected.visible = true;
			}
			Anchor();
			
			
			
			selected.rectTransform.SetAsLastSibling(); // in front
			selected.rectTransform.anchoredPosition = newPosition;
			selected.rectTransform.sizeDelta = newSize;
			
		}
		
		public void SelectLastAnchored() {
			selected = anchoredList[anchoredList.Count - 1];
		}
		public void Select() {
			selected = boxList[boxPos];
		}
		public void Select(int i) {
			selected = boxList[i];
		}
		public bool FindFirstAvail() {
			if (selected == null)
				Select();
			
			if (!selected.visible)
				return true;
			
			int count = boxList.Count;
			for (int i = (int)Mathf.Repeat(boxPos + 1, boxList.Count) ; 
			i != boxPos ; 
			i = (int)Mathf.Repeat(i + 1, boxList.Count))
			{
				boxPos = i;
				Select();
				if( !selected.visible)
				{
					//boxPos = oldpos; // could move it. idk
					return true;
				}
				count--;
				if (count < 0) break;
			}
			
			return false;
		}
		public void SwapFirstBox() {
			
			boxPos = 0;
			Select();
			ResetBox();
		}
		public void ReuseCurrentBox() {
			
			ResetBox();
			boxPos = boxList.IndexOf(selected);
			
		}
		public void ActivateCurrentBox() {
			
			selected.rectTransform.SetParent( rectTransform, false);
		}
		public void DeactivateCurrentBox() {
			
			selected.rectTransform.SetParent( offGroup, false);
			selected.visible = false;
			selected.Break();
		}
		
		public void DeactivateAll() {
			boxesActive = 0;
			int oldpos = boxPos;
			for (int i = 0; i < boxList.Count; i++)
			{
				boxPos = i;
				Select();
				DeactivateCurrentBox();
			}
			boxPos = oldpos;
			Select();
			
		}
		public void ResetBox() {
			// get the first inactive box
			//...
			
			
			RemoveWidget();
			RemoveIcon();
			RemoveText();
			RemoveTitle();

		}
		
//### THIS .. COULD BE FOR RECALLING OLDER MEMORIES	
		
		public void Remember(string s) {
			// or dtext?
		}
		
		
		public static void Release(){
			
			//DText p = memory.hub.streamedText;
			//foreach (MemoryBoxNode box in memory.boxList)
			//if ( box.streamedText == p)
			//{
			//	box.streamedText = null;
			//	box.goalText = null;
			//}
		}
		
		public void NewLine(){
			// really I might need to identify the text better
			ManageElements();
			//float charPos = selected.charPos;
			DisplayParser g = selected.parser;
			
			int cpos = selected.charPos;
			selected.Break();
			
			ShowBox();
			selected.parser = g;
			selected.charPos = cpos;
			
			selected.Continue();
			
		}
		
		
		protected float heightDelta;
		protected float selectedHeight;
		protected float selectedWidth;
		protected float listMoveDelta;
		protected float bottom;
		protected Vector3 newPosition = new Vector3();
		protected Vector2 newSize; //xx
		protected Vector2 selectedSize;
		protected RectTransform nextT;
		protected RectTransform prevT;
		protected float listSpeed = 150f;
		
		
		// what this needs to do list rects.
		// this doesn't have to be part of memory box, it's a physical list emulation -that only deals with the most recent element (linear and cheap)
	
		public bool dragging = false;
		public MemoryBoxNode draggedNode;
		protected void WhileDragging(){
			if (!dragging) return;
			
			// it's on release actually
			MemoryBoxNode mt = draggedNode.touchedNode; // eg prevnode
			if (mt && mt.anchored )
			{
				// the current list, only one thing can touch another in order. But there's ultimately one list unless...
				selected = draggedNode;
				Anchor();
			}
		}
		protected void ManageElements(){
			if (anchoredList.Count < 1)
			{
				// anchored position
				newPosition.Set(0,0,0);
				
				return;
			}
			
			// get last anchored node
			// SelectLastAnchored()
			// if selected.done return;
			selectedWidth = selected.nodeSize.x ; // or newsize...
			selectedWidth = (selectedWidth > newSize.x) ? selectedWidth : newSize.x;
			
			// maybe a title frame isn't necessary, I just have to change the text height?
			// "body" it can resize vertically
			selectedHeight = selected.nodeSize.y ; // 
			
			
			
			selectedHeight = (selectedHeight > newSize.y) ? selectedHeight : newSize.y;
			
			selectedSize.y = selectedHeight ;
			selectedSize.x = selectedWidth ;
			
			// I'm controlling the rectTransform while it's anchored
			selected.rectTransform.sizeDelta = selectedSize;
				
			// this to determine where a new anchored box appears
			newPosition = selected.rectTransform.anchoredPosition;
			newPosition.y -= selectedHeight;
			
			// this is where the bottom of selected box should be
			heightDelta = rectTransform.sizeDelta.y;
			
			// this is where the bottom of selected box is
			bottom = newPosition.y + heightDelta;
			
			
			if (bottom < -0.01f)
			{
				
				if (bottom < Time.deltaTime * listSpeed)
					listMoveDelta= Time.deltaTime * listSpeed;
				else
					listMoveDelta = bottom;
				
				heightDelta += listMoveDelta;
				
				
				rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, heightDelta);
				
				
			}
			else if (bottom> 0.01f)
			{
				
				
				if (bottom > Time.deltaTime * listSpeed)
					listMoveDelta= Time.deltaTime * listSpeed;
				else
					listMoveDelta = bottom;
				
				heightDelta -= listMoveDelta;
				
				
				
				rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, heightDelta);
				
			}
			// after a while the memories can vanish.
		}
		protected void CloseOld(){
			// timed
			FindFirstAvail();
			DeactivateCurrentBox();
		}
		protected override void OnUpdate(){
			base.OnUpdate();
			if (selected != null) selected.Step();
			
			WhileDragging();
			ManageElements();
			
		}
		
		
		
		
		
	}
}