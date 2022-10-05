using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PlayerAssets.Game
{
	[RequireComponent(typeof (SphereCollider))]
	public class WaypointDropper : MonoBehaviour {

		private float distanceThreshold;
		public Transform other;
		public int maxIndex = 0;

		private List<Vector3> path = new List<Vector3>();

		// Use this for initialization
		void Start () {
			distanceThreshold = GetComponent<SphereCollider>().radius;
		}
		
		// Update is called once per frame
		void Update () {
			if ((transform.position - other.position).magnitude > distanceThreshold)
				AddWaypoint ();
		}
		void AddWaypoint () {
			// add a transform to stack
			path.Add (transform.position);
			maxIndex ++;
			other.position = transform.position; // has effect similar to this prev point =current point
		}

		public Vector3 GetWaypoint (int step) {
			if (step < maxIndex)
				return path [step];
			else
				return path [maxIndex - 1];
		}
	}

}