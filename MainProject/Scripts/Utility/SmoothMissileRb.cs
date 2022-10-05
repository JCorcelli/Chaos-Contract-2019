using UnityEngine;

namespace Utility.Move
{
	public class SmoothMissileRb : UpdateBehaviour
	{
		

		// The target we are following
		
		public Transform target;
		protected Transform prev_target;
		new protected Rigidbody rigidbody;
		public bool m_look = true;
		public bool m_move = true;
		
		public float smoothTime = 0.5f;
		
		
		// required in intermediate calculations
		protected Vector3 direction_velocity = Vector3.zero; 
		protected float yVelocity = 0f;
		protected float xVelocity = 0f;
		protected float zVelocity = 0f;
		protected float force = 75f;
		
		
		public void SetVelocity(Vector3 newv) {
			direction_velocity = newv;
		}
		public void SetVelocity() {
			direction_velocity = Vector3.zero;
					
		}
		
		protected void Awake() {
			rigidbody = GetComponent<Rigidbody>();
			rigidbody.useGravity = false;
		}
		
		// Update is called once per frame
		public void SmoothMove(){
			
			if (target != prev_target)
			{				
				direction_velocity = Vector3.zero;
				prev_target = target;
			}
			
			Vector3 goal = Vector3.SmoothDamp(rigidbody.position,  target.position, ref direction_velocity, smoothTime);
			
			
			Vector3 direction = (goal - rigidbody.position);
			Vector3 dForce;
			if (direction.magnitude > force)
			{
				dForce = direction.normalized * force;
			}
			else
				dForce = direction;
				
			rigidbody.AddForce( dForce , ForceMode.VelocityChange);
		}
		public void SmoothLook(){
			float yAngle = Mathf.SmoothDampAngle(rigidbody.rotation.eulerAngles.y, target.eulerAngles.y, ref yVelocity, smoothTime);
			float xAngle = Mathf.SmoothDampAngle(rigidbody.rotation.eulerAngles.x, target.eulerAngles.x, ref xVelocity, smoothTime);
			float zAngle = Mathf.SmoothDampAngle(rigidbody.rotation.eulerAngles.z, target.eulerAngles.z, ref zVelocity, smoothTime);
			
			
			rigidbody.rotation *= Quaternion.Euler(xAngle, yAngle, zAngle);
			
		}
		
		protected override void OnLateUpdate()
		{
			
			// Early out if we don't have a target
			if (!target)
				return;
			
			if (m_move && smoothTime > 0f)
				SmoothMove();
			else
				rigidbody.MovePosition( target.position );

			
			// Early out if we don't have a target
			if (m_look)
				SmoothLook();
		}
		
	}
}