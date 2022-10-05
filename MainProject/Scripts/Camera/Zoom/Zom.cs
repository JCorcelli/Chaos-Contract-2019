using UnityEngine;
using System.Collections;

namespace CameraSystem
{
	[RequireComponent (typeof(ZoomToFitFrom))]
	public class Zom : UpdateBehaviour {
		public bool sliding = true;
		public bool zooming = true;
		
		protected ZoomToFitFrom zom;
		protected RegionCheckUtil rt;
		public RectTransform region;
		
		protected Transform arm;
		
		// Use this for initialization
		protected Vector3 armoffset = new Vector3 ();
		protected Vector3 slideoffset = new Vector3 ();
		protected Vector3 zoomoffset = new Vector3 ();
		protected void Awake () {
			
			
			arm = transform.GetChild(0);
			zom = GetComponent<ZoomToFitFrom>();
			zom.transform = arm;
			
			rt = new RegionCheckUtil();
			rt.cam = Camera.main;
			rt.region = region;
			rt.target = zom.targetA;
			
		}
		protected void Start() {
			tLastPos = zom.targetA.position;
		}
		
		// Update is called once per frame
		protected Vector3 targetMotion;
		protected override void OnLateUpdate () {
			// reset variables?
			arm.localPosition = Vector3.zero;
			
			targetMotion = zom.targetA.position - tLastPos;
			
			if (sliding) 
			{
				DoSlide();
			}
			
			
			if (zooming) {
				DoZoom();
			
			}
			
		
			
			tLastPos= zom.targetA.position;
				
		}
		protected Vector3 tLastPos;
			
		protected void DoSlide(){
			
			armoffset *= 0.99f;
			
			arm.position += armoffset;
			
			
			bool safe = rt.Contains(zom.targetA.position + targetMotion);
			
			if (!safe)
			{
				
				
				armoffset += targetMotion ;
				arm.position += targetMotion;
				

			}
			
		}
		protected void DoZoom(){
			
			
			
			arm.localPosition += zoomoffset;
			
			
			bool hasPoint = zom.SetZoomPoint(new Vector3[]{zom.targetA.position, zom.targetA.position + targetMotion}); // finds. true if anything.
			if (hasPoint) 
			{
				
				
				Vector3 zoom = zom.Zoom(1f, false); // finds target location
				
				Vector3 izoom = arm.InverseTransformPoint(zoom);
				
				float z = (izoom.z);
				if (z < 0) 
				{
					// I want to pan the camera to fit this
					zoomoffset += Vector3.forward * z;
					arm.localPosition += Vector3.forward  * z;
					StopCoroutine("DDebounce");
					StartCoroutine("DDebounce");
					
				}
				
			}
			
			if (!debounce)
				zoomoffset *= .99f;
			
		}
		protected bool debounce= false;
		protected IEnumerator DDebounce() {
			debounce = true;
			yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(.5f));
			debounce = false;
			
		}
		
	}
}