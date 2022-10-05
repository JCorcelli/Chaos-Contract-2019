using UnityEngine;
using System.Collections;
using NPC.Strategy;

namespace NPC.BTree.Jerri
{
	public class TouchBunnyMeasure : BeStateRegister {

		public static int touching = 0; // sentinel value for script
		
		new protected Transform transform;
		public Transform target;
		public string targetName = "OnClickBunny";
		public string targetTag = "Player";
		
		protected JerriBeStateHUB hub;
		protected StrategyHUB shub;
		
		protected string strat;
		
		protected void Start () {
			strat = "Touching";
			shub = GetComponentInParent<StrategyHUB>();
				
			
			transform = GetComponent<Transform>();
			hub = GetComponentInParent<JerriBeStateHUB>();
			
			if (target == null)
				target = gameObject.FindNameXTag(targetName, targetTag).transform;
			if (target == null)
				Debug.Log("target == null",gameObject);
		}
		
		protected void OnTriggerEnter(Collider col)
		{
			if (col.name == target.name)
				current = true;
		}
		protected void OnTriggerExit(Collider col)
		{
			if (col.name == target.name)
				current = false;
		}
		
		protected override void Register() {
			TouchBunnyMeasure.touching += 1;
			if (TouchBunnyMeasure.touching > 1) return;
			hub.Add(ActiveStatesEnum.TouchBunny);
			
			shub.Add(strat);
			
		}
		protected override void Unregister() {
			TouchBunnyMeasure.touching -= 1;
			if (TouchBunnyMeasure.touching <= 0 ) 
			{
				TouchBunnyMeasure.touching = 0;
				hub.Remove(ActiveStatesEnum.TouchBunny);
				shub.Remove(strat);
			}
			
			
		}
		
	}
}