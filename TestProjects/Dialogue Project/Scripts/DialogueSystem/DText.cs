
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

using Utility.GUI;
using SelectionSystem;
using System.Text.RegularExpressions;
using System.Linq;


namespace DialogueSystem
{

	public partial class DText {
		// This is a text object
		// Bound to current/last user / storage

		public DSource sourceText; // the absolute baseline text
		public DText parent; 
		public StringBuilder storedText = new StringBuilder(); // marked
		
		
		// ACTIVE STATUS
		
		
		public DUser _currentUser;
		public DUser currentUser
		{
			get 
			{ 
				return _currentUser;
				
			}
			set
			{ 
				lastUser = _currentUser;
				
				_currentUser = value;
				
			}
		}
		public DUser lastUser;
		public DUser creator ;
		
		public string media
		{
			// no circular ref
			get{return (storage != null) ? storage.media : null;}
			set{}
		}
		public DMutator mutator
		{
			// no circular ref
			get{return (storage != null) ? storage.mutator : null;}
			set{}
		}
		public Vector3 _location;
		public Vector3 location
		{
			// no circular ref
			get{return (storage != null) ? storage.location : this._location;}
			set{this._location = value;}
		}
		
		
		public DStorage storage;
		// STATUS
		public int charPos = 0;
		public bool readOnly  = false;
		public int  mutations = 0;
		
		public bool isRead = false;
		public bool isStored = false;
		public bool isFiltered = false;
		public bool isDisplayed = false; // present
		public bool isDead = false; // present
		
		public bool isSource = false;
		public bool isPure = false;
		public bool isFact = false;
		public bool isTrue = false;
		public bool isDistorted = false;
		
		// PROC?
		
		// eyesight "use"
		// seeing the source of noise improves fidelity
		
		
		public int sensorMask = 0; // 0 is any i guess
		public string storageType = "Any";
					
		//knowledge base
		//stimuli
		
		
		
		
		
		
		public string creationDate = DateTime.Now.ToString();
		public float tmodified = -1f; // streamed time, can be estimated later with creation date
		public float tstream = -1f; // streamed time, can be estimated later with creation date
		public int parentIndex; // streamed time, can be estimated later with creation date
		
		
		// not to be mistaken for stringbuilder append.
		public void DAppend<T>(T s){
			storedText.Append(s);
			tmodified = Time.time;
			if (tstream < 0)tstream = Time.time;
		}
		public void DReplace(string s, string rep){
			storedText.Replace(s, rep);
			
		}
		public void DReplace(char s, char rep){
			storedText.Replace(s, rep);
			
		}
		public int DIndexOf(string s){
			return storedText.IndexOf(s, 0);
			
		}
		public int DIndexOf(string s, int start, int end = -1){
			return storedText.IndexOf(s, start, end);
			
		}
		
		// rename if I want to use a property named "Item"
		//[System.Runtime.CompilerServices.IndexerName("TheItem")]
		public char this[int index]
		{
			get => storedText[index];
			set {
				storedText.Remove(index, 1); 
				DInsert(index, ""+value);
			}
		}	

		public void DInsert(int i, string s, int count = 1){
			storedText.Insert(i, s, count);
			tmodified = Time.time;
			
		}
		
		public void DRemove(int i, int length ){
			storedText.Remove(i, length);
			tmodified = Time.time;
			
		}
		
		public void Save()
		{
			// not for streams
			
		}
		public DText(string s):this()
		{
			
			DAppend(s);
			
		}
		public DText(){
			//location
			//creator
			//currentUser
			
			
		}
		
		
		
		
		//public void EstimateDuration(){
		//	// a play would be something like length/cps + sleep + wait
		//	// a book is length/cps
		//}
		
		
		public int Length{
			get{ return storedText.Length; }
		}
		public void Clear(){EmptyStream();}
		public void EmptyStream(){
			// this would appear to be redundant with Clone("")
			storedText.Clear();
			tmodified = Time.time;
			if (tstream < 0)tstream = Time.time;
			
		}
		
		
		public string DToString(int start = -1, int length = -1){
			// before determining fidelity
			
			if (length >= 0)
				return this.storedText.ToString(start, length);
			else if (start > 0)
				return this.storedText.ToString(start, storedText.Length - start);
		
			else
				return this.storedText.ToString();
		
		}
		public DText Clone(){
			// before determining fidelity
			
			return Clone(this.storedText.ToString());
		
		}
		public DText Clone(string ptext){
			
			
			DText newText = Activator.CreateInstance(
			this.GetType()) as DText;
			
			
			newText.sourceText = this.sourceText; 
			
			newText.parent = this.parent; // there's an issue, when does the parent change? they all reference the purest source?
			newText.parentIndex = this.parentIndex;
			newText.creationDate = this.creationDate;
			
			
			newText.storedText = new StringBuilder(ptext);
			
			newText.mutations = this.mutations;
			newText.currentUser = this.currentUser;
			newText.lastUser = this.lastUser;
			newText.storageType =  this.storageType;
			newText.isStored =  this.isStored;
			newText.tstream = Time.time;
			newText.tmodified = Time.time;
			return newText;
		}
	}
	
}