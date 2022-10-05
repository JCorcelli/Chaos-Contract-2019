using System;
using UnityEngine;
using UnityEngine.AI;


namespace Utility
{
	
    public class GoToAtStartNavAgent : MonoBehaviour
    {

        public string targetName = "";
        public string targetTag = "";

        public Transform target;
        public Vector3 offset = new Vector3(0f, 0f, 0f);
		
        public NavMeshAgent agent { get; protected set; } 
		
        protected void Start()
        {
			
            agent = GetComponentInChildren<NavMeshAgent>();
			if (target == null)	
				target = gameObject.FindNameXTag(targetName, targetTag).transform;
			if (target == null)	Debug.Log(name + "no target found.",gameObject);
            agent.Warp( target.position + offset);
        }
    }
}
