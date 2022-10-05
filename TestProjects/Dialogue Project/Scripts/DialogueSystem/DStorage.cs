
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

using Utility.GUI;
using SelectionSystem;



namespace DialogueSystem
{

	public class DStorage {
		// This is what a text is held by. A physical brain, disk, or metaphysical part of the game.
		// creation data like the specific time, location, medium ... non user data. 
		
		// hypothetically
		// 
		
		public static Dictionary<string, DStorage> allTypes = new Dictionary<string, DStorage>(){
			["Default"] = new DStorage()
		};
		
		
		public static List<DStorage> all = new List<DStorage>();
		
		public bool isAnalogue = false;
		public bool isBiological = false;
		public bool isSpirit = false;
		public List<DFilter> exe {
			get => processor.filters;
			set => processor.filters = value;
		}
		
		// why list? hypothetically the text has a logical order, the result of a stream tends to be a list of single char strings, so in the early version they're concatenated
		public List<DText> storedText = new List<DText>();
		public int storageLimit = -1; // -1 inf
		
		
		public DMutator mutator = new DMutator();
		public DProcessor processor = new DOrganizer() as DProcessor;
		
		public string creationDate = DateTime.Now.ToString();
		
		public TimeSpan age
		{
			get{
				TimeSpan interval = DateTime.Now - DateTime.Parse(creationDate);
				return interval;
			}
		}
		
		// ACTIVE STATUS
		
		public DUser _currentUser ;
		public DUser currentUser {get => _currentUser; set {lastUser = _currentUser; _currentUser = value ;}}
		public DUser lastUser ;
		public DUser creator ;
		public string name = "None";
		
		// descriptive
		public string storageType = "Mind";
		public Vector3 location = new Vector3();
		public string media = "Grey matter"; // eg. what type of action might damage this media
		
		
		public DStorage(){
			if (all == null) return;
			all.Add(this);
			
		
		}
		
		
		
		// more: empty, remove?
		
		public bool Store(DText newText)
		{
			bool b = ( newText != null);

			if (b 
			&& newText.tstream < 0f) newText.tstream = Time.time;
			
			b = (b 
			&& (storageLimit < 0 || storedText.Count < storageLimit)
			&& (newText.storageType.Contains("Any"))
			|| newText.storageType.Contains(storageType));
			
			if (!b) return false;
			
			
			storedText.Add(newText);
			newText.isStored = true;
			if (mutator == null) mutator = new DMutator();
			mutator.Add(newText);
			return true;
		}
		public void Store( List<DText> newText)
		{
			
			for (int i = 0 ; i < newText.Count ; i++)
			{
				
				if (Store(newText[i]))
					newText.RemoveAt(i--);
			}
			
			
		}
		
		public virtual DStorage Clone(bool cloneContents = false){ 
			// so, be sure to replace this
			DStorage dm = Activator.CreateInstance(
			this.GetType()) as DStorage;
			dm.creationDate = this.creationDate;
			dm.storageType = this.storageType;
			
			dm.lastUser = this.lastUser;
			dm.creator =  this.creator ;
			dm.name =     this.name + all.Count;
			dm.mutator = this.mutator.Clone();
			dm.processor = this.processor.Clone();
			dm.media = this.media;

			dm.location = this.location;
			dm.isAnalogue = this.isAnalogue;
			dm.isBiological = this.isBiological;
			dm.isSpirit = this.isSpirit;
			dm.storageLimit = this.storageLimit;
			
			if (cloneContents)
			{
				for (int i = 0 ; i < storedText.Count ; i++)
				{
					dm.storedText.Add(storedText[i].Clone());
				}
			}
			return dm;
		}
		public virtual void Step()
		{
			
			
			processor.Load(storedText);
			processor.Exe();
			storedText = processor.output;
			processor.Eject();
			
			
		}
		
	}
	
}