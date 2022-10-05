using UnityEngine;
using System.Collections;


namespace TestProject.Cameras
{

	public class CAFlyingMKII : MonoBehaviour 
	{

		public float damping = 2.0f;
		public float moveSpeed = 15.0f;
		public float turnSpeed = 15.0f;
		public float zoomMultiplier = 1.5f;
		
		public SphereCollider distanceLimiter;
		public Transform lookAtTarget;
		public Transform lookAtDefault;
		protected CAExpert ccam;
		protected CharacterController controller;
		
		protected bool m1 = false;
		protected bool m2 = false;
		
		protected void Start() {
			ccam = gameObject.GetComponent<CAExpert>();
			
			controller = gameObject.AddComponent<CharacterController>();
			controller.height = 1.5f;
			controller.stepOffset = 1f;
			controller.radius = .1f;
			controller.detectCollisions = false;
			
		}
		
		
		protected void Update () {
			
			m2 = Input.GetButton("mouse 2");
			m1 = Input.GetButton("mouse 1");
		}
		private void LateUpdate () {
			Main();
			StayInZone();
			//FitZoom();
			LookAt ();
			
		}
		void LookAt () {
			if (lookAtTarget && lookAtTarget.gameObject.activeInHierarchy)
			{
				ccam.releaseCondition = false;
			
				Quaternion rotate = Quaternion.LookRotation(lookAtTarget.position - transform.position);
				transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * damping);
			}
			else
			{
			
				Quaternion rotate = Quaternion.LookRotation(lookAtDefault.position - transform.position);
				transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * damping);
			}
		}
		
		protected void StayInZone() {
			float d = Vector3.Distance( distanceLimiter.transform.position, transform.position ); // distance from center
			if ( d  > distanceLimiter.radius )
				controller.Move(Vector3.MoveTowards(transform.position, distanceLimiter.transform.position, d - distanceLimiter.radius)-transform.position); // extra distance after subtracting the max
		}
			
		protected void FitZoom(){
			Vector3 goal = ccam.GetZoom(zoomMultiplier);
			
			float speed = moveSpeed * Time.deltaTime;
			controller.Move(Vector3.MoveTowards(transform.position, goal, speed)-transform.position); // this is a vaccuum motion that doesn't care about distance towards the zoom position, assuming that the zoom position works
		}
		protected void Main(){
			if (ccam != null)
			{
				if (m1)
					ccam.releaseCondition = true;
				if (Input.GetKey("space")) 
				{
					ccam.enabled = false;
					return;
				}
			}
			if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
			{
				
				ccam.releaseCondition = false;
				ccam.enabled = true;
				
				
				controller.Move(transform.forward*Time.deltaTime * moveSpeed*Input.GetAxis("Vertical"));
				controller.Move(transform.right*Time.deltaTime *moveSpeed*Input.GetAxis("Horizontal"));
			}
			if (m2)
			{
				Vector2 mousePos = Input.mousePosition;
				mousePos.x = mousePos.x / Screen.width * 2 - 1;
				mousePos.y = mousePos.y / Screen.height * 2 - 1;
				mousePos.x = Mathf.RoundToInt(mousePos.x);
				mousePos.y = Mathf.RoundToInt(mousePos.y);
				
				// rotate !!!
				Quaternion rotation = Quaternion.LookRotation(transform.forward);
				rotation *= Quaternion.Euler(-mousePos.y, mousePos.x, 0);
				transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turnSpeed);
				
			}
			
			
		}
		
	}
}