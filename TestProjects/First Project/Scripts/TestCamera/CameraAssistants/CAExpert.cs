using UnityEngine;
using System.Collections;

namespace TestProject. Cameras
{
	
	public class CAExpert : CARelease {
		// Has implementations of numerous components
		// Recognizing distance to colliders, visibility, and center
	
		protected  GetFidelityToFront	gff ;
		protected  SeeTarget			st  ;	
		protected  PredictCollision		pcol;
		protected 	ZoomToFitFrom		zom;
	
		protected override void Start () {
			base.Start();
			
			// IsAtCenter()
			gff  = gameObject.GetComponent<GetFidelityToFront>();
			
			// IsVisible()
			st   = gameObject.GetComponent<SeeTarget>();
			
			// Proximity()
			pcol = gameObject.GetComponent<PredictCollision>();
			
			// Angle()
			zom = gameObject.GetComponent<ZoomToFitFrom>();
			
			enabled = false;
		}
		
		public bool IsInitialized {
			// consider 
			get{
			if (gff != null 
				&& 	st 	!= null 
				&& 	pcol!= null
				&& 	zom	!= null) 
								return true;
			else 				return false;
			}
			set{}
			
		}
		public bool Init(bool activeAll) {
			// Sets new component.enabled = false;
			
			// IsAtCenter()
			if (gff == null)
			{
				gff  = gameObject.AddComponent<GetFidelityToFront>();
				gff.enabled = activeAll;
			}
			
			// IsVisible()
			if (st == null)
			{
				st   = gameObject.AddComponent<SeeTarget>();
				st.enabled = activeAll;
			}
			
			// Proximity()
			if (pcol == null)
			{
				pcol = gameObject.AddComponent<PredictCollision>();
				pcol.enabled = activeAll;
			}
			// Angle()
			if (zom == null)
			{
				zom = gameObject.AddComponent<ZoomToFitFrom>();
				zom.enabled = activeAll;
			}
			
			return IsInitialized;
			
		}
		// SELF
		public bool CameraExists () { return CameraHolder.instance != null; }
		public Vector3 CameraPosition () { return CameraHolder.instance.transform.position; }
		
		public Vector3 diffCamera () {
			if ( CameraExists ())
				return CameraPosition () - transform.position;
			else
				return new Vector3();
			
		}
		public Vector3 diffCamera (Vector3 point) {
			if ( CameraExists ())
				return CameraPosition () - point;
			else
				return new Vector3();
			
		}
		
		// BASED ON CAMERA REGIONS
		// vector3 . IsAtCenter (region)
		public bool Contains (Vector3 point) {
			// is looking at.. according to gff
			return gff.Contains (point);
		}
		public Vector3 WorldToCameraViewport(Vector3 point) { return WorldToCameraViewport(point); }
		
		public bool IsAtRegion (Vector3 point, RectTransform region) {
			// is looking at.. according to gff
			return gff.Contains (point, region);
		}
		public bool IsAtCenter () {
			// is looking at.. according to gff
			if (gff != null && gff.enabled)
				return gff.atCenter;
			else return false;
		}
		
		// BASED ON RAYS
		public bool IsVisible (Vector3 point) {
			
			return st.GetVisible(point);
			
		}
		public bool IsVisible (Vector3 point, string tname) {
			
			return st.GetVisible(point, tname);
			
		}
		public bool IsVisible () {
			
			if (st != null && st.enabled)
				return st.visible;
			else return false;
			
		}
		
		// BASED ON MOVEMENT
		public bool Proximity () {
			
			if (pcol != null && pcol.enabled)
				return pcol.likelyCollision;
			else return false;
		}
		public float GetProximity (Vector3 point) {
			// returns float based on speed last frame, magnitude of point if invalid, 0 if near
			if (pcol != null && pcol.enabled)
				return pcol.ComparePoint(point);
			else return - 1;
		}
		
		// DECISION: BASED ON ANGLE AND POSITION
		/* viewing 3D world space as though 2D, referred to as viewport space
			
			
		
		
		*/
		
		
		public virtual Vector3 GetP() {
			return zom.GetPerpendicularMid();
		}
		public virtual Ray GetPRay() {
			return zom.GetPerpendicularRayMid();
		}
		
		
		public Vector3 GetMiddle() {
			return  zom.GetMiddleOfTargets();
		}
		public Vector3 GetMiddle2D() {
			return  zom.GetMiddleOfTargets(true);
		}
		
		// ZOOM TO FIT
		/// <summary>
		///
		///	zooming backwards from any facing (default current facing)
		///	so all objects within range are visible
		/// </summary>
		public Vector3 GetZoom (float multi = 1f) {
			// needs damping
			if (zom != null && zom.enabled)
			{
				// zoom position in 3d space which fits both targets of zom
				
				return zom.Zoom(multi);
			}
			else
				return transform.position;
		}
		/// <summary>
		///
		/// sets A and B of zom. 
		/// A and B can be swapped during runtime with KeepALeft = (default) true
		/// setting KeepALeft = false will reset A and B so check the variable swapped ahead of time.
		/// </summary>
		public void SetZoomTargets (Transform targetA, Transform targetB) {
			zom.targetA = targetA;
			zom.targetB = targetB;
		}
		public Vector3 GetClosestPointOnLineSegment(Vector3 vA, Vector3 vB, Vector3 vPoint) {
			
			return zom.GetClosestPointOnLineSegment(vA, vB, vPoint);
		}
		
		
	}
}