using UnityEngine;
using System.Collections;

namespace PlayerAssets.Game
{
	/// <summary>
	/// Creates wandering behaviour for a Rigidbody.
	/// </summary>
	[RequireComponent(typeof(Rigidbody))]
	public class WanderPhys : MonoBehaviour
	{
		public float speed = 5;
		public float directionChangeInterval = 1;
		public float movementInterval = 2;
		public float maxHeadingChange = 30;
		public bool running = true;
		
		Rigidbody controller;
		float heading;
		Vector3 targetRotation;
		
		void Awake ()
		{
			controller = GetComponent<Rigidbody>();
			
			// Set random initial rotation
			heading = Random.Range(0, 360);
			transform.eulerAngles = new Vector3(0, heading, 0);
			
			StartCoroutine(NewHeading());
		}

		void Update(){
			if (running)
			{


				transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
				var forward = transform.TransformDirection(Vector3.forward).normalized;
				controller.MovePosition(transform.position + forward * speed);
			}


		}
		/// <summary>
		/// Repeatedly calculates a new direction to move towards.
		/// Use this instead of MonoBehaviour.InvokeRepeating so that the interval can be changed at runtime.
		/// </summary>
		IEnumerator NewHeading ()
		{
			while (true) {
				if (running)
					NewHeadingRoutine();
				yield return new WaitForSeconds(directionChangeInterval);
			}
		}
		
		/// <summary>
		/// Calculates a new direction to move towards.
		/// </summary>
		void NewHeadingRoutine ()
		{
			var floor = Mathf.Clamp(heading - maxHeadingChange, 0, 360);
			var ceil  = Mathf.Clamp(heading + maxHeadingChange, 0, 360);
			heading = Random.Range(floor, ceil);
			targetRotation = new Vector3(0, heading, 0);
		}
	}
}