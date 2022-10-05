using UnityEngine;
using System.Collections;

namespace NPCSystem
{

	public class CharConstrainNavigate : CharacterNavigate {

		// Use this for initialization
		public SphereCollider distanceLimiter;
        public string _sphereColliderName;
        public string _sphereColliderTag;
		
		// Update is called once per frame
		
		override protected void Start () {
			base.Start ();
			if (_sphereColliderName != "")
			{
				distanceLimiter = gameObject.FindNameXTag(_sphereColliderName, _sphereColliderTag).GetComponent<SphereCollider>();
			}
			
		}
		protected override void OnUpdate () {
            if (target != null)
			{
				if (agent.enabled)
				{
					agent.SetDestination(target.position);
					
				}
				if (!agent.hasPath) 	
				{
					return;
				}
				Move(agent.desiredVelocity);
				
				
			}
            else
            {
                //agent.ResetPath();
                // We still need to call the character's move function, but we send zeroed input as the move param.
                Move(Vector3.zero);
				if (agent.enabled)
					agent.isStopped = true;
            }
			StayInZone();
		}
		protected void StayInZone() {
			float d = Vector3.Distance( distanceLimiter.transform.position, agent.nextPosition ); // distance from center
			
			float scaledDistanceLimiter = distanceLimiter.radius * distanceLimiter.transform.lossyScale.y;
			if ( d   > scaledDistanceLimiter  )
			{
				
				agent.nextPosition = Vector3.MoveTowards(transform.position, distanceLimiter.transform.position, d - scaledDistanceLimiter); // extra distance after subtracting the max
			}
			
			
		}
	}
}