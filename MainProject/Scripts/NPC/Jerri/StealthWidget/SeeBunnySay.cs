using UnityEngine;
using System.Collections;


using ActionSystem;

using Utility.Managers;

namespace NPCSystem
{
	public class SeeBunnySay : UpdateBehaviour {
		public float responseDelay = 0.5f;
		protected float timer = 0f;
		
		protected SeeBunny sb;
		
		protected bool cantSee = true;
		
		protected void Awake() {
			sb = GetComponent<SeeBunny>();
			timer = responseDelay;
		}
		protected override void OnLateUpdate () {
			if (timer <= 0)
			{
				Action();
			
			}
			else
				timer -= Time.deltaTime;
		}
		
		protected void Action()
		{
			if ( sb.hasVisibility && sb.st.visible && cantSee) 
			{
				Speak();
				cantSee = false;
				timer = responseDelay;
			}
			else if (!sb.st.visible)
				cantSee = true;
		
			
				
		}
		
		protected void Speak() {
			
			ActionManager.SubmitEffect(JerriActions.GetAction(JerriActions.say_bunny));
			ActionManager.SubmitEffect(JerriActions.GetAction(JerriActions.alert));
			
		}
		
	}
}