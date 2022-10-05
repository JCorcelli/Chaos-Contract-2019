using System;
using UnityEngine;
using PlayerAssets.Game;
using UnityStandardAssets.Cameras;
using UnityEngine.EventSystems;
using TestProject.Cameras;

namespace PlayerAssets.Interface
{
    public class SelectSomethingWithMouse : MonoBehaviour
    {
		public bool reverse;
        public float surfaceOffset = 1.5f;
		public Transform camDestination;
		
        public GameObject setTargetOn;
		public LayerMask activeLayer;

		private SmoothRotate srotate;
		private AutoCam acam;
		private GameObject thisChild;
		
        // Update is called once per frame
		private RaycastHit hit;
		private Ray ray;
		
		
		
		
		public void Cancel(){
			thisChild.SetActive(false);
			acam.enabled = true;
			srotate.enabled = false;
			PlayerInterface.looking = false;
		}
		
		public void Look()
		{
			PlayerInterface.looking = true;
			thisChild.SetActive(true);
			acam.enabled = false;
			srotate.enabled = true;	
		}
		
		private void Start() {
			thisChild = this.transform.GetChild(0).gameObject;
			thisChild.SetActive(false);
			srotate = setTargetOn.GetComponent<SmoothRotate>();
			acam = setTargetOn.GetComponent<AutoCam>();
		}
		
		
        private void Update()
        {
			if (PlayerInterface.locked ) 
			{
				Cancel();
				return;
			}
			if (Input.anyKeyDown && !(Input.GetButtonDown("mouse 1") || Input.GetButtonDown("mouse 2"))) 
				Cancel();
			if (!Input.GetButton("mouse 2")) return;
			// disabled by disabled parent
			if (setTargetOn == null || !setTargetOn.activeInHierarchy)
				return;

			// checking for intersecting layer mask
	        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
