
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


namespace GuiGame
{

	public class GuiBuild : MonoBehaviour{ 
	
		public TextAsset text;
		
		public Transform newTransform {
			get => a_prefab[0];
		}
		public Utility.PrefabArray a_prefab;
		
		public Dictionary<string, Transform> d_transform = new Dictionary<string, Transform>();
		public Transform selected;
		public Transform selectedPrefab;
		public Component selectedComponent;
		
		protected void Awake(){
			if (a_prefab == null) return;
			Build();
		}
		
		public void Build(){
			
			if (text == null) {
				Debug.Log("text, where?", gameObject);
				return;
			}
			BParser guiParser = new BParser();
			guiParser.Load(text);
			guiParser.Parse();
			
			// make Interface
			GuiDict d = GuiGameVars.game["Interface"];
			for (int i = 0 ; i < d.Count ; i++)
			{
				Build(d[i].key, d[i]);
			}
			
		}
		public void Build(string name, GuiDict dict){
			AddGroup( name, transform);
			
		}
		public void Select(string s){ 
		if (d_transform.ContainsKey(s))
			selected = d_transform[s];
		else
			Debug.LogError(s+" doesn't exist, but an attempt to retrieve it was made.");
		}
		
		public void SelectPrefab(string s){
			
			selectedPrefab = a_prefab.GetPrefab(s);
			if (selectedPrefab == null) selectedPrefab = newTransform;
		}
		public void AddGroup(string newName, Transform parent){
			selectedPrefab = newTransform;
			AddPrefab(newName, parent);
		}
		public void AddPrefab(string prefabName, string newName, Transform parent){
			SelectPrefab(prefabName);
			AddPrefab(newName, parent);
		}
		public void AddPrefab(string newName, Transform parent){
			selected = Instantiate(selectedPrefab, parent);
			selected.name = newName;
			d_transform.Add(newName, selected);
		}
		
	}
	
}