using UnityEngine;
using System.Collections;

namespace ActionSystem
{

	public delegate void ActionDelegate ( ActionEventDetail interrogativeData );
	//public class NPCStats
	
	
	public class ActionEventDetail 
	{
		// used to save the details, so the handler can be dismissed between locations
		// required for callback
		public string		who 	= "";	// Who did that?
		public string		what	= "";   // What happened?
		public string		when	= "";   // When did it take place?
		public string		where	= ""; 	// Where did it take place?
		public string		why 	= "";	// Why did that happen?	
		public string		how 	= "";	// extra: How did it happen?
		public string		message = "";
		
		
		public GameObject sender; 
		public GameObject receiver;
		/// <summary>
		/// default values
		/// </summary>
		
		public ActionEventDetail(){}
		public static implicit operator ActionEventDetail(string act){
			ActionEventDetail a = new ActionEventDetail();
			a.what = act;
			return a;
			
		}
		
		/// <summary>
		/// basically a copy of other
		/// </summary>
			
		public ActionEventDetail(ActionHandler a){
			who 	= a.who    ;
			what	= a.what   ;
			when	= a.when   ;
			where	= a.where  ;	
			why 	= a.why    ;
			how     = a.how    ;
			message = a.message;
			
			
			
		}
		
		public void Write(ActionEventDetail a, bool replace = false){
			
			if (replace){
				if (a.who    	!= "") who 		= a.who    ;
				if (a.what   	!= "") what		= a.what   ;
				if (a.when   	!= "") when		= a.when   ;
				if (a.where  	!= "") where	= a.where  ;	
				if (a.why    	!= "") why 		= a.why    ;
				if (a.how    	!= "") how      = a.how    ;
				if (a.message	!= "") message  = a.message;
			}
			else
			{
				if (a.who    	!= ""	&& who 		== "") who 	 = a.who    ;
				if (a.what   	!= ""	&& what		== "") what	 = a.what   ;
				if (a.when   	!= ""	&& when		== "") when	 = a.when   ;
				if (a.where  	!= ""	&& where	== "") where	= a.where  ;	
				if (a.why    	!= ""	&& why 		== "") why 		= a.why    ;
				if (a.how    	!= ""	&& how      == "") how     = a.how    ;
				if (a.message	!= ""	&& message  == "") message = a.message;
			}                   
			
			
			
		}
	}
	public class ActionComponent : MonoBehaviour
	{
		public string		who 	= "";	// Who did that?
		public string		what	= "";   // What happened?
		public string		when	= "";   // When did it take place?
		public string		where	= ""; 	// Where did it take place?
		public string		why 	= "";	// Why did that happen?	
		public string		how 	= "";	// extra: How did it happen?
		public string		message = "";
		
		
		
		
		public void Write(ActionEventDetail a, int replace = 0){
			
			if (replace > 1){
				who 		= a.who    ;
				what		= a.what   ;
				when		= a.when   ;
				where		= a.where  ;	
				why 		= a.why    ;
				how      	= a.how    ;
				message  	= a.message;
			}
			else if (replace > 0){
				if (a.who    	!= "") who 		= a.who    ;
				if (a.what   	!= "") what		= a.what   ;
				if (a.when   	!= "") when		= a.when   ;
				if (a.where  	!= "") where	= a.where  ;	
				if (a.why    	!= "") why 		= a.why    ;
				if (a.how    	!= "") how      = a.how    ;
				if (a.message	!= "") message  = a.message;
			}
			else
			{
				if (a.who    	!= ""	&& who 		== "") who 	 = a.who    ;
				if (a.what   	!= ""	&& what		== "") what	 = a.what   ;
				if (a.when   	!= ""	&& when		== "") when	 = a.when   ;
				if (a.where  	!= ""	&& where	== "") where	= a.where  ;	
				if (a.why    	!= ""	&& why 		== "") why 		= a.why    ;
				if (a.how    	!= ""	&& how      == "") how     = a.how    ;
				if (a.message	!= ""	&& message  == "") message = a.message;
			}                   
			
			
			
		}
		
	}
		
}