using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

using Utility.GUI;
using SelectionSystem;
using Zone;

namespace Datesim
{
	public class DatesimDesktopHook : DatesimVariables {
		// Variables slave
		// a hub and a hook. hvuwa
		
		public bool connected = false;
		public bool subscribed = false;
		
		public TaskbarListener task;
		public DatesimVariables vars;
		
		
		public virtual void CheckConnected(){
			if (!subscribed) SubscribeHub();
			
			connected = (task != null && vars != null);
			
			if (connected) return;
			//if (_hook) 
			StaticHub.Ping();
		}
		
	
		protected virtual void OnEnable( ){
			
			
			CheckConnected();
			
			
			calling = false;
		}
		
		
		public virtual void UnsubscribeHub(){
			subscribed = false;
			
			StaticHub.onConnect -= OnConnect;
			
		}
		
		public virtual void SubscribeHub(){
			subscribed = true;
			
			UnsubscribeHub();
			StaticHub.onConnect += OnConnect;
			
		}
		protected virtual void OnConnect(object ob) {
			// for hooks
			if (ob.GetType() == typeof(DatesimVariablesListener) )
			{
				vars = ((DatesimVariablesListener)ob).vars;
				
				
				
				vars.onChange -= OnVarsChange;
				vars.onChange += OnVarsChange;
				
			
				
				OnVarsChange();
				
			}
			else if (ob.GetType() == typeof(TaskbarListener) )
			{
				// situational
				task = ((TaskbarListener)ob);
				task.connected = connected = true;
				
				
				
				task.onChange -= OnTask;
				task.onChange += OnTask;
				
				OnTask();
				
			}
		}
		
		
		
		protected bool calling = false;
		public virtual void OnTask() {
			// called by the task
			if (calling) return;
			calling = true;
			app_on = task.running;
			if (vars != null) 
			{
				if (app_on != vars.app_on)
				{
					if (app_on)
						vars.StartApp();
					else
					{
						vars.QuitApp();
					}
				}
				vars.app_on = app_on ;
			}
			if (onChange != null) onChange();
			calling = false;
		}
		
		
		public void Sync(DatesimVariables b, DatesimVariables a){
			// this is for hard resetting
			
			// vars. ... = ...;
			b.realtime  = a.realtime ;
			b.datestep_time  = a.datestep_time ;
			b.level  = a.level ;
		
			b.has_played  = a.has_played ;
			b.mode  = a.mode ;
			b.sky  = a.sky ;
			b.location  = a.location ;
			b.weekday  = a.weekday ;
			b.event_day  = a.event_day ;
			
		
			b.stage  = a.stage ; // 0, off, n of 4
			b.responses  = a.responses ;
			b.option  = a.option ;
			b.effect = a.effect ;
			b.response_stage  = a.response_stage ;
			b.relation  = a.relation ;
			b.date_on  = a.date_on ;
			b.app_on  = a.app_on ;
			b.power_on  = a.power_on ;
			b.topic  = a.topic ;
			b.minigame  = a.minigame ;
			b.ptitle  = a.ptitle ;
		
			b.proximity  = a.proximity ;
			b.dateZone  = a.dateZone ; // this is app related
			
		}
		public override void OnChange(){
			// guess this is where I update everything in variables. maybe make a sync
			
			if (calling) return;
			calling = true;
			// reflection... maybe
			Sync(vars, this);
			
			
			// the other way, this is messy
			
			base.OnChange();
			vars.OnChange();
			
			// or vars.clearFlags
			
			NoBounce();
		}
		
		protected void NoBounce(){
			
			StartCoroutine("_NoBounce");
		}
		protected IEnumerator _NoBounce(){
			yield return null;
			calling = false;
		}
		public virtual void OnVarsChange() {
			// in this I check everything that matters
			
			if (calling) return;
			calling = true;
			Sync(this, vars);
			if (task != null) 
			{
				
				if (app_on != task.running)
				{
					if (app_on)
						task.StartExe();
					else
					{
						task.message = "close";
						task.OnChange();
					}
				}
			}
			
			if (onChange != null) onChange();
			NoBounce();
		}
		

	}
}