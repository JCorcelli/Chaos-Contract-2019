using UnityEngine;
using System.Collections;
using ActionSystem;

namespace StealthSystem
{
	
	public class DetectionSpotlightMessage : SeeTarget {

		protected float viewConeMinimum;
		new protected Light light;
		protected override void Start () {
			light = GetComponent<Light>();
			FindTargetSelf();
			CalculateView();
		}
		protected void CalculateView() {
			float viewAngle = light.spotAngle;
			viewConeMinimum =  1 - (viewAngle / 180f);

#if UNITY_EDITOR
			// helper to visualise the ground check ray in the scene view
			Quaternion q = Quaternion.identity;
			q.SetLookRotation(-transform.right);
			
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
		
		public bool visibleThisFrame = false;
		protected override void OnUpdate () {
		
			// ray = thisposition to playerposition;
			if (target == null) return;
			bool visibleLastFrame = visible; // true visibility
			
			CalculateView();
			if (!GetWithinAngle(target.position)) { visible = false; }
			else
				visibleThisFrame = GetVisible(target.position);
			if (visibleLastFrame && !visibleThisFrame)
				StartCoroutine("DelayHide");
			else if (!visibleLastFrame && visibleThisFrame)
				StartCoroutine("DelayUnhide");
			
			
		}
		public float delayHideTimer = 0.5f;
		protected bool delayHideRunning = false;
		protected IEnumerator DelayHide(){
			if (delayHideRunning) yield break;
			delayHideRunning = true;
			
			if (delayHideTimer > 0.01f)
				yield return new WaitForSeconds(delayHideTimer);
		
			if (!visibleThisFrame)
			{
				ActionEventDetail a = new ActionEventDetail();
				a.what = "lights--";
				a.receiver = target.gameObject;
				a.sender = gameObject;
				ActionManager.SubmitEffect(a);
				
				visible = false;
			}
			delayHideRunning = false;
			
		}
		
		public float delayUnhideTimer = 0.5f;
		protected bool delayUnhideRunning = false;
		protected IEnumerator DelayUnhide(){
			if (delayUnhideRunning) yield break;
			delayUnhideRunning = true;
			
			if (delayUnhideTimer > 0.01f)
				yield return new WaitForSeconds(delayUnhideTimer);
		
			if (visibleThisFrame)
			{
				StopCoroutine("DelayHide"); 
				delayHideRunning = false;
				// just in case someone's going in and out
				
				ActionEventDetail a = new ActionEventDetail();
				a.what = "lights++";
				a.receiver = target.gameObject;
				a.sender = gameObject;
				ActionManager.SubmitEffect(a);
				
				visible = true;
			}
			delayUnhideRunning = false;
		}
				
	}
}