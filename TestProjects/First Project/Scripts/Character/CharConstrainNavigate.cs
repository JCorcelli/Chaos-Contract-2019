using UnityEngine;
using System.Collections;

namespace TestProject.Cameras
{

	public class CharConstrainNavigate : CharacterNavigate {

		// Use this for initialization
		public SphereCollider distanceLimiter;
		
		// Update is called once per frame
		private void Update () {
		
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
				Move(agent.desiredVelocity, false, false);
				
			}
            else
            {
                //agent.ResetPath();
                // We still need to call the character's move function, but we send zeroed input as the move param.
                Move(Vector3.zero, false, false);
            }
			StayInZone();
		}
		protected void StayInZone() {
			float d = Vector3.Distance( distanceLimiter.transform.position, agent.nextPosition ); // distance from center
			if ( d   > distanceLimiter.radius  )
			{
				
				agent.nextPosition = Vector3.MoveTowards(transform.position, distanceLimiter.transform.position, d - distanceLimiter.radius); // extra distance after subtracting the max
			}
			
		}
	}
}