using UnityEngine;

namespace CameraSystem
{
	public class GoToTransform : MonoBehaviour
	{
		

		// The target we are following
		
		public Transform target;
		protected Transform prev_target;
		public bool m_look = true;
		public bool m_move = true;
		
		public float smoothTime = 0.5f;
		
		
		// required in intermediate calculations
		protected Vector3 direction_velocity = Vector3.zero; 
		protected float yVelocity = 0f;
		protected float xVelocity = 0f;
		protected float zVelocity = 0f;
		
		
		public void SetVelocity(Vector3 newv) {
			direction_velocity = newv;
		}
		public void SetVelocity() {
			direction_velocity = Vector3.zero;
					
		}
		
		
		
		// Update is called once per frame
		public void SmoothMove(){
			
			if (target != prev_target)
			{				
				direction_velocity = Vector3.zero;
				prev_target = target;
			}
			
			transform.position = Vector3.SmoothDamp(transform.position,  target.position, ref direction_velocity, smoothTime);
		}
		public void SmoothLook(){
			float yAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target.eulerAngles.y, ref yVelocity, smoothTime);
			float xAngle = Mathf.SmoothDampAngle(transform.eulerAngles.x, target.eulerAngles.x, ref xVelocity, smoothTime);
			float zAngle = Mathf.SmoothDampAngle(transform.eulerAngles.z, target.eulerAngles.z, ref zVelocity, smoothTime);
			
			
			transform.eulerAngles= new Vector3(xAngle, yAngle, zAngle);
			
		}
		
		void LateUpdate()
		{
			
			// Early out if we don't have a target
			if (!target)
				return;
			
			if (m_move && smoothTime > 0f)
				SmoothMove();
			else
				transform.position = target.position;

			
			// Early out if we don't have a target
			if (m_look)
				SmoothLook();
		}
		
	}
}