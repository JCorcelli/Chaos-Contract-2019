using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SelectionSystem
{
	public class StaticCloneHook : StaticHubConnect {

		public RectTransform rectTransform;
		public RectTransform copiedOb;
		
		public string targetName = "StaticClone";
		public  StaticCloneListener target;
		
		public override void CheckConnected(){
			if (!subscribed) SubscribeHub();
			
			connected = (target != null);
			
			if (connected) return;
			//if (_hook) 
			Ping();
		}
		
		public int setIndex = -1;
		
		protected override void OnEnable(){
		}
		protected IEnumerator Start() {
			yield return new WaitForSeconds(1f);
			rectTransform = GetComponent<RectTransform>();
			
			CheckConnected();
			
			
			// maybe check if I override or not
		}
		
		protected void Synchronize()
		{
			if (copiedOb != null) Destroy(copiedOb.gameObject);
			copiedOb = Instantiate(target.GetComponent<RectTransform>()) as RectTransform;
			
			
			copiedOb.gameObject.SetActive(false);
			copiedOb.position = new Vector3(0,0,0);
			//copiedOb.copyFlag = true;
			copiedOb.anchorMax = new Vector2(1f,1f);
			copiedOb.anchorMin = new Vector2(0f,0f);
			copiedOb.sizeDelta = new Vector2(0f,0f);
			copiedOb.SetParent(rectTransform, false);
			
			if (setIndex > -1) 
			{
				if (setIndex < rectTransform.childCount)
					copiedOb.SetSiblingIndex(setIndex);
				else
					copiedOb.SetAsLastSibling();
			}
			copiedOb.gameObject.SetActive(true);
		}
		
		protected override void OnConnect(object ob) {
			// for hooks
			if ((Object)ob == target) return;
			
			if (ob.GetType() == typeof(StaticCloneListener) )
			{
			
				StaticCloneListener newTarget = ((StaticCloneListener)ob);
				
				if (newTarget.name != targetName) return;
				
				if (target != null)
					target.onChange -= OnChange;
				
				target = newTarget;
				target.connected = connected = true;
				
				target.onChange += OnChange;
				
				Synchronize();
				
			}
		}
		
		
		public override void OnChange() {
			// behavior
			Synchronize();
		}
	}
}