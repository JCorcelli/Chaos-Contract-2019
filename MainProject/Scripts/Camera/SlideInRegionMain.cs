using UnityEngine;
using System.Collections;
using SelectionSystem;


namespace CameraSystem {
	public class SlideInRegionMain : UpdateBehaviour {
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
		void Start () {
			if (distanceLimiter == null && _sphereColliderName != "")
			{
				distanceLimiter = gameObject.FindNameXTag(_sphereColliderName, _sphereColliderTag).GetComponent<SphereCollider>();
			}
			
			rt = GetComponent<RegionCheck>();
			instaCam = rt.cam;
			
			targetPrevPosition = targetA.position;
			oldPosition = Camera.main.transform.position;
		}
		
		
		protected Camera instaCam;
		
		
		protected Vector3 targetPrevPosition;
		
		
		protected Vector3 oldPosition;
		
		protected override void OnLateUpdate () {
			
			
			if (targetA == null || targetB == null) return;
			
			
			if (Input.GetButtonUp("mouse 1"))
				StartCoroutine("DDebounce");
			
			else if (Input.GetButtonDown("mouse 1"))
			{
				StopCoroutine("DDebounce");
				debounce = false;
			}
			
			
			bool pressed =  Input.GetButton("mouse 1");
			Vector3 targetMotion = targetA.position - targetPrevPosition;
			if ( !bounced && !SelectGlobal.locked && !SelectGlobal.uiSelect &&  pressed && rt.GetSafe(instaCam) )
			{
			
				
				
				transform.position = Vector3.MoveTowards(transform.position, targetB.position, zoomSpeed * Time.unscaledDeltaTime + targetMotion.magnitude);
				
				
			}
			else if (!pressed && targetACentered && !debounce)
			{
					
				transform.position = Vector3.MoveTowards(transform.position, targetA.position, releaseSpeed * Time.unscaledDeltaTime) ;
					
			}
			
			StayOnCamera();
			StayInZone();
			
			
			targetPrevPosition = targetA.position;
		}
		protected bool bounced = false;
		protected void StayOnCamera(){
			
			
			Vector3 targetMotion = targetA.position - targetPrevPosition;
			
			Vector3 targetposition = rt.target.position;
			rt.cam = Camera.main;
			if ( !rt.Contains(targetposition + targetMotion) || !rt.Contains(targetposition))
			{
				transform.position = Vector3.MoveTowards(transform.position, targetA.position, releaseSpeed * Time.unscaledDeltaTime + targetMotion.magnitude) ;
				
				//CameraHolder.instance.smoothTransform.moveTime = .1f;
				//CameraHolder.instance.smoothTransform.lookTime = .2f;
				
				bounced = true;
			}
			else
			{
				bounced = false;
				//CameraHolder.instance.smoothTransform.moveTime = .5f;
				//CameraHolder.instance.smoothTransform.lookTime = .4f;
			}
			

		}
		
		
		protected IEnumerator DDebounce() {
			if (debounce) yield break;
			debounce = true;
			yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(releaseDelay));
			debounce = false;
			
		}
		
		protected void StayInZone() {
			float d = Vector3.Distance(targetA.position, transform.position);
			
			float scaledDistanceLimiter = distanceLimiter.radius * distanceLimiter.transform.lossyScale.y;
			
			if (  d > scaledDistanceLimiter  )
			{
				
				transform.position = Vector3.MoveTowards(transform.position, targetA.position, d - scaledDistanceLimiter); // extra distance after subtracting the max
			}
		}
		
	}
}