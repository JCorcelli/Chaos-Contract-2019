using UnityEngine;
using System.Collections;

public class PlaybackControl : MonoBehaviour {

	// Use this for initialization
	
	public string state = "";
	
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
	
	
	
}
