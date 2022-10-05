using System;
using UnityEngine;


namespace CameraSystem
{
	
    public class SnapToTarget : UpdateBehaviour
    {
        public Transform target;
		public string _targetName;
		public string _targetTag;


		
		protected void Start () {
			
			if (_targetName != "" )
				target = gameObject.FindNameXTag(_targetName, _targetTag).transform;
			
		}
        protected override void OnUpdate()
        {
            transform.position = target.position;
        }
    }
}
