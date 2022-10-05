using UnityEngine;
using System.Collections;

namespace PlayerAssets.Game
{
	[RequireComponent(typeof (SphereCollider))]
	public class WaypointMarcher : MonoBehaviour {
		public GameObject setTargetOn;
		public WaypointDropper m_WpDropper;
		private float stopDistance = 1.5f; // using the sphere I can visualise easier
		private int step = 0;

		// Use this for initialization
		void Start () {
			stopDistance = GetComponent<SphereCollider>().radius;


			transform.position = setTargetOn.transform.position;
		}

		void OnEnable(){
			if (setTargetOn != null && setTargetOn.activeInHierarchy )
				setTargetOn.SendMessage("SetTarget", transform);
		}

		void Update () {

			// am I currently running?
			if (m_WpDropper.maxIndex < 1)
				return;

			// am I close enough to update the target?
			Transform other = setTargetOn.transform;
			if ((transform.position - other.position).magnitude < stopDistance)
				Next ();


		}
		// seamlessly cirles, needs a loop checkbox
		void Next () {

			// am I moving?
			step++;
			if (step >= m_WpDropper.maxIndex)
				step = 0;

			transform.position = m_WpDropper.GetWaypoint (step);

			// something else should read he
		}
		void Prev () {
			
			// am I moving?
			step--;

			if (step < 0)
				step = m_WpDropper.maxIndex-1;
			
			transform.position = m_WpDropper.GetWaypoint (step);
			
			// something else should read he
		}
	}
}