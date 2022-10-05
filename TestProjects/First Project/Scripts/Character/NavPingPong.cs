using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace TestProject 
{
    //[RequireComponent(typeof (ThirdPersonBunny))]
	
    [RequireComponent(typeof (NavMeshAgent))]
	
	public class NavPingPong : MonoBehaviour {
		
		public bool repeat = true;
		public float waypointThreshold = 1f;
		public Transform[] target;
		private Transform currentTarget;
		
		private int i = 0;
		private NavMeshAgent agent;
		// Use this for initialization
		void Start () {
			
			agent = GetComponentInChildren<NavMeshAgent>();
            if (target.Length >0 )
			{
				currentTarget = target[0];
			
				agent.SetDestination(target[0].position);
			}
			
		}
		
		// Update is called once per frame
		void Update () {
			
            if (currentTarget != null)
			{
				if (agent.enabled)
				{
					
					bool hit =  (agent.remainingDistance <= waypointThreshold || agent.velocity.magnitude < 0.1f);
					if (hit) i ++;
					
					float ia = Mathf.PingPong(i, target.Length - 1);
					if (i > target.Length * 2 - 2) // twice the distance minus the endpoints is back to start. alternatively I could reverse the array
					{
						i = 0;
						if (!repeat) enabled = false;
					}
					
						
					
					
					int iaa = Mathf.RoundToInt(ia);
					
					agent.SetDestination(target[iaa].position);
					currentTarget = target[iaa];
					
				}
				//character.Move(agent.desiredVelocity, false, false);
				
			}
            else
            {
                agent.ResetPath();
                // We still need to call the character's move function, but we send zeroed input as the move param.
                //character.Move(Vector3.zero, false, false);
            }
		}
		
        public void SetTarget(int index, Transform target)
        {
            this.target[index] = target;
        }
	}
}