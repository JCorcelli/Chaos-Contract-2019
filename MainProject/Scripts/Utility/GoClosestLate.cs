using UnityEngine;
using System.Collections;

namespace Utility
{
	public class GoClosestLate : UpdateBehaviour {

		public Transform startTrans;
		public Transform endTrans;
		public Transform target;
		
        public string targetName = "PlayerCenter";
        public string targetTag = "PlayerRig";
		
		protected Transform GetTarget (string name, string tag) {
			Transform target;
			
			
			target = gameObject.FindNameXTag(name, tag).transform;
			
			if (target == null)  Debug.Log(name + "no target " + name +" found.",gameObject);
			return target;
		}
		
		protected override void OnEnable () {
			if (target == null) 
			{
				if (targetName == "") Debug.LogError("Please enter a target!");
				target = GetTarget(targetName, targetTag);
			}
		}
		protected override void OnLateUpdate () {
			
			Vector3 start = startTrans.position;
			Vector3 end = endTrans.position;
			Vector3 point = target.position;
			transform.position = GetClosestPointOnLineSegment(start, end, point);
		}
		
		public Vector3 GetClosestPointOnLineSegment(Vector3 ntarget) { 
			Vector3 start = startTrans.position;
			Vector3 end = endTrans.position;
			Vector3 point = ntarget;
		
			return GetClosestPointOnLineSegment(start, end, point);
		}
		public Vector3 GetClosestPointOnLineSegment(Vector3 vA, Vector3 vB, Vector3 vPoint) {
			
			Vector3 vVector1 = vPoint - vA;
			Vector3 vVector2 = (vB - vA).normalized;

			float d = Vector3.Distance(vA, vB);
			float t = Vector3.Dot(vVector2, vVector1);

			if (t <= 0)
				return vA;

			if (t >= d)
				return vB;

			Vector3 vVector3 = vVector2 * t;

			Vector3 vClosestPoint = vA + vVector3;

		
			return vClosestPoint;

		}
			
	}
}