using UnityEngine;
using System.Collections;


namespace CameraSystem
{
	public class SmoothTransformZom : SmoothTransformPhysic
	
	{

		
		protected void Start(){
			InitZom();
		}
		
		protected override void OnUpdate() {
			// if arm.localPosition = Vector3.zero; is in here it makes the 3D pointer screw up when it's zoomed out
			
		}
		protected override void OnLateUpdate()
		{
			arm.localPosition = Vector3.zero;
			
			// unless I override it this way, it won't use the new smoothlook, alternatively I could use a delegate 
			
			// Early out if we don't have a target
			if (!target)
				return;
			
			Retarget(); // checks if target changed, and shifts the speed
			
			// I turn the camera before moving at all
			if (m_look)
				SmoothLook();
			
			
			if (m_move)
			{
				if (moveTime > 0f)
				{
					SmoothMove();
					
					Zom();
					TouchGround(arm);
				}
				else
				{
					transform.position = target.position;
					TouchGround(transform);
				}
			}
			
			
			
		}

		public bool sliding = true;
		public bool zooming = true;
		protected ZoomToFitFrom zom;
		protected RegionCheckUtil rt;
		public RectTransform region;
		protected Transform arm;
		
		protected Vector3 targetMotion;
		protected Vector3 tLastPos;
		
		protected void InitZom() {
			// init classes
			arm = transform.GetChild(0);
			zom = GetComponent<ZoomToFitFrom>();
			zom.transform = arm;
			tLastPos = zom.targetA.position;
			
			// checking a square region
			rt = new RegionCheckUtil();
			rt.cam = Camera.main;
			rt.region = region;
			rt.target = zom.targetA;
		}
		protected Vector3 zoomoffset = new Vector3 ();
		
		protected void Zom() {
			
			targetMotion = (zom.targetA.position - tLastPos) * Time.deltaTime ;
			
			
			if (sliding) 
			{
				DoSlide();
			}
			
			
			if (zooming) {
				DoZoom();
			
			}
				
			tLastPos= zom.targetA.position;
		}
		protected void DoSlide(){
			
			
			
			
			/// even if this movement is with the camera I move closer.
				
			bool safe = rt.Contains(zom.targetA.position );
			if (safe) return;
			transform.position = Vector3.MoveTowards(transform.position,  target.position, targetMotion.magnitude);
				

			
		}
		
		protected float zoomBuoy = 0f;
		public float zoomRangeMax = 2f;
		protected void DoZoom(){
			
			
			
			arm.localPosition += zoomoffset;
			
			//Vector3 nextFrameLoc = zom.targetA.position + targetMotion - direction_velocity * Time.unscaledDeltaTime;
			bool hasPoint = zom.SetZoomPoint(new Vector3[]{zom.targetA.position, zom.targetB.position}); // finds. true if anything.
			if (hasPoint) 
			{
				
				
				Vector3 zoom = zom.Zoom(1f, false); // finds target location
				
				Vector3 izoom = arm.InverseTransformPoint(zoom);
				
				float z = (izoom.z);
				if (z < 0) 
				{
					// I want to pan the camera to fit this
					zoomBuoy = Mathf.Lerp(zoomBuoy, z, (targetMotion.magnitude + 0.05f ) );
				
					arm.localPosition += Vector3.forward * zoomBuoy;
						
					StopCoroutine("DDebounce");
					StartCoroutine("DDebounce");
					
				}
				else
					zoomBuoy = Mathf.Lerp(zoomBuoy, 0f, 3f * Time.unscaledDeltaTime);
					
			}
			
			if (!debounce)
			{
				zoomoffset *= .99f;
			}
			else 
			{
				
				zoomoffset += Vector3.forward * zoomBuoy;
				
				if (Mathf.Abs(arm.localPosition.z) >= zoomRangeMax)
				{
					Vector3 pos = arm.localPosition;
					pos.z = -zoomRangeMax;
					zoomoffset = pos;
				}
			}
			arm.localPosition = zoomoffset;
			
			
		}
		protected bool debounce= false;
		protected IEnumerator DDebounce() {
			debounce = true;
			yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(.5f));
			
			zoomBuoy = 0f;
			yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(2f));
			debounce = false;
			
			
		}
		
		
		
		
	}
}