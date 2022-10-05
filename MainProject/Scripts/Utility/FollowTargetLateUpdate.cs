using System;
using UnityEngine;
using System.Collections;


namespace Utility
{
    public class FollowTargetLateUpdate : UpdateBehaviour
    {
        public Transform target;
        public string targetName = "PlayerCenter";
        public string targetTag = "PlayerRig";
		
        public Vector3 offset = new Vector3(0f, 0f, 0f);

		
        protected override void OnLateUpdate()
        {
            transform.position = target.position + offset;
        }
		
		
        protected void Start()
        {
			
			if (target == null)	
				target = gameObject.FindNameXTag(targetName, targetTag).transform;
			if (target == null)	Debug.Log(name + "no target found.",gameObject);
            transform.position = target.position + offset;
        }
		
    }
}
