﻿using UnityEngine;
using System.Collections;

namespace Utility.Triggers 
{
	public class TriggerEnablesChild : MonoBehaviour {

		public string targetName = "PresenceIndicator";
		
		private Transform child;
		
		public int count = 0; 
		/*
		if it's rigidbody it can have more than one collider triggered at once 
		// if it's any it can have more than one collider enter it at once, I need to keep count because of these pitfalls
		*/
		void Awake () { 
			child = transform.GetChild(0);  
			child.gameObject.SetActive( false);
		}
		

		void OnTriggerEnter( Collider col ) {
			if (targetName == col.name) {
				count ++;
				child.gameObject.SetActive(true);
				
			}
		}
		void OnTriggerExit( Collider col ) {
			if (targetName == col.name) {
				
				count --;
				if (count <= 0)
				{
					count = 0;
					child.gameObject.SetActive(false);
				}
				
			}
			
		}
	}
}