using System;
using UnityEngine;


namespace UnityStandardAssets.Utility
{
	
    public class FollowTargetFixedUpdate : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset = new Vector3(0f, 0f, 0f);


        private void FixedUpdate()
        {
            transform.position = target.position + offset;
        }
    }
}
