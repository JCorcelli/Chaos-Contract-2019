using UnityEngine;
using System.Collections;
using SelectionSystem;


namespace CameraSystem {
	public class SlideInRegion : UpdateBehaviour {
		// alternative to SlideTowardsMousePosition
		
		// Update is called once per frame
		public float zoomSpeed = 1f;
		
		public float releaseSpeed = 1f;
		
		
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
		public float releaseDelay = 3.5f;
		void Start () {
			if (distanceLimiter == null && _sphereColliderName != "")
			{
				distanceLimiter = gameObject.FindNameXTag(_sphereColliderName, _sphereColliderTag).GetComponent<SphereCollider>();
			}
			
			
			rt = GetComponent<RegionCheck>();
			instaCam = rt.cam;
		}
		
		protected Camera instaCam;
		protected float lastMove = 0f;
		protected override void OnUpdate () {
			
			
			if (targetA == null || targetB == null) return;
			/*
			Vector2 mousePos = Input.mousePosition;
			mousePos.x = mousePos.x / Screen.width * 2 - 1;
			mousePos.x = Mathf.Clamp(mousePos.x, -1, 1);
			mousePos.y = mousePos.y / Screen.height * 2 - 1;
			mousePos.y = Mathf.Clamp(mousePos.y, -1, 1); // + ( Camera.main.transform.right * mousePos.x * maxDifferencex);
			*/
			
			if (Input.GetButtonUp("mouse 1"))
				StartCoroutine("DDebounce");
			
			else if (Input.GetButtonDown("mouse 1"))
			{
				StopCoroutine("DDebounce");
				debounce = false;
			}
			
			
			
			if (!rt.GetSafe(instaCam)) // I am oob on instant camera, so I'm about to go oob on real camera.
				transform.position = Vector3.MoveTowards(transform.position, targetA.position, zoomSpeed * Time.unscaledDeltaTime) ;
			else if (!rt.GetSafe(Camera.main)) // I am oob on view camera
			{
				transform.position = Vector3.MoveTowards(transform.position, targetA.position, releaseSpeed * Time.unscaledDeltaTime) ;
				
			}
				
			else if (SelectGlobal.locked || !Input.GetButton("mouse 1"))
			{
					
				if (targetACentered && !debounce)
					transform.position = Vector3.MoveTowards(transform.position, targetA.position, releaseSpeed * Time.unscaledDeltaTime) ;
					
			}
			else if (!SelectGlobal.uiSelect)
			{
			
				
				transform.position = Vector3.MoveTowards(transform.position, targetB.position, zoomSpeed * Time.unscaledDeltaTime);
			}
			StayInZone();
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