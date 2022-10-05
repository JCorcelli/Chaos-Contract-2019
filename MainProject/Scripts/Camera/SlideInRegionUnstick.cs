using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using SelectionSystem;


namespace CameraSystem {
	public class SlideInRegionUnstick : UpdateBehaviour {
		// alternative to SlideTowardsMousePosition
		
		// Update is called once per frame
		public float zoomSpeed = 1f;
		
		public float releaseSpeed = 1f;
		public float releaseDelay = 3.5f;
		
		public Transform targetA {
			
			get{return CameraHolder.instance.targetA;}
		}
		public Transform targetB {
			get{return CameraHolder.instance.targetB;}
		}
		public SphereCollider distanceLimiter;
        public string _sphereColliderName;
        public string _sphereColliderTag;
		protected RegionCheck rt;
		
		protected bool debounce = false;
		public bool targetACentered = false;
		protected void Start () {
			if (distanceLimiter == null && _sphereColliderName != "")
			{
				distanceLimiter = gameObject.FindNameXTag(_sphereColliderName, _sphereColliderTag).GetComponent<SphereCollider>();
			}
			
			
			rt = GetComponent<RegionCheck>();
			instaCam = rt.cam;
			
			targetPrevPosition = targetA.position;
			
		}
		
		
		protected Camera instaCam;
		
		
		protected Vector3 targetPrevPosition;
		
		
		Vector3 targetMotion;
		
		protected override void OnDisable() {
			base.OnDisable();
			StopAllCoroutines();
		}
		protected override void OnLateUpdate () {
			
			
			if (Input.GetButtonUp("mouse 1"))
				StartCoroutine("DDebounce");
			
			else if (Input.GetButtonDown("mouse 1"))
			{
				StopCoroutine("DDebounce");
				debounce = false;
			}
			
			
			bool pressed =  Input.GetButton("mouse 1");
			targetMotion = targetA.position - targetPrevPosition;
			targetPrevPosition = targetA.position;
			
			if ( !SelectGlobal.locked && !SelectGlobal.uiSelect &&  pressed && rt.GetSafeRegion(instaCam) )
			{
			
				
				
				transform.position = Vector3.MoveTowards(transform.position, targetB.position, zoomSpeed * Time.unscaledDeltaTime );
				
				
			}
			else if (!pressed && targetACentered && !debounce)
			{
					
				transform.position = Vector3.MoveTowards(transform.position, targetA.position, releaseSpeed * Time.unscaledDeltaTime) ;
					
			}
			
			
			StayOnCamera(); 
			StayInZone();
			
		}
		protected void StayOnCamera(){
			
			// trying not to move the target off screen
			
			if ( !rt.GetSafeRegion(instaCam))
			{
				
				
					
				transform.position = Vector3.MoveTowards(transform.position, targetA.position, .1f + targetMotion.magnitude ) ; // can I get avatar speed?
					
				
			}
			

		}
		
		
		protected IEnumerator DDebounce() {
			if (debounce) yield break;
			debounce = true;
			yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(releaseDelay));
			debounce = false;
			
		}
		
		protected void StayInZone() {
			// trying to stay close enough to target
			
			float d = Vector3.Distance(targetA.position, transform.position);
			
			float scaledDistanceLimiter = distanceLimiter.radius * distanceLimiter.transform.lossyScale.y;
			
			if (  d > scaledDistanceLimiter  )
			{
				
				transform.position = Vector3.MoveTowards(transform.position, targetA.position, d - scaledDistanceLimiter); // extra distance after subtracting the max
			}
		}
		
		
		
	}
}