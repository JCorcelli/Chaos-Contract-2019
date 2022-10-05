using UnityEngine;
using System.Collections;

public class CrossToState : MonoBehaviour {

	// Use this for initialization
	
	public string state = "";
	public float crossFadeTime = 1f;
	public float playbackSpeed = 1f;
	public int layer = -1;
	public float normalizedTime = 0.5f;
	public float startTime = 0f;
	
	private Animation anim;
	
	void Awake () { anim = GetComponent<Animation>(); }
	
	void OnEnable () {
		anim[state].speed = playbackSpeed;
		anim.Play(state);
		//StartCoroutine("StartAndStopThreeTimes");
	}
	void OnDisable () {
		anim[state].speed = -playbackSpeed;
		if (anim[state].time <= 0f)
			anim[state].time = anim[state].length;
		anim.Play(state);
		//StartCoroutine("StartAndStopThreeTimes");
	}
	
	
	
	IEnumerator CrossFade() {
		
		float fadeTime = crossFadeTime;
		float duration = 0f;
		
		float nStartTime = startTime;
		float nEndTime = normalizedTime;
		
		
		// float diffTime = nStartTime - nEndTime;
		float tween = 0f;
		while (fadeTime > duration)
		{
			
			GetComponent<Animator>().Play(state, layer, tween);
			duration += Time.deltaTime;
			tween = Mathf.Lerp(nStartTime, nEndTime, duration / fadeTime);
			//nStartTime - diffTime * duration / fadeTime; // fraction completed
			
			yield return null;
		}
		tween = nEndTime;
		
		
		GetComponent<Animator>().Play(state, layer, tween);
	}
	
	IEnumerator StartAndStopThreeTimes() {
		
		
		//normalizedTime
		startTime = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime;
		normalizedTime = 0.5f;
		StartCoroutine("CrossFade");
		yield return new WaitForSeconds(2f);
		
		normalizedTime = 0.1f;
		startTime = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime;
		StopCoroutine("CrossFade");
		StartCoroutine("CrossFade");
		yield return new WaitForSeconds(2f);
		
		normalizedTime = 0.95f;
		startTime = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime;
		StopCoroutine("CrossFade");
		StartCoroutine("CrossFade");
		
	}
	
	
}
