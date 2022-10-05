using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System.Collections;
using System.Collections.Generic;


namespace Utility.GUI
{
	
	// interface?
	public interface IGenericList {
		// the cursor, the pointer position
		int GetSelected(); // based on selected game object. 
		string GetSelectedString(int i); // based on selecting a string, assuming hashed list
		
		// string operations, seen in list
		void Append(string content);
		void Insert(int position, string content);
		void Add(string content); // uses pointer (inserts at get selected, or appends)
		
		void Delete(int position);
		void Delete(string content);
		bool SearchSelected(string s);
		void SearchReset();
		bool SetSelected(int position);
		
		bool IsRunning();
		bool GetAutoSort();
		bool GetAutoSelect();
		
		void SetAutoSort(bool b);
		void SetAutoSelect(bool b);
		
		// build the list, cleans the list and sets the elements. or rebuild it
		void Build(List<string> elements);
		void Rebuild();
		
		List<string> GetList();
		void SetList(List<string> elements); // basically build
		
		string GetMessage();
		void SetMessage(string s);
		
		void Recolor(Color c, int i);
		void Recolor(Color c, string s);
		
		// it's about coloring stuff I deleted or just decided to color
		void ClearMarkers();
		void SetMarker(Color c, string s);
		
		// the group change makes it slide to the cursor
		bool GetDirty();
		void SetDirty(bool b);
	}
	
	public class GenericListBuilder : MonoBehaviour, IGenericList
	{
		
		public class GenericListMarker {
			public Color c;
			public string s;
			
			public GenericListMarker(Color c1, string s1)
			{
				c = c1;
				s = s1;
			}
		}
	
		// if gameobject moves without updating the list, the builder won't function without rebuild.
		
		protected List<string> listed = new List<string>();
		protected List<GenericListMarker> marked = new List<GenericListMarker>();
		
		
		public bool GetDirty()
		{ return group.dirty;}
		public void SetDirty(bool b)
		{
			group.dirty = b;
		}
		public List<string> elements{
			get{return listed;}
			set{Build(value);}
		}
		
		public List<string> GetList() {
			
			return listed;
		}
		public void SetList(List<string> e) {
			Build(e);
		}
		
		
		public void ClearMarkers(){
			marked.Clear();
		}
		public void SetMarker(Color c, string s){
			marked.Add(new GenericListMarker(c, s));
		}
		
		
		public string message = "";
		public string GetMessage(){
			return message;
		}
		public void SetMessage(string s){
			message = s;
		}
		
		public Transform copied; // the blank
		
		protected GroupSelection group;

		public bool autoSelect = true;
		public bool GetAutoSelect(){
			return autoSelect;
		}
		public void SetAutoSelect(bool b){
			autoSelect =b;
		}
		public bool autoSort = true;
		public bool GetAutoSort(){
			return autoSort;
		}
		
		public void SetAutoSort(bool b){
			autoSort =b;
		}
		
		protected bool running = false;
		public bool IsRunning(){
			// means it's busy
			return running;
		}
		
		protected void OnEnable () {
			if (group == null) group = GetComponentInParent<GroupSelection>();
			if (group == null) Debug.Log("Looked for group, found nothing", gameObject);
		}
		
		
		
		public int GetSelected() {
			if (running) return -1;
			GameObject g = group.selected ;
			
			if (group.selected == null) return -1;
			
			return g.transform.GetSiblingIndex();
			
		}
		public string GetSelectedString(int i) {
			
			return listed[i];
			
		}
		
		public void Append(string content)
		{
			
			listed.Add(content);
			Fill(content);
			
			if (autoSelect) 
			{
				group.selected = transform.GetChild(transform.childCount - 1).gameObject;
				
				group.dirty = true;
			}			
						
			if (autoSort) AutoSort();
			
			
				
		}
		public void Insert(int position, string content)
		{
			if (autoSort) {Append(content); return;}
			
			if ( position < listed.Count)
			{
				listed.Insert(position, content);
				Fill(position, content);
				
				if (autoSelect) 
				{
					group.selected = transform.GetChild(position).gameObject;
				
					group.dirty = true;
				}			
			
			
			}
			else
			{
				Append(content);
			}
		}
		
		
		public void Add(string content)
		{
			if (group != null) AddWithPointer(content);
			else
				Append(content);
				

			
		}
		protected void AddWithPointer(string content)
		{
			if ( autoSort || group.selected == null) 
			{
				Append(content);
				
			}
			else
			{
				int i = GetSelected();
				if (i == 0) i ++;
				Insert(i, content);
				
			}
		}
		
		public void Delete(int position)
		{
			if (  position < listed.Count) 
			{
				listed.RemoveAt(position);
				GameObject.Destroy(transform.GetChild(position).gameObject);
				
			}
		}
		public void Delete(string content)
		{
			int i = listed.IndexOf(content);
			Delete(i);
			
		}
		
		public void Recolor(Color c, int position)
		{
			if (  position < listed.Count) 
			{
				Transform t = transform.GetChild(position).Find("Background");
				
				if (t == null) 
				{
					Debug.Log("no background to recolor", gameObject); 
					
				}
				else
				{
					Button b = t.GetComponent<Button>();
					
					ColorBlock cb = b.colors;
					
					cb.normalColor = cb.highlightedColor = c;
					
					b.colors = cb;
				}
				
			}
			
		}
		public void Recolor(Color c, string content)
		{
			int i = listed.IndexOf(content);
			Recolor(c, i);
			
		}
		
		
		protected void Fill(int position, string content){
			
			
			Transform c = Fill(content);
			
			c.SetSiblingIndex(position);
		} 
		
		public bool SearchSelected(string s)
		{
			
			bool foundOne = false;
			bool casewise = false;
			GameObject final = null;
			GameObject g;
			for (int i = 0; i < listed.Count ; i++)
			{
				g = transform.GetChild(i).gameObject;
				
				if (listed[i].Contains(s, true))
				{
					if (!casewise) 
					{
						// check if the case is perfect
						if ( listed[i].Contains(s))
						
						{
							// case perfect success
							final = g;
							casewise = true;
						}
						else if (!foundOne)
						{
							// default success
							final = g;
							foundOne = true;
						}
					}
					
					g.SetActive(true);
						
				}
				else
					g.SetActive(false);
			}
			
			if (foundOne || casewise)
			{
				group.selected = final;
				group.dirty = true;
			}
							
			return foundOne || casewise;
			
		}
		
		public void SearchReset() {
			
			foreach (Transform t in transform)
			{
				t.gameObject.SetActive(true); // if they are supposed to be active?
			}
			group.dirty = true;
		}
		public bool SetSelected(int position)
		{
			if (position >= listed.Count) return false;
			if (group == null) return false;
			group.selected = transform.GetChild(position).gameObject;
			
			group.dirty = true;
			
			return true;
		}
		
		
		public void Remark() {
			running = true;
			
			StartCoroutine("_Remark");
		}
		
		protected IEnumerator _Remark() {
			foreach (GenericListMarker m in marked)
			{
				Recolor(m.c, m.s);
				yield return null;
			}
			running = false;
		}
		
		public void Sort() {
			if (transform.childCount < 1) return;
			if (running) return;
			
			running = true;
			StartCoroutine("_Sort");
		}
		
		
		protected void AutoSort()
		{
			// already added item to list and filled game list
			int sortedItem = listed.Count -1;
			if (sortedItem <= 0) return;
			
			int sortPos = sortedItem;
			
			int decr = sortedItem - 1;
			while ( (decr >= 0 ))
			{
				if (string.CompareOrdinal(listed[sortedItem], listed[decr]) < 0)
					sortPos = decr;
				decr --;
			}
			
			if (sortedItem != sortPos)
			{
				transform.GetChild(sortedItem).SetSiblingIndex(sortPos);
				
				string copy = listed[sortedItem];
				listed.RemoveAt(sortedItem);
				listed.Insert(sortPos, copy);
				
				// the item goes as far back as it can, and done
			}
			// else the item's at the end of the list
				
		}
		protected IEnumerator _Sort() {
			
			
			
			int sortedItem = 0; // the item I'm comparing backwards
			int sortPos = 0; // the position the item will end up in
			
			int decr; // the index partner with i, will reach - 1 repeatedly, in order to get the sorted item as low as it can go
			
			for (int i = 1; i < listed.Count ; i++)
			{
				
				sortedItem = i;
				sortPos = i;
				decr = i - 1;
				while ( (decr >= 0 ))
				{
					if (string.CompareOrdinal(listed[i], listed[decr]) < 0)
						sortPos = decr;
					decr --;
				}
				
				if (sortedItem != sortPos)
				{
					transform.GetChild(sortedItem).SetSiblingIndex(sortPos);
					
					string copy = listed[sortedItem];
					listed.RemoveAt(sortedItem);
					listed.Insert(sortPos, copy);
					
					// an item moves, so I delay a frame
					yield return null;
				}
				
			}
			
			group.dirty = true;
			running = false;
		}
		protected virtual Transform Fill(string content){
			
			
			Transform c = Object.Instantiate(copied); 
			
			c.SetParent( transform ); // and the autosort should cover the position
			
			c.gameObject.SetActive(true);
			
			
			Text t = c.Find("Text").GetComponent<Text>() ;
			
			t.text = content;
			
			return c;
		} 
		
		
		protected IEnumerator _Build() {
			
			yield return null;
			
			
			// first destroy the current list
			List<Transform> tList = new List<Transform>();
			foreach (Transform child in transform)
			{
				tList.Add(child);
			}
			
			foreach (Transform child in tList)
			{
				GameObject.Destroy(child.gameObject);
				yield return null;
			}
			
			
			// Populate
			
			foreach (string s in listed)
			
			{
				Fill(s);
				yield return null;
			}
			
			
			
			if (autoSort) yield return StartCoroutine("_Sort");
			
			running = true;
			yield return StartCoroutine("_Remark");
			
		}
		
		
		public void Build(List<string> used){
			
			// an exceptionally long list would need to be handled by coroutine
			if (running) StopAllCoroutines();
			
			listed = used; // no llonger requires a dummy at 0
			
			running = true;
			
			StartCoroutine("_Build");
		}
		
		public void Rebuild() {
			
			if (running) StopAllCoroutines();
			running = true;
			
			StartCoroutine("_Build");
			
		}
	}
		
}