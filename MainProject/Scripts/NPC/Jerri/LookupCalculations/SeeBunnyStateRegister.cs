using UnityEngine;
using System.Collections;


using ActionSystem;

using Utility.Managers;
using NPCSystem;

namespace NPC.BTree.Jerri
{
	public class SeeBunnyStateRegister : UpdateBehaviour {
		
		
		protected SeeBunny sb;
		
		protected bool canSee = true;
		
		
		protected JerriBeStateHUB hub;
		
		protected void Awake() {
			sb = GetComponent<SeeBunny>();
			
			
			hub = GetComponentInParent<JerriBeStateHUB>();
			
		}
		
		protected void Start() {
			if (!canSee) 
				Unregister() ;
			else Register();
		}
		protected override void OnLateUpdate () {
			
			Action();
			
			
		}
		
		protected void Action()
		{
			if ( sb.hasVisibility && sb.st.visible ) 
			{
				if (!canSee)
				{
					Register();
					canSee = true;
				}
				// or do nothing
			}
			else if (canSee)
			{
				canSee = false;
				Unregister();
			}
			
		
			
				
		}
		
		
		protected void Register() {
			hub.Add(ActiveStatesEnum.SeeBunny);
			
			
		}
		protected void Unregister() {
			hub.Remove(ActiveStatesEnum.SeeBunny);
			
			
		}
		
	}
}