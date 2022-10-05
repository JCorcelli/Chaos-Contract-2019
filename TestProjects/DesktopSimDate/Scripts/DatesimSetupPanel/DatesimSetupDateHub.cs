using UnityEngine;
using System.Collections;

using System.Collections.Generic;

using SelectionSystem;
namespace Datesim.Setup
{
	public class DatesimSetupDateHub : DatesimDesktopConnectLite {
		// this will hold variables until i's connected properly?
		
		public DatesimDelegate onChange;
		
		
		
		//variables
		public int step = 1;
		public string location = "";
		public string weekday = "";
		public int intday = 0;
		public string sky = "";
		public bool event_day = false;
		public List<DatesimSetupAmbienceObject> ambienceList = new List<DatesimSetupAmbienceObject>();
		
		
		protected string[]_weekdayOptions = new string[]{"Monday"   ,
			"Tuesday"   ,
			"Wednesday"     ,
			"Thursday" ,
			"Friday" ,
			"Saturday"  ,
			"Sunday",
			"Who cares?"};
			
		public string[] weekdayOptions { 	get{return _weekdayOptions;}	
			protected set{}
		
		}
		protected int[] skyPermission = new int[]{1, 2, 4, 6, 8};
		
		protected string[] _skyOptions = new string[]{
			"Day"     ,
			"None"   ,
			"Sundown" ,
			"Sunrise" ,
			"Sunset"  ,
			"Night"   ,
			"Aurora",
			"FullMoon",
			"Space"};
		public string[] skyOptions { 	get{return _skyOptions;}	
			protected set{}
		
		}
		public Sprite[] bgList;
		protected void SetLocationOptions(){
			if (_locationOptions.Length > 0) return;
			
			_locationOptions = new string[bgList.Length];
			Sprite t;
			string current;
			for (int i = 0; i < bgList.Length; i ++) 
			{
				t = bgList[i];
				current = t.name.Replace("_"," ");
				_locationOptions[i] = current;
			}
			
		}
			
		protected string[] _locationOptions = new string[]{};
		public string[] locationOptions { 	get{return _locationOptions;}	
			set{_locationOptions = value;}
		
		}
		
		public Sprite[] ambienceOptions;
		
		protected int[] _weekdayAccess = new int[]{3,4,4, 7,7};
		
		public int[] weekdayAccess {protected set{}get{return _weekdayAccess;}}
		
		public int access = 1;
		protected bool _init = false;
		public void Init(){
			// so basically, if hook were the hook of reality
			if (_init || hook == null) return;
			hook.onChange -= OnChange;
			
			weekday 	= hook.weekday;
			for (int i = 0; i < weekdayOptions.Length; i ++)
			{
				if (weekday.ToLower() == weekdayOptions[i].ToLower())
				{
					intday = i;
					break;
				}
			}
			
			location 	= hook.location;
			event_day 	= hook.event_day;
			
			OnChange();
		}
		
		public void Revert()
		{
			vars.OnChange(); // hook loses all changes
			UndoAll();
			Preview();
			
		}
		protected void SubmitSetting()
		{
			
			hook.weekday = weekday;
			hook.event_day = event_day;
			hook.location = location;
			hook.sky = sky;
			hook.SubmitAmbience(ambienceList);
			vars.SubmitAmbience(ambienceList);
			
		}
		public void Submit()
		{
			SubmitSetting();
			
			hook.OnChange(); // hook is forced to copy hook
			hook.dirtyAmbience = false;
			vars.dirtyAmbience = false;
		}
		
		public void Preview()
		{
			OnChange(); // just this setting panel changes
			
		}
		protected override void OnChange()
		{
			
			// ambienceList? nah.
			if (event_day) weekday = "Vacation";
			if (onChange != null) onChange();
		}
		
		protected IEnumerator Gethub(){
			// reconnect
			while (vars == null)
				yield return new WaitForSeconds(.1f);
			
			Init();
		}
		protected override void OnEnable()
		{
			base.OnEnable();
			
			if (vars == null) 
				StartCoroutine("Gethub");
			else
				Init();
		}
		
		protected int _eventRelation = 3;
		public int eventRelation {get{return _eventRelation;}protected set{}}
		public void SetDayRandom(){
			
			event_day = false;
			
			if (vars.relation >= eventRelation) 
				intday = (int)Random.Range(0, 7.9f);
			else
				intday = (int)Random.Range(0, 6.9f); // never vacation
			
			event_day = intday > 6;
			if (event_day)
			{
				return;
			}
			
			intday = (int)Mathf.Repeat(intday, weekdayAccess[access]);
			
			if (weekdayOptions.Length  > 0) weekday = weekdayOptions[intday];
			
			
			
		}
		
		
		public void SetLocationRandom(){
			
			int randomsky = (int)Random.Range(0, skyPermission[access] +.9f);
			sky = skyOptions[randomsky];
			
			
			int randomLocation = (int)Random.Range(0, locationOptions.Length - .1f);
			location = locationOptions[randomLocation];
		}
		public void SetAmbienceRandom(){
			// idk if this will really help
		}
		
		public void SetAllRandom(){
			SetDayRandom();
			SetLocationRandom();
			SetAmbienceRandom();
		}
		
		public void UndoAll(){
			
			weekday = hook.weekday;
			event_day = hook.event_day;
			location = hook.location;
			sky = hook.sky;
			undoAmbience = true;
		}
		
		public string defaultLocation = "Tree";
		public string defaultSky = "Day";
		public bool clearAmbience = false;
		
		public bool undoAmbience = false;
		public void ClearAll(){
			weekday = "Monday";
			event_day = false;
			location = defaultLocation;
			sky = defaultSky;
			clearAmbience = true;
		}
		
	}
}
