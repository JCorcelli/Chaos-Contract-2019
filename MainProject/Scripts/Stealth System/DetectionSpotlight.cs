using UnityEngine;
using System.Collections;

namespace StealthSystem
{
	
	public class DetectionSpotlight : SeeTarget {

		protected float viewConeMinimum;
		new protected Light light;
		
		protected override void Start () {
			light = GetComponent<Light>();
			FindTargetSelf();
			CalculateView();
		}
		
		protected void CalculateView() {
			float viewAngle = light.spotAngle; // there is some sort of overshoot
			viewConeMinimum =  1 - (viewAngle / 180f);
			
#if UNITY_EDITOR
			// helper to visualise the ground check ray in the scene view
			Quaternion q = Quaternion.identity;
			q.SetLookRotation(transform.up);
			
			float halfAngle = viewAngle * Mathf.Deg2Rad/2f;
			Vector3 angle1 = new Vector3(Mathf.Cos(halfAngle),-0, Mathf.Sin(halfAngle));
			Vector3 angle2 = new Vector3(Mathf.Cos(-halfAngle),-0, Mathf.Sin(-halfAngle));
			
			Vector3 vec = q * angle1 * 15f;
			Debug.DrawLine(transform.position, transform.position+ vec);
			vec = q * angle2 * 15f;
			Debug.DrawLine(transform.position, transform.position+ vec);
#endif
			
		}
		protected bool GetWithinAngle(Vector3 t) {
			Vector3 heading = t - transform.position;
			float fidelity = Vector3.Dot(heading.normalized, transform.forward);
			return ( viewConeMinimum < fidelity );
		}
		protected override void OnUpdate () {
		
			// ray = thisposition to playerposition;
			if (target == null) return;
			
			CalculateView();
			if (!GetWithinAngle(target.position)) { visible = false; return; }
			visible = GetVisible(target.position);
			
		}
	}
}