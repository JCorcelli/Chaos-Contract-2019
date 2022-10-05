
using UnityEngine;
using System.Collections;
using SelectionSystem;

namespace Zone
{
	
	public class ZoneHub : ConnectHub
	{
		// the connect hub sends messages, this can store variables.
		public ConnectHubDelegate onChange;
		
		public bool proximity = false; // distance to 'physical touch'
		public bool inFocus = false;
		
		public bool inZone {
			get {return ZoneGlobal.inZone;} 
			set {ZoneGlobal.inZone = value;
			GlobalChange();}
		} 
		public bool inInv {
			get {return ZoneGlobal.inInv;} 
			set {ZoneGlobal.inInv = value;
			GlobalChange();}
		} 
		public bool escMenu {
			get {return ZoneGlobal.escMenu;} 
			set {ZoneGlobal.escMenu = value;
			GlobalChange();}
		} 
		
		protected bool _connected = false;
		public void Awake(){
			ZoneGlobal.onChange += OnChange;
		}
		
		public void GlobalChange(){
			ZoneGlobal.OnChange();
		}
		public virtual void OnChange(){
			if (onChange != null) onChange();
		}
		public void OnDestroy() {
			ZoneGlobal.onChange -= onChange;
		}
	}
}