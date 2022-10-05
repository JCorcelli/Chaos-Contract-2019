using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerVars 
{
	// some things don't follow the player, use wisely
	static Transform _target;
	public static Transform target{get { 
			if (_target == null) GetTarget();
			return _target;
		}
	}
	static void GetTarget () {
		
		
		
		_target = FindNameXTag(playerName, playerTag).transform;
		
		if (_target == null)  Debug.Log("No player target. Name=" + playerName +":Tag =" + playerTag +": search prefabs.");
		
	}
	public const string playerName = "PlayerCenter";
	public const string playerTag = "PlayerRig";
	
	
	public static GameObject FindNameXTag(string findByName, string findByTag)
	{
		foreach (GameObject t in GameObject.FindGameObjectsWithTag(findByTag))
		{
			if (t.name == findByName)
			{
				return t;
				
			}
			
		}
		return null;
	}
}
