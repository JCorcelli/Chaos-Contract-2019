using UnityEngine;
using UnityEngine.Events;

using System;
using System.Collections;

namespace Utility.Timers
{
	[System.Serializable]
	public class TimerEvent : IComparable
	{
		public float time = 0f;
		public bool enabled = true;
		public UnityEvent events;
		public UnityEvent fastEvents;
		public UnityEvent reverseEvents;
		public UnityEvent rewindEvents;
		
		public int CompareTo(object obj) {
			if (obj == null) return 1;
				
			TimerEvent other = obj as TimerEvent;
			if (other != null) 
				return this.time.CompareTo(other.time);
			else
			   throw new ArgumentException("Object is not a TimerEvent");
	   
		}
	}
	public class EventTimer : UpdateBehaviour {
		

		
		// *** INITIAL STARTUP
		[SerializeField] protected float startTimer = 0f; // seconds
		
		[SerializeField] protected int startIndex = 0;
		
		
		// used repeatedly 
		[SerializeField] protected int nextIndex = 0; 
		
		protected bool reverse = false;
		
		[SerializeField] protected float timer = 0f;
		
		[ SerializeField] protected float timeScale = 1f;
			
		
		
		// *** REPEAT
		[SerializeField] protected float repeatTimer = 0f;
		[SerializeField] protected int repeatIndex = 0;
		[SerializeField] protected bool repeat_on = false;
		
		// looks messy anywhere but end
		[ SerializeField] protected TimerEvent[] timedEvents;
		
		protected void Start() {
			nextIndex = startIndex;
			timer = startTimer;
			Array.Sort(timedEvents);
			FixReverse();
			
			if (reverse && timedEvents.Length > 0)
			{
				if (repeatTimer < 0f)
					repeatTimer = timedEvents[timedEvents.Length - 1].time;
				if (repeatIndex < 0f)
					repeatIndex = timedEvents.Length -1;
			}
		}
		
		protected int naturalIndex;
		
		public void DisableOnCall(){
			timedEvents[naturalIndex].enabled = false;
		}
		
		
		public void SeekIndex(int newIndex){
			
			SeekByIndex(newIndex);
		}
		public void SeekTimer(float newTime){
			
			SeekByTime(newTime);
		}
		
		public void SetIndex(int newIndex){
			
			SetByIndex(newIndex);
		}
		public void SetTimer(float newTime){
			
			SetByTime(newTime);
		}
		
		
		public void SetRepeatIndex(int newIndex){
			
			repeatIndex = newIndex;
		}
		public void SetRepeatTime(int newTime){
			
			repeatTimer = newTime;
		}
		
		public void SetRepeat(bool doRepeat){
			
			repeat_on = doRepeat;
		}
		
		protected bool _fixReverse = false;
		public void SetTimeScale(float f){
			timeScale = f;
			_fixReverse = true;
		}
		
		public void ReverseToggle(){
			timeScale *= -1;
			_fixReverse = true;
		}
		
		
		
		protected void FixReverse(){
			if (!_fixReverse) return;
			_fixReverse = false;
			
			// after all updates the next index is incremented up one
			// without this helper method an event would trigger instantly next frame.
			
			// calling something the timer won't pass over makes no sense
			
			bool re = Mathf.Sign(timeScale) < 0f;
			if (reverse == re || timer.IsZero()) return;
			reverse = re;
			
			if ( re ) nextIndex --; 
			
			else nextIndex ++; 
			
		}
		
		/// <summary>
		/// this does not set the nextIndex but may be useful to set negative values via script
		/// </summary>
		public void TimerOverride(float newTime){
			
			timer = newTime;
		}
		/// <summary>
		/// this does not set the timer and will cause any events before the timer to trigger
		/// </summary>
		public void IndexOverride(int newIndex){
			
			nextIndex = newIndex;
		}
		
		protected override void OnUpdate () {
			
			// this can happen on start which is really annoying
			if (Time.unscaledDeltaTime > 1.1f) return;
			
			
			if (reverse)
				ReversedTime();
			else
				NaturalTime();
			
			FixReverse(); 
			
			
		}
		
		/// <summary>
		/// time moving backwards. Reversed.
		/// </summary>
		protected void ReversedTime() {
			timer = timer - Time.unscaledDeltaTime;
			
			// I iterate through every event I have passed, only those enabled are called.
			while (nextIndex >= 0 && timer < timedEvents[nextIndex].time)
			{
				naturalIndex = nextIndex;
				if (timedEvents[nextIndex].enabled)
				{
					if (timedEvents[nextIndex].reverseEvents != null) timedEvents[nextIndex].reverseEvents.Invoke();
				}
				
				
				if (naturalIndex == nextIndex)// no time leap. check if reverse was changed.
					naturalIndex = nextIndex--;
				
			}
			
			// I check if I'm past 0
			if (repeat_on && nextIndex < 0)
			{
				OnRepeat();
			}
		}
		
		/// <summary>
		/// time moving forwards. technically it doesn't need to but NaturalTime
		/// </summary>
		protected void NaturalTime() {
			
			timer = timer + Time.unscaledDeltaTime;
			
			// I iterate through every event I have passed, only those enabled are called.
			while (nextIndex < timedEvents.Length && timer >= timedEvents[nextIndex].time)
			{
				naturalIndex = nextIndex;
				if (timedEvents[nextIndex].enabled)
				{
					if (timedEvents[nextIndex].events != null) timedEvents[nextIndex].events.Invoke();
				}
				
				
				if (!reverse && naturalIndex == nextIndex)// no time leap.
					naturalIndex = nextIndex++;
			}
			
			// I check if I'm past the length of maxindex
			if (repeat_on && nextIndex >= timedEvents.Length)
			{
				OnRepeat();
			}
		}
		
		/// <summary>
		/// seeks between current time and t. Events and rewind events will trigger if animate is true.
		/// </summary>
		protected void SeekByTime(float t, bool animate = true){
			//1. seek is essentially less than a hundredth of a second away
			//2. time moves forwards
			//3. time moves backwards
			if (Mathf.Approximately(t, timer))
				return;
			
			else if (t > timer) // forward
			{
				timer = t;
				while (nextIndex < timedEvents.Length && timer >= timedEvents[nextIndex].time)
				{
					if (animate && timedEvents[nextIndex].enabled)
					{
						if (timedEvents[nextIndex].fastEvents != null) 
							timedEvents[nextIndex].fastEvents.Invoke();
						else if (timedEvents[nextIndex].events != null)
							timedEvents[nextIndex].events.Invoke();
							
					}
					
					nextIndex++;
				}
			
				
			}
			else if (t < timer) // back
			{
				timer = t;
				nextIndex -= 1;
				while (nextIndex >= 0 && timer < timedEvents[nextIndex].time)
				{
					if (animate && timedEvents[nextIndex].enabled)
					{
						if (timedEvents[nextIndex].rewindEvents != null) 
							timedEvents[nextIndex].rewindEvents.Invoke();
						else if (timedEvents[nextIndex].reverseEvents != null) 
							timedEvents[nextIndex].reverseEvents.Invoke();
					}
					nextIndex--;
				}
			
				nextIndex ++; // it either hits -1, or it is set just before the true next index.
			}
			
		}
		
		
		/// <summary>
		/// Limitation: timer only goes between 0 and the highest index. To explicity set timer, use the method TimerOverride.
		/// seeks between current time and time index of i. Events and rewind events will trigger.
		/// </summary>
		
		// fix, help, hack, incomplete
		protected void SeekByIndex(int i, bool animate = true){
			// this looks eassier than the time seek since I just iterate through the indices, then set the time.
			//1 index is the same
			//2. new index is greater (time moves forwards)
			//3. new index is less
			if (i == nextIndex) return;
			else if (i > nextIndex) // forward
			{
				while (nextIndex < timedEvents.Length && nextIndex <= i)
				{
					if (animate && timedEvents[nextIndex].enabled)
					{
						if (timedEvents[nextIndex].fastEvents != null) 
							timedEvents[nextIndex].fastEvents.Invoke();
						else if (timedEvents[nextIndex].events != null)
							timedEvents[nextIndex].events.Invoke();
					}
					nextIndex++;
				}
				SetByIndex(nextIndex); // set time
			
				
			}
			else // should be true if i < nextIndex
			{
				if (i < 0) i = 0; 
				nextIndex -= 1;
				while (nextIndex >= i)
				{
					if (animate && timedEvents[nextIndex].enabled)
					{
						if (timedEvents[nextIndex].rewindEvents != null) 
							timedEvents[nextIndex].rewindEvents.Invoke();
						else if (timedEvents[nextIndex].reverseEvents != null) 
							timedEvents[nextIndex].reverseEvents.Invoke();
					}
				}
				nextIndex ++; // it either hits -1, or it is set just before the true next index. In this case "i" is nextIndex.
				SetByIndex(nextIndex); // set time
			}
			
		}
		
		
		
		/// <summary>
		/// sets nextIndex to next index after time. No events will trigger, for that use SeekByIndex or SeekByTime.
		/// </summary>
		protected void SetByTime(float t){
			//1. timer is after events end
			//2. timer is within events
			// else it is 0
			if (t >= timedEvents[timedEvents.Length -1].time)
			{
				timer = 0f;
				nextIndex = 0;
			}
			else if (!t.IsZero()) 
			{
				int count = 0;
				foreach (TimerEvent e in timedEvents)
				{
					if (e.time >= t)
					{
						nextIndex = count;
						break;
					}
					count ++;
						
				}
			}
			else
			{
				timer = 0f;
				nextIndex = 0;
			}
		}
		
		/// <summary>
		/// SetByTime, reversed
		/// </summary>
		protected void SetByTimeReverse(float t){
			//1. timer is after events end
			//2. timer is within events
			// else it is 0
			if (t < -0.001f)
			{
				nextIndex = timedEvents.Length - 1;
				timer = timedEvents[timedEvents.Length -1].time;
			}
			else if (t < timedEvents[timedEvents.Length -1].time)
			{
				int count = timedEvents.Length -1;
				TimerEvent e ;
				while (count >= 0)
				{
					e = timedEvents[count];
					
					if (e.time < t)
					{
						nextIndex = count;
						break;
					}
					count --;
						
				}
			}
			else
			{
				nextIndex = timedEvents.Length - 1;
				timer = timedEvents[timedEvents.Length -1].time;
			}
		}
		
		
		/// <summary>
		/// sets the time to the time index of event i. No events will trigger, for that use SeekByIndex or SeekByTime.
		/// </summary>
	
		protected void SetByIndex(int i){
			// 1. index is greater than the highest index in timedEvents
			// 2. index is between 0 and maxIndex
			// else index is 0.
			
			if (i >= timedEvents.Length)
			{
				timer = 0f;
				nextIndex = 0;
			}
				
			else if (i > 0)
			{
				timer = timedEvents[i - 1].time;
				nextIndex = i;
			}
			else
			{
				timer = 0f;
				nextIndex = 0;
			}
			
		}
		
		/// <summary>
		/// SetByIndex, reversed
		/// </summary>
	
		protected void SetByIndexReverse(int i){
			// 1. index is greater than the highest index in timedEvents
			// 2. index is between 0 and maxIndex
			// else index is 0.
			
			if (i < 0) // basically start time = -1
			{
				timer = timedEvents[timedEvents.Length - 1].time;
				nextIndex = timedEvents.Length - 1;
			}
				
			else if (i < timedEvents.Length - 2)
			{
				timer = timedEvents[i + 1].time;
				nextIndex = i;
			}
			else
			{
				timer = timedEvents[timedEvents.Length - 1].time;
				nextIndex = timedEvents.Length - 1;
			}
			
		}
		
		
		/// <summary>
		/// determines if index i or time t is higher and sets the time. No events will trigger. 
		/// </summary>
		protected void SetByPoll(int i, float t){
			if (i > 0 && i <= timedEvents.Length && timedEvents[i - 1].time >= t)
			{
				SetByIndex(i);
			}
			else if (!t.IsZero()) // timer is a float
			{
				SetByTime(t);
			}
			else
			{
				timer = 0f;
				nextIndex = 0;
			}
			
		}
		
		/// <summary>
		/// determines if index i or time t is higher and sets the time. No events will trigger. 
		/// </summary>
		protected void SetByPollReverse(int i, float t){
			if (i >= 0 && i < timedEvents.Length && timedEvents[i + 1].time < t)
			{
				SetByIndexReverse(i);
			}
			else if (Mathf.Abs(t - timedEvents[timedEvents.Length - 1].time) < .001f) // timer is a float
			{
				SetByTimeReverse(t);
			}
			else
			{
				timer = timedEvents[timedEvents.Length - 1].time;
				nextIndex = timedEvents.Length - 1;
			}
			
		}
		
		protected bool detect_r = false;
		
		public void OnRepeat(){
			if (detect_r) 
			{
				Debug.Log(name + "recursion detected in poll, repeat values are too high. Repeat is now off.",gameObject);
				
				repeat_on = false;
				
				
				detect_r = false;
				return;
			}
			detect_r = true;
			// not quite reset.
			if (reverse)
				SetByPollReverse(repeatIndex, repeatTimer);
			else
				SetByPoll(repeatIndex, repeatTimer);
			
			detect_r = false;
		}
		
	}
}
