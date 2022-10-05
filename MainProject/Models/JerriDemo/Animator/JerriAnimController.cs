using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using NPCSystem;
namespace Anim.Jerri
{
	public delegate void OnJerriAnimControllerDelegate(JerriAnimController caller);
	
	public class JerriAnimController : JerriAnimAbstract {

		new public Transform transform;
		public AICharacterControl tpc;
		public Rigidbody irigid;
		public CapsuleCollider col; //better crawling y=.14 col.direction = z-axis
		
		public NavMeshAgent agent;
		
		public OnJerriAnimControllerDelegate onCall;
		protected override void Awake() {
			base.Awake();
			agent = 	body.GetComponent<NavMeshAgent>();
			transform = body.GetComponent<Transform>();
			tpc = 		body.GetComponent<AICharacterControl>();
			irigid =	body. GetComponent<Rigidbody>();
			col = 		body.GetComponent<CapsuleCollider>();
		}
		
		public override void OnCall(){
			// any subscriber can check the variables after being called
			if (onCall != null) onCall(this);
			//if (play_name == "Stand_Walk_Idle") Debug.LogError("error", this.gameObject);
		}
		
		
		
	}
}