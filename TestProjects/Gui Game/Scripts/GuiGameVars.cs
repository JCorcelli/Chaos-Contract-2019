
using UnityEngine;

using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using static GuiGame.GuiGameVars;

namespace GuiGame
{

	public delegate void GuiDictUpdate(GuiDict updateDict);
	public class GuiDict{ 
		public static GuiDictUpdate onNullify;
		public GuiDictUpdate onUpdate;
		
		public GuiDict(){
			
			onNullify += OnNullify;
		}
		
		public GuiDict(GuiDict dic):this()
		{
			dict = new List<GuiDict>(dic.dict);
		}
		public GuiDict(bool t):this()
		{
			Instance();
		}
		public List<GuiDict> dict = null;
		
		
		public string _key = null;
		
		public string key{
			get => _key;
			set {
				
				if (value == null){
					if (_key == null) return;
					else{
						_key = null;
						keyCount--;
					}	
				}	
				else{
					
					_key = value;
					keyCount++;
				}
				
				queue_update = true; 
			}
		}
		
		public bool queue_update = false;
		
		public void OnUpdate(GuiDict u = null){
			SendUpdate(u);
		}
		
		// passes a dict to subscribers, or this
		public void SendUpdate(GuiDict u = null){
			if (onUpdate == null) return;
			if (u == null) onUpdate(this);
			else onUpdate(u);
			
			
		}
		
		public GuiDict parent = null;
		
		// 0 not circular
		// 1 circular
		// 2 depth limit reached, a parent dictionary was circular
		// aka hub
		public int IsCircular(int maxDepth = DEFAULTDEPTH) {
			
			int count = 0;
			GuiDict g = this;
			
			while (g.parent != null
			&& g.parent != this )
			{
				
				g = g.parent;
				count++;
				if (count > maxDepth) // this could be a false negative
					return 2;
			}
			
			if (g.parent == this) 
				return 1;
			else
				return 0;
				
			
			
			
		}
		
		public GuiDict Root {
			get {
				GuiDict g = this;
				while (g.parent != null
				&& g.parent.parent.key != GAME 
				&& g.parent != this ) 
					g = g.parent;
				return g;
			}
			
		}
		public GuiDict FindHub(string name)
		{
			GuiDict g = this;
			GuiDict hub = null;
			GuiDict vars = null;
			while (hub == null
			&& g.parent != null
			&& g.parent != this
			&& g.key != name) 
			{
				g = g.parent;
				vars = g.Find(VARIABLE_OBJECT);
				if (vars != null)
					hub = vars.Find(name);
				
				
			}
			return hub;
		}
		
		
		public bool isVar => GetIsVar();
		
		public bool GetIsVar() => FindRootContainer().key == VARIABLE_OBJECT  ;
		
		
		public GuiDict FindRootContainer(string[] container = null)
		{
			if (container == null) container = ALLGUICONST;
			GuiDict g = this;
			while (g.parent != null
			&& g.parent != this 
			&& !container.Contains(g.key)) 
				g = g.parent;
			return g;
		}
			
		public GuiDict FindRoot(string name)
		{
			GuiDict g = this;
			while (g.parent != null
			&& g.parent != this 
			&& g.key != name) 
				g = g.parent;
			return g;
		}
			
		
		
		public static int keyCount = 0;
		public static int logs = 0;
		
		
		// simple version
		public bool IsExternal(GuiDict dict) {
			return (this.dict == null || dict.parent != this);
		}
		
		// 0 child of this
		// 1 child of other
		// 2 orphan
		// 3 orphan without children
		// it may have subscribers, and setting something's parent to this will be invisible
		public int IsAdopted(GuiDict child) {
			
			if (child.parent == this) return 0;
			if (child.parent != null) return 1;
			else if (child.Count > 0) return 2;
			else return 3;
		}
		public void Instance(){
			if (dict == null) dict = new List<GuiDict>();
		}
		
		public void Increment(BText bt){
			// alter BText
			
			// A less obvious but faster approach, 
			//string k = bt.ToString();
			//string result = Increment(k);
			
			bt.Append(Increment(bt.ToString()));
			
		}
		
		public string Increment(string key, int trail = 0){
			// return new string indexer
			if (key == null || dict == null) return "" ; 
			pathText.Clear();
			int indexer = 0;
			for (int i = 0 ; i < dict.Count ; i++)
			{
				if (dict[i].key == key) indexer++;
			}
			if (indexer < 1) return "";
			
			string stringindex = indexer +"";
			pathText.Append(stringindex);
			
			trail -= stringindex.Length;
			
			if (trail >0 ) 
				pathText.Insert(0,"0",trail);
			
			pathText.Insert(0,".");
			
			
			return pathText.ToString();
			
		}
		
		public void Include(string key){
			if (key == null) return ; // variable, no dictionary
			Instance();
			
			
			GuiDict g = new GuiDict(){ key = key, parent = this};
			Add(g);
			
		}
				
		public void Instance(string key){
			if (key == null) return; // variable, no dictionary
			
			
			Instance();
			
			if (GetFromKey(key) == null) {
				GuiDict g = new GuiDict(){ key = key, parent = this};
				Add(g);
				
				//Debug.Log("New Key: " + key);
			}
		}
		
		public void Add(string key, GuiDict newDict){
			// null key isn't acceptable
			
		
			Add(key);
			
			this[key].Add(newDict); 
			
		}
		
		// this merely updates
		public void Connect(GuiDict newDict){
			newDict.onUpdate += OnUpdate;
		}
		// this sets the DNA path
		public void SetParent(GuiDict newParent){
			newParent.Add(this);
			this.parent = newParent;
		}
		// this is more like "adopt"
		public void Add(GuiDict newDict){
			if (newDict == null) return;
			
			Instance();
			
			
			if (newDict.key == null){
				newDict.key = "*";
			}
			// best guess, if it was removed, and reinserted it should balance out
			// if it was created...
			
			if (dict == null) 
				dict = new List<GuiDict>(){newDict};
			else if (!dict.Contains(newDict))
				dict.Add(newDict);
			
			newDict.onUpdate += OnUpdate;
			
		}
		public void Include(GuiDict newDict){
			if (newDict == null) return;
			
			
			if (newDict.key == null){
				newDict.key = "*";
			}
			// best guess, if it was removed, and reinserted it should balance out
			// if it was created...
			
			if (dict == null) dict = new List<GuiDict>();
			
			dict.Add(newDict);
			
			
			newDict.onUpdate += OnUpdate;
		}
		public void Add(string key){
			if (key == null) return; // variable, no dictionary
			
			Instance(key);
			
				
			
		}
		
		public void Include(string key, string g){
			GuiDict k = FindLast(key);
			if (k == null){
				Include(key);
				k = this[key];
			}
			//Debug.Log("Key:" +key+"... New Value: " + g);
			if (g == null) return;
			
			k.Include(g);
			
			
		}
		public void Add(string key, string g){
			GuiDict k = GetFromKey(key);
			if (k == null){
				k = new GuiDict(){key = key, parent = this};
				
				Add(k);
			}
			//Debug.Log("Key:" +key+"... New Value: " + g);
			if (g == null) return;
			
			k.Add(g);
			
			
		}
		public bool Contains(string key)
		{
			if (dict == null) return false;
			for (int i = 0 ; i < dict.Count ; i++)
			{
				if ( dict[i].key == key) return true;
			}
			return false;
			
		}
		
		public GuiDict FindLast(string key){
			if (dict == null) return null;
			
			for (int i = dict.Count-1 ; i >= 0 ; i--)
			{
				if ( dict[i].key == key) return dict[i];
			}
			
			return null;
		}
		
		public int GetIndexFromDict(GuiDict d)
		{
			if (dict == null) return -1;
			
			for (int i = 0 ; i < dict.Count ; i++)
			{
				if ( dict[i] == d) return i;
			}
			
			return -1;
		}
		public int GetIndexFromKey(string key, int startIndex = 0)
		{
			if (dict == null) return -1;
			for (int i = startIndex ; i < dict.Count ; i++)
			{
				if ( dict[i].key == key) return i;
			}
			return -1;
			
		}
		
		public GuiDict GetFromKey(string key)
		{
			
			if (dict == null) return null;
			for (int i = 0 ; i < dict.Count ; i++)
			{
				if ( dict[i].key == key) return dict[i];
			}
			return null;
			
		}
		
		// change first found value in place. To add to front use Add / Remove.
		public bool Replace(string key, GuiDict value)
		{
		
			if (value == null) {
				return Remove(key);
			}
			int i = GetIndexFromKey(key);
			if (i < 0) 
				return false;
			else{
				if (value.key == null) value.key = key;
				dict[i] = value; 
			}
			return true;
				
		}
		public static BText pathText = new BText();
		public BText GetPath(int maxDepth = DEFAULTDEPTH){
			pathText.Clear();
			pathText.Append( key);
			
			
			if (parent == null) return pathText;
			GuiDict g = this.parent;
			
			int count = 0;
			
			int end = maxDepth + 1;
			while (g != null && g != this && count < end && g.key != GAME){
				count++;
				pathText.Insert( 0, "/");
				pathText.Insert( 0, g.key);
				g = g.parent;
			}
			
			//CODE_CIRCULAR_REF = "<--/";
			if (count > maxDepth) 
			{
				pathText.Insert( 0, "/");
				pathText.Insert( 0, "-->");
				pathText.Insert( 0, maxDepth + "");
				pathText.Insert( 0, ":");
				pathText.Insert( 0, CODE_DEPTH);
				
			}
			else if (g == this) 
			{
				pathText.Insert( 0, "/");
				pathText.Insert( 0, CODE_CIRCULAR_REF);
				pathText.Insert( 0, "<--");
				
			}
			return pathText;
		}
		
		
		public BText GetComponentPath(){
			pathText.Clear();
			pathText.Append( key);
			pathText.Append( "/");
			if (parent == null) return pathText;
			GuiDict m = this.parent;
			GuiDict m1 = m;
			GuiDict m2 = m;
			
			if (m != null) m1 = m.parent;
			if (m1 != null) m2 = m1.parent;
			while (m != null){
				if ((m2 != null && (m2.key == COMPONENT_OBJECT || m2.key == WINDOW_ASCII )) || (m1 != null && (m1.key == WINDOW_ASCII )) || m.key == WINDOW_ASCII)
				{
					pathText.Insert( 0, "/");
					pathText.Insert( 0, m.key);
					//pathText.Insert("/", 0);
					//pathText.Insert(m2.key, 0);
				}
				m = m1;
				if (m1 != null) m1 = m2;
				if (m2 != null) m2 = m2.parent;
				
			}
			
			return pathText;
		}
		// re
		protected void DestroyAll(string key, int recursive = 0){
			GuiDict c;
			
			
			for (int i = 0 ; i < dict.Count ; i++)
			{
				c = dict[i];
				if (c.key == key && c.parent == this) {
					if (recursive > 0 || recursive < 0) {
						recursive--;
						c._DestroyAll(recursive);
						recursive++;
					}
					c.parent = null;
					
					RemoveAt(i--);
				}
			}
			
			
		}
		public void Nullify(string key, int recursive = 0){
			GuiDict c ;
			for (int i = 0 ; i < dict.Count ; i++)
			{
				c = dict[i];
				if ( c.key == key && c.parent == this) {


					if (recursive > 0 || recursive < 0) {
						recursive--;
						c._NullifyAll(key, recursive);
						recursive++;
					}
					break;
				}	
				
			}
			
		}
		
		protected void _NullifyAll(string key, int recursive = 0){
			GuiDict c;
			
			
			for (int i = 0 ; i < dict.Count ; i++)
			{
				c = dict[i];
				if (c.parent == this) {
					if (recursive > 0 || recursive < 0) {
						recursive--;
						c._NullifyAll(key, recursive);
						recursive++;
					}
					
					Nullify();
				}
			}
			
		}
		protected void _DestroyAll(int recursive = 0){
			GuiDict c;
			
			
			for (int i = 0 ; i < dict.Count ; i++)
			{
				c = dict[i];
				if (c.parent == this) {
					if (recursive > 0 || recursive < 0) {
						recursive--;
						c._DestroyAll(recursive);
						recursive++;
					}
					c.parent = null;
					
				}
			}
			
			Clear();
		}
		
		
		
		// remove all reference to "key"
		public void Forget(GuiDict g){
			if (dict == null) return;
			if (dict.Contains(g)) 
				dict.Remove(g);
			
			// maybe it's connected
			onUpdate -= g.OnUpdate;
			
			if (parent == g) 
			{
				parent = null;
				g.onUpdate -= OnUpdate;
			}
		}
		public void OnNullify(GuiDict g){
			
			Forget(g);
			
		}
		
		
		public void Join(string jointhis)
		{
			GuiDict d = FindAll(jointhis, true);
			for (int i = 0 ; i < d.Count ; i++)
			{
				d[i].Cut();
			}
		}
				
		public void Cut(){
			if (parent != null)
				parent.Forget(this);
				
			if (dict != null)
			if (parent != null)
			foreach (GuiDict g in dict)
			{
				g.SetParent(parent);
				g.Forget(this);
			}
			else
			foreach (GuiDict g in dict)
				g.Forget(this);
			
			
			parent = null;
			
			key = null;
			onNullify -= OnNullify;
		}
		public void NullifyQuick(){
			if (parent != null)
				parent.Forget(this);
			
			if (dict != null)
			foreach (GuiDict g in dict)
			{
				g.Forget(this);
			}
			parent = null;
			key = null;
			
			onNullify -= OnNullify;
		}
		public void Nullify(){
			onNullify(this);
			key = null;
			
			onNullify -= OnNullify;
			
		}
		
		public void Destroy(GuiDict child, int recursive = 0){
			if (dict == null) return;
			
			if (child.parent == this)
			{	
				if (recursive > 0 || recursive < 0) {
					recursive--;
					child._DestroyAll(recursive);
					recursive++;
				}
				child.parent = null;
				
				dict.Remove(child);
			}
			
		}
		
		public void Destroy(string key, int recursive = 0){
			GuiDict c ;
			for (int i = 0 ; i < dict.Count ; i++)
			{
				c = dict[i];
				if ( c.key == key && c.parent == this) {


					
					RemoveAt(i);
					if (recursive > 0 || recursive < 0) {
						recursive--;
						c._DestroyAll(recursive);
						recursive++;
					}
					c.parent = null;
					break;
				}	
				
			}
			
		}
		
		
		protected static GuiDict all = null;
		public static int _output = 0;
		public int output {
			get => _output; 
			set => _output = value;
		}
		
		public bool success => output > 0;
		
		
		public string SliceKey(int start, int? direction)
		{
			BText b = new BText();
			
			int len = key.Length;
			
			if (direction == null)
			{
				if (start >= 0)
				for (int i = start ; i < len ; i++)
					b.Append(key[i]);
				else
				for (int i = start ; i >= -len ; i--)
					b.Append(key[(int)Mathf.Repeat(i,len)]);
				return b.ToString();
			}
			
			
			
			start = (int)Mathf.Repeat(start,Count);
			if (direction - start >=0 )
			for (int i = start ; i < direction ; i++)
				b.Append(key[(int)Mathf.Repeat(i,len)]);
			
			else
			for (int i = start-1 ; i >= direction ; i--)
				b.Append(key[(int)Mathf.Repeat(i,len)]);
			
			return b.ToString();
		}
		public GuiDict SliceList(int start, int? direction)
		{
			
			
			
			GuiDict b = new GuiDict(){key = KEY_ALL};
			
			if (direction == null)
			{
				if (start >= 0)
				for (int i = start ; i < Count ; i++)
					b.Include(this[i]);
				else
				for (int i = start ; i >= -Count ; i--)
					b.Include(this[(int)Mathf.Repeat(i,Count)]);
				return b;
			}
			
			start = (int)Mathf.Repeat(start,Count);
			if (direction - start >=0 )
			for (int i = start ; i < direction ; i++)
				b.Include(this[(int)Mathf.Repeat(i,Count)]);
			else
			for (int i = start-1 ; i >= direction ; i--)
				b.Include(this[(int)Mathf.Repeat(i,Count)]);
			
			return b;
			
		}
		public GuiDict FindPartialMatch(string key, bool recursive = false){
			GuiDict all = new GuiDict(){key = KEY_ALL};
			GuiDict dnext = this;
			if (recursive)
				dnext = dnext.Hierarchy;
			
			for (int i = 0 ; i < dnext.Count ; i++)
			if (dnext[i].key.Contains( key) )
				all.Add(dnext[i]);
			
			if (all.Count > 0)
				return all;
				
			return null;
				
		}
		public GuiDict FindAll(string key, bool recursive = false){
			if (dict == null) return null;
			all = new GuiDict();
			all.key = KEY_ALL;
			
			_FindAll(key, recursive);
				
			GuiDict g = all;
			all = null;
			return g;
		}
		
		
		public GuiDict CloneDict(){
			GuiDict d = this;
			GuiDict g = new GuiDict();
			
			g.key = d.key;
			
			if (d.dict != null)
			for (int i = 0 ; i < d.dict.Count ; i++)
			
				g.Add(d.dict[i]);
			
			return g;
		}
		public static GuiDict CloneDict(GuiDict d){
			GuiDict g = new GuiDict();
			
			g.key = d.key;
			
			if (d.dict != null)
			for (int i = 0 ; i < d.dict.Count ; i++)
			
				g.Add(d.dict[i]);
			
			return g;
		}
		public void ReplaceInParent(GuiDict c){
			int pos = parent.GetIndexFromDict(this);
			parent[pos] = c;
			//c.SetParent(parent);
			c.parent = parent;
			c.onUpdate += parent.OnUpdate;
		}
		
		public void Merge(GuiDict c){
			if (c == null) return ;
			
			for (int i = 0 ; i < c.Count ; i++)
			
				Include(c[i]);
			
		}
		public void IncludeList(GuiDict other){
			for (int i = 0 ; i < other.Count ; i++)
				other[i].SetParent(this);
			
		}
		public void RemoveList(GuiDict other){
			for (int i = 0 ; Count > 0 && i < other.Count ; i++)
			{
				Remove(other[i].key);
			}
		}
		public GuiDict Clone(){
			if (dict == null) return CloneDict(this);
			
			GuiDict c = CloneDict(this);
			
			for (int i = 0 ; i < c.Count ; i++)
			{
				c[i] = c[i].Clone();
				c[i].SetParent(c);
				
			}
			
			return c;
		}
		
		protected void _FindAll(string key, bool recursive = false){
			if (dict == null) return;
			
			GuiDict c ;
			for (int i = 0 ; i < dict.Count ; i++)
			{
				c = dict[i];
				
				if ( c.key == key) {
					all.Add(c);
				}
				if (recursive && !this.IsExternal(c))
				c._FindAll(key, recursive);
				
				
			}
			
		}
		public string DebugHierarchy() {
			GuiDict hier = Hierarchy;
			if (hier== null) return "No Hierarchy";
			string s = "";
			s+= hier.dict[0].GetPath().ToString() ;
			
			
			for (int i = 1 ; i < hier.Count ; i++)
			{
				s+=  "\n" + hier.dict[i].GetPath().ToString();
			}
			
			return s;
		}
		public GuiDict Hierarchy {
			get {
				
			if (dict == null) return null;
			
			all = new GuiDict();
			all.Instance();
			all.key = KEY_ALL;
			
			_Hierarchy();
				
			GuiDict g = all;
			all = null;
			return g;
			}
		}
		protected void _Hierarchy(){
			
			if (dict == null) return;
			GuiDict c ;
			
			for (int i = 0 ; i < dict.Count ; i++)
			{
				c = dict[i];
				all.Include(c);
				
				c._Hierarchy();
				
			}
			
		}
		
		public GuiDict Find(string key, bool recursive = false){
			if (dict == null) return null;
			GuiDict c = null;
			
			
			for (int i = 0 ; i < dict.Count ; i++)
			{
				c = dict[i];
				
				if ( c.key == key) {
					
					return c;
				};
				
			}
			if (recursive)
			for (int i = 0 ; i < dict.Count ; i++)
			{
				c = dict[i];
				c = c.Find(key, recursive);
				if (c != null) return c;
			}
			return null;
		}
		public void Clear()
		{
			if (dict == null) return;
			
			dict.Clear();
			
		}
		public void RemoveAll()
		{
			if (dict == null) return;
			
			int count = Count;
			for (int i = 0 ; i < count ; i++)
				RemoveAt(0);
			
		}
		
		public int RemoveAll(string key)
		{
			int count = 0;
			if (dict == null) return -1;
			for (int i = 0 ; i < dict.Count ; i++)
			{
				if ( dict[i].key == key) {
					
					RemoveAt(i--);
					count++;
				}
			}
			
			return count;
		}
		public void RemoveAt(int index)
		{
			if (dict == null) return;
			GuiDict d = dict[index];
			d.onUpdate -= OnUpdate;
			if (IsAdopted(d) > 2) // it's gone
				keyCount--;
				
			
			dict.RemoveAt(index);
		}
		public bool Remove(string key)
		{
			
			if (dict == null) return false;
			for (int i = 0 ; i < dict.Count ; i++)
			{
				if ( dict[i].key == key) {
					
					RemoveAt(i);
					return true;
				}
			}
			
			return false;
		}
		public GuiDict this[string key]
		{
			get => GetFromKey(key);
			set {
				if (value == null) {
					Remove(key); 
					return;
				}
				int i = GetIndexFromKey(key);
				if (value.key == null) value.key = key;
				
				if (i < 0)
					Add(value);
				else
				{
					if (IsAdopted(dict[i]) > 2) // it's gone
						keyCount--;
					dict[i].onUpdate -= OnUpdate;
					dict[i] = value; 
				}
				return;
			}
		}			
		
		// inherit this class, and implement operators, lower
		
		
		
		public GuiDict ThisComponent {
			
			get
			{
				GuiDict g = this;
				while (g.parent?.parent != null && g.parent.parent.key != COMPONENT_OBJECT)
					g = g.parent;
				return g;
			}
		}
		
		// helper
		public GuiDict esc(){
			if (parent == null) return null;
			GuiDict g = this;
			
			if (g.parent.parent.key == COMPONENT_OBJECT)
				return g.parent.parent.parent;
			
			if (ALLGUICONST.Contains(g.parent.key))
				return g.parent.parent;
			
			
			return g.parent;
		}
		// this is supposed to logically search local, hub, and global
		public GuiDict GetLocalVars(bool create = true){
			GuiDict g = this;
			GuiDict d = g.Find(VARIABLE_OBJECT);
			
			if (create && d == null)
			{	
				g.Add(VARIABLE_OBJECT);
				return g[VARIABLE_OBJECT];
			}
			return d;
		}
		
		// list access is preferred
		public GuiDict Fetch(string findThis ){
			if (parent == null) return null;
			GuiDict g = this;
			GuiDict d ;
			
			while (g.parent != null)
			{
				d = g.FetchLocal(findThis);
				if (d != null) return d; 
				g = g.parent;
			}
			
			// game?
			return null;
		}
		public GuiDict FetchGlobal(string findThis ){
			if (parent == null) return null;
			GuiDict g ;
			
			//global/var
			g= Root.Find(VARIABLE_OBJECT);
			if (g != null) g= g.Find(findThis);
			
			return g;
			
		}
		public GuiDict FetchLocal(string findThis ){
			if (parent == null) return null;
			GuiDict g ;
			
			g = this.Find(findThis);
			if (g != null) return g; 
			
			g = GetLocalVars(false);
			// this/var
			if (g != null) g = g.Find(findThis);
			
			
			return g;
		}
		public GuiDict FetchParent(string findThis ){
			if (parent == null) return null;
			GuiDict g ;
			
			// this parent/parent var
			g = this.esc().Find(findThis);
			if (g != null) return g; 
			
			g = this.esc().GetLocalVars(false);
			if (g != null) g = g.Find(findThis);
			
			
			//static?/var
			return g;
		}
		public GuiDict FetchAll(string findThis = VARIABLE_OBJECT){
			if (parent == null || dict == null) return this;
			
			GuiDict g = new GuiDict(){key = KEY_ALL};
			GuiDict m;
			for (int i = 0 ; i < Count ; i++)
			{
				m = dict[i].FindAll(findThis);
				
				if (m != null)
				for (int ii = 0 ; ii < m.Count ; ii++)
				{
					g.Add(m[ii]);
				}
			}
			
			return g;
		}
		
		protected GuiDict __i(int back = -500)
		{
			GuiDict g = this;
			int i = 0;
			for (; i > back && g.parent != null && g.parent != this ; i--)
			{
				g = g.parent;
			}
			
			if (g.parent == this)
				output = -1;
			else if (i > back)
				output = 0;
			else
				output = 1;
			
			
			return g;
		}
		public GuiDict i(int index, int deep = 500)
		{
			if (Count <= index) return this;
			return this[index].i(deep);
		}
			
		public GuiDict i(int deep = 500)
			=> (deep < 0) ?  __i(deep) : i__(deep);
		
		public GuiDict i__(int deep = 500)
		{
			GuiDict g = this;
			int i = 0;
			for (; i < deep && g.dict != null ; i++)
			{
				g = g[0];
			}
			
			
			if (i < deep)
				output = 0;
			else
				output = 1;
				
			return g;
		}
		public string GetVar() => this[0]?.key ;
		public string GetVar(int index) => this[index]?.key ;
		public bool SetVar(string g) => SetVar(0,g);
		public bool SetVar(int index, string g)
		{
			Instance();
			if (dict.Count <= index) Include(g);
			else
				dict[index].key = g;
			
			return true;
		}
		
		public GuiDict this[int index]
		{
			get => Count > index ? dict[index] : null;
			set {
				if (Count <= index) return ;
				if (value.key == null)
					value.key = ""+index;
				dict[index] = value;
				
			}
		}	
		
		public string[] Keys
		{
			get { 
				if (dict == null) return new string[0];
				string[] keys = new string[dict.Count];
				for (int i = 0 ; i < dict.Count ; i++)
				{
					keys[i] = dict[i].key;
				}
			
				return keys;
			}
			
		}
		
		public int Count {
			get => (dict != null) ? dict.Count : 0;
		}
		
		public string DebugVariableTree(string[] delimiter = null, string exception = VARIABLE_OBJECT) 
		{
			if (delimiter == null) delimiter = ALLGUICONST;
			
			
			GuiDict hier = VariableTree(delimiter, exception);
			string s = "";
			for (int i = 0 ; i < hier.Count ; i++)
			{
				s+= hier.dict[i].GetPath().ToString() + "\n";
			}
			
			return s;
		}
		public GuiDict VariableTree(string[] delimiter = null, string exception = VARIABLE_OBJECT) 
		{
			if (delimiter == null) delimiter = ALLGUICONST;
			
			if (dict == null) return null;
			all = new GuiDict();
			all.Instance();
			all.key = "Tree";
			
			_VariableTree(delimiter, exception);
				
			GuiDict g = all;
			all = null;
			return g;
			
		}
		protected void _VariableTree(string[] delimiter, string exception){
			if (dict == null) return;
			
			GuiDict c ;
			
			for (int i = 0 ; i < dict.Count ; i++)
			{
				c = dict[i];
				all.Add(c);
				
				if (c.key == exception || !delimiter.Contains(c.key)
				)
				c._VariableTree(delimiter, exception);
				
			}
			
		}
		
		public static void InstanceStack(GuiDict dict, params string[] key){
			
			int len = key.Length;
			GuiDict droot = dict;
			droot.Instance();
			for (int i = 0 ; i < len ; i++)
			{
				
				
				droot.Instance(key[i]);
				droot = droot.FindLast(key[i]);
			}
			
		}

	
	}
	
		
	public static class GuiGameVars {
		
		public const int DEFAULTDEPTH = 500;
		
		// common dictionary names
		public const string GAME = "_GAME";
		public const string WINDOW_ASCII = "_WNDO";
		public const string REPLACEMENT = "_REPL";
		public const string VARIABLE_OBJECT = "_VAR";
		public const string ANON_OBJECT = "_VAR";
		public const string COMPONENT_OBJECT = "_CPOB";
		public const string OPERATION = "_OPER";
		public const string ACTIVATOR = "_ATVA";
		public const string ACTION = "_ACTN";
		public const string METHOD = "_MTHD";
		public const string STATIC = "_STTC";
		
		
		public static string[] ALLGUICONST = new string[]{GAME, 
		WINDOW_ASCII, REPLACEMENT, VARIABLE_OBJECT,
		COMPONENT_OBJECT, OPERATION, ACTIVATOR,
		ACTION,METHOD, STATIC };

		// dictionary names
		public const string TEMPORARY = "~TEMP";
		public const string VARIABLE_ASSIGNMENT = ".VAR";
		public const string KEY_FILL = "*FIL";
		public const string KEY_ALL = "*ALL";
		
		public const string WINDOW_ATLAS = "W_ATL";
		// code values
		public const string CODE_DEPTH = "_DEPTH_LIMIT";
		public const string CODE_CIRCULAR_REF = "<--CIRC_REF";
		
		public static GuiDict game = new GuiDict(){
			key = GAME
		};
		
		
	}
}