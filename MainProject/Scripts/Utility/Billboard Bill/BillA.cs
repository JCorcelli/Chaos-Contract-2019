using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Utility
{
	public class BillA : UpdateBehaviour {
		
		// physics/transform properties
		
		protected Transform lookTarget;
		public float speed;
		public Vector3 startVector = new Vector3();
		protected Vector3 inVector;
		public float startSpeed = 0f;
		public float maxSpeed = 5f;
		protected float accelerationRate;
		protected Vector3 vector;
		public Vector3 acceleration = new Vector3();
		public float gravityMultiplier = -0.1f;
		
		// sprite-like qualities
		public float lifespanSeconds = 5f;
		public bool canDie = true;
		public bool rotateYOnly = true;
		
		protected float timeAlive = 0f;
		protected bool isAlive = false;
		
		
		protected Transform transformParent;

		// bill properties
		public BillAParent parent;
		public Transform anchor;
		public string anchorName, anchorTag = "";
		
		public bool localSpace = false;
		public bool isAutonomous = true;
		public string text = "";
		
		// other
		protected GameObject child;
		
		protected void Awake () {
			
			transformParent = transform.parent;
			
			child = transform.GetChild(0).gameObject;
			CalcSpeed();
			// set accel rate
			
			
			lookTarget = Camera.main.transform;
		}
		
		
		protected void Start() {
			if (anchor == null && anchorTag != "")	
				anchor = gameObject.FindNameXTag(anchorName, anchorTag).transform;
			
			
			if (parent == null)
				parent = GetComponentInParent<BillAParent>();
		
			if (!isAlive)
				Kill();
		}
		
		public void Kill(){
			isAlive = false;
			child.SetActive(false);
			if (!localSpace)
			{
				transform.parent = transformParent;
			}
			
		}
		
		protected void CalcSpeed()
		{
			speed = startSpeed;
			
			// set accel vector
			
			if (anchor != null)
			{
				vector = anchor.TransformDirection(acceleration) + gravityMultiplier * Physics.gravity;
				inVector = anchor.TransformDirection(startVector);
			}
			else
			{
				vector = acceleration + gravityMultiplier * Physics.gravity;
				inVector = startVector;
			}
				
			accelerationRate = vector.magnitude;
		}
		
		public bool Play(){
			// or reset
			
			// I send it to the anchor
			if (anchor != null)
			{
				transform.position = anchor.position;
				
				// I parent it to the anchor
				if (localSpace)
					transform.parent = anchor;
			}
			
			isAlive = true;
			timeAlive = 0f;
			child.SetActive(true);
			CalcSpeed();
			
			return true;
			
		}
		
		protected override void OnFixedUpdate () {
			if (!isAlive)
			{
				if (isAutonomous && parent.hasText) 
				{
					text = parent.Pop();
					Play();
				}
				return;
			}
			
			
			timeAlive += Time.deltaTime;
			if (timeAlive >= lifespanSeconds) 
			{
				Kill();
			}
		}
		protected override void OnLateUpdate () {
			if (!isAlive) return;
			
			speed += accelerationRate * Time.deltaTime;
			speed = Mathf.Min(maxSpeed, speed);
			
			if (localSpace)
				transform.localPosition = transform.localPosition + ((inVector + vector.normalized * speed) * Time.deltaTime);
				
			else
				transform.position = transform.position + ((inVector + vector.normalized * speed) * Time.deltaTime);
			
			
			
			if (rotateYOnly)
			{
				transform.LookAt(new Vector3(lookTarget.position.x, transform.position.y, lookTarget.position.z));
			}
			else
			{
				
				transform.LookAt(lookTarget.position);
			}
			
				
		}
	}
}
