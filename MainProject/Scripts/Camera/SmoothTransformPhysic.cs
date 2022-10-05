using UnityEngine;

namespace CameraSystem
{
	public class SmoothTransformPhysic : SmoothTransform
	
	{
		public float maxDistanceDelta = 5f;
		protected float throttle = 0f;
		public bool ignoreGround = false;
		
		public override void SmoothMove(){
			
		
			Vector3 position = Vector3.SmoothDamp(transform.position,  target.position, ref direction_velocity, moveTime);
			
			TouchGround(transform);
				
				
			float diff1 = Vector3.Distance(transform.position, position);
			
			if (diff1.IsZero())
			{
				throttle = 1;
				return;
			}
			
			position = Vector3.MoveTowards(transform.position, position, maxDistanceDelta * Time.unscaledDeltaTime);
			
			float diff2 = Vector3.Distance(transform.position, position);
			
			transform.position = position;
			
			throttle = diff2/ diff1;
			
			
		
		}
		public override void SmoothLook(){
			
			float lookTimeAlt;
			if ( throttle.IsZero() ) 
				lookTimeAlt = lookTime;
			else
				lookTimeAlt = lookTime/throttle;
			
			
			
			float yAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target.eulerAngles.y, ref yVelocity, lookTimeAlt);
			float xAngle = Mathf.SmoothDampAngle(transform.eulerAngles.x, target.eulerAngles.x, ref xVelocity, lookTimeAlt);
			float zAngle = Mathf.SmoothDampAngle(transform.eulerAngles.z, target.eulerAngles.z, ref zVelocity, lookTimeAlt);
			
			
			transform.eulerAngles= new Vector3(xAngle, yAngle, zAngle);
			
		}
		

		protected RaycastHit hitInfo;
		protected Vector3 groundPosition;
		protected Vector3 groundNormal = Vector3.up;
		protected float checkDistance = 1000f;
		protected bool groundExists = false;
		public LayerMask activeLayers = 1;
		protected void FloorCheck(Transform transform)
		{
		
			if (Physics.Raycast(transform.position + (Vector3.up * .1f), Vector3.down, out hitInfo, checkDistance, activeLayers))
			{
				groundPosition = hitInfo.point;
				groundNormal = hitInfo.normal;
				groundExists = true;
			}
		}

		protected void TouchGround(Transform t){
			FloorCheck(t);
			if (ignoreGround) return;
			if (groundExists &&  t.position.y < groundPosition.y + .1f)
			
				t.position =  new Vector3(t.position.x, groundPosition.y + .1f, t.position.z);
		}
		
		
		protected override void OnLateUpdate()
		{
			// unless I override it this way, it won't use the new smoothlook, alternatively I could use a delegate 
			
			// Early out if we don't have a target
			if (!target)
				return;
			
			Retarget();
			if (m_move && moveTime > 0f)
			{
				SmoothMove();
			
			}
			else
				transform.position = target.position;

			
			TouchGround(transform);
				
			// Early out if we don't have a target
			if (m_look)
				SmoothLook();
		}	
		
		
	}
}