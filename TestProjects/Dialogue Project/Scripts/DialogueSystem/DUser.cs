
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

using Utility.GUI;
using SelectionSystem;



namespace DialogueSystem
{
	
	public class DUser {

		public static List<DUser> sceneUsers = new List<DUser>(); // very useful right now
		
		public static List<DUser> all = new List<DUser>(); // scene props
		
		
		public DLocalAction localAction = new DLocalAction();
		public DStream outStream; 
		public DStreamParser inStream; 
		public DMediaProc media = new DMediaProc(){mediaType = "User.inout"};

		public StaticMediaHubHook mediaHook;
		public DSensor sensor = DSensor.New("Default");
		
		public bool isReal = false;
		
			
		public StaticMediaHub mediaHub { 
		
			get{ return mediaHook.hub; }
			
		}
		public bool isFake { 
		
			get{ return !isReal; }
			set{ isReal = !value; }
		}
		public bool _inScene = false;
		public bool inScene 
		{
			get { return _inScene; }
			set
			{ 
				_inScene = value;
				
				if (!_inScene) 
					sceneUsers.Remove(this);
				else if (!sceneUsers.Contains(this))
					sceneUsers.Add(this);
			}
			
				
		}
		
		public static DUser selected ; // Default: This is the player
		
		public string name = "Unknown" ; // nickname?
		public Vector3 location = new Vector3(); // nickname?
		
		public DStorage processor ; // storage the user is about to access
		public DStorage storage ; // storage the user is about to access
		public List<DStorage> nstorage ; // storage the user is about to access
		
		
		
		
		public void MakeReal(){
			isReal = true;
			nstorage = new List<DStorage>();
			
			
		}
		public DUser(bool real = false, bool spawnInScene = false){
			isReal = real;
			inScene = spawnInScene;
			if (real) 
			{	
				MakeReal();
				
			}
			// even TV can access media
			storage = new DStorage();
			processor = new DStorage();
			outStream = new DStream();
			ConnectLibrary();
			if (all == null) return;
			all.Add(this);
		}
		public void OnDestroy(){
			
			
			all.Remove(this);
			
		}
		
		public static bool Select(string n){
			selected = DUser.Find(n);
			return (selected != null);
		}
		public static DUser Find(string n){
			
			foreach (DUser d in all)
			{
				if (d.name == n) return d;
			}
			
			return null;
		}
		
		protected virtual void ConnectLibrary(){
			mediaHook = new StaticMediaHubHook();
			
			mediaHook.onChange += UpdateLibrary;
			if (mediaHub != null) UpdateAllLibrary();
		}
		
		// this is for general knowledge updates, not entirely realistic
		protected virtual void UpdateAllLibrary(){
			List<DMediaLibrary> fromHub = mediaHub.libraries;
			for (int i = 0 ; i < fromHub.Count ; i++)
			{
				DownloadLibrary(fromHub[i]);
			}
		}
		protected virtual void UpdateLibrary(){
			Debug.Log("1");
			DMediaLibrary newLibrary = mediaHub.selected;
			if (newLibrary == null) return;
			
			
			DownloadLibrary(newLibrary);
		}
		 // make a new library ?
		protected virtual void UploadLibrary(){
			mediaHook.onChange -= UpdateLibrary;
			// upload actions
			mediaHook.onChange += UpdateLibrary;
			
		}
		protected virtual void DownloadLibrary(DMediaLibrary m){
			
			//if ( mediaHub != null) 
			//List<DMediaLibrary> d = mediaHub.hub.libraries;
			DText copy;
			List<DText> fromLib;
			for (int i = 0 ; i < m.library.Count ; i++)
			{
				fromLib = m.library[i].storedText;
				
				for (int ix = 0 ; ix < fromLib.Count ; ix++)
				{
					copy = fromLib[ix].Clone();
					storage.Store(copy);
				}
			}
			
		}
		
		// extend this for tool use
		public virtual bool Use<TUsable>(TUsable used)
		{
			if (used is DStorage) return Use(used as DStorage);
			
			return false;
		}
		public virtual bool Use(DStorage useStorage)
		{
			// this user created the text, somehow.
			// maybe this is where i should scan
			if (processor == useStorage || storage == useStorage || (nstorage != null && nstorage.Contains(useStorage)))
			{				
				return true;
			
			}
			if (storage.currentUser == this)
			{	
		
				storage.currentUser = null;
			}
			storage = useStorage;
			if (useStorage == null) return true;
			
			// if I can use the storage...
			useStorage.currentUser = this;
				
			
			return true;
		}
		
// ############################################
		// ANALOGUE SCAN
		
		
		public bool isAnalogue = false;
		// wires?
		// antennae?
		
		public bool isBiological = true;
// ############################################
		// BIOLOGICAL SCAN
		public bool ear			= true; // hears
		public bool clever		= true; // smart
		public bool eye			= true; // sees user
		public bool awake	 	= true; // is awake
		public bool psychic		= false; // is controlling
		public bool microbe		= true; // is a microbe
		public bool taster  	= true; // taste
		public bool supertaster	= true; // v. taste
		public bool nose  		= true; // nose
		public bool supernose   = true; // good nose
		public bool feel		= true; // /numb/
		public bool sexual 		= true; // vagina
		
		
		
		
		
// ############################################
		// SPIRIT SCAN
		public bool isSpirit = false; // can be mind controlled
		public bool senseGhosts = false; 
		public bool magical		= false; 
		
		public bool clairvoyant = false; // basically god mode, but only filters in things you want 
		
		public string creationDate;
		
		//age
		
		
		public bool numb = false;
		protected void Scan<T>(T target){
			sensor.Scan(target);
			
			//sensor.scanned //list
		}
		
		
		
		public virtual void Step(){
			// it's like the case with storage, something could update here
			if (sensor.scanned.Count > 0 && !sensor.media.running) sensor.Push();
			sensor.media.Step();
			media.Step();
			
		}
	}
	
}