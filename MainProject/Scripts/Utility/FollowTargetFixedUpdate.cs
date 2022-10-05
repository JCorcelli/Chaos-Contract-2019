using System;
using UnityEngine;
using System.Collections;


namespace Utility
{
    public class FollowTargetFixedUpdate : UpdateBehaviour
    {
        public Transform target;
        public Vector3 offset = new Vector3(0f, 0f, 0f);

		
        protected override void OnFixedUpdate()
        {
			if (target == null) Debug.LogError("no target", this.gameObject);
            transform.position = target.position + offset;
        }
		
		
    }
}
