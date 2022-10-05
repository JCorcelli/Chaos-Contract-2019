using UnityEngine;
using System.Collections;
using SelectionSystem.IHSCx;

namespace SelectionSystem.Magnets
{
	
	public class DimensionSwapTarget : IHSCxConnect {

		
		protected override void OnEnable() {
			Connect();
			if (ih == null) return;
			ih.onEnter 	+= _Enter;
			ih.onExit 	+= _Exit;
			if (ih.isHovered) 
			{
				Enter();
			}
			
		}
		protected override void OnDisable() {
			if (ih == null) return;
			ih.onEnter 	-= _Enter;
			ih.onExit 	-= _Exit;
			Exit();
		}
		protected void _Enter(HSCxController a){Enter();}
		protected void Enter(){
			if (touching) return;
			touching = true;
			OnEnter();
		}
		
		protected bool touching = false;
		protected virtual void OnEnter() {
			MagnetGlobal.canSwap = true;
		}
		
		protected void _Exit(HSCxController a){Exit();}
		protected void Exit(){
			if (!touching) return;
			touching = false;
			OnExit();
		}
		
		protected virtual void OnExit() {
			
			MagnetGlobal.canSwap = false;
		}
	
	}
}