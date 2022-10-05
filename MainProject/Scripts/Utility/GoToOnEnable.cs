using System;
using UnityEngine;


namespace Utility
{
	
    public class GoToOnEnable : MonoBehaviour
    {
		public string targetName = "";
		public string targetTag = "";
        public Transform target;
        public Vector3 offset = new Vector3(0f, 0f, 0f);

        private void Awake()
		{
			
			if (target == null)	
				target = gameObject.FindNameXTag(targetName, targetTag).transform;
			if (target == null)	Debug.LogError("No target found.", gameObject);
		}

        private void OnEnable()
        {
            transform.position = target.position + offset;
        }
    }
}
