using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim.Setup
{
	public class DatesimSetupAmbienceHub : ConnectHub {
		// This is a list of locations, I should be able to auto-generate them
		
		
		public ConnectHubDelegate onChange;
		public int totalAmbience = 0;
		public int maxAmbience = 50;
		public List<int> scenes = new List<int>(){1};
		public bool showAll = false;
		public int nextBrush = 0;
		public RectTransform rectTransform;
		public RectTransform draggedAmbience;
		public RectTransform selectedAmbience;
		
		public GameObject selected;
		public List<DatesimSetupAmbienceObject> previewList = new List<DatesimSetupAmbienceObject>();
		public List<DatesimSetupAmbienceObject> backup = new List<DatesimSetupAmbienceObject>();
		
		public bool newList = true;
		
		public virtual void DeselectAll(){
			scenes.Clear();
			
		}
		public virtual void ClearAll()
		{
			DatesimSetupAmbienceObject[] pvArray = previewList.ToArray();
			foreach(DatesimSetupAmbienceObject pv in pvArray)
			{
				pv.Kill();
			}
			previewList.Clear();
			totalAmbience = 0;
		}
		
		public virtual void Backup()
		{
			DatesimSetupAmbienceObject b;
			foreach(DatesimSetupAmbienceObject pv in previewList)
			{
				pv.gameObject.SetActive(false);
				
				b = GameObject.Instantiate(pv) as DatesimSetupAmbienceObject;
				
				backup.Add(b);
				b.rectTransform.SetParent(pv.rectTransform.parent, false);
				
				pv.gameObject.SetActive(true);
			}
			
			
		}
		
		public virtual void Refresh()
		{
			
			DatesimSetupAmbienceObject b;
			foreach(DatesimSetupAmbienceObject pv in backup)
			{
				
				b = GameObject.Instantiate(pv) as DatesimSetupAmbienceObject;
				
				previewList.Add(b);
				b.rectTransform.SetParent(pv.rectTransform.parent, false);
				
				b.gameObject.SetActive(true);
			}
			
			
		}
		
		public virtual void UndoAll()
		{
			if (totalAmbience > 0) ClearAll();
			
			Refresh();
			totalAmbience = previewList.Count;
			newList = true;
		}
		
		public virtual void Add(DatesimSetupAmbienceObject newOb)
		{
			if (previewList.Contains(newOb)) return;
			
			if (totalAmbience >= maxAmbience)
			{
				newOb.Kill();
				return;
			}
			previewList.Add(newOb);
			totalAmbience ++;
			
		}
		public virtual void Remove(DatesimSetupAmbienceObject newOb)
		{
			if (!previewList.Contains(newOb)) return;
			previewList.Remove(newOb);
			totalAmbience --;
		}
		
		protected virtual void OnEnable()
		{
			newList = true; // always needs to sync at start
			if(rectTransform == null) rectTransform = GetComponent<RectTransform>();
			
		}
		
		
		public virtual void OnChange()
		{
			if (onChange != null) onChange();
			
		}
		
		
		
		
		
		

	}
}