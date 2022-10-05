using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Utility.Timers
{
	public class EggTimer : UpdateBehaviour {
		

		protected float _timer;
		
		public float timer = 60; // seconds
		public bool resetOnEnabled = true;
		public bool onZero = true;
		
		// makes list for selecting any function
		[System.Serializable]
		public class EggTimerEvent : UnityEvent { }
		public EggTimerEvent onZeroEvents;
			
		
		public void OnReset(){
			_timer = timer;
		}
		
		void Awake() {
			_timer = timer;
			
		}
		protected override void OnEnable(){
			base.OnEnable();
			if ( resetOnEnabled )
			{
				OnReset();
			}
			
		}
		
		// Update is called once per frame
		protected override void OnFixedUpdate () {
			
			_timer -= Time.deltaTime;
			if (_timer <= 0)
			{
				_timer = 0;
				if (onZero)
				{
					if (onZeroEvents != null) onZeroEvents.Invoke();
					return;
				}
			}
			
		}
	}
}
