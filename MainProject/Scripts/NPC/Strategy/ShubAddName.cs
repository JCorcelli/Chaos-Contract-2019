using UnityEngine;

namespace NPC.Strategy
{
	public class ShubAddName : MonoBehaviour {

		protected StrategyHUB shub;
		protected void Connect() {
			
			shub = GetComponentInParent<StrategyHUB>();
		}
		protected virtual void OnEnable() {
			if (shub == null)
				Connect();
			if (shub == null)
				return;
			
			shub.Add(name);
		}
		protected virtual void OnDisable() {
			if (shub == null)
				return;
			shub.Remove(name);
		}
	}
}