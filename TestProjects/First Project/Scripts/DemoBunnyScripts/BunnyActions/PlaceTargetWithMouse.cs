using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PlayerAssets.Interface
{
    public class PlaceTargetWithMouse : MonoBehaviour
    {
        public float surfaceOffset = 1.5f;
        public GameObject setTargetOn;
		public LayerMask activeLayer;

        // Update is called once per frame
		
		private Ray ray;
		private RaycastHit hit;
		
		private GameObject thisChild;
		
		private bool blocked = false; // initial move raycast blocked
		
		void Awake()
		{
			thisChild = transform.GetChild(0).gameObject;
			thisChild.SetActive(false);
		}
		
		
		
        private void Update()
        {
			
			// disabled by disabled target
			if (setTargetOn == null || !setTargetOn.activeInHierarchy)
				return;
			
			// checks if blocked
			if (Input.GetButtonDown("mouse 1") && EventSystem.current.IsPointerOverGameObject()) blocked = true;
			
			if (Input.GetButtonUp("mouse 1")) blocked = false;
			
			
			// returns if interface is locked
			if (PlayerInterface.locked || !Input.GetButton("mouse 1")) 
			{
				thisChild.SetActive( false );
				return;
			}
			if (blocked)  return;
			
			// checking for intersecting layer mask
			
	        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
	        if (!Physics.Raycast(ray, out hit, Mathf.Infinity, activeLayer.value))
	        {
	            return;
	        }
			thisChild.SetActive( true );
	        transform.position = hit.point + hit.normal*surfaceOffset;

			// sends a message to an object, which contains a component with public function "settarget"
	        setTargetOn.SendMessage("SetTarget", transform);

        }
    }
}
