using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace PlayerAssets.Interface
{
	[RequireComponent(typeof (SphereCollider))]
	public class AStarWMInterface : MonoBehaviour 
	{
		public AStarWMControl control;
		public float surfaceOffset = .1f;
		public LayerMask activeLayer = 0;
		public GameObject thisChild;

		private List<Vector3> agentPath;
		private int step = 0;

		public GameObject setTargetOn;

		public bool loop = false;

		private bool stopped = true;
		// Update is called once per frame
		private float stopDistance = 1.5f; // I use the sphere to determine the stop distance
		private Transform other;
		private Ray ray;
		private RaycastHit hit;
		private bool blocked = false;
		
		void Start () {
			
			stopDistance = GetComponent<SphereCollider>().radius;
			GetComponent<SphereCollider>().enabled = false;
			agentPath = control.GetPath ();
			
		}

		private bool SetEndpoint(){
			
			ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			
			if (!Physics.Raycast (ray, out hit, Mathf.Infinity, activeLayer.value)) {
				return false;
			}
			control.EndpointDestination.position = hit.point + hit.normal * surfaceOffset;

			return true;
		}
		void Update () {
			// always lets you change you destination
			// checks if blocked
			if (Input.GetButtonDown("mouse 1") && EventSystem.current.IsPointerOverGameObject()) blocked = true;
			
			
			// skips if true
			if (!(blocked || PlayerInterface.locked || EventSystem.current.IsPointerOverGameObject() ))
			{
				if (Input.GetButtonDown ("mouse 1")) stopped = true;
				else if (Input.GetButtonUp ("mouse 1")) {
					
					if (!SetEndpoint()) return;
					control.Activate ();
					if (control.hasPath == 1)
					{
						agentPath = control.GetPath ();
						
						thisChild.SetActive( true );
						step = 0; // defaults to next position
						stopped = false;
					}
				}
			}
			
			else if (Input.GetButtonUp("mouse 1")) blocked = false;
			// disabled by disabled parent
			
			if (stopped || setTargetOn == null || !setTargetOn.activeInHierarchy)
			{
				thisChild.SetActive( false );
				return;
			}
			


			other = setTargetOn.transform;
			transform.position = agentPath[step];
			if ((transform.position - other.position).magnitude < stopDistance)
				Next ();

			
			// sends a message to an object, which contains a component with public function "settarget"
			setTargetOn.SendMessage("SetTarget", transform);


		}	

		void Next () {

			// am I moving?
			step++;
			if (step >= agentPath.Count)
			{
				if (loop) step = 0;
				else
					stopped = true;
			}
			else
				transform.position = agentPath[step];

			// something else should read he
		}
		void Prev () {
			
			// am I moving?
			step--;

			if (step < 0)

			{
				if (loop) step = agentPath.Count-1;
				else
					stopped = true;
			}
			else
				transform.position = agentPath[step];
			
			// something else should read he
		}
	}
}
