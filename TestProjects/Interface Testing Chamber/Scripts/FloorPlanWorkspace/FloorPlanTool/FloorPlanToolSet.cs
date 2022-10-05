using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace FloorDesigner.Tools
{
	public class FloorPlanToolSet	: FloorPlanWorkspaceHubConnect {
		// This is a group of tools that are disabled by an extension call
		
		
		protected const int message_start = (int)FloorPlanWorkspaceHub.Enum.ExtensionStart;
		protected const int message_end= (int)FloorPlanWorkspaceHub.Enum.ExtensionEnd;
		
		
		
		protected override void OnEnable( ){
			base.OnEnable();
			
			// the extended features can be checked case by case.
		}
		
		
		
		
		protected override void OnMessage(int senderEnum, int msgEnum) {
			if (senderEnum != (int)channel) return;
			if (msgEnum == 0) Connect();
			
			if (msgEnum == (int)FloorPlanWorkspaceHub.Enum.Extension)
			{} // could be a ping
		
			if (msgEnum == message_start)
			{
				gameObject.SetActive(false);
			}
			else if (msgEnum == message_end)
			{
				gameObject.SetActive(true);
			}
			
			
				
		}
		protected override void OnConnect(Object other) {
			// responds specifically to messages
		}
		
		

	}
}