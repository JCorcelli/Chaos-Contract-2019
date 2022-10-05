using UnityEngine;
using System.Collections;

namespace NPC.BTree
{
	public class BTPingBool : BTPinger {

		// Use this for initialization
		
		public bool wanting = true;
		
		
		protected override void OnUpdate() {
			current = wanting;
		}
			
	}
}