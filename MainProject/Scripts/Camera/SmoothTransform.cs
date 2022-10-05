using UnityEngine;

namespace CameraSystem
{
	public class SmoothTransform : UpdateBehaviour
	{
		

		// The target we are following
		
		public Transform target;
		
		protected Transform prev_target;
		public bool m_look = true;
		public bool m_move = true;
		
		public float moveTime = 0.5f;
		public float lookTime = 0.5f;
		
		
		
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
		
		public void Retarget() {
			
			if (target != prev_target)
			{				
				direction_velocity = Vector3.zero;
				yVelocity = 0f;
				xVelocity = 0f;
				zVelocity = 0f;
				prev_target = target;
			}
			

			
		}
		public virtual void SmoothMove(){
			
			
			transform.position = Vector3.SmoothDamp(transform.position,  target.position, ref direction_velocity, moveTime);
		}
		public virtual void SmoothLook(){
			float yAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target.eulerAngles.y, ref yVelocity, lookTime);
			float xAngle = Mathf.SmoothDampAngle(transform.eulerAngles.x, target.eulerAngles.x, ref xVelocity, lookTime);
			float zAngle = Mathf.SmoothDampAngle(transform.eulerAngles.z, target.eulerAngles.z, ref zVelocity, lookTime);
			
			transform.eulerAngles= new Vector3(xAngle, yAngle, zAngle);
			
		}
		
		protected override void OnLateUpdate()
		{
			
			// Early out if we don't have a target
			if (!target)
				return;
			
			Retarget();
			if (m_move && moveTime > 0f)
				SmoothMove();
			else
				transform.position = target.position;

			
			// Early out if we don't have a target
			if (m_look)
				SmoothLook();
		}
		
	}
}