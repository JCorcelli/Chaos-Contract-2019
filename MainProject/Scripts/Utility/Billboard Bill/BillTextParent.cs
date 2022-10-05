using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Utility
{
	public class BillTextParent : UpdateBehaviour {

		public BillText billTextPrefab;
		
		public bool hasText = true;
		public bool repeat = false;
		public int maxChildren = 3;
		public List<string> text;
		
		
		
		
		public float delay = 1f;
		protected float timer = 0f;
		protected override void OnUpdate(){
			if (transform.childCount > maxChildren)
				recycleOldest();
			
			// available when <=0
			if (timer <= 0 && text.Count > 0)
				hasText = true;
			else
				hasText = false;
			// if it is delayed this guarantees 1 frame
			
			timer -= Time.deltaTime;
			
		}
		
		protected void recycleOldest(){}
		
		public void Add(string s){
			text.Add(s);
		}
		public string Pop(int i){
			if (text.Count == 0) return "";
			string extract = text[i];
			
			if (repeat)
				text.Add(extract);
			text.RemoveAt(i);
			if (text.Count == 0 || delay > 0) 
			{
				hasText = false; 
				timer = delay; 
			}
			return extract;	
					
		}
		public string Pop(){
	
			return Pop(0);		
		}
		
	}
}