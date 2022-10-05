using UnityEngine;
using System.Collections;
using Utility.Managers;

namespace NPC.BTree.Jerri
{
	public class BeStateRegister : MonoBehaviour {

		///<summary>
		///register, unregister, onupdate need to be defined
		/// hub needs to be defined
		///</summary>
	
		public bool current 	= false;
		public bool pingChangeOnly = true;
		
		protected bool prev 	= false;
		public bool firstCall = true;	

		protected virtual void OnEnable()

		{
			GeneralUpdateManager.onUpdate += _OnUpdate;
			
			firstCall = true;
			
			
			
		}
			
		protected virtual void OnDisable () {
			
			GeneralUpdateManager.onUpdate -= _OnUpdate;
			
			firstCall = true;
			
		}

		protected void Ping(){
			if (!firstCall && pingChangeOnly && prev == current) return;
			prev = current;
			firstCall = false;
			
			if (current) 
			{
				Register();
			}
			else
			{
				Unregister();
			}
		}
		protected void _OnUpdate() {
			OnUpdate();
			Ping();
		}
		
		protected virtual void OnUpdate(){}
		protected virtual void Register() {
			
			
		}
		protected virtual void Unregister() {
			
			
		}
		
		
	}
}