using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;

namespace Dungeon.Save
{
	
	
	[System.Serializable]
	public class SpS : Spawnable {
		// an Item Save/Spawn. contains information about an unspawned prefab.
		// Parameters, and if no position specified, the position
		
		
		//META, debug, dictionary, list
		
		public string savedName = "SavablePlayState";
		
		public string GetName() { return prefabName; }
		
		public string GetSavedName() { return savedName; }
		public void SetSavedName(string s) { savedName = s; }
		
		
		public string prefabName = "GenericBlankPlayState"; // I suppose I might want a list of prefabs?
		
		
		
		public string prefabPath = "";
		
		
		protected bool isSpawned = false; // debug, check if I'm double spawning. check if I somehow spawned it twice as an item
		
		
		
		//TRANSFORM (req. instantiation)
		public SaveV3 position = new SaveV3();
		public SaveV3 rotation = new SaveV3();
		// public Vector3 scale   ;
		
		
		[System.NonSerialized]
		protected UnityEngine.Transform transform;
		
		public void Spawn(Transform realTransform) {
			// set the position variable
			
			transform = realTransform;
			
			TransformChange();
			// rotation = new SaveV3();
			
			
			isSpawned = true;
		}
		//onload
		
		public void Spawn(Vector3 newPos) {
			// for an unspawned item
			
			// alternative to setting vars, then calling Spawn().
			position = new SaveV3(newPos);
			
			Spawn();
		}
		//onload
		
		public void Spawn() {
			//Debug.Log("spawning " + prefabName);
			if (prefabPath == "") return;
			
			Transform prefab = UnityEngine.Resources.Load(DungeonVars.resourceFolder+prefabPath, typeof (Transform)) as Transform;
			
			
			transform = UnityEngine.Object.Instantiate(prefab) as Transform;
			
			isSpawned = true;
			
			SetTransform ();
			
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
		
		public void Save() {
			
			DungeonSave file = DungeonSave.instance;
			file.current.GetLevel(DungeonVars.level).Add(this); // add this script (not this object)
			
			
		}
		
		public void Despawn() {
			
			if (transform != null)
				UnityEngine.Object.Destroy(transform.gameObject); // works fine!
		
			isSpawned = false;
			
			DungeonSave file = DungeonSave.instance;
			file.current.GetLevel(DungeonVars.level).Remove(this); 
			
		}
		
		
		
		
		public SpS Clone() {
			
			return new SpS();
			
		}
		
		public Spawnable IClone() {
			SpS newBlank = new SpS();
			
			return (Spawnable)newBlank;
			
		}
		
		
		public string[] AsPreview() {
			

			return new string[0];
			
			
		}
	}
	
}