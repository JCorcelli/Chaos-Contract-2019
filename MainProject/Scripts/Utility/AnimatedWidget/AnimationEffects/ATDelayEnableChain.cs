using UnityEngine;
using System.Collections;

namespace Utility.AnimationEffects
{
	public class ATDelayEnableChain : AnimTriggerNode {

		public float delay = 0.0f;
		protected GameObject child;
		protected WrapMode wrapMode;
		protected override void Awake() {
			base.Awake();
			child = transform.GetChild(0).gameObject;
			child.SetActive(false);
			wrapMode = anim[anim.clip.name].wrapMode;
		}
		protected override void onTriggerEnter () {
			StopCoroutine("_DelayStop");
			delayStopRunning = false;
			anim.wrapMode = wrapMode;
			
			if (!child.activeSelf)
				StartCoroutine("_DelayPlay");
		}
		
		protected bool delayPlayRunning = false;
		protected IEnumerator _DelayPlay(){
			if (delayPlayRunning) yield break;
			delayPlayRunning = true;
			yield return new WaitForSeconds(delay);
			child.SetActive(true);
			delayPlayRunning = false;
		}
		
		protected override void onTriggerExit () {
			StopCoroutine("_DelayPlay");
			delayPlayRunning = false;
			
			if (child.activeSelf)
				StartCoroutine("_DelayStop");
			
		}
		protected bool delayStopRunning = false;
		protected IEnumerator _DelayStop(){
			if (delayStopRunning) yield break;
			delayStopRunning = true;
			
			anim[anim.clip.name].normalizedTime = Mathf.Repeat(anim[anim.clip.name].normalizedTime,1);
			anim.wrapMode = WrapMode.Default;
			
			while (anim[anim.clip.name].time > 0) yield return null;
			child.SetActive(false);
			delayStopRunning = false;
		}
	}
}