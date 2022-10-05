using UnityEngine;
using System.Collections;

namespace Utility.Tree
{
	
	public class LoadingTree : MonoBehaviour {

		public static AsyncOperation synced;
	
		public static string level = "Default";
		public static string exit = "";
		public static bool additive = false;
		
		public static void Load(string newLevel, bool add = false)
		{
			additive = add;
			_Load(newLevel);
		}
		public static void _Load(string newLevel)
		{
			if (instance == null) 
			{
				Debug.Log("No loading tree instance exists");
				return;
			}
			level = newLevel;
			
			instance.Run();
		}
		
		
		
		public static LoadingTree instance;
		
		
		public void Run() {
			transform.GetChild(0).gameObject.SendMessage("Ready");
		}
		
		protected void Awake() {
			
			if (instance == null) instance = this;
			else
			{
				GameObject.Destroy(this);
				return;
			}
			
		}
		
	}

}