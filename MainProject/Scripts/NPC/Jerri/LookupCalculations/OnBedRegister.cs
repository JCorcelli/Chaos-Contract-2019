using UnityEngine;
using System.Collections;
using NPCSystem;

namespace NPC.BTree.Jerri
{
	public class OnBedRegister : BeStateRegister {

	
		protected JerriBeStateHUB hub;
		protected JerriBedToFloor bf;
		protected void Start () {
			
			
			hub = GetComponentInParent<JerriBeStateHUB>();
			
			bf = GetComponent<JerriBedToFloor>();
			
		}
		
		
		protected override void OnUpdate () {
			
			
			if (bf.state != 0)
				current = true;
			else
				current = false;
		}
		
		protected ActiveStatesEnum state = ActiveStatesEnum.OnBed;
		protected override void Register() {
			hub.Add(state);
			
			
		}
		protected override void Unregister() {
			hub.Remove(state);
			
			
		}
		
	}
}