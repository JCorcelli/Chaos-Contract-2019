
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using SelectionSystem;


namespace DialogueSystem
{
	
	public class VisualFilterResource : UpdateBehaviour  {
		// All filter types should be supplied information
		protected string[] keys = new string[]{
			"Video.in","Filter.out"
			};
		
		
		protected FilterHook filterHook = new FilterHook(){
			filters = new List<DFilter>()
		};
		 
		public DFilter GetFilter(string key){
			return filterHook.filters[IndexOf(key)];
		}
		protected int IndexOf(string key){
			return System.Array.IndexOf(keys, key);
		}
		protected void Awake(){
			
			int len = keys.Length;
			DFilter f;
			for (int i = 0 ; i < len ; i++)
			{
				f = DFilter.New(keys[i]);
				
				filterHook.filters.Add(f);
			}
			
			GetFilter("Video.in"  ).updateRequest += UpdateVideo;
			GetFilter("Filter.out").updateRequest += UpdateOut;
			
			filterHook.hub = GetComponentInParent<ConnectResource>();
			
			filterHook.Connect();
		}
		
		protected void UpdateVideo(DText t){ 
			GetFilter("Video.in"  );}
		protected void UpdateOut(DText t){   
			GetFilter("Filter.out");}
		
		protected override void OnUpdate(){
			
			GetFilter("Video.in"  );
			GetFilter("Filter.out");
		}
		
		
		
	}
}