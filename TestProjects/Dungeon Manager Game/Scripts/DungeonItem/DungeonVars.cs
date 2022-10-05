using UnityEngine;
using UnityEngine.AI;

using System.Collections;
using System.Collections.Generic;

using Dungeon.Save;

namespace Dungeon
{
	public class DungeonVars : MonoBehaviour {

		
		//##
		// the player avatars
		
		public static string resourceFolder = ""; // Resources/[resourceFolder]
		public static List<Transform> teleportedThings = new List<Transform>(); // assign all teleported things here
		
		public static List<NavMeshAgent> teleportedAgents = new List<NavMeshAgent>(); // assign all teleported things that care about agents here
		public static Transform keyObjectTransform; // the thing I care is moving
		
		public static string keyPlayerCollider = "PlayerCollider";
		
		//##
		// the level load state
		public static string _level = "";
		
		public static string level {
			set
			{
				prevLevel = _level;
				_level = value;
			} 
			get{return _level;}
		
		}
		
		public string thisLevel = ""; // set a default level, per scene
		
		public static string _exit = ""; // spawn point moves
		
		public static string exit {
			set
			{
				prevExit = _exit;
				_exit = value;
			} 
			
			get{return _exit;}
		
		}
		
		public static string exitTag = "Player";
		
		public static string prevExit = ""; // spawn point moves
		public static string prevLevel = "";
		
		
		public static List<string> keyNames = new List<string>();
		
		
		public static DungeonVars instance;
		protected void Awake () {
			
			instance = this;
			
			
			if (level == "") level = thisLevel;
			
			
			SetupSceneVars();
		}
		
		protected void Start() {
			// boot sequence, for the whole game basically
			DungeonSave file = DungeonSave.instance;
			
			
			if (file.current == null) Debug.LogError("The DngeonVars.current didn't load, wtf.", gameObject);
			file.current.SpawnLevel(DungeonVars.level);
			
		}
		
		protected void SetupSceneVars () {
			
			//#######################
			//#######################
			//#######################
			// Clear and replace key objects
			keyNames.Clear(); // remove temporary dungeon keys
			
			// add persistent keys found in Saved keys
			
			
			
			//#######################
			//#######################
			//#######################
			// Clear and replace teleported objects
			teleportedThings.Clear();
			teleportedAgents.Clear();
			GameObject	target = gameObject.FindNameXTag("ThimbleAvatar", "Player");
			if (target == null) Debug.LogError("Coulnd't find" + "ThimbleAvatar", gameObject);
			// player main body
			keyObjectTransform = target.transform;
			teleportedThings.Add(keyObjectTransform);
			
			
			//the thing that might float beside/infront of it
			
			target = gameObject.FindNameXTag("DesireIndicator", "Magnets");
			if (target == null) Debug.LogError("Couldn't find " + "DesireIndicator", gameObject);
			
			
			teleportedThings.Add(target.transform);
			
			//the primary target indicator
			target = gameObject.FindNameXTag("TrueTargetIndicator", "Magnets");
			if (target == null) Debug.LogError("Couldn't find " + "TrueTargetIndicator", gameObject);
			
			teleportedThings.Add(target.transform);
			
			target = gameObject.FindNameXTag("CameraProxPivot", "CameraRig");
			teleportedThings.Add(target.transform);
			
			
			
			// a little ghost that's blocked.. it's supposed to be a fast version of the first thing
			target = gameObject.FindNameXTag("GhostTargetIndicator", "Magnets");
			if (target == null) Debug.LogError("Couldn't find " + "GhostTargetIndicator", gameObject);
			
			teleportedThings.Add(target.transform);
		}
		
	}
}