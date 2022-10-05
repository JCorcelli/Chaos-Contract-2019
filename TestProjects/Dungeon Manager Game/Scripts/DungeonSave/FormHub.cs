using UnityEngine;
using System.Collections;


namespace Dungeon.Save
{
	public delegate void FormHubDelegate();
	public delegate void Msg_FormHubDelegate(int receiver, string message);
	
	public delegate void Re_FormHubDelegate(int receiver, GameObject sender);
	
	public class FormHub : MonoBehaviour {

	
		public static FormHub selected;
		
		public bool isSelected = false;
		public void Select() {
			if (selected != null) selected.Deselect();
			selected = this;
			isSelected = true;
			
			if (onSelect != null) onSelect();
		}
		public void Deselect() {
			isSelected = false;
			if (onDeselect != null) onDeselect();
		}
		public FormHubDelegate onSelect;
		public FormHubDelegate onDeselect;
		public FormHubDelegate onLoad;
		public FormHubDelegate onSave;
		public FormHubDelegate onBackup;
		public Msg_FormHubDelegate onMessage;
		public Re_FormHubDelegate onConnect;
		
		public GameObject connected;
		
		
		public void Load() {
			if (onLoad != null) onLoad();
		}
		public void Save() {
			if (onSave != null) onSave();
		}
		public void Backup() {
			if (onBackup != null) onBackup();
		}
		public void SendMessage(int receiver, string message) {
			if (onMessage != null) onMessage(receiver, message);
		}
		
		public void Connect(int receiver, GameObject sender) {
			if (onConnect != null) onConnect(receiver, sender);
		}
		public void Disconnect() {
			connected = null;
		}
		
	}
}