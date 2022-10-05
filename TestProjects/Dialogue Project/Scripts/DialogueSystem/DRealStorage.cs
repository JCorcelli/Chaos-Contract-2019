
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

using Utility.GUI;
using SelectionSystem;



namespace DialogueSystem
{
	// 1 object 1 storage
	public class DRealStorage : DScannable {
		// you can put things in it
		public DStorage storage = new DStorage();
		
		public TextAsset[] textFiles;
		
		// when someone's appearance is altered this should be altered

		//filter user
		public FilterHook filterHook;
		
		public DRealStorage(){
			
			AddDetail("Default", DefaultDetail);
			AddDetail("Skim", SkimDetail);
			
		}
		
		
		private DText DefaultDetail()
		{
			DText dt = new DText();
			dt.DAppend($"This {storage.name} is a {storage.storageType}");
			return dt;
		}
		private DText SkimDetail()
		{
			DText dt = new DText();
			
			int count = storage.storedText.Count;
			
			if (count > 0)
			{
				dt.DAppend($"{storage.name} contains ");
			
				dt.DAppend(count);
				dt.DAppend(" things.");
			}
			else 
				dt.DAppend($"{storage.name} is empty.");
			return dt;
		}
		
		protected override void OnEnable(){
			base.OnEnable();
			
			
			int len = textFiles.Length;
			DText t;
			DSource d;
			for (int i = 0 ; i < len ; i++)
			{
				d = new DSource();
				d.textAsset = textFiles[i];
				t = d.Spawn();
				if (d.origin == null) d.origin = storage;
				if (d.creator == null) d.creator = storage.creator;
				storage.Store(t);
				
			}
			
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
					storage.exe.Add(filters[i]);
				
			}
		}
	}
}