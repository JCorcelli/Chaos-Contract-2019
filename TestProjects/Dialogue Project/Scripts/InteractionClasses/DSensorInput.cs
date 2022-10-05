
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
 
using Utility.GUI;
using SelectionSystem;



namespace DialogueSystem
{
	public class DSensorInput : UpdateBehaviour {
		
		public static DSensorInput instance;
		
		
		public static List<DText> outPut { // this is output though
			get{ if (sensor != null) return DUser.selected.sensor.scanned;
				return null;
			}
		}
		
		public static DSensor sensor {
			get{ if (DUser.selected != null) return DUser.selected.sensor;
				return null;
			}
		}
		public static DUser user{ // this is output though
			get{ return DUser.selected;
			}
		}
		// quality order contain||intersect, hmm, how much info was lost? = spills
		
		public static void Contains( DText t ){
			user.sensor.Contains(t);
		}
		
		public static int Intersection( DText t ){
			return user.sensor.Intersection(t);
		}
		public static int Spills( DText t ){
			return user.sensor.Spills(t);
		}
		
		public static void Scan<T>( T t ){
			user.sensor.Scan( t);
			
			
		}
		
		
	}
	
}