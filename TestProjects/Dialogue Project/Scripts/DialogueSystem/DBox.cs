
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;


using Utility.GUI;
using SelectionSystem;



namespace DialogueSystem
{
	
	public class DBox : UpdateBehaviour {
		// hypotheticaly I stream a text without a user or storage
		public static DBox box;
		public List<DStream> streams = new List<DStream>();
		protected override void OnEnable()
		{
			base.OnEnable();
			if (box == null) 
			{
				box = this;
			}
			else Destroy(this);
			// in the future a silent dbox like this can filter the offscreen events
			
			// add global action to cause proc?
			DAction d= new DAction("silent.proc");
			d.Add(Proc);
		}
		protected override void OnUpdate()
		{
			base.OnUpdate();
			for (int i = 0 ; i < streams.Count ; i++)
			{
				streams[i].Step();
			}
		}
		public void Proc( string s ){
			// pending: generate dtext from s
		}
		public void Proc( DText t ){
			DStream d = new DStream();
			d.Proc(t);
			streams.Add(d);
		}
	}
	
}