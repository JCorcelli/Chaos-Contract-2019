using UnityEngine;
using System.Collections;
using Utility.Managers;

public class AnimPlaybackOnHold : AbstractTriggerHUBSubscriber {

	// Use this for initialization
	
	public string state = "";
	
	public float playbackSpeed = 1f;
	public int layer = -1;
	
	private Animator anim;
	
	protected override void Awake () { 
		base.Awake();
		anim = GetComponent<Animator>(); 
		
		if (anim == null) anim = GetComponentInParent<Animator>();
		}
	
	protected override void OnEnter () {
		Play();
	}
	
	protected override void OnExit() {
		
		anim.speed = 0f;
		
	}
	
	protected void Play() {
		
		if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0f)
			anim.Play(state, 0, 0f);
		anim.SetFloat("animSpeed", playbackSpeed);
		anim.speed = 1;
		//anim.Play(state);
		//StartCoroutine("StartAndStopThreeTimes");
		
	}
	
	protected void Reverse() {
		
		if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
			anim.Play(state, 0, 1f);
		anim.SetFloat("animSpeed", -playbackSpeed);
		anim.speed = 1;
		//if anim.GetCurrentAnimatorStateInfo(-1).normalizedTime == 0;
		//anim.Play(state, -1, 1);
	}
		
	
	
}
