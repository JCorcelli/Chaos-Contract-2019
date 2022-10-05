using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace DialogueSystem
{
	public partial class MemoryPage : DisplayNodeParent {
		
		// list of stored text
		
		
		public RectTransform rectTransform;
		
		protected DisplayNode page;
		protected DAction action;
		public DLocalAction localAction;
		
		protected override void OnEnable(){
			base.OnEnable();
			
			page = node;
			page.Load(); // is actually init
			if (page == null) 
			{
				Debug.LogError("No Page ", gameObject);
				return;
			}
			
			localAction = page.localAction;
			
			DAction.Use(localAction.a);
			
			action =new DAction("MemoryEnd") ;
			
			action.Add(MemoryEnd);
			action =new DAction("MemoryBegin") ;
			
			action.Add(MemoryBegin);
			
		}
		
		protected void MemoryBegin(string var){
			/* 
			MemoryBoxNode m = MemoryBox.memory.selected;
			
			page.streamedText = m.streamedText;
			page.goalText = m.goalText;
			page.PlayUnsafe(); 
			page.charPos = MemoryBox.memory.selected.charPos; */
		}
		protected void MemoryEnd(string var){
			// page.Break();
		}
		
		protected override void OnUpdate(){
			base.OnUpdate();
			page.Step();
		}
		
	}
}