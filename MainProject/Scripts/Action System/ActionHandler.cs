using UnityEngine;
using System.Collections;

namespace ActionSystem
{
	
	public class ActionHandler : ActionComponent
	{
		
		public ActionDelegate onAction; // called during
		
		// interrogative text strings
		
		
		protected ActionManager manager;
		
		
		
		public string action
		{ 
			set { what = value; }
			get { return what; }
		}
		
		
		protected virtual void Start() { Init () ; }
		
		protected void Init () 
		{
			
			manager = ActionManager.instance;
			
			
		}
		
		protected void OnDisable()
		{
			manager.preRecord -= OnCollect;
		}
		protected void OnEnable()
		{
			manager.preRecord += OnCollect;
		}
		
		
		protected void OnCollect(ActionEventDetail data)
		{
			if (data.who != who) return;
			
			Write(data, 2); // over-write self
			
			
			
		}
		
	}
}
