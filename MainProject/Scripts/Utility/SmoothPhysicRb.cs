using UnityEngine;

namespace Utility.Move
{
	public class SmoothPhysicRb : UpdateBehaviour
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
		public float force = 75f;
		public float drag = .02f;
		public float stopForce = .3f;
		
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
			
			Vector3 goal;
			if (smoothTime > 0.01f)
			{
				goal = Vector3.SmoothDamp(rigidbody.position,  target.position, ref direction_velocity, smoothTime);
			}
			else
				goal = target.position;
			
			float distance = (target.position - rigidbody.position).magnitude;
			
			Vector3 direction = (goal - rigidbody.position);
			Vector3 dForce;
			if (direction.magnitude > force)
			{
				dForce = direction.normalized * force;
			}
			else
				dForce = direction;
			
			if (distance < 1f)
			{
				rigidbody.AddForce(-rigidbody.velocity * stopForce, ForceMode.VelocityChange);
				
			}
			else
				rigidbody.AddForce(-rigidbody.velocity * drag, ForceMode.VelocityChange);
				
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
			
			if (m_move )
				SmoothMove();
			

			
			// Early out if we don't have a target
			if (m_look)
				SmoothLook();
		}
		
	}
}