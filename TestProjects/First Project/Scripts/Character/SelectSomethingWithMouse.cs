using System;
using UnityEngine;
using PlayerAssets.Game;
using PlayerAssets.Interface;
using UnityStandardAssets.Cameras;
using UnityEngine.EventSystems;
using TestProject.Cameras;

namespace TestProject
{
    public class SelectSomethingWithMouse : MonoBehaviour
    {
		public float deltaSpeed = 5f;
		public bool reverse;
        public float surfaceOffset = 1.5f;
		public Transform camDestination;
		
        public GameObject setTargetOn;
		public LayerMask activeLayer;

		private SmoothRotate srotate;
		private GameObject thisChild;
		
        // Update is called once per frame
		private RaycastHit hit;
		private Ray ray;
		
		private Vector3 lastMousePosition = Vector3.zero;
		
		public void Cancel(){
			thisChild.SetActive(false);
			PlayerInterface.looking = false;
			srotate.Cancel();
		}
		
		public void Look()
		{
			PlayerInterface.looking = true;
			thisChild.SetActive(true);
		}
		
		private void Start() {
			thisChild = this.transform.GetChild(0).gameObject;
			thisChild.SetActive(false);
			srotate = setTargetOn.GetComponent<SmoothRotate>();
		}
		
		
        private void Update()
        {
			if (PlayerInterface.locked ) 
			{
				Cancel();
				return;
			}
			if (Input.GetKeyDown("space"))
				Cancel();
			if (Input.GetButtonDown("mouse 2")) // down once
			{
				Vector3 currentPosition = Input.mousePosition;
				lastMousePosition = currentPosition;
			}
			else if (!Input.GetButton("mouse 2")) return; // no button at all
			else // holding
			{
				Vector3 currentPosition = Input.mousePosition;
				Vector3 delta = currentPosition-lastMousePosition;
				if (delta.magnitude > deltaSpeed) 
					lastMousePosition += delta.normalized * deltaSpeed;
				else
					lastMousePosition = currentPosition;
			}
			// disabled by disabled parent
			if (setTargetOn == null || !setTargetOn.activeInHierarchy)
				return;

			// checking for intersecting layer mask
	        ray = Camera.main.ScreenPointToRay(lastMousePosition );
	        if (!Physics.Raycast(ray, out hit, Mathf.Infinity, activeLayer.value))
	        {
	            return;
	        }
			
			Look();
	        this.transform.position = hit.point + hit.normal*surfaceOffset;

			// sends a message to an object, which contains a component with public function "settarget"
			if (reverse)
			{
				srotate.lookAtTarget = camDestination;
				srotate.frameTarget = this.transform;
				return;
			}
			
	        srotate.lookAtTarget =  this.transform;
			srotate.frameTarget = camDestination;

        }
    }
}
