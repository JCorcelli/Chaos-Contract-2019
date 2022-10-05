using UnityEngine;
using System.Collections;
using Utility.Managers;
using NPCSystem;

namespace NPC.BTree.Jerri
{
	public class JerriBeGoTo : BTUpdateNode {

		// this is like Pinger, but it never returns until explicitly programmed.
		
		public string targetName = "OnClickBunny";
		public string targetTag = "Player";
		public Transform target;
		
		public GameObject iSetTransform;
		public INavTargeting agent;
		
		public NavTargetType navType = (NavTargetType)0;
		
		protected JerriBeStateHUB hub;
		
		protected override void Awake() {
			base.Awake();
			
			hub = GetComponentInParent<JerriBeStateHUB>();
				
		}
		protected override void OnEnable() {
			base.OnEnable();
			
			
			agent = iSetTransform.GetComponent<INavTargeting>();
			if (agent == null) Debug.LogError("missing nav agent", gameObject);
			
			if (target == null)
			target = gameObject.FindNameXTag(targetName, targetTag).transform;
			
			if (target == null) Debug.Log("no target", gameObject);
			SetAgent();
		}
	
		protected override void OnUpdate(){
			if (Near()) 
			{
				Succeed();
			}
			
		}
		
		protected void SetAgent(){
			agent.ResetClear();
			agent.SetNavType((int)navType); 
			agent.SetTransform(target);
			
		}
		protected bool Near(){
			
			// I actually want to know if I'm near one of my destinations.
			return agent.AnyCleared();
			
		}
		
	}
}