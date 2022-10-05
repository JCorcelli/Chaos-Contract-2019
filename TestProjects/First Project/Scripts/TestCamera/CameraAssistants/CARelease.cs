using UnityEngine;
using System.Collections;

namespace TestProject.Cameras
{
	public class CARelease : CameraAssistant {
		
		public enum ConditionType {idle = 0, moving = 1, _static =2, kinetic =3, manual = 4}
		public ConditionType conditionType = 0;
		public float releaseDelay = 4f;
		
		public bool releaseCondition = false;
		private float _ci_countdown;
		private Vector3 lastPosition;
		
		protected override void Start () {
			base.Start();
			_ci_countdown = releaseDelay;
			lastPosition = transform.position;
		}
		private bool CheckCondition() {
			bool condition = false;
			if (conditionType == ConditionType.idle)
				condition = !Input.anyKey; // not moving
			else if (conditionType == ConditionType.moving)
				condition = Input.anyKey; // is moving
			else if (conditionType == ConditionType._static)
			{
				condition = Vector3.Distance(transform.position, lastPosition) < 0.1f; // eh?
				lastPosition = transform.position;
			}
			else if (conditionType == ConditionType.kinetic)
			{
				condition = !(Vector3.Distance(transform.position, lastPosition) < 0.1f); // eh?
				lastPosition = transform.position;
			}
			else
			{
				condition = releaseCondition;
				return condition;
			}
			releaseCondition = condition;
			return condition;
		}
		public void SetRelease(bool b) {
			releaseCondition = b;
		}
		protected void ReleaseCheck(){
			// protected void ReleaseCheck() {} ...
			bool _ci_condition = CheckCondition();
			if (_ci_condition)
			{
				_ci_countdown -= Time.deltaTime;
				
				if (_ci_countdown <= 0)
				{
					enabled = false;
				}
				
			}
			else
				_ci_countdown = releaseDelay; // start at the time limit
					
					
		}
		protected override void Update() {
			// protected void ReleaseCheck() {} ...
			ReleaseCheck();
					
					
		}
		
	}
}