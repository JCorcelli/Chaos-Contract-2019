using UnityEngine;
using System.Collections;

public class AnimPlaybackControl : MonoBehaviour {

	// Use this for initialization
	
	public string state = "";
	
	public float playbackSpeed = 1f;
	public int layer = -1;
	public float normalizedTime = 0.5f;
	public float startTime = 0f;
	
	private Animator anim;
	
	void Awake () {
		anim = GetComponent<Animator>();
		if (anim == null)
			anim = GetComponentInParent<Animator>();
		}
	
	protected void OnEnable () {
		Play();
	}
	protected void OnDisable () {
		Stop();
	}
	
	
	protected void Stop() {
		
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
