using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PlayerAssets.Interface
{

	public class AStarWMControl : MonoBehaviour {
		public Transform Startpoint;
		public Transform StartpointDestination;

		public Transform Endpoint;
		public Transform EndpointDestination;
		public bool activate = false;
		
		private List<Vector3> path = new List<Vector3>();
		public int hasPath = 0;
		void Update () {
			if (!activate)
				return;
			activate = false;
			Activate ();

		}

		public void Activate () {
			Startpoint.position = StartpointDestination.position;
			Endpoint.position = EndpointDestination.position;

			gameObject.SendMessage ("FindPath"); // start, end

		}

		public List<Vector3> GetPath (){
			return path;
		}
		public void SetPath (Vector3 newPoint) {
			path.Add ( newPoint );
		}


		public void SuccessPath (int result) {
			hasPath = result;
		}
		public void ClearPath () {
			path.Clear ();
		}

	}


}