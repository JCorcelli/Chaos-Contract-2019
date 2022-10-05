using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Utility
{
	public class NamedObjectSolo : MonoBehaviour {

		public static HashSet<string> names = new HashSet<string>();
		
		protected bool valid = false;
		
		public bool persistantOnLoad = false;
		protected void Awake() {
			if (names.Contains(this.name.ToLower())) 
			{
				GameObject.Destroy(this.gameObject);
				return;
			}
			
			names.Add(this.name.ToLower());
				
			valid = true;
			if (persistantOnLoad)
				Object.DontDestroyOnLoad(this.gameObject);
		}
		protected void OnDestroy() {
			if (names == null) return;
			if (!valid) return;
			names.Remove(this.name.ToLower());
		}
	}
}