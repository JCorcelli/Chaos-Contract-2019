using UnityEngine;

namespace CameraSystem
{
	public class CamRegionSnap : MonoBehaviour
	{
		protected RegionCheck rt;
		public Transform targetA {
			
			get{return CameraHolder.instance.targetA;}
		}
		public Transform targetB {
			get{return CameraHolder.instance.targetB;}
		}
	
			
		public float speed = 1f; // remove later, I can calculate through iterations
		void Start() {
			rt = GetComponent<RegionCheck>();
			
			rt.target = targetA;
		}
		void LateUpdate()
		{
			
			// Early out if we don't have a target
			
			
			bool targetVisible = rt.GetSafe();
			if (!targetVisible)
			{
				// need a distance threshold
				transform.localPosition *= .98f * Time.unscaledDeltaTime;
			}

			
		}	
		
		
	}
}