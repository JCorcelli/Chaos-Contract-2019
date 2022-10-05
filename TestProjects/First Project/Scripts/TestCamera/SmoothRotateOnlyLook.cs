using UnityEngine;
namespace TestProject.Cameras
{
	public class SmoothRotateOnlyLook : MonoBehaviour {
		
		public Transform lookAtTarget;
		
		public float damping = 2.0f;
		
		private Transform laDef;
		private Transform ftDef;
		
		void Awake () {
			laDef = lookAtTarget;
			
		}
		
		void Update () {
			if (!lookAtTarget || !lookAtTarget.gameObject.activeInHierarchy)
				return;
			
			
			Quaternion rotate = Quaternion.LookRotation(lookAtTarget.position - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * damping);
		}
		
		public void RSetTarget(Transform t){
			lookAtTarget = t;
			enabled = true;
		}
		
		
		public void Cancel(){
			lookAtTarget = laDef;
			
		}
	}
}