using System;
using UnityEngine;


namespace Utility
{
	
    public class GoToAtStart : MonoBehaviour
    {

        public string targetName = "";
        public string targetTag = "";

        public Transform target;
        public Vector3 offset = new Vector3(0f, 0f, 0f);
		
        protected void Start()
        {
			
			if (target == null)	
				target = gameObject.FindNameXTag(targetName, targetTag).transform;
			if (target == null)	Debug.Log(name + "no target found.",gameObject);
            transform.position = target.position + offset;
        }
    }
}
