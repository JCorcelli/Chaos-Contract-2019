using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Utility
{
	public class BillText : UpdateBehaviour {
		
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
		
		
		// bill properties
		public BillTextParent parent;
		public Transform anchor;
		public bool localSpace = false;
		public bool isAutonomous = true;
		
		// other
		protected GameObject child;
		protected TextMesh tMesh;
		
		protected void Awake () {
			
			tMesh = gameObject.GetComponentInChildren<TextMesh>();
			child = transform.GetChild(0).gameObject;
			
			CalcSpeed();
			// set accel rate
			
			
			lookTarget = Camera.main.transform;
		}
		// Update is called once per frame
		
		protected void Start() {
			if (parent == null)
				Reparent();
			if (!isAlive)
				Kill();
		}
		protected void Reparent() {
			if (parent == null)
				parent = GetComponentInParent<BillTextParent>();
		}
		protected void Retext() {
			
			
		}
		public void Kill(){
			isAlive = false;
			child.SetActive(false);
			if (!localSpace)
				transform.parent = parent.transform;
			
		}
		protected void CalcSpeed()
		{
			speed = startSpeed;
			
			// set accel vector
			vector = acceleration + gravityMultiplier * Physics.gravity;
			
			accelerationRate = vector.magnitude;
			if (anchor != null)
				inVector = anchor.TransformDirection(startVector);
			else
				inVector = startVector;
				
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
					tMesh.text = parent.Pop(); // taking first
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
				transform.localPosition = transform.localPosition + ((inVector + vector * speed) * Time.deltaTime);
				
			else
				transform.position = transform.position + ((inVector + vector * speed) * Time.deltaTime);
			
			
			
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
