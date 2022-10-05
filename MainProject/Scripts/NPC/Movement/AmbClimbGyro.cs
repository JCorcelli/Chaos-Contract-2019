using UnityEngine;
using System.Collections;

using Dungeon;
using SelectionSystem;

namespace NPCSystem
{
	public class AmbClimbGyro : UpdateBehaviour {

		protected Vector3 m_GroundNormal = Vector3.up;
		protected Vector3 m_GroundPoint = Vector3.up;
		// Use this for initialization
		
		
		[SerializeField] protected LayerMask activeLayers = 1;
		
		
		protected override void OnLateUpdate () {
			// hopping
			CheckGroundStatus();
					
			float rTime = Mathf.Repeat( Time.time, .8f) ;
			if (rTime > .45f)
			{
				
				transform.localPosition = transform.up * .05f;
			}
			else
				transform.localPosition = Vector3.zero;
		}
		protected void CheckGroundStatus()
		{
			RaycastHit hitInfo;
			
			
			
			
			
			
			// 0.1f is a small offset to start the ray from inside the character
			// it is also good to note that the transform position in the sample assets is at the base of the character
			
			
			if (Physics.Raycast(transform.parent.position + (Vector3.up * 0.1f), -transform.parent.up, out hitInfo, .5f, activeLayers))
			{
				
				
				m_GroundNormal = hitInfo.normal;
				m_GroundPoint = hitInfo.point;

					
				// transform.up = m_GroundNormal;
				
					
				
			}
			else
			{
				
				m_GroundNormal = Vector3.up;
				m_GroundPoint = transform.position;
				

			}
				Vector3 fore = Vector3.ProjectOnPlane(transform.parent.forward * 10f, m_GroundNormal);
				
				transform.rotation = Quaternion.LookRotation(fore);
				
		}	
		
	}
}