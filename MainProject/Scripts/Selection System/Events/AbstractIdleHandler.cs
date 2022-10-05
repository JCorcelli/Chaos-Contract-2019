using UnityEngine;
using System.Collections;


namespace SelectionSystem
{
	public abstract class AbstractIdleHandler : MonoBehaviour
	{

		protected bool _activeInternal = false;
		protected virtual void Start() {
			SelectionManager.onPress += _OnPress;
			SelectionManager.onRelease += _OnIdle;
		}
		
		
		protected virtual void OnDestroy() {
			
			SelectionManager.onPress -= _OnPress;
			SelectionManager.onRelease -= _OnIdle;
		}
		
		public virtual void _OnPress(){
			if (gameObject.activeInHierarchy) 
				StopCoroutine("_Idle");
			_idle = false;
			OnPress();
				
		}
		protected virtual void OnPress(){}
		
		protected bool _idle = false;
		
		
		
		public virtual void _OnIdle(){
			OnIdle();
			if (gameObject.activeInHierarchy) 
				StartCoroutine("_Idle");
		
		}
			
		protected virtual IEnumerator _Idle(){
			if (_idle) yield break;
			_idle = true;
			while (_idle)
			{
				Idle();
				
				yield return null;
			}
		}
		protected virtual void Idle(){}
		protected virtual void OnIdle(){}
		
	
	}
}