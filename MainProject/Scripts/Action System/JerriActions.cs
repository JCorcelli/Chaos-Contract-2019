using UnityEngine;
using System.Collections;

namespace ActionSystem
{

	
	
	public class JerriActions
	{
		
		public static string name = "Jerri";
		public static string mhm = 		"JerriMhm"		;
		public static string say_bunny = "JerriSayBunny"	;
		public static string alert = 	"JerriAlert"	;
		
		
		public static string[] actionList = new string[3] {
			mhm,
			say_bunny,
			alert
			};
		
		public static ActionEventDetail GetAction(string s)
		{
			
			foreach (string action in actionList)
			{
				if (action == s)
				{
					// it's a copy. not a template.
					ActionEventDetail a = (ActionEventDetail) s ;
					a.who = name;
			
					return a;
				}
			}
			return null;
		}
		
		
			
			
		
		
	}
		
}