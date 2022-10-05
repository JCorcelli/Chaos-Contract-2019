
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


namespace GuiGame
{


	public partial class BText {
		// This is a text object
		// Bound to current/last user / storage

		public TextAsset sourceText; // the absolute baseline text
		public StringBuilder storedText = new StringBuilder(); // marked
		
		
		
		public void Replace(string s, string rep){
			storedText.Replace(s, rep);
			
		}
		public void Replace(char s, char rep){
			storedText.Replace(s, rep);
			
		}
		// not to be mistaken for stringbuilder append.
		public void Append(char s, int count = 1){
			storedText.Append(s, count);
			
		}
		public void Append(string s){
			storedText.Append(s);
			
		}
		public int IndexOf(string s){
			return storedText.IndexOf(s, 0);
			
		}
		public int IndexOf(string s, int start, int end = -1){
			return storedText.IndexOf(s, start, end);
			
		}
		
		public bool HasLetter()
		
		{
			
			for (int i = 0 ; i < storedText.Length ; i++)
			{
				if (storedText[i].isLetter()) return true;
			}
			return false;
			
		}
		public int Count(string s){
			
			return this.ToString().Count(s);
		}
		
		public int Count(char s){
			
			return this.ToString().Count(s);
		}
		
		// rename if I want to use a property named "Item"
		//[System.Runtime.CompilerServices.IndexerName("TheItem")]
		public char this[int index]
		{
			get => storedText[index];
			set {
				storedText.Remove(index, 1); 
				Insert(index, ""+value);
			}
		}	

		public void Insert(int i, char s, int count = 1){
			storedText.Insert(i, ""+s, count);
			
			
		}
		public void Insert(int i, string s, int count = 1){
			storedText.Insert(i, s, count);
			
			
		}
		
		
		public void Remove(string s ){
			int i = IndexOf(s);
			if (i >= 0)
				Remove(i, s.Length);
			
		}
		
		public void Remove(int i, int length ){
			storedText.Remove(i, length);
			
			
		}
		
		
		public BText(TextAsset s):this()
		{
			sourceText = s;
			Append(s.ToString());
			
		}
		public BText(string s):this()
		{
			
			Append(s);
			
		}
		public BText(){
			
			
			
		}
		
		
		
		public int Length{
			get{ return storedText.Length; }
		}
		public void Clear(){EmptyStream();}
		public void EmptyStream(){
			// this would appear to be redundant with Clone("")
			storedText.Clear();
			
			
		}
		
		
		public string ToString(int start = -1, int length = -1){
			// before determining fidelity
			
			if (length >= 0)
				return this.storedText.ToString(start, length);
			else if (start > 0)
				return this.storedText.ToString(start, storedText.Length - start);
		
			else
				return this.storedText.ToString();
		
		}
		public BText Clone(){
			// before determining fidelity
			
			return Clone(this.storedText.ToString());
		
		}
		public BText Clone(string ptext){
			
			
			BText newText = Activator.CreateInstance(
			this.GetType()) as BText;
			
			
			newText.sourceText = this.sourceText; 
			
			
			return newText;
		}
	}
	
}