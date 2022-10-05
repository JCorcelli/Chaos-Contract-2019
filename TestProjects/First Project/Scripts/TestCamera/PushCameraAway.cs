using UnityEngine;
using System.Collections;
using UnityStandardAssets.Utility;

namespace TestProject.Cameras
{
	public class PushCameraAway : MonoBehaviour {

		// Use this for initialization
		private FollowTarget camHolder;
		public float smoothTime = 0f;
		public float force = 1f;
		public string findByTag = "CameraHolder";
		private Vector3 direction_velocity = Vector3.zero;

		//private SphereCollider sc;
		
		void Start () {
			//sc = gameObject.GetComponent<SphereCollider>();
			direction_velocity = transform.forward * force;
		}
			
		void Awake () {
			camHolder = GameObject.FindWithTag(findByTag).GetComponent<FollowTarget>(); // assume camera
			
		}
		
		
		void OnTriggerEnter(){
			camHolder.offset += direction_velocity;
			
		}
		void OnTriggerExit(){
			camHolder.offset -= direction_velocity;
			
		}
	}
}