using System;
using UnityEngine;
using System.Collections;


namespace SelectionSystem
{
    public class FollowTargetFixedUpdateLockable : AbstractButtonHandler
    {
        public Transform target;
        public Vector3 offset = new Vector3(0f, 0f, 0f);


		
        protected override void OnPress() { enabled = true; }
		
        protected override void OnFixedUpdate()
        {
			if (SelectGlobal.locked) { enabled = false; return; }
            transform.position = target.position + offset;
        }
		
		
    }
}
