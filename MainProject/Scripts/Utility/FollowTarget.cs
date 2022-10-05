using System;
using UnityEngine;
using System.Collections;


namespace Utility
{
    public class FollowTarget : UpdateBehaviour
    {
        public Transform target;
        public Vector3 offset = new Vector3(0f, 0f, 0f);

		
        protected override void OnUpdate()
        {
            transform.position = target.position + offset;
        }
		
		
    }
}
