using UnityEngine;
using System.Collections;

namespace TestProject
{
	public class TogglePlayingOnClick : MonoBehaviour {

		private Animator animator;
		// Use this for initialization
		void Start () {
			animator = gameObject.GetComponent<Animator>();
		}
		
		// Update is called once per frame
		void Update () {
		
		}
		
		public void ToggleAnimation () {
			bool current = animator.GetBool("playing");
			animator.SetBool("playing", !current);
			
		}
	}
}