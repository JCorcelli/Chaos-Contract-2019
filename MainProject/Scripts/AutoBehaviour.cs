using UnityEngine;
using System.Collections;
using Utility.Managers;



public class AutoBehaviour : MonoBehaviour
{
	// intended to have enable/disable toggled autonymously, for certain features

	new protected Transform transform;
	
	
	protected virtual void Awake() {
		transform = GetComponent<Transform>();
		
		// this is a bit of a hack since the class is getting less and less abstract
		
	}
	protected virtual void OnEnable () {
		transform = GetComponent<Transform>();
	}
	
	protected virtual void Start() {
		transform = GetComponent<Transform>();
		
		// this is a bit of a hack since the class is getting less and less abstract
		
		GeneralUpdateManager.onFixedUpdate += OnFixedUpdate;
		GeneralUpdateManager.onUpdate += OnUpdate;
		GeneralUpdateManager.onLateUpdate += OnLateUpdate;
	
	}
	
	public void OnDestroy() {
		
		
		GeneralUpdateManager.onFixedUpdate -= OnFixedUpdate;
		GeneralUpdateManager.onUpdate -= OnUpdate;
		GeneralUpdateManager.onLateUpdate -= OnLateUpdate;
	}
	
		
	

	protected virtual void OnFixedUpdate() {
		
	}
	protected virtual void OnUpdate () {
		
	}
	protected virtual void OnLateUpdate () {
		
	}

}
