using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace NPCSystem
{
    [RequireComponent(typeof (NavMeshAgent))]
	
	public class SpriteNavigate	: UpdateBehaviour {
		
		
		public bool updatePosition = true;
		public bool updateRotation = false;
		
		bool m_IsGrounded;
		const float k_Half = 0.5f;
		public Transform target;
		public string _targetName;
		public string _targetTag;
		
		protected NavMeshAgent agent;
		
		
		protected virtual void Start () {
			if (_targetName != "" )
				target = gameObject.FindNameXTag(_targetName, _targetTag).transform;
			agent = GetComponent<NavMeshAgent>();
			
			agent.updatePosition = updatePosition;
			agent.updateRotation = updateRotation;
		}
		protected override void OnUpdate () {
		
            if (target != null)
			{
				if (agent.enabled)
					agent.SetDestination(target.position);
				if (!agent.hasPath) 	
				{
					return;
				}
				
			}
            else
            {
				if (agent.enabled)
					agent.isStopped = true;
            }
		}
		
        public void SetTarget(Transform target)
        {
            this.target = target;
        }
		






		
				


		
	}

}