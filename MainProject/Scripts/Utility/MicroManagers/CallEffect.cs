using UnityEngine;
using System.Collections;
using Utility.Managers;

public class CallEffect : MonoBehaviour {


	protected EffectHUB ehub;
	IEnumerator Start () {
		yield return new WaitForSeconds(5f);
		Debug.Log("I'm calling it",gameObject);
		ehub = GetComponent<EffectHUB>();
		ehub.SubmitString("Heart");
	}
	
}
