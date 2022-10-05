using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Utility.Timers
{
	public class EggTimerText : UpdateBehaviour {
		

		protected float _timer;
		
		public float timer = 60; // seconds
		public int minutes;
		public int seconds;
		public bool resetOnEnabled = true;
		public bool onZero = true;
		protected UnityEngine.UI.Text target;
		
		// makes list for selecting any function
		[System.Serializable]
		public class EggTimerEvent : UnityEvent { }
		public EggTimerEvent onZeroEvents;
			
		
		public void OnReset(bool enable = true){
			_timer = timer;
			this.enabled = enable;
		}
		
		void Awake() {
			_timer = timer;
			
			if (target == null)
				target = GetComponentInChildren<UnityEngine.UI.Text>();
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
					this.enabled = false;
					return;
				}
			}
			
			minutes = (int)_timer/60;
			seconds = (int)_timer % 60;
			target.text = minutes.ToString("D2") + ":" + seconds.ToString("D2");
		}
	}
}
