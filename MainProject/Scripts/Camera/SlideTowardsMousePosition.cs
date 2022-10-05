using UnityEngine;
using System.Collections;



namespace CameraSystem {
	public class SlideTowardsMousePosition : MonoBehaviour {

		
		// Update is called once per frame
		public float timeDrag = .98f;
		public float timeRelease = 3f;
		public float releaseTimer = 3f;
		public float maxDifferencex = 1f;
		public float maxDifferencey = 1f;
		public float zoomSpeed = 1f;
		public float m_GroundCheckDistance = 1000f;
		protected LayerMask activeLayers = 1;
		protected Vector3 m_GroundNormal = Vector3.zero;
		public Transform targetA;
		public Transform targetB;
		public SphereCollider distanceLimiter;
        public string _sphereColliderName;
        public string _sphereColliderTag;
		protected Vector3 savedPosition = Vector3.zero; // the world position after moving
		
		
		void Start () {
			if (distanceLimiter == null && _sphereColliderName != "")
			{
				distanceLimiter = gameObject.FindNameXTag(_sphereColliderName, _sphereColliderTag).GetComponent<SphereCollider>();
			}
			CameraHolder.instance.Init();
			targetA = CameraHolder.instance.targetA;
			targetB = CameraHolder.instance.targetB;
			
		}
		void Update () {
			Vector2 mousePos = Input.mousePosition;
			mousePos.x = mousePos.x / Screen.width * 2 - 1;
			mousePos.x = Mathf.Clamp(mousePos.x, -1, 1);
			Vector3 move = Vector3.zero; // + ( Camera.main.transform.right * mousePos.x * maxDifferencex);
			
			if (!Input.GetButton("mouse 1"))
			{
				releaseTimer += Time.unscaledDeltaTime;
				if (releaseTimer > timeRelease)
				{
					transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, zoomSpeed) ;
					
				}
				else
					savedPosition *= timeDrag * Time.unscaledDeltaTime;
				StayInZone();
				return;
			}
			mousePos.y = mousePos.y / Screen.height * 2 - 1;
			mousePos.y = Mathf.Clamp(mousePos.y, -1, 1);
			
			
			
			if (targetA == null || targetB == null) return;

			
			releaseTimer = 0f;
			move += ( (targetB.position - targetA.position) );
			
			
			transform.localPosition = Vector3.MoveTowards(transform.localPosition, move, zoomSpeed * Time.unscaledDeltaTime) ;
			StayInZone();
			savedPosition = transform.localPosition;
		}
		
		protected void StayInZone() {
			float d = transform.localPosition.magnitude;
			float scaledDistanceLimiter = distanceLimiter.radius * distanceLimiter.transform.lossyScale.y;
			
			if (  d > scaledDistanceLimiter  )
			{
				
				transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, d - scaledDistanceLimiter); // extra distance after subtracting the max
			}
			
			
		}
		
	}
}