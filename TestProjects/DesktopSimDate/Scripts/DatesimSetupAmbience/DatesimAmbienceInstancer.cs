using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim.Setup
{
	
	public class DatesimAmbienceInstancer : UpdateBehaviour {
		public static DatesimAmbienceInstancer instance;
		
		protected override void OnEnable(){
			base.OnEnable();
			if (instance == null) instance = this;
		}
		public class Filter
		{
			
			public DatesimAppAmbienceZone target;
			public DatesimSetupAmbienceObject copied;
			public Filter(DatesimAppAmbienceZone t, DatesimSetupAmbienceObject a)
			{
				target = t;
				copied = a;
			}
		}

	
		public RectTransform rectTransform;
		//protected RectTransform scaledTransform;
		
		
		public List<Filter> filterList = new List<Filter>();
		
		
		
		public void Stop(DatesimAppAmbienceZone target){
			
			Filter[] al = filterList.ToArray();
			
			for (int i = 0; i < al.Length ; i++)
			{
				if (al[i].target == target)
					filterList.Remove(al[i]);
			}
			
		}
		public void Add(DatesimAppAmbienceZone target, DatesimSetupAmbienceObject copied){
			// individual transforms
			
			
			Filter f = new Filter(target, copied);
			filterList.Add (f);
		}
		
		protected DatesimSetupAmbienceObject b;
		protected DatesimSetupAmbienceObject pv;
		protected DatesimAmbienceRectUtil ar;
		
		protected override void OnUpdate(){
			if (filterList.Count < 1) return;
			MakeInstance();
		}
		public void MakeInstance()
		{
			
			while (pv == null)
			{
				
				if (filterList.Count < 1) return;
				
				if (filterList[0] == null)
				{
					filterList.RemoveAt(0); 
					continue;
				}
				else
					pv = filterList[0].copied;
				
				if (pv == null)
				filterList.RemoveAt(0); 
			
			}
			pv.gameObject.SetActive(false);
			ar = pv.GetComponent<DatesimAmbienceRectUtil>().CloneTo(filterList[0].target.transform);
			
			b = ar.gameObject.GetComponent<DatesimSetupAmbienceObject>();
			
			filterList[0].target.ambienceList.Add(b);
			
			b.MakeProxy();
			
			filterList.RemoveAt(0); 
			
			pv.gameObject.SetActive(true);
			b.gameObject.SetActive(true);
			ar = null; b = null; pv = null;
				
				
		}
	}
}