
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using static GuiGame.GuiGameVars;

namespace GuiGame
{

	public class GuiBuildLaptop : MonoBehaviour{ 
		// this special case assumes a TextAsset is being compiled
		
		public TextAsset text;
		
		public  List<Transform> reparentList = new List<Transform>();
		public Transform newTransform {
			get => a_prefab[0];
		}
		public Utility.PrefabArray a_prefab;
		
		public List<Transform> reparent = new List<Transform>();
		public List<Transform> cloner = new List<Transform>();
		
		public GuiDict cloneContents = new GuiDict();
		public List<Transform> cloneContents_com = new List<Transform>();
		public List<Transform> unused_transform = new List<Transform>();
		
		public List<Transform> temp_transform = new List<Transform>();
		public List<Transform> d_transform = new List<Transform>();
		public Transform _selected;
		public Transform selected
		{
			get => _selected;
			set { _selectedDictCom = null;
				_selected = value;
			}	
		}
		public Transform rootTransform;
		
		public int replacementScope = 0;
		public int stackPos = 0;
		public GuiDict globalRep;
		public GuiDict globalDict;
		public GuiDict currentDict; 
		public string currentKey; 
		public GuiDict cloneDest; 

		public GuiDictComponent _selectedDictCom;
		public GuiDictComponent selectedDictCom{
			
			get{
				if (_selectedDictCom == null)
				{
					_selectedDictCom = selected.GetComponent<GuiDictComponent>();
					if (_selectedDictCom == null)
					{
						_selectedDictCom = selected.gameObject.AddComponent(typeof(GuiDictComponent)) as GuiDictComponent;
					}
				}
				return _selectedDictCom;
			}
		}
		public Canvas canvas => selected.GetComponentInParent<Canvas>();
		
		public Transform selectedPrefab;
		
		
		protected void Awake(){
			if (a_prefab == null)  {
				Debug.Log("add prefab array", gameObject);
				return;
			}
			Build();
		}
		
		public void Build(){
			
			if (text == null) {
				Debug.Log("text, where?", gameObject);
				return;
			}
			
			BParser guiParser = new BParser();
			
			guiParser.Load(text);
			
			BParser.current = guiParser;
			guiParser.Parse();
			
			
			// todo: make realtime instance control ?
			GuiDict dict = guiParser.globalVars.Root.parent;
			
			var keys = dict.Keys;
			selected = transform;
			
			NewTransform("OffGroup", transform);
			offGroup = selected;
			offGroup.gameObject.SetActive(false);
			// start
			selected = transform;
			
			if (dict != null && dict.Count > 0)
			foreach (string key in keys)
				BuildGlobal(dict[key], key);
			
			
			
			BParser.current = null;
		}
		
		public Transform offGroup;
		public Transform GetObjectFromDict(GuiDict path){
			BText s = path.GetComponentPath();
			
			
			
			int pos = 0;
			Transform t = rootTransform;
			char c = ' ';
			while (pos < s.Length && c!= '/')
				c = s[pos++];
			string name = s.ToString(pos, s.Length - pos);
			
			
			t = t.Find(name);
			// next try searching dict components?
			
			return t;
		}
		
		public Transform FindInTemp(string name, int count = 1)
		{
			Transform t;
			List<Transform> a_temp;

			int matchCount = 0;

			
			a_temp = temp_transform;
			
			for (int i = 0 ; i < a_temp.Count ; i++)
			{
				t = a_temp[i];
				
				
				if (t.name == name)
				{
					matchCount++;
					if (matchCount >= count)
						return t;
				}
			}
			
			
			return null;
		}
		public void ReparentBuild()
		{
			// completion
			
			
			
			GuiDict dest;
			GuiDict destList;
			Transform com_ob;
			
			// variable object, the best, all number above 1 get cloned
			for (int i = 0 ; i < cloneContents_com.Count ; i++)
			{
				com_ob = cloneContents_com[i];
				selected = com_ob;
				
				destList = selectedDictCom["apply_var"]; // should be just 1
				if (destList == null) {
					SetParent(com_ob, offGroup);
					continue;
				}
				
				for (int ii = 0 ; ii < destList.dict.Count ; ii++)
				{
					dest = destList[ii];
					if (dest == null) {
						continue;
					}
					
					Transform dt = FindInTemp(destList[0].key, ii+1);
					
					
					if (ii > 0)
					{	
						
						Transform n = NewInstance(com_ob, com_ob.parent);
						d_transform.Add(n);
						n.name = com_ob.name;
						selected = n;
							
							
							
						selectedDictCom.dict = new GuiDict();
						selectedDictCom.Add("apply_var",dest);
						reparentList.Add(n); 
					}
					else
					{
						reparentList.Add(com_ob); 
					}
					reparentList.Add(dt.parent);
					reparentList.Add(dt);
				
				}
			}
			cloneContents_com.Clear();
			foreach (Transform dt in cloneContents_com)
			{
				Destroy(dt.gameObject);
			}
			
			// components, original remains unless reparented
			for (int i = 0 ; i < cloner.Count ; i++)
			{
				com_ob = cloner[i];
				selected = com_ob;
				
				destList = selectedDictCom["apply_var"]; 
				
				
				if (destList == null) {
					SetParent(com_ob, offGroup);
					continue;
				}
				
				// different
				for (int ii = 0 ; ii < destList.dict.Count ; ii++)
				{
					dest = destList[ii];
					if (dest == null) {
						continue;
					}
					
				
					
					Transform dt = FindInTemp(destList[0].key, ii+1);
					
					
					
					Transform n = NewInstance(com_ob, com_ob.parent);
					d_transform.Add(n);
					n.name = com_ob.name;
					selected = n;
					
					selectedDictCom.dict = new GuiDict();
					selectedDictCom.Add("apply_var",dest);
					reparentList.Add(n); 
						
					
					reparentList.Add(dt.parent);
					reparentList.Add(dt);
				
				}
					
			}
			cloner.Clear();
			
			// object with parent variable, gets moved out of logical order to the parent's body
			for (int i = 0 ; i < reparent.Count ; i++)
			{
				com_ob = reparent[i];
				selected = com_ob;
				
				dest = selectedDictCom["apply_var"][0];
				if (dest == null) {
					SetParent(com_ob, offGroup);
					continue;
				}
				
				int indexer = dest.key.LastIndexOf('.');
				int count = 0;
				int result;
				string s;
				string destName = dest.key;
				
				// different
				if (indexer > 0)
				{
					s = dest.key.Substring(indexer+1);
					if (System.Int32.TryParse (s, out result))
					{	
						count = result;
						destName = dest.key.Substring(0,indexer);
					}
				}
				
				Transform dt = FindInTemp(destName, count+1);
				//Debug.Log(destName +","+ count+","+dt.name, dt.gameObject);
				reparentList.Add(com_ob); 
				reparentList.Add(dt.parent);
				reparentList.Add(dt);
				
				
				
			}
			reparent.Clear();
			
			// apply
			if (reparentList.Count > 0)
			{
				int i = 0;
				Transform t  ;
				Transform p  ;
				Transform sib;
				
				
				while (i < reparentList.Count)
				{
					t = reparentList[i++]; 
					p = reparentList[i++]; 
					SetParent(t,p);
					
				
					sib = reparentList[i++];
				
					t.SetSiblingIndex(sib.GetSiblingIndex()+1);
					
					selected = t;
					
					BuildFromWindowDef(  );
					
					//Destroy(sib.gameObject);
				}
			}
			reparentList.Clear();
			
			// erase
			//foreach (Transform dt in temp_transform)
			//{
			//	Destroy(dt.gameObject);
			//}
			//temp_transform.Clear();
		}
			
		public void FinishBuild()
		{
			ReparentBuild();
			
			cloneContents.Clear();
			int disabled = 0;
			foreach (Transform t in d_transform)
			{
				// also try setting them in h order
				selected = t;
				
				if (selectedDictCom.Contains("inVar")){
					Debug.Log("The variable component <color=red>"+t.name + "</color> is disabled.", t.gameObject);
					disabled ++;
				}
				
				else
					t.gameObject.SetActive(true);
				
			}
			
			foreach (Transform t in d_transform)
			{
				selected = t;
				BuildFromMods();
			}
			
			Debug.Log("<color=yellow>" +d_transform.Count +" objects instanced</color>, "+disabled+" disabled");
			d_transform.Clear();
		}
		
/*
WINDOW_ASCII 
REPLACEMENT 
VARIABLE_OBJECT 
COMPONENT_OBJECT
OPERATION 
ACTIVATOR 
ACTION 
METHOD 

ANON_OBJECT == VARIABLE_OBJECT
*/		
		//if HasReplacement
		//if isReplacement
		
		public GuiDict globalVars {
			get => globalDict[VARIABLE_OBJECT];
		}
		
		public GuiDict globalMethods {
			get => globalDict[METHOD];
		}
		
		protected bool isComponent = false;
		protected int inVar = 0;
		public bool cloneNextCom = false;
		
		
		public void KeyFunctions(){
			
		
		/*
			basic:
			I can copy and reparent everything after it's instanced
			
			special:
			contents are interpreted by whatever reads the variable later
		*/
			bool b = false;
			GuiDict v = currentDict[REPLACEMENT];
			GuiDict destList = new GuiDict();
			if (v != null && v.Contains(currentKey))
			{
					
				b = true;
			}
			else if (globalRep.Contains(currentKey))
			{
				v = globalRep;
				b = true;
			}
			
			if (!b) return;
		
			int i = 1;
			string istr = ""+(i++);
			string str;
			int len = 0;
			BText s = new BText();
			destList.Add(v[currentKey]);
			str = currentKey+"."+istr;
			s.Append(str);
			while ( v.Contains(str) )
			{	
			
				destList.Add(v[str]);
				
				len = istr.Length;
				s.Remove(s.Length - len, len);
				istr = ""+(i++);
				s.Append(istr);
				str = s.ToString();
			}
				
			destList = new GuiDict(destList);
			destList.key = "apply_var";
			currentDict.Add(destList);
			destList.parent = currentDict;
			
			cloneDest = destList;
				
			if (isComponent){
				
				cloner.Add(selected);
			}
			else
			{	
				// this is good for adding a name, or windows, I guess
				
				
				cloneContents.Add(currentDict);
				cloneNextCom = true;
			}
				
		}
	
			
			
			
			
		
		public void BuildFromVar(){
			
			
			GuiDict d = currentDict[currentKey];
			if (d == null || d.Count < 1) return;
			
			GuiDict save = currentDict;
			
			inVar ++;
			currentDict = d;
			KeyFunctions(); // sets cloneNextCom
			BuildDict();
			inVar --;
			currentDict = save;
			cloneNextCom = false; cloneDest = null;
		}
		public void BuildFromVars(){
			GuiDict d = currentDict[VARIABLE_OBJECT];
			if (d == null || d.Count < 1) return;
			
			if (currentDict.key == COMPONENT_OBJECT)
				replacementScope = stackPos;
			
			
			GuiDict save = currentDict;
			
			var keys = d.Keys;
			
			foreach(string key in keys)
			{
				currentDict = d;
				currentKey = key;
				
				
				BuildFromVar();
			}
			
			
			
			currentDict = save;
				
		}
		
		 
		 
		/* These are methods that alter the interpretation of code I wrote. But how am I implementing methods?
		
		I'd have to do it here right?
		*/
		public void BuildFromMethods(){
			GuiDict d = currentDict[METHOD];
			if (d == null || d.Count < 1) return;
			
			// filter components, anon don't appear in buildfrommod             
			
			if (currentDict.parent.key != ANON_OBJECT) return;
			/*
			GuiDict hier = currentDict.Hierarchy();
			
			for (int i = 0 ; i < hier.Count ; i++)
			{

					Debug.Log(hier.dict[i].GetPath().ToString());
			}	*/			
		}
		public void BuildFromWindow(){
			
			
			
			GuiDict g = currentDict.FindAll(WINDOW_ASCII);
			
			
			if (g == null || g.Count < 1) return;
			
			GuiDict d;
			GuiDict hub;// = currentDict[REPLACEMENT];
			GuiDict workingDict;
			string workingKey   ;
			string renameKey ;
			bool checkPrefab;
			for (int i = 0 ; i < g.Count ; i++)
			{
				d = g[i];
				
				
				//todo check for window name?
				AddPrefab("Window",WINDOW_ASCII); 
				selectedDictCom.dict = d;
						
				checkPrefab = false;
				
				hub = d[REPLACEMENT];
				
				BuildFromWindowDef();
				for (int ii = 0 ; ii < d.Count ; ii++)
				{
					workingDict = d[ii];
					workingKey = workingDict.key;
					
					//todo, cleaning this would take effort (2)
					
					if (workingKey == "name" || workingKey == "start" || workingKey == "apply_var" || workingKey == "mods" ) continue;
					if (workingDict.Contains("name"))
						workingKey = workingDict["name"][0].key;
					
					renameKey = workingKey;
					if (hub == null || hub[workingKey] == null || !hub[workingKey].Contains("rename"))
						checkPrefab = true;
					else
					{	
						renameKey = hub[workingKey]["rename"][0].key;
						if (hub[renameKey] == null)
							checkPrefab = true;
					}
					
					// todo, blank renamed transforms must be skipped here
					if (renameKey == "") continue;
					// either will push
					if (checkPrefab && a_prefab.Contains(renameKey))
					{
						// this needs to be more specific than findintemp
						
						// this is where things are cloned, but lack dict connection (ie calculator)
						
						AddPrefab(renameKey);
						
						// todo name hack (2)
						// reconnect
						d.Include("__include");
						selectedDictCom.dict = d.FindLast("__include") ;
						// todo deprecated from build
						selectedDictCom.Add("apply_var", workingDict);
						
						// add dict component?
						BuildFromWindowDef();
						
					
					}
					else 
					
						AddTemp("Window",renameKey);
					
					
					Pull();
					
				}
				
				
				
				Pull();
				
			
			}
				
			
		}
		
		// This builds from component Mods
		public void BuildFromMods(){
			GuiMod interpreter = selected.GetComponent<GuiMod>();
			
			if (interpreter != null)
			{
				// use custom rules
				interpreter.Run(selected, selectedDictCom.dict);
				
			}
			else
			{
				GuiRule.RunStatic(selected, selectedDictCom.dict);
				
			}
		}
		public void BuildFromWindowDef(){
			GuiMod interpreter = selected.GetComponent<GuiMod>();
			
			if (interpreter != null)
			{
				// use custom rules
				interpreter.BuildWindow(selected, selectedDictCom.dict);
				
			}
			else
			{
				GuiRule.BuildWindowStatic(selected, selectedDictCom.dict);
				
			}
			
			
		}
		public BText stackText = new BText();
		
		// this gets the correct prefab
		public void BuildFromComponent(){
			GuiDict d = currentDict[COMPONENT_OBJECT];
			if (d == null || d.Count < 1) return;
			
			GuiDict save = currentDict;
			Transform saveTransform = selected;
			
			
			var keys = d.Keys;
			
			
			string componentType;
			//Dictionary<string, GuiGame.GuiDict>.KeyCollection componentNames;
			bool cloneAll = cloneNextCom;
			GuiDict saveDest = cloneDest;
			foreach(string type_key in keys)
			{
				componentType = type_key; // so there can be multiple of a single type
				
				
				var dicts = d[type_key].dict;
				foreach(GuiDict g in dicts)
				{
					currentDict = g;
					currentKey = g.key;
					
					cloneDest = saveDest;
					cloneNextCom = cloneAll; // this will allow insertion of a list, without changing the inner variables
					AddPrefab(type_key, currentKey);
					selectedDictCom.dict = currentDict;


					if (!cloneNextCom)
					isComponent = true;
					
					KeyFunctions(); // allow copy without reparenting
					isComponent = false;
					
					BuildSelected();
					Pull();
					
					selected = saveTransform;
				}
				currentDict = save;
			}	
			
		}
		public void BuildSelected(){
			
			
			if (currentDict == null) {
				return;
			}
			selectedDictCom.dict = currentDict;
			
			if (cloneNextCom ){
				selectedDictCom.Add( cloneDest);
				cloneContents_com.Add(selected);
				
				
				cloneNextCom = false;
			}
			//else if (inVar > 0)
			//{
			//	selectedDictCom.Add("inVar");
			//}
			
			GuiDict vo = currentDict[VARIABLE_OBJECT];
			if ( vo != null && vo.Contains("parent")) {
				
				
				currentDict.Add("apply_var");
				reparent.Add(selected);

				GuiDict v = currentDict[REPLACEMENT];
				
				
				
				string pKey = vo["parent"][0].key;
				// reparent multiple times? with some changes here
				if (v != null && v.Contains(pKey))
				{
					GuiDict dest = v[pKey][0];
					currentDict.Add("apply_var", dest);
					
				}
				else if (globalRep.Contains(pKey))
				{
					
					GuiDict dest = globalRep[pKey][0];
					currentDict.Add("apply_var", dest);
				}
				
			}
			BuildDict();
		}
		
		public void BuildDict()
		{
			// vars must be resolved first
			BuildFromVars();
			
			// certain methods might be called
			
			BuildFromMethods();
			// A component may be inserted into a window
			
			BuildFromWindow();
			
			BuildFromComponent();
			
		}
		
		public void BuildGlobal(GuiDict dict, string name)
		{
			NewTransform( name, transform);
			selected.gameObject.SetActive(true);
			
			currentDict = globalDict = new GuiDict(dict);
			globalRep = globalDict[REPLACEMENT];
			
			if (globalRep == null) globalRep = new GuiDict();
			
			temp_transform = new List<Transform>();
			rootTransform = selected;
			// now do what the parser did not do. make things.
			BuildSelected();
			
			FinishBuild();
			
			Debug.Log(globalDict.DebugHierarchy());
		}
		
		public void SetParent(Transform child, Transform parent)
		{
			if (child.transform is RectTransform)
				child.SetParent(parent, false);
			else
				child.SetParent(parent);
			
			
		}
		
		
		public Transform NewInstance(Transform ob, Transform parent){
			if (selectedPrefab.transform is RectTransform)
				return Instantiate(ob, parent, false);
			else
				return Instantiate(ob, parent);
		}
		public void Push(){
			// Guess I'm just going to set them active logically after it's all done
			selectedPrefab.gameObject.SetActive(false);
			
			
			selected = NewInstance(selectedPrefab, selected);
			
			stackPos++;
			
		}
		public void Pull(){
			selected = selected.parent;
			stackPos--;
			
			if ( stackPos < replacementScope)
			
				ReparentBuild();
				
			
		}
		
		
		
		public void Select(string s){ 
			for (int i = 0 ; i < d_transform.Count ; i++)
			{
				if (d_transform[i].name == s){
					selected = d_transform[i]; 
					return;
				}
			}
		}
		
		public void SelectPrefab(string s){
			
			selectedPrefab = a_prefab.GetPrefab(s);
			
			Push();
		}
		
		// This is only called at the start and end, otherwise it will break stack position
		public void NewTransform(string newName, Transform parent){
			selectedPrefab = newTransform;
			selected = NewInstance(selectedPrefab, selected);
				
			selected.name = newName;
			
			
			selectedDictCom.dict.key = selected.name;
		}
		public void AddPrefab(string prefabName, string newName = ""){
			SelectPrefab(prefabName);
			
			
			if (newName != "")
				selected.name = newName;
			else
				selected.name = prefabName;
			
			
			d_transform.Add(selected);
			
			selectedDictCom.dict.key = selected.name;
			
		}
		public void AddTemp(string prefabName, string newName){
			SelectPrefab(prefabName);
			
			selected.name = newName;
			
			
			GuiDict v = currentDict[REPLACEMENT];
			if (v != null && v.Contains(newName))
			{
				
				temp_transform.Add(selected);
			}
			else if (globalRep.Contains(newName))
			{
				
				temp_transform.Add(selected);
			}
			else
				unused_transform.Add(selected);
			selectedDictCom.dict.key = selected.name;
		}
		
	}
	
}