using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace NPCSystem 
{
	
	
    [RequireComponent(typeof (SphereCollider))]
	public class WPTargeting : UpdateBehaviour, INavTargeting {
		
		
		protected delegate void VoidMethod();
		protected delegate void SetTransformMethod(Transform t);
	
		protected VoidMethod navMethod;
		protected SetTransformMethod setMethod;
		public NavTargetType navType = (NavTargetType)0;
		public bool repeat = true;
		public float waypointThreshold = 1f;
		public Transform _waypointList;
		public Transform currentTarget;
		
		public Transform anchor;
		protected List<Transform> waypointList = new List<Transform>();
		
		protected int i = 0;
		protected float remainingDistance = 0f;
		
		protected SphereCollider distanceLimiter;
		protected Transform radiusTransform;
		
		public void SetTransform(Transform t){
			// by default this means... I go to this last
			setMethod(t);
		}
		
		
		protected void SetCurrent(Transform t) {
			currentTarget = t;
		}
		protected void AddWaypoint(Transform t)
		{
			waypointList.Add(t);
			
		}
		
		protected void ReplaceWaypoints(Transform transformWChildren)
		{
			_waypointList = transformWChildren;
			_ReplaceWaypoints();
			
		}
		protected void _ReplaceWaypoints()
		{
			
			if (_waypointList == null) Debug.LogError("Error: NO _waypointList given", gameObject);
			waypointList.Clear();
			foreach (Transform t in _waypointList)
				waypointList.Add(t);
			
		}
		
		public void SetNavType(int type){
			SetNavType((NavTargetType)type);
			
		}
		public void SetNavType(NavTargetType type){
			
			navType = type;
			// set the nav type
			
			switch (type)
			{
				case NavTargetType.Single:
				
					navMethod = Single;
					break;
				case NavTargetType.Multi:
					navMethod = Multi;
					break;
				case NavTargetType.Pingpong:
					navMethod = PingPong;
					break;
				case NavTargetType.Fixed:
					navMethod = Fixed;
					break;
				default:
					Debug.LogError("Invalid navtype", gameObject);
					break;
			}
				
			
			// set the setMethod
			switch (type)
			{
				case NavTargetType.Single:
				
					setMethod = SetCurrent;
					break;
				case NavTargetType.Multi:
					setMethod = AddWaypoint;
					break;
				case NavTargetType.Pingpong:
				case NavTargetType.Fixed:
					setMethod = ReplaceWaypoints;
					_ReplaceWaypoints(); // works if there's a transform
					break;
				default:
					Debug.LogError("Invalid settype '"+(int)type+"'", gameObject);
					break;
			}
		}
		protected void Start () {
			
			
			
			distanceLimiter = GetComponent<SphereCollider>();
			radiusTransform = transform;
			
			SetNavType(navType);
			
		}
		
		protected void Finish() {

		}
		
		public bool AnyCleared() {return anyCleared;}
		public void ResetClear() {anyCleared = false;}
		
		public bool anyCleared = false;
		protected float ia;
		protected int iaa;
		protected void Single() {
			if (currentTarget == null) return;
			
			bool hit =  (Vector3.Distance(transform.position, currentTarget.position) <= waypointThreshold  );
			
			if (hit)
			{
				anyCleared = true;
				
				if (!repeat)
				{
					Finish();
					return;
				}
			}
			
			
			SetDestination(currentTarget.position);
		}
		protected Vector3 goal;
		protected void SetDestination(Vector3 v){ goal = v;}
		protected void Move(){
			Vector3 newPos = Vector3.MoveTowards(transform.position, goal, 1f);
			transform.position = newPos;
			
		}
		protected void Multi() {
			// repeat will rotate the objects, no repeat will erase objects
			
			
			bool hit =  (remainingDistance <= waypointThreshold  );
			if (hit)
			{
				anyCleared = true;
				
				if (waypointList.Count < 1)
				{
					Finish();
					return;
				}
				
			}
			
			Transform extract = waypointList[0];
			
			waypointList.RemoveAt(0);
			if (repeat) waypointList.Add(extract);
			
			currentTarget = extract;
			SetDestination(currentTarget.position);
		}
		
		protected void Fixed() {
			
			
			bool hit =  (remainingDistance <= waypointThreshold  );
			if (hit)
			{				
				anyCleared = true;
				i ++;
				if (i > waypointList.Count -1 )
				{
					i = 0;
					if (!repeat) 
					{
						Finish();
						return;
					}
					
				}
			
			}
			
			
			currentTarget = waypointList[i];
			
			SetDestination(currentTarget.position);
		}
		protected void PingPong() {
					
			if (waypointList.Count == 0) 
				{currentTarget = null; return; }
			bool hit =  (remainingDistance <= waypointThreshold  );
			
			if (hit)
			{				
				anyCleared = true;
				i ++;
				if (i > waypointList.Count * 2 - 2) // twice the distance minus the endpoints is back to start. alternatively I could reverse the array
				{
					i = 0;
					if (!repeat) 
					{
						Finish();
					}
				}
			}

			ia = Mathf.PingPong(i, waypointList.Count - 1);
			iaa = Mathf.RoundToInt(ia);
			
			currentTarget = waypointList[iaa];
			
			SetDestination(currentTarget.position);			
		}
		
		protected override void OnUpdate () {
			
            if (currentTarget != null)
			{
				
				StayInZone();
				
				remainingDistance = Vector3.Distance(			currentTarget.position, transform.position );
				navMethod();
					
				
				Move();
				
			}
            else
            {
				
                Move();
            }
		}
		protected void StayInZone() {
			// reel back the agent
			if (anchor == null || distanceLimiter == null)return;
			float d = Vector3.Distance( 
			anchor.position, transform.position ); // distance from center
			
			float scaledDistance = distanceLimiter.radius *
			radiusTransform.lossyScale.y;
			if ( d   > scaledDistance  )
			{
				
				transform.position = Vector3.MoveTowards(transform.position, anchor.position, d - scaledDistance); // extra distance after subtracting the max
			}
			
			
		}
		
		
	}
}