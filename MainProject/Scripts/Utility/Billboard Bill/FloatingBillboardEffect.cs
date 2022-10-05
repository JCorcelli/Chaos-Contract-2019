using UnityEngine;
using System.Collections;


namespace Utility
{
	public class FloatingBillboardEffect : UpdateBehaviour {
		
		
		protected Transform target;
		public float speed;
		public float startSpeed = 0f;
		public float maxSpeed = 5f;
		protected float accelerationRate;
		public Vector3 vector;
		public Vector3 acceleration = new Vector3();
		public float gravityMultiplier = -0.1f;
		
		// sprite-like qualities
		public float lifespanSeconds = 5f;
		public bool canKill = true;
		public bool rotateYOnly = true;
		public bool flipY = true;
		
		protected float timeAlive = 0f;
		protected bool isAlive = false;
		
		new protected Renderer renderer;
		
		protected void Awake () {
			renderer = gameObject.GetComponent<Renderer>();
			speed = startSpeed;
			
			// set accel vector
			vector = acceleration + gravityMultiplier * Physics.gravity;
			
			// set accel rate
			accelerationRate = vector.magnitude;
			
			
			target = Camera.main.transform;
		}
		// Update is called once per frame
		
		public void Kill(){
			isAlive = false;
			timeAlive = 0f;
			renderer.enabled = false;
		}
		public bool Play(){
			// or reset
			if (isAlive) return false;
			
			isAlive = true;
			renderer.enabled = true;
			
			return true;
			
		}
		protected override void OnFixedUpdate () {
			if (!isAlive) return;
			
			timeAlive += Time.deltaTime;
			if (timeAlive > lifespanSeconds) 
			{
				Kill();
			}
		}
		protected override void OnLateUpdate () {
			if (!isAlive) return;
			
			speed += accelerationRate * Time.deltaTime;
			speed = Mathf.Min(maxSpeed, speed);
			transform.position = transform.position + vector * speed * Time.deltaTime;
			
			if (rotateYOnly)
			{
				transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
			}
			else
			{
				
				transform.LookAt(target.position);
			}
			if (flipY)
				transform.eulerAngles += new Vector3(0, 180, 0);
			
				
		}
	}
}
