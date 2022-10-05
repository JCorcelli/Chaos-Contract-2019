using UnityEngine;
using System.Collections;

namespace PlayerAssets.Game
{
    [RequireComponent(typeof (ThirdPersonBunny))]
	[RequireComponent(typeof (PhysicsAgent))]
	///Continuously Follows a target - may be clone of AIBunnyControl script that uses PhysicsAgent instead
	public class OnClickBunnyControl : MonoBehaviour {
        public PhysicsAgent agent { get; private set; } // the navmesh agent required for the path finding
        public ThirdPersonBunny character { get; private set; } // the character we are controlling
		public Transform target;

		// Use this for initialization
		private void Start () {
			agent = GetComponentInChildren<PhysicsAgent>();
            character = GetComponent<ThirdPersonBunny>();
		}
		
		// Update is called once per frame
		private void Update () {
			
            if (target != null && agent.isFollowing)
            {
                agent.SetDestination(target.position);

                // use the values to move the character
                character.Move(agent.desiredVelocity, false, false);
            }
            else
            {
                //agent.ResetPath();
                // We still need to call the character's move function, but we send zeroed input as the move param.
                character.Move(Vector3.zero, false, false);
            }
		
		}
        public void SetTarget(Transform target)
        {
			agent.isFollowing = true;
            this.target = target;
        }
		
	}
}