using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace ActionSystem.Subscribers
{
	
	public class ListenForBunnyAction : ListenForAnyAction {
		
		
		public BunnyActionEnum action; // enumerator would make more sense
		
		protected override void OnAction(ActionEventDetail data) {
			if (data.what.ToLower().Replace(" ", "_") == ((BunnyActionEnum)action).ToString().ToLower())
				onAction(data);
		}
	}
}