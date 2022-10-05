using UnityEngine;
using System.Collections;
using TestProject;

namespace PlayerAssets.Game
{
	public class BunnyPetting : MonoBehaviour {

		// Use this for initialization
		
		public LayerMask activeLayer = 0; // best for compound collider
		public float surfaceOffset = .05f;
		
		private GameObject thisChild;
		
		private Ray ray;
		private RaycastHit hit;
		private bool pressing = false;
		
		private BunnyPettingTarget target;
		
		public void CancelAction() {
			target.enabled = false;
			this.enabled = false;
		}
		public void PetAction() {
			target.enabled = true;
			this.enabled = true;
		}
		void Awake() {
			thisChild = transform.GetChild(0).gameObject;
			thisChild.SetActive(false);
			
			target = GameObject.FindObjectOfType(typeof(BunnyPettingTarget)) as BunnyPettingTarget;
		}
		
		void Update () {
			
			pressing = Input.GetButton("mouse 1");
			if (!pressing) 
			{
				thisChild.SetActive(false);
				return;
			}
			
			
			// checking for intersecting layer mask
			
	        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
	        if (!Physics.Raycast(ray, out hit, Mathf.Infinity, activeLayer.value))
	        {
				thisChild.SetActive(false);
	            return;
	        }
			thisChild.SetActive(true);
	        transform.position = hit.point + hit.normal*surfaceOffset;
			
		}
	}
}