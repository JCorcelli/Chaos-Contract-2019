using UnityEngine;
using System.Collections;


namespace ActionSystem
{

	/// <summary>
	/// 
	/// This is intended to be a go-between for realistic actions, effects, and bookkeepers
	///	only to be updated for extremely particular cases
	/// delegates to subscribers of  pre/on/post-Record 
	/// 
	/// preRecord - gather information, modifications to original action.
	/// onRecord - effects, if the game is playing
	/// postRecord - saving the info
	///
	/// static Submit / Effect accessors
	/// 
	/// </summary>

	public class ActionManager : MonoBehaviour
	{
		public bool mute = false;
		public bool pre = true;
		public bool isPlaying = true; // set false while loading.
		public bool post = false;
		public static ActionManager instance;
		
		
		public ActionDelegate preRecord; // anything that needs to gather & modify the data
		public ActionDelegate onRecord; // anything that will react
		public ActionDelegate postRecord; // anything recording or bookkeeping
		
		protected void Awake () { 
			if (ActionManager.instance == null)
				ActionManager.instance = this;
			else 
				GameObject.Destroy(this); 
			
			
			}




/*
Interrogative Message Headers
*	who			: Character
*	doing what	: Action
*	when		: Timestamp
*	where		: Location (eg here, in station)
*	why			: reasons  (eg : bumping into the wall, a routine about making cake )
*	how?		: NPC. items, knowledge, skill, emotions [,misc detail]
*	messages	: keywords / extra				

Action Manager Protocols 
- usually called by keyword actions
#	silent		: this is not recorded
#	empty		: there is no data
- called by macro actions ad-hoc (multiple times)
#	alter		: change a record
#	insert		: add records			
#	delete		: erase a record		
*/

		public static bool Submit( ActionEventDetail action) { 
			if (ActionManager.instance == null) 
				return false;
			
			ActionManager.instance.Record(action);
			return true;
		}
		
		public static bool SubmitEffect( ActionEventDetail action) { 
			if (ActionManager.instance == null) 
				return false;
			
			ActionManager.instance.Effect(action);
			return true;
		}
		public void Effect( ActionEventDetail action) {
			// transmits effect if playing, check mute.
			
			if (isPlaying && onRecord != null)
				onRecord( action ); 
		}
		
		public void Record( ActionEventDetail action ){
			
			if (mute) return; // not sure why this would be a thing
			
			if (pre && preRecord != null)
				preRecord( action ); 
			
			
			
			// actions that would be visible to player are called here.
			if (isPlaying && onRecord != null)
				onRecord( action ); 
			
				
			
			// bookkeepers should subscribe here
			if (post && postRecord != null)
				postRecord( action ); 
		}
				
		
		
		
	}

	
	
	
}