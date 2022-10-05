
using UnityEngine;

using UnityEngine.UI;

using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

using Utility.GUI;
using SelectionSystem;



namespace DialogueSystem
{

	[Serializable]
	public class DMediaLibrary  {
		
		// this will be a large collection of physical / digital storage treated as though they were selectively crafted banks of knowledge
		
		public DLibrarySource source ; // original stored text
		public TextCategory[] categories {get {return source != null ? source.categories : null;}} // original stored text
		protected List<DStorage> _library = new List<DStorage>(); // original stored text
		public List<DStorage> library {get{return _library;}set{_library= value;}} // original stored text
		
		protected bool init = false;
		protected string _mediaType = "Library"; 
		public string mediaType{
			get{ return _mediaType;}
			set{ _mediaType = value;}
		}
		public DUser creator;
		
		// could add some more variables to determine if someone would access this 
		
		
		public virtual void Build()
		{
			if (init || categories == null) return;
			
			init = true;
			
			
			
			DStorage storage;
			DText dt;
			DSource ds;
			TextCategory tc;
			TextAsset ta;
			
			int len = categories.Length;
			int len2;
			for (int i = 0 ; i < len ; i++)
			{
				
				tc = categories[i];
				if (tc == null) continue;
				// tc.name
				len2 = tc.t.Length;
				for (int ic = 0 ; ic < len2 ; ic++)
				{
					ta = tc.t[ic];
					if (ta == null) continue;
					
					ds = new DSource();
					ds.textAsset = ta;
					dt = ds.Spawn();
					storage = new DStorage(); // reminder to parse correct storage out of source somehow
					
					bool stored = false;
					if (ds.origin != null)
					for (int ix = 0 ; ix < library.Count ; ix++)
					{
						stored = (library[ix].name == ds.origin.name);
							
						if (stored) 
						{
							library[ix].Store(dt);
							break;
						}
					}
					if(!stored) storage.Store(dt);
					library.Add(storage);
				}
			}
			
		}
		
	}
}