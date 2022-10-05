using UnityEngine;
using System.Collections;
using SelectionSystem;

namespace CameraSystem
{
	public class FrameAndTargetDelayNorotate : AbstractButtonHandler  {
		
		public Transform lookAtTarget;
		public Transform frameTarget;
		
		public float height = 2.0f;
		public float distance = 2.0f;
		
		
		protected bool isRotating = false;
		protected Transform laDef;
		protected Transform ftDef;
		
		protected void Awake () {
			laDef = lookAtTarget;
			ftDef = frameTarget;
			buttonName = "mouse 2";
		}
		
		protected override void OnEnable() {
			base.OnEnable();
			isRotating = true;
			
				
		}
		protected override void OnPress() {
			isRotating = true;
			StopCoroutine("_DelayOff");
		}
		protected override void OnRelease() {
			if (SelectGlobal.rotateOffTimer < 0) return;
			DelayOff();
		}
		protected override void OnDisable() {
			base.OnDisable();
			isRotating = false;
			StopCoroutine("_DelayOff");
		}
		protected void DelayOff() {
			StopCoroutine("_DelayOff");
			StartCoroutine("_DelayOff");
		}
		protected IEnumerator _DelayOff(){
			yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(SelectGlobal.rotateOffTimer));
			isRotating = false;
			
		}
		
		protected void DoTask() {
			
			if (lookAtTarget == null || !lookAtTarget.gameObject.activeInHierarchy)
				return;
			
				
			if (isRotating)
			{
				Quaternion rotate = Quaternion.LookRotation(lookAtTarget.position - transform.position);
				transform.rotation = rotate;
			}
			
			if (frameTarget != null && frameTarget.gameObject.activeInHierarchy)
			{
			
			

				transform.position = frameTarget.position - transform.forward * distance + Vector3.up * height;
				
			}
			
			
		}
		protected override void OnLateUpdate () {
			DoTask();
				
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