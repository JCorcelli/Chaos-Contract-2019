using UnityEngine;
using System.Collections;

using TestProject;

namespace PlayerAssets.Game
{
	public class BunnyPushing : MonoBehaviour {

		// Use this for initialization
		
		public LayerMask activeLayer = 0; // best for compound collider
		public float surfaceOffset = .05f;
		public float powerMultiplier = 5f;
		public float flickTimer = 2f;
		//private GameObject thisChild;
		public bool overused = false; // something to be observed or changed into a message.
		private AudioSource m_audio;
		private ParticleSystem particles;
		
		private Ray ray;
		private RaycastHit hit;
		
		private bool keepPressed = false; // a held key
		//private BunnyPushingTarget target;
		
		public void CancelAction() {
			//target.enabled = false;
			this.enabled = false;
		}
		
		// PushAction
		public void PetAction() {
			//target.enabled = true;
			this.enabled = true;
		}
		void Awake() {
			//thisChild = transform.GetChild(0).gameObject;
			//thisChild.SetActive(false);
			
			m_audio = gameObject.GetComponentInChildren<AudioSource>();
			particles = gameObject.GetComponentInChildren<ParticleSystem>();
			//target = GameObject.FindObjectOfType(typeof(BunnyPushingTarget)) as BunnyPushingTarget;
		}
		
		private IEnumerator timeOut(){
			yield return new WaitForSeconds(flickTimer);
			keepPressed = false;
			yield return null;
		}
		void Update () {
			
			// limiter
			if (particles.particleCount >= particles.main.maxParticles)
			{
				overused = true;
				return;
			}
			// button Down
			if (Input.GetButtonDown("mouse 1") || Input.GetButtonDown("mouse 2"))
			{
				//thisChild.SetActive(false);
				keepPressed = true;
				StartCoroutine("timeOut");
			
			}
			
			else if (Input.GetButtonUp("mouse 1") || Input.GetButtonUp("mouse 2"))
			{
				if (keepPressed)
					DoPush();
				
				keepPressed = false;
				StopCoroutine("timeOut");
				return;
				
			}
			
			
			
			
		}
		private void OnPlay(){
			m_audio.Play();
			particles.Play();
		}
		private void DoPush(){
	        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
	        if (!Physics.Raycast(ray, out hit, Mathf.Infinity, activeLayer.value))
	        {
				//thisChild.SetActive(false); // stop aiming
	            return;
	        }
			
			//
			//thisChild.SetActive(true); // do the push motion / effect
			OnPlay();
	        transform.position = hit.point + hit.normal*surfaceOffset;
			
			if (Input.GetButtonUp("mouse 1"))
				hit.rigidbody.AddForceAtPosition((Camera.main.transform.forward+Vector3.up).normalized*powerMultiplier, transform.position);
			else
				hit.rigidbody.AddForceAtPosition((-Camera.main.transform.forward+Vector3.up).normalized*powerMultiplier, transform.position);
		}
	}
}