using UnityEngine;
using System.Collections;


namespace TestProject.Cameras
{

	public class FlyThrough : MonoBehaviour 
	{

		public float moveSpeed = 15.0f;
		public float turnSpeed = 15.0f;
		private CameraAssistant ccam;
		private CharacterController controller;
		
		
		private bool m2 = false;
		
		void Start() {
			ccam = gameObject.GetComponent<CameraAssistant>();
			
			controller = gameObject.AddComponent<CharacterController>();
			controller.height = 1f;
			controller.stepOffset = 1f;
			controller.radius = .1f;
			controller.detectCollisions = false;
		}
		
		
		void Update () {
			
			m2 = Input.GetButton("mouse 2");
		}
		void LateUpdate () {
			
			
			
			if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
			{
				
				ccam.enabled = true;
				//transform.position += transform.forward*moveSpeed*Input.GetAxis("Vertical");
				//transform.position += transform.right*moveSpeed*Input.GetAxis("Horizontal");
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