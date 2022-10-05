using System;
using UnityEngine;
using System.Collections;


namespace Utility
{
    public class FollowPlayer : UpdateBehaviour
    {
		
        public Vector3 offset = new Vector3(0f, 0f, 0f);

		
        protected override void OnUpdate()
        {
            transform.position = PlayerVars.target.position + offset;
        }
		
		
    }
}
