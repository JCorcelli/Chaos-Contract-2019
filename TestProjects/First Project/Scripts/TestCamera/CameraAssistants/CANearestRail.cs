using UnityEngine;
using System.Collections;


namespace TestProject.Cameras
{

	public class CANearestRail : MonoBehaviour 
	{

		public float damping = 2.0f;
		public float zoomMultiplier = 1.5f;
		
		public Transform target;
		public Transform lookAtTarget;
		public Transform lookAtDefault;
		
		[SerializeField] protected Transform startPoint;
		[SerializeField] protected Transform endPoint;
		protected CAExpert ccam;
		protected CharacterController controller;
		
		
		protected void Start() {
			ccam = gameObject.GetComponent<CAExpert>();
			
		}
		
		
		private void LateUpdate () {
			Main();
			LookAt ();
			
		}
		void LookAt () {
			
			if (lookAtTarget && lookAtTarget.gameObject.activeInHierarchy)
			{
				Vector3 railPosition = ccam.GetClosestPointOnLineSegment(startPoint.position, endPoint.position, lookAtTarget.position);
				transform.position = railPosition;
			
			}
			else
			{
				Vector3 railPosition = ccam.GetClosestPointOnLineSegment(startPoint.position, endPoint.position, lookAtDefault.position);
				transform.position = railPosition;
			
			}
			
		}
		
			
		protected void Main(){
			
			if (ccam != null)
			{
				Vector3 railPosition = ccam.GetClosestPointOnLineSegment(startPoint.position, endPoint.position, target.position);
				transform.position = railPosition;
				
				
				if (ccam.Contains(target.position)) {
					if (lookAtTarget && !lookAtTarget.gameObject.activeInHierarchy)
						ccam.enabled = true;
					else
						ccam.enabled = false;
				}
				else
					ccam.enabled = false;
				// IsOnScreen?
			
			}
			
			
			
		}
		
	}
}