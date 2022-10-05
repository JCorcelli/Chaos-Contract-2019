using UnityEngine;
using System.Collections;


namespace Dungeon
{
	public interface IOnFall {
		
		void OnFall();
		Vector3 GetPosition();
	}
	public class RespawnOnFall : UpdateBehaviour {

		// Use this for initialization
		
		
		public IOnFall target;
		
		public float lowerLimit = -10f;
		
		void Start () {
			target = DungeonVars.keyObjectTransform.GetComponent<IOnFall>();
			
			
			
		}
		
		
		protected override void OnUpdate(){
			if (target != null && target.GetPosition().y < lowerLimit)
			{
				target.OnFall();
			}
		}
		
	}
}