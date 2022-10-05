using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;


namespace SelectionSystem
{
    public class FollowTargetNavAgent : AbstractButtonHandler
    {
        public Transform target;
		protected NavMeshAgent agent;
		public bool updatePosition = true;
		public bool updateRotation = false;
		public bool tracePath = true;
		protected float safeSpeed = 0.05f;
		public NavMeshAgent[] traceFrom;
		public bool warp = false;

		private void Awake() {
			agent = GetComponent<NavMeshAgent>();
			agent.updatePosition = updatePosition;
			agent.updateRotation = updateRotation;
		}
        protected override void OnPress() { control = true; }
		
		
		protected NavMeshHit navHit;
		protected bool control = true;
        protected override void OnUpdate()
        {
			if (!control) return;
			if (SelectGlobal.locked) { control = false; return; }
			
			if (warp)
			{
				agent.Warp( target.position );
				
			}
				
			if (tracePath && agent.Raycast(target.position, out navHit))
			{
				
					
				foreach(NavMeshAgent a in traceFrom)
				{
					if (!a.Raycast(target.position, out navHit)) 
					{
						
						agent.Warp( target.position );
						break;
					}
				}
				
			}
			else
			{
				if (agent.Raycast(target.position, out navHit))
				{
					Vector3 direction = target.position - transform.position;
					agent.nextPosition = navHit.position + direction.normalized * safeSpeed;
						
					
				}
				else
						agent.nextPosition = target.position;
					
			}
        }
		
		
    }
}
