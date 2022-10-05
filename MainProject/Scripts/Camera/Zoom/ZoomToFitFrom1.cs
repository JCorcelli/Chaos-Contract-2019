using UnityEngine;
using System.Collections;

	
namespace CameraSystem
{
	public class ZoomToFitFrom1 : MonoBehaviour {

	
		new public Transform transform;
		public Transform targetA 	;
		public Transform targetB 	;
		public float extents 		= 2000f;
		public float minViewDistance = - 200f;
		public float maxViewDistance = 1000f;
		public bool orthoToCameraHeight 	 = false;
		// orthoWall could create a definite Max or Min
		// orthoFloor could create a definite Max or Min
		
		public float radius			= 3f;
		public LayerMask activeLayer = 1 << 12;
		
		[SerializeField] protected bool debug = false;		
		
		protected bool swapped 		 = false;
		protected Camera cam		;
		
		
		// used for the FOV operation
		protected Vector3 frontOrtho = Vector3.zero; // represents infinite plane in World coordinates
		
		protected Vector3 zoomPoint = Vector3.zero;
		
		protected Vector3 _perpendicularPoint  = Vector3.zero ;
		protected Ray _PRay = new Ray(); // GetPerpendicular ray
		protected Ray _DRay = new Ray(); // directional ray
		protected Ray _MPRay = new Ray(); // 
		public Vector3 outPosition = Vector3.one; // reused for storing a position between methods
		

		protected bool _keepALeft = true;

		/// <summary> 
		/// this allows switching A and B so that A is continuously camera left of the transform. 
		/// it's important to store which is which external from this class while KeepALeft is true.
		/// </summary>			
		public bool KeepALeft
		{
			get{ return _keepALeft; }
			set{ 
			_keepALeft = value; 
			if (swapped)
			{
				Swap(); // this will reset A to original position
			}
			
			}
		}		
		
		void Awake() {
			transform = GetComponent<Transform>();
			cam  = Camera.main;
		}
		
		/// <summary>
		/// a huge dividing line that intersects points A and B (the two targets)
		/// uses extents as the radius
		/// </summary>
		public Vector3[] TargetLine
		{
			get {
			Vector3 middle = GetMiddleOfTargets();
			
			Vector3 direction = targetA.position - targetB.position;
			
			return new Vector3[]{middle + direction * extents, middle - direction * extents};
			
			}
			private set{}
		}
		
		// true if any point between min and max
		public bool SetZoomPoint() {
			// no param = internal targets
			return SetZoomPoint(new Vector3[]{targetA.position, targetB.position});
		}
		public bool SetZoomPoint(Vector3[] targets) {
			SetPerpendicularRay() ;
			
			bool success = false;
			if (FitAll(targets))
			{
				success = true;
				zoomPoint = outPosition; // most recent out call will be zoom point
			}
			
			
			if (debug)
			{
				// a huge dividing line
				Vector3 middle = GetMiddleOfTargets();
				
				Vector3 direction = targetA.position - targetB.position;
				
				Debug.DrawLine(middle + direction * extents, middle - direction * extents, Color.black);
				
				// GetPerpendicular lines
				Debug.DrawLine(transform.position, _PRay.origin, Color.white);
				Debug.DrawLine(GetPerpendicularMid(), middle, Color.white);
				
				// the zoom
				if (success)
					Debug.DrawLine(frontOrtho, zoomPoint, Color.black);
				
				
				// a huge dividing line
				middle = frontOrtho;
				
				direction = transform.right;
				
				Debug.DrawLine(middle + direction * extents, middle - direction * extents, Color.red);
				
			}
			
			return success;
			
			
		}
		

		/// <summary> 
		/// switches A and B and then keeps the change in memory
		/// </summary>	
		
		public void Swap () {
			Transform t = targetA;
			targetA = targetB;
			targetB = t;
			swapped = !swapped;
			
		}
		
		public bool IsSwapped () { return swapped; }
		
		
		/// <summary> 
		/// returns the direction and magnitude in a world coordinate axis
		/// </summary>	
		
		public Vector3 Zoom(float multiplier = 1f, bool setzoom = true) {
			if (setzoom) SetZoomPoint();
			return _DRay.GetPoint((frontOrtho - zoomPoint).magnitude * multiplier);
		}
		
		
		
		/// <summary> 
		/// returns a point perpendicular to both targets in 3D coordinates from a ray whose origin is between the two.
		/// if initialized distance = current distance
		/// if _keepALeft then targetA will always be on our left side of transform
		/// </summary>	
		
		public Vector3 GetPerpendicular() {
			return _PRay.GetPoint((_perpendicularPoint -transform.position).magnitude);
		}
		
		/// <summary> 
		/// if initialized returns a point perpendicular to both targets in 3D coordinates from a ray on line intersecting both with a facing pointing directly at transform.position.
		
		/// </summary>	
		public Vector3 GetPerpendicular(float distance) {
			
			return _PRay.GetPoint(distance);
		}
		/// <summary> 
		/// if initialized returns a point perpendicular to both targets in 3D coordinates from a ray whose origin is between the two.
		
		/// </summary>	
		public Vector3 GetPerpendicularMid(float distance) {
			
			return _MPRay.GetPoint(distance);
		}
		public Vector3 GetPerpendicularMid() {
			
			return _perpendicularPoint;
		}
		
		/// <summary> 
		/// if initialized returns a ray at the point perpendicular to a line which intersects targetA and targetB
		/// </summary>	
		
		
		public Ray GetPerpendicularRay() {
			return _PRay;
		}
		/// <summary> 
		/// if initialized returns a ray at the point perpendicular to a line which intersects targetA and targetB, directly between the two.
		/// </summary>	
		
		public Ray GetPerpendicularRayMid() {
			return _MPRay;
		}
		
		/// <summary> 
		/// Given two vectors, the average of both
		/// </summary>	
		
		public Vector3 GetMiddleOfTargets(Vector3 A, Vector3 B) {
			Vector3 middle = (B + A) / 2f;
			
			if (orthoToCameraHeight)
			{
				float height =  transform.position.y;
				middle =  middle + Vector3.up * height;
			}			
			
			return middle;
		}
		/// <summary> 
		/// Average of the two transform positions targetA and targetB
		/// </summary>	
		public Vector3 GetMiddleOfTargets() {
			return GetMiddleOfTargets( targetA.position,  targetB.position);
		}
		/// <summary> 
		/// call SetFrontOrtho ahead of time if not initialized
		/// if initialized returns the average of both transform positions
		/// projected onto the frontOrtho (view space)
		/// </summary>	
		public Vector3 GetMiddleOfTargets(bool ortho) {
			Vector3 A = ProjectedOrtho(targetA.position, frontOrtho);
			Vector3 B = ProjectedOrtho(targetB.position, frontOrtho);
			
			return GetMiddleOfTargets( A,  B);
		}
		/// <summary> 
		/// call SetFrontOrtho ahead of time if not initialized
		/// returns the average of both vectors 
		/// projected onto the frontOrtho (view space)
		/// </summary>	
		
		public Vector3 GetMiddleOfTargets(bool ortho, Vector3 A, Vector3 B) {
			A = ProjectedOrtho(A, frontOrtho);
			B = ProjectedOrtho(B, frontOrtho);
			return GetMiddleOfTargets(A, B);
		}
		

		/// <summary> 
		/// call SetFrontOrtho ahead of time if not initialized
		/// MACRO projects a given vector A in world space
		/// onto an orthogonal plane given by view space
		/// and returns the result minus current position
		/// </summary>			
		
		public Vector3 ProjectedOrtho(Vector3 A, Vector3 ortho) {
			return Vector3.Project((ortho-transform.position), A-transform.position);
			
		}
		

		/// <summary> 
		/// Given a vector in world space target, 
		///the front orthogonal plane is set at right angle in view space
		/// Vector3 => frontOrtho
		/// "" 		=> outPosition
		/// </summary>	
		
		public void SetFrontOrtho(Vector3 target) {
			
			GetPositionOnLineSegment(target);
			frontOrtho = outPosition;
			
			
			
		}

		/// <summary> 
		/// A position set by other methods and used internally.
		/// </summary>	
		
		
		public Vector3 GetOutPosition() {return outPosition;}

		/// <summary> 
		/// Finds the vector that is farthest to the rear of transform and sets outPosition.
		/// Culls objects outside or exactly on min and max
		/// see maxViewDistance, minViewDistance
		/// returns true if a position is newly stored
		/// Vector3 => outPosition
		/// 
		/// </summary>	
		
		public bool GetRearmost(Vector3[] vList, Vector3 facing) {
			bool hasPosition = false;
			Vector3 back = transform.position + facing * minViewDistance;
			Vector3 front = transform.position + facing * maxViewDistance;
	
			Vector3 saved = new Vector3();
			
			Vector3 point;
	
			float compareDistance = maxViewDistance - minViewDistance;		
			float newDistance;
			
			
			foreach ( Vector3 v in vList ) {
				point = GetClosestPointOnLineSegment(front, back, v);
				
				newDistance = Vector3.Distance(back, point);
				
				// 0 to float maxViewDistance (unknown). If a point is outside the range it is the same as one or the other.
				if (!newDistance.IsZero() && newDistance < compareDistance){
					compareDistance = newDistance;
					saved   = v;
					
					hasPosition = true;
				}
					
				
				
			}
			if (hasPosition)
				outPosition = saved;
			
			return hasPosition;
		}
		/// <summary> 
		/// Finds the vector that is farthest to the rear of transform and sets outPosition.
		/// Culls objects outside or exactly on min and max
		/// see maxViewDistance, minViewDistance
		/// Assumes forward
		///
		/// returns true if a position is newly stored
		/// Vector3 => outPosition
		/// 
		/// </summary>	
		public bool GetRearmost(Vector3[] vList) {
			return GetRearmost(vList, transform.forward);
		}
		/// <summary> 
		/// Finds the vector that is farthest to the rear of transform and sets outPosition.
		/// Culls objects outside or exactly on min and max
		/// see maxViewDistance, minViewDistance
		/// Assumes forward
		///
		/// returns true if a position is newly stored
		/// Vector3 => outPosition
		/// optional out parameter, same use as outPosition
		/// </summary>
		public bool GetRearmost(Vector3[] vList, out Vector3 vec) {
			
			bool bo = GetRearmost(vList, transform.forward);
			vec = outPosition;
			return bo;
		}
		
		
		protected float GetPositionOnLineSegment(Vector3 v) {
			// returns 0 back or behind that, 1 is front or in front of
			
			// pretty much exactly like a camera setup. back to front
			Vector3 back = transform.position + transform.forward * minViewDistance;
			Vector3 front = transform.position + transform.forward * maxViewDistance;
			
			Vector3 point = GetClosestPointOnLineSegment(front, back, v);
			
			float d = maxViewDistance - minViewDistance;
			float d2 = Vector3.Distance(back, point);
			// 0 back, 1 is front
			float distFront = d2/d;
			// set animation paramter distanceToFront
			
			return distFront;
			
		}
		
		/// <summary> 
		/// 
		/// returns 0 from or behind that, 1 is to or beyond
		/// Vector3 => outPosition
		/// 
		/// </summary>
		public float GetPositionOnLineSegment(Vector3 from, Vector3 to, Vector3 v) {
			
			Vector3 point = GetClosestPointOnLineSegment(to, from, v);
			
			float d = Vector3.Distance(from, to);
			float d2 = Vector3.Distance(from, point);
			// 0 from, 1 is to
			float distFront = d2/d;
			// set animation paramter distanceToFront
			
			return distFront;
			
		}
		/// <summary> 
		/// sets and returns outPosition
		/// result position is clamped from vA to vB, always returns 
		/// </summary>
		
		public Vector3 GetClosestPointOnLineSegment(Vector3 vA, Vector3 vB, Vector3 vPoint) {
			
			Vector3 vVector1 = vPoint - vA;
			Vector3 vVector2 = (vB - vA).normalized;

			float d = Vector3.Distance(vA, vB);
			float t = Vector3.Dot(vVector2, vVector1);

			if (t <= 0)
				return vA;

			if (t >= d)
				return vB;

			Vector3 vVector3 = vVector2 * t;

			Vector3 vClosestPoint = vA + vVector3;

			outPosition = vClosestPoint;
		
			return outPosition;
	
		}
		
		
		
		public void MaintainALeft(){
			if (targetA == null || targetB == null) return ;
			
			
			float Aright = Vector3.Dot(targetA.position - transform.position, transform.right);
			float Bright = Vector3.Dot(targetB.position - transform.position, transform.right);
			
			if (Aright > Bright)
			{
			// changes side so result is closer to Transform (if allowed)
				Swap();
			}
				
			
		}
		
		/// <summary> 
		/// this gets a point where both objects are aligned on a single plane at the current distance.
			
		/// => _PRay to transform direction Set perpendicular to A and B
		/// => _perpendicular is a position perpendicular from between A and B
		/// tip: pan transform left or right to move perpendicular to the targets seen by viewport
			
		/// </summary>
		
		protected void SetPerpendicularRay() {
			
			if (_keepALeft) MaintainALeft();
			
			// setting up
			Vector3 middle =  GetMiddleOfTargets();

			Vector3 direction = (targetB.position - targetA.position).normalized;	
			
			
			
			/*
			
			//Quat representing the forward of direction
			Quaternion q = Quaternion.identity;
			q.SetLookRotation(direction);
			
			
			// Updating direction, rotating it GetPerpendicular with -v.right, and then setting it a distance away.
			direction = q * -Vector3.right * distance;
			
			// Adding the current height back in
			Vector3 pointOnCircle = midUpper + direction;
			
			*/

			float result = GetPositionOnLineSegment(middle + direction * extents, middle - direction * extents, transform.position);
			
			if (0f < result && result < 1f) // we know it's orthogonal, between the bounds then
			{
				_PRay.direction = transform.position - outPosition;
				// between the two
				_PRay.origin = middle;
				_MPRay = new Ray(_PRay.origin, _PRay.direction);
				
				// Determine which is farther, this object or the two objects. Used for making a projection.
				float distance = Vector3.Distance(middle,zoomPoint);					
				_perpendicularPoint = _PRay.GetPoint(distance);

				// now from where I am
				
				_PRay.origin = outPosition;
			}

			 

			
		}
		
	
		// ###################
		/// <summary> 
		/// true if any point is between maxViewDistance and minViewDistance
		/// if (true) rearmost point => outPosition
		/// assumes forward of transform
		/// </summary>
		
		public bool FitAll(Vector3 [] vList) {
			return FitAll(vList, transform.forward);
		}
		/// <summary> 
		/// true if any point is between maxViewDistance and minViewDistance
		/// if (true) rearmost point => outPosition
		/// the normalized 'forward' vector is the facing 
		/// </summary>
		public bool FitAll(Vector3 [] vList, Vector3 facing) {
			Vector3[] newV = new Vector3[vList.Length];
			int i = 0;
			foreach (Vector3 v in vList)
			{
				newV[i] = (Fit(v));
				i ++;
			}
		
			return GetRearmost(newV, facing);
		}
		
		/// <summary> 
		/// Fits a point in the viewport assuming the camera is perspective
		/// => outPosition
		/// => frontOrtho becomes adjacent leg 
			
		/// </summary>
		/*
		screen check (viewport vs screen
		0 < screenPoint.x && screenPoint.x < 1
				&& 0 < screenPoint.y && screenPoint.y < 1;
				
		*/
		public Vector3 Fit(Vector3 point) {
			
			SetFrontOrtho(point);
			
			
			// *** Camera angle FOV ***
			float radAngle = cam.fieldOfView * Mathf.Deg2Rad;
			float radHFOV = Mathf.Atan(Mathf.Tan(radAngle / 2f) * cam.aspect);
			
			
			float opposite = Vector3.Distance(point, frontOrtho);
			float adjacent = opposite/radHFOV ;
			
			// *** Have ray face backward
			_DRay = new Ray(frontOrtho, -transform.forward);
			Vector3 p = _DRay.GetPoint(adjacent);
			return p;
			
		}
	
// METHODS TO CHECK FOR CLIPPING / BLOCKAGE, and MOTION
/* e.g.
	spot = point
	swat = lerp from current position to GetPerpendicular
	slide = direct from cur to GetPerpendicular
*/	
		/// <summary> 
		/// returns CheckSphere at pre recommended position
		/// </summary>	
		
		public bool SpotCheck() {
			return Physics.CheckSphere(zoomPoint, radius, activeLayer);
		}
		/// <summary> 
		/// returns CheckSphere at v
		/// </summary>	
		public bool SpotCheck(Vector3 v) {
			// this checks a specific location
			if (Physics.CheckSphere(v, radius, activeLayer)) {
				outPosition = v;
				return true;
			}
			return false;
		}
		

		/// <summary> 
			
		/// interpolates from position to GetPerpendicular using Lerp
		/// returns true if any point on a rail
		/// if (true) hit.point => outPosition
		/// </summary>			
		
		public Vector3 GetSlide(Vector3 target)	{
			
			Vector3 direction =  (GetPerpendicular() - GetMiddleOfTargets()); // - transform.position;
			Ray ray = new Ray(target,  _PRay.direction);				
			
			Vector3 goal = ray.GetPoint( direction.magnitude );
			return goal;
		}
		
		/// <summary> 
			
		/// interpolates from position to GetPerpendicular using Lerp
		/// returns true if any point on a rail
		/// if (true) hit.point => outPosition
		/// </summary>	
		
		public bool SlideCheck(Vector3 target)	{
			
			Vector3 direction =  (GetPerpendicular() - GetMiddleOfTargets()); // - transform.position;
			Ray ray = new Ray(target,  _PRay.direction);				
			
			Vector3 goal = ray.GetPoint( direction.magnitude );
			
			direction = goal - transform.position;
			ray = new Ray(transform.position,  direction);			
			
			
			RaycastHit rh;
			
			
			Debug.DrawLine(ray.origin, GetPerpendicular(), Color.red);
			if (SpotCheck(ray.origin))
				return true;
			else if (Physics.Raycast( ray, out rh, direction.magnitude, activeLayer))
			{
				if (debug) 
					Debug.DrawLine(rh.point, ray.origin, Color.green);
				
				outPosition = rh.point;
				return true;
			}
			else if (debug) 
					Debug.DrawLine(ray.origin, GetPerpendicular(), Color.green);
	
			outPosition = goal;			
				
			return false;
		}
		
		/// <summary> 
			
		/// interpolates from position to GetPerpendicular using Lerp
		/// returns true if any point on a rail
		/// if (true) hit.point => outPosition
		/// </summary>	
		
		public bool SlideRay(Vector3 target)	{
			
			Vector3 tween_r;
			
			float frac = 0f;
			Ray ray = new Ray(target,  _PRay.direction);
			float length =( GetPerpendicular()- GetMiddleOfTargets()).magnitude;
			
			RaycastHit rh;
			Vector3 goal = ray.GetPoint(length);
			ray.direction = - ray.direction;
			
			while (frac <= 1)
			{
				tween_r = Vector3.Lerp( transform.position, goal, frac);
				
				ray.origin = tween_r;
				
				
				Debug.DrawLine(tween_r, ray.GetPoint(length), Color.red);
				if (Physics.Raycast( ray, out rh, length, activeLayer))
				{
					if (debug) 
						Debug.DrawLine(rh.point, tween_r, Color.green);
					
					outPosition = rh.point;
					return true;
				}
				else if (debug) 
						Debug.DrawLine(tween_r,  ray.GetPoint(length), Color.green);
					
				
				frac += 0.1f;
			}
			outPosition = goal;
			return false;
		}
		
		/// <summary> 
			
		/// interpolates from position to GetPerpendicular using Slerp
		/// returns true if any point on a rail
		/// if (true) hit.point => outPosition
		///	else goal => outPosition
		/// </summary>	
		
		public Vector3[] SwatArray( Vector3 target, float distfromtarget = 0.5f, int arraySize = 10) {
			
			distfromtarget *= (GetPerpendicular()- GetMiddleOfTargets()).magnitude;
			Ray ray = new Ray(target, _PRay.direction);
			ray.origin = ray.GetPoint(distfromtarget);
			
			Vector3 original = transform.position - ray.origin;
			Vector3 tween_r;
			int i = 0;
			Vector3 goal = ray.GetPoint(distfromtarget) - ray.origin;
			Vector3[] vList = new Vector3[arraySize];
			float frac;
			while (i < arraySize) {
				frac = 0.1f * i;
				tween_r = Vector3.Slerp(original, goal, frac);
				ray.direction = tween_r;
				
				if (debug)
					Debug.DrawLine(ray.GetPoint(tween_r.magnitude), ray.origin, Color.green);
				vList[i] = (ray.GetPoint(tween_r.magnitude));
				i ++;
			}
			
			return vList;
		}
		
		/// <summary> 
			
		/// interpolates from position to GetPerpendicular using Slerp
		/// returns true if any point on a rail
		/// if (true) hit.point => outPosition
		///	else goal => outPosition
		/// </summary>	
		
		
		public bool SwatCheck( Vector3 target, float distfromtarget = 0.5f) {
			
			distfromtarget *= (GetPerpendicular()- GetMiddleOfTargets()).magnitude;
			Ray ray = new Ray(target, _PRay.direction);
			ray.origin = ray.GetPoint(distfromtarget);
			
			Vector3 original = transform.position - ray.origin;
			Vector3 tween_r;
			float frac = 0f;
			
			Vector3 goal = ray.GetPoint(distfromtarget) - ray.origin;
			
			while (frac <= 1.01) {
				tween_r = Vector3.Slerp(original, goal, frac);
				ray.direction = tween_r;
				frac += 0.1f;
				
				if (debug)
					Debug.DrawLine(ray.GetPoint(tween_r.magnitude), ray.origin, Color.green);
				if (SpotCheck(ray.GetPoint(tween_r.magnitude)))
					return true;
				
			}
			outPosition = ray.origin + goal;
			return false;
		}
		
		/// <summary> 
			
		/// interpolates from position to GetPerpendicular using Slerp
		/// returns true if any point on a rail
		/// if (true) hit.point => outPosition
		///	else goal => outPosition
		/// </summary>	
		
		public bool SwatRay( Vector3 target, float distfromtarget = 0.5f) {
			
			distfromtarget *= (GetPerpendicular()- GetMiddleOfTargets()).magnitude;
			Ray ray = new Ray(target, _PRay.direction);
			ray.origin = ray.GetPoint(distfromtarget);
			
			Vector3 original = transform.position - ray.origin;
			Vector3 tween_r;
			float frac = 0f;
			
			Vector3 goal = ray.GetPoint(distfromtarget) - ray.origin;
			
			
			RaycastHit rh;
			
			while (frac <= 1.01) {
				tween_r = Vector3.Slerp(original, goal, frac);
				ray.direction = tween_r;
				frac += 0.1f;
				
				if (debug)
					Debug.DrawLine(ray.GetPoint(tween_r.magnitude), ray.origin, Color.green);
				else if (Physics.Raycast(ray, out rh, tween_r.magnitude, activeLayer))
				{
					outPosition = rh.point;
					return true;
				}
				
			}
			outPosition = ray.origin + goal;
				
			return false;
		}
		
// ################# END OF CLIP CHECKING METHODS
	}
}


