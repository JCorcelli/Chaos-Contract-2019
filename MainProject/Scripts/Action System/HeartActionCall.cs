using UnityEngine;
using System.Collections;

using ActionSystem; // required for use of ActionManager, ActionEventDetail

public class HeartActionCall : MonoBehaviour {

	public float delay = 2f;

	void OnEnable () {
		Invoke("CallHeart", delay);
	}
	
	
	void CallHeart() {
		
		ActionEventDetail eventDetail = new ActionEventDetail();
		eventDetail.who = "Bunny";
		eventDetail.what = "Heart";
		ActionManager.Submit(eventDetail);
	}
	void CallHeart(float delay) {
		Invoke("CallHeart", delay);
	}
}
