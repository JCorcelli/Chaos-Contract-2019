
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

using Utility.GUI;
using SelectionSystem;


namespace DialogueSystem
{
	public partial class DSource  {
		// when Spawn() is called, the source parses and calls all its methods
		protected DLocalAction localAction ;
		
		
		protected void AddAllActions()
		{
			localAction = new DLocalAction();
			DAction.Use(localAction.a);
			DAction a;
			
			a = new DAction("source.start");
			a.Add(Qualify);
			
			a = new DAction("source.creator");
			a.Add(SetCreator);
			a = new DAction("source.origin");
			a.Add(SetOrigin);
			a = new DAction("source.end");
			a.Add(StopParser);
			
		}
		
		
		public void Qualify(string s){
			qualified = true;
		}
		public void StopParser(string s){
			parser.Stop();
		}
		
		public void SetCreator(string inName){
			//search for
			for (int i = 0 ; i < DUser.all.Count ; i++)
			{
				if (DUser.all[i].name == inName)
				{
					this.creator = DUser.all[i];
					return;
				}
			}
			//else
				
			this.creator = new DUser(false,false);
		}
		public void SetOrigin(string splitThis){
			//search for
			string[] vars = splitThis.Split(new Char[]{'\n'}, StringSplitOptions.RemoveEmptyEntries );
			
			int len = vars.Length;
			var dict = new Dictionary<string, string>();
			int comment ;
			for (int i = 0 ; i < len ; i++)
			{
				if (vars[i].Trim()[0] == '#') continue;
				
				comment = vars[i].IndexOf('#');
				if (comment >= 0)
				{
					vars[i] = vars[i].Substring(0, comment);
				}
				string[] x = vars[i].Split(new Char[]{'=',':'}, StringSplitOptions.RemoveEmptyEntries );
				
				if (x.Length > 1)
					dict[x[0].Trim()] = x[1].Trim();
				else if (x.Length > 1 && !dict.ContainsKey(x[0].Trim()))
					dict[x[0].Trim()] = null;
			}
			
			bool newOrigin = true;
			for (int i = 0 ; i < DStorage.all.Count ; i++)
			{
				if (DStorage.all[i].name == dict["name"])
				{
					this.origin = DStorage.all[i];
					newOrigin = false;
					break;
				}
			}
			//else
			DStorage o;
		
			string m;
			if (newOrigin)
			{
				o = this.origin = new DStorage();
				m = "name";
				if (dict.ContainsKey(m))
					o.name = dict[m] ;
			}
			else
				o = this.origin;
		
			
			
			 
			 
			m = "+origin";
			if (dict.ContainsKey(m))
			{
				DUser u = DUser.Find(dict["creator"]);
				if (u == null) 
				{
					u = new DUser(false, false);
					u.name = dict["creator"];
				}
				u.nstorage.Add(o) ;
			}
			
			m = "analogue";
			if (dict.ContainsKey(m) ) o.isAnalogue = bool.Parse(dict[m]) ; 

			m = "biological";
			if (dict.ContainsKey(m) ) o.isBiological = bool.Parse(dict[m]) ; 

			m = "spirit";
			if (dict.ContainsKey(m) ) o.isSpirit = bool.Parse(dict[m]) ; 

			m = "storageLimit";
			if (dict.ContainsKey(m) ) o.storageLimit = int.Parse(dict[m]) ; 

			m = "mutator";
			if (dict.ContainsKey(m) ) 
				o.mutator = DMutator.New(dict[m]) ; 
			else
				 o.mutator = new DMutator();
			m = "processor";
			if (dict.ContainsKey(m) )
				o.processor = DProcessor.New(dict[m]) ; 
			else
				 o.processor = new DOrganizer();
			m = "creationDate";
			if (dict.ContainsKey(m) ) 
				o.creationDate = dict[m] ; 
			
			m = "currentUser";
			if (dict.ContainsKey(m))  
			{
				DUser u = DUser.Find(dict[m]);
				if (u == null) 
				{
					u = new DUser(false, false);
					u.name = dict[m];
				}
				
				u.Use( o );
			}
			
			m = "lastUser";
			if (dict.ContainsKey(m) )
			{
				DUser u = DUser.Find(dict[m]);
				if (u == null) 
				{
					u = new DUser(false, false);
					u.name = dict[m];
				}
				
				u.Use( o );
			}
			
			m = "creator";
			if (dict.ContainsKey(m) ) 
			{
				DUser u = DUser.Find(dict[m]);
				if (u == null) 
				{
					u = new DUser(false, false);
					u.name = dict[m];
				}
				
				u.Use( o );
			}
			
			m = "+creator";
			if (dict.ContainsKey(m))
			{
				DUser u = DUser.Find(dict["creator"]);
				if (u == null) 
				{
					u = new DUser(false, false);
					u.name = dict["creator"];
				}
				u.nstorage.Add(o.Clone());
			}
			
			m = "storageType";
			if (dict.ContainsKey(m) ) o.storageType = dict[m] ; 
			m = "media";
			if (dict.ContainsKey(m) ) o.media = dict[m] ; 
			m = "location";
			if (dict.ContainsKey(m) ) 
			{
				string[] xyz = dict[m].Split(new Char[]{',',' ','x','y','z'}, StringSplitOptions.RemoveEmptyEntries );
				
				int px = int.Parse(xyz[0]);
				int py = int.Parse(xyz[1]);
				int pz = int.Parse(xyz[2]);
				o.location = new Vector3(px,py,pz); 
			}
			
			
			m = "+users";
			if (dict.ContainsKey(m) ) 
			{
				string[] allUsers = dict[m].Split(new Char[]{',','"',}, StringSplitOptions.RemoveEmptyEntries );
				
				int len2 = allUsers.Length;
				string sn;
				DUser u;
				for (int i = 0 ; i < len2 ; i++)
				{
					sn= allUsers[i].Trim();
					u = DUser.Find(sn);
					if ( u == null)
					{
						u = new DUser(false,false);
						u.name = sn;
					}
					u.nstorage.Add(o.Clone());
				}
			
			
				
			}
		}
	}
}