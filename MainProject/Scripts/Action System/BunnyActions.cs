using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;


namespace ActionSystem
{
	
	public enum BunnyActionEnum { 
		Being_Held  =0 ,
		Being_Pet   =1 ,
		Eating      =2 ,
		Exploring   =3 ,
		Moving      =4 ,
		Napping     =5 ,
		Sleeping    =6 ,
		Sniffing    =7 , 
		Digging    =8 }
		
		
		
	public class BunnyActions : MonoBehaviour
	{
		// ActionCallback is subscribed to manager or multiple callback functions
		
		
	
		
		
		/// <summary>
		/// default, similar to idle but as an action
		/// </summary>
		public string being_held  = "being held"  ;
		public string being_pet   = "being pet"   ;
		public string eating      = "eating"      ;
		public string exploring   = "exploring"   ;
		public string moving      = "moving"      ;
		public string napping     = "napping"     ;
		public string sleeping    = "sleeping"    ;
		public string sniffing    = "sniffing"    ;	
		public string digging     = "digging"     ;	
		
	}
}