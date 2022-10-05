using UnityEngine;
using System.Collections;

namespace NPCSystem
{
	public class JerriBedroomCharacter : UpdateBehaviour
	{
		
		
		protected Animator ianimate;
		protected AICharacterControl imove;
		protected ThirdPersonCharacter ichar;
		protected Transform imoveDefault;
		
		
		protected int state = 0;
		public int animState = 0;
		public Transform bed_target	;
		public Transform bed_contact;
		public Transform bed_face	;
		public Transform bed_goal	;
		protected bool running = false;
		void Awake() {
			ichar = GetComponent<ThirdPersonCharacter>();
			ianimate = GetComponent<Animator>();
			imove = GetComponent<AICharacterControl>();
			
			imoveDefault = imove.target;
		}
		
		
		
		
		protected override void OnUpdate()
		{
			if (state == 0 ) return;
			else if (state == 1)
				GoToBed();
			
			else if (state == 2)
			{
				StartCoroutine("TurnTowardsBed");
				state = 3;
			}
			else if (state == 3)
			{
				// running
				if (!running) // Facing bed
				{
					state = 4;
				}
			}
			else if (state == 4)
			{
				state = 5;
			}
			else if (state == 5)
			{
				
				ianimate.SetInteger("State", 1);
				animState = 1;
				
				StartCoroutine("GetInBed");
				state = 6;
			}
			else if (state == 6)
			{
				if (! running) // in bed > auto
					state = 7;
			}
			else if (state == 7)
			{
				ianimate.SetInteger("SubState", 1);
				StartCoroutine("GoToEdge");
				state = 8;
			}
			else if (state == 8)
			{
				if (! running) // in bed > auto
					state = 9;
			}
			else if (state == 9)
			{
				StartCoroutine("GetUp");
				state = 10;
			}
			
			else if (state == 10)
			{
				if (! running) // in bed > auto
					state = 11;
			}
		
			else if (state == 11)
			{
				state = 0;
				NavOn();
				imove.target = imoveDefault;
			}
				
			//else if animState == 1 prepare to change to another routine, reset animState to 
		}

		protected void GoToBed(){
			
			imove.target.transform.position = bed_target.position; // or I could change the target
			if (Vector3.Distance(bed_target.position, imove.transform.position) <= 0.7f)
				state = 2;
		}
		protected void NavOn()
		{
			imove.agent.enabled =true;
			imove.enabled = true;
		}
		protected void NavOff()
		{
			imove.agent.enabled =false;
			imove.enabled = false;
			ichar.Move(Vector3.zero);
		}
		
		protected IEnumerator TurnTowardsBed()
		{
			if (running) yield break;
			running = true;
			float timer = 0f;
			imove.target = bed_face;
			
			
			Vector3 lookPos = bed_face.position - bed_contact.position;
			lookPos.y = 0;
			Quaternion rotation = Quaternion.LookRotation(lookPos);
			Quaternion saveRot = transform.rotation;
			
			Vector3 savePos = transform.position;
			Vector3 newPos = bed_target.position;
			
			
			while (timer < 1.2f) 
			{
				yield return null;
				transform.rotation = Quaternion.Slerp(saveRot, rotation, timer / 1.2f);
				timer += Time.deltaTime;
				
				transform.position = Vector3.Lerp(savePos, newPos, timer / 1.2f);
				
			}
			NavOff(); // HACK, might result in lack of accuracy
			transform.rotation = rotation;
			transform.position = newPos;
			
	
			running = false;
		}
		
		
		protected IEnumerator GetInBed()
		{
			if (running) yield break;
			running = true;
			
			yield return null;
			
			
			// the lerp fraction should be based on animation frame %
			
			
			
			Vector3 newPos = bed_contact.position;
			
			Vector3 savePos = transform.position;
			
			float timer = Time.deltaTime;
			while (timer < 0.4f)
			{
				
				
				
				transform.position = Vector3.Lerp(savePos, newPos, timer / 0.4f); // 0.4f is transition duration
				
				yield return null;
				
				timer += Time.deltaTime;
			}
			
			timer = 0f;
			savePos = transform.position;
			while (timer < 1.1f)
			{
				yield return null;
				
				
				transform.position = Vector3.Lerp(savePos, bed_goal.position, timer / 1.1f);
				
				timer += Time.deltaTime;
			}
			
			transform.position = bed_goal.position;
			running = false;
			yield return null;
		}

		protected IEnumerator GetUp(){
			if (running) yield break;
			running = true;
			
			// should probably getstate, something else could have changed it.
			ianimate.SetInteger("State", 0);
			animState = 0;
			ianimate.SetInteger("SubState", 2); // swinging leg
			yield return null;
			
			
			while (!ianimate.GetAnimatorTransitionInfo(0).IsName("GettingOutOfBed -> Exit") )
			{
				yield return null;
			}
			
			
			
			Vector3 savePos = transform.position;
			
			while (ianimate.GetAnimatorTransitionInfo(0).IsName("GettingOutOfBed -> Exit"))
			{
				
				
				transform.position = Vector3.Lerp(savePos, bed_target.position, ianimate.GetAnimatorTransitionInfo(0).normalizedTime);
				
				yield return null;
			}
			transform.position = bed_target.position;
			
			ianimate.SetInteger("SubState", 0); // swinging leg
			running = false;
			
		}
		protected IEnumerator GoToEdge(){
			
			if (running) yield break;
			running = true;
			yield return new WaitForSeconds(3f);
			
			
			float timer = 0f;
			Vector3 savePos = transform.position;
			
			while (timer < 1.1f)
			{
				
				
				transform.position = Vector3.Lerp(savePos, bed_contact.position, timer / 1.1f);
				
				timer += Time.deltaTime;
				yield return null;
			}
			transform.position = bed_contact.position;
			
			running = false;
		}
	}
}
