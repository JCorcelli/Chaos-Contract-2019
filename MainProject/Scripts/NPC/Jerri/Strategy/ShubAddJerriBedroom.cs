using UnityEngine;
using NPC.Strategy;

namespace NPC.Jerri.Bedroom
{
	public enum Strategy // this is used to improve the inspector efficiency
	{
		Wait      = 0,
		Comfort   = 1,
		Pet       = 2,
		Food      = 3,
		FeedBunny = 4,
		Laptop    = 5,
		Sleep     = 6,
		//7
		ShooBunny = 8,
		WantBunny = 9,
		GoToBunny = 10,
		Patrol    = 11,
		WantBed   = 12,
		WantDoor  = 13
	}
}


namespace NPC.Jerri.Bedroom
{
	public class ShubAddJerriBedroom: MonoBehaviour {

		protected StrategyHUB shub;
		public Jerri.Bedroom.Strategy assignName;
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