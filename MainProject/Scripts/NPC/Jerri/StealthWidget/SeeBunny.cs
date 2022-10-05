using UnityEngine;
using System.Collections;


using StealthSystem;
using ActionSystem;
using ActionSystem.OnActionScripts;
namespace NPCSystem
{
	public class SeeBunny : MonoBehaviour, IOnAction {
	
		public SeeTarget st;
		
		public bool hasVisibility = false;
		
		public void OnAction(ActionEventDetail data) {
			if ( data.what.ToLower() == "hidden") 	
			{
				hasVisibility = false;
			}
			else if ( data.what.ToLower() == "visible")
				hasVisibility = true;
		}
	}
}