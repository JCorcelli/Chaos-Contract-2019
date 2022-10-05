using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GuiGame;
using MEC;

namespace BehaviorTree
{
	public class BTLambda : BTEmpty {


		public GuiDict action;
		public GuiDict message;
		public GuiDict lambda;
		public BTRule rule;
		public float delay = 0f;
		protected override void OnEnable(){
			EnableChildren();
			
		}
		protected override void OnDisable(){
			DisableChildren();
			
		}
		// this will probably be a coroutine
		protected void RunLambda(string s) {
			if (message != null)
			{
				message.SetVar(s);
				message.SendUpdate();
			}
			
			if (lambda != null)
				rule.UseLambda(lambda[s]);
			
			// alternative that runs every method
			//rule.RunAllLambda(lambda);
			
			DoActions();
			
			
			
		}
		public void DoActions(){ 
			if (action == null) return;
			StartCoroutine(_DoActions);
		}
		public IEnumerator<float> _DoActions(){ 
			for (int i = 0 ; i < action.Count ; i++)
			{
				DoAction(action[i].key);
				yield return 0; // force at least one frame
			}
		}
		public void DoAction(string key)
		{ 
		    switch (key)
			{
				//case "hi" : Debug.Log(key);break;
				case "success": Succeed();break;
				case "failure": Failure();break;
				case "repeat" : Repeat();break;
				case "message": 
					message.SendUpdate();break;
					
			};
			action.SendUpdate();
		}
		
		protected void Repeat(){
			if (activeInHierarchy)
				StartCoroutine(_Repeat);
		}
			
		protected IEnumerator<float> _Repeat(){
			DisableChildren();
			yield return 0; // force at least one frame
			yield return Timing.WaitForSeconds(delay);
			EnableChildren();
			
		}
		// actually this is going to have to generate a leaf node based on what it says
		//public delegate void LeafDelegate();
		//protected static Dictionary<string, LeafDelegate> filter = new Dictionary<string, LeafDelegate>(){
		//	["repeat"] = Repeat,
		//	["success"] = OnSuccess,
		//	["failure"] = OnFailure
		//};
		protected override void OnSuccess() {
			// receive success
			RunLambda("success");
			
			if (action == null) Succeed();
			
		}
		protected override void OnFailure() {
			// receive failure
			RunLambda("failure");
			
			if (action == null) Failure();
		}
		
		
		
	}
}