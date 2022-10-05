using UnityEngine;
using System.Collections;


namespace NPC.BTree.Jerri
{
	
	
	// bytes, powers of 2 or 1 << position
	// enum max 0, 1, 1<< 1 ... 1 << 30 (total 32 values)
	[System.Flags]
	public enum ActiveStatesEnum {
		Nothing  = 0,
		WantBunny= 1 << 0,
		WantBed  = 1 << 1,
		WantDoor = 1 << 2,
		
		SeeBunny = 1 << 3,
		NearBunny= 1 << 4,
		NearBed  = 1 << 5,
		OnBed    = 1 << 6,
		NearBunnyMax= 1 << 7,
		WantSleep= 1 << 8,
		WantLaptop= 1 << 9,
		TouchBunny= 1 << 10,
		NearDoor= 1 << 11
	}
	
	
	public class JerriBeStateHUB	: MonoBehaviour {
		
		public bool everything = false;
		public ActiveStatesEnum[] _initActiveStates;
		
		
		// this is where the calculations take place, the above is for accessibility
		public int activeStates = 0;
		public bool debug = false;
		protected void Update() {
			if (!debug) {enabled = false;return;}
			if (everything){All(); return;}
            int bits = 0;
            foreach (ActiveStatesEnum enumValue in _initActiveStates)
            {
                bits |= (int)enumValue;
            }
			activeStates = bits;
			
		}
		protected void Awake(){
			if (everything){All(); return;}
            int bits = 0;
            foreach (ActiveStatesEnum enumValue in _initActiveStates)
            {
                bits |= (int)enumValue;
            }
			activeStates = bits;
			
		}
		public bool Has(ActiveStatesEnum inMask){
			
			return (activeStates & (int)inMask) != 0;
		}
		
		public void Clear(){
			activeStates = 0;
		}
		
		public void All(){
            int bits = 0;
            foreach (ActiveStatesEnum enumValue in System.Enum.GetValues(typeof(ActiveStatesEnum)))
            {
               bits |= (int)enumValue;
            }
			activeStates = bits;
		}
		public void Add(ActiveStatesEnum state)
		{
			
			activeStates |= (int)state;
			
		}
			
		public void Remove(ActiveStatesEnum state) {
			
			activeStates &= ~(int)state;
			
				
		}
		
		
		
		
	}

}