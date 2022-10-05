using UnityEngine;
using System.Collections;

namespace TestProject
{
	[RequireComponent (typeof (SeeTarget))]
	public class HotSwapCamera : MonoBehaviour {
		private HotSwapCameraTarget target;
		private SeeTarget seeTarget;
		
		// Use this for initialization
		void Start () {
			target = transform.parent.gameObject.GetComponent<HotSwapCameraTarget>();
			seeTarget = GetComponent<SeeTarget>();
		}
		
		// Update is called once per frame
		void Update () {
			if (Input.GetButton("mouse 1")) return;
			if (seeTarget.visible)
			{
				target.AutoSwap(transform.GetChild(0));
				
			}
			else
			{
				target.Kill(transform.GetChild(0));
				
			}
				
		}
		
		void OnDisable() {
			target.Kill(transform.GetChild(0));
			
		}
		
	}
}