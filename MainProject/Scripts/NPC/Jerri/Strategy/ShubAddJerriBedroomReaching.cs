using UnityEngine;
using NPC.Strategy;



namespace NPC.Jerri.Bedroom
{
	public enum Reaching // this is used to improve the inspector efficiency
	{
		ReachStanding   = 0,
		ReachInBed		= 1,
		ReachKneeling   = 2,
		Touching 		= 3,
		ReachForDoor	= 4
	}
	
	public class ShubAddJerriBedroomReaching: MonoBehaviour {

		protected StrategyHUB shub;
		public Jerri.Bedroom.Reaching assignName;
		protected string strat = "";
		protected void Connect() {
			strat = assignName.ToString();
			//if (name != strat) Debug.Log(name + " != " + strat,gameObject);
			shub = GetComponentInParent<StrategyHUB>();
		}
		protected virtual void OnEnable() {
			if (shub == null || strat == "")
				Connect();
			if (shub == null)
				return;
			
			shub.Add(strat);
		}
		protected virtual void OnDisable() {
			if (shub == null)
				return;
			
			shub.Remove(strat);
		}
	}
}