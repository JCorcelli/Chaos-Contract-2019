using UnityEngine;
using System.Collections;


namespace NPCSystem
{
	public class RoutineNode : UpdateBehaviour {

		public int EXITSTATE {private set{} get{ return 0;}}
		public int _state = 0;
		public int nextState = 0;
		public float timeEstimate = 0f;
		public bool busy = false;
		
		
		public void SetNextState(int s){
			nextState = s;
		}
		public int state{ protected set{_state = value;} get{return _state;}  }
		
		
	}
}