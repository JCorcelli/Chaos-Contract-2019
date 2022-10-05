
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

using Utility.GUI;
using SelectionSystem;



namespace DialogueSystem 
{
	
	public class DStream : StaticDialogueHubHook {
		// This hooks to a static hub storing lists
		// This carries text like a medium
		
		
		public DText procText ; // parent text
		public DText goalText ; // a clone, mutated
		
		public DMediaProc media = new DMediaProc(){mediaType = "Stream.out"}; // a simplified sender
		public DRedParser redparser; // analogous to lag and physics
		
		
		public DStream():base()
		{
			
			Init();
			
			
		}
		
		public virtual void Cps (float newCps){redparser.cps = newCps;} // Set Speed
		protected void SetCps(string var){
			float x = float.Parse(var);
			redparser.cps = x;
			redparser.charAdvance = 0f;
		}
		
		
		
		protected DLocalAction localAction;
		
		
		public virtual void Step(){
			
			if (redparser != null && redparser.running)
				redparser.Next();
			if (media != null && media.running)
				media.Step();
		}
		
		
		protected virtual void Init(){
			if (localAction == null)
			{
				localAction = new DLocalAction();
				
				DAction.Use(localAction.a);
				DAction action =new DAction("cps") ;
				
				action.Add(SetCps);
			
			}
			if (redparser == null)
			{
				
				redparser = new DRedParser(localAction);
				
			}
		}
		
		public virtual bool Proc( DText t ){
			// I guess I'm getting first crack at it.
			
			procText = t;
			goalText = t.Clone();
			
			// Mutation: 1, creating and setting goal text
			
			
			

			hub.procText.Add( t   );
			hub.goalText.Add( goalText);
			
			redparser.procText = procText ;
			redparser.goalText = goalText ;
			
			redparser.Load();
			

			hub.OnChange();
			
			
			// this class checks next frame. If something kills the message, oh well.
			return true;
		}
		
		//onconnect
	}
	
}