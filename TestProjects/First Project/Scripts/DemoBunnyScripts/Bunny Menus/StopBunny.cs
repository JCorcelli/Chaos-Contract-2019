using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;

namespace TestProject 
{
	public class StopBunny : MonoBehaviour {

		// responds to a message
		private Animator m_Animator;
		void Start(){
			m_Animator = gameObject.GetComponent<Animator>();
		}
		
		public void CancelAction(){
			
			// need to touch ground first...
			gameObject.GetComponent<NavMeshAgent>().nextPosition = transform.position;
			gameObject.GetComponent<NavMeshAgent>().updatePosition = true;
			Rigidbody rb = gameObject.GetComponent<Rigidbody>();
			// really need to touch ground first...
			rb.useGravity = false;
			//rb.constraints = RigidbodyConstraints.FreezeRotation;
			transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
			
			gameObject.GetComponent<AICharacterControl>().enabled = true;
			// gameObject.GetComponent<AICharacterControl>().SetTarget(null);
			
			gameObject.GetComponent<ThirdPersonCharacter>().enabled = true;
		}
		
		public void PetAction(){ EnableMenuAction(); }
		public void PushAction(){ EnableMenuAction(); }
		
		private void EnableMenuAction(){
			
			m_Animator.SetFloat("Forward", 0f);
			m_Animator.SetFloat("Turn", 0f);
			gameObject.GetComponent<Animator>().applyRootMotion = false;
			gameObject.GetComponent<NavMeshAgent>().updatePosition = false;
			Rigidbody rb = gameObject.GetComponent<Rigidbody>();
			
			rb.useGravity = true;
			//rb.constraints =  RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
			
			gameObject.GetComponent<AICharacterControl>().enabled = false;
			
			gameObject.GetComponent<ThirdPersonCharacter>().enabled = false;
			
			
		}
	}
}