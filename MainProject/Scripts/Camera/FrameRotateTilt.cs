using UnityEngine;
namespace CameraSystem
{
	public class FrameRotateTilt : MonoBehaviour {
		
		public Transform lookAtTarget;
		public Transform frameTarget;
		
		public float height = 2.0f;
		public float distance = 2.0f;
		public float damping = 2.0f;
		
		protected Transform laDef;
		protected Transform ftDef;
		
		void Awake () {
			laDef = lookAtTarget;
			ftDef = frameTarget;
			
		}
		
		void Update () {
			
			if (frameTarget != null && frameTarget.gameObject.activeInHierarchy)
			{
			
			
				transform.position = Vector3.Lerp(transform.position, frameTarget.position - transform.forward * distance + Vector3.up * height, Time.unscaledDeltaTime * damping);
			}
			
			if (lookAtTarget == null || !lookAtTarget.gameObject.activeInHierarchy)
				return;
			
				
				
			Quaternion rotate = Quaternion.LookRotation(lookAtTarget.position - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.unscaledDeltaTime * damping);
				
		}
		
		public void RSetTarget(Transform t){
			lookAtTarget = t;
			enabled = true;
		}
		public void RSetFrame(Transform t){
			frameTarget = t;
			enabled = true;
		}
		
		
		public void Cancel(){
			lookAtTarget = laDef;
			frameTarget = ftDef;
			
		}
	}
}