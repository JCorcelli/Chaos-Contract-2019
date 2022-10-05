using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace Dungeon
{
	public class DungeonPreviewItem : MonoBehaviour {
		
		public static DungeonPreviewItem instance;
		
		public DungeonPreviewRead[] columns;
		protected void Awake() {
			
			if (instance != null) return;
			
			instance = this;
		}
		
		public void Run(string[] s){
			if (s.Length < 1) return;
			
			columns[0].Run(s[0]);
			columns[1].Run(s[1]);
		}
		
		
	}
}