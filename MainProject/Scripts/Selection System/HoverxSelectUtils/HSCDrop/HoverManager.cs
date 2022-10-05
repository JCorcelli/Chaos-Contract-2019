using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SelectionSystem.IHSCx
{
	
	public delegate void HoverManagerInt(int amt);
	public delegate void HoverManagerOb(GameObject ob);
	
	public class HoverManager {

		public static List<Collider2D> dropTargets2D = new List<Collider2D>();
		public static List<SphereCollider> dropTargets = new List<SphereCollider>();
		
		
		public static HoverManagerInt onDrop;
		public static HoverManagerOb onDropOb;
		public static HoverManagerOb onEnableTarget;
		public static HoverManagerOb onEnableTarget2D;
		public static HoverManagerOb onDisableTarget;
		public static HoverManagerOb onDisableTarget2D;
		
		// this makes it drag ready. Just let the dropper I've made handle it or extend the class.
		public static int holding = 0; // something I drop to can read this number
		public static string typeHeld = "dropper"; // something I drop to can read this number
		
		
		public static int dropping = 0;
		public static int remainder = 0;
		public static void Drop(int amt = 1){
			dropping = amt;
			
			holding -= dropping;
			if (onDrop != null) onDrop(amt);
			
			remainder = dropping;
			dropping = 0;
		}
		
		public static void Drop(GameObject ob){
			dropping = 1;
			
			holding -= dropping;
			if (onDropOb != null) onDropOb(ob);
			
			remainder = dropping;
			dropping = 0;
		}
		
	}
}