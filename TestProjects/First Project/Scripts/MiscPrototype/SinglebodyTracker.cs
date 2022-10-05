using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class SinglebodyTracker : MonoBehaviour {

	private Transform thisChild;
	[System.Serializable]
	public class TrackerEvent : UnityEvent { }
	public TrackerEvent doOnHit;
	
	void Awake(){
		thisChild = transform.GetChild(0);
	}
	
	public void SetTarget(GameObject go){
		thisChild.parent = go.transform;
		thisChild.gameObject.SetActive(true);
		thisChild.transform.localPosition = Vector3.zero; // so it should always be the center of the target object
	}
	private void OnTriggerEnter(Collider col) {
		if (col.gameObject == thisChild.gameObject)
		{
			thisChild.gameObject.SetActive(false);
			thisChild.parent = transform;
			doOnHit.Invoke();
		}
	}
	
}
