using UnityEngine;
using System.Collections;
using ActionSystem;

namespace Utility.Managers
{
	
	///<summary>
	
	/// A hub that micromanages subscribers
	///</summary>
	public class EffectHUB : MonoBehaviour {

		public ActionDelegate onAction;
		
		///<summary>
		
		/// delegates event to all subscribers of onAction
		///</summary>
		public void Submit( ActionEventDetail data ) {
			if (onAction != null) onAction(data);
		}
		
		
		///<summary>
		
		/// delegates event to all subscribers of onAction
		
		///</summary>
		public void SubmitString( string data) {
			if (onAction == null) return;
			
			ActionEventDetail av = new ActionEventDetail( );
			av.what = data; onAction(av);
		}
		
		
	}
}