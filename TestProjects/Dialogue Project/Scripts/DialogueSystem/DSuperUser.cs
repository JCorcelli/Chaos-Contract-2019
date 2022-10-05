
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

using Utility.GUI;
using SelectionSystem;



namespace DialogueSystem
{

	public class DSuperUser : DScannable {
		/* 
			this uses the DUser class to store information about its aliases
		 
			this arranges and plugs various components in
		
		*/
		
		public float delay = .1f;
		public TextAsset textFile;
		public TextAsset[] textFiles;
		public DSource useSource; // something needs to make the source
		public DUser myUser;
		public DStorage processor
		{
			get { return myUser.processor; }
			set { myUser.processor = value; }
		}
		//filter user
		public FilterHook filterHook;
		
		public DisplayNodeParent display;
		
		public DStorage storage
		{
			get { return myUser.storage; }
			set { myUser.Use(value); }
		}
		public DStream outStream
		{
			get { return myUser.outStream ; }
			set { myUser.outStream = value; }
		}
		public DStreamParser inStream
		{
			get { return myUser.inStream; }
			set { myUser.inStream = value; }
		}
		public List<DFilter> filters = new List<DFilter>();
		
		public DLocalAction localAction{
				get {return myUser.localAction;}
		}
		public DMediaProc media{
				get {return myUser.media;}
		}
		public bool alive = false;
		protected virtual IEnumerator Start(){
			
			if (alive) yield break;
			yield return new WaitForSeconds(delay);
			alive = true;
			// If there were actions they'd be validated now
			base.OnEnable();
			
			SpawnUser();
			
			if (textFile != null)
				SpawnSource(); // basically, the params
			
			
		}
		
		protected override void OnEnable(){
			
			filterHook = new FilterHook();
			filterHook.onTryConnectFilter += AddFilter;
			filterHook.hub = GetComponentInParent<ConnectResource>();
			
			filterHook.Connect();
		}
		
		protected virtual void AddFilter(List<DFilter> filters) {
			// processor.filters.Add(f);
			for (int i = 0 ; i < filters.Count ; i++)
			{
				if (filters.Contains(filters[i])) continue;
				
				if (filters[i].type.EndsWith( 	".exe"))
					processor.exe.Add(filters[i]);
				else if (filters[i].type.EndsWith(".in"))
					inStream.filters.Add(filters[i]);
					
				else if (filters[i].type.EndsWith(".out"))
					outStream.redparser.filters.Add(filters[i]);
			}
		}
		
		protected virtual void SpawnSource()
		{
			// source better exist
			useSource = new DSource(); // future creep, source sets params
			
			
			useSource.textAsset = textFile;
			DText dt = useSource.Spawn();
			
			if (DSource.errors > 0) Debug.Log(" <size=24><color=blue>file " + textFile.name +" has "+DSource.errors+" errors</color></size>",textFile);
		
			
				
			storage.Store(dt);
			storage.Step();
		}
		
		protected virtual void SpawnUser()
		
		{
			DUser u = DUser.Find(name);
			if (u == null)
			{	
			
				myUser = new DUser();
				myUser.name = name;
				
				
				outStream =  new DStream();
				inStream = new DStreamParser(localAction);
				
			}
			else
				myUser = u;
			
		}
		
		
		protected override void OnLateUpdate()
		{
			
			if (inStream != null) 
			{
				
				inStream.Next();
				
				storage.Store(inStream.streamList);
			}
			
		}
		protected override void OnUpdate()
		{
			
			// carrying? storage.location = transform.position;
			myUser.location  = transform.position;
			myUser.media.Step();
			
			// there's a 'used' storage, and (pending) personal storage
			
			
			if (storage != null) 
			{
				storage.location = transform.position;
				
				
				// inStream.streamedText.EmptyStream(); // streamedText is garbage
				
				storage.Step();
				
				if (display != null && storage.storedText.Count > 0) {
					display.goalText = storage.storedText[0];
				}
			}
			
			if (outStream != null) outStream.Step();
			
		}
		
		
	}
	
}