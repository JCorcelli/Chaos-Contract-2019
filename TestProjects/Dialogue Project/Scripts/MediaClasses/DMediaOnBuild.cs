
using UnityEngine;

using UnityEngine.UI;

using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

using Utility.GUI;
using SelectionSystem;



namespace DialogueSystem
{


	
	public class DMediaOnBuild : UpdateBehaviour  {
		// This will generate a string of text representing the action of accessing storage
		// And this will be a fast method of transporting text
		
		public TextCategory[] categories ; // original stored text
		
		
		public const string mediaType = "Simulation"; // caused by flaws, environment, distortion. Word for word.
		
		
		protected IEnumerator Start(){ 
			
			yield return new WaitForSeconds(.2f);
			Build();
		
		}
		public virtual void Build()
		{
			DText dt;
			DSource ds;
			TextCategory tc;
			DUser u;
			TextAsset ta;
			
			int len = categories.Length;
			int len2;
			for (int i = 0 ; i < len ; i++)
			{
				
				tc = categories[i];

				len2 = tc.t.Length;
				for (int ic = 0 ; ic < len2 ; ic++)
				{
					ta = tc.t[ic];
					if (ta == null) continue;
					
					ds = new DSource();
					ds.textAsset = ta;
					dt = ds.Spawn();
					
					for (int ix = 0 ; ix < DUser.all.Count ; ix++)
					{
						u = DUser.all[ix];
						if (u.isReal) u.storage.Store( dt); // that's 100% reference
					}
				}
			}
			
		}
		
	}
}