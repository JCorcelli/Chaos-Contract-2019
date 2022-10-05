using UnityEngine;

namespace NPC.Strategy
{
	public abstract class ShubConnect : MonoBehaviour {

		protected StrategyHUB shub;
		protected Transform _transform;
		
		new public Transform transform{get{if (_transform == null) _transform = GetComponent<Transform>();
		return _transform;}set{_transform = value;}}
		
		protected void Connect() {
			
			shub = GetComponentInParent<StrategyHUB>();
		}
		
		public bool InList(string s) {return Listed(s);}
		public bool Listed(string s) {
			return shub.list.Contains(s);
		}
		protected virtual void OnEnable() {
			if (shub == null)
				Connect();
			if (shub == null)
				return;
			
			shub.onStart += OnStart;
			shub.onStop  += OnStop;
			
		}
		protected virtual void OnDisable() {
			if (shub == null)
				return;
			shub.onStart -= OnStart;
			shub.onStop  -= OnStop;
		}
		
		
		protected virtual void OnStart(){
			// something like: if (shub.recentIn == )
				
			// pump with: GeneralUpdateManager.OnUpdate += OnUpdate;
		}
		protected virtual void OnStop(){
			
			// something like: if (shub.recentOut == )
				
			// pump with: GeneralUpdateManager.OnUpdate -= OnUpdate;
		}
		

		// utility functions
		protected void EnableChildren(){
			foreach (Transform child in transform)
				child.gameObject.SetActive(true);
		}
		protected void DisableChildren(){
			foreach (Transform child in transform)
				child.gameObject.SetActive(false);
		}
		
		protected void EnableChild(int n = 0)
		{
			transform.GetChild(n).gameObject.SetActive(true);
		}
		protected void DisableChild(int n = 0)
		{
			transform.GetChild(n).gameObject.SetActive(false);
		}
		
		
	}
}