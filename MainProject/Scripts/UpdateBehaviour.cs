using UnityEngine;
using System.Collections;
using Utility.Managers;
using SelectionSystem;
	
public class UpdateBehaviour : MonoBehaviour {

	///<summary>
	/// requires GeneralUpdateManager to be instanced
	/// it's a faster Update than standard monobehaviour
	///</summary>
	
	
	
	protected virtual void DisableButtons(){
	
		
		SelectionManager.onPress -= _OnPress;
		
		SelectionManager.onRelease -= _NoInput;
	}
	
		
	protected virtual void EnableButtons(){
		
		
		
		DisableButtons();
		SelectionManager.onPress += _OnPress;
		SelectionManager.onRelease += _NoInput;
	}
	protected void EnableUpdate(){
		
		
		
		DisableUpdate();
		GeneralUpdateManager.onFixedUpdate += OnFixedUpdate;
		GeneralUpdateManager.onUpdate += OnUpdate;
		GeneralUpdateManager.onLateUpdate += OnLateUpdate;
	}
	protected void DisableUpdate(){
		
		GeneralUpdateManager.onFixedUpdate -= OnFixedUpdate;
		GeneralUpdateManager.onUpdate -= OnUpdate;
		GeneralUpdateManager.onLateUpdate -= OnLateUpdate;
		
	}
	
	protected virtual void OnEnable () {
		EnableUpdate();
		//EnableButtons();
	}
	
	// Update is called once per frame
	
	protected virtual void OnDisable () {
		
		DisableUpdate();
		DisableButtons();
	}
	
	protected virtual void OnFixedUpdate() {
		
	}
	protected virtual void OnUpdate () {
		
	}
	protected virtual void OnLateUpdate () {
		
	}
	protected virtual void _NoInput(){}
	protected virtual void _OnPress(){}
	
	protected bool Pressing(string button)
	{
		return Input.GetButtonDown(button);
	}
	protected bool Holding(string button)
	{
		return Input.GetButton(button);
	}
	protected bool Releasing(string button)
	{
		return Input.GetButtonUp(button);
	}
	
	
	
	
	
}