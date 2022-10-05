using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;

namespace Dungeon.Save
{
	
	
	[System.Serializable]
	public class ExitS : Spawnable {
		// an Item Save/Spawn. contains information about an unspawned prefab.
		// Parameters, and if no position specified, the position
		
		
		//META, debug, dictionary, list
		
		public string savedName = "ExitSpawner";
		
		public string GetName() { return prefabName; }
		public string GetSavedName() { return savedName; }
		public void SetSavedName(string s) { savedName = s; }
		
		
		public const string prefabName = "ExitS"; // I suppose I might want a list of prefabs?
		
		[System.NonSerialized]
		public static UnityEngine.Transform prefab = UnityEngine.Resources.Load(DungeonVars.resourceFolder+"Exit", typeof (Transform)) as Transform;
		
		
		protected bool isSpawned = false; // debug, check if I'm double spawning. check if I somehow spawned it twice as an item
		
		
		
		//TRANSFORM (req. instantiation)
		
		public SaveV3 position = new SaveV3();
		public SaveV3 rotation = new SaveV3();
		// public Vector3 scale   ;
		
		
		public bool moved = false; // DEPRECATED
		
		public void Spawn(Vector3 newPos) {
			// alternative to setting vars, then calling Spawn().
			position = new SaveV3(newPos);
			
			Spawn();
		}
		//onload
		
		[System.NonSerialized]
		protected UnityEngine.Transform transform;
		
		public void Spawn() {
			// generic spawn, loads object and parents to root
			
			transform = UnityEngine.Object.Instantiate(prefab) as Transform;
			
			//comp = transform.AddComponent<DungeonSave.OnDestroy>();
			//comp.target = this;
			
			
			isSpawned = true;
			
			SetTransform ();
			AddTitle	();
			SetExit     ();
			RenameExit2 ();
			
			
		}
		public void Save() {
			// saves directly to dungeon's level
			
			
			DungeonSave file = DungeonSave.instance;
			file.current.GetLevel(DungeonVars.level).Add(this); // add this script (not this object)
			
		}
		protected void TransformChange() {
			// easily sync the transform and position
			
			position = new SaveV3(transform.position);
			
			rotation = new SaveV3( transform.eulerAngles);
		}
		
		protected void SetTransform() {
			
			transform.position = position.ToVector3();
			transform.rotation = Quaternion.Euler(rotation.ToVector3());
		}
		public void Despawn() {
			// I can add an OnDestroy that references this to the transform I spawn (each time I load).
			
			// I'd need to reference this from the original transform, somehow, that way I remove this when it's despawned.
			
			if (transform != null)
				UnityEngine.Object.Destroy(transform.gameObject); // works fine!
		
			isSpawned = false;
			
			DungeonSave file = DungeonSave.instance;
			file.current.GetLevel(DungeonVars.level).Remove(this); 
		}
		
		
		//TITLE 
		//Text mesh (script)
		public string title = "Dungeon 4"; // text above entrance
		
		protected void AddTitle() {
			Transform setThis = transform.Find("Title");
			setThis.GetComponent<TextMesh>().text = title;
		}
		
		// BillboardEffect180(script)
		// billboard = false;
		
		//EXIT IN, make things go away
		//Loading Tree Exit (script)
		public string level = "Dungeon 4"; // the level to enter.
		
		//#### LINKS >>>> exit2 ####
		public string exit = "Exit 4c"; // "" results in a valid default. An invalid target same, as "".
		
		// trigger for exit
		// public string triggerTarget = "PlayerCollider"; // target you go to
		protected void SetExit() {
			Transform exitIn = transform.Find("ExitIn");
			LoadingTreeExit ltx = exitIn.GetComponent<LoadingTreeExit>();
			ltx.level = level;
			ltx.exit = exit;
			//ltx.targetName
			
		}
		
		
		//EXIT OUT, spawnpoint moves here
		//#### LINKS <<<< exit ####
		public string exit2 = "Exit 4c"; // a reference, should probably bear a name like Dungeon 4{a:z} inside the HUB world
		
		protected void RenameExit2() {
			Transform exitOut = transform.Find("ExitOut");
			exitOut.GetChild(0).gameObject.name = exit2; // tag is always the same
		}
		
		
		
		public ExitS Clone() {
			ExitS newExit = new ExitS();
			
			newExit.savedName 	= savedName ; // copy
			newExit.level 		= level 	;
			newExit.exit 		= exit 		;
			newExit.exit2 		= exit2 	;
			newExit.title 		= title 	;
			
			return newExit;
			
		}
		
		public Spawnable IClone() {
			ExitS newExit = new ExitS();
			
			newExit.savedName 	= savedName ; // copy
			newExit.level 		= level 	;
			newExit.exit 		= exit 		;
			newExit.exit2 		= exit2 	;
			newExit.title 		= title 	;
			
			return (Spawnable)newExit;
			
		}
		
		public string[] AsPreview() {
			
			
			string s2 = string.Format(
@"ExitS Requisition
{0}
{1}
{2}
{3}
{4}", savedName, level, exit, exit2, title);

			string s1 = 
@"Item
Save Name
To Level
Other Exit
This Exit
Displayed Name";

			return new string[]{s1,s2};
			
			
		}
	}
	
	
		
	
}