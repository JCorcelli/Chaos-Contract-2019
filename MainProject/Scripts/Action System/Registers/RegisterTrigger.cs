using UnityEngine;
using System.Collections;


namespace ActionSystem.Registers
{
	public class RegisterTrigger : RegisterDetails {

		// Use this for initialization
		// previous collider
		// current collider
		
		
		// Update is called once per frame
		public GameObject target;
		public string _targetName = "PresenceIndicator";
		public string _targetTag = "PlayerRig";
		
		void Awake() {
			if (target == null)
				target = gameObject.FindNameXTag(_targetName, _targetTag).gameObject;
			else
			{
				_targetName = target.name;
				_targetTag = target.tag;
			}
		}
		
		void OnTriggerEnter( Collider col ) {
			if (col.name != _targetName || col.tag != _targetTag) return;
			ActionEventDetail act = new ActionEventDetail();
			act. who 		= this.who;
			act.what		= this.what   ;
			act.when		= this.when   ;
			act.where		= this.where  ;
			act.why 		= this.why    ;
			act.how      	= this.how    ;
			act.message  	= this.message;
			
			
			ActionManager.Submit(act);
			
			
		}
		
		public string		exit_who 	= "";	// Who did that?
		public string		exit_what	= "";   // What happened?
		public string		exit_when	= "";   // When did it take place?
		public string		exit_where	= ""; 	// Where did it take place?
		public string		exit_why 	= "";	// Why did that happen?	
		public string		exit_how 	= "";	// extra: How did it happen?
		public string		exit_message = "";
		
		
		void OnTriggerExit( Collider col ) {
			if (col.name != _targetName || col.tag != _targetTag) return;
			ActionEventDetail act = new ActionEventDetail();
			act. who 		= this.exit_who;
			act.what		= this.exit_what   ;
			act.when		= this.exit_when   ;
			act.where		= this.exit_where  ;
			act.why 		= this.exit_why    ;
			act.how      	= this.exit_how    ;
			act.message  	= this.exit_message;
			
			
			ActionManager.Submit(act);
			
		}
		
	}
}