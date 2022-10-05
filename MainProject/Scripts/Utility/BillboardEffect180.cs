using UnityEngine;
using System.Collections;

namespace Utility
{
	public class BillboardEffect180 : UpdateBehaviour {

		protected Transform target;
		// Update is called once per frame
		protected void Awake() {
			target = Camera.main.transform;
		}
		protected override void OnLateUpdate () {
			transform.forward = target.forward;
		}
	}
}