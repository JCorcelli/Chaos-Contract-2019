using System;
using UnityEngine;


namespace CameraSystem
{
	
    public class SetParentToTarget : MonoBehaviour
    {
        public Transform target;

        private void Start()
        {
			if (target == null) return;
            transform.parent = target;
			transform.localPosition = Vector3.zero;
        }

    }
}
