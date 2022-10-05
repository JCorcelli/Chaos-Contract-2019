
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

using Utility.GUI;
using SelectionSystem;



namespace DialogueSystem
{
	public class StaticDialogueHub : StaticHubConnect {
		// This needs to connect with some features from statichubconnect
		
		
		
		public List<DText> procText = new List<DText>()	;
		public List<DText> goalText	= new List<DText>() ; 
		public List<DText> streamedText = new List<DText>(); 
		public void AddStreamedText(DText d){
			
			streamedText.Add(d);
		}
		
		public static StaticDialogueHub instance;
		
		
		public override void CheckConnected(){
			if (!subscribed) SubscribeHub();
			
			Connect();
		}
		
		protected override void OnEnable( ){
			
			if (instance != null && instance != this){ 
				Destroy(this);
			
				return;
			}
			base.OnEnable();
			
			instance = this;
			CheckConnected();
			
			
		}
		
		
		protected override void OnConnect(object ob) {
		}
		
		public override void OnChange() {
			// behavior
			if (onChange != null) onChange();
		}
		

		protected override void OnUpdate() {
			DParser.stopwatch.Reset();
			DFilter.dead = false;
			for (int i = 0 ; i < DMutator.all.Count; i++)
				DMutator.all[i].Step();
			for (int i = 0 ; i < DStorage.all.Count; i++)
				DStorage.all[i].Step();
			for (int i = 0 ; i < DUser.all.Count; i++)
				DUser.all[i].Step();
			
		}
	}
}