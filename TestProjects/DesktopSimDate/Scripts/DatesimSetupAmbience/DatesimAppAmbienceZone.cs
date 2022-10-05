using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim.Setup
{
	public class DatesimAppAmbienceZone : DatesimAppConnectLite {
		
		public RectTransform rectTransform;
		//protected RectTransform scaledTransform;
		
		public RectTransform dragged;
		
		public List<DatesimSetupAmbienceObject> ambienceList = new List<DatesimSetupAmbienceObject>();
		
		
		
		protected override void OnEnable( ){
			base.OnEnable();
			
			if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
			vars.onChange -= OnChange;
			vars.onChange += OnChange;
			//ContinuePopulate();
		}
		
		protected override void OnDisable( ){
			base.OnDisable( );
			
			
		}
		protected override void OnChange(){
			
			if (vars.clearAmbience) ClearAll();
			
			else if (vars.undoAmbience) 
				UndoAll();
			else if (vars.dirtyAmbience) 
				UpdateAmbience();
		}
		
		protected void UpdateAmbience(){
			CopyVars();
		}
		public virtual void UndoAll()
		{
			ClearAll();
			
			Populate();
			
			vars.undoAmbience = false;
			
		}
		
		public virtual void ClearList()
		{
			DatesimSetupAmbienceObject[] pvArray = ambienceList.ToArray();
			foreach(DatesimSetupAmbienceObject pv in pvArray)
			{
				pv.Kill();
			}
			ambienceList.Clear();
		
			
			vars.clearAmbience = false;
			
		}
		public virtual void ClearAll()
		{
			
			ClearList();
			iCount = -1;
			vars.clearAmbience = false;
		}
		public void OnDestroy() {
			// ???
		}
		
		
		protected void Populate(){
			ClearList();
			iCount = 0; // signified start
			pvArray = vars.refAmbienceList.ToArray();
			
			StopCoroutine("_Populate");
			StartCoroutine("_Populate");
			
		}
		protected void ContinuePopulate(){
			
			if (iCount >= 0)
			StartCoroutine("_Populate");
		}
		
		
		protected virtual void CopyVars(){
			//Populate();
			if (DatesimAmbienceInstancer.instance == null) return;
			
			iCount = 0;
				
			ClearList();
			
			DatesimAmbienceInstancer.instance.Stop(this);
			
			pvArray = vars.refAmbienceList.ToArray();
			
			while(iCount < pvArray.Length )
			{
				DatesimAmbienceInstancer.instance.Add(this, pvArray[iCount]);
			
				iCount ++;
			}
			
		}
		
		
		protected DatesimAmbienceRectUtil ar;
		
		
		protected int iCount = -1; //-1 is non starter
		protected DatesimSetupAmbienceObject[] pvArray = new DatesimSetupAmbienceObject[0] ;
		
		public IEnumerator _Populate()
		{
			
			DatesimSetupAmbienceObject b;
			DatesimSetupAmbienceObject pv;
			
			while(iCount < pvArray.Length )
			{
				pv = pvArray[iCount];
				if (pv == null) continue;
				
				ar = pv.GetComponent<DatesimAmbienceRectUtil>().CloneTo(rectTransform);
				
				b = ar.gameObject.GetComponent<DatesimSetupAmbienceObject>();
				
				ambienceList.Add(b);
				
				b.MakeProxy();
				
				iCount ++;
				yield return null;
			}
			
			
		}
		
		
	}
}