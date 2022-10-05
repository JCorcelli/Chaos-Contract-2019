using UnityEngine;
using System.Collections;
using Utility.Managers;

namespace BehaviorTree
{
	public class BTDecorPingBool : BTDecor {

		
		public bool truth = true;
		protected override void OnUpdate() {
			current = truth;
		}
	}
}