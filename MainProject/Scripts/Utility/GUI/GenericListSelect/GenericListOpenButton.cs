using UnityEngine;
using System.Collections;
using System.Collections.Generic;


using SelectionSystem;


namespace Utility.GUI
{
	public class GenericListOpenButton : SelectAbstract {
		// needs to generate the list. easily done by taking variables from the GenericListNode
		
		public IGenericList list; // connection
		public Transform nodeTransform;
		
		public Transform prefab; // does generic list cut it? I should remove the extra functions, change the title, and possibly update the overall appearance

		protected MenuHUBScripted menuhub;
		protected MenuScripted form;
		
		public int intSelection = -1;
		public string stringSelection = "";
		public string message = "";
		
		
		protected int 	 prevIntSelection = -1;
		protected string prevStringSelection = "";
		
		protected virtual void Start() {
			
			
			menuhub = GetComponentInParent<MenuHUBScripted>();
			form = GetComponentInParent<MenuScripted>();
			
		}
		
		protected void Create() {
			Transform t;
			nodeTransform = t = Object.Instantiate(prefab);
			
			GenericListNode node = t.GetComponent<GenericListNode>();
			
			list = node.GetList() as IGenericList;
			// GenericListBuilder.instance = null;
			
			if (!nodeTransform.gameObject.activeSelf)
				nodeTransform.gameObject.SetActive(true);
			
			Build();
		}

		
		protected override void OnDisable() {
			base.OnDisable();
			
			Close();
		}
		
		protected override void OnClick() {
			if (nodeTransform != null) 
				Close();
			else
				Create();
		}
		
		
		//## CURRENT FORM HOOK. close if not current
		protected void Close() {
			// chances are this will be destroyed eventually, disable should work?
			if (nodeTransform != null) 
			{
				GameObject.Destroy(nodeTransform.gameObject);
				nodeTransform = null;
				list = null;
			}
		}
		
		protected override void OnLateUpdate() {
			base.OnLateUpdate();
			if (menuhub.currentMenu != form) Close();  // checking this hub.
			message = "";
		}
		
		protected override void OnUpdate(){
			base.OnUpdate();
			
			GetSelect();
			
		}
		
		
		protected void GetSelect(){
			if (list == null || list.IsRunning()) return;
			intSelection = list.GetSelected(); // saves if nothing's selected
			
			
			message = list.GetMessage();
			list.SetMessage("");
			
			
			if (intSelection >= 0 )
			{
				stringSelection = list.GetSelectedString(intSelection);
				
				if (intSelection != prevIntSelection)
				{					
					OnSelect(intSelection);
				
				}
				if (stringSelection != prevStringSelection)
				{
					OnSelectString(stringSelection);
					
				}
				prevIntSelection = intSelection;
				prevStringSelection = stringSelection;
			}
			
		}

		protected virtual void Build() {
			// something like this on enable
			List<string> elements = new List<string>(){"a","b","c","d","e","f","g","h"};
			list.Build(elements);
		}
				
		protected virtual void OnSelect(int i) {
			// optionally use OnSelect(string s), uses variable stringSelection
		}
		protected virtual void OnSelectString(string s) { }
		
		

	}
}