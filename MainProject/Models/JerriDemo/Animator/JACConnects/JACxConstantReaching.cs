using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using NPCSystem;

namespace Anim.Jerri
{
	
	
	public class JACxConstantReaching : JACxPlay, ISetTargetTransform {

		public Transform target; // obv bunny
		
		protected float height = 1f;
		protected float lookY = 0f;
		protected float turn = 0f;
		protected float lookX = 0f;
		protected float distance = 100f;
		
		new protected Transform transform;
		protected AICharacterControl tpc;
		protected Rigidbody irigid;
		protected CapsuleCollider col; //better crawling y=.14 col.direction = z-axis
		protected Animator anim;
		
		protected NavMeshAgent agent;
		
		// used for lerping the transform, and sentinel bool
		protected bool _running = false;
		protected bool setPosition = false;
		protected bool setRotation = false;
		protected float timer = 0f;
		protected float delay = -1f;
		protected Quaternion startRotation;
		protected Vector3 startPosition;
		protected Quaternion endRotation;
		protected Vector3 endPosition;
		protected Vector3 centerCrawl = new Vector3(0, 0.14f, 0);
		protected Vector3 standCrawl = new Vector3(0, 0.51f, 0);
		
		protected void Awake() {
			
			agent = 	GetComponentInParent<NavMeshAgent>();
			tpc = 		GetComponentInParent<AICharacterControl>();
			irigid = 	GetComponentInParent<Rigidbody>();
			col = 		GetComponentInParent<CapsuleCollider>(); 
		}
		protected void Start() {
			
			transform = GetComponent<Transform>();
			//agent.gameObject.GetComponent<Transform>();
		}
		
		
		protected override void OnLateUpdate(){
			// may determine if I kneel, instead of the behavior tree
			
			if (target == null) // or maybe I disable this
			{
				lookX = 0f;
				lookY = 0f;
				ih.SetReach(lookX, lookY); 
				
				return;
			}
			
			Vector3 heading = target.position - transform.position;
			
			ih.SetReachDist(heading.magnitude);
			
			Vector3 normheading = transform.InverseTransformDirection(heading).normalized;
			
			lookX = Mathf.Atan2(normheading.x, normheading.z);
			lookY = Mathf.Atan2(normheading.z, -normheading.y);
			//lookX = Mathf.Clamp(lookX, -1f,1f) ;
			//lookY = heading.z;
			
			ih.SetReach(lookX, lookY);
			
		
			
			
			
			
			
		}
		
		public void SetTargetTransform(Transform t)
		{
			target = t;
		}
	}
}