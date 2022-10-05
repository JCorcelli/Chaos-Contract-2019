using System;
using UnityEngine;


namespace Dungeon
{
	
    public class GoToDungeonExit : MonoBehaviour
    {


        public Vector3 offset = new Vector3(0f, 0f, 0f);
		
		public GameObject target;
        protected void Awake()
        {
			
			string ex = "Exit Default";
			if (DungeonVars.exit !="") ex = DungeonVars.exit;
			
			target = gameObject.FindNameXTag(ex, DungeonVars.exitTag);
			
			if (target == null)	
			{
				// can I salvage this?
				Debug.Log(name + " no target found.",gameObject);
				
				ex = "Exit Default";
				target = gameObject.FindNameXTag(ex, DungeonVars.exitTag);
			}
			
            transform.position = target.transform.position + offset;
        }
    }
}
