using UnityEngine;
using System.Collections;

public class CageDoorControl : MonoBehaviour {

	// Use this for initialization
	
	public bool isOpen = false;
	private Animator anim;
	
	void Awake () { anim = GetComponent<Animator>(); }
	void OnEnable () {if (isOpen) Open(); else Close();}
	
	public void Open() {
		
		
		anim.SetTrigger("Open");
		isOpen = true;
	}
	public void Close() {
		
		anim.SetTrigger("Close");
		isOpen = false;
		
	}
	
	
	
}
