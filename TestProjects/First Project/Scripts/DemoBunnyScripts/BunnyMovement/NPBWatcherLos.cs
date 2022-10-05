using UnityEngine;
using System.Collections;

namespace PlayerAssets.Game
{
	public class NPBWatcherLos : MonoBehaviour {
		private PlayerStatus status;
		private GameObject player;
		
		//private GameObject p_Status;
		private AIBunnyControl target;
		public bool isTriggering = false;
		
		
		
		const float k_Half = 0.5f;
		
		// Use this for initialization
		private void Awake () {
			
			player = GameObject.FindWithTag("Player"); // collider & body
			
			GameObject gobtemp = transform.parent.gameObject;
			target = gobtemp.GetComponent<AIBunnyControl>();
			
		}
		
		// Update is called once per frame
		void Update () {
			
			if (isTriggering) 
			{
				// visual effect
				// raycast 
				Vector3 sightAngle = player.transform.position - transform.position;
				Ray sightRay = new Ray(transform.position, sightAngle);
				
				float sightRayLength = 100.0f;
				RaycastHit hit;
				if (Physics.Raycast(sightRay, out hit, sightRayLength))
				{
					if (hit.collider.tag == "Player")
					{
						target.SetTarget(player.transform);
						
					}
					else
					{
						target.SetTarget(null);
						
					}
				}				
				
				
			}
			
		}
		void OnTriggerEnter (Collider hit) {
			// note: not necessary -> if (ob.GetComponent<type>() != null)
			if (hit.tag == "Player")
			{
				isTriggering = true;
			}
		}
		void OnTriggerExit (Collider hit) {
		
			if (hit.tag == "Player")
			{
				isTriggering = false;
				target.SetTarget(null);
			}
		}
	}
}