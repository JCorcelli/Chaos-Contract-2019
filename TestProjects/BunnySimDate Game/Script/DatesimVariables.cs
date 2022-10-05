using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SelectionSystem;
using Zone;
using Datesim.Setup;

namespace Datesim
{
	public delegate void DatesimDelegate();
	
	public class DatesimVariables: MonoBehaviour {
		
		public DatesimDelegate onChange;
		public Re_ConnectHubDelegate onConnect;
		
		public float realtime = 0f;
		public float datestep_time = 0f;
		
		public int level = 1; // reached
		public bool app_cleared = false; 
		public bool step6_triggered = false; 
		public bool step6_cleared = false; 
		
		// current date, setup
		public bool has_played = false;
		public string mode = "wait mode"; // wait, realtime, extra
		public string location = "";
		public string weekday = "";
		public string sky = "None";
		public bool event_day = false;
		public List<DatesimSetupAmbienceObject> refAmbienceList = new List<DatesimSetupAmbienceObject>();
		
		public bool undoAmbience =false;
		public bool dirtyAmbience =false;
		public int ambienceTotal = 0;
		public void SubmitAmbience(List<DatesimSetupAmbienceObject> newList)
		{
			refAmbienceList = newList;
			ambienceTotal = newList.Count;
			// maybe later this'll do something unusual.
			dirtyAmbience = true;
		
		
		}
		public string[] locationOptions = new string[]{}; // for string reference
		
		
		
		
		
		public string[] skyOptions = new string[]{"None"   ,
			"Night"   ,
			"Day"     ,
			"Sundown" ,
			"Sunrise" ,
			"Sunset"  ,
			"FullMoon",
			"White",
			"Aurora"};
			
		public string[] weekdayOptions = new string[]{"Monday"   ,
			"Tuesday"   ,
			"Wednesday"     ,
			"Thursday" ,
			"Friday" ,
			"Saturday"  ,
			"Sunday",
			"Who cares?"};
		
		
		// current date, common use
		public int stage = 1; // 0, off, n of 4
		public int response_stage = 0;
		public int effect = -1;
		public int option = -1;
		
		public int responses = 0;
		public float relation = 1.0f;
		public bool date_on = true; // started date
		public bool showEndStatus = false; // started date
		public bool newTopic = true; // started date
		public bool app_on = true; // started app on laptop
		public bool power_on = true; // laptop is on
		public string topic = "";
		public string minigame = "";
		public string ptitle = "none";
		public string pcomment = "";
		
		public bool proximity = false; // distance to 'physical touch'
		public bool inZone {
			get{return ZoneGlobal.inZone;} 
			set{ZoneGlobal.inZone = value; ZoneGlobal.OnChange();}
		}
			 // standard game zone
		public bool dateZone = true; // app visibility
		
		
		public virtual void OnChange() {
			if (onChange != null) onChange();
		}
		public virtual void Connect(Object ob) {
			if (onConnect != null) onConnect(ob);
		}
		public void StartApp(){
			CleanStats();
			stage = 0;
			newTopic = true;
			app_on = true;
			OnChange();
		}
		public void QuitApp(){
			// this shuts off the app
			realtime = 0f;
			datestep_time = 0f;
			mode = "wait mode"; // wait, timed, extra
			
			stage = 0; // 0, off, n of 4
			responses = 0;
			response_stage = 0;
			
			date_on = false;
			app_on = false;
			//power_on = false;
			topic = "";
			minigame = "";
			ptitle = "none";
			//proximity = false;
			//signalZone = false; 
			dateZone = false; 
			OnChange();
		}
		
		public bool clearAmbience = false;
		public void CleanAmbience(){
			
			clearAmbience = true;
			
		}
		

		public void CleanApp(){
			// this is for hard resetting
			realtime = 0f;
			datestep_time = 0f;
			level = 1;
			
			has_played = false;
			mode = "wait mode";
			location = "";
			weekday = "";
			event_day = false;
			CleanAmbience();
			
			stage = 0; // 0, off, n of 4
			responses = 0;
			response_stage = 0;
			relation = 1.0f;
			date_on = false;
			//app_on = false;
			//power_on = false;
			topic = "";
			minigame = "";
			ptitle = "none";
			
			proximity = false;
			dateZone = false; // this is app related
			OnChange();
		}
		public void CleanStats(){
			datestep_time = 0f;
			realtime = 0f;
			if (mode == "")
				mode = "wait mode";
			if (location == "")
				location = "alley";
			if (weekday == "")
				weekday = "monday";
			
			stage = 0; // 0, off, n of 4
			responses = 0;
			response_stage = 0;
			
			//topic = "";
			minigame = "";
			// relation = (float)level;
			
		}
		public void PowerOff(){
			power_on = false;
			QuitApp();
		}
		public void PowerOn() {
			power_on = true;
			OnChange();
		}
		public void StartDate() {
			CleanStats();
			date_on = true;
			stage = 1;
			response_stage = 0;
			OnChange();
			
		}
		public void SetDate(){
			// something either set these, or it will be set here?
			
			// location = "";
			// weekday = "";
			// event_day = false;
			// ambience  = 0;
			OnChange();
		}
		
		
		public void EraseData() {CleanApp();}
		
		public void Response(float amt = 0.13f) {
			
			if (response_stage >= stage) amt = 0;
			
			// wait mode only?
			
			relation += amt;
			
			if (relation < 0f)
			{
				relation = 1f;
				EndDate();
				
			}
			
			else if (relation < 1f) 
				relation = 1f;
			else if (relation > 3.99f)
				relation = 4f;
			
			WaitResponse();
			OnChange();
			
			if (response_stage < stage) response_stage = stage;
		}
		protected void WaitResponse(){
			
			if (response_stage < stage)
			
				responses ++;
			
			
		}
		
		public void Next() {
			if (response_stage < stage) response_stage = stage;
			
			stage ++;
			OnChange();
			
		}
		public void Back() {
			if (stage <= 1) return;
			stage --;
			
			OnChange();
		}
		
		public void EndDate() {
			// exit date normally
			newTopic = true;
			showEndStatus = true;
			SetStatus();
			stage = 0;
			response_stage = 0;
			date_on = false;
			//relation = (float)level;
			
			minigame = "";
			
		}
		
		public void SetStatus() {
			
			level = (int)relation; // can be overridden by bad stats
			pcomment = "";
			
			if (!has_played)
				FirstTimeStatus();
			else if (mode == "wait mode") 
				WaitModeStatus();
			else if (mode == "realtime")
				TimeModeStatus();
		
			// something else needs to read it? or is this going to be put into another thing?
		}
		
		public void FirstTimeStatus() {
			has_played = true;
			
				
			if (stage < 2)
			{
				level = 1;
				ptitle = "Why. End.End";
				pcomment = "Don't just quit, have fun.\n";
			}
			else if (stage < 4)
			{
				
				level = 1;
				ptitle = "Round.exe";
				pcomment = "You finished early, there are 4 stages.\n";
				
			}
				
				
			pcomment += "Real-time is available.";
		}
		public void WaitModeStatus() {
			// completed wait mode
				
			ptitle = "Quality.Bun";
			pcomment = "Wait-mode max is relation 4.";
				
			if (stage < 4)
			{
				ptitle = "Uncooperative Round.Thing";
				pcomment = "Finish your date slower. Get to stage 4.";
				return;
			}
			else if (relation > 3.99)
			{
				
				ptitle = "Bunny!Win";
				pcomment = "You did it! Take the real-time test.";
			}
			
			if (responses < 3)
			{
				ptitle = "Making progress";
				pcomment = "Try responding every chance for points.";
			}
			
		}
		public void TimeModeStatus() {
			
			if (stage < 2)
			{
				ptitle = "Do I know you?exe";
				pcomment = "Hai! Bai. You failed, try not to leave so Soon?";
				
			}
			else if (stage < 4)
			{
				ptitle = "Not even mad.nope";
				pcomment = "Just a hair.";
				
			}
			else if (responses < 5)
			{
				ptitle = "Friend who cares.icon";
				pcomment = "Are you stuffed?";
				
			}
			else if (relation < 4f)
			{
				ptitle = "Barely made it.tffs";
				pcomment = "You're almost there. Max the level. Don't get her angry.";
				
			}
			else
			{
				// random
				
				// Cupid of bunny land.
				ptitle = "Gold star.gift";
				pcomment = "I hope it's everything you wanted. There's more.";
				
				// start coroutine? Step 6
				step6_triggered = true;
			}
			
		}
		
	}
}