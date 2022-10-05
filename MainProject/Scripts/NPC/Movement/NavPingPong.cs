using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace NPCSystem 
{
    //[RequireComponent(typeof (ThirdPersonBunny))]
	
    [RequireComponent(typeof (NavMeshAgent))]
	
	public class NavPingPong : UpdateBehaviour {
		
		public bool repeat = true;
		public float waypointThreshold = 1f;
		public Transform waypointList;
		public Transform currentTarget;
		
		protected int i = 0;
		protected NavMeshAgent _agent;
		public NavMeshAgent agent {protected set{_agent = value;} get{return _agent;}}
		// Use this for initialization
		protected void Start () {
			agent = GetComponentInChildren<NavMeshAgent>();
			
			if (waypointList == null)
			{
				Debug.Log(name + " has no waypoint list",gameObject);
				enabled = false;
				return;
			}
			
            if (currentTarget == null && waypointList.childCount >0 )
			{
				currentTarget = waypointList.GetChild(0);
			
			}
			
            if (currentTarget == null) 
			{
				Debug.Log(name + " has no target",gameObject);
				enabled = false;
				return;
			}
			
			agent.SetDestination(currentTarget.position);
		}
		
		// Update is called once per frame
		override protected void OnUpdate () {
			
            if (currentTarget != null)
			{
				if (agent.enabled)
				{
					
					bool hit =  (agent.remainingDistance <= waypointThreshold || agent.velocity.magnitude < 0.1f);
					if (hit) i ++;
					
					float ia = Mathf.PingPong(i, waypointList.childCount - 1);
					if (i > waypointList.childCount * 2 - 2) // twice the distance minus the endpoints is back to start. alternatively I could reverse the array
					{
						i = 0;
						if (!repeat) enabled = false;
					}
					
						
					
					
					int iaa = Mathf.RoundToInt(ia);
					
					currentTarget = waypointList.GetChild(iaa);
					
					agent.SetDestination(currentTarget.position);
					
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
		
	}
}